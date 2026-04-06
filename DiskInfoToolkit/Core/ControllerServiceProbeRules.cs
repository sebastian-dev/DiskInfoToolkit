/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 *
 * Copyright (c) 2026 Florian K.
 */

using DiskInfoToolkit.Constants;
using DiskInfoToolkit.Utilities;

namespace DiskInfoToolkit.Core
{
    public static class ControllerServiceProbeRules
    {
        #region Public

        public static bool IsUsbMassStorageService(string controllerService)
        {
            if (string.IsNullOrWhiteSpace(controllerService))
            {
                return false;
            }

            string service = StringUtil.TrimStorageString(controllerService);
            return service.Equals(ControllerServiceNames.UaspStor, StringComparison.OrdinalIgnoreCase)
                || service.Equals(ControllerServiceNames.UsbStor, StringComparison.OrdinalIgnoreCase)
                || service.Equals(ControllerServiceNames.UsbStorWithTrailingSpace, StringComparison.OrdinalIgnoreCase)
                || service.Equals(ControllerServiceNames.AsusStpt, StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsAtaLikeScsiController(StorageDevice device)
        {
            if (device == null)
            {
                return false;
            }

            string service = StringUtil.TrimStorageString(device.Controller.Service);
            string controllerClass = StringUtil.TrimStorageString(device.Controller.Class);
            string deviceTypeLabel = StringUtil.TrimStorageString(device.DeviceTypeLabel);

            if (controllerClass.Equals(ControllerClassNames.Usb, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (service.Equals(ControllerServiceNames.AmdSata, StringComparison.OrdinalIgnoreCase)
                || service.Equals(ControllerServiceNames.AmdSataAlt, StringComparison.OrdinalIgnoreCase)
                || service.Equals(ControllerServiceNames.AsusStpt, StringComparison.OrdinalIgnoreCase)
                || service.Equals(ControllerServiceNames.Storahci, StringComparison.OrdinalIgnoreCase)
                || service.Equals(ControllerServiceNames.UaspStor, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if ((service.Equals(ControllerServiceNames.LsiSas, StringComparison.OrdinalIgnoreCase)
                    || service.Equals(ControllerServiceNames.LsiSas2, StringComparison.OrdinalIgnoreCase)
                    || service.Equals(ControllerServiceNames.LsiSas2i, StringComparison.OrdinalIgnoreCase)
                    || service.Equals(ControllerServiceNames.LsiSas3, StringComparison.OrdinalIgnoreCase)
                    || service.Equals(ControllerServiceNames.LsiSas3i, StringComparison.OrdinalIgnoreCase))
                && deviceTypeLabel.StartsWith("ATA ", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if ((service.Equals(ControllerServiceNames.ItSas35, StringComparison.OrdinalIgnoreCase)
                    || service.Equals(ControllerServiceNames.ItSas35i, StringComparison.OrdinalIgnoreCase))
                && controllerClass.Equals(ControllerClassNames.Sas, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }

        public static bool IsScsiRaidController(StorageDevice device)
        {
            if (device == null)
            {
                return false;
            }

            string service = StringUtil.TrimStorageString(device.Controller.Service);
            string controllerClass = StringUtil.TrimStorageString(device.Controller.Class);

            if (service.Equals(ControllerServiceNames.MegaSas, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (service.Equals(ControllerServiceNames.LsiSas, StringComparison.OrdinalIgnoreCase)
                || service.Equals(ControllerServiceNames.LsiSas2, StringComparison.OrdinalIgnoreCase)
                || service.Equals(ControllerServiceNames.LsiSas2i, StringComparison.OrdinalIgnoreCase)
                || service.Equals(ControllerServiceNames.LsiSas3, StringComparison.OrdinalIgnoreCase)
                || service.Equals(ControllerServiceNames.LsiSas3i, StringComparison.OrdinalIgnoreCase)
                || service.Equals(ControllerServiceNames.ItSas35, StringComparison.OrdinalIgnoreCase)
                || service.Equals(ControllerServiceNames.ItSas35i, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (controllerClass.Equals(ControllerClassNames.Sas, StringComparison.OrdinalIgnoreCase)
                || controllerClass.Equals(ControllerClassNames.Raid, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }

        public static bool ShouldFilterNoSmartSupport(StorageDevice device)
        {
            if (device == null)
            {
                return false;
            }

            string display = device.DisplayName ?? string.Empty;
            return display.StartsWith(StorageDetectionFilter.Drobo5D, StringComparison.OrdinalIgnoreCase)
                || display.StartsWith(StorageDetectionFilter.VirtualDisk, StringComparison.OrdinalIgnoreCase);
        }

        #endregion
    }
}
