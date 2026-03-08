using System.Runtime.InteropServices;

namespace DiskInfoToolkit.Interop.Structures
{
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
    internal struct CSMI_SAS_RAID_INFO
    {
        public CSMI_SAS_RAID_INFO()
        {
            bReservedByteFields = new byte[7];
			bReserved = new byte[44];
        }

        public uint uNumRaidSets;
		public uint uMaxDrivesPerSet;
		public uint uMaxRaidSets;
		public byte bMaxRaidTypes;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
		public byte[] bReservedByteFields;
		public LargeUInteger MinRaidSetBlocks;
		public LargeUInteger MaxRaidSetBlocks;
		public uint uMaxPhysicalDrives;
		public uint uMaxExtents;
		public uint uMaxModules;
		public uint uMaxTransformationMemory;
		public uint uChangeCount;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 44)]
		public byte[] bReserved;
    }
}
