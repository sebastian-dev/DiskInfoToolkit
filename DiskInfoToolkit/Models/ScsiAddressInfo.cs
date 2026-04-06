/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 *
 * Copyright (c) 2026 Florian K.
 */

namespace DiskInfoToolkit.Models
{
    public sealed class ScsiAddressInfo
    {
        #region Properties

        public int Length { get; set; }

        public byte PortNumber { get; set; }

        public byte PathID { get; set; }

        public byte TargetID { get; set; }

        public byte Lun { get; set; }

        #endregion
    }
}
