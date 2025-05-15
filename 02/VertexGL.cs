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
    /// Struktura pro vrchol skládající se z pozice a barvy. Využívá třídu OpenTK.Mathematics.Vector3
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 64)]
    public struct VertexGL
    {
        public Vector3 position;
        public Vector3 color;

        public VertexGL(Vector2 position, ColorRGB color)
        {
            this.position = new Vector3((float)position.X, (float)position.Y, 0);
            this.color = new Vector3((float)color.R, (float)color.G, (float)color.B);
        }

        /// <summary>
        /// Vrátí velikost struktury v bytech
        /// </summary>
        /// <returns></returns>
        public static int SizeOf()
        {
            return Marshal.SizeOf(typeof(VertexGL));
        }
    }
}
