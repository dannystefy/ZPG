using OpenTK.GLControl;
using OpenTK.Graphics.OpenGL;
using System.Windows.Forms;

namespace ZPG01
{
    public partial class Form1 : Form
    {
        private Model Model { get; set; } = new Model();
        private Viewport Viewport { get; set; } = new Viewport();
        private Random Random { get; set; } = new Random(0);

        public Form1()
        {
            InitializeComponent();

            Model.Vertices.Add(new Vertex(new Vector2(0.0f, -0.5f), new ColorRGB(1, 0, 1)));
            Model.Vertices.Add(new Vertex(new Vector2(-0.5f, 0.5f), new ColorRGB(0, 1, 0)));
            Model.Vertices.Add(new Vertex(new Vector2(0.5f, 0.5f), new ColorRGB(0, 0, 1)));
            
            Viewport = new Viewport()
            {
                Top = 0,
                Left = 0,
                Width = 1,
                Height = 1,
                Control = glControl1
            };

            Application.Idle += Application_Idle;
        }

        private void Application_Idle(object? sender, EventArgs e)
        {
            glControl1.Invalidate();
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            glControl1.MakeCurrent();
            Viewport.Set();
            Viewport.Clear();

            GL.ClearColor(Color.Red);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.PointSize(10);
            GL.Begin(PrimitiveType.Points);
            foreach (var vertex in Model.Vertices)
            {
                GL.Color3(vertex.Color.R, vertex.Color.G, vertex.Color.B);
                GL.Vertex2(vertex.Position.X, vertex.Position.Y);
            }
            GL.End();

            glControl1.SwapBuffers();
        }

        private void glControl1_MouseMove(object sender, MouseEventArgs e)
        {
            var pos = Viewport.WindowViewport(e.X, e.Y);
            Text = $"{pos.X} {pos.Y}";
        }

        private void glControl1_Click(object sender, MouseEventArgs e)
        {
            var vertex = new Vertex(Viewport.WindowViewport(e.X, e.Y), RandomColor());
            Model.Vertices.Add(vertex);
        }

        private ColorRGB RandomColor()
        {
            return new ColorRGB(Random.NextDouble(), Random.NextDouble(), Random.NextDouble());
        }
    }
}
