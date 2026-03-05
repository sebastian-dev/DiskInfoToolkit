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

using BlackSharp.Core.Extensions;
using BlackSharp.Core.Interop.Windows.Enums;
using BlackSharp.Core.Interop.Windows.Mutexes;
using BlackSharp.Core.Interop.Windows.Utilities;
using DiskInfoToolkit.Disk;
using DiskInfoToolkit.Globals;
using DiskInfoToolkit.Interop;
using DiskInfoToolkit.Interop.Enums;
using DiskInfoToolkit.Interop.Structures;
using System.Runtime.InteropServices;

namespace DiskInfoToolkit.NVMe
{
    internal static class NVMeSmartIdentifier
    {
        public static bool DoIdentifyDeviceNVMeStorageQuery(Storage storage, IntPtr handle, out IdentifyDevice identifyDevice)
        {
            var nptwb = new TStorageQueryWithBuffer();
            var size = Marshal.SizeOf<TStorageQueryWithBuffer>();

            nptwb.ProtocolSpecific.ProtocolType = TStorageProtocolType.ProtocolTypeNvme;
            nptwb.ProtocolSpecific.DataType = (uint)TStorageProtocolNVMeDataType.NVMeDataTypeIdentify;
            nptwb.ProtocolSpecific.ProtocolDataOffset = (uint)Marshal.SizeOf<TStorageProtocolSpecificData>();
            nptwb.ProtocolSpecific.ProtocolDataLength = SharedConstants.BUFFER_SIZE;
            nptwb.ProtocolSpecific.ProtocolDataRequestValue = 0;
            nptwb.ProtocolSpecific.ProtocolDataRequestSubValue = 1;

            nptwb.Query.PropertyId = STORAGE_PROPERTY_ID.StorageAdapterProtocolSpecificProperty;
            nptwb.Query.QueryType = STORAGE_QUERY_TYPE.PropertyStandardQuery;

            var ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(nptwb, ptr, false);

            if (Kernel32.DeviceIoControl(handle, Kernel32.IOCTL_STORAGE_QUERY_PROPERTY, ptr, size, ptr, size, out _, IntPtr.Zero))
            {
                var offset = Marshal.OffsetOf<TStorageQueryWithBuffer>(nameof(nptwb.Buffer));

                var totalLBA = MarshalExtensions.ReadUInt64(ptr, offset.ToInt32());
                var sectorSize = 1 << Marshal.ReadByte(ptr, offset.ToInt32() + 130);

                //storage.TotalSize = totalLBA * (ulong)sectorSize / 1000 / 1000;
            }

            nptwb = new TStorageQueryWithBuffer();

            nptwb.ProtocolSpecific.ProtocolType = TStorageProtocolType.ProtocolTypeNvme;
            nptwb.ProtocolSpecific.DataType = (uint)TStorageProtocolNVMeDataType.NVMeDataTypeIdentify;
            nptwb.ProtocolSpecific.ProtocolDataOffset = (uint)Marshal.SizeOf<TStorageProtocolSpecificData>();
            nptwb.ProtocolSpecific.ProtocolDataLength = SharedConstants.BUFFER_SIZE;
            nptwb.ProtocolSpecific.ProtocolDataRequestValue = 1; /*NVME_IDENTIFY_CNS_CONTROLLER*/
            nptwb.ProtocolSpecific.ProtocolDataRequestSubValue = 0;

            nptwb.Query.PropertyId = STORAGE_PROPERTY_ID.StorageAdapterProtocolSpecificProperty;
            nptwb.Query.QueryType = STORAGE_QUERY_TYPE.PropertyStandardQuery;

            Marshal.StructureToPtr(nptwb, ptr, false);

            bool ok = Kernel32.DeviceIoControl(handle, Kernel32.IOCTL_STORAGE_QUERY_PROPERTY, ptr, size, ptr, size, out _, IntPtr.Zero);

            nptwb = Marshal.PtrToStructure<TStorageQueryWithBuffer>(ptr);

            identifyDevice = new IdentifyDevice();

            Marshal.Copy(nptwb.Buffer, 0, identifyDevice.IdentifyDevicePtr, identifyDevice.PtrSize);

            Marshal.FreeHGlobal(ptr);

            return ok;
        }

