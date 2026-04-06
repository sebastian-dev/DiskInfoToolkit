/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 *
 * Copyright (c) 2026 Florian K.
 */

using DiskInfoToolkit.Utilities;
using System.Text;

namespace DiskInfoToolkit.Probes
{
    public static class AtaStringDecoder
    {
        #region Public

        public static string ReadWordSwappedString(byte[] identifyData, int wordOffset, int wordCount)
        {
            if (identifyData == null)
            {
                return string.Empty;
            }

            int offset = wordOffset * 2;
            int length = wordCount * 2;

            if (offset < 0 || offset + length > identifyData.Length)
            {
                return string.Empty;
            }

            var tmp = new byte[length];
            Buffer.BlockCopy(identifyData, offset, tmp, 0, length);

            for (int i = 0; i + 1 < tmp.Length; i += 2)
            {
                byte a = tmp[i];
                tmp[i] = tmp[i + 1];
                tmp[i + 1] = a;
            }

            return StringUtil.TrimStorageString(Encoding.ASCII.GetString(tmp));
        }

        #endregion
    }
}
