using System.Runtime.InteropServices;

namespace DiskInfoToolkit.Interop.Structures
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 2)]
    internal struct CSMI_SAS_DRIVER_INFO
    {
        public CSMI_SAS_DRIVER_INFO()
        {
            szDescription = new byte[81];
        }

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
        public string szName;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)]
        public byte[] szDescription;
        public ushort usMajorRevision;
        public ushort usMinorRevision;
        public ushort usBuildRevision;
        public ushort usReleaseRevision;
        public ushort usCSMIMajorRevision;
        public ushort usCSMIMinorRevision;
    }
}
    