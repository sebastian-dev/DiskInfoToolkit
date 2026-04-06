/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 *
 * Copyright (c) 2026 Florian K.
 */

namespace DiskInfoToolkit.Interop
{
    public enum CmDeviceRegistryProperty : uint
    {
        HardwareId = 0x00000002,
        Service = 0x00000005,
        Class = 0x00000008
    }
}
