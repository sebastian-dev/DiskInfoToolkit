/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 *
 * Copyright (c) 2026 Florian K.
 */

using System.Text;

namespace DiskInfoToolkit.Probes
{
    public static class IntelRaidMiniportConstants
    {
        #region Public

        public static byte[] CreateNvmeMiniSignature()
        {
            return CreateEightByteSignature("NvmeMini");
        }

        public static byte[] CreateIntelMiniSignature()
        {
            return CreateEightByteSignature("IntelNvm");
        }

        public static byte[] CreateNvmeRaidSignature()
        {
            return CreateEightByteSignature("NvmeRAID");
        }

        #endregion

        #region Private

        private static byte[] CreateEightByteSignature(string text)
        {
            byte[] signature = new byte[8];
            byte[] source = Encoding.ASCII.GetBytes(text ?? string.Empty);
            Array.Copy(source, signature, Math.Min(source.Length, signature.Length));
            return signature;
        }

        #endregion
    }
}
