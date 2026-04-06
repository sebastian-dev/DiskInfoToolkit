/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 *
 * Copyright (c) 2026 Florian K.
 */

using DiskInfoToolkit.Constants;
using DiskInfoToolkit.Core;
using DiskInfoToolkit.Interop;
using DiskInfoToolkit.Models;
using DiskInfoToolkit.Utilities;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using System.Text;

namespace DiskInfoToolkit.Probes
{
    public static class SmartProbe
    {
        #region Fields

        private const byte SmartCommand = 0xB0;

        private const byte SmartReadDataSubcommand = 0xD0;

        private const byte SmartReadThresholdSubcommand = 0xD1;

        private const byte SmartEnableOperationsSubcommand = 0xD8;

        private const byte SmartCylinderLow = 0x4F;

        private const byte SmartCylinderHigh = 0xC2;

        private const int SmartSectorBytes = 512;

        private static readonly byte[] ScsiSignature = Encoding.ASCII.GetBytes("SCSIDISK");

        #endregion

        #region Public

        public static bool TryPopulateSmartData(StorageDevice device, IStorageIoControl ioControl)
        {
            if (device == null || ioControl == null || string.IsNullOrWhiteSpace(device.DevicePath))
            {
                return false;
            }

            SafeFileHandle handle = ioControl.OpenDevice(
                device.DevicePath,
                IoAccess.GenericRead | IoAccess.GenericWrite,
                IoShare.ReadWrite,
                IoCreation.OpenExisting,
                IoFlags.Normal);

            if (handle == null || handle.IsInvalid)
            {
                return false;
            }

            using (handle)
            {
                byte[] smartData = null;
                byte[] smartThresholds = null;

                bool ok =
                    (TryReadSmartViaAtaPassThrough(ioControl, handle, SmartReadDataSubcommand, out smartData)
                        && TryReadSmartViaAtaPassThrough(ioControl, handle, SmartReadThresholdSubcommand, out smartThresholds))
                    || (TryReadSmartViaIdePassThrough(ioControl, handle, SmartReadDataSubcommand, out smartData)
                        && TryReadSmartViaIdePassThrough(ioControl, handle, SmartReadThresholdSubcommand, out smartThresholds))
                    || (TryReadSmartViaDfp(ioControl, handle, SmartReadDataSubcommand, out smartData)
                        && TryReadSmartViaDfp(ioControl, handle, SmartReadThresholdSubcommand, out smartThresholds))
                    || (TryReadSmartViaScsiMiniport(ioControl, handle, IoControlCodes.IOCTL_SCSI_MINIPORT_READ_SMART_ATTRIBS, out smartData)
                        && TryReadSmartViaScsiMiniport(ioControl, handle, IoControlCodes.IOCTL_SCSI_MINIPORT_READ_SMART_THRESHOLDS, out smartThresholds));

                if (!ok)
                {
                    TryEnableSmartViaAtaPassThrough(ioControl, handle);
                    TryEnableSmartViaIdePassThrough(ioControl, handle);
                    TryEnableSmartViaDfp(ioControl, handle);
                    TryEnableSmartViaScsiMiniport(ioControl, handle);

                    ok =
                        (TryReadSmartViaAtaPassThrough(ioControl, handle, SmartReadDataSubcommand, out smartData)
                            && TryReadSmartViaAtaPassThrough(ioControl, handle, SmartReadThresholdSubcommand, out smartThresholds))
                        || (TryReadSmartViaIdePassThrough(ioControl, handle, SmartReadDataSubcommand, out smartData)
                            && TryReadSmartViaIdePassThrough(ioControl, handle, SmartReadThresholdSubcommand, out smartThresholds))
                        || (TryReadSmartViaDfp(ioControl, handle, SmartReadDataSubcommand, out smartData)
                            && TryReadSmartViaDfp(ioControl, handle, SmartReadThresholdSubcommand, out smartThresholds))
                        || (TryReadSmartViaScsiMiniport(ioControl, handle, IoControlCodes.IOCTL_SCSI_MINIPORT_READ_SMART_ATTRIBS, out smartData)
                            && TryReadSmartViaScsiMiniport(ioControl, handle, IoControlCodes.IOCTL_SCSI_MINIPORT_READ_SMART_THRESHOLDS, out smartThresholds));
                }

                if (!ok)
                {
                    return false;
                }

                var attributes = ParseSmartPages(smartData, smartThresholds);
                if (attributes.Count == 0)
                {
                    return false;
                }

                device.SupportsSmart = true;
                device.SmartAttributes = attributes;

                return true;
            }
        }

