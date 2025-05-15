using System.Runtime.InteropServices;

namespace ZPG05
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TriangleGL
    {
        public int i1, i2, i3;

        public static int SizeOf()
        {
            return Marshal.SizeOf(typeof(TriangleGL));
        }
    }
}
