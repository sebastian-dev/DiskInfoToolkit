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
    public struct CSMI_SAS_STP_PASSTHRU_STATUS
    {
        #region Fields

        public byte bConnectionStatus;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] bReserved;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public byte[] bStatusFIS;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public uint[] uSCR;

        public uint uDataBytes;

        #endregion

        #region Public

        public static CSMI_SAS_STP_PASSTHRU_STATUS CreateDefault()
        {
            CSMI_SAS_STP_PASSTHRU_STATUS value = new CSMI_SAS_STP_PASSTHRU_STATUS();
            value.bReserved = new byte[3];
            value.bStatusFIS = new byte[20];
            value.uSCR = new uint[16];
            return value;
        }

        #endregion
    }
}
