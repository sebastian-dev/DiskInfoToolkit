/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 *
 * Copyright (c) 2026 Florian K.
 */

namespace DiskInfoToolkit.PCI
{
    internal class PCISubDevice
    {
        #region Constructor

        public PCISubDevice(int subVendor, int subDevice, string name)
        {
            SubVendor = subVendor;
            SubDevice = subDevice;
            Name = name;
        }

        #endregion

        #region Properties

        public int SubVendor { get; }
        public int SubDevice { get; }
        public string Name { get; }

        #endregion
    }
}
