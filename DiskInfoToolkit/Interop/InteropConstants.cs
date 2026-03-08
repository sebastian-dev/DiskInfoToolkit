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

using System.Text;

namespace DiskInfoToolkit.Interop
{
    internal sealed class InteropConstants
    {
        static InteropConstants()
        {
            NVME_SIG_STR_ARR       = Encoding.ASCII.GetBytes(NVME_SIG_STR.ToCharArray());
            NVME_INTEL_SIG_STR_ARR = Encoding.ASCII.GetBytes(NVME_INTEL_SIG_STR.ToCharArray());
            NVME_RAID_SIG_STR_ARR  = Encoding.ASCII.GetBytes(NVME_RAID_SIG_STR.ToCharArray());
            SIL_SIG_STR_ARR        = Encoding.ASCII.GetBytes(SIL_SIG_STR.ToCharArray());
            SCSI_SIG_STR_ARR       = Encoding.ASCII.GetBytes(SCSI_SIG_STR.ToCharArray());
            RAID_SIG_STR_ARR       = Encoding.ASCII.GetBytes(RAID_SIG_STR.ToCharArray());
        }

        public const int ERROR_INVALID_FUNCTION = 1;
        public const int ERROR_NOT_SUPPORTED    = 50;
        public const int ERROR_DEV_NOT_EXIST    = 55;

        public const int MAX_SEARCH_SCSI_PORT = 16;

        public const int SCSI_IOCTL_DATA_OUT = 0;
        public const int SCSI_IOCTL_DATA_IN  = 1;

        public const int IDENTIFY_BUFFER_SIZE = 512;
        public const int READ_ATTRIBUTE_BUFFER_SIZE = 512;
        public const int READ_THRESHOLD_BUFFER_SIZE = 512;
        public const int SCSI_MINIPORT_BUFFER_SIZE = 512;

        public const int NVME_IOCTL_VENDOR_SPECIFIC_DW_SIZE = 6;
        public const int NVME_IOCTL_CMD_DW_SIZE             = 16;
        public const int NVME_IOCTL_COMPLETE_DW_SIZE        = 4;

        public const uint NVME_PASS_THROUGH_SRB_IO_CODE = 0xE0002000;

        public const string NVME_SIG_STR = "NvmeMini";
        public readonly static byte[] NVME_SIG_STR_ARR;
        public const uint NVME_SIG_STR_LEN = 8;

        public const string NVME_INTEL_SIG_STR = "IntelNvm";
        public readonly static byte[] NVME_INTEL_SIG_STR_ARR;
        public const uint NVME_INTEL_SIG_STR_LEN = 8;

        public const string NVME_RAID_SIG_STR = "NvmeRAID";
        public readonly static byte[] NVME_RAID_SIG_STR_ARR;
        public const uint NVME_RAID_SIG_STR_LEN = 8;

        public const string SIL_SIG_STR = "CMD_IDE";
        public readonly static byte[] SIL_SIG_STR_ARR;
        public const uint SIL_SIG_STR_LEN = 8;

        public const string SCSI_SIG_STR = "SCSIDISK";
        public readonly static byte[] SCSI_SIG_STR_ARR;
        public const uint SCSI_SIG_STR_LEN = 8;

        public const string RAID_SIG_STR = "LSILOGIC";
        public readonly static byte[] RAID_SIG_STR_ARR;
        public const uint RAID_SIG_STR_LEN = 8;

        public const uint NVME_PT_TIMEOUT = 40;
        public const uint NVME_FROM_DEV_TO_HOST = 2;

        public const byte ATAPI_ID_CMD = 0xA1; // Returns ID sector for ATAPI.
        public const byte ID_CMD       = 0xEC; // Returns ID sector for ATA.
        public const byte SMART_CMD    = 0xB0; // Performs SMART cmd.
                                               // Requires valid bFeaturesReg,
                                               // bCylLowReg, and bCylHighReg

        public const int ATA_FLAGS_DATA_IN  = 0x02;
        public const int ATA_FLAGS_DATA_OUT = 0x04;

        // Cylinder register defines for SMART command
        public const byte SMART_CYL_LOW = 0x4F;
        public const byte SMART_CYL_HI  = 0xC2;

        // Feature register defines for SMART "sub commands"
        public const byte READ_ATTRIBUTES = 0xD0;
        public const byte READ_THRESHOLDS = 0xD1;
        public const byte ENABLE_SMART    = 0xD8;
        public const byte DISABLE_SMART   = 0xD9;

        public const byte CSMI_SAS_LINK_RATE_NEGOTIATED = 0x00;
        public const byte CSMI_SAS_STP_UNSPECIFIED = 0x04;
        public const byte CSMI_SAS_STP_READ = 0x01;
        public const byte CSMI_SAS_STP_PIO = 0x10;
        public const byte CSMI_SAS_TIMEOUT = 60;

        public const byte CC_CSMI_SAS_GET_DRIVER_INFO = 1;
        public const byte CC_CSMI_SAS_GET_RAID_INFO = 10;
        public const byte CC_CSMI_SAS_GET_RAID_CONFIG = 11;
        public const byte CC_CSMI_SAS_GET_PHY_INFO = 20;
        public const byte CC_CSMI_SAS_STP_PASSTHRU = 25;

        public const string CSMI_ALL_SIGNATURE  = "CSMIALL";
        public const string CSMI_SAS_SIGNATURE  = "CSMISAS";
        public const string CSMI_RAID_SIGNATURE = "CSMIARY";

        public const byte MFI_CMD_PD_SCSI_IO = 0x04;
        public const byte MFI_FRAME_DIR_READ = 0x10;

        public static readonly IntPtr InvalidHandle = new IntPtr(-1);
    }
}
