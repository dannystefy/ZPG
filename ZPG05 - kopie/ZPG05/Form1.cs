using OpenTK.Graphics.OpenGL;

namespace ZPG05
{
    public partial class Form1 : Form
    {
        public Cube Cube1 { get; set; }
        public Cube Cube2 { get; set; }
        public Cube Cube3 { get; set; }

        public Viewport Viewport { get; set; }
        public Viewport MiniMapViewport { get; set; }

        public Camera Camera { get; set; }
        public OrthoCamera MiniMapCamera { get; set; }

        public Form1()
        {
            InitializeComponent();

            Viewport = new Viewport
            {
                Top = 0,
                Left = 0,
                Width = 1,
                Height = 1,
                Control = glControl
            };

            MiniMapViewport = new Viewport
            {
                Top = 0.1f,
                Left = 0.1f,
                Height = 0.2f,
                Width = 0.2f,
                Control = glControl
            };

            Application.Idle += Application_Idle;
        }

        private void Application_Idle(object? sender, EventArgs e)
        {
            glControl.Invalidate();
        }

        private void glControl_Paint(object sender, PaintEventArgs e)
        {
            glControl.MakeCurrent();

            // Render scene
            Viewport.Set();
            Viewport.Clear();

           // Camera.SetProjection();
           //Camera.SetView();

            GL.ClearColor(Color.White);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.PointSize(10);

            Cube1.Draw(Camera);
            Cube2.Draw(Camera);
            Cube3.Draw(Camera);

            // Render minimap
            MiniMapViewport.Set();
            MiniMapViewport.Clear();

           // MiniMapCamera.SetProjection();
           // MiniMapCamera.SetView();

            GL.ClearColor(Color.LightGray);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            Cube1.Draw(MiniMapCamera);
            Cube2.Draw(MiniMapCamera);
            Cube3.Draw(MiniMapCamera);

            // Camera position
            GL.Begin(BeginMode.Points);
            GL.Color3(Color.Blue);
            GL.Vertex3(Camera.x, Camera.y, Camera.z);
            GL.End();

            glControl.SwapBuffers();
            glControl.Focus();
        }

        private void glControl_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

            }
            else if (e.Button == MouseButtons.Right)
            {

            }

            var pos2 = Viewport.WindowViewport(e.X, e.Y);
        }

        private void glControl_Load(object sender, EventArgs e)
        {

            var shader = new Shader("Shaders/basic.vert", "Shaders/basic.frag");

            Camera = new Camera(Viewport);
            MiniMapCamera = new OrthoCamera(MiniMapViewport)
            {
                scale = 10,
                rx = MathF.PI / 2,
                y = 10
            };

            Cube1 = new Cube();
            Cube2 = new Cube
            {
                Position = new Vector3(0, -2, -2)
            };
            Cube3 = new Cube
            {
                Position = new Vector3(0, 2, 2)
            };


            Cube1.Shader = shader;
            Cube2.Shader = shader;  
            Cube3.Shader = shader;  

            glControl.MouseWheel += glControl_MouseWheel;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Cube1?.Dispose();
            Cube2?.Dispose();
            Cube3?.Dispose();
        }

        private void glControl_MouseWheel(object? sender, MouseEventArgs e)
        {
            Camera.Zoom(e.Delta < 0 ? 0.9f : 1.1f);
        }

        private void glControl_MouseMove(object sender, MouseEventArgs e)
        {
            var mid = new Point(glControl.Width / 2, glControl.Height / 2);
            Camera.RotateY((e.X - mid.X) / 250f);
            Camera.RotateX((e.Y - mid.Y) / 250f);
            if (e.X != mid.X || e.Y != mid.Y)
                Cursor.Position = glControl.PointToScreen(mid);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.A: Camera.Move(-0.1f, 0); break;
                case Keys.D: Camera.Move(0.1f, 0); break;
                case Keys.W: Camera.Move(0, 0.1f); break;
                case Keys.S: Camera.Move(0, -0.1f); break;
                case Keys.Escape: Application.Exit(); break;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
