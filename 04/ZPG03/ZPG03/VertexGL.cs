using OpenTK.Mathematics;
using System.Runtime.InteropServices;

namespace ZPG01
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct VertexGL
    {
        public OpenTK.Mathematics.Vector3 position;
        public OpenTK.Mathematics.Vector3 color;

        public static int SizeOf()
        {
            return Marshal.SizeOf(typeof(VertexGL));
        }
    }
}
