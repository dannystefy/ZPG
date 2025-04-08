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

        public override Vector3[] GetLocalCoordinateSystem(Vector3 surfacePoint)
        {
            return new Vector3[] {Vector3.Cross(t1, t2), t1, t2 };  
        }

        public override float Intersect(Ray r)
        {
            Vector3 rns = r.origin - this.point;
            float solution = MathUtils.Determinant(t1, t2, rns) / MathUtils.Determinant(t1, t2, -r.direction);
            return solution;
        }
    }
}
