/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 *
 * Copyright (c) 2025 Florian K.
 *
 * Code inspiration, improvements and fixes are from, but not limited to, following projects:
 * CrystalDiskInfo
 */

using BlackSharp.Core.Interop.Windows.Enums;
using BlackSharp.Core.Interop.Windows.Utilities;
using DiskInfoToolkit.Interop.Structures;
using System.Runtime.InteropServices;

namespace DiskInfoToolkit.Interop
{
    internal static class SharedMethods
    {
        #region Public

        public static bool GetScsiAddress(IntPtr handle, out SCSI_ADDRESS scsiAddress)
        {
            var sa = new SCSI_ADDRESS();
            var length = Marshal.SizeOf<SCSI_ADDRESS>();

            var ptr = Marshal.AllocHGlobal(length);

            try
            {
                Marshal.StructureToPtr(sa, ptr, false);

                if (!Kernel32.DeviceIoControl(handle, Kernel32.IOCTL_SCSI_GET_ADDRESS, IntPtr.Zero, 0, ptr, length, out _, IntPtr.Zero))
                {
                    scsiAddress = default;

                    return false;
                }

                scsiAddress = Marshal.PtrToStructure<SCSI_ADDRESS>(ptr);

                return true;
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        public static bool TryGetScsiHandle(SCSI_ADDRESS scsiAddress, FileFlagsAndAttributes fileFlagsAndAttributes, out IntPtr scsiHandle)
        {
            var scsiStr = $@"\\.\Scsi{scsiAddress.PortNumber}:";

            scsiHandle = SafeFileHandler.OpenHandle(scsiStr, fileFlagsAndAttributes);

            if (!SafeFileHandler.IsHandleValid(scsiHandle))
            {
                return false;
            }

            return true;
        }

        public static bool TryGetScsiHandle(SCSI_ADDRESS scsiAddress, out IntPtr scsiHandle)
        {
            return TryGetScsiHandle(scsiAddress, (FileFlagsAndAttributes)0, out scsiHandle);
        }

        #endregion
    }
}
