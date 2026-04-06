/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 *
 * Copyright (c) 2026 Florian K.
 */

using DiskInfoToolkit.Interop;
using System.Runtime.InteropServices;

namespace DiskInfoToolkit.Tests.Interop.Structures
{
    [TestClass]
    public class StructuresSizeTest
    {
        [TestMethod]
        public void TestStructureSizes()
        {
            TestStructureSize<ATA_PASS_THROUGH_EX>(48);
            TestStructureSize<ATA_PASS_THROUGH_EX_WITH_BUFFERS>(564);

            TestStructureSize<DEV_BROADCAST_DEVICEINTERFACE>(32);
            TestStructureSize<DEV_BROADCAST_HDR>(12);

            TestStructureSize<DISK_EXTENT_RAW>(24);
            TestStructureSize<DISK_GEOMETRY>(24);
            TestStructureSize<DISK_GEOMETRY_EX>(32);

            TestStructureSize<DRIVE_LAYOUT_INFORMATION_EX_RAW>(192);
            TestStructureSize<DRIVE_LAYOUT_INFORMATION_GPT_RAW>(40);
            TestStructureSize<DRIVE_LAYOUT_INFORMATION_MBR_RAW>(8);
            TestStructureSize<DRIVE_LAYOUT_INFORMATION_UNION_RAW>(40);

            TestStructureSize<DRIVERSTATUS>(12);
            TestStructureSize<GETVERSIONINPARAMS>(24);

            //HPT
            TestStructureSize<HPT_CONTROLLER_INFO>(76);
            TestStructureSize<HPT_CONTROLLER_INFO_V2>(88);
            TestStructureSize<HPT_CONTROLLER_INFO_V3>(256);
            TestStructureSize<HPT_DEVICE_INFO>(162);
            TestStructureSize<HPT_DEVICE_INFO_V2>(218);
            TestStructureSize<HPT_IDE_PASS_THROUGH_HEADER>(16);
            TestStructureSize<HPT_IDE_PASS_THROUGH_HEADER_V2>(20);
            TestStructureSize<HPT_IDENTIFY_DATA2>(150);
            TestStructureSize<HPT_LOGICAL_DEVICE_INFO>(174);
            //TestStructureSize<HPT_LOGICAL_DEVICE_INFO_V2>(216); //TODO: proper union
            //TestStructureSize<HPT_LOGICAL_DEVICE_INFO_V3>(250); //TODO: proper union
            //TestStructureSize<HPT_LOGICAL_DEVICE_INFO_V4>(452); //TODO: proper union
            TestStructureSize<HPT_SCSI_PASSTHROUGH_IN>(28);
            TestStructureSize<HPT_SCSI_PASSTHROUGH_OUT>(8);

            TestStructureSize<IDEREGS>(8);

            //Intel
            TestStructureSize<INTEL_NVME_COMMAND>(64);
            TestStructureSize<INTEL_NVME_PASS_THROUGH>(4260);
            TestStructureSize<INTEL_NVME_PAYLOAD>(136);

            //MegaRaid
            //TestStructureSize<MegaRaidPassThroughCommand>(TODO);
            //TestStructureSize<MegaRaidProcessLibCommand>(TODO);

            TestStructureSize<MSG>(40);

            TestStructureSize<NVME_PASS_THROUGH_IOCTL>(4248);
            //TestStructureSize<NVME_STORAGE_QUERY_WITH_BUFFER>(TODO);

            TestStructureSize<PARTITION_INFORMATION_EX_RAW>(144);
            TestStructureSize<PARTITION_INFORMATION_GPT_RAW>(112);
            TestStructureSize<PARTITION_INFORMATION_MBR_RAW>(24);
            TestStructureSize<PARTITION_INFORMATION_UNION_RAW>(112);

            TestStructureSize<POINT>(8);

            TestStructureSize<SCSI_ADDRESS>(8);
            TestStructureSize<SCSI_PASS_THROUGH>(56);
            //TestStructureSize<SCSI_PASS_THROUGH_WITH_BUFFERS>(4177);
            //TestStructureSize<SCSI_PASS_THROUGH_WITH_BUFFERS_EX>(4173);

            TestStructureSize<SENDCMDINPARAMS>(33);
            TestStructureSize<SENDCMDOUTPARAMS>(17);

            TestStructureSize<SFFDISK_QUERY_DEVICE_PROTOCOL_DATA>(20);

            TestStructureSize<SP_DEVICE_INTERFACE_DATA>(32);
            TestStructureSize<SP_DEVINFO_DATA>(32);

            TestStructureSize<SRB_IO_CONTROL>(28);

            TestStructureSize<STORAGE_ADAPTER_DESCRIPTOR>(32);
            TestStructureSize<STORAGE_DEVICE_DESCRIPTOR>(40);
            TestStructureSize<STORAGE_DEVICE_NUMBER>(12);
            TestStructureSize<STORAGE_PREDICT_FAILURE>(516);
            TestStructureSize<STORAGE_PROPERTY_QUERY>(12);
            //TestStructureSize<STORAGE_PROPERTY_QUERY_NVME>(TODO);
            TestStructureSize<STORAGE_PROTOCOL_SPECIFIC_DATA>(40);

            TestStructureSize<VOLUME_DISK_EXTENTS_RAW>(32);

            TestStructureSize<WNDCLASSEX>(80);
        }

