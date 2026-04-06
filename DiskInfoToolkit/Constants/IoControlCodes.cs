/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 *
 * Copyright (c) 2026 Florian K.
 */

namespace DiskInfoToolkit.Constants
{
    public static class IoControlCodes
    {
        #region Fields

        public const uint IOCTL_STORAGE_QUERY_PROPERTY = 0x002D1400;

        public const uint IOCTL_STORAGE_GET_DEVICE_NUMBER = 0x002D1080;

        public const uint IOCTL_STORAGE_PREDICT_FAILURE = 0x002D1100;

        public const uint IOCTL_DISK_GET_DRIVE_GEOMETRY_EX = 0x000700A0;

        public const uint IOCTL_DISK_GET_DRIVE_LAYOUT_EX = 0x00070050;

        public const uint IOCTL_VOLUME_GET_VOLUME_DISK_EXTENTS = 0x00560000;

        public const uint IOCTL_SCSI_GET_ADDRESS = 0x00041018;

        public const uint IOCTL_SCSI_PASS_THROUGH = 0x0004D004;

        public const uint IOCTL_SCSI_MINIPORT = 0x0004D008;

        public const uint IOCTL_ATA_PASS_THROUGH = 0x0004D02C;

        public const uint IOCTL_IDE_PASS_THROUGH = 0x0004D028;

        public const uint IOCTL_SMART_GET_VERSION = 0x00074080;

        public const uint IOCTL_SFFDISK_QUERY_DEVICE_PROTOCOL = 0x00071E80;

        public const uint IOCTL_INTEL_NVME_PASS_THROUGH = 0xF0002808;

        public const uint IOCTL_SCSI_MINIPORT_IDENTIFY = 0x001B0501;

        public const uint IOCTL_SCSI_MINIPORT_READ_SMART_ATTRIBS = 0x001B0502;

        public const uint IOCTL_SCSI_MINIPORT_READ_SMART_THRESHOLDS = 0x001B0503;

        public const uint IOCTL_SCSI_MINIPORT_ENABLE_SMART = 0x001B0504;

        public const uint IOCTL_SCSI_MINIPORT_DISABLE_SMART = 0x001B0505;

        public const uint DFP_SEND_DRIVE_COMMAND = 0x0007C084;

        public const uint DFP_RECEIVE_DRIVE_DATA = 0x0007C088;

        public const uint IOCTL_STORAGE_CHECK_VERIFY = 0x002D4800;

        public const uint IOCTL_STORAGE_CHECK_VERIFY2 = 0x002D0800;

        #endregion
    }
}
