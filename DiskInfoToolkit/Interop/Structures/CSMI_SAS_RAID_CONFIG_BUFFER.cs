using System.Runtime.InteropServices;

namespace DiskInfoToolkit.Interop.Structures
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    internal struct CSMI_SAS_RAID_CONFIG_BUFFER
    {
        public CSMI_SAS_RAID_CONFIG_BUFFER()
        {
            IoctlHeader = new();
            Configuration = new();
        }

        public SRB_IO_CONTROL IoctlHeader;
        public CSMI_SAS_RAID_CONFIG Configuration;
    }
}
