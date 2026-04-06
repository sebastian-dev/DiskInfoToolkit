/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 *
 * Copyright (c) 2026 Florian K.
 */

namespace DiskInfoToolkit.Interop
{
    public struct DRIVE_LAYOUT_INFORMATION_EX_RAW
    {
        #region Fields

        public uint PartitionStyle;

        public uint PartitionCount;

        public DRIVE_LAYOUT_INFORMATION_UNION_RAW Layout;

        public PARTITION_INFORMATION_EX_RAW PartitionInformation;

        #endregion
    }
}
