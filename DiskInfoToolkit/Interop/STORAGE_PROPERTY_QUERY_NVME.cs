/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 *
 * Copyright (c) 2026 Florian K.
 */

namespace DiskInfoToolkit.Interop
{
    public struct STORAGE_PROPERTY_QUERY_NVME
    {
        #region Fields

        public int PropertyID;

        public int QueryType;

        #endregion

        #region Public

        public static STORAGE_PROPERTY_QUERY_NVME CreateDefault()
        {
            return new STORAGE_PROPERTY_QUERY_NVME();
        }

        #endregion
    }
}
