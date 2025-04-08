using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PathTracer
{
    public class Triangle : SceneObject
    {
        public Vector3 p1;
        public Vector3 p2;
        public Vector3 p3;
        public Vector3 center;
        public Triangle(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            this.p1 = p1;
            this.p2 = p2;
            this.p3 = p3;
            this.center = (p1 + p2 + p3) / 3;

        }


        public override Vector3[] GetLocalCoordinateSystem(Vector3 surfacePoint)
        {
            Vector3 normal = Vector3.Normalize(Vector3.Cross(p2 - p1, p3 - p1));
            Vector3 t1 = Vector3.Normalize(new Vector3(normal.Z, 0, -normal.X));
            Vector3 t2 = Vector3.Cross(normal, t1);
            return new Vector3[] { normal, t1, t2 };
        }
        public override float Intersect(Ray r)
        {
            Vector3 p = r.origin - center;


            return float.NaN; // Placeholder
        }
    }
}
