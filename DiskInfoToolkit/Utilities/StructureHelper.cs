/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 *
 * Copyright (c) 2026 Florian K.
 */

using System.Runtime.InteropServices;

namespace DiskInfoToolkit.Utilities
{
    public static class StructureHelper
    {
        #region Public

        public static byte[] GetBytes<T>(T structure)
            where T : struct
        {
            int size = Marshal.SizeOf<T>();
            var buffer = new byte[size];

            var ptr = Marshal.AllocHGlobal(size);

            try
            {
                Marshal.StructureToPtr(structure, ptr, false);
                Marshal.Copy(ptr, buffer, 0, size);
                return buffer;
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        public static T FromBytes<T>(byte[] buffer)
            where T : struct
        {
            int size = Marshal.SizeOf<T>();
            var ptr = Marshal.AllocHGlobal(size);

            try
            {
                Marshal.Copy(buffer, 0, ptr, size);
                return Marshal.PtrToStructure<T>(ptr);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        #endregion
    }
}
