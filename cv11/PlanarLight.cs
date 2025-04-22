using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PathTracer
{
    /// <summary>
    /// planar area light. Light emited from points x = point + t*v1 + s*v2, 0<t<1, 0<s<1
    /// </summary>
    public class PlanarLight : SceneObject
    {
        private Vector3 point;
        private Vector3 v1;
        private Vector3 v2;
        public Vector3 radiance = new Vector3(1, 1, 1);

        public PlanarLight(Vector3 point, Vector3 v1, Vector3 v2)
        {
            this.point = point;
            this.v1 = v1;
            this.v2 = v2;
        }

        public override Vector3 emitingRadiance(HitPoint position, Vector3 direction)
        {
            return radiance; 
        }

        public override Vector3[] GetLocalCoordinateSystem(HitPoint surfacePoint)
        {
            return new Vector3[] { Vector3.Cross(v1, v2), v1, v2};
        }

        public override HitPoint Intersect(Ray r)
        {
            Vector3 rhs = r.origin - this.point;
            float solution = determinant(v1, v2, rhs) / determinant(v1, v2, -r.direction);
            float p1 = determinant(v1, rhs, -r.direction) / determinant(v1, v2, -r.direction);
            float p2 = determinant(rhs, v2, -r.direction) / determinant(v1, v2, -r.direction);
            if (p1 < 0) return null;
            if (p2 < 0) return null;
            if (p1 > 1) return null;
            if (p2 > 1) return null;
            return new HitPoint(solution);
        }

        float determinant(Vector3 c1, Vector3 c2, Vector3 c3)
        {
            return c1.X * c2.Y * c3.Z + c1.Y * c2.Z * c3.X + c1.Z * c2.X * c3.Y - c1.Z * c2.Y * c3.X - c1.X * c2.Z * c3.Y - c1.Y * c2.X * c3.Z;
        }
    }
}
