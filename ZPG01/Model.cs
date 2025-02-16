using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ZPG01
{
    public class Model
    {
        public BindingList<Vertex> Vertices { get; set; }

        public Model()
        {
            Vertices = new BindingList<Vertex>();
        }
    }
}
