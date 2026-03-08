/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 *
 * Copyright (c) 2025 Florian K.
 *
 * Code inspiration, improvements and fixes are from, but not limited to, following projects:
 * CrystalDiskInfo
 */

using BlackSharp.Core.Interop.Windows.Enums;
using BlackSharp.Core.Interop.Windows.Mutexes;
using BlackSharp.Core.Interop.Windows.Utilities;
using DiskInfoToolkit.Globals;
using DiskInfoToolkit.Interop;
using DiskInfoToolkit.Interop.Enums;
using DiskInfoToolkit.Interop.Realtek;
using DiskInfoToolkit.Interop.Structures;
using System.Runtime.InteropServices;

namespace DiskInfoToolkit.NVMe
{
    internal static class NVMeSmartAttributes
    {
        #region Fields

        const int BUFFER_SIZE = 4096;

        #endregion

        #region Internal

        internal static bool GetSmartAttributeNVMeStorageQuery(Storage storage, IntPtr handle, byte[] buffer)
        {
            var nptwb = new TStorageQueryWithBuffer();
            var size = Marshal.SizeOf<TStorageQueryWithBuffer>();

            nptwb.ProtocolSpecific.ProtocolType = TStorageProtocolType.ProtocolTypeNvme;
            nptwb.ProtocolSpecific.DataType = (uint)TStorageProtocolNVMeDataType.NVMeDataTypeLogPage;
            nptwb.ProtocolSpecific.ProtocolDataRequestValue = 2; //SMART Health Information
            nptwb.ProtocolSpecific.ProtocolDataRequestSubValue = 0;
            nptwb.ProtocolSpecific.ProtocolDataOffset = (uint)Marshal.SizeOf<TStorageProtocolSpecificData>();
            nptwb.ProtocolSpecific.ProtocolDataLength = BUFFER_SIZE;

            nptwb.Query.PropertyId = STORAGE_PROPERTY_ID.StorageAdapterProtocolSpecificProperty;
            nptwb.Query.QueryType = STORAGE_QUERY_TYPE.PropertyStandardQuery;

            var ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(nptwb, ptr, false);

            bool ok = Kernel32.DeviceIoControl(handle, Kernel32.IOCTL_STORAGE_QUERY_PROPERTY, ptr, size, ptr, size, out _, IntPtr.Zero);

            if (!ok)
            {
                nptwb = Marshal.PtrToStructure<TStorageQueryWithBuffer>(ptr);

                nptwb.ProtocolSpecific.ProtocolDataRequestSubValue = 0xFFFFFFFF;

                Marshal.StructureToPtr(nptwb, ptr, false);

                ok = Kernel32.DeviceIoControl(handle, Kernel32.IOCTL_STORAGE_QUERY_PROPERTY, ptr, size, ptr, size, out _, IntPtr.Zero);
            }

            nptwb = Marshal.PtrToStructure<TStorageQueryWithBuffer>(ptr);

            Array.Copy(nptwb.Buffer, buffer, buffer.Length);

            Marshal.FreeHGlobal(ptr);

            return ok;
        }

        internal static bool GetSmartAttributeNVMeIntel(Storage storage, IntPtr handle, byte[] buffer)
        {
            var nptwb = new NVME_PASS_THROUGH_IOCTL();
            var size = Marshal.SizeOf<NVME_PASS_THROUGH_IOCTL>();

            nptwb.SrbIoCtrl.ControlCode = InteropConstants.NVME_PASS_THROUGH_SRB_IO_CODE;
            nptwb.SrbIoCtrl.HeaderLength = (uint)Marshal.SizeOf<SRB_IO_CONTROL>();
            Array.Copy(InteropConstants.NVME_SIG_STR_ARR, nptwb.SrbIoCtrl.Signature, InteropConstants.NVME_SIG_STR_LEN);
            nptwb.SrbIoCtrl.Timeout = InteropConstants.NVME_PT_TIMEOUT;
            nptwb.SrbIoCtrl.Length = (uint)(size - Marshal.SizeOf<SRB_IO_CONTROL>());
            nptwb.DataBufferLen = (uint)nptwb.DataBuffer.Length;
            nptwb.ReturnBufferLen = (uint)size;
            nptwb.Direction = InteropConstants.NVME_FROM_DEV_TO_HOST;

            nptwb.NVMeCmd[0] = 2; //GetLogPage
            nptwb.NVMeCmd[1] = 0xFFFFFFFF; //GetLogPage
            nptwb.NVMeCmd[10] = 0x007f0002;

            var ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(nptwb, ptr, false);

            if (!Kernel32.DeviceIoControl(handle, Kernel32.IOCTL_SCSI_MINIPORT, ptr, size, ptr, size, out _, IntPtr.Zero))
            {
                Marshal.FreeHGlobal(ptr);

                return false;
            }

            nptwb = Marshal.PtrToStructure<NVME_PASS_THROUGH_IOCTL>(ptr);

            if (false == nptwb.DataBuffer.Any(b => b != 0))
            {
                Marshal.FreeHGlobal(ptr);

                return false;
            }

            Array.Copy(nptwb.DataBuffer, buffer, buffer.Length);

            Marshal.FreeHGlobal(ptr);

            return true;
        }

