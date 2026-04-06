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
    [StructLayout(LayoutKind.Sequential)]
    public struct SCSI_PASS_THROUGH_WITH_BUFFERS
    {
        #region Fields

        public SCSI_PASS_THROUGH Spt;

        public uint Filler;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] SenseBuf;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
        public byte[] DataBuf;

        #endregion

        #region Public

        public static SCSI_PASS_THROUGH_WITH_BUFFERS CreateDefault()
        {
            SCSI_PASS_THROUGH_WITH_BUFFERS value = new SCSI_PASS_THROUGH_WITH_BUFFERS();
            value.Spt = SCSI_PASS_THROUGH.CreateDefault();
            value.SenseBuf = new byte[32];
            value.DataBuf = new byte[512];
            return value;
        }

        #endregion
    }
}
