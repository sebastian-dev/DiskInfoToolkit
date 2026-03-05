﻿/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 *
 * Copyright (c) 2026 Florian K.
 *
 * Code inspiration, improvements and fixes are from, but not limited to, following projects:
 * CrystalDiskInfo
 */

using BlackSharp.Core.Interop.Windows.Mutexes;

namespace DiskInfoToolkit.Globals
{
    /// <summary>
    /// Holds instances to all <see cref="WorldMutex"/>es.
    /// </summary>
    internal class WorldMutexManager
    {
        #region Constructor

        static WorldMutexManager()
        {
            WorldJMicronMutex = new WorldMutex(JMicronMutexName);
        }

        #endregion

        #region Fields

        const string JMicronMutexName = "Access_JMicron_SMART";

        #endregion

        #region Properties

        internal static WorldMutex WorldJMicronMutex { get; }

        #endregion
    }
}
