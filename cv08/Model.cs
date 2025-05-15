using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Zpg
{
    /// <summary>
    /// Jednoduchý model obsahující seznam vrcholů
    /// </summary>
    public class Model : IDisposable
    {

        public Shader Shader { get; set; }

        public Material Material { get; set; } = new Material();             

        public Vector3 position = new Vector3(0,-1.5f,0);

        public Dictionary<string, Texture> Textures { get; set; } = new Dictionary<string, Texture>();


        int vbo;       
        int ibo;
        int vao;

        int triangles;

        public Model(Vertex[] vertices, int[] indices)
        {
            Create(vertices, indices);
        }

        public Model(string objFilename)
        { 
            (Vertex[] vertices, int[] indices) = BetterObj.LoadObj(objFilename);
            Create(vertices, indices);
        }

        protected void Create(Vertex[] vertices, int[] indices)
        {
            triangles = indices.Length / 3;

            // vytvoření a připojení VAO
            GL.GenVertexArrays(1, out vao);
            GL.BindVertexArray(vao);

            // Vytvoření a připojení VBO
            vbo = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * Vertex.SizeOf(), vertices, BufferUsageHint.StaticDraw);

            // vytvoření a připojení IBO
            ibo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo);

            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(int), indices, BufferUsageHint.StaticDraw);

            // namapování pointerů na lokace v shaderu
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, Vertex.SizeOf(), IntPtr.Zero);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, Vertex.SizeOf(), (IntPtr)3 * sizeof(float));
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, Vertex.SizeOf(), (IntPtr)6 * sizeof(float));
            GL.EnableVertexAttribArray(1);
            GL.EnableVertexAttribArray(2);
            GL.BindVertexArray(0);
        }


        /// <summary>
        /// Vykreslení modelu
        /// </summary>
        public void Draw(Camera camera, Light light)
        {
            Matrix4 translate = Matrix4.CreateTranslation((float)position.X, (float)position.Y, (float)position.Z);

            Shader.Use();
            Shader.SetUniform("projection", camera.Projection);
            Shader.SetUniform("view", camera.View);
            Shader.SetUniform("model", translate);

            Shader.SetUniform("cameraPosWorld", camera.pos);

            Shader.SetUniform("lightPosWorld", light.position);
            Shader.SetUniform("lightColor", light.color);
            Shader.SetUniform("lightIntensity", light.intensity * 0.5f);





            Material.SetUniforms(Shader);


            foreach (var kw in Textures)
            {
                kw.Value.Bind(Shader.GetUniformLocation(kw.Key));

            }

            // Připojení bufferu
            GL.BindVertexArray(vao);

            //GL.Enable(EnableCap.CullFace);
            //GL.LineWidth(5);
            GL.PolygonMode(TriangleFace.FrontAndBack, PolygonMode.Fill);

            // Vykreslení pole vrcholů
            GL.DrawElements(PrimitiveType.Triangles, 3*triangles, DrawElementsType.UnsignedInt, IntPtr.Zero);
            GL.BindVertexArray(0);
        }

        #region Dispose - uvolnění paměti
        bool disposed = false;
        public void Dispose()
        {
            if (!disposed)
            {
                GL.DeleteBuffer(vbo);
		disposed = true;
	    }
        }
        #endregion
    }
}
