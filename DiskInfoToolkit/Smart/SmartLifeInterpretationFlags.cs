/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 *
 * Copyright (c) 2026 Florian K.
 */

namespace DiskInfoToolkit.Smart
{
    [Flags]
    internal enum SmartLifeInterpretationFlags
    {
        None                        = 0x00,
        RawValue                    = 0x01,
        RawValueIncrement           = 0x02,
        SanDiskUsbMemory            = 0x04,
        SanDiskHundredthsInTwoBytes = 0x08,
        SanDiskByte1Remaining       = 0x10,
        SanDiskCurrentValue         = 0x20,
    }
}
