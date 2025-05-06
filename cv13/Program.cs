using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;

var nativeWindowSettings = new NativeWindowSettings()
{
    ClientSize = new Vector2i(1400, 1400),
    Title = "Marching Cubes",
};

using var window = new MainWindow(GameWindowSettings.Default, nativeWindowSettings);
window.Run();
