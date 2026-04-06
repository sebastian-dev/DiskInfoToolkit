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
    public struct SENDCMDOUTPARAMS
    {
        #region Fields

        public uint cBufferSize;

        public DRIVERSTATUS DriverStatus;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] bBuffer;

        #endregion

        #region Public

        public static SENDCMDOUTPARAMS CreateDefault()
        {
            SENDCMDOUTPARAMS value = new SENDCMDOUTPARAMS();
            value.DriverStatus = DRIVERSTATUS.CreateDefault();
            value.bBuffer = new byte[1];
            return value;
        }

        #endregion
    }
}
