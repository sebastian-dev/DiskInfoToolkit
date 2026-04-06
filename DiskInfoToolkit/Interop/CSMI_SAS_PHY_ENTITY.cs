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
    public struct CSMI_SAS_PHY_ENTITY
    {
        #region Fields

        public CSMI_SAS_IDENTIFY Identify;

        public byte bPortIdentifier;

        public byte bNegotiatedLinkRate;

        public byte bMinimumLinkRate;

        public byte bMaximumLinkRate;

        public byte bPhyChangeCount;

        public byte bAutoDiscover;

        public byte bPhyFeatures;

        public byte bReserved;

        public CSMI_SAS_IDENTIFY Attached;

        #endregion

        #region Public

        public static CSMI_SAS_PHY_ENTITY CreateDefault()
        {
            CSMI_SAS_PHY_ENTITY value = new CSMI_SAS_PHY_ENTITY();
            value.Identify = CSMI_SAS_IDENTIFY.CreateDefault();
            value.Attached = CSMI_SAS_IDENTIFY.CreateDefault();
            return value;
        }

        #endregion
    }
}
