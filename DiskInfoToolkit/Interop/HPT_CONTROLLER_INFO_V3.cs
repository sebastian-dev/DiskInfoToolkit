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
    internal struct HPT_CONTROLLER_INFO_V3
    {
        #region Fields

        public byte ChipType;

        public byte InterruptLevel;

        public byte NumBuses;

        public byte ChipFlags;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 36)]
        public byte[] ProductID;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 36)]
        public byte[] VendorID;

        public uint GroupID;

        public byte PciTree;

        public byte PciBus;

        public byte PciDevice;

        public byte PciFunction;

        public uint ExFlags;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] IopModel;

        public uint SdramSize;

        public byte BatteryInstalled;

        public byte BatteryStatus;

        public ushort BatteryVoltage;

        public uint BatteryBackupTime;

        public uint FirmwareVersion;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] SerialNumber;

        public byte BatteryMbInstalled;

        public byte BatteryTemperature;

        public sbyte CpuTemperature;

        public sbyte BoardTemperature;

        public ushort FanSpeed;

        public ushort Power12v;

        public ushort Power5v;

        public ushort Power3p3v;

        public ushort Power2p5v;

        public ushort Power1p8v;

        public ushort Core1p8v;

        public ushort Core1p2v;

        public ushort Ddr1p8v;

        public ushort Ddr1p8vRef;

        public ushort Core1p0v;

        public ushort Fan2Speed;

        public ushort Power1p0v;

        public ushort Power1p5v;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] SasAddress;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 48)]
        public byte[] Reserve;

        #endregion
    }
}
