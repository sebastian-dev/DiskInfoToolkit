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
    public struct DRIVERSTATUS
    {
        #region Fields

        public byte bDriverError;

        public byte bIDEError;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] bReserved;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public uint[] dwReserved;

        #endregion

        #region Public

        public static DRIVERSTATUS CreateDefault()
        {
            DRIVERSTATUS value = new DRIVERSTATUS();
            value.bReserved = new byte[2];
            value.dwReserved = new uint[2];
            return value;
        }

        #endregion
    }
}
