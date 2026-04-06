/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 *
 * Copyright (c) 2026 Florian K.
 */

using System.Runtime.InteropServices;

namespace DiskInfoToolkit.Native
{
    internal static class CfgMgr32Native
    {
        #region Fields

        public const string DLL_NAME = "cfgmgr32.dll";

        #endregion

        #region Public

        [DllImport(DLL_NAME, CharSet = CharSet.Unicode)]
        public static extern int CM_Get_Parent(out uint pdnDevInst, uint dnDevInst, int flags);

        [DllImport(DLL_NAME, CharSet = CharSet.Unicode)]
        public static extern int CM_Get_Device_ID(uint dnDevInst, [Out] char[] buffer, int bufferLen, int flags);

        [DllImport(DLL_NAME, CharSet = CharSet.Unicode)]
        public static extern int CM_Get_DevNode_Registry_Property(uint dnDevInst, uint ulProperty, out int regDataType, [Out] byte[] buffer, ref int bufferLen, int flags);

        #endregion
    }
}
