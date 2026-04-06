/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 *
 * Copyright (c) 2026 Florian K.
 */

namespace DiskInfoToolkit.Vendors
{
    public sealed class HighPointBackendCapabilities
    {
        #region Properties

        public bool HasVersion { get; set; }

        public bool HasControllerCount { get; set; }

        public bool HasControllerInfo { get; set; }

        public bool HasControllerInfoV2 { get; set; }

        public bool HasControllerInfoV3 { get; set; }

        public bool HasPhysicalDevices { get; set; }

        public bool HasDeviceInfo { get; set; }

        public bool HasDeviceInfoV2 { get; set; }

        public bool HasDeviceInfoV3 { get; set; }

        public bool HasDeviceInfoV4 { get; set; }

        public bool HasDeviceInfoV5 { get; set; }

        public bool HasIdePassThrough { get; set; }

        public bool HasIdePassThroughV2 { get; set; }

        public bool HasScsiPassThrough { get; set; }

        public bool HasNvmePassThrough { get; set; }

        public bool HasCoreExports
        {
            get
            {
                return HasControllerCount
                    && (HasControllerInfo || HasControllerInfoV2 || HasControllerInfoV3)
                    && HasPhysicalDevices;
            }
        }

        #endregion
    }
}
