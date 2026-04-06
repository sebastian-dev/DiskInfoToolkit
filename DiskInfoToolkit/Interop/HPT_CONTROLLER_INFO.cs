/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 *
 * Copyright (c) 2026 Florian K.
 */

using System.Runtime.InteropServices;

namespace DiskInfoToolkit.Interop
{
    internal struct HPT_CONTROLLER_INFO
    {
        #region Fields

        public byte ChipType;

        public byte InterruptLevel;

        public byte NumBuses;

        public byte ChipFlags;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 36)]
        public byte[] ProductID;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 36)]
        public byte[] VendorID;

        #endregion
    }
}
