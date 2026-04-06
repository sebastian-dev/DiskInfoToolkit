/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 *
 * Copyright (c) 2026 Florian K.
 */

namespace DiskInfoToolkit.Interop
{
    public struct DRIVE_LAYOUT_INFORMATION_GPT_RAW
    {
        #region Fields

        public Guid DiskID;

        public long StartingUsableOffset;

        public long UsableLength;

        public uint MaxPartitionCount;

        public uint Reserved;

        #endregion
    }
}
