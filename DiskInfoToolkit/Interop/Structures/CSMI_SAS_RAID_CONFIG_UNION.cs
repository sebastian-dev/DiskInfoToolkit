using System.Runtime.InteropServices;

namespace DiskInfoToolkit.Interop.Structures
{
    [StructLayout(LayoutKind.Explicit, Pack = 4, Size = 136)]
    internal struct CSMI_SAS_RAID_CONFIG_UNION
    {
        public CSMI_SAS_RAID_CONFIG_UNION()
        {
            Drives = new();
            DeviceId = new();
            Data = new();
        }

        [FieldOffset(0)] public CSMI_SAS_RAID_DRIVES Drives;
        [FieldOffset(0)] public CSMI_SAS_RAID_DEVICE_ID DeviceId;
        [FieldOffset(0)] public CSMI_SAS_RAID_SET_ADDITIONAL_DATA Data;
    }
}
