using OpenTK.Graphics.OpenGL;
using System.ComponentModel;
using ZPG03;

namespace ZPG01
{
    public class Model : IDisposable
    {
        public int vbo;
        public int vboSize;

        public int ibo;
        public int iboSize;

        public bool Changed { get; set; } = true;
        public BindingList<Vertex> Vertices { get; set; }
        public BindingList<Triangle> Triangles { get; set; }

        public Model()
        {
            Vertices = new BindingList<Vertex>();
            vbo = GL.GenBuffer();
            vboSize = 10;

            Triangles = new BindingList<Triangle>();
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
            for (int i = 0; i < Vertices.Count; i++)
            {
                array[i] = new VertexGL
                {
                    position = new OpenTK.Mathematics.Vector3((float)Vertices[i].Position.X, (float)Vertices[i].Position.Y, (float)Vertices[i].Position.Z),
                    color = new OpenTK.Mathematics.Vector3((float)Vertices[i].Color.R, (float)Vertices[i].Color.G, (float)Vertices[i].Color.B)
                };
            }
            return array;
        }





        public TriangleGL[] GenTriangleGLData()
        {
            var array = new TriangleGL[Triangles.Count];
            for (int i = 0; i < Triangles.Count; i++)
            {
                array[i] = new TriangleGL
                {
                    i1 = Triangles[i].I1,
                    i2 = Triangles[i].I2,
                    i3 = Triangles[i].I3
                };
            }
            return array;
        }



        public void Draw()
        {
            // Připojení bufferu
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo);

            // Pokud došlo ke změně, přegenerují se data
            if (Changed)
            {
                var vertices = GenVertexGLData();

                // Pokud jsou data větší než aktuální buffer, zvětší se 
                if (vertices.Length > vboSize)
                {
                    vboSize *= 2;
                    GL.BufferData(BufferTarget.ArrayBuffer, vboSize * VertexGL.SizeOf(), IntPtr.Zero, BufferUsageHint.DynamicDraw);
                }

                // Nahrání dat
                GL.BufferSubData(BufferTarget.ArrayBuffer, 0, vertices.Length * VertexGL.SizeOf(), vertices);

                var triangles = GenTriangleGLData();

                if (triangles.Length > iboSize)
                {
                    iboSize *= 2;
                    GL.BufferData(BufferTarget.ElementArrayBuffer, iboSize * TriangleGL.SizeOf(), IntPtr.Zero, BufferUsageHint.DynamicDraw);
                }
                // Nahrání dat
                GL.BufferSubData(BufferTarget.ElementArrayBuffer, 0, triangles.Length * TriangleGL.SizeOf(), triangles);

                Changed = false;
            }

            // Povolení poloh vrcholů a barev a nastavení odpovídajících ukazatelů do paměti
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.ColorArray);
            GL.VertexPointer(3, VertexPointerType.Float, VertexGL.SizeOf(), IntPtr.Zero);
            GL.ColorPointer(3, ColorPointerType.Float, VertexGL.SizeOf(), (IntPtr)(3 * sizeof(float)));

            GL.LineWidth(5);
            GL.Enable(EnableCap.CullFace);
            GL.PolygonMode(TriangleFace.FrontAndBack, PolygonMode.Line);

            // Vykreslení pole vrcholů
            GL.DrawArrays(PrimitiveType.Points, 0, Vertices.Count);
            GL.DrawElements(PrimitiveType.Triangles, 3 * Triangles.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);

        }


        public void Drdaw()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo);

            if (Changed)
            {
                var vertices = GenVertexGLData();
                if (vertices.Length > vboSize)
                {
                    vboSize *= 2;
                    GL.BufferData(BufferTarget.ArrayBuffer, vboSize * VertexGL.SizeOf(), IntPtr.Zero, BufferUsageHint.DynamicDraw);
                }
                GL.BufferSubData(BufferTarget.ArrayBuffer, 0, vertices.Length * VertexGL.SizeOf(), vertices);

                var triangles = GenTriangleGLData();
                if (triangles.Length > iboSize)
                {
                    iboSize *= 2;
                    GL.BufferData(BufferTarget.ElementArrayBuffer, iboSize * TriangleGL.SizeOf(), IntPtr.Zero, BufferUsageHint.DynamicDraw);
                    GL.BufferSubData(BufferTarget.ElementArrayBuffer, 0, triangles.Length * TriangleGL.SizeOf(), triangles);
                }

                Changed = false;
            }

            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.ColorArray);
            GL.VertexPointer(3, VertexPointerType.Float, VertexGL.SizeOf(), IntPtr.Zero);
            GL.ColorPointer(3, ColorPointerType.Float, VertexGL.SizeOf(), (IntPtr)(3 * sizeof(float)));

            GL.Enable(EnableCap.CullFace);
            GL.LineWidth(5);
            GL.PolygonMode(TriangleFace.FrontAndBack, PolygonMode.Line);

            GL.DrawArrays(PrimitiveType.Points, 0, Vertices.Count);
            GL.DrawElements(PrimitiveType.Triangles, 3 * Vertices.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);

        }

        bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {

                }
                GL.DeleteBuffer(vbo);
                GL.DeleteBuffer(ibo);
            }
        }

        ~Model() => Dispose(false);

    }
}
