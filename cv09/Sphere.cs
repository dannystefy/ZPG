using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PathTracer
{
    public class Sphere : SceneObject
    {
        public Vector3 center;
        public float radius;
        public Sphere(Vector3 center, float radius)
        {
            this.center = center;
            this.radius = radius;
        }

        public override Vector3[] GetLocalCoordinateSystem(Vector3 surfacePoint)
        {
            Vector3 normal = Vector3.Normalize(surfacePoint - center);
            Vector3 t1 = Vector3.Normalize(new Vector3(normal.Z, 0, -normal.X));
            Vector3 t2 = Vector3.Cross(normal, t1);
            return new Vector3[] { normal, t1, t2 };

        }

        public override float Intersect(Ray r)
        {
            Vector3 p = r.origin - center;
            float a = Vector3.Dot(r.direction, r.direction);
            float b = 2 * Vector3.Dot(p, r.direction);
            float c = Vector3.Dot(p, p) - radius * radius;
            float d = b * b - 4 * a * c;

            if (d < 0) return float.NaN;

            float solution = (-b - (float)Math.Sqrt(d)) / (2 * a);
            
            return solution;
        
        }
    }
}
