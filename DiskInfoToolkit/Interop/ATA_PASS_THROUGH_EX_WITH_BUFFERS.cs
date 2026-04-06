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
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct ATA_PASS_THROUGH_EX_WITH_BUFFERS
    {
        #region Fields

        public ATA_PASS_THROUGH_EX Apt;

        public uint Filler;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
        public byte[] Buf;

        #endregion

        #region Public

        public static ATA_PASS_THROUGH_EX_WITH_BUFFERS CreateDefault()
        {
            ATA_PASS_THROUGH_EX_WITH_BUFFERS value = new ATA_PASS_THROUGH_EX_WITH_BUFFERS();
            value.Buf = new byte[512];
            return value;
        }

        #endregion
    }
}
