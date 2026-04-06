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
    public struct GETVERSIONINPARAMS
    {
        #region Fields

        public byte bVersion;

        public byte bRevision;

        public byte bReserved;

        public byte bIDEDeviceMap;

        public uint fCapabilities;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public uint[] dwReserved;

        #endregion

        #region Public

        public static GETVERSIONINPARAMS CreateDefault()
        {
            GETVERSIONINPARAMS value = new GETVERSIONINPARAMS();
            value.dwReserved = new uint[4];
            return value;
        }

        #endregion
    }
}