        public static List<SmartAttributeEntry> ParseSmartPages(byte[] dataPage, byte[] thresholdPage)
        {
            var result = new List<SmartAttributeEntry>();

            var thresholds = ParseThresholds(thresholdPage);

            if (dataPage == null || dataPage.Length < 362)
            {
                return result;
            }

            for (int offset = 2; offset + 12 <= 362; offset += 12)
            {
                byte id = dataPage[offset];
                if (id == 0)
                {
                    continue;
                }

                var entry = new SmartAttributeEntry();
                entry.ID = id;
                entry.StatusFlags = BitConverter.ToUInt16(dataPage, offset + 1);
                entry.CurrentValue = dataPage[offset + 3];
                entry.WorstValue = dataPage[offset + 4];
                entry.RawValue = ReadUInt48(dataPage, offset + 5);

                if (thresholds.TryGetValue(id, out var threshold))
                {
                    entry.ThresholdValue = threshold;
                }

                result.Add(entry);
            }

            return result;
        }

        #endregion

        #region Private

        private static bool TryReadSmartViaAtaPassThrough(IStorageIoControl ioControl, SafeFileHandle handle, byte feature, out byte[] data)
        {
            data = null;

            var request = ATA_PASS_THROUGH_EX_WITH_BUFFERS.CreateDefault();
            request.Apt.Length = (ushort)Marshal.SizeOf<ATA_PASS_THROUGH_EX>();
            request.Apt.TimeOutValue = 2;
            request.Apt.AtaFlags = 0x02;
            request.Apt.DataTransferLength = SmartSectorBytes;
            request.Apt.DataBufferOffset = (ulong)Marshal.OffsetOf<ATA_PASS_THROUGH_EX_WITH_BUFFERS>(nameof(ATA_PASS_THROUGH_EX_WITH_BUFFERS.Buf)).ToInt64();
            request.Apt.CurrentTaskFile.bFeaturesReg = feature;
            request.Apt.CurrentTaskFile.bSectorCountReg = 1;
            request.Apt.CurrentTaskFile.bSectorNumberReg = 1;
            request.Apt.CurrentTaskFile.bCylLowReg = SmartCylinderLow;
            request.Apt.CurrentTaskFile.bCylHighReg = SmartCylinderHigh;
            request.Apt.CurrentTaskFile.bCommandReg = SmartCommand;
            request.Apt.CurrentTaskFile.bDriveHeadReg = 0xA0;

            var buffer = StructureHelper.GetBytes(request);

            if (!ioControl.TryAtaPassThrough(handle, buffer, buffer, out var bytesReturned))
            {
                return false;
            }

            var response = StructureHelper.FromBytes<ATA_PASS_THROUGH_EX_WITH_BUFFERS>(buffer);

            data = new byte[SmartSectorBytes];
            Buffer.BlockCopy(response.Buf, 0, data, 0, data.Length);

            return true;
        }