        public static bool DoIdentifyDeviceNVMeIntelVroc(Storage storage, IntPtr handle, out IdentifyDevice identifyDevice)
        {
            if (!SharedMethods.GetScsiAddress(handle, out var scsiAddress))
            {
                identifyDevice = null;
                return false;
            }

            if (!SharedMethods.TryGetScsiHandle(scsiAddress, FileFlagsAndAttributes.Normal, out var scsiHandle))
            {
                identifyDevice = null;
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

                nptwb.NVMeCmd[0] = 6; //Identify
                nptwb.NVMeCmd[1] = 1; //Namespace Identifier (CDW1.NSID)
                nptwb.NVMeCmd[10] = 0; //Controller or Namespace Structure (CNS)

                nptwb.DataBuffer[0] = 1;

                var ptr = Marshal.AllocHGlobal(size);
                Marshal.StructureToPtr(nptwb, ptr, false);

                if (Kernel32.DeviceIoControl(scsiHandle, Kernel32.IOCTL_SCSI_MINIPORT, ptr, size, ptr, size, out _, IntPtr.Zero))
                {
                    var offset = Marshal.OffsetOf<NVME_PASS_THROUGH_IOCTL>(nameof(nptwb.DataBuffer));

                    var totalLBA = MarshalExtensions.ReadUInt64(ptr, offset.ToInt32());
                    var sectorSize = 1 << Marshal.ReadByte(ptr, offset.ToInt32() + 130);

                    //storage.TotalSize = totalLBA * (ulong)sectorSize / 1000 / 1000;
                }

                nptwb = Marshal.PtrToStructure<NVME_PASS_THROUGH_IOCTL>(ptr);

                nptwb.SrbIoCtrl.ReturnCode = (uint)(0x86000000 + (scsiAddress.PathId << 16) + (scsiAddress.TargetId << 8) + scsiAddress.Lun);

                nptwb.NVMeCmd[1] = 0; //Namespace Identifier (CDW1.NSID)
                nptwb.NVMeCmd[10] = 1; //Controller or Namespace Structure (CNS)

                Marshal.StructureToPtr(nptwb, ptr, false);

                if (!Kernel32.DeviceIoControl(scsiHandle, Kernel32.IOCTL_SCSI_MINIPORT, ptr, size, ptr, size, out _, IntPtr.Zero))
                {
                    Marshal.FreeHGlobal(ptr);

                    identifyDevice = null;
                    return false;
                }

                nptwb = Marshal.PtrToStructure<NVME_PASS_THROUGH_IOCTL>(ptr);

                if (false == nptwb.DataBuffer.Any(b => b != 0))
                {
                    Marshal.FreeHGlobal(ptr);

                    identifyDevice = null;
                    return false;
                }

                identifyDevice = new IdentifyDevice();

                Marshal.Copy(nptwb.DataBuffer, 0, identifyDevice.IdentifyDevicePtr, identifyDevice.PtrSize);

                Marshal.FreeHGlobal(ptr);

                return true;
            }
            finally
            {
                SafeFileHandler.CloseHandle(scsiHandle);
            }
        }