        internal static bool GetSmartAttributeNVMeIntelRst(Storage storage, IntPtr handle, byte[] buffer)
        {
            var scsiAddress = new SCSI_ADDRESS();

            if (storage.ScsiPort >= 0 && storage.ScsiTargetID >= 0)
            {
                scsiAddress.PortNumber = storage.ScsiPort;
                scsiAddress.TargetId = storage.ScsiTargetID;
            }
            else
            {
                if (!SharedMethods.GetScsiAddress(handle, out scsiAddress))
                {
                    return false;
                }
            }

            if (!SharedMethods.TryGetScsiHandle(scsiAddress, out var scsiHandle))
            {
                return false;
            }

            try
            {
                var nvmeData = new INTEL_NVME_PASS_THROUGH();
                var size = Marshal.SizeOf<INTEL_NVME_PASS_THROUGH>();

                nvmeData.SRB.HeaderLength = (uint)Marshal.SizeOf<SRB_IO_CONTROL>();
                Array.Copy(InteropConstants.NVME_INTEL_SIG_STR_ARR, nvmeData.SRB.Signature, InteropConstants.NVME_INTEL_SIG_STR_LEN);
                nvmeData.SRB.Timeout = 10;
                nvmeData.SRB.ControlCode = Kernel32.IOCTL_INTEL_NVME_PASS_THROUGH;
                nvmeData.SRB.Length = (uint)(size - Marshal.SizeOf<SRB_IO_CONTROL>());

                nvmeData.Payload.Version = 1;
                nvmeData.Payload.PathId = scsiAddress.PathId;
                nvmeData.Payload.Cmd.CDW0.Opcode = 0x02; //ADMIN_GET_LOG_PAGE
                nvmeData.Payload.Cmd.NSID = 0xFFFFFFFF; //NVME_NAMESPACE_ALL
                nvmeData.Payload.Cmd.u.GET_LOG_PAGE.CDW10.LID = 2; //= 0x7f0002
                nvmeData.Payload.Cmd.u.GET_LOG_PAGE.CDW10.NUMD = 0x7F;
                nvmeData.Payload.ParamBufLen = (uint)(Marshal.SizeOf<INTEL_NVME_PAYLOAD>() + Marshal.SizeOf<SRB_IO_CONTROL>()); //0xA4;
                nvmeData.Payload.ReturnBufferLen = 0x1000;
                nvmeData.Payload.CplEntry[0] = 0;

                var ptr = Marshal.AllocHGlobal(size);
                Marshal.StructureToPtr(nvmeData, ptr, false);

                if (Kernel32.DeviceIoControl(scsiHandle, Kernel32.IOCTL_SCSI_MINIPORT, ptr, size, ptr, size, out _, IntPtr.Zero))
                {
                    nvmeData = Marshal.PtrToStructure<INTEL_NVME_PASS_THROUGH>(ptr);

                    Array.Copy(nvmeData.DataBuffer, buffer, buffer.Length);

                    Marshal.FreeHGlobal(ptr);

                    return true;
                }

                Marshal.FreeHGlobal(ptr);

                return false;
            }
            finally
            {
                SafeFileHandler.CloseHandle(scsiHandle);
            }
        }

