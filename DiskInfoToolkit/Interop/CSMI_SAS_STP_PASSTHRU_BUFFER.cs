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
    public struct CSMI_SAS_STP_PASSTHRU_BUFFER
    {
        #region Fields

        public SRB_IO_CONTROL IoctlHeader;

        public CSMI_SAS_STP_PASSTHRU Parameters;

        public CSMI_SAS_STP_PASSTHRU_STATUS Status;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] bDataBuffer;

        #endregion

        #region Public

        public static CSMI_SAS_STP_PASSTHRU_BUFFER CreateDefault()
        {
            CSMI_SAS_STP_PASSTHRU_BUFFER value = new CSMI_SAS_STP_PASSTHRU_BUFFER();
            value.IoctlHeader = new SRB_IO_CONTROL();
            value.IoctlHeader.Signature = new byte[8];
            value.Parameters = CSMI_SAS_STP_PASSTHRU.CreateDefault();
            value.Status = CSMI_SAS_STP_PASSTHRU_STATUS.CreateDefault();
            value.bDataBuffer = new byte[1];
            return value;
        }

        #endregion
    }
}
