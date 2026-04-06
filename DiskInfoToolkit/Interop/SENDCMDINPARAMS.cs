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
    public struct SENDCMDINPARAMS
    {
        #region Fields

        public uint cBufferSize;

        public IDEREGS irDriveRegs;

        public byte bDriveNumber;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] bReserved;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public uint[] dwReserved;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] bBuffer;

        #endregion

        #region Public

        public static SENDCMDINPARAMS CreateDefault()
        {
            SENDCMDINPARAMS value = new SENDCMDINPARAMS();
            value.bReserved = new byte[3];
            value.dwReserved = new uint[4];
            value.bBuffer = new byte[1];
            return value;
        }

        #endregion
    }
}
