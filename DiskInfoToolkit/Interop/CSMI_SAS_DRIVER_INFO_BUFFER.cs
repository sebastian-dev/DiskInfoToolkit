/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 *
 * Copyright (c) 2026 Florian K.
 */

namespace DiskInfoToolkit.Interop
{
    public struct CSMI_SAS_DRIVER_INFO_BUFFER
    {
        #region Fields

        public SRB_IO_CONTROL IoctlHeader;

        public CSMI_SAS_DRIVER_INFO Information;

        #endregion

        #region Public

        public static CSMI_SAS_DRIVER_INFO_BUFFER CreateDefault()
        {
            CSMI_SAS_DRIVER_INFO_BUFFER value = new CSMI_SAS_DRIVER_INFO_BUFFER();
            value.IoctlHeader = new SRB_IO_CONTROL();
            value.IoctlHeader.Signature = new byte[8];
            value.Information = CSMI_SAS_DRIVER_INFO.CreateDefault();
            return value;
        }

        #endregion
    }
}
