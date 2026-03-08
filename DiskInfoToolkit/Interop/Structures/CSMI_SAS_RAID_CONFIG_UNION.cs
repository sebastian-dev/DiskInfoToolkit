using System.Runtime.InteropServices;

namespace DiskInfoToolkit.Interop.Structures
{
    [StructLayout(LayoutKind.Explicit)]
    internal struct CSMI_SAS_RAID_CONFIG_UNION
    {
        public CSMI_SAS_RAID_CONFIG_UNION()
        {
            //TODO: nothing here will work; temporary only
            Drives = new CSMI_SAS_RAID_DRIVES[1];
            DeviceId = new CSMI_SAS_RAID_DEVICE_ID[1];
            Data = new CSMI_SAS_RAID_SET_ADDITIONAL_DATA[1];
        }

        [FieldOffset(0)] public CSMI_SAS_RAID_DRIVES[] Drives;
        [FieldOffset(0)] public CSMI_SAS_RAID_DEVICE_ID[] DeviceId;
        [FieldOffset(0)] public CSMI_SAS_RAID_SET_ADDITIONAL_DATA[] Data;
    }
}
