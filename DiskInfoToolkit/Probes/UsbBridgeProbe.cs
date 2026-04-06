/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 *
 * Copyright (c) 2026 Florian K.
 */

using DiskInfoToolkit.Core;

namespace DiskInfoToolkit.Probes
{
    public static class UsbBridgeProbe
    {
        #region Public

        public static bool TryPopulateData(StorageDevice device, IStorageIoControl ioControl)
        {
            if (device == null || ioControl == null)
            {
                return false;
            }

            UsbBridgeClassifier.Apply(device);
            UsbNvmeSetupModeDetector.Apply(device);

            if (!string.IsNullOrWhiteSpace(device.Usb.BridgeFamily))
            {
                ProbeTraceRecorder.Add(device, "USB path: bridge family classified as " + device.Usb.BridgeFamily + ".");
            }

            if (!string.IsNullOrWhiteSpace(device.Usb.NvmeSetupMode))
            {
                ProbeTraceRecorder.Add(device, "USB path: NVMe setup mode classified as " + device.Usb.NvmeSetupMode + ".");
            }

            if (ControllerServiceProbeRules.ShouldFilterNoSmartSupport(device))
            {
                if (string.IsNullOrWhiteSpace(device.FilterReason))
                {
                    device.FilterReason = "No SMART support on this USB storage path.";
                }

                ProbeTraceRecorder.Add(device, "USB path: filtered because the device matches a known no-SMART USB profile.");
                return true;
            }

            if (UsbNvmeSetupModeDetector.IsUsbNvmeCandidate(device))
            {
                if (UsbNvmeBridgeProbe.TryPopulateData(device, ioControl))
                {
                    UsbBridgeClassifier.ApplyInquiryHeuristics(device);
                    return true;
                }
            }

            if (device.Usb.IsMassStorageLike || UsbBridgeClassifier.IsUsbSatCapableBridge(device))
            {
                bool any = UsbMassStorageProbe.TryPopulateData(device, ioControl);

                if (!device.SupportsSmart && UsbVendorScsiSmartProbe.TryPopulateData(device, ioControl))
                {
                    any = true;
                }

                if (ScsiInquiryProbe.TryPopulateData(device, ioControl))
                {
                    any = true;
                }

                if (ScsiCapacityProbe.TryPopulateData(device, ioControl))
                {
                    any = true;
                }

                UsbBridgeClassifier.ApplyInquiryHeuristics(device);

                return any;
            }

            bool vendorSmart = UsbVendorScsiSmartProbe.TryPopulateData(device, ioControl);
            bool inquiry     = ScsiInquiryProbe       .TryPopulateData(device, ioControl);
            bool capacity    = ScsiCapacityProbe      .TryPopulateData(device, ioControl);

            UsbBridgeClassifier.ApplyInquiryHeuristics(device);

            return vendorSmart || inquiry || capacity;
        }

        #endregion
    }
}
