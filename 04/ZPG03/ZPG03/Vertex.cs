using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZPG03;

namespace ZPG01
{
    public class Vertex
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public Vector3 Position { get; set; }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public ColorRGB Color { get; set; }

        public Vertex(Vector3 position)
        {
            Position = position;
            Color = new ColorRGB(1, 1, 1);
        }

        public Vertex(Vector3 position, ColorRGB color)
        {
            Position = position;
            Color = color;
        }

        public override string ToString()
        {
            return $"{Position.X:F2} {Position.Y:F2} {Position.Z:F2} ({Color.R:F2} {Color.G:F2} {Color.B:F2})";
        }
    }
}
