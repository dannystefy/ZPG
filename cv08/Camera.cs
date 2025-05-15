using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Zpg
{
    public class Camera
    {
        private float zoom = 1;
        public Vector3 pos = new Vector3(0,1.7f,5);
        public float rx = 0;
        public float ry = 0;
        public float speed = 1;
        public float sensitivity = 1;

        public Viewport viewport;
        public Camera(Viewport viewport)
        {
            this.viewport = viewport;
        }

        public virtual Matrix4 Projection
        {
            get
            {
                float ratio = (float)((viewport.Width * viewport.window.ClientSize.X) / (viewport.Height * viewport.window.ClientSize.Y));
                return Matrix4.CreatePerspectiveFieldOfView(zoom, ratio, 0.1f, 50);
            }
        }

        public void Zoom(float coef)
        {
            //zoom *= coef;
            zoom += coef / 10.0f;
            zoom = Math.Max(0.2f, Math.Min(2, zoom));
            Console.WriteLine(zoom);
        }

        public void Move(float xdt, float ydt)
        {
            pos.X += speed * (float)(xdt * Math.Cos(ry) + ydt * Math.Sin(ry));
            pos.Z += speed * (float)(xdt * Math.Sin(ry) - ydt * Math.Cos(ry));
        }

        public void RotateX(float a)
        {
            rx += a * sensitivity;
            rx = (float)Math.Max(-Math.PI / 2, Math.Min(Math.PI / 2, rx));
        }

        public void RotateY(float a)
        {
            ry += a * sensitivity;
        }


        public virtual Matrix4 View
        {
            get
            {
                Matrix4 view;
                view = Matrix4.Identity;
                view *= Matrix4.CreateTranslation(-pos);
                view *= Matrix4.CreateRotationY(ry);
                view *= Matrix4.CreateRotationX(rx);
                return view;
            }

        }
    }
}
