using System.Runtime.InteropServices;

namespace DiskInfoToolkit.Interop.Structures
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    internal struct CSMI_SAS_RAID_SET_ADDITIONAL_DATA
    {
        public CSMI_SAS_RAID_SET_ADDITIONAL_DATA()
        {
            bLabel = new byte[16];
            bRaidSetLun = new byte[8];
            bReservedBytes = new byte[11];
            bApplicationScratchPad = new byte[16];
            bReserved = new byte[24];
        }

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[]  bLabel;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[]  bRaidSetLun;
        public byte  bWriteProtection;
        public byte  bCacheSetting;
        public byte  bCacheRatio;
        public ushort usBlockSize;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)]
        public byte[]  bReservedBytes;
        public LargeUInteger RaidSetExtentOffset;
        public LargeUInteger RaidSetBlocks;
        public uint uStripeSizeInBlocks;
        public uint uSectorsPerTrack;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[]  bApplicationScratchPad;
        public uint uNumberOfHeads;
        public uint uNumberOfTracks;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
        public byte[]  bReserved;
    }
}
