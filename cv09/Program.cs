
using System.Numerics;
namespace PathTracer
{
    class Program()
    {
        public static void Main(string[] args)
        {
            Scene scene = new Scene();
            scene.objects.Add(new InfinitePlane(new Vector3(0, -1, 0), new Vector3(0, 0, 1), new Vector3(1, 0, 0)));
            scene.objects.Add(new Sphere(new Vector3(-0.55f, -0.6f, 9), 0.5f));
            //scene.objects.Add(new PlanarLight(new Vector3(-1, -1, 10), new Vector3(2, 0, 0), new Vector3(0, 2, -1)));
//            scene.objects.Add(new CircularLight(new Vector3(0, 5, 10), new Vector3(0, -1, 0), 2.0f));

            scene.objects.Add(
                new CircularLight(
                    new Vector3(0, 0f, 10f),    
                    new Vector3(0, 0, 1),           
                    1f)
            );


            LambertianBRDF brdf = new LambertianBRDF();
            foreach (var obj in scene.objects)
            {
                obj.brdf = brdf;
            }

            var sph = new Sphere(new Vector3(0.55f, -0.6f, 8), 0.5f);
            LambertianBRDF blue = new LambertianBRDF();
            blue.albedo = new Vector3(0.0f, 1.0f, 0.0f);
            sph.brdf = blue;
            scene.objects.Add(sph);

            Camera c = new Camera(800, 600, 30);
            c.Render(scene, "image.png");
        }
    }
}