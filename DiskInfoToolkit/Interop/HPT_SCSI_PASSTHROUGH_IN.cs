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
    internal struct HPT_SCSI_PASSTHROUGH_IN
    {
        #region Fields

        public uint DeviceID;

        public byte Protocol;

        public byte Reserve1;

        public byte Reserve2;

        public byte CdbLength;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] Cdb;

        public uint DataLength;

        #endregion
    }
}