        public static bool DoIdentifyDeviceNVMeIntelRst(Storage storage, IntPtr handle, out IdentifyDevice identifyDevice)
        {
            if (!SharedMethods.GetScsiAddress(handle, out var scsiAddress))
            {
                identifyDevice = null;
                return false;
            }

            if (!SharedMethods.TryGetScsiHandle(scsiAddress, out var scsiHandle))
            {
                identifyDevice = null;
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
                nvmeData.Payload.Cmd.CDW0.Opcode = 0x06; //ADMIN_IDENTIFY
                nvmeData.Payload.Cmd.NSID = 1;
                nvmeData.Payload.Cmd.u.IDENTIFY.CDW10.CNS = 0;
                nvmeData.Payload.ParamBufLen = (uint)(Marshal.SizeOf<INTEL_NVME_PAYLOAD>() + Marshal.SizeOf<SRB_IO_CONTROL>()); //0xA4;
                nvmeData.Payload.ReturnBufferLen = 0x1000;
                nvmeData.Payload.CplEntry[0] = 0;

                var ptr = Marshal.AllocHGlobal(size);
                Marshal.StructureToPtr(nvmeData, ptr, false);

                if (Kernel32.DeviceIoControl(scsiHandle, Kernel32.IOCTL_SCSI_MINIPORT, ptr, size, ptr, size, out _, IntPtr.Zero))
                {
                    var offset = Marshal.OffsetOf<INTEL_NVME_PASS_THROUGH>(nameof(nvmeData.DataBuffer));

                    var totalLBA = MarshalExtensions.ReadUInt64(ptr, offset.ToInt32());
                    var sectorSize = 1 << Marshal.ReadByte(ptr, offset.ToInt32() + 130);

                    //storage.TotalSize = totalLBA * (ulong)sectorSize / 1000 / 1000;
                }

                nvmeData = Marshal.PtrToStructure<INTEL_NVME_PASS_THROUGH>(ptr);

                nvmeData.Payload.Cmd.NSID = 0;
                nvmeData.Payload.Cmd.u.IDENTIFY.CDW10.CNS = 1;

                Marshal.StructureToPtr(nvmeData, ptr, false);

                if (!Kernel32.DeviceIoControl(scsiHandle, Kernel32.IOCTL_SCSI_MINIPORT, ptr, size, ptr, size, out _, IntPtr.Zero))
                {
                    Marshal.FreeHGlobal(ptr);

                    identifyDevice = null;
                    return false;
                }

                nvmeData = Marshal.PtrToStructure<INTEL_NVME_PASS_THROUGH>(ptr);

                if (false == nvmeData.DataBuffer.Any(b => b != 0))
                {
                    Marshal.FreeHGlobal(ptr);

                    identifyDevice = null;
                    return false;
                }

                identifyDevice = new IdentifyDevice();

                Marshal.Copy(nvmeData.DataBuffer, 0, identifyDevice.IdentifyDevicePtr, identifyDevice.PtrSize);

                Marshal.FreeHGlobal(ptr);

                return true;
            }
            finally
            {
                SafeFileHandler.CloseHandle(scsiHandle);
            }
        }

