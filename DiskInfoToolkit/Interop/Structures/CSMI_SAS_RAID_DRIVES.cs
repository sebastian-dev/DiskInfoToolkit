using System.Runtime.InteropServices;

namespace DiskInfoToolkit.Interop.Structures
{
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    internal struct CSMI_SAS_RAID_DRIVES
    {
        public CSMI_SAS_RAID_DRIVES()
        {
            bModel = new byte[40];
            bFirmware = new byte[8];
            bSerialNumber = new byte[40];
            bSASAddress = new byte[8];
            bSASLun = new byte[8];
            bReserved = new byte[15];
        }

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
        public byte[] bModel;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] bFirmware;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
        public byte[] bSerialNumber;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] bSASAddress;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] bSASLun;
        public byte bDriveStatus;
        public byte bDriveUsage;
        public ushort usBlockSize;
        public byte bDriveType;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
        public byte[] bReserved;
        public uint uDriveIndex;

        public uint TotalUserBlocksLowPart;
        public uint TotalUserBlocksHighPart;
    }
}