        private static bool TryReadSmartViaIdePassThrough(IStorageIoControl ioControl, SafeFileHandle handle, byte feature, out byte[] data)
        {
            data = null;

            var request = ATA_PASS_THROUGH_EX_WITH_BUFFERS.CreateDefault();
            request.Apt.Length = (ushort)Marshal.SizeOf<ATA_PASS_THROUGH_EX>();
            request.Apt.TimeOutValue = 2;
            request.Apt.AtaFlags = 0x02;
            request.Apt.DataTransferLength = SmartSectorBytes;
            request.Apt.DataBufferOffset = (ulong)Marshal.OffsetOf<ATA_PASS_THROUGH_EX_WITH_BUFFERS>(nameof(ATA_PASS_THROUGH_EX_WITH_BUFFERS.Buf)).ToInt64();
            request.Apt.CurrentTaskFile.bFeaturesReg = feature;
            request.Apt.CurrentTaskFile.bSectorCountReg = 1;
            request.Apt.CurrentTaskFile.bSectorNumberReg = 1;
            request.Apt.CurrentTaskFile.bCylLowReg = SmartCylinderLow;
            request.Apt.CurrentTaskFile.bCylHighReg = SmartCylinderHigh;
            request.Apt.CurrentTaskFile.bCommandReg = SmartCommand;
            request.Apt.CurrentTaskFile.bDriveHeadReg = 0xA0;

            var buffer = StructureHelper.GetBytes(request);

            if (!ioControl.TryIdePassThrough(handle, buffer, buffer, out var bytesReturned))
            {
                return false;
            }

            var response = StructureHelper.FromBytes<ATA_PASS_THROUGH_EX_WITH_BUFFERS>(buffer);

            data = new byte[SmartSectorBytes];
            Buffer.BlockCopy(response.Buf, 0, data, 0, data.Length);

            return true;
        }

        private static bool TryReadSmartViaDfp(IStorageIoControl ioControl, SafeFileHandle handle, byte feature, out byte[] data)
        {
            data = null;

            var input = SENDCMDINPARAMS.CreateDefault();
            input.cBufferSize = SmartSectorBytes;
            input.irDriveRegs.bFeaturesReg = feature;
            input.irDriveRegs.bSectorCountReg = 1;
            input.irDriveRegs.bSectorNumberReg = 1;
            input.irDriveRegs.bCylLowReg = SmartCylinderLow;
            input.irDriveRegs.bCylHighReg = SmartCylinderHigh;
            input.irDriveRegs.bCommandReg = SmartCommand;
            input.irDriveRegs.bDriveHeadReg = 0xA0;

            var inputBuffer = StructureHelper.GetBytes(input);
            var outputBuffer = new byte[Marshal.SizeOf<SENDCMDOUTPARAMS>() + SmartSectorBytes - 1];

            if (!ioControl.TrySmartReceiveDriveData(handle, inputBuffer, outputBuffer, out var bytesReturned))
            {
                return false;
            }

            int dataOffset = Marshal.SizeOf<SENDCMDOUTPARAMS>() - 1;
            if (dataOffset + SmartSectorBytes > outputBuffer.Length)
            {
                return false;
            }

            data = new byte[SmartSectorBytes];
            Buffer.BlockCopy(outputBuffer, dataOffset, data, 0, data.Length);
            return true;
        }

