/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 *
 * Copyright (c) 2026 Florian K.
 */

namespace DiskInfoToolkit.Models
{
    public sealed class DiskGeometryInfo
    {
        #region Properties

        public long Cylinders { get; set; }

        public uint MediaType { get; set; }

        public uint TracksPerCylinder { get; set; }

        public uint SectorsPerTrack { get; set; }

        public uint BytesPerSector { get; set; }

        public ulong DiskSize { get; set; }

        #endregion
    }
}
