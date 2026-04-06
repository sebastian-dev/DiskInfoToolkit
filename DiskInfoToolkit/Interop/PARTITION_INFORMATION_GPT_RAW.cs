/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 *
 * Copyright (c) 2026 Florian K.
 */

using DiskInfoToolkit.Utilities;
using System.Text;

namespace DiskInfoToolkit.Interop
{
    public unsafe struct PARTITION_INFORMATION_GPT_RAW
    {
        #region Fields

        public Guid PartitionType;

        public Guid PartitionID;

        public ulong Attributes;

        public fixed byte Name[72];

        #endregion

        #region Properties

        public string NameStr
        {
            get
            {
                fixed (byte* ptr = Name)
                {
                    return StringUtil.TrimStorageString(Encoding.Unicode.GetString(ptr, 72));
                }
            }
        }

        #endregion
    }
}