        internal static bool GetSmartAttributeNVMeIntelVroc(Storage storage, IntPtr handle, byte[] buffer)
        {
            var scsiAddress = new SCSI_ADDRESS();

            if (storage.ScsiPort >= 0 && storage.ScsiTargetID >= 0)
            {
                scsiAddress.PortNumber = storage.ScsiPort;
                scsiAddress.TargetId = storage.ScsiTargetID;
            }
            else
            {
                if (!SharedMethods.GetScsiAddress(handle, out scsiAddress))
                {
                    return false;
                }
            }

            if (!SharedMethods.TryGetScsiHandle(scsiAddress, FileFlagsAndAttributes.Normal, out var scsiHandle))
            {
                return false;
            }

            try
            {
                var nptwb = new NVME_PASS_THROUGH_IOCTL();
                var size = Marshal.SizeOf<NVME_PASS_THROUGH_IOCTL>();

                Array.Copy(InteropConstants.NVME_RAID_SIG_STR_ARR, nptwb.SrbIoCtrl.Signature, InteropConstants.NVME_RAID_SIG_STR_LEN);
                nptwb.SrbIoCtrl.ControlCode = InteropConstants.NVME_PASS_THROUGH_SRB_IO_CODE;
                nptwb.SrbIoCtrl.Timeout = InteropConstants.NVME_PT_TIMEOUT;
                nptwb.SrbIoCtrl.HeaderLength = (uint)Marshal.SizeOf<SRB_IO_CONTROL>();
                nptwb.SrbIoCtrl.Length = (uint)(size - Marshal.SizeOf<SRB_IO_CONTROL>());

                nptwb.SrbIoCtrl.ReturnCode = (uint)(0x86000000 + (scsiAddress.PathId << 16) + (scsiAddress.TargetId << 8) + scsiAddress.Lun);

                nptwb.Direction = InteropConstants.NVME_FROM_DEV_TO_HOST;
                nptwb.QueueId = 0;
                nptwb.MetaDataLen = 0;
                nptwb.DataBufferLen = (uint)nptwb.DataBuffer.Length;
                nptwb.ReturnBufferLen = (uint)size;

                nptwb.NVMeCmd[0] = 0x02; //Log Page
                nptwb.NVMeCmd[1] = 0xFFFFFFFF; //Namespace Identifier (CDW1.NSID)
                nptwb.NVMeCmd[10] = 0x7f0002; //Controller or Namespace Structure (CNS)

                nptwb.DataBuffer[0] = 1;

                var ptr = Marshal.AllocHGlobal(size);
                Marshal.StructureToPtr(nptwb, ptr, false);

                if (!Kernel32.DeviceIoControl(scsiHandle, Kernel32.IOCTL_SCSI_MINIPORT, ptr, size, ptr, size, out _, IntPtr.Zero))
                {
                    Marshal.FreeHGlobal(ptr);

                    return false;
                }

                nptwb = Marshal.PtrToStructure<NVME_PASS_THROUGH_IOCTL>(ptr);

                if (false == nptwb.DataBuffer.Any(b => b != 0))
                {
                    Marshal.FreeHGlobal(ptr);

                    return false;
                }

                Array.Copy(nptwb.DataBuffer, buffer, buffer.Length);

                Marshal.FreeHGlobal(ptr);

                return true;
            }
            finally
            {
                SafeFileHandler.CloseHandle(scsiHandle);
            }
        }

