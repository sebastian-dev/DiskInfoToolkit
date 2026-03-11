using System.Runtime.InteropServices;

namespace DiskInfoToolkit.Interop.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct CSMI_SAS_RAID_DEVICE_ID
    {
        public fixed byte bDeviceIdentificationVPDPage[1];
    }
}
