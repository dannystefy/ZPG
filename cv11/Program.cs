
using System.Numerics;
namespace PathTracer
{
    class Program()
    {
        public static void Main(string[] args)
        {
            DateTime start = DateTime.Now;
            Scene scene = new Scene();
            scene.objects.Add(new PlanarLight(new Vector3(-2, -1, 4), new Vector3(-2, 0, 2), new Vector3(0, 2, 0)));
            //scene.objects.Add(new MeshWithBVH("cow.obj", new Vector3(0, 0.05f, 5), 0.15f));
            LambertianBRDF brdf = new LambertianBRDF(1, 1, 1);
            foreach (var obj in scene.objects)
            {
                obj.brdf = brdf;
            }

            InfinitePlane plane = new InfinitePlane(new Vector3(0, -0.5f, 0), new Vector3(0, 0, 1), new Vector3(1, 0, 0));
            plane.brdf = new LambertianCheckerBoardBRDF(1, 1, 1, 0.1f, 0.1f, 0.1f);
            scene.objects.Add(plane);

            //scene.objects.Add(new Sphere(new Vector3(-0.5f, 0, 6), 0.5f, new LambertianBRDF(1, 0, 0)));
            //scene.objects.Add(new Sphere(new Vector3(0, 0, 5), 0.5f, new LambertianBRDF(0, 1, 0)));
            //scene.objects.Add(new Sphere(new Vector3(0.5f, 0, 4), 0.5f,new LambertianBRDF(0, 0, 1)));


            CameraWithLens c = new CameraWithLens(800, 600, 30);
            c.Render(scene, "image.png");
            var span = DateTime.Now - start;
            Console.WriteLine("\n" + span.TotalMilliseconds);
        }
    }
}