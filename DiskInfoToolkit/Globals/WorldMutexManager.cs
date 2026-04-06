/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 *
 * Copyright (c) 2026 Florian K.
 */

using BlackSharp.Core.Interop.Windows.Mutexes;

namespace DiskInfoToolkit.Globals
{
    internal static class WorldMutexManager
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
