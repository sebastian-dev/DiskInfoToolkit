using System.Runtime.InteropServices;

namespace DiskInfoToolkit.Interop.Structures
{
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    internal unsafe struct CSMI_SAS_RAID_DRIVES
    {
        public fixed byte bModel[40];
        public fixed byte bFirmware[8];
        public fixed byte bSerialNumber[40];
        public fixed byte bSASAddress[8];
        public fixed byte bSASLun[8];
        public byte bDriveStatus;
        public byte bDriveUsage;
        public ushort usBlockSize;
        public byte bDriveType;
        public fixed byte bReserved[15];
        public uint uDriveIndex;

        public uint TotalUserBlocksLowPart;
        public uint TotalUserBlocksHighPart;
    }
}
