using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using cv13;

public class MainWindow : GameWindow
{
    private int cellShader;
    Matrix4 projection;
    Matrix4 view;
    Matrix4 model;
    MarchingCubes mc;
    IsoCells cells;
    int mvpLoc;

    public MainWindow(GameWindowSettings gws, NativeWindowSettings nws) : base(gws, nws) { }


    public float[] GenerateSphere(int samples)
    {
        float c = 0.5f;
        float[] data = new float[samples*samples*samples];
        for (int k = 0; k < samples; k++)
        {
            for (int j = 0; j < samples; j++)
            {
                for(int i = 0; i < samples; i++)
                {
                    float nk = (float)k / samples;
                    float nj = (float)j / samples;
                    float ni = (float)i / samples;
                    data[(k * samples + j) * samples + i] = MathF.Sqrt((nk-c)*(nk-c) + (nj-c)*(nj-c) + (ni-c)*(ni-c));
                }
            }
        }
        return data;
    }



    public float[] GenerateTorus(int samples)
    {
        float c = 0.5f;   
        float R = 0.30f;  
        float r = 0.005f;  

        float[] data = new float[samples * samples * samples];
        for (int k = 0; k < samples; k++)
            for (int j = 0; j < samples; j++)
                for (int i = 0; i < samples; i++)
                {
                    float nk = (float)k / (samples - 1);
                    float nj = (float)j / (samples - 1);
                    float ni = (float)i / (samples - 1);

                    float x = ni - c;
                    float y = nj - c;
                    float z = nk - c;

                    float dXZ = MathF.Sqrt(x * x + z * z);

                    float q = dXZ - R;

                 
                    int idx = (k * samples + j) * samples + i;
                    data[idx] = MathF.Sqrt(q * q + y * y) - r;
                }

        return data;
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.ClearColor(0,0,0, 1f);

        cellShader = ShaderHelper.CreateCellShader();
        mvpLoc = GL.GetUniformLocation(cellShader, "mvp");
        projection = Matrix4.CreatePerspectiveFieldOfView(float.Pi/3, (float)ClientSize.X / ClientSize.Y, 0.01f, 3);

        GL.Enable(EnableCap.DebugOutput);
        GL.Enable(EnableCap.DebugOutputSynchronous);
        GL.DebugMessageCallback(DebugCallback, IntPtr.Zero);

        var res = 25;
        var data = GenerateTorus(res);
        cells = new IsoCells(data, res, res, 0.4f);
        mc = new MarchingCubes(data, res, res, 0.1f);

    }

    private static void DebugCallback(DebugSource source, DebugType type, int id, DebugSeverity severity, int length, IntPtr message, IntPtr userParam)
    {
        string msg = System.Runtime.InteropServices.Marshal.PtrToStringAnsi(message, length);
        Console.WriteLine($"OpenGL Debug Message: {msg}\nSource: {source}, Type: {type}, Severity: {severity}, ID: {id}");
    }

    float angle = 0;

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);
        
        view = Matrix4.CreateTranslation(0, 0, -1.5f);
        view = Matrix4.CreateRotationY(angle) * view;
        angle += 0.0001f;

        Matrix4 mvp;
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        GL.UseProgram(cellShader);
        model = Matrix4.CreateTranslation(-0.5f,-0.5f,-0.5f);
        mvp = model * view * projection;
        GL.UniformMatrix4(mvpLoc, false, ref mvp);
        mc.Draw();
        //cells.Draw();

        SwapBuffers();
    }

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
    }

    protected override void OnUnload()
    {
        base.OnUnload();
    }
}

