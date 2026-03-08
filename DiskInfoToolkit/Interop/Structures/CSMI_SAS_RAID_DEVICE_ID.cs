using System.Runtime.InteropServices;

namespace DiskInfoToolkit.Interop.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct CSMI_SAS_RAID_DEVICE_ID
    {
        public CSMI_SAS_RAID_DEVICE_ID()
        {
            bDeviceIdentificationVPDPage = new byte[1];
        }

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] bDeviceIdentificationVPDPage;
    }
}
