using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using static OpenTK.Graphics.OpenGL.GL;



/// <summary>
/// Struktura popisující vrchol, který má souřadnici a barvu.
/// StructLayout říká, jak budou data v paměti. Pack = 1 znamená, že se nebude paměť zarovnávat do větších bloků, 
/// což by mohlo být důležité, pokud použijeme nějaké kratší datové typy.
/// </summary>
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct VertexColor
{
    public Vector3 position;
    public Vector3 color;

    /// <summary>
    /// Zjištění velikosti struktury
    /// </summary>
    public static int SizeInBytes { get => Marshal.SizeOf(typeof(VertexColor)); }
}

/// <summary>
/// Objekt 
/// </summary>
public class MyObject
{
    VertexColor[] vertices;
    Vector3i[] triangles;
    int vbo, ibo;

    /// <summary>
    /// Naplpnění daty
    /// </summary>
    public MyObject()
    {
        vertices = new VertexColor[6];
        triangles = new Vector3i[3];

        for (int i = 0; i < 3; i++)
        {
            vertices[2 * i] = new()
            {
                position = new Vector3(-1 * MathF.Sin(i / 3f * 2 * MathF.PI), 1 * MathF.Cos(i / 3f * 2 * MathF.PI), 0),
                color = new Vector3(1, 1, 1)
            };
            vertices[2 * i + 1] = new()
            {
                position = new Vector3(0.5f * MathF.Sin(((i + 2) % 3) / 3f * 2 * MathF.PI), -0.5f * MathF.Cos(((i + 2) % 3) / 3f * 2 * MathF.PI), 0),
                color = new Vector3(1, 1, 0)
            };
        }

        triangles[0] = new(0, 1, 5);
        triangles[1] = new(1, 2, 3);
        triangles[2] = new(3, 4, 5);

        // vygenerování opengl bufferu a vrácení jeho id
        vbo = GL.GenBuffer();
        // připojení bufferu z předchozího kroku
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        // nahrání dat: počet vrcholů * velikost jednoho, StaticDraw radí ovladači, že se s daty nebude nic dít
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * VertexColor.SizeInBytes, vertices, BufferUsageHint.StaticDraw);

        // vygenerování bufferu pro konektivitu
        ibo = GL.GenBuffer();
        // připojení bufferu pro konektivitu
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo);
        // nahrání dat
        GL.BufferData(BufferTarget.ElementArrayBuffer, triangles.Length * Vector3i.SizeInBytes, triangles, BufferUsageHint.StaticDraw);
    }


    /// <summary>
    /// Vykreslení objektu
    /// </summary>
    public void Draw()
    {
        // připojení bufferů
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo);

        // povolení pozic a namapování dat (časem změníme)
        GL.EnableClientState(ArrayCap.VertexArray);
        GL.VertexPointer(3, VertexPointerType.Float, VertexColor.SizeInBytes, IntPtr.Zero);

        // povolení barev a namapování dat (časem změníme)
        GL.EnableClientState(ArrayCap.ColorArray);
        GL.ColorPointer(3, ColorPointerType.Float, VertexColor.SizeInBytes, (IntPtr)(3 * sizeof(float)));

        // vykreslení vrcholů (většinou nechceme)
        GL.DrawArrays(PrimitiveType.Points, 0, 6);

        // vykrelsení primitiv (v našem případě trojúhelníků). POZOR! count je počet primitiv krát počet vrcholů
        GL.DrawElements(PrimitiveType.Triangles, 3 * triangles.Length, DrawElementsType.UnsignedInt, IntPtr.Zero);
    }
}

class Zpg : GameWindow
{
    MyObject m;

    public Zpg(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
    {
    }

    protected override void OnLoad()
    {
        base.OnLoad();
        m = new MyObject();

        // toto je praktická záležitost, povolení ladicích hlášek z opengl systému
        GL.Enable(EnableCap.DebugOutput);
        GL.Enable(EnableCap.DebugOutputSynchronous);
        // DebugCallback
        GL.DebugMessageCallback(DebugCallback, IntPtr.Zero);
    }

    /// <summary>
    /// Metoda zpracovávající debugovací hlášky
    /// </summary>
    /// <param name="source">Zdroj zprávy (např. API, Shader, Aplikace...)</param>
    /// <param name="type">Typ zprávy (chyba, varování, informace...)</param>
    /// <param name="id">Id zprávy</param>
    /// <param name="severity">Závažnost zprávy (high, medium, low, notification)</param>
    /// <param name="length">Délka zprávy</param>
    /// <param name="message">Ukazatel na text zprávy</param>
    /// <param name="userParam">Uživatelský parametr (často nepoužito)</param>
    private static void DebugCallback(DebugSource source, DebugType type, int id, DebugSeverity severity, int length, IntPtr message, IntPtr userParam)
    {
        // převod textu zprávy na string
        string msg = System.Runtime.InteropServices.Marshal.PtrToStringAnsi(message, length);
        Console.WriteLine($"OpenGL Debug Message: {msg}\nSource: {source}, Type: {type}, Severity: {severity}, ID: {id}");
    }


    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);
        GL.ClearColor(0, 0, 0.1f, 0);               // barva mazání
        GL.Clear(ClearBufferMask.ColorBufferBit);   // vymazání paměti předchozího snímku
        GL.PointSize(9);                            // velikost bodu
        m.Draw();                                   // vykreslení objektu
        SwapBuffers();                              // zobrazení připravené paměti
    }

    public static void Main()
    {
        GameWindowSettings gws = GameWindowSettings.Default;
        NativeWindowSettings nws = new NativeWindowSettings()
        {
            // Kompatibility profil umožňuje využívat starý neefektivní, ale jednoduchý způsob interkace s OpenGL
            Profile = OpenTK.Windowing.Common.ContextProfile.Compatability,
            Title = "ZPG",
            ClientSize = new OpenTK.Mathematics.Vector2i(800, 500),
            WindowState = WindowState.Fullscreen
        };
        var zpg = new Zpg(gws, nws);
        zpg.Run();
    }
}