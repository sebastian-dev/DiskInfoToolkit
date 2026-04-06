/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 *
 * Copyright (c) 2026 Florian K.
 */

namespace DiskInfoToolkit.Interop
{
    public struct DEV_BROADCAST_DEVICEINTERFACE
    {
        #region Fields

        public int dbcc_size;

        public int dbcc_devicetype;

        public int dbcc_reserved;

        public Guid dbcc_classguid;

        public short dbcc_name;

        #endregion
    }
}
