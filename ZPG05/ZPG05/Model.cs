using System.ComponentModel;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using ZPG05.Cameras;

namespace ZPG05
{
    public class Model : IDisposable
    {
        public Shader Shader { get; set; }
        int vao;


        public int vbo;
        public int vboSize;
        public int ibo;
        public int iboSize;

        public bool Changed { get;set; } = true;

        public BindingList<Vertex> Vertices { get; set; }
        public BindingList<Triangle> Triangles { get; set; }

        public Vector3 Position { get; set; } = new Vector3(0, 0, 0);

        public Model()
        {
            GL.GenVertexArrays(1, out vao);
            GL.BindVertexArray(vao);

            Vertices = new BindingList<Vertex>();
            Triangles = new BindingList<Triangle>();

            vbo = GL.GenBuffer();
            vboSize = 10;
            ibo = GL.GenBuffer();
            iboSize = 10;

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vboSize * VertexGL.SizeOf(), IntPtr.Zero, BufferUsageHint.DynamicDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, iboSize * TriangleGL.SizeOf(), IntPtr.Zero, BufferUsageHint.DynamicDraw);
        }

        public VertexGL[] GenVertexGLData()
        {
            var array = new VertexGL[Vertices.Count];
            for (var i = 0; i < Vertices.Count; i++)
            {
                var pos = Vertices[i].Position;
                var col = Vertices[i].Color;

                array[i] = new VertexGL
                {
                    position = new OpenTK.Mathematics.Vector3((float)pos.X, (float)pos.Y, (float)pos.Z),
                    color = new OpenTK.Mathematics.Vector3(col.R, col.G, col.B)
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

        public void Draw(ICamera camera)
        {
            var translation = Matrix4.CreateTranslation((float)Position.X, (float)Position.Y, (float)Position.Z);


            /*
            GL.PushMatrix();
            GL.MultMatrix(ref translation);
            */


            Shader.Use();
            Shader.SetUniform("projection", camera.Projection);
            Shader.SetUniform("view", camera.View);
            Shader.SetUniform("model", translation);

            GL.BindVertexArray(vao);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo);

            if (Changed)
            {
                var vertices = this.GenVertexGLData();
                if (vertices.Length > vboSize)
                {
                    vboSize *= 2;
                    GL.BufferData(BufferTarget.ArrayBuffer, vboSize * VertexGL.SizeOf(), IntPtr.Zero, BufferUsageHint.DynamicDraw);
                }
                GL.BufferSubData(BufferTarget.ArrayBuffer, 0, vertices.Length * VertexGL.SizeOf(), vertices);

                var triangles = this.GenTriangleGLData();
                if (triangles.Length > iboSize)
                {
                    iboSize *= 2;
                    GL.BufferData(BufferTarget.ElementArrayBuffer, iboSize * TriangleGL.SizeOf(), IntPtr.Zero, BufferUsageHint.DynamicDraw);
                }
                GL.BufferSubData(BufferTarget.ElementArrayBuffer, 0, triangles.Length * TriangleGL.SizeOf(), triangles);

                Changed = false;
            }
            /*
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.ColorArray);
            GL.VertexPointer(3, VertexPointerType.Float, VertexGL.SizeOf(), IntPtr.Zero);
            GL.ColorPointer(3, ColorPointerType.Float, VertexGL.SizeOf(), (IntPtr)(3 * sizeof(float)));
            */


            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, VertexGL.SizeOf(), IntPtr.Zero);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, VertexGL.SizeOf(), IntPtr.Zero);
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);


            GL.LineWidth(5);
            //GL.Enable(EnableCap.CullFace);
            GL.PolygonMode(TriangleFace.FrontAndBack, PolygonMode.Line);

            GL.DrawArrays(PrimitiveType.Points, 0, this.Vertices.Count);
            GL.DrawElements(PrimitiveType.Triangles, 3 * Triangles.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);

            //GL.PopMatrix();
        }

        bool disposed = false;
        public void Dispose()
        {
            if (!disposed) 
            {
                GL.DeleteBuffer(vbo);
                disposed = true;
            }
            GC.SuppressFinalize(this);
        }

        ~Model() => Dispose();
    }
}