        internal static bool GetSmartAttributeNVMeSamsung(Storage storage, IntPtr handle, byte[] buffer)
        {
            var sptwb = new SCSI_PASS_THROUGH_WITH_BUFFERS24();

            sptwb.Spt.Length = (ushort)Marshal.SizeOf<SCSI_PASS_THROUGH>();
            sptwb.Spt.PathId = 0;
            sptwb.Spt.TargetId = 0;
            sptwb.Spt.Lun = 0;
            sptwb.Spt.SenseInfoLength = 24;
            sptwb.Spt.DataIn = InteropConstants.SCSI_IOCTL_DATA_IN;
            sptwb.Spt.DataTransferLength = InteropConstants.IDENTIFY_BUFFER_SIZE;
            sptwb.Spt.TimeOutValue = 2;
            sptwb.Spt.DataBufferOffset = (ulong)Marshal.OffsetOf<SCSI_PASS_THROUGH_WITH_BUFFERS24>(nameof(sptwb.DataBuf)).ToInt64();
            sptwb.Spt.SenseInfoOffset = (uint)Marshal.OffsetOf<SCSI_PASS_THROUGH_WITH_BUFFERS24>(nameof(sptwb.SenseBuf)).ToInt32();

            sptwb.Spt.CdbLength = 16;
            sptwb.Spt.Cdb[0] = 0xB5; //SECURITY PROTOCOL OUT
            sptwb.Spt.Cdb[1] = 0xFE; //SAMSUNG PROTOCOL
            sptwb.Spt.Cdb[2] = 0;
            sptwb.Spt.Cdb[3] = 6;
            sptwb.Spt.Cdb[4] = 0;
            sptwb.Spt.Cdb[5] = 0;
            sptwb.Spt.Cdb[6] = 0;
            sptwb.Spt.Cdb[7] = 0;
            sptwb.Spt.Cdb[8] = 0;
            sptwb.Spt.Cdb[9] = 0x40;

            sptwb.Spt.DataIn = InteropConstants.SCSI_IOCTL_DATA_OUT;
            sptwb.DataBuf[0] = 2;
            sptwb.DataBuf[4] = 0xFF;
            sptwb.DataBuf[5] = 0xFF;
            sptwb.DataBuf[6] = 0xFF;
            sptwb.DataBuf[7] = 0xFF;

            var size = (int)(Marshal.OffsetOf<SCSI_PASS_THROUGH_WITH_BUFFERS24>(nameof(sptwb.DataBuf)).ToInt32() + sptwb.Spt.DataTransferLength);
            var ptrSize = Marshal.SizeOf<SCSI_PASS_THROUGH_WITH_BUFFERS24>();

            var ptr = Marshal.AllocHGlobal(ptrSize);
            Marshal.StructureToPtr(sptwb, ptr, false);

            if (!Kernel32.DeviceIoControl(handle, Kernel32.IOCTL_SCSI_PASS_THROUGH, ptr, size, ptr, size, out _, IntPtr.Zero))
            {
                Marshal.FreeHGlobal(ptr);

                return false;
            }

            sptwb = new SCSI_PASS_THROUGH_WITH_BUFFERS24();

            sptwb.Spt.Length = (ushort)Marshal.SizeOf<SCSI_PASS_THROUGH>();
            sptwb.Spt.PathId = 0;
            sptwb.Spt.TargetId = 0;
            sptwb.Spt.Lun = 0;
            sptwb.Spt.SenseInfoLength = 24;
            sptwb.Spt.DataIn = InteropConstants.SCSI_IOCTL_DATA_IN;
            sptwb.Spt.DataTransferLength = InteropConstants.IDENTIFY_BUFFER_SIZE;
            sptwb.Spt.TimeOutValue = 2;
            sptwb.Spt.DataBufferOffset = (ulong)Marshal.OffsetOf<SCSI_PASS_THROUGH_WITH_BUFFERS24>(nameof(sptwb.DataBuf)).ToInt64();
            sptwb.Spt.SenseInfoOffset = (uint)Marshal.OffsetOf<SCSI_PASS_THROUGH_WITH_BUFFERS24>(nameof(sptwb.SenseBuf)).ToInt32();

            sptwb.Spt.CdbLength = 16;
            sptwb.Spt.Cdb[0] = 0xA2; //SECURITY PROTOCOL OUT
            sptwb.Spt.Cdb[1] = 0xFE; //SAMSUNG PROTOCOL
            sptwb.Spt.Cdb[2] = 0;
            sptwb.Spt.Cdb[3] = 6;
            sptwb.Spt.Cdb[4] = 0;
            sptwb.Spt.Cdb[5] = 0;
            sptwb.Spt.Cdb[6] = 0;
            sptwb.Spt.Cdb[7] = 0;
            sptwb.Spt.Cdb[8] = 1;
            sptwb.Spt.Cdb[9] = 0;

            sptwb.Spt.DataIn = InteropConstants.SCSI_IOCTL_DATA_IN;
            sptwb.DataBuf[0] = 0;
            sptwb.DataBuf[4] = 0xFF;
            sptwb.DataBuf[5] = 0xFF;
            sptwb.DataBuf[6] = 0xFF;
            sptwb.DataBuf[7] = 0xFF;

            Marshal.StructureToPtr(sptwb, ptr, false);

            if (!Kernel32.DeviceIoControl(handle, Kernel32.IOCTL_SCSI_PASS_THROUGH, ptr, size, ptr, size, out _, IntPtr.Zero))
            {
                Marshal.FreeHGlobal(ptr);

                return false;
            }

            sptwb = Marshal.PtrToStructure<SCSI_PASS_THROUGH_WITH_BUFFERS24>(ptr);

            if (false == sptwb.DataBuf.Any(b => b != 0))
            {
                Marshal.FreeHGlobal(ptr);

                return false;
            }

            Array.Copy(sptwb.DataBuf, buffer, buffer.Length);

            Marshal.FreeHGlobal(ptr);

            return true;
        }