        private static bool TryReadSmartViaScsiMiniport(IStorageIoControl ioControl, SafeFileHandle handle, uint controlCode, out byte[] data)
        {
            data = null;

            if (!ioControl.TryGetScsiAddress(handle, out var scsiAddress))
            {
                return false;
            }

            var srb = new SRB_IO_CONTROL();
            srb.HeaderLength = (uint)Marshal.SizeOf<SRB_IO_CONTROL>();
            srb.Signature = new byte[8];

            Array.Copy(ScsiSignature, srb.Signature, Math.Min(ScsiSignature.Length, srb.Signature.Length));

            srb.Timeout = 2;
            srb.ControlCode = controlCode;
            srb.Length = (uint)(Marshal.SizeOf<SENDCMDOUTPARAMS>() + SmartSectorBytes);

            var input = SENDCMDINPARAMS.CreateDefault();
            input.irDriveRegs.bCommandReg = SmartCommand;
            input.bDriveNumber = scsiAddress.TargetID;

            var srbBytes = StructureHelper.GetBytes(srb);
            var inputBytes = StructureHelper.GetBytes(input);

            var buffer = new byte[Marshal.SizeOf<SRB_IO_CONTROL>() + Marshal.SizeOf<SENDCMDOUTPARAMS>() + SmartSectorBytes];

            Buffer.BlockCopy(srbBytes, 0, buffer, 0, srbBytes.Length);
            Buffer.BlockCopy(inputBytes, 0, buffer, srbBytes.Length, inputBytes.Length);

            if (!ioControl.TryScsiMiniport(handle, buffer, buffer, out var bytesReturned))
            {
                return false;
            }

            int outputOffset = Marshal.SizeOf<SRB_IO_CONTROL>();
            int dataOffset = outputOffset + Marshal.SizeOf<SENDCMDOUTPARAMS>() - 1;

            if (dataOffset + SmartSectorBytes > buffer.Length)
            {
                return false;
            }

            data = new byte[SmartSectorBytes];
            Buffer.BlockCopy(buffer, dataOffset, data, 0, data.Length);

            return true;
        }

        private static bool TryEnableSmartViaAtaPassThrough(IStorageIoControl ioControl, SafeFileHandle handle)
        {
            var request = ATA_PASS_THROUGH_EX_WITH_BUFFERS.CreateDefault();
            request.Apt.Length = (ushort)Marshal.SizeOf<ATA_PASS_THROUGH_EX>();
            request.Apt.TimeOutValue = 2;
            request.Apt.AtaFlags = 0;
            request.Apt.DataTransferLength = 0;
            request.Apt.DataBufferOffset = 0;
            request.Apt.CurrentTaskFile.bFeaturesReg = SmartEnableOperationsSubcommand;
            request.Apt.CurrentTaskFile.bSectorCountReg = 1;
            request.Apt.CurrentTaskFile.bSectorNumberReg = 1;
            request.Apt.CurrentTaskFile.bCylLowReg = SmartCylinderLow;
            request.Apt.CurrentTaskFile.bCylHighReg = SmartCylinderHigh;
            request.Apt.CurrentTaskFile.bCommandReg = SmartCommand;
            request.Apt.CurrentTaskFile.bDriveHeadReg = 0xA0;

            var buffer = StructureHelper.GetBytes(request);

            return ioControl.TryAtaPassThrough(handle, buffer, buffer, out var bytesReturned);
        }

        private static bool TryEnableSmartViaIdePassThrough(IStorageIoControl ioControl, SafeFileHandle handle)
        {
            var request = ATA_PASS_THROUGH_EX_WITH_BUFFERS.CreateDefault();
            request.Apt.Length = (ushort)Marshal.SizeOf<ATA_PASS_THROUGH_EX>();
            request.Apt.TimeOutValue = 2;
            request.Apt.AtaFlags = 0;
            request.Apt.DataTransferLength = 0;
            request.Apt.DataBufferOffset = (ulong)Marshal.OffsetOf<ATA_PASS_THROUGH_EX_WITH_BUFFERS>(nameof(ATA_PASS_THROUGH_EX_WITH_BUFFERS.Buf)).ToInt64();
            request.Apt.CurrentTaskFile.bFeaturesReg = SmartEnableOperationsSubcommand;
            request.Apt.CurrentTaskFile.bSectorCountReg = 1;
            request.Apt.CurrentTaskFile.bSectorNumberReg = 1;
            request.Apt.CurrentTaskFile.bCylLowReg = SmartCylinderLow;
            request.Apt.CurrentTaskFile.bCylHighReg = SmartCylinderHigh;
            request.Apt.CurrentTaskFile.bCommandReg = SmartCommand;
            request.Apt.CurrentTaskFile.bDriveHeadReg = 0xA0;

            var buffer = StructureHelper.GetBytes(request);

            return ioControl.TryIdePassThrough(handle, buffer, buffer, out var bytesReturned);
        }

