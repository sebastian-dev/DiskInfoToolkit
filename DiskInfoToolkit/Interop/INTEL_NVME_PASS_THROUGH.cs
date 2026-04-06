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
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct INTEL_NVME_PASS_THROUGH
    {
        #region Fields

        public SRB_IO_CONTROL Srb;

        public INTEL_NVME_PAYLOAD Payload;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = BufferSizeConstants.Size4K)]
        public byte[] DataBuffer;

        #endregion

        #region Public

        public static INTEL_NVME_PASS_THROUGH CreateDefault()
        {
            INTEL_NVME_PASS_THROUGH value = new INTEL_NVME_PASS_THROUGH();
            value.Srb = new SRB_IO_CONTROL();
            value.Payload = INTEL_NVME_PAYLOAD.CreateDefault();
            value.DataBuffer = new byte[BufferSizeConstants.Size4K];
            return value;
        }

        #endregion
    }
}