        internal static bool GetSmartAttributeNVMeSamsung951(Storage storage, IntPtr handle, byte[] buffer)
        {
            if (!SharedMethods.GetScsiAddress(handle, out var scsiAddress))
            {
                return false;
            }

            var nptwb = new NVME_PASS_THROUGH_IOCTL();
            var size = Marshal.SizeOf<NVME_PASS_THROUGH_IOCTL>();

            nptwb.SrbIoCtrl.ControlCode = InteropConstants.NVME_PASS_THROUGH_SRB_IO_CODE;
            nptwb.SrbIoCtrl.HeaderLength = (uint)Marshal.SizeOf<SRB_IO_CONTROL>();
            Array.Copy(InteropConstants.NVME_SIG_STR_ARR, nptwb.SrbIoCtrl.Signature, InteropConstants.NVME_SIG_STR_LEN);
            nptwb.SrbIoCtrl.Timeout = InteropConstants.NVME_PT_TIMEOUT;
            nptwb.SrbIoCtrl.Length = (uint)(size - Marshal.SizeOf<SRB_IO_CONTROL>());
            nptwb.DataBufferLen = (uint)nptwb.DataBuffer.Length;
            nptwb.ReturnBufferLen = (uint)size;

            nptwb.SrbIoCtrl.ReturnCode = (uint)(0x86000000 + (scsiAddress.PathId << 16) + (scsiAddress.TargetId << 8) + scsiAddress.Lun);
            nptwb.Direction = InteropConstants.NVME_FROM_DEV_TO_HOST;

            nptwb.NVMeCmd[0] = 2; //Log Page
            nptwb.NVMeCmd[1] = 0xFFFFFFFF; //GetLogPage
            nptwb.NVMeCmd[10] = 0x00000002; //S.M.A.R.T.

            var ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(nptwb, ptr, false);

            if (!Kernel32.DeviceIoControl(handle, Kernel32.IOCTL_SCSI_MINIPORT, ptr, size, ptr, size, out _, IntPtr.Zero))
            {
                Marshal.FreeHGlobal(ptr);

                return false;
            }

            nptwb = Marshal.PtrToStructure<NVME_PASS_THROUGH_IOCTL>(ptr);

            if (false == nptwb.DataBuffer.Any(b => b != 0))
            {
                Marshal.FreeHGlobal(ptr);

                return false;
            }

            Array.Copy(nptwb.DataBuffer, buffer, buffer.Length);

            Marshal.FreeHGlobal(ptr);

            return true;
        }

