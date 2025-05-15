using OpenTK.Graphics.OpenGL;

namespace ZPG01
{
    public class Viewport
    {
        public double Top { get; set; }
        public double Left { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        public Control Control { get; set; }
        public Viewport() { }

        public void Set()
        {
            GL.Viewport((int)(Left * Control.Width),
                (int)((1 - (Top + Height)) * Control.Height),
                (int)(Width * Control.Width),
                (int)(Height * Control.Height)
            );
        }

        public void Clear()
        {
            GL.Enable(EnableCap.ScissorTest);
            GL.Scissor((int)(Left * Control.Width),
                (int)((1 - Top - Height) * Control.Height),
                (int)(Width * Control.Width),
                (int)(Height * Control.Height)
            );
            GL.ClearColor(0, 0, 0, 0);
            GL.Clear(ClearBufferMask.ColorBufferBit);
        }

        public Vector2 WindowViewport(int x, int y)
        {
            return new Vector2(((double)x / Control.Width - Left) / Width * 2 - 1, -(((double)y / Control.Height - Top) / Height * 2 - 1));
        }
    }
}
