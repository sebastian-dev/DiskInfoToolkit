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
    public struct STORAGE_PROTOCOL_SPECIFIC_DATA
    {
        #region Fields

        public uint ProtocolType;

        public uint DataType;

        public uint ProtocolDataRequestValue;

        public uint ProtocolDataRequestSubValue;

        public uint ProtocolDataOffset;

        public uint ProtocolDataLength;

        public uint FixedProtocolReturnData;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public uint[] Reserved;

        #endregion

        #region Public

        public static STORAGE_PROTOCOL_SPECIFIC_DATA CreateDefault()
        {
            STORAGE_PROTOCOL_SPECIFIC_DATA value = new STORAGE_PROTOCOL_SPECIFIC_DATA();
            value.Reserved = new uint[3];
            return value;
        }

        #endregion
    }
}
