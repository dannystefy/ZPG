using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ZPG
{
    /// <summary>
    /// Struktura pro indexy trojúhelníků
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 64)]
    public struct TriangleGL
    {
        public int i1, i2, i3;

        public TriangleGL(Triangle t)
        {
            i1 = t.I1;
            i2 = t.I2;
            i3 = t.I3;
        }

        /// <summary>
        /// Vrátí velikost struktury v bytech
        /// </summary>
        /// <returns></returns>
        public static int SizeOf()
        {
            return Marshal.SizeOf(typeof(TriangleGL));
        }
    }
}