        internal static bool GetSmartAttributeNVMeJMicron(Storage storage, IntPtr handle, byte[] buffer)
        {
            var sptwb = new SCSI_PASS_THROUGH_WITH_BUFFERS24();

            sptwb.Spt.Length = (ushort)Marshal.SizeOf<SCSI_PASS_THROUGH>();
            sptwb.Spt.PathId = 0;
            sptwb.Spt.TargetId = 0;
            sptwb.Spt.Lun = 0;
            sptwb.Spt.SenseInfoLength = 24;
            sptwb.Spt.DataIn = InteropConstants.SCSI_IOCTL_DATA_OUT;
            sptwb.Spt.DataTransferLength = InteropConstants.IDENTIFY_BUFFER_SIZE;
            sptwb.Spt.TimeOutValue = 2;
            sptwb.Spt.DataBufferOffset = (ulong)Marshal.OffsetOf<SCSI_PASS_THROUGH_WITH_BUFFERS24>(nameof(sptwb.DataBuf)).ToInt64();
            sptwb.Spt.SenseInfoOffset = (uint)Marshal.OffsetOf<SCSI_PASS_THROUGH_WITH_BUFFERS24>(nameof(sptwb.SenseBuf)).ToInt32();

            sptwb.Spt.CdbLength = 12;
            sptwb.Spt.Cdb[0] = 0xA1; //NVME PASS THROUGH
            sptwb.Spt.Cdb[1] = 0x80; //ADMIN
            sptwb.Spt.Cdb[2] = 0;
            sptwb.Spt.Cdb[3] = 0;
            sptwb.Spt.Cdb[4] = 0x2;
            sptwb.Spt.Cdb[5] = 0;
            sptwb.Spt.Cdb[6] = 0;
            sptwb.Spt.Cdb[7] = 0;
            sptwb.Spt.Cdb[8] = 0;
            sptwb.Spt.Cdb[9] = 0;
            sptwb.Spt.Cdb[10] = 0;
            sptwb.Spt.Cdb[11] = 0;
            sptwb.DataBuf[0] = Convert.ToByte('N');
            sptwb.DataBuf[1] = Convert.ToByte('V');
            sptwb.DataBuf[2] = Convert.ToByte('M');
            sptwb.DataBuf[3] = Convert.ToByte('E');
            sptwb.DataBuf[8] = 0x02;  //GetLogPage, S.M.A.R.T.
            sptwb.DataBuf[10] = 0x56;
            sptwb.DataBuf[12] = 0xFF;
            sptwb.DataBuf[13] = 0xFF;
            sptwb.DataBuf[14] = 0xFF;
            sptwb.DataBuf[15] = 0xFF;
            sptwb.DataBuf[0x21] = 0x40;
            sptwb.DataBuf[0x22] = 0x7A;
            sptwb.DataBuf[0x30] = 0x02;
            sptwb.DataBuf[0x32] = 0x7F;

            var size = (int)(Marshal.OffsetOf<SCSI_PASS_THROUGH_WITH_BUFFERS24>(nameof(sptwb.DataBuf)).ToInt32() + sptwb.Spt.DataTransferLength);
            var ptrSize = Marshal.SizeOf<SCSI_PASS_THROUGH_WITH_BUFFERS24>();

            using var guard = new WorldMutexGuard(WorldMutexManager.WorldJMicronMutex);

            var ptr = Marshal.AllocHGlobal(ptrSize);
            Marshal.StructureToPtr(sptwb, ptr, false);

            if (!Kernel32.DeviceIoControl(handle, Kernel32.IOCTL_SCSI_PASS_THROUGH, ptr, size, ptr, size, out _, IntPtr.Zero))
            {
                Marshal.FreeHGlobal(ptr);

                return false;
            }

            sptwb = new SCSI_PASS_THROUGH_WITH_BUFFERS24();

            sptwb.Spt.Length = (ushort)Marshal.SizeOf<SCSI_PASS_THROUGH>();
            sptwb.Spt.PathId = 0;
            sptwb.Spt.TargetId = 0;
            sptwb.Spt.Lun = 0;
            sptwb.Spt.SenseInfoLength = 24;
            sptwb.Spt.DataIn = InteropConstants.SCSI_IOCTL_DATA_IN;
            sptwb.Spt.DataTransferLength = InteropConstants.IDENTIFY_BUFFER_SIZE;
            sptwb.Spt.TimeOutValue = 2;
            sptwb.Spt.DataBufferOffset = (ulong)Marshal.OffsetOf<SCSI_PASS_THROUGH_WITH_BUFFERS24>(nameof(sptwb.DataBuf)).ToInt64();
            sptwb.Spt.SenseInfoOffset = (uint)Marshal.OffsetOf<SCSI_PASS_THROUGH_WITH_BUFFERS24>(nameof(sptwb.SenseBuf)).ToInt32();

            sptwb.Spt.CdbLength = 12;
            sptwb.Spt.Cdb[0] = 0xA1; //NVME PASS THROUGH
            sptwb.Spt.Cdb[1] = 0x82; //ADMIN + DMA-IN
            sptwb.Spt.Cdb[2] = 0;
            sptwb.Spt.Cdb[3] = 0;
            sptwb.Spt.Cdb[4] = 0x2;
            sptwb.Spt.Cdb[5] = 0;
            sptwb.Spt.Cdb[6] = 0;
            sptwb.Spt.Cdb[7] = 0;
            sptwb.Spt.Cdb[8] = 0;
            sptwb.Spt.Cdb[9] = 0;
            sptwb.Spt.Cdb[10] = 0;
            sptwb.Spt.Cdb[11] = 0;

            Marshal.StructureToPtr(sptwb, ptr, false);

            if (!Kernel32.DeviceIoControl(handle, Kernel32.IOCTL_SCSI_PASS_THROUGH, ptr, size, ptr, size, out _, IntPtr.Zero))
            {
                Marshal.FreeHGlobal(ptr);

                return false;
            }

            sptwb = Marshal.PtrToStructure<SCSI_PASS_THROUGH_WITH_BUFFERS24>(ptr);

            if (false == sptwb.DataBuf.Any(b => b != 0))
            {
                Marshal.FreeHGlobal(ptr);

                return false;
            }

            Array.Copy(sptwb.DataBuf, buffer, buffer.Length);

            Marshal.FreeHGlobal(ptr);

            return true;
        }

