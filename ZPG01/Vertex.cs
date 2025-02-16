using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZPG01
{
    public class Vertex
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public Vector2 Position { get; set; }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public ColorRGB Color { get; set; }

        public Vertex(Vector2 position)
        {
            Position = position;
            Color = new ColorRGB(1, 1, 1);
        }

        public Vertex(Vector2 position, ColorRGB color)
        {
            Position = position;
            Color = color;
        }

        public override string ToString()
        {
            return $"{Position.X} {Position.Y} ({Color.R} {Color.G} {Color.B})";
        }
    }
}
