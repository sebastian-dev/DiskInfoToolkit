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
    public struct CSMI_SAS_PHY_INFO
    {
        #region Fields

        public byte bNumberOfPhys;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] bReserved;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public CSMI_SAS_PHY_ENTITY[] Phy;

        #endregion

        #region Public

        public static CSMI_SAS_PHY_INFO CreateDefault()
        {
            CSMI_SAS_PHY_INFO value = new CSMI_SAS_PHY_INFO();
            value.bReserved = new byte[3];
            value.Phy = new CSMI_SAS_PHY_ENTITY[32];
            for (int i = 0; i < value.Phy.Length; ++i)
            {
                value.Phy[i] = CSMI_SAS_PHY_ENTITY.CreateDefault();
            }
            return value;
        }

        #endregion
    }
}
