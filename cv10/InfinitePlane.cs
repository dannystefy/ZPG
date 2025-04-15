using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace PathTracer
{
    public class InfinitePlane : SceneObject
    {
        public Vector3 point, t1, t2;
        public InfinitePlane(Vector3 point, Vector3 t1, Vector3 t2)
        {
            this.point = point;
            this.t1 = t1;
            this.t2 = t2;
        }

        public override Vector3[] GetLocalCoordinateSystem(HitPoint surfacePoint)
        {
            return new Vector3[] {Vector3.Cross(t1,t2), t1, t2};
        }

        public override HitPoint Intersect(Ray r)
        {
            Vector3 rhs = r.origin - this.point;
            float solution = determinant(t1, t2, rhs) / determinant(t1, t2, -r.direction);
            return new HitPoint(solution);
        }

        float determinant(Vector3 c1, Vector3 c2, Vector3 c3)
        {
            return c1.X * c2.Y * c3.Z + c1.Y * c2.Z * c3.X + c1.Z * c2.X * c3.Y - c1.Z * c2.Y * c3.X - c1.X * c2.Z * c3.Y - c1.Y * c2.X * c3.Z;
        }
    }
}