        public static bool DoIdentifyDeviceNVMeSamsung(Storage storage, IntPtr handle, out IdentifyDevice identifyDevice)
        {
            var sptwb = new SCSI_PASS_THROUGH_WITH_BUFFERS24();

            sptwb.Spt.Length = (ushort)Marshal.SizeOf<SCSI_PASS_THROUGH>();
            sptwb.Spt.PathId = 0;
            sptwb.Spt.TargetId = 0;
            sptwb.Spt.Lun = 0;
            sptwb.Spt.SenseInfoLength = 24;
            sptwb.Spt.DataIn = InteropConstants.SCSI_IOCTL_DATA_IN;
            sptwb.Spt.DataTransferLength = 4096;
            sptwb.Spt.TimeOutValue = 2;
            sptwb.Spt.DataBufferOffset = (ulong)Marshal.OffsetOf<SCSI_PASS_THROUGH_WITH_BUFFERS24>(nameof(sptwb.DataBuf)).ToInt64();
            sptwb.Spt.SenseInfoOffset = (uint)Marshal.OffsetOf<SCSI_PASS_THROUGH_WITH_BUFFERS24>(nameof(sptwb.SenseBuf)).ToInt32();

            sptwb.Spt.CdbLength = 16;
            sptwb.Spt.Cdb[0] = 0xB5; //SECURITY PROTOCOL OUT
            sptwb.Spt.Cdb[1] = 0xFE; //SAMSUNG PROTOCOL
            sptwb.Spt.Cdb[2] = 0;
            sptwb.Spt.Cdb[3] = 5;
            sptwb.Spt.Cdb[4] = 0;
            sptwb.Spt.Cdb[5] = 0;
            sptwb.Spt.Cdb[6] = 0;
            sptwb.Spt.Cdb[7] = 0;
            sptwb.Spt.Cdb[8] = 0;
            sptwb.Spt.Cdb[9] = 0x40;
            sptwb.Spt.DataIn = InteropConstants.SCSI_IOCTL_DATA_OUT;
            sptwb.DataBuf[0] = 1;

            var length = (int)(Marshal.OffsetOf<SCSI_PASS_THROUGH_WITH_BUFFERS24>(nameof(sptwb.DataBuf)).ToInt32() + sptwb.Spt.DataTransferLength);

            var ptr = Marshal.AllocHGlobal(length);
            Marshal.StructureToPtr(sptwb, ptr, false);

            if (!Kernel32.DeviceIoControl(handle, Kernel32.IOCTL_SCSI_PASS_THROUGH, ptr, length, ptr, length, out _, IntPtr.Zero))
            {
                Marshal.FreeHGlobal(ptr);

                identifyDevice = null;
                return false;
            }

            sptwb = Marshal.PtrToStructure<SCSI_PASS_THROUGH_WITH_BUFFERS24>(ptr);

            sptwb.Spt.Cdb[0] = 0xA2; //SECURITY PROTOCOL OUT
            sptwb.Spt.Cdb[1] = 0xFE; //SAMSUNG PROTOCOL
            sptwb.Spt.Cdb[8] = 1;
            sptwb.Spt.Cdb[9] = 0;
            sptwb.Spt.DataIn = InteropConstants.SCSI_IOCTL_DATA_IN;
            sptwb.DataBuf[0] = 0;

            Marshal.StructureToPtr(sptwb, ptr, false);

            if (!Kernel32.DeviceIoControl(handle, Kernel32.IOCTL_SCSI_PASS_THROUGH, ptr, length, ptr, length, out _, IntPtr.Zero))
            {
                Marshal.FreeHGlobal(ptr);

                identifyDevice = null;
                return false;
            }

            sptwb = Marshal.PtrToStructure<SCSI_PASS_THROUGH_WITH_BUFFERS24>(ptr);

            identifyDevice = new IdentifyDevice();

            Marshal.Copy(sptwb.DataBuf, 0, identifyDevice.IdentifyDevicePtr, identifyDevice.PtrSize);

            Marshal.FreeHGlobal(ptr);

            return true;
        }

        public static bool DoIdentifyDeviceNVMeIntel(Storage storage, IntPtr handle, out IdentifyDevice identifyDevice)
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

            nptwb.NVMeCmd[0] = 6; //Identify
            nptwb.NVMeCmd[1] = 1; //Namespace Identifier (CDW1.NSID)
            nptwb.NVMeCmd[10] = 0; //Controller or Namespace Structure (CNS)

            var ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(nptwb, ptr, false);

            if (Kernel32.DeviceIoControl(handle, Kernel32.IOCTL_SCSI_MINIPORT, ptr, size, ptr, size, out _, IntPtr.Zero))
            {
                var offset = Marshal.OffsetOf<NVME_PASS_THROUGH_IOCTL>(nameof(nptwb.DataBuffer));

                var totalLBA = MarshalExtensions.ReadUInt64(ptr, offset.ToInt32());
                var sectorSize = 1 << Marshal.ReadByte(ptr, offset.ToInt32() + 130);

                //storage.TotalSize = totalLBA * (ulong)sectorSize / 1000 / 1000;
            }

            nptwb = Marshal.PtrToStructure<NVME_PASS_THROUGH_IOCTL>(ptr);

            nptwb.NVMeCmd[1] = 0; //Namespace Identifier (CDW1.NSID)
            nptwb.NVMeCmd[10] = 1; //Controller or Namespace Structure (CNS)

            Marshal.StructureToPtr(nptwb, ptr, false);

