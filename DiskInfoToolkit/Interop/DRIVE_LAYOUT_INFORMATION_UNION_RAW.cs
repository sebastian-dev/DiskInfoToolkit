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
    [StructLayout(LayoutKind.Explicit)]
    public struct DRIVE_LAYOUT_INFORMATION_UNION_RAW
    {
        #region Fields

        [FieldOffset(0)]
        public DRIVE_LAYOUT_INFORMATION_MBR_RAW Mbr;

        [FieldOffset(0)]
        public DRIVE_LAYOUT_INFORMATION_GPT_RAW Gpt;

        #endregion
    }
}
