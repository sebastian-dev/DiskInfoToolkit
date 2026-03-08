using System.Runtime.InteropServices;

namespace DiskInfoToolkit.Interop.Structures
{
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    internal struct CSMI_SAS_DRIVER_INFO_BUFFER
    {
        public CSMI_SAS_DRIVER_INFO_BUFFER()
        {
            IoctlHeader = new();
            Information = new();
        }

        public SRB_IO_CONTROL IoctlHeader;
        public CSMI_SAS_DRIVER_INFO Information;
    }
}
