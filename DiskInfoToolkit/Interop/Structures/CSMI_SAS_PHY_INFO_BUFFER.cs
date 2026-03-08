using System.Runtime.InteropServices;

namespace DiskInfoToolkit.Interop.Structures
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct CSMI_SAS_PHY_INFO_BUFFER
    {
        public CSMI_SAS_PHY_INFO_BUFFER()
        {
            IoctlHeader = new();
            Information = new();
        }

        public SRB_IO_CONTROL IoctlHeader;
        public CSMI_SAS_PHY_INFO Information;
    }
}
