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
    public struct PARTITION_INFORMATION_MBR_RAW
    {
        #region Fields

        [FieldOffset(0)]
        public byte PartitionType;

        [FieldOffset(1)]
        public byte BootIndicator;

        [FieldOffset(2)]
        public byte RecognizedPartition;

        [FieldOffset(3)]
        public byte Reserved;

        [FieldOffset(4)]
        public uint HiddenSectors;

        [FieldOffset(8)]
        public Guid PartitionID;

        #endregion
    }
}
