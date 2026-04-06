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
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct STORAGE_DEVICE_DESCRIPTOR
    {
        #region Fields

        public uint Version;

        public uint Size;

        public byte DeviceType;

        public byte DeviceTypeModifier;

        [MarshalAs(UnmanagedType.I1)]
        public byte RemovableMedia;

        [MarshalAs(UnmanagedType.I1)]
        public byte CommandQueueing;

        public uint VendorIDOffset;

        public uint ProductIDOffset;

        public uint ProductRevisionOffset;

        public uint SerialNumberOffset;

        public uint BusType;

        public uint RawPropertiesLength;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] RawDeviceProperties;

        #endregion
    }
}
