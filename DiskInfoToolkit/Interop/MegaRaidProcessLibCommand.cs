/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 *
 * Copyright (c) 2026 Florian K.
 */

namespace DiskInfoToolkit.Interop
{
    internal struct MegaRaidProcessLibCommand
    {
        #region Fields

        public uint CommandType;

        public uint InitializeFlag;

        public uint SubCommand;

        public uint ControllerID;

        public uint PhysicalDriveID;

        public uint OutputSize;

        public IntPtr OutputBuffer;

        #endregion
    }
}
