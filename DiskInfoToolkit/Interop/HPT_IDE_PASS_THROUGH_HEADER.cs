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
    internal struct HPT_IDE_PASS_THROUGH_HEADER
    {
        #region Fields

        public uint DeviceID;

        public byte FeaturesReg;

        public byte SectorCountReg;

        public byte LbaLowReg;

        public byte LbaMidReg;

        public byte LbaHighReg;

        public byte DriveHeadReg;

        public byte CommandReg;

        public byte SectorTransferCount;

        public byte Protocol;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] Reserved;

        #endregion
    }
}
