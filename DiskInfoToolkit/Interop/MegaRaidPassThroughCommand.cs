/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 *
 * Copyright (c) 2026 Florian K.
 */

namespace DiskInfoToolkit.Interop
{
    internal struct MegaRaidPassThroughCommand
    {
        #region Fields

        public byte CommandType;

        public byte SubCommand;

        public ushort Reserved;

        public uint ControllerID;

        public uint BufferLength;

        public IntPtr Buffer;

        #endregion
    }
}
