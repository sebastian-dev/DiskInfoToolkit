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

using DiskInfoToolkit.Interop.Structures;
using System.Runtime.InteropServices;

namespace DiskInfoToolkit.Interop
{
    internal static class SharedMethods
    {
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
    }
}
