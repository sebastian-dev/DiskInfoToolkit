using System.Runtime.InteropServices;

namespace DiskInfoToolkit.Interop.Structures
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct CSMI_SAS_PHY_ENTITY
    {
        public CSMI_SAS_PHY_ENTITY()
        {
            Identify = new();
            Attached = new();
        }

        public CSMI_SAS_IDENTIFY Identify;
		public byte bPortIdentifier;
		public byte bNegotiatedLinkRate;
		public byte bMinimumLinkRate;
		public byte bMaximumLinkRate;
		public byte bPhyChangeCount;
		public byte bAutoDiscover;
		public byte bPhyFeatures;
		public byte bReserved;
		public CSMI_SAS_IDENTIFY Attached;
    }
}
