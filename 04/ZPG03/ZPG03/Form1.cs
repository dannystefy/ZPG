using OpenTK.Graphics.OpenGL;
using ZPG03;

namespace ZPG01
{
    public partial class Form1 : Form
    {
        public const float RemoveVertexDistance = 0.05f;

        private Model Model { get; set; }
        private Viewport Viewport { get; set; } = new Viewport();
        private Random Random { get; set; } = new Random(0);

        private List<int> ClickedVertices { get; set; } = new List<int>();

        public Camera Camera { get; set; }

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

            Camera.SetProjection();
            Camera.SetView();

            GL.ClearColor(Color.White);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.PointSize(10);
            //GL.Begin(PrimitiveType.Points);
            //foreach (var vertex in Model.Vertices)
            //{
            //    GL.Color3(vertex.Color.R, vertex.Color.G, vertex.Color.B);
            //    GL.Vertex2(vertex.Position.X, vertex.Position.Y);
            //}
            //GL.End();

            Model.Draw();

            GL.PointSize(5);
            GL.Begin(PrimitiveType.Points);
            foreach (var vertex in Model.Vertices)
            {
                GL.Color3(vertex.Color.R, vertex.Color.G, vertex.Color.B);
                GL.Vertex3(vertex.Position.X, vertex.Position.Y, vertex.Position.Z);
            }
            GL.End();



            glControl1.SwapBuffers();
            glControl1.Focus();
        }

        private void glControl1_MouseMove(object sender, MouseEventArgs e)
        {
            var pos = Viewport.WindowViewport(e.X, e.Y);
            Text = $"{pos.X} {pos.Y}";
        }

        /*  private void glControl1_Click(object sender, MouseEventArgs e)
          {
              if (e.Button == MouseButtons.Left)
                  LeftClick(sender, e);
              if (e.Button == MouseButtons.Right)
                  RightClick(sender, e);
          }
          /*
          private void LeftClick(object sender, MouseEventArgs e)
          {
              // Vytvareni a mazani vrcholu
              var pos = Viewport.WindowViewport(e.X, e.Y);

              var minDist = double.PositiveInfinity;
              var nearest = -1;

              // Najde nejblizsi vrchol
              for (var i = 0; i < Model.Vertices.Count; i++)
              {
                  var dist = Vector2.Distance(Model.Vertices[i].Position, pos);
                  if (dist < minDist)
                  {
                      minDist = dist;
                      nearest = i;
                  }
              }

              // Pokud je nejblizsi vrchol dost blizko, odeberu jej
              if (minDist < RemoveVertexDistance)
              {
                  // Odstraneni vrcholu
                  Model.Vertices.RemoveAt(nearest);

                  // Odstraneni trojuhelniku
                  for (var i = 0; i < Model.Triangles.Count; i++)
                  {
                      if (Model.Triangles[i].I1 == nearest || Model.Triangles[i].I2 == nearest || Model.Triangles[i].I2 == nearest)
                      {
                          Model.Triangles.RemoveAt(i);
                          i--;
                      }
                  }

                  // Precislovani vrcholu u kterych doslo ke zmene indexu
                  for (var i = 0; i < Model.Triangles.Count; i++)
                  {
                      if (Model.Triangles[i].I1 > nearest)
                          Model.Triangles[i].I1--;
                      if (Model.Triangles[i].I2 > nearest)
                          Model.Triangles[i].I2--;
                      if (Model.Triangles[i].I3 > nearest)
                          Model.Triangles[i].I3--;
                  }
              }
              else
              {
                  var newVertex = new Vertex(Viewport.WindowViewport(e.X, e.Y), RandomColor());
                  Model.Vertices.Add(newVertex);
              }

              Model.Changed = true;
          }
          /*
          private void RightClick(object sender, MouseEventArgs e)
          {
              // Spojovani vrcholu do trojuhelniku
              var pos = Viewport.WindowViewport(e.X, e.Y);

              var minDist = double.PositiveInfinity;
              var nearest = -1;

              // Najde nejblizsi vrchol
              for (var i = 0; i < Model.Vertices.Count; i++)
              {
                  var dist = Vector2.Distance(Model.Vertices[i].Position, pos);
                  if (dist < minDist)
                  {
                      minDist = dist;
                      nearest = i;
                  }
              }

              // Pokud je dost blizku, pridam do seznamu
              // Obsahuje-li seznam 3 vrcholy, spojim je do trojuhelniku
              if (minDist < RemoveVertexDistance)
              {
                  ClickedVertices.Add(nearest);
                  if (ClickedVertices.Count >= 3)
                  {
                      Model.Triangles.Add(new Triangle(ClickedVertices[0], ClickedVertices[1], ClickedVertices[2]));
                      ClickedVertices.Clear();
                      Model.Changed = true;
                  }
              }
          }
          */
        private ColorRGB RandomColor()
        {
            return new ColorRGB(Random.NextDouble(), Random.NextDouble(), Random.NextDouble());
        }

