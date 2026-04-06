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
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct SCSI_PASS_THROUGH
    {
        #region Fields

        public ushort Length;

        public byte ScsiStatus;

        public byte PathID;

        public byte TargetID;

        public byte Lun;

        public byte CdbLength;

        public byte SenseInfoLength;

        public byte DataIn;

        public uint DataTransferLength;

        public uint TimeOutValue;

        public /*UIntPtr*/ulong DataBufferOffset;

        public uint SenseInfoOffset;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] Cdb;

        #endregion

        #region Public

        public static SCSI_PASS_THROUGH CreateDefault()
        {
            SCSI_PASS_THROUGH value = new SCSI_PASS_THROUGH();
            value.Cdb = new byte[16];
            return value;
        }

        #endregion
    }
}
