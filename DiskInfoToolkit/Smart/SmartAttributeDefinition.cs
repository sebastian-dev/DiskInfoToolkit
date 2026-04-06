/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 *
 * Copyright (c) 2026 Florian K.
 */

namespace DiskInfoToolkit.Smart
{
    public sealed class SmartAttributeDefinition
    {
        #region Constructor

        public SmartAttributeDefinition(byte id, string attributeKey)
        {
            Id = id;
            AttributeKey = attributeKey ?? string.Empty;
        }

        #endregion

        #region Properties

        public byte Id { get; }

        public string AttributeKey { get; }

        public string TextKey
        {
            get
            {
                return SmartTextKeys.GetAttributeNameKey(AttributeKey);
            }
        }

        #endregion
    }
}
