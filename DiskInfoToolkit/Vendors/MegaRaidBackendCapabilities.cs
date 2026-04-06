/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 *
 * Copyright (c) 2026 Florian K.
 */

namespace DiskInfoToolkit.Vendors
{
    public sealed class MegaRaidBackendCapabilities
    {
        #region Properties

        public bool HasProcessLibCommand { get; set; }

        public bool HasCoreExports
        {
            get { return HasProcessLibCommand; }
        }

        #endregion
    }
}
