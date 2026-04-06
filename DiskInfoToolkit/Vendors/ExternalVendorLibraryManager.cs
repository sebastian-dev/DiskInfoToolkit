/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 *
 * Copyright (c) 2026 Florian K.
 */

using DiskInfoToolkit.Native;
using DiskInfoToolkit.Utilities;

namespace DiskInfoToolkit.Vendors
{
    public sealed class ExternalVendorLibraryManager
    {
        #region Constructor

        public ExternalVendorLibraryManager()
        {
            MegaRaidLibraryName = "storelib.dll";
            HighPointLibraryName = "hptintf.dll";
        }

        #endregion

        #region Fields

        private SafeLibraryHandle _megaRaidHandle;

        private SafeLibraryHandle _highPointHandle;

        #endregion

        #region Properties

        public string MegaRaidLibraryName { get; set; }

        public string HighPointLibraryName { get; set; }

        #endregion

        #region Public

        public SafeLibraryHandle GetMegaRaidLibrary()
        {
            if (_megaRaidHandle == null || _megaRaidHandle.IsInvalid)
            {
                _megaRaidHandle = Kernel32Native.LoadLibrarySafe(MegaRaidLibraryName);
            }

            return _megaRaidHandle;
        }

        public SafeLibraryHandle GetHighPointLibrary()
        {
            if (_highPointHandle == null || _highPointHandle.IsInvalid)
            {
                _highPointHandle = Kernel32Native.LoadLibrarySafe(HighPointLibraryName);
            }

            return _highPointHandle;
        }

        #endregion
    }
}
