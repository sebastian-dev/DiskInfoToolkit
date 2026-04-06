/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 *
 * Copyright (c) 2026 Florian K.
 */

using System.Runtime.InteropServices;

namespace DiskInfoToolkit.Interop
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct HPT_IDENTIFY_DATA2
    {
        #region Fields

        public ushort GeneralConfiguration;

        public ushort NumberOfCylinders;

        public ushort Reserved1;

        public ushort NumberOfHeads;

        public ushort UnformattedBytesPerTrack;

        public ushort UnformattedBytesPerSector;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] SasAddress;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public ushort[] SerialNumber;

        public ushort BufferType;

        public ushort BufferSectorSize;

        public ushort NumberOfEccBytes;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public ushort[] FirmwareRevision;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public ushort[] ModelNumber;

        public byte MaximumBlockTransfer;

        public byte VendorUnique2;

        public ushort DoubleWordIo;

        public ushort Capabilities;

        public ushort Reserved2;

        public byte VendorUnique3;

        public byte PioCycleTimingMode;

        public byte VendorUnique4;

        public byte DmaCycleTimingMode;

        public ushort TranslationFieldsValid;

        public ushort NumberOfCurrentCylinders;

        public ushort NumberOfCurrentHeads;

        public ushort CurrentSectorsPerTrack;

        public uint CurrentSectorCapacity;

        public ushort CurrentMultiSectorSetting;

        public uint UserAddressableSectors;

        public byte SingleWordDmaSupport;

        public byte SingleWordDmaActive;

        public byte MultiWordDmaSupport;

        public byte MultiWordDmaActive;

        public byte AdvancedPioModes;

        public byte Reserved4;

        public ushort MinimumMwxferCycleTime;

        public ushort RecommendedMwxferCycleTime;

        public ushort MinimumPioCycleTime;

        public ushort MinimumPioCycleTimeIordy;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public ushort[] Reserved5;

        public ushort ReleaseTimeOverlapped;

        public ushort ReleaseTimeServiceCommand;

        public ushort MajorRevision;

        public ushort MinorRevision;

        #endregion
    }
}
