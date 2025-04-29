using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using cv12;

public class MainWindow : GameWindow
{
    //private List<Line> lines = new List<Line>();

    //private List<BSplineCubic> lines = new List<BSplineCubic>();
    private int shaderProgram;

    private int selectedPointIndex = -1; 
    private bool isDragging = false;


    private List<Vector2> controlPoints = new List<Vector2>();


    public MainWindow(GameWindowSettings gws, NativeWindowSettings nws) : base(gws, nws) { }

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.ClearColor(0.2f, 0.2f, 0.2f, 1f);

        shaderProgram = ShaderHelper.CreateSimpleShader();
        GL.UseProgram(shaderProgram);

        //lines.Add(new Line(new Vector2(100, 100), new Vector2(700, 500), 20));
        //lines.Add(new BSplineCubic(new Vector2(10, 10), new Vector2(100, 100), new Vector2(200, 200), new Vector2(300, 10), 20));
        controlPoints.Add(new Vector2(10, 10));
        controlPoints.Add(new Vector2(100, 100));
        controlPoints.Add(new Vector2(200, 200));
        controlPoints.Add(new Vector2(300, 10));
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.Clear(ClearBufferMask.ColorBufferBit);

        GL.UseProgram(shaderProgram);
        /*
        foreach (var line in lines)
        {
            line.Draw();
            line.DrawControlPoints();
        }
        */

        if (IsKeyDown(Keys.Escape))
        {
            Close();
        }

        if (IsKeyPressed(Keys.Delete))
        {
            controlPoints.Clear();
        }



        for (int i = 0; i <= controlPoints.Count - 4; i++)
        {
            var spline = new BSplineCubic(
                controlPoints[i],
                controlPoints[i + 1],
                controlPoints[i + 2],
                controlPoints[i + 3],
                20
            );

            spline.Draw();
            spline.DrawControlPoints();
            spline.Dispose(); 
        }
        SwapBuffers();
    }


   

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
        if(e.Button == MouseButton.Left)
        {
            /*
            var lastLine = lines.Last();
            var points = lastLine.GetControlPoints();


           // var points = lastLine.GetControlPoints().Last();
           //var point2 = MousePosition;


            var newLine = new BSplineCubic(points[1], points[2], points[3], MousePosition, 10);

            lines.Add(newLine);
            */


            Vector2 mousePos = MousePosition;
            float threshold = 10f;

            // Zkusíme najít bod blízko myši
            for (int i = 0; i < controlPoints.Count; i++)
            {
                if ((controlPoints[i] - mousePos).Length < threshold)
                {
                    selectedPointIndex = i;
                    isDragging = true;
                    return;
                }
            }

                
            controlPoints.Add(MousePosition);
           

        }

        if (e.Button == MouseButton.Right)
        {
            Vector2 mousePos = MousePosition;
            float threshold = 10f;

            for (int i = 0; i < controlPoints.Count; i++)
            {
                if ((controlPoints[i] - mousePos).Length < threshold)
                {
                    if (controlPoints.Count > 4)
                    {
                        controlPoints.RemoveAt(i);
                    }
                    return;
                }
            }
        }

    }


    protected override void OnMouseMove(MouseMoveEventArgs e)
    {
        if (isDragging && selectedPointIndex != -1)
        {
            controlPoints[selectedPointIndex] = MousePosition;
        }
    }


    protected override void OnMouseUp(MouseButtonEventArgs e)
    {
        if (e.Button == MouseButton.Left && isDragging)
        {
            selectedPointIndex = -1;
            isDragging = false;
        }
    }

    /*
    protected override void OnUnload()
    {
        base.OnUnload();

        foreach (var line in lines)
            line.Dispose();

        GL.DeleteProgram(shaderProgram);
    }
    */
}

