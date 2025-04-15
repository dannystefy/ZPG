
using System.Numerics;
namespace PathTracer
{
    class Program()
    {
        public static void Main(string[] args)
        {
            DateTime start = DateTime.Now;
            Scene scene = new Scene();
            scene.objects.Add(new InfinitePlane(new Vector3(0, -1, 0), new Vector3(0, 0, 1), new Vector3(1, 0, 0)));
            scene.objects.Add(new PlanarLight(new Vector3(-2, -1, 6), new Vector3(-2, 0, 2), new Vector3(0, 2, 0)));
            scene.objects.Add(new MeshWithBVH("cow.obj", new Vector3(0, 0, 8), 0.3f));
            LambertianBRDF brdf = new LambertianBRDF();
            foreach (var obj in scene.objects)
            {
                obj.brdf = brdf;
            }

            Camera c = new Camera(800, 600, 30);
            c.Render(scene, "image.png");
            var span = DateTime.Now - start;
            Console.WriteLine(span.TotalMilliseconds);
        }
    }
}