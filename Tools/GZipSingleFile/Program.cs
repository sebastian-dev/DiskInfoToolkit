/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 *
 * Copyright (c) 2026 Florian K.
 */

namespace GZipSingleFile;

static class Program
{
    static void Main(string[] args)
    {
        if (args.Length < 1)
        {
            Console.WriteLine("Argument must be one file to be gzipped, optionally also output file.");
            return;
        }

        var file = args[0];

        string outputFile = null;

        if (args.Length >= 2)
        {
            outputFile = args[1];
        }

        if (!File.Exists(file))
        {
            Console.WriteLine($"Provided file does not exist '{file}'");
            return;
        }

        if (outputFile != null)
        {
            var outputDirectory = Path.GetDirectoryName(outputFile);

            if (outputDirectory != null
             && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }
        }

        Console.WriteLine($"Zipping file '{file}'");

        var destination = outputFile ?? file + ".gz";

        GZipper.CompressFile(file, destination, false);
    }
}
