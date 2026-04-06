/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 *
 * Copyright (c) 2026 Florian K.
 */

namespace DiskInfoToolkit.Interop
{
    public struct DISK_GEOMETRY
    {
        #region Fields

        public long Cylinders;

        public uint MediaType;

        public uint TracksPerCylinder;

        public uint SectorsPerTrack;

        public uint BytesPerSector;

        #endregion
    }
}
