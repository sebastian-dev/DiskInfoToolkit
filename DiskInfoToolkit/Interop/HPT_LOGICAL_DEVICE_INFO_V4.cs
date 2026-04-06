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
    internal struct HPT_LOGICAL_DEVICE_INFO_V4
    {
        #region Fields

        public uint Size;

        public byte Revision;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        public byte[] Reserved;

        public byte Type;

        public byte CachePolicy;

        public byte VBusID;

        public byte TargetID;

        public ulong Capacity;

        public uint ParentArray;

        public uint TotalIOs;

        public uint TotalMBs;

        public uint IoPerSec;

        public uint MbPerSec;

        public HPT_DEVICE_INFO_V2 Device;

        #endregion
    }
}
