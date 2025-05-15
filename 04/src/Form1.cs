using OpenTK.GLControl;
using OpenTK.Graphics.OpenGL;
using System.Windows.Forms;

namespace ZPG
{
    public partial class Form1 : Form
    {
        public Model model;// = new Model();
        public Viewport viewport = new Viewport();


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

            glControl.SwapBuffers();
        }

        private void glControl_MouseClick(object sender, MouseEventArgs e)
        {
            model.vertices.Add(new Vertex(viewport.WindowViewport(e.X, e.Y), new ColorRGB(1, 1, 1)));
            model.Changed = true;
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
            model.vertices.Add(new Vertex(new Vector2(-0.9, -0.9), new ColorRGB(1, 0, 0)));
            model.vertices.Add(new Vertex(new Vector2(0.9, 0.9), new ColorRGB(1, 0, 0)));
            model.vertices.Add(new Vertex(new Vector2(-0.9, 0.9), new ColorRGB(1, 1, 0)));
            model.vertices.Add(new Vertex(new Vector2(0.9, -0.9), new ColorRGB(1, 1, 0)));
            model.triangles.Add(new Triangle(0, 1, 2));
            model.triangles.Add(new Triangle(1, 3, 0));

            // napojení dat listbox <-> propertygrid
            listVertices.DataSource = model.vertices;
            propertyVertex.PropertyValueChanged += ((s, e) =>
            {
                model.vertices.ResetBindings();
                model.Changed = true;
            });
            listVertices.SelectedIndexChanged += ((s, e) =>
            {
                propertyVertex.SelectedObject = listVertices.SelectedItem;
                propertyVertex.ExpandAllGridItems();
            }
            );

            listTriangles.DataSource = model.triangles;
            propertyTriangle.PropertyValueChanged += ((s, e) =>
            {
                model.triangles.ResetBindings();
                model.Changed = true;
            });
            listTriangles.SelectedIndexChanged += ((s, e) =>
            {
                propertyTriangle.SelectedObject = listTriangles.SelectedItem;
                propertyTriangle.ExpandAllGridItems();
            }
            );

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            model?.Dispose();
        }

        private void buttonTriangleAdd(object sender, EventArgs e)
        {
            model.triangles.Add(new Triangle(0, 0, 0));
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