            if (!Kernel32.DeviceIoControl(handle, Kernel32.IOCTL_SCSI_MINIPORT, ptr, size, ptr, size, out _, IntPtr.Zero))
            {
                Marshal.FreeHGlobal(ptr);

                identifyDevice = null;
                return false;
            }

            nptwb = Marshal.PtrToStructure<NVME_PASS_THROUGH_IOCTL>(ptr);

            if (false == nptwb.DataBuffer.Any(b => b != 0))
            {
                Marshal.FreeHGlobal(ptr);

                identifyDevice = null;
                return false;
            }

            identifyDevice = new IdentifyDevice();

            Marshal.Copy(nptwb.DataBuffer, 0, identifyDevice.IdentifyDevicePtr, identifyDevice.PtrSize);

            Marshal.FreeHGlobal(ptr);

            return true;
        }

        public static bool DoIdentifyDeviceNVMeJMicron(Storage storage, IntPtr handle, out IdentifyDevice identifyDevice)
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
            sptwb.Spt.Cdb[0] = 0xA1; // NVME PASS THROUGH
            sptwb.Spt.Cdb[1] = 0x80; // ADMIN
            sptwb.Spt.Cdb[2] = 0;
            sptwb.Spt.Cdb[3] = 0;
            sptwb.Spt.Cdb[4] = 0x02;
            sptwb.Spt.Cdb[5] = 0;
            sptwb.Spt.Cdb[6] = 0;
            sptwb.Spt.Cdb[7] = 0;
            sptwb.Spt.Cdb[8] = 0;
            sptwb.Spt.Cdb[9] = 0;
            sptwb.Spt.Cdb[10] = 0;
            sptwb.Spt.Cdb[11] = 0;
            sptwb.Spt.DataIn = InteropConstants.SCSI_IOCTL_DATA_OUT;
            sptwb.DataBuf[0] = Convert.ToByte('N');
            sptwb.DataBuf[1] = Convert.ToByte('V');
            sptwb.DataBuf[2] = Convert.ToByte('M');
            sptwb.DataBuf[3] = Convert.ToByte('E');
            sptwb.DataBuf[8] = 0x06; // Identify
            sptwb.DataBuf[0x30] = 0x01;

            var length = (int)(Marshal.OffsetOf<SCSI_PASS_THROUGH_WITH_BUFFERS24>(nameof(sptwb.DataBuf)).ToInt32() + sptwb.Spt.DataTransferLength);
            var ptrSize = Marshal.SizeOf<SCSI_PASS_THROUGH_WITH_BUFFERS24>();

            using var guard = new WorldMutexGuard(WorldMutexManager.WorldJMicronMutex);

            var ptr = Marshal.AllocHGlobal(ptrSize);
            Marshal.StructureToPtr(sptwb, ptr, false);

            if (!Kernel32.DeviceIoControl(handle, Kernel32.IOCTL_SCSI_PASS_THROUGH, ptr, length, ptr, length, out _, IntPtr.Zero))
            {
                Marshal.FreeHGlobal(ptr);

                identifyDevice = null;
                return false;
            }

            sptwb = new();

            sptwb.Spt.Length = (ushort)Marshal.SizeOf<SCSI_PASS_THROUGH>();
            sptwb.Spt.PathId = 0;
            sptwb.Spt.TargetId = 0;
            sptwb.Spt.Lun = 0;
            sptwb.Spt.SenseInfoLength = 24;
            sptwb.Spt.DataTransferLength = SharedConstants.BUFFER_SIZE;
            sptwb.Spt.TimeOutValue = 2;
            sptwb.Spt.DataBufferOffset = (ulong)Marshal.OffsetOf<SCSI_PASS_THROUGH_WITH_BUFFERS24>(nameof(sptwb.DataBuf)).ToInt64();
            sptwb.Spt.SenseInfoOffset = (uint)Marshal.OffsetOf<SCSI_PASS_THROUGH_WITH_BUFFERS24>(nameof(sptwb.SenseBuf)).ToInt32();

