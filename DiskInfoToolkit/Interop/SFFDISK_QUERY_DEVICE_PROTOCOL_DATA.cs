/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 *
 * Copyright (c) 2026 Florian K.
 */

namespace DiskInfoToolkit.Interop
{
    public struct SFFDISK_QUERY_DEVICE_PROTOCOL_DATA
    {
        #region Fields

        public ushort Size;

        public ushort Reserved;

        public Guid ProtocolGuid;

        #endregion
    }
}
