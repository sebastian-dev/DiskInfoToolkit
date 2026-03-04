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

using BlackSharp.Core.BitOperations;
using System.Runtime.InteropServices;

namespace DiskInfoToolkit.Interop.Structures
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NVME_GET_LOG_PAGE_CDW10
    {
        private uint _value;

        public byte LID    { get => (byte  )BitHandler.GetBits(_value,  0,  7); set => _value = BitHandler.SetBits(_value, value,  0,  7); }
        public ushort NUMD { get => (ushort)BitHandler.GetBits(_value, 16, 27); set => _value = BitHandler.SetBits(_value, value, 16, 27); }
        public uint AsDWord => _value;
    }
}
