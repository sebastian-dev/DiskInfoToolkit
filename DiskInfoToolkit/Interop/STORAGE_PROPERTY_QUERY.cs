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
    public struct STORAGE_PROPERTY_QUERY
    {
        #region Fields

        public int PropertyID;

        public int QueryType;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] AdditionalParameters;

        #endregion

        #region Public

        public static STORAGE_PROPERTY_QUERY CreateDefault()
        {
            STORAGE_PROPERTY_QUERY value = new STORAGE_PROPERTY_QUERY();
            value.AdditionalParameters = new byte[1];
            return value;
        }

        #endregion
    }
}
