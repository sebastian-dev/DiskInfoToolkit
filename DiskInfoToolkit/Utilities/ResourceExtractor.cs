/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 *
 * Copyright (c) 2026 Florian K.
 */

using System.IO.Compression;
using System.Reflection;

namespace DiskInfoToolkit.Utilities
{
    internal static class ResourceExtractor
    {
        #region Public

        public static GZipStream GetResourceFileGZipStream(string resourceName)
        {
            var assembly = typeof(ResourceExtractor).Assembly;

            try
            {
                var stream = assembly.GetManifestResourceStream(resourceName);
                if (stream != null)
                {
                    return new GZipStream(stream, CompressionMode.Decompress);
                }
            }
            catch (Exception)
            {
                return null;
            }

            return null;
        }

        #endregion
    }
}
