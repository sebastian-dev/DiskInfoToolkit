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
    public struct CSMI_SAS_STP_PASSTHRU
    {
        #region Fields

        public byte bPhyIdentifier;

        public byte bPortIdentifier;

        public byte bConnectionRate;

        public byte bReserved;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] bDestinationSASAddress;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] bReserved2;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public byte[] bCommandFIS;

        public uint uFlags;

        public uint uDataLength;

        #endregion

        #region Public

        public static CSMI_SAS_STP_PASSTHRU CreateDefault()
        {
            CSMI_SAS_STP_PASSTHRU value = new CSMI_SAS_STP_PASSTHRU();
            value.bDestinationSASAddress = new byte[8];
            value.bReserved2 = new byte[4];
            value.bCommandFIS = new byte[20];
            return value;
        }

        #endregion
    }
}
