/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 *
 * Copyright (c) 2026 Florian K.
 */

namespace DiskInfoToolkit.Interop
{
    public struct SCSI_ADDRESS
    {
        #region Fields

        public int Length;

        public byte PortNumber;

        public byte PathID;

        public byte TargetID;

        public byte Lun;

        #endregion
    }
}
