/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 *
 * Copyright (c) 2026 Florian K.
 */

using System.Globalization;

namespace DiskInfoToolkit.Pnp
{
    public static class VendorIDParser
    {
        #region Public

        public static bool TryParse(string hardwareId, out ushort? vendorId, out ushort? deviceId, out ushort? revision, out bool isUsbStyle)
        {
            vendorId = null;
            deviceId = null;
            revision = null;
            isUsbStyle = false;

            if (string.IsNullOrWhiteSpace(hardwareId))
            {
                return false;
            }

            if (hardwareId.StartsWith("PCI\\VEN_", StringComparison.OrdinalIgnoreCase)
                || hardwareId.StartsWith("SD\\VID_", StringComparison.OrdinalIgnoreCase))
            {
                return TryParsePattern(hardwareId, out vendorId, out deviceId, out revision);
            }

            if (hardwareId.StartsWith("USB\\VID_", StringComparison.OrdinalIgnoreCase)
                || hardwareId.StartsWith("USB_TI\\VID_", StringComparison.OrdinalIgnoreCase))
            {
                isUsbStyle = true;
                return TryParsePattern(hardwareId, out vendorId, out deviceId, out revision);
            }

            return false;
        }

        #endregion

        #region Private

        private static bool TryParsePattern(string text, out ushort? first, out ushort? second, out ushort? third)
        {
            first = null;
            second = null;
            third = null;

            int firstUnderscore = text.IndexOf('_');
            if (firstUnderscore < 0 || !TryParseHex16(text, firstUnderscore + 1, out first))
            {
                return false;
            }

            int secondUnderscore = text.IndexOf('_', firstUnderscore + 1);
            if (secondUnderscore < 0 || !TryParseHex16(text, secondUnderscore + 1, out second))
            {
                return false;
            }

            int thirdUnderscore = text.IndexOf('_', secondUnderscore + 1);
            if (thirdUnderscore >= 0)
            {
                if (TryParseHex16(text, thirdUnderscore + 1, out var parsed))
                {
                    third = parsed;
                }
            }

            int revIndex = text.IndexOf("&REV_", StringComparison.OrdinalIgnoreCase);
            if (revIndex >= 0)
            {
                if (TryParseHex8(text, revIndex + 5, out var rev))
                {
                    third = rev;
                }
            }

            return true;
        }

        private static bool TryParseHex16(string text, int startIndex, out ushort? value)
        {
            value = null;
            if (startIndex < 0 || startIndex + 4 > text.Length)
            {
                return false;
            }

            if (ushort.TryParse(text.Substring(startIndex, 4), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var parsed))
            {
                value = parsed;
                return true;
            }

            return false;
        }

        private static bool TryParseHex8(string text, int startIndex, out ushort? value)
        {
            value = null;
            if (startIndex < 0 || startIndex + 2 > text.Length)
            {
                return false;
            }

            if (byte.TryParse(text.Substring(startIndex, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var parsed))
            {
                value = parsed;
                return true;
            }

            return false;
        }

        #endregion
    }
}
