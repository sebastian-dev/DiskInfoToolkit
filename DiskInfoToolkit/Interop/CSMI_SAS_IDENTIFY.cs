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
    public struct CSMI_SAS_IDENTIFY
    {
        #region Fields

        public byte bDeviceType;

        public byte bRestricted;

        public byte bInitiatorPortProtocol;

        public byte bTargetPortProtocol;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] bRestricted2;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] bSASAddress;

        public byte bPhyIdentifier;

        public byte bSignalClass;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] bReserved;

        #endregion

        #region Public

        public static CSMI_SAS_IDENTIFY CreateDefault()
        {
            CSMI_SAS_IDENTIFY value = new CSMI_SAS_IDENTIFY();
            value.bRestricted2 = new byte[8];
            value.bSASAddress = new byte[8];
            value.bReserved = new byte[6];
            return value;
        }

        #endregion
    }
}
