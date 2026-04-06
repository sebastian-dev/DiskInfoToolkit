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
    public struct STORAGE_PREDICT_FAILURE
    {
        #region Fields

        public uint PredictFailure;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
        public byte[] VendorSpecific;

        #endregion

        #region Public

        public static STORAGE_PREDICT_FAILURE CreateDefault()
        {
            STORAGE_PREDICT_FAILURE value = new STORAGE_PREDICT_FAILURE();
            value.VendorSpecific = new byte[512];
            return value;
        }

        #endregion
    }
}
