/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 *
 * Copyright (c) 2026 Florian K.
 */

namespace DiskInfoToolkit.Interop
{
    public struct INTEL_NVME_COMMAND
    {
        #region Fields

        public uint CDW0;

        public uint NSID;

        public uint Reserved1;

        public uint Reserved2;

        public ulong MetadataPointer;

        public ulong PRP1;

        public ulong PRP2;

        public uint CDW10;

        public uint CDW11;

        public uint CDW12;

        public uint CDW13;

        public uint CDW14;

        public uint CDW15;

        #endregion
    }
}
