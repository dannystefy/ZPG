using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZPG01
{
    public class ObjSaver
    {
        public void Save(string filePath, Model model)
        {
            using (TextWriter tw = new StreamWriter(filePath))
            {
                for (var i = 0; i < model.Vertices.Count; i++)
                {
                    var v = model.Vertices[i];
                    var r = (int)(v.Color.R * 255);
                    var g = (int)(v.Color.G * 255);
                    var b = (int)(v.Color.B * 255);
                    tw.WriteLine($"v {v.Position.X} {v.Position.Y} 0 {r} {g} {b}");
                }
            }
        }
    }
}
