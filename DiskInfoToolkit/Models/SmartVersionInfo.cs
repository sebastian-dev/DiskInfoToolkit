/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 *
 * Copyright (c) 2026 Florian K.
 */

namespace DiskInfoToolkit.Models
{
    public sealed class SmartVersionInfo
    {
        #region Constructor

        public SmartVersionInfo()
        {
            ReservedValues = [];
        }

        #endregion

        #region Properties

        public byte Version { get; set; }

        public byte Revision { get; set; }

        public byte Reserved { get; set; }

        public byte IdeDeviceMap { get; set; }

        public uint Capabilities { get; set; }

        public uint[] ReservedValues { get; set; }

        #endregion
    }
}
