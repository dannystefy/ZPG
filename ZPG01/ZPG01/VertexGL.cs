using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

namespace ZPG01
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct VertexGL
    {
        public Vector3 position;
        public Vector3 color;

        public static int Sizeof()
        {
            return Marshal.SizeOf(typeof(VertexGL));
        }
    }
}
