/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 *
 * Copyright (c) 2026 Florian K.
 */

namespace DiskInfoToolkit.Interop
{
    internal struct HPT_IDE_PASS_THROUGH_HEADER_V2
    {
        #region Fields

        public uint DeviceID;

        public ushort FeaturesReg;

        public ushort SectorCountReg;

        public ushort LbaLowReg;

        public ushort LbaMidReg;

        public ushort LbaHighReg;

        public byte DriveHeadReg;

        public byte CommandReg;

        public ushort SectorTransferCount;

        public byte Protocol;

        public byte Reserved;

        #endregion
    }
}
