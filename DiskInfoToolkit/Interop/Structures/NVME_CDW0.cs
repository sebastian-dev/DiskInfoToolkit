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
    public struct NVME_CDW0
    {
        private uint _value;

        public byte Opcode { get => (byte  )BitHandler.GetBits(_value,  0,  7); set => _value = BitHandler.SetBits(_value, value,  0,  7); }
        public byte FUSE   { get => (byte  )BitHandler.GetBits(_value,  8,  9); set => _value = BitHandler.SetBits(_value, value,  8,  9); }
        public byte Rsvd   { get => (byte  )BitHandler.GetBits(_value, 10, 13); set => _value = BitHandler.SetBits(_value, value, 10, 13); }
        public byte PSDT   { get => (byte  )BitHandler.GetBits(_value, 14, 15); set => _value = BitHandler.SetBits(_value, value, 14, 15); }
        public ushort CID  { get => (ushort)BitHandler.GetBits(_value, 16, 31); set => _value = BitHandler.SetBits(_value, value, 16, 31); }
        public uint AsDWord => _value;
    }
}
