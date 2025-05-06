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
    internal class MarchingCubes
    {
        float[] data;
        int w, h, d;
        int size;
        float threshold;
        List<Vector3> points = new List<Vector3>();
        List<int> indices = new List<int>();
        int vbo, vao;


        public MarchingCubes(float[] data, int w, int h, float threshold)
        {
            this.data = data; this.w = w; this.h = h; this.threshold = threshold;
            this.size = Math.Max(w, Math.Max(h,d));
            this.d = data.Length / (w * h);

            /* Zde doplnit samotné genrování geometrie */

            for (int k = 0; k < d - 1; k++)
            {
                for (int j = 0; j < h - 1; j++)
                {
                    for (int i = 0; i < w - 1; i++)
                    {
                       var triangles = GetTriangles(i, j, k);
                       CreateGeometry(i, j, k, triangles);
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


        private List<int[]> GetTriangles(int i, int j, int k)
        {
            int mask = 0;
            int bit = 1;


            for (int oz = 0; oz < 2; oz++)
            {
                int z = (k + oz) * w * h;
                for (int oy = 0; oy < 2; oy++)
                {
                    int y = (j + oy) * w;
                    for (int ox = 0; ox < 2; ox++)
                    {
                        int x = i + ox;
                        if (data[x + y + z] < threshold) mask |= bit;
                        bit <<= 1;

                    }
                }
            }
            return MCTables.TriangleTable[mask];

        }


        private void CreateGeometry(int i, int j, int k, List<int[]> triangles)
        {
            foreach (var tri in triangles)
            {
                for (int t = 0; t < 3; t++)
                {
                    var i1 = MCTables.EdgeVertexIndices[tri[t], 0];
                    var i2 = MCTables.EdgeVertexIndices[tri[t], 1];

                    var idx1 = new Vector3i(i + ((i1 & 1)) >> 0, j + ((i1 & 2) >> 1), k + ((i1 & 4) >> 2));
                    var idx2 = new Vector3i(i + ((i2 & 1)) >> 0, j + ((i2 & 2) >> 1), k + ((i2 & 4) >> 2));
                    
                    var vec1 = new Vector3(idx1.X, idx1.Y, idx1.Z);
                    var vec2 = new Vector3(idx2.X, idx2.Y, idx2.Z);


                    var val1 = data[(idx1.Z * h + idx1.Y) * w + idx1.X];
                    var val2 = data[(idx2.Z * h + idx2.Y) * w + idx2.X];

                    float p = 0.5f;
                    if (val1 - val2 != 0)
                    {
                        p = (threshold - val1) / (val2 - val1);
                    }

                    points.Add(((1 - p) * vec1 + p * vec2) / size );

                }
            }
        }

        public void Draw()
        {
            GL.PolygonMode(TriangleFace.FrontAndBack, PolygonMode.Line);
            GL.Disable(EnableCap.CullFace);
            GL.BindVertexArray(vao);
            GL.DrawArrays(PrimitiveType.Triangles, 0, points.Count);
        }

    }
}
