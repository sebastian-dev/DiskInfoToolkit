/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 *
 * Copyright (c) 2026 Florian K.
 */

using System.Runtime.InteropServices;

namespace DiskInfoToolkit.Interop
{
    [StructLayout(LayoutKind.Explicit)]
    public struct PARTITION_INFORMATION_EX_RAW
    {
        #region Fields

        [FieldOffset(0)]
        public int PartitionStyle;

        [FieldOffset(8)]
        public long StartingOffset;

        [FieldOffset(16)]
        public long PartitionLength;

        [FieldOffset(24)]
        public uint PartitionNumber;

        [FieldOffset(28)]
        public byte RewritePartition;

        [FieldOffset(29)]
        public byte IsServicePartition;

        [FieldOffset(32)]
        public PARTITION_INFORMATION_UNION_RAW Layout;

        #endregion
    }
}
