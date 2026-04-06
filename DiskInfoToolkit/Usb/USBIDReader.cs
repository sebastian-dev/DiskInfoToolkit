/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 *
 * Copyright (c) 2026 Florian K.
 */

using DiskInfoToolkit.Utilities;
using System.Globalization;
using System.Text.RegularExpressions;

namespace DiskInfoToolkit.Usb
{
    internal static class USBIDReader
    {
        #region Constructor

        static USBIDReader()
        {
            ReadUSBIDs();
        }

        #endregion

        #region Fields

        const string USBIDFileName = "usb.ids.gz";

        static Regex _regex = new Regex(@"^([0-9A-Fa-f]+|\d+)\s\s(.+)$");
        static List<UsbVendor> _vendors = new List<UsbVendor>();

        #endregion

        #region Properties

        public static IReadOnlyList<UsbVendor> Vendors => _vendors;

        #endregion

        #region Public

        public static bool TryGetVendorAndDeviceName(ushort vendorId, ushort productId, out string vendorName, out string deviceName)
        {
            vendorName = string.Empty;
            deviceName = string.Empty;

            foreach (var vendor in _vendors)
            {
                if (vendor.ID != vendorId)
                {
                    continue;
                }

                vendorName = vendor.Name ?? string.Empty;

                foreach (var device in vendor.Devices)
                {
                    if (device.ID == productId)
                    {
                        deviceName = device.Name ?? string.Empty;
                        break;
                    }
                }

                return !string.IsNullOrWhiteSpace(vendorName) || !string.IsNullOrWhiteSpace(deviceName);
            }

            return false;
        }

        #endregion

        #region Private

        static void ReadUSBIDs()
        {
            string resourceName = $"{nameof(DiskInfoToolkit)}.Resources.{USBIDFileName}";

            using (var stream = ResourceExtractor.GetResourceFileGZipStream(resourceName))
            {
                if (stream == null)
                {
                    return;
                }

                using (var reader = new StreamReader(stream))
                {
                    string rawLine;
                    UsbVendor vendor = null;
                    UsbDevice device = null;

                    while ((rawLine = reader.ReadLine()) != null)
                    {
                        if (rawLine.StartsWith("#", StringComparison.Ordinal) || rawLine.Length == 0)
                        {
                            continue;
                        }

                        var lineValues = GetLineValues(rawLine.Trim());
                        if (lineValues == null)
                        {
                            continue;
                        }

                        if (rawLine.StartsWith("\t\t", StringComparison.Ordinal))
                        {
                            if (device != null)
                            {
                                device.Interfaces.Add(new UsbInterface(lineValues.Item1, lineValues.Item2));
                            }
                        }
                        else if (rawLine.StartsWith("\t", StringComparison.Ordinal))
                        {
                            if (vendor != null)
                            {
                                device = new UsbDevice(lineValues.Item1, lineValues.Item2);
                                vendor.Devices.Add(device);
                            }
                        }
                        else
                        {
                            vendor = new UsbVendor(lineValues.Item1, lineValues.Item2);
                            _vendors.Add(vendor);
                        }
                    }
                }
            }
        }

        static Tuple<int, string> GetLineValues(string line)
        {
            var match = _regex.Match(line);
            if (match.Success)
            {
                int.TryParse(match.Groups[1].Value, NumberStyles.HexNumber, null, out int value);
                return Tuple.Create(value, match.Groups[2].Value);
            }

            return null;
        }

        #endregion
    }
}
