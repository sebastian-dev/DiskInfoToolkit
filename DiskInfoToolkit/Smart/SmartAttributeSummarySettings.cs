/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 *
 * Copyright (c) 2026 Florian K.
 */

namespace DiskInfoToolkit.Smart
{
    internal sealed class SmartAttributeSummarySettings
    {
        #region Constructor

        public SmartAttributeSummarySettings()
        {
            Profile = SmartAttributeProfile.Unknown;
            HostReadWriteUnit = SmartHostReadWriteUnit.Unknown;
            NandWriteUnit = SmartNandWriteUnit.Unknown;
            LifeFlags = SmartLifeInterpretationFlags.None;
        }

        #endregion

        #region Properties

        public SmartAttributeProfile Profile { get; set; }

        public SmartHostReadWriteUnit HostReadWriteUnit { get; set; }

        public SmartNandWriteUnit NandWriteUnit { get; set; }

        public SmartLifeInterpretationFlags LifeFlags { get; set; }

        #endregion
    }
}
