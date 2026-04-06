/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 *
 * Copyright (c) 2026 Florian K.
 */

using DiskInfoToolkit.Constants;
using DiskInfoToolkit.Core;
using DiskInfoToolkit.Utilities;

namespace DiskInfoToolkit.Probes
{
    public static class UsbNvmeSetupModeDetector
    {
        #region Public

        public static bool IsUsbNvmeCandidate(StorageDevice device)
        {
            if (device == null)
            {
                return false;
            }

            var service = StringUtil.TrimStorageString(device.Controller.Service);
            if (service.Equals(ControllerServiceNames.SecNvme, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (!device.Controller.VendorID.HasValue)
            {
                return false;
            }

            switch (device.Controller.VendorID.Value)
            {
                case VendorIDConstants.Asmedia:
                case VendorIDConstants.Realtek:
                case VendorIDConstants.JMicron:
                case VendorIDConstants.Samsung:
                    return true;
                default:
                    return false;
            }
        }

        public static void Apply(StorageDevice device)
        {
            if (device == null)
            {
                return;
            }

            string service = StringUtil.TrimStorageString(device.Controller.Service);
            ushort vendorId = device.Controller.VendorID.GetValueOrDefault();

            if (service.Equals(ControllerServiceNames.SecNvme, StringComparison.OrdinalIgnoreCase))
            {
                device.Usb.NvmeSetupMode = UsbNvmeSetupModeNames.SamsungVendorScsi;
                if (string.IsNullOrWhiteSpace(device.Usb.BridgeFamily))
                {
                    device.Usb.BridgeFamily = UsbBridgeFamilyNames.Samsung;
                }
                return;
            }

            if (vendorId == VendorIDConstants.JMicron)
            {
                device.Usb.NvmeSetupMode = UsbNvmeSetupModeNames.JMicronPassThrough;
                return;
            }

            if (vendorId == VendorIDConstants.Realtek)
            {
                device.Usb.NvmeSetupMode = UsbNvmeSetupModeNames.RealtekPassThrough;
                return;
            }

            if (vendorId == VendorIDConstants.Asmedia)
            {
                device.Usb.NvmeSetupMode = UsbNvmeSetupModeNames.ASMediaPassThrough;
                return;
            }

            if (ControllerServiceProbeRules.IsUsbMassStorageService(service))
            {
                device.Usb.NvmeSetupMode = UsbNvmeSetupModeNames.StandardStorageQuery;
            }
        }

        #endregion
    }
}
