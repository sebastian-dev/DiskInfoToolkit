using System.Runtime.InteropServices;

namespace DiskInfoToolkit.Interop.Structures
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    internal unsafe struct CSMI_SAS_RAID_CONFIG
    {
        public CSMI_SAS_RAID_CONFIG()
        {
            Union = new();
        }

        public uint uRaidSetIndex;
        public uint uCapacity;
        public uint uStripeSize;
        public byte bRaidType;
        public byte bStatus;
        public byte bInformation;
        public byte bDriveCount;
        public byte bDataType;
        public fixed byte bReserved[11];
        public uint uFailureCode;
        public uint uChangeCount;

        public CSMI_SAS_RAID_CONFIG_UNION Union;
    }
}
