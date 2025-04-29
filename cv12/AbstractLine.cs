using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace cv12
{
    public abstract class AbstractLine
    {


        /// <summary>
        /// pocet segmentu
        /// </summary>
        protected int Segments;

        /// <summary>
        /// VAO pro segmenty čáry
        /// </summary>
        protected int vbo;
        protected int vao;

        /// <summary>
        /// VAO pro řídící body
        /// </summary>
        protected int controlVbo;
        protected int controlVao;

        protected int pointCount;
        protected List<Vector2> points = new List<Vector2>();


        protected virtual void Initialize()
        {
            vao = GL.GenVertexArray();
            vbo = GL.GenBuffer();

            GeneratePoints(points);
            pointCount = points.Count;

            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, pointCount * Vector2.SizeInBytes, points.ToArray(), BufferUsageHint.DynamicDraw);

            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, Vector2.SizeInBytes, 0);
            GL.EnableVertexAttribArray(0);

            controlVao = GL.GenVertexArray();
            controlVbo = GL.GenBuffer();

            GL.BindVertexArray(controlVao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, controlVbo);
            var controlPoints = GetControlPoints();
            GL.BufferData(BufferTarget.ArrayBuffer, controlPoints.Count * Vector2.SizeInBytes, controlPoints.ToArray(), BufferUsageHint.DynamicDraw);

            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, Vector2.SizeInBytes, 0);
            GL.EnableVertexAttribArray(0);
        }


        protected abstract void GeneratePoints(List<Vector2> output);


        protected virtual void UpdateBuffer()
        {
            GeneratePoints(points);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferSubData(BufferTarget.ArrayBuffer, 0, pointCount * Vector2.SizeInBytes, points.ToArray());

            GL.BindBuffer(BufferTarget.ArrayBuffer, controlVbo);
            var controlPoints = GetControlPoints();
            GL.BufferSubData(BufferTarget.ArrayBuffer, 0, controlPoints.Count * Vector2.SizeInBytes, controlPoints.ToArray());
        }


        /// <summary>
        /// Vykresleni cary
        /// </summary>
        public virtual void Draw()
        {
            GL.BindVertexArray(vao);
            GL.DrawArrays(PrimitiveType.LineStrip, 0, pointCount);
        }

        /// <summary>
        /// Vykresleni rididich bodu
        /// </summary>
        public virtual void DrawControlPoints()
        {
            GL.BindVertexArray(controlVao);
            GL.PointSize(5f);
       
  
            GL.DrawArrays(PrimitiveType.Points, 0, GetControlPoints().Count);
        }

        /// <summary>
        /// Uvolneni VBO/VAO
        /// </summary>
        public virtual void Dispose()
        {
            GL.DeleteBuffer(vbo);
            GL.DeleteVertexArray(vao);
            GL.DeleteBuffer(controlVbo);
            GL.DeleteVertexArray(controlVao);
        }


        public abstract List<Vector2> GetControlPoints();

    }
}
