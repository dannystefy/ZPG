using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Zpg;

public class Program : GameWindow
{    
    public static void Main() => new Program().Run();

    Viewport mainViewport;
    Camera mainCamera;
    Light light;
    List<Model> models = new(); 


    public Program() : base(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize=new OpenTK.Mathematics.Vector2i(600,600) }) { }

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.Enable(EnableCap.DebugOutput);
        GL.Enable(EnableCap.DebugOutputSynchronous);
        GL.DebugMessageCallback(DebugCallback, IntPtr.Zero);
        GL.Enable(EnableCap.DepthTest);

        CursorState = CursorState.Grabbed;

        mainViewport = new Viewport()
        {
            Left = 0,
            Top = 0,
            Width = 1,
            Height = 1,
            window = this
        };

        var model = new Model("Earth.obj");
        model.position = new Vector3(0,1,0);
        model.Shader = new Shader("basic.vert", "basic.frag");       
        model.Material = new Material() { diffuse = new Vector3(0.5f, 0.5f, 0.5f), specular = new Vector3(0.0f, 0.0f, 0.0f), shininess = 50 };
        
        model.Textures.Add("dayTex", new Texture("EarthDay.jpg", 0));
        model.Textures.Add("nightTex", new Texture("EarthNight.jpg", 1));
        models.Add(model);



        mainCamera = new Camera(mainViewport);
        light = new Light(new Vector3(-1, -1, -1), true);
    }

    private static void DebugCallback(DebugSource source, DebugType type, int id, DebugSeverity severity, int length, IntPtr message, IntPtr userParam)
    {
        string msg = System.Runtime.InteropServices.Marshal.PtrToStringAnsi(message, length);
        Console.WriteLine($"OpenGL Debug Message: {msg}\nSource: {source}, Type: {type}, Severity: {severity}, ID: {id}");
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        mainViewport.Set();
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        foreach (var model in models)
        {
            model.Draw(mainCamera, light);
        }
        SwapBuffers();
    }

    float timetotal  = 0;
    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        Vector2 direction = new Vector2(0, 0);

        if (KeyboardState.IsKeyDown(Keys.W)) direction += Vector2.UnitY;
        if (KeyboardState.IsKeyDown(Keys.S)) direction -= Vector2.UnitY;
        if (KeyboardState.IsKeyDown(Keys.A)) direction -= Vector2.UnitX;
        if (KeyboardState.IsKeyDown(Keys.D)) direction += Vector2.UnitX;
        if (KeyboardState.IsKeyDown(Keys.Escape)) Close();

        if (KeyboardState.IsKeyPressed(Keys.Q))
        {
            WindowState = WindowState == WindowState.Fullscreen ? WindowState.Normal : WindowState.Fullscreen;
        }

        Console.WriteLine(light.position);

        var len = direction.Length;
        if (len > 0)
        {
            direction *= (float)args.Time/len;
            mainCamera.Move(direction.X, direction.Y);
        }

        if(mouseDelta.LengthSquared > 0.00001)
        {
            mainCamera.RotateX(mouseDelta.Y*(float)args.Time);
            mainCamera.RotateY(mouseDelta.X*(float)args.Time);
            mouseDelta = Vector2.Zero;
        }
    }

    Vector2 mouseDelta;
    protected override void OnMouseMove(MouseMoveEventArgs e)
    {
        mouseDelta += new Vector2(e.DeltaX, e.DeltaY);
    }
}