using OpenTK.GLControl;
using OpenTK.Graphics.OpenGL;

namespace ZPG
{
    public partial class Form1 : Form
    {
        Model model;// = new Model();
        Viewport viewport = new Viewport();


        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// vykreslovani sceny
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            glControl.MakeCurrent();
            viewport.Set();
            viewport.Clear();

            GL.PointSize(10);

            model.Draw();

            /*GL.Begin(PrimitiveType.Points);
            foreach (var vertex in model.vertices)
            {
                GL.Color3(vertex.Color.R, vertex.Color.G, vertex.Color.B);
                GL.Vertex2(vertex.Position.X, vertex.Position.Y);
            }
            GL.End();*/

            glControl.SwapBuffers();
        }

        private void glControl_MouseMove(object sender, MouseEventArgs e)
        {
            var pos = viewport.WindowViewport(e.X, e.Y);
        }

        private void glControl_MouseClick(object sender, MouseEventArgs e)
        {
            model.vertices.Add(new Vertex(viewport.WindowViewport(e.X, e.Y), new ColorRGB(1, 1, 1)));
            model.Changed = true;
        }

        private void glControl_KeyDown(object sender, KeyEventArgs e)
        {
            model.vertices[0].Position.X += 0.01f;
        }

        private void glControl_Load(object sender, EventArgs e)
        {
            // viewport zatím pøes celou kontrolku
            viewport = new Viewport()
            {
                Top = 0,
                Left = 0,
                Width = 1,
                Height = 1,
                Control = glControl
            };

            // zajistí automatické pøekreslování
            Application.Idle += (s, e) => glControl.Invalidate();

            model = new Model();
            model.vertices.Add(new Vertex(new Vector2(0.2, 0), new ColorRGB(1, 0, 0)));
            model.vertices.Add(new Vertex(new Vector2(-0.5, -0.5), new ColorRGB(1, 0, 0)));
            model.vertices.Add(new Vertex(new Vector2(0.5, 0.5), new ColorRGB(1, 1, 0)));

            // napojení dat listbox <-> propertygrid
            listBox1.DataSource = model.vertices;
            propertyGrid1.PropertyValueChanged += ((s, e) =>
            {
                model.vertices.ResetBindings(); Console.WriteLine('l');
                model.Changed = true;
            });
            listBox1.SelectedIndexChanged += ((s, e) =>
            {
                propertyGrid1.SelectedObject = listBox1.SelectedItem;
                propertyGrid1.ExpandAllGridItems();
            }
            );

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            model?.Dispose();
        }
    }
}
