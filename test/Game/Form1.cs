
using OpenTK.Graphics.OpenGL;


namespace Game
{
    public class Vector2 { 
        
        public double X {  get; set; } 
        public double Y { get; set; }

        public Vector2(double x, double y)
        {
            X = x;
            Y = y;
        }

    }

    public class Vertex
    {
        public Vector2 Position { get; set; }

        public Vertex(Vector2 position)
        {
            Position = position;
        }
    
    }


    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private void glControl_Paint(object sender, PaintEventArgs e)
        {
            glControl.MakeCurrent();
            GL.ClearColor(Color.Green);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.PointSize(10);
            GL.Begin(PrimitiveType.Points);
            GL.Vertex2(0, 0);
            GL.End();

            glControl.SwapBuffers();
        }
    }
}
