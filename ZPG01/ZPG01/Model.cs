using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using OpenTK.Graphics.OpenGL;

namespace ZPG01
{
    public class Model : IDisposable
    {
        public int vbo;
        public int vboSize;
        public int ibo;
        public int iboSize;
        public BindingList<Vertex> Vertices { get; set; }
        public BindingList<Triangle> Triangles { get; set; }
        public bool Changed { get; set; } = true;
        public Model()
        {
            Vertices = new BindingList<Vertex>();
            Triangles = new BindingList<Triangle>();
            
            vbo = GL.GenBuffer();
            vboSize = 10;

            ibo = GL.GenBuffer();
            iboSize = 10;

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vboSize * VertexGL.Sizeof(), IntPtr.Zero, BufferUsageHint.DynamicDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, iboSize * TriangleGL.Sizeof(), IntPtr.Zero, BufferUsageHint.DynamicDraw);
        }

        public VertexGL[] GenVertexGLData()
        {
            var array = new VertexGL[Vertices.Count];

            for (int i = 0; i < Vertices.Count; i++)
            {
                var pos = Vertices[i].Position;
                var col = Vertices[i].Color;



                array[i] = new VertexGL
                {
                    position = new OpenTK.Mathematics.Vector3((float)pos.X, (float)pos.Y, 0),
                    color = new OpenTK.Mathematics.Vector3((float)col.R, (float)col.G, (float)col.B)
                };

            }
            return array;
        }


        public TriangleGL[] GenTriangleGLData()
        {
            var array = new TriangleGL[Triangles.Count];

            for (var i = 0; i < Triangles.Count; i++)
            {
                array[i] = new TriangleGL
                {
                    i1 = Triangles[i].I1,
                    i2 = Triangles[i].I2,
                    i3 = Triangles[i].I3,
                };

            }
            return array;
        }
        public void Draw()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo);

            if (Changed)
            {
                var vertices = this.GenVertexGLData();
                if (vertices.Length > vboSize)
                {
                    vboSize *= 2;
                    GL.BufferData(BufferTarget.ArrayBuffer, vboSize * VertexGL.Sizeof(), IntPtr.Zero, BufferUsageHint.DynamicDraw);
                }
                
                GL.BufferSubData(BufferTarget.ArrayBuffer, 0, vertices.Length * VertexGL.Sizeof(), vertices);
                
                var triangles = this.GenTriangleGLData();
                if (triangles.Length > iboSize)
                {
                    iboSize *= 2;
                    GL.BufferData(BufferTarget.ElementArrayBuffer, iboSize * TriangleGL.Sizeof(), IntPtr.Zero, BufferUsageHint.DynamicDraw);
                }
                GL.BufferSubData(BufferTarget.ElementArrayBuffer, 0, triangles.Length * VertexGL.Sizeof(), triangles);


                Changed = false;
            }
            
            
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.ColorArray);
            GL.VertexPointer(3, VertexPointerType.Float, VertexGL.Sizeof(), IntPtr.Zero);
            GL.ColorPointer(3, ColorPointerType.Float, VertexGL.Sizeof(), (IntPtr)(3 * sizeof(float)));

            GL.LineWidth(5);
            GL.Enable(EnableCap.CullFace);
            GL.PolygonMode(TriangleFace.FrontAndBack, PolygonMode.Line);
            

            GL.DrawArrays(PrimitiveType.Points, 0, this.Vertices.Count);
            //GL.DrawElements(PrimitiveType.Triangles, 3 * Triangles.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);
            if (Triangles.Count > 0)
            {
                GL.DrawElements(PrimitiveType.Triangles, 3 * Triangles.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);
            }

        }

        bool disposed = false;
        public void Dispose()
        {
            if (!disposed)
            {
                GL.DeleteBuffer(vbo);
            }
            GC.SuppressFinalize(this);
        }


        ~Model() => Dispose(); 
    }


}