            sptwb.Spt.CdbLength = 12;
            sptwb.Spt.Cdb[0] = 0xA1; // NVME PASS THROUGH
            sptwb.Spt.Cdb[1] = 0x82; // ADMIN + DMA-IN
            sptwb.Spt.Cdb[2] = 0;
            sptwb.Spt.Cdb[3] = 0;
            sptwb.Spt.Cdb[4] = 0x10;
            sptwb.Spt.Cdb[5] = 0;
            sptwb.Spt.Cdb[6] = 0;
            sptwb.Spt.Cdb[7] = 0;
            sptwb.Spt.Cdb[8] = 0;
            sptwb.Spt.Cdb[9] = 0;
            sptwb.Spt.Cdb[10] = 0;
            sptwb.Spt.Cdb[11] = 0;
            sptwb.Spt.DataIn = InteropConstants.SCSI_IOCTL_DATA_IN;

            length = (int)(Marshal.OffsetOf<SCSI_PASS_THROUGH_WITH_BUFFERS24>(nameof(sptwb.DataBuf)).ToInt32() + sptwb.Spt.DataTransferLength);

            Marshal.StructureToPtr(sptwb, ptr, false);

            if (!Kernel32.DeviceIoControl(handle, Kernel32.IOCTL_SCSI_PASS_THROUGH, ptr, length, ptr, length, out _, IntPtr.Zero))
            {
                Marshal.FreeHGlobal(ptr);

                identifyDevice = null;
                return false;
            }

            sptwb = Marshal.PtrToStructure<SCSI_PASS_THROUGH_WITH_BUFFERS24>(ptr);

            if (false == sptwb.DataBuf.Any(b => b != 0))
            {
                Marshal.FreeHGlobal(ptr);

                identifyDevice = null;
                return false;
            }

            identifyDevice = new IdentifyDevice();

            Marshal.Copy(sptwb.DataBuf, 0, identifyDevice.IdentifyDevicePtr, identifyDevice.PtrSize);

            Marshal.FreeHGlobal(ptr);

