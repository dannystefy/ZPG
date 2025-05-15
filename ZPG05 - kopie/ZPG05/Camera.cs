using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using ZPG05.Cameras;

namespace ZPG05
{
    public class Camera : ICamera
    {
        public Viewport viewport;

        private float zoom = 1f;

        public float x = 0f;
        public float y = 0f;
        public float z = 0f;

        float rx = 0f;
        float ry = 0f;



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

        public Matrix4 Projection{
           
            get
            {
                float ratio = (float)((viewport.Width * viewport.Control.Width) / (viewport.Height * viewport.Control.Height));
                // Matrix4 projection = Matrix4.CreateOrthographic(zoom * 2, zoom * 2 / ratio, -10, 10);
                Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(zoom, ratio, 0.1f, 100f);
                return projection;

            }
        }


        public Camera(Viewport viewport)
        {
            this.viewport = viewport;
        }

    /*    public void SetProjection()
        {
            float ratio = (float)((viewport.Width * viewport.Control.Width) / (viewport.Height * viewport.Control.Height));
            // Matrix4 projection = Matrix4.CreateOrthographic(zoom * 2, zoom * 2 / ratio, -10, 10);
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(zoom, ratio, 0.1f, 100f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);
        }
    */
        public void Zoom(float coef)
        {
            // zoom *= coef;
            zoom += coef / 10f;
            zoom = Math.Max(0.2f, Math.Min(2, zoom));
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
