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
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG
    {
        #region Fields

        public IntPtr hwnd;

        public uint message;

        public UIntPtr wParam;

        public IntPtr lParam;

        public uint time;

        public POINT pt;

        #endregion
    }
}
