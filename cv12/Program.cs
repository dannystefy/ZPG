using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;

var nativeWindowSettings = new NativeWindowSettings()
{
    ClientSize = new Vector2i(800, 600),
    Title = "Křivky",
};

using var window = new MainWindow(GameWindowSettings.Default, nativeWindowSettings);
window.Run();
