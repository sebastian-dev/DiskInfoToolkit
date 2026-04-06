/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 *
 * Copyright (c) 2026 Florian K.
 */

namespace DiskInfoToolkit.PCI
{
    internal class PCIVendor
    {
        #region Constructor

        public PCIVendor(int id, string name)
        {
            ID = id;
            Name = name;
        }

        #endregion

        #region Properties

        public int ID { get; }
        public string Name { get; }
        public List<PCIDevice> Devices { get; } = new List<PCIDevice>();

        #endregion
    }
}
