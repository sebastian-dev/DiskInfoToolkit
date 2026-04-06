/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 *
 * Copyright (c) 2026 Florian K.
 */

namespace DiskInfoToolkit.Interop
{
    public struct ATA_PASS_THROUGH_EX
    {
        #region Fields

        public ushort Length;

        public ushort AtaFlags;

        public byte PathID;

        public byte TargetID;

        public byte Lun;

        public byte ReservedAsUchar;

        public uint DataTransferLength;

        public uint TimeOutValue;

        public uint ReservedAsUlong;

        public uint Padding;

        public ulong DataBufferOffset;

        public IDEREGS PreviousTaskFile;

        public IDEREGS CurrentTaskFile;

        #endregion
    }
}