        private static bool TryEnableSmartViaDfp(IStorageIoControl ioControl, SafeFileHandle handle)
        {
            var input = SENDCMDINPARAMS.CreateDefault();
            input.cBufferSize = 0;
            input.irDriveRegs.bFeaturesReg = SmartEnableOperationsSubcommand;
            input.irDriveRegs.bSectorCountReg = 1;
            input.irDriveRegs.bSectorNumberReg = 1;
            input.irDriveRegs.bCylLowReg = SmartCylinderLow;
            input.irDriveRegs.bCylHighReg = SmartCylinderHigh;
            input.irDriveRegs.bCommandReg = SmartCommand;
            input.irDriveRegs.bDriveHeadReg = 0xA0;

            var inputBuffer = StructureHelper.GetBytes(input);
            var outputBuffer = new byte[Marshal.SizeOf<SENDCMDOUTPARAMS>()];

            return ioControl.TrySmartSendDriveCommand(handle, inputBuffer, outputBuffer, out var bytesReturned);
        }

        private static bool TryEnableSmartViaScsiMiniport(IStorageIoControl ioControl, SafeFileHandle handle)
        {
            if (!ioControl.TryGetScsiAddress(handle, out var scsiAddress))
            {
                return false;
            }

            var srb = new SRB_IO_CONTROL();
            srb.HeaderLength = (uint)Marshal.SizeOf<SRB_IO_CONTROL>();
            srb.Signature = new byte[8];

            Array.Copy(ScsiSignature, srb.Signature, Math.Min(ScsiSignature.Length, srb.Signature.Length));

            srb.Timeout = 2;
            srb.ControlCode = IoControlCodes.IOCTL_SCSI_MINIPORT_ENABLE_SMART;
            srb.Length = (uint)Marshal.SizeOf<SENDCMDINPARAMS>();

            var input = SENDCMDINPARAMS.CreateDefault();
            input.irDriveRegs.bFeaturesReg = SmartEnableOperationsSubcommand;
            input.irDriveRegs.bSectorCountReg = 1;
            input.irDriveRegs.bSectorNumberReg = 1;
            input.irDriveRegs.bCylLowReg = SmartCylinderLow;
            input.irDriveRegs.bCylHighReg = SmartCylinderHigh;
            input.irDriveRegs.bCommandReg = SmartCommand;
            input.bDriveNumber = scsiAddress.TargetID;

            var srbBytes = StructureHelper.GetBytes(srb);
            var inputBytes = StructureHelper.GetBytes(input);

            var buffer = new byte[Marshal.SizeOf<SRB_IO_CONTROL>() + inputBytes.Length];

            Buffer.BlockCopy(srbBytes, 0, buffer, 0, srbBytes.Length);
            Buffer.BlockCopy(inputBytes, 0, buffer, srbBytes.Length, inputBytes.Length);

            return ioControl.TryScsiMiniport(handle, buffer, buffer, out var bytesReturned);
        }

        private static Dictionary<byte, byte> ParseThresholds(byte[] thresholdPage)
        {
            var result = new Dictionary<byte, byte>();

            if (thresholdPage == null || thresholdPage.Length < 362)
            {
                return result;
            }

            for (int offset = 2; offset + 12 <= 362; offset += 12)
            {
                byte id = thresholdPage[offset];
                if (id == 0)
                {
                    continue;
                }

                result[id] = thresholdPage[offset + 1];
            }

            return result;
        }

        private static ulong ReadUInt48(byte[] data, int offset)
        {
            ulong value = 0;

            for (int i = 0; i < 6; ++i)
            {
                value |= ((ulong)data[offset + i]) << (8 * i);
            }

            return value;
        }

        #endregion
    }
}
