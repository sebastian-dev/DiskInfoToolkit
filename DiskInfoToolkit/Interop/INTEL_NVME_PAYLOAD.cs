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
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct INTEL_NVME_PAYLOAD
    {
        #region Fields

        public byte Version;

        public byte PathID;

        public byte TargetID;

        public byte Lun;

        public INTEL_NVME_COMMAND Cmd;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public uint[] CompletionEntry;

        public uint QueueID;

        public uint ParameterBufferLength;

        public uint ReturnBufferLength;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x28)]
        public byte[] Reserved;

        #endregion

        #region Public

        public static INTEL_NVME_PAYLOAD CreateDefault()
        {
            INTEL_NVME_PAYLOAD value = new INTEL_NVME_PAYLOAD();
            value.CompletionEntry = new uint[4];
            value.Reserved = new byte[0x28];
            return value;
        }

        #endregion
    }
}
