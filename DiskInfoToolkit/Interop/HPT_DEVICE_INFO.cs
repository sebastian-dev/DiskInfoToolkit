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
    internal struct HPT_DEVICE_INFO
    {
        #region Fields

        public byte ControllerID;

        public byte PathID;

        public byte TargetID;

        public byte DeviceModeSetting;

        public byte DeviceType;

        public byte UsableMode;

        public byte FeatureFlags1;

        public byte FeatureFlags2;

        public uint Flags;

        public HPT_IDENTIFY_DATA2 IdentifyData;

        #endregion
    }
}
