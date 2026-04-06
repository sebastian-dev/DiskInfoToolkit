/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 *
 * Copyright (c) 2026 Florian K.
 */

using DiskInfoToolkit.Constants;
using System.Runtime.InteropServices;

namespace DiskInfoToolkit.Interop
{
    public struct NVME_STORAGE_QUERY_WITH_BUFFER
    {
        #region Fields

        public STORAGE_PROPERTY_QUERY_NVME Query;

        public STORAGE_PROTOCOL_SPECIFIC_DATA ProtocolSpecific;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = BufferSizeConstants.Size4K)]
        public byte[] Buffer;

        #endregion

        #region Public

        public static NVME_STORAGE_QUERY_WITH_BUFFER CreateDefault()
        {
            NVME_STORAGE_QUERY_WITH_BUFFER value = new NVME_STORAGE_QUERY_WITH_BUFFER();
            value.Query = STORAGE_PROPERTY_QUERY_NVME.CreateDefault();
            value.ProtocolSpecific = STORAGE_PROTOCOL_SPECIFIC_DATA.CreateDefault();
            value.Buffer = new byte[BufferSizeConstants.Size4K];
            return value;
        }

        #endregion
    }
}