            return true;
        }

        public static bool DoIdentifyDeviceNVMeASMedia(Storage storage, IntPtr handle, out IdentifyDevice identifyDevice)
        {
            var sptwb = new SCSI_PASS_THROUGH_WITH_BUFFERS();

            sptwb.Spt.Length = (ushort)Marshal.SizeOf<SCSI_PASS_THROUGH>();
            sptwb.Spt.PathId = 0;
            sptwb.Spt.TargetId = 0;
            sptwb.Spt.Lun = 0;
            sptwb.Spt.SenseInfoLength = 24;
            sptwb.Spt.DataTransferLength = SharedConstants.BUFFER_SIZE;
            sptwb.Spt.TimeOutValue = 2;
            sptwb.Spt.DataBufferOffset = (ulong)Marshal.OffsetOf<SCSI_PASS_THROUGH_WITH_BUFFERS>(nameof(sptwb.DataBuf)).ToInt64();
            sptwb.Spt.SenseInfoOffset = (uint)Marshal.OffsetOf<SCSI_PASS_THROUGH_WITH_BUFFERS>(nameof(sptwb.SenseBuf)).ToInt32();

            sptwb.Spt.CdbLength = 16;
            sptwb.Spt.Cdb[0] = 0xE6; // NVME PASS THROUGH
            sptwb.Spt.Cdb[1] = 0x06; // IDENTIFY
            sptwb.Spt.Cdb[3] = 0x01;

            sptwb.Spt.DataIn = InteropConstants.SCSI_IOCTL_DATA_IN;

            var size = (int)(Marshal.OffsetOf<SCSI_PASS_THROUGH_WITH_BUFFERS>(nameof(sptwb.DataBuf)).ToInt32() + sptwb.Spt.DataTransferLength);
            var ptrSize = Marshal.SizeOf<SCSI_PASS_THROUGH_WITH_BUFFERS>();

            var ptr = Marshal.AllocHGlobal(ptrSize);
            Marshal.StructureToPtr(sptwb, ptr, false);

            if (!Kernel32.DeviceIoControl(handle, Kernel32.IOCTL_SCSI_PASS_THROUGH, ptr, size, ptr, size, out _, IntPtr.Zero))
            {
                Marshal.FreeHGlobal(ptr);

                identifyDevice = null;
                return false;
            }

            sptwb = Marshal.PtrToStructure<SCSI_PASS_THROUGH_WITH_BUFFERS>(ptr);

            if (false == sptwb.DataBuf.Any(b => b != 0))
            {
                Marshal.FreeHGlobal(ptr);

                identifyDevice = null;
                return false;
            }

            identifyDevice = new IdentifyDevice();

            Marshal.Copy(sptwb.DataBuf, 0, identifyDevice.IdentifyDevicePtr, identifyDevice.PtrSize);

            Marshal.FreeHGlobal(ptr);

            return true;
        }

        public static bool DoIdentifyDeviceNVMeRealtek(Storage storage, IntPtr handle, out IdentifyDevice identifyDevice)
        {
            var sptwb = new SCSI_PASS_THROUGH_WITH_BUFFERS();

            sptwb.Spt.Length = (ushort)Marshal.SizeOf<SCSI_PASS_THROUGH>();
            sptwb.Spt.PathId = 0;
            sptwb.Spt.TargetId = 0;
            sptwb.Spt.Lun = 0;
            sptwb.Spt.CdbLength = 16;
            sptwb.Spt.SenseInfoLength = 32;
            sptwb.Spt.DataIn = InteropConstants.SCSI_IOCTL_DATA_IN;
            sptwb.Spt.DataTransferLength = SharedConstants.BUFFER_SIZE;
            sptwb.Spt.TimeOutValue = 2;
            sptwb.Spt.DataBufferOffset = (ulong)Marshal.OffsetOf<SCSI_PASS_THROUGH_WITH_BUFFERS>(nameof(sptwb.DataBuf)).ToInt64();
            sptwb.Spt.SenseInfoOffset = (uint)Marshal.OffsetOf<SCSI_PASS_THROUGH_WITH_BUFFERS>(nameof(sptwb.SenseBuf)).ToInt32();

            sptwb.Spt.Cdb[0] = 0xE4; // NVME READ
            sptwb.Spt.Cdb[1] = 0; //BYTE(4096);
            sptwb.Spt.Cdb[2] = 16; //BYTE(4096 >> 8);
            sptwb.Spt.Cdb[3] = 0x06; // IDENTIFY
            sptwb.Spt.Cdb[4] = 0x01;

            var size = (int)(Marshal.OffsetOf<SCSI_PASS_THROUGH_WITH_BUFFERS>(nameof(sptwb.DataBuf)).ToInt32() + sptwb.Spt.DataTransferLength);
            var ptrSize = Marshal.SizeOf<SCSI_PASS_THROUGH_WITH_BUFFERS>();

            var ptr = Marshal.AllocHGlobal(ptrSize);
            Marshal.StructureToPtr(sptwb, ptr, false);

            if (!Kernel32.DeviceIoControl(handle, Kernel32.IOCTL_SCSI_PASS_THROUGH, ptr, size, ptr, size, out _, IntPtr.Zero))
            {
                Marshal.FreeHGlobal(ptr);

                identifyDevice = null;
                return false;
            }

            sptwb = Marshal.PtrToStructure<SCSI_PASS_THROUGH_WITH_BUFFERS>(ptr);

            if (false == sptwb.DataBuf.Any(b => b != 0))
            {
                Marshal.FreeHGlobal(ptr);

                identifyDevice = null;
                return false;
            }

            identifyDevice = new IdentifyDevice();

            Marshal.Copy(sptwb.DataBuf, 0, identifyDevice.IdentifyDevicePtr, identifyDevice.PtrSize);

            Marshal.FreeHGlobal(ptr);

            return true;
        }
    }
}
