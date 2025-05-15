using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ZPG01
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TriangleGL
    {
        public int i1, i2, i3;

        public static int Sizeof()
        {
            return Marshal.SizeOf(typeof(TriangleGL));
        }
    }
}
