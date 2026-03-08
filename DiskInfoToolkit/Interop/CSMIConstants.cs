/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 *
 * Copyright (c) 2025 Florian K.
 *
 * Code inspiration, improvements and fixes are from, but not limited to, following projects:
 * CrystalDiskInfo
 */

namespace DiskInfoToolkit.Interop
{
    internal static class CSMIConstants
    {
        public const string AMDRaidXpertDriverName = "rcraid";

        /* * * * * * * * * * Class Independent IOCTL Constants * * * * * * * * * */

        // Return codes for all IOCTL's regardless of class
        // (IoctlHeader.ReturnCode)
        public const int CSMI_SAS_STATUS_SUCCESS              = 0;
        public const int CSMI_SAS_STATUS_FAILED               = 1;
        public const int CSMI_SAS_STATUS_BAD_CNTL_CODE        = 2;
        public const int CSMI_SAS_STATUS_INVALID_PARAMETER    = 3;
        public const int CSMI_SAS_STATUS_WRITE_ATTEMPTED      = 4;

        // Signature value
        // (IoctlHeader.Signature)

        public const string CSMI_ALL_SIGNATURE = "CSMIALL";

        // IOCTL Control Codes
        // (IoctlHeader.ControlCode)

        // Control Codes requiring CSMI_ALL_SIGNATURE

        public const int CC_CSMI_SAS_GET_DRIVER_INFO    = 1;
        public const int CC_CSMI_SAS_GET_CNTLR_CONFIG   = 2;
        public const int CC_CSMI_SAS_GET_CNTLR_STATUS   = 3;
        public const int CC_CSMI_SAS_FIRMWARE_DOWNLOAD  = 4;

        // Control Codes requiring CSMI_RAID_SIGNATURE

        public const int CC_CSMI_SAS_GET_RAID_INFO      = 10;
        public const int CC_CSMI_SAS_GET_RAID_CONFIG    = 11;
        public const int CC_CSMI_SAS_GET_RAID_FEATURES  = 12;
        public const int CC_CSMI_SAS_SET_RAID_CONTROL   = 13;
        public const int CC_CSMI_SAS_GET_RAID_ELEMENT   = 14;
        public const int CC_CSMI_SAS_SET_RAID_OPERATION = 15;

        // Control Codes requiring CSMI_SAS_SIGNATURE

        public const int CC_CSMI_SAS_GET_PHY_INFO       = 20;
        public const int CC_CSMI_SAS_SET_PHY_INFO       = 21;
        public const int CC_CSMI_SAS_GET_LINK_ERRORS    = 22;
        public const int CC_CSMI_SAS_SMP_PASSTHRU       = 23;
        public const int CC_CSMI_SAS_SSP_PASSTHRU       = 24;
        public const int CC_CSMI_SAS_STP_PASSTHRU       = 25;
        public const int CC_CSMI_SAS_GET_SATA_SIGNATURE = 26;
        public const int CC_CSMI_SAS_GET_SCSI_ADDRESS   = 27;
        public const int CC_CSMI_SAS_GET_DEVICE_ADDRESS = 28;
        public const int CC_CSMI_SAS_TASK_MANAGEMENT    = 29;
        public const int CC_CSMI_SAS_GET_CONNECTOR_INFO = 30;
        public const int CC_CSMI_SAS_GET_LOCATION       = 31;

        // Signature value
        // (IoctlHeader.Signature)

        public const string CSMI_SAS_SIGNATURE = "CSMISAS";

        // Timeout value default of 60 seconds
        // (IoctlHeader.Timeout)

        public const int CSMI_SAS_TIMEOUT       = 60;

        // Signature value
        // (IoctlHeader.Signature)

        public const string CSMI_RAID_SIGNATURE = "CSMIARY";
    }
}
