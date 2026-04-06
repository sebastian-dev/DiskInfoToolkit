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

namespace DiskInfoToolkit.PCI
{
    internal static class PCIIDReader
    {
        #region Constructor

        static PCIIDReader()
        {
            ReadPCIIDs();
        }

        #endregion

        #region Fields

        const string PCIIDFileName = "pci.ids.gz";

        static Regex _regexNormal = new Regex(@"^([0-9A-Fa-f]+|\d+)\s\s(.+)$");
        static Regex _regexSub = new Regex(@"^([0-9A-Fa-f]+|\d+)\s([0-9A-Fa-f]+|\d+)\s\s(.+)$");
        static List<PCIVendor> _vendors = new List<PCIVendor>();

        #endregion

        #region Properties

        public static IReadOnlyList<PCIVendor> Vendors => _vendors;

        #endregion

        #region Public

        public static bool TryGetVendorAndDeviceName(ushort vendorId, ushort deviceId, out string vendorName, out string deviceName)
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
                    if (device.ID == deviceId)
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

        static void ReadPCIIDs()
        {
            string resourceName = $"{nameof(DiskInfoToolkit)}.Resources.{PCIIDFileName}";

            using (var stream = ResourceExtractor.GetResourceFileGZipStream(resourceName))
            {
                if (stream == null)
                {
                    return;
                }

                using (var reader = new StreamReader(stream))
                {
                    string rawLine;
                    PCIVendor vendor = null;
                    PCIDevice device = null;

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
                                device.SubDevices.Add(new PCISubDevice(lineValues.Item1, lineValues.Item2, lineValues.Item3));
                            }
                        }
                        else if (rawLine.StartsWith("\t", StringComparison.Ordinal))
                        {
                            if (vendor != null)
                            {
                                device = new PCIDevice(lineValues.Item1, lineValues.Item3);
                                vendor.Devices.Add(device);
                            }
                        }
                        else
                        {
                            vendor = new PCIVendor(lineValues.Item1, lineValues.Item3);
                            _vendors.Add(vendor);
                        }
                    }
                }
            }
        }

        static Tuple<int, int, string> GetLineValues(string line)
        {
            var match = _regexNormal.Match(line);
            if (match.Success)
            {
                int.TryParse(match.Groups[1].Value, NumberStyles.HexNumber, null, out int vendor);
                return Tuple.Create(vendor, -1, match.Groups[2].Value);
            }

            match = _regexSub.Match(line);
            if (match.Success)
            {
                int.TryParse(match.Groups[1].Value, NumberStyles.HexNumber, null, out int vendor);
                int.TryParse(match.Groups[2].Value, NumberStyles.HexNumber, null, out int device);
                return Tuple.Create(vendor, device, match.Groups[3].Value);
            }

            return null;
        }

        #endregion
    }
}