        internal static bool GetSmartAttributeNVMeASMedia(Storage storage, IntPtr handle, byte[] buffer)
        {
            var sptwb = new SCSI_PASS_THROUGH_WITH_BUFFERS();

            sptwb.Spt.Length = (ushort)Marshal.SizeOf<SCSI_PASS_THROUGH>();
            sptwb.Spt.PathId = 0;
            sptwb.Spt.TargetId = 0;
            sptwb.Spt.Lun = 0;
            sptwb.Spt.SenseInfoLength = 24;
            sptwb.Spt.DataTransferLength = InteropConstants.IDENTIFY_BUFFER_SIZE;
            sptwb.Spt.TimeOutValue = 2;
            sptwb.Spt.DataBufferOffset = (ulong)Marshal.OffsetOf<SCSI_PASS_THROUGH_WITH_BUFFERS>(nameof(sptwb.DataBuf)).ToInt64();
            sptwb.Spt.SenseInfoOffset = (uint)Marshal.OffsetOf<SCSI_PASS_THROUGH_WITH_BUFFERS>(nameof(sptwb.SenseBuf)).ToInt32();

            sptwb.Spt.CdbLength = 16;
            sptwb.Spt.Cdb[0] = 0xE6; //NVME PASS THROUGH
            sptwb.Spt.Cdb[1] = 0x02; //GetLogPage
            sptwb.Spt.Cdb[3] = 0x02; //S.M.A.R.T.
            sptwb.Spt.Cdb[7] = 0x7F;

            sptwb.Spt.DataIn = InteropConstants.SCSI_IOCTL_DATA_IN;

            var size = (int)(Marshal.OffsetOf<SCSI_PASS_THROUGH_WITH_BUFFERS>(nameof(sptwb.DataBuf)).ToInt32() + sptwb.Spt.DataTransferLength);
            var ptrSize = Marshal.SizeOf<SCSI_PASS_THROUGH_WITH_BUFFERS>();

            var ptr = Marshal.AllocHGlobal(ptrSize);
            Marshal.StructureToPtr(sptwb, ptr, false);

            if (!Kernel32.DeviceIoControl(handle, Kernel32.IOCTL_SCSI_PASS_THROUGH, ptr, size, ptr, size, out _, IntPtr.Zero))
            {
                Marshal.FreeHGlobal(ptr);

                return false;
            }

            sptwb = Marshal.PtrToStructure<SCSI_PASS_THROUGH_WITH_BUFFERS>(ptr);

            if (false == sptwb.DataBuf.Any(b => b != 0))
            {
                Marshal.FreeHGlobal(ptr);

                return false;
            }

            Array.Copy(sptwb.DataBuf, buffer, buffer.Length);

            Marshal.FreeHGlobal(ptr);

            return true;
        }

