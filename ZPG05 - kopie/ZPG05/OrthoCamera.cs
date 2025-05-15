using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using ZPG05.Cameras;

namespace ZPG05
{
    public class OrthoCamera: ICamera
    {
        public Viewport viewport;

        public float scale = 1f;

        public float x = 0f;
        public float y = 0f;
        public float z = 0f;

        public float rx = 0f;
        public float ry = 0f;

        public Matrix4 View
        {
            get
            {
                Matrix4 view;
                view = Matrix4.Identity;
                view *= Matrix4.CreateTranslation(x, y, z);
                view *= Matrix4.CreateRotationY(ry);
                view *= Matrix4.CreateRotationX(rx);
                return view;
            }
        }


        public Matrix4 Projection
        {
            get
            {
                float ratio = (float)((viewport.Width * viewport.Control.Width) / (viewport.Height * viewport.Control.Height));
                Matrix4 projection = Matrix4.CreateOrthographic(scale * 2, scale * 2 / ratio, -20, 20);
                return projection;
            }
        }
        public OrthoCamera(Viewport viewport)
        {
            this.viewport = viewport;
        }
/*
        public void SetProjection()
        {
            float ratio = (float)((viewport.Width * viewport.Control.Width) / (viewport.Height * viewport.Control.Height));
            Matrix4 projection = Matrix4.CreateOrthographic(scale * 2, scale * 2 / ratio, -20, 20);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);
        }
*/
        public void Zoom(float coef)
        {
            scale *= coef;
        }

        public void Move(float x, float y)
        {
            this.x -= (float)(x * Math.Cos(ry) + y * Math.Sin(ry));
            this.z -= (float)(x * Math.Sin(ry) - y * Math.Cos(ry));
        }

        public void RotateX(float a)
        {
            rx += a;
        }

        public void RotateY(float a)
        {
            ry += a;
        }
        /*
                public void SetView()
                {
                    Matrix4 view;

                    view = Matrix4.Identity;
                    view *= Matrix4.CreateTranslation(x, y, z);
                    view *= Matrix4.CreateRotationY(ry);
                    view *= Matrix4.CreateRotationX(rx);
                    GL.MatrixMode(MatrixMode.Modelview);
                    GL.LoadMatrix(ref view);
                }
        */
    }
}
