/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 *
 * Copyright (c) 2026 Florian K.
 */

namespace DiskInfoToolkit.Interop
{
    public struct STORAGE_ADAPTER_DESCRIPTOR
    {
        #region Fields

        public uint Version;

        public uint Size;

        public uint MaximumTransferLength;

        public uint MaximumPhysicalPages;

        public uint AlignmentMask;

        public byte AdapterUsesPio;

        public byte AdapterScansDown;

        public byte CommandQueueing;

        public byte AcceleratedTransfer;

        public byte BusType;

        public ushort BusMajorVersion;

        public ushort BusMinorVersion;

        public byte SrbType;

        public byte AddressType;

        #endregion
    }
}