        [TestMethod]
        public void TestStructureMemberOffsets()
        {
            TestStructureMemberOffset<DRIVE_LAYOUT_INFORMATION_EX_RAW>(nameof(DRIVE_LAYOUT_INFORMATION_EX_RAW.PartitionStyle),  0);
            TestStructureMemberOffset<DRIVE_LAYOUT_INFORMATION_EX_RAW>(nameof(DRIVE_LAYOUT_INFORMATION_EX_RAW.PartitionCount),  4);
            TestStructureMemberOffset<DRIVE_LAYOUT_INFORMATION_EX_RAW>(nameof(DRIVE_LAYOUT_INFORMATION_EX_RAW.Layout        ),  8);

            TestStructureMemberOffset<PARTITION_INFORMATION_EX_RAW>(nameof(PARTITION_INFORMATION_EX_RAW.PartitionStyle    ),  0);
            TestStructureMemberOffset<PARTITION_INFORMATION_EX_RAW>(nameof(PARTITION_INFORMATION_EX_RAW.StartingOffset    ),  8);
            TestStructureMemberOffset<PARTITION_INFORMATION_EX_RAW>(nameof(PARTITION_INFORMATION_EX_RAW.PartitionLength   ), 16);
            TestStructureMemberOffset<PARTITION_INFORMATION_EX_RAW>(nameof(PARTITION_INFORMATION_EX_RAW.PartitionNumber   ), 24);
            TestStructureMemberOffset<PARTITION_INFORMATION_EX_RAW>(nameof(PARTITION_INFORMATION_EX_RAW.RewritePartition  ), 28);
            TestStructureMemberOffset<PARTITION_INFORMATION_EX_RAW>(nameof(PARTITION_INFORMATION_EX_RAW.IsServicePartition), 29);
            TestStructureMemberOffset<PARTITION_INFORMATION_EX_RAW>(nameof(PARTITION_INFORMATION_EX_RAW.Layout            ), 32);
        }

        void TestStructureSize<TStructure>(int expectedSize)
            where TStructure : struct
        {
            Assert.AreEqual(expectedSize, Marshal.SizeOf<TStructure>(), $"Invalid size for {typeof(TStructure).Name}.");
        }

        void TestStructureMemberOffset<TStructure>(string member, int expectedOffset)
            where TStructure : struct
        {
            var offset = Marshal.OffsetOf<TStructure>(member);
            Assert.AreEqual(expectedOffset, offset.ToInt32(), $"Invalid offset for {typeof(TStructure).Name}.{member}.");
        }
    }
}