        internal static bool GetSmartAttributeNVMeRealtek(Storage storage, IntPtr handle, byte[] buffer)
        {
            var sptwb = new SCSI_PASS_THROUGH_WITH_BUFFERS();

            sptwb.Spt.Length = (ushort)Marshal.SizeOf<SCSI_PASS_THROUGH>();
            sptwb.Spt.PathId = 0;
            sptwb.Spt.TargetId = 0;
            sptwb.Spt.Lun = 0;
            sptwb.Spt.CdbLength = 16;
            sptwb.Spt.SenseInfoLength = 32;
            sptwb.Spt.DataIn = InteropConstants.SCSI_IOCTL_DATA_IN;
            sptwb.Spt.DataTransferLength = InteropConstants.IDENTIFY_BUFFER_SIZE;
            sptwb.Spt.TimeOutValue = 2;
            sptwb.Spt.DataBufferOffset = (ulong)Marshal.OffsetOf<SCSI_PASS_THROUGH_WITH_BUFFERS>(nameof(sptwb.DataBuf)).ToInt64();
            sptwb.Spt.SenseInfoOffset = (uint)Marshal.OffsetOf<SCSI_PASS_THROUGH_WITH_BUFFERS>(nameof(sptwb.SenseBuf)).ToInt32();

            sptwb.Spt.Cdb[0] = 0xE4; // NVME READ
            sptwb.Spt.Cdb[1] = 0; //BYTE(512);
            sptwb.Spt.Cdb[2] = 2; //BYTE(512 >> 8);
            sptwb.Spt.Cdb[3] = 0x02; //GetLogPage

            var size = (int)(Marshal.OffsetOf<SCSI_PASS_THROUGH_WITH_BUFFERS>(nameof(sptwb.DataBuf)).ToInt32() + sptwb.Spt.DataTransferLength);
            var ptrSize = Marshal.SizeOf<SCSI_PASS_THROUGH_WITH_BUFFERS>();

            var ptr = Marshal.AllocHGlobal(ptrSize);
            Marshal.StructureToPtr(sptwb, ptr, false);

            if (!Kernel32.DeviceIoControl(handle, Kernel32.IOCTL_SCSI_PASS_THROUGH, ptr, size, ptr, size, out _, IntPtr.Zero))
            {
                Marshal.FreeHGlobal(ptr);

                return false;
            }

            sptwb = Marshal.PtrToStructure<SCSI_PASS_THROUGH_WITH_BUFFERS>(ptr);

            if (false == sptwb.DataBuf.Any(b => b != 0))
            {
                Marshal.FreeHGlobal(ptr);

                return false;
            }

            Array.Copy(sptwb.DataBuf, buffer, buffer.Length);

            Marshal.FreeHGlobal(ptr);

            return true;
        }

        internal static bool GetSmartAttributeNVMeRealtek9220DP(Storage storage, IntPtr handle, byte[] buffer)
        {
            bool ok = false;

            if (RealtekMethods.RealtekRAIDMode(storage, handle))
            {
                if (RealtekMethods.RealtekSwitchMode(storage, handle, true, 1))
                {
                    ok = GetSmartAttributeNVMeRealtek(storage, handle, buffer);
                    RealtekMethods.RealtekSwitchMode(storage, handle, true, 0);
                }
            }

            return ok;
        }

        internal static bool GetSmartAttributeNVMeJMS586_40(Storage storage, IntPtr handle, byte[] buffer)
        {
            var smartInfo = new UNION_SMART_ATTRIBUTE();

            //TODO: JMicron Library Init / Load / ...

            throw new NotImplementedException("TODO");
        }

        internal static bool GetSmartAttributeNVMeJMS586_20(Storage storage, IntPtr handle, byte[] buffer)
        {
            var smartInfo = new UNION_SMART_ATTRIBUTE();

            //TODO: JMicron Library Init / Load / ...

            throw new NotImplementedException("TODO");
        }

        #endregion
    }
}
