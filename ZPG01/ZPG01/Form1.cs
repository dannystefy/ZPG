using OpenTK.GLControl;
using OpenTK.Graphics.OpenGL;
using System.Windows.Forms;

namespace ZPG01
{
    public partial class Form1 : Form
    {
        private Model Model { get; set; }
        private Viewport Viewport { get; set; } = new Viewport();
        private Random Random { get; set; } = new Random(0);

        public Form1()
        {
            InitializeComponent();



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
            /* GL.Begin(PrimitiveType.Points);
             foreach (var vertex in Model.Vertices)
             {
                 GL.Color3(vertex.Color.R, vertex.Color.G, vertex.Color.B);
                 GL.Vertex2(vertex.Position.X, vertex.Position.Y);
             }
             GL.End(); */

            Model.Draw();
            glControl1.SwapBuffers();
        }

        private void glControl1_MouseMove(object sender, MouseEventArgs e)
        {
            var pos = Viewport.WindowViewport(e.X, e.Y);
            Text = $"{pos.X} {pos.Y}";
        }

        private void glControl1_Click(object sender, MouseEventArgs e)
        {
            int rightcliks = 0;
            Vector2 pos = Viewport.WindowViewport(e.X,e.Y);
            Triangle t;
            if (e.Button == MouseButtons.Left)
            {
                for (int i = 0; i < Model.Vertices.Count; i++)
                {
                    t = Model.Triangles[i];
                    if (t.I1 == i ||  t.I2 == i || t.I3 == i)
                    {
                        Model.Triangles.RemoveAt(i);
                        continue;
                    }
                    t.I1 = t.I1 >i ? t.I1-1 : i;
                    t.I2 = t.I2 > i ? t.I2-1 : i;
                    t.I3 = t.I3 > i ? t.I3-1 : i;

                    Model.Vertices.RemoveAt(i);
                    Model.Changed = true;


                }
                Model.Vertices.Add(new Vertex(pos, RandomColor()));
                Model.Changed = true;
            }

            if (e.Button == MouseButtons.Right)
            {

                
               

            }
        }

        private ColorRGB RandomColor()
        {
            return new ColorRGB(Random.NextDouble(), Random.NextDouble(), Random.NextDouble());
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }

        private void glControl1_Load(object sender, EventArgs e)
        {
            Model = new Model();
            Model.Vertices.Add(new Vertex(new Vector2(-0.9, -0.9), new ColorRGB(1, 0, 1)));
            Model.Vertices.Add(new Vertex(new Vector2(0.9, 0.9), new ColorRGB(0, 1, 0)));
            Model.Vertices.Add(new Vertex(new Vector2(-0.9, 0.9), new ColorRGB(0, 0, 1)));
            Model.Vertices.Add(new Vertex(new Vector2(0.9, -0.9), new ColorRGB(0, 0, 1)));

            Model.Triangles.Add(new Triangle(0, 1, 2));
            Model.Triangles.Add(new Triangle(0, 3, 1));
            listBox1.DataSource = Model.Vertices;


        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Model?.Dispose();
        }

        private void glControl1_Click(object sender, EventArgs e)
        {

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer2_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            Model.Changed = true;
        }

        private void propertyGrid1_Click(object sender, EventArgs e)
        {

        }
    }
}
