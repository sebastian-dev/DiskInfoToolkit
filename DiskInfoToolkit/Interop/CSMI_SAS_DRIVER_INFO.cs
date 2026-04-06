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
    public struct CSMI_SAS_DRIVER_INFO
    {
        #region Fields

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)]
        public byte[] szName;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)]
        public byte[] szDescription;

        public ushort usMajorRevision;

        public ushort usMinorRevision;

        public ushort usBuildRevision;

        public ushort usReleaseRevision;

        public ushort usCSMIMajorRevision;

        public ushort usCSMIMinorRevision;

        #endregion

        #region Public

        public static CSMI_SAS_DRIVER_INFO CreateDefault()
        {
            CSMI_SAS_DRIVER_INFO value = new CSMI_SAS_DRIVER_INFO();
            value.szName = new byte[81];
            value.szDescription = new byte[81];
            return value;
        }

        #endregion
    }
}
