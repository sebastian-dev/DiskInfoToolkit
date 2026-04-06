/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 *
 * Copyright (c) 2026 Florian K.
 */

using DiskInfoToolkit.Constants;
using System.Runtime.InteropServices;

namespace DiskInfoToolkit.Interop
{
    public struct SCSI_PASS_THROUGH_WITH_BUFFERS_EX
    {
        #region Fields

        public SCSI_PASS_THROUGH Spt;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] SenseBuf;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = BufferSizeConstants.Size4K)]
        public byte[] DataBuf;

        #endregion

        #region Public

        public static SCSI_PASS_THROUGH_WITH_BUFFERS_EX CreateDefault()
        {
            SCSI_PASS_THROUGH_WITH_BUFFERS_EX value = new SCSI_PASS_THROUGH_WITH_BUFFERS_EX();
            value.Spt = SCSI_PASS_THROUGH.CreateDefault();
            value.SenseBuf = new byte[32];
            value.DataBuf = new byte[BufferSizeConstants.Size4K];
            return value;
        }

        #endregion
    }
}
