using System.ComponentModel;

namespace ZPG05
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
            Color = new ColorRGB(0, 0, 0);
        }

        public override string ToString()
        {
            return $"[{Position.X};{Position.Y};{Position.Z}] ({Color.R};{Color.G};{Color.B})";
        }
    }
}
