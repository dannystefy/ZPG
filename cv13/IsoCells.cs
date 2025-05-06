using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace cv13
{
    internal class IsoCells
    {
        float[] data;
        int w, h, d;
        int size;
        float threshold;
        List<Vector3> points = new List<Vector3>();
        int vbo, vao;

        Vector3[] edges = new Vector3[] {
                new Vector3(0.0f,0.0f, 0.0f), new Vector3(0.0f,0.0f, 1.0f),
                new Vector3( 1.0f,0.0f, 0.0f), new Vector3( 1.0f,0.0f, 1.0f),
                new Vector3( 1.0f, 1.0f, 0.0f), new Vector3( 1.0f, 1.0f, 1.0f),
                new Vector3(0.0f, 1.0f, 0.0f), new Vector3(0.0f, 1.0f, 1.0f),

                new Vector3(0.0f,0.0f, 0.0f), new Vector3( 1.0f,0.0f,0.0f),
                new Vector3( 1.0f,0.0f, 0.0f), new Vector3( 1.0f, 1.0f,0.0f),
                new Vector3( 1.0f, 1.0f, 0.0f), new Vector3(0.0f, 1.0f,0.0f),
                new Vector3(0.0f, 1.0f, 0.0f), new Vector3(0.0f,0.0f,0.0f),

                new Vector3(0.0f,0.0f,  1.0f), new Vector3( 1.0f,0.0f, 1.0f),
                new Vector3( 1.0f,0.0f,  1.0f), new Vector3( 1.0f, 1.0f, 1.0f),
                new Vector3( 1.0f, 1.0f,  1.0f), new Vector3(0.0f, 1.0f, 1.0f),
                new Vector3(0.0f, 1.0f,  1.0f), new Vector3(0.0f,0.0f, 1.0f),
        };



        public IsoCells(float[] data, int w, int h, float threshold)
        {
            this.data = data; this.w = w; this.h = h; this.threshold = threshold;
            this.d = data.Length / (w * h);
            this.size = Math.Max(w, Math.Max(h, d));

            /* Zde doplnit samotné genrování geometrie */

            for (int k = 0; k < d - 1; k++)
            {
                for (int j = 0; j < h - 1; j++)
                {
                    for (int i = 0; i < w - 1; i++)
                    {
                        GenCell(i, j, k);  
                    }
                }

            }

                        vao = GL.GenVertexArray();
            vbo = GL.GenBuffer();
            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, points.Count * Vector3.SizeInBytes, points.ToArray(), BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, Vector3.SizeInBytes, 0);
            GL.EnableVertexAttribArray(0);
        }
        

        private void GenCell(int i, int j, int k)
        {
            int t = 0;

            for (int oz = 0; oz < 2; oz++)
            {
                int z = (k + oz) * w * h;
                for (int oy = 0; oy < 2; oy++)
                {
                    int y = (j + oy) * w;
                    for (int ox = 0; ox < 2; ox++)
                    {
                        int x = i + ox;
                        if (data[z + y + x] < threshold) t++;                      
                    }
                }
            }

            if (t != 0 && t != 8)
            {
                for (var e = 0; e < edges.Length; e++)
                {
                    points.Add((edges[e] + new Vector3(i, j, k)) / size);
                }
            }
        }

        public void Draw()
        {
            GL.PolygonMode(TriangleFace.FrontAndBack, PolygonMode.Fill);
            GL.Disable(EnableCap.CullFace);
            GL.BindVertexArray(vao);
            GL.DrawArrays(PrimitiveType.Lines, 0, points.Count);
        }
    }
}