        private void glControl1_Load(object sender, EventArgs e)
        {

            Camera = new Camera(Viewport);

            // Vytvoreni modelu
            Model = new Model();
            Model.Vertices.Add(new Vertex(new Vector3(-0.9, -0.9, 0), new ColorRGB(1, 0, 0)));
            Model.Vertices.Add(new Vertex(new Vector3(0.9, 0.9, 0), new ColorRGB(1, 0, 0)));
            Model.Vertices.Add(new Vertex(new Vector3(-0.9, 0.9, 0), new ColorRGB(1, 1, 0)));
            Model.Vertices.Add(new Vertex(new Vector3(0.9, -0.9, 0), new ColorRGB(1, 1, 0)));
            Model.Triangles.Add(new Triangle(0, 1, 2));
            Model.Triangles.Add(new Triangle(1, 3, 0));


            GenerateSpherePoints(1000, 1.0f); // 
            Model.Changed = true;
            // Zobrazeni vrcholu a trojuhelniku v listboxech
            this.listBox1.DataSource = Model.Vertices;
            this.listBox2.DataSource = Model.Triangles;
            glControl1.MouseWheel += glControl1_MouseWheel;
        }


        private void GenerateSpherePoints(int count, float radius)
        {
            Model.Vertices.Clear(); // Vyčistí předchozí body

            for (int i = 0; i < count; i++)
            {
                double theta = Random.NextDouble() * 2 * Math.PI;  // Azimut
                double phi = Math.Acos(2 * Random.NextDouble() - 1); // Polární úhel

                float x = (float)(radius * Math.Sin(phi) * Math.Cos(theta));
                float y = (float)(radius * Math.Sin(phi) * Math.Sin(theta));
                float z = (float)(radius * Math.Cos(phi));

                Model.Vertices.Add(new Vertex(new Vector3(x, y, z), RandomColor()));
            }

            Model.Changed = true;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Model?.Dispose();
        }

        // Pri vyberu z listboxu se vlastnosti zobrazi v property gridu
        private void listBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            this.propertyGrid1.SelectedObject = this.listBox1.SelectedItem;
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.propertyGrid2.SelectedObject = this.listBox2.SelectedItem;
        }

        // Pri zmene vlastnosti v property gridu se model prekresli
        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            Model.Changed = true;
        }

        private void propertyGrid2_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            Model.Changed = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }



        private void glControl1_MouseWheel(object? sender, MouseEventArgs e)
        {
            if (e.Delta < 0)
            {
                Camera.Zoom(0.9f);
            }
            else
            {
                Camera.Zoom(1.1f);
            }
        }

        private void glControl1_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar) { 
                case 'a':
                    Camera.Move(-0.1f, 0);
                    break;
                case 'd':
                    Camera.Move(0.1f, 0);
                    break;

                case 'w':
                    Camera.Move(0, 0.1f);
                    break;

                case 's':
                    Camera.Move(0, -0.1f);
                    break;


            }
        }
    }
}
