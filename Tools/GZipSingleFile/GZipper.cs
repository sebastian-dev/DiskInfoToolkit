/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 *
 * Copyright (c) 2026 Florian K.
 */

using System.IO.Compression;

namespace GZipSingleFile
{
    internal class GZipper
    {
        public static void CompressFile(string source, string destination, bool trashByte)
        {
            using FileStream sourceFile = new(source, FileMode.Open, FileAccess.Read);
            using FileStream targetFile = new(destination, FileMode.OpenOrCreate);

            using GZipStream gzipStream = new(targetFile, CompressionMode.Compress);

            var buffer = new byte[1024];
            int read;

            if (trashByte)
            {
                gzipStream.Write([7]);
            }

            while ((read = sourceFile.Read(buffer, 0, buffer.Length)) > 0)
            {
                gzipStream.Write(buffer, 0, read);
            }
        }
    }
}
