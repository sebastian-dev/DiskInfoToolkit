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
    internal struct HPT_LOGICAL_DEVICE_INFO
    {
        #region Fields

        public byte Type;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] Reserved;

        public uint Capacity;

        public uint ParentArray;

        public HPT_DEVICE_INFO Device;

        #endregion
    }
}
