/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 *
 * Copyright (c) 2026 Florian K.
 */

namespace DiskInfoToolkit.Interop
{
    public struct IDEREGS
    {
        #region Fields

        public byte bFeaturesReg;

        public byte bSectorCountReg;

        public byte bSectorNumberReg;

        public byte bCylLowReg;

        public byte bCylHighReg;

        public byte bDriveHeadReg;

        public byte bCommandReg;

        public byte bReserved;

        #endregion
    }
}
