using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PathTracer
{
    /// <summary>
    /// Camera. Sits at (0,0,0), looks towards (0, 0, 1). Left handed coordinate system.
    /// </summary>
    public class Camera
    {
        // resolution
        public int pixelWidth, pixelHeight;

        // horizontal field of view in degrees
        public double fovX; 
        public Camera(int w, int h, double fovX)
        {
            this.pixelWidth = w;
            this.pixelHeight = h;
            this.fovX = fovX;
        }

        /// <summary>
        /// renders image at set resolution by tracking primary rays into scene
        /// </summary>
        /// <param name="scene">scene to render</param>
        /// <param name="fileName">image file to write</param>
        internal void Render(Scene scene, string fileName)
        {
            // resulting radiances will be stored here
            Vector3[,] frameBuffer = new Vector3[pixelWidth, pixelHeight];

            // world space width and height of image plane.
            float width = (float)(2 * Math.Tan(fovX / 180 * Math.PI / 2));
            float height = width/pixelWidth*pixelHeight;

            // world spacepixel size
            float pixelSize = width / pixelWidth;

            for (int x = 0; x < pixelWidth; x++)
            {
                for (int y = 0; y < pixelHeight; y++)
                {
                    float dirX = -width/2 + x * pixelSize;
                    float dirY = height/2 - y * pixelSize;

                    // primary ray
                    Ray r = new Ray(new Vector3(0, 0, 0), new Vector3(dirX, dirY, 1), 0);
                    Vector3 radiance = r.TraceScene(scene);
                    frameBuffer[x, y] = radiance;
                }
                Console.Write("*");
            }

            // maximum radiance for normalization
            Vector3 max = new Vector3(0, 0, 0);
            foreach (Vector3 v in frameBuffer)
            {
                max = Vector3.Max(max, v);
            }

            // maximum over channels
            float totMax = Math.Max(max.X, max.Y);
            totMax = Math.Max(totMax, max.Z);

            // writing image
            var image = new Image<Rgba32>(pixelWidth, pixelHeight);

            // multiplier for correcting brightness. Set m>1 when light sources are directly visible.
            float m = 2;
            for (int x = 0; x < pixelWidth; x++)
                for (int y = 0; y < pixelHeight; y++)
                {
                    // normalized brightness
                    Vector3 pixel = frameBuffer[x, y] / totMax*m;

                    // gamma correction
                    pixel.X = (float)Math.Pow(pixel.X, 1 / 2.2);
                    pixel.Y = (float)Math.Pow(pixel.Y, 1 / 2.2);
                    pixel.Z = (float)Math.Pow(pixel.Z, 1 / 2.2);

                    // brightness shift
                    pixel *= m;

                    // clamping
                    Rgba32 c = new Rgba32(Math.Min(1, pixel.X),
                        Math.Min(1, pixel.Y),
                        Math.Min(1, pixel.Z));

                    // pixel output
                    image[x, y] = c;
                }
            image.Save(fileName);
        }
    }
}
