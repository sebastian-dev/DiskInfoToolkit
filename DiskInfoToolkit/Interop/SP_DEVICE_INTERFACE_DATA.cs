/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 *
 * Copyright (c) 2026 Florian K.
 */

namespace DiskInfoToolkit.Interop
{
    public struct SP_DEVICE_INTERFACE_DATA
    {
        #region Fields

        public int cbSize;

        public Guid InterfaceClassGuid;

        public int Flags;

        public IntPtr Reserved;

        #endregion
    }
}
