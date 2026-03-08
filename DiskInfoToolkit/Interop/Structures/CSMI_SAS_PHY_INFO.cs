using System.Runtime.InteropServices;

namespace DiskInfoToolkit.Interop.Structures
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct CSMI_SAS_PHY_INFO
    {
        public CSMI_SAS_PHY_INFO()
        {
            bReserved = new byte[3];
            Phy = new CSMI_SAS_PHY_ENTITY[32];
        }

        public byte bNumberOfPhys;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] bReserved;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public CSMI_SAS_PHY_ENTITY[] Phy;
    }
}
