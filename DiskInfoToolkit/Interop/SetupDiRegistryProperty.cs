/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 *
 * Copyright (c) 2026 Florian K.
 */

namespace DiskInfoToolkit.Interop
{
    public enum SetupDiRegistryProperty : uint
    {
        SPDRP_DEVICEDESC = 0x00000000,
        SPDRP_HARDWAREID = 0x00000001,
        SPDRP_FRIENDLYNAME = 0x0000000C
    }
}
