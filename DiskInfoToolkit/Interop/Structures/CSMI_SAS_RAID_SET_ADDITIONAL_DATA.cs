using System.Runtime.InteropServices;

namespace DiskInfoToolkit.Interop.Structures
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    internal unsafe struct CSMI_SAS_RAID_SET_ADDITIONAL_DATA
    {
        public fixed byte  bLabel[16];
        public fixed byte  bRaidSetLun[8];
        public byte  bWriteProtection;
        public byte  bCacheSetting;
        public byte  bCacheRatio;
        public ushort usBlockSize;
        public fixed byte  bReservedBytes[11];
        public LargeUInteger RaidSetExtentOffset;
        public LargeUInteger RaidSetBlocks;
        public uint uStripeSizeInBlocks;
        public uint uSectorsPerTrack;
        public fixed byte  bApplicationScratchPad[16];
        public uint uNumberOfHeads;
        public uint uNumberOfTracks;
        public fixed byte  bReserved[24];
    }
}
