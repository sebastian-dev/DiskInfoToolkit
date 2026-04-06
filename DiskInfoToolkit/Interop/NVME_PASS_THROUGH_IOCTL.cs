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
    public struct NVME_PASS_THROUGH_IOCTL
    {
        #region Fields

        public SRB_IO_CONTROL SrbIoCtrl;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public uint[] VendorSpecific;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public uint[] NVMeCmd;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public uint[] CplEntry;

        public uint Direction;

        public uint QueueID;

        public uint DataBufferLen;

        public uint MetaDataLen;

        public uint ReturnBufferLen;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = BufferSizeConstants.Size4K)]
        public byte[] DataBuffer;

        #endregion

        #region Public

        public static NVME_PASS_THROUGH_IOCTL CreateDefault()
        {
            NVME_PASS_THROUGH_IOCTL value = new NVME_PASS_THROUGH_IOCTL();
            value.SrbIoCtrl = new SRB_IO_CONTROL();
            value.VendorSpecific = new uint[6];
            value.NVMeCmd = new uint[16];
            value.CplEntry = new uint[4];
            value.DataBuffer = new byte[BufferSizeConstants.Size4K];
            return value;
        }

        #endregion
    }
}
