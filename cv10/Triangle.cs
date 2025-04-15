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
        // points
        public Vector3 p1, p2, p3;

        // edge vectors
        private Vector3 v1, v2;

        // tangent space (orthonormal)
        private Vector3[] lcs;

        public Triangle(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            this.p1 = p1;
            this.p2 = p2;
            this.p3 = p3;

            this.v1 = p2-p1;
            this.v2 = p3-p1;

            var v1n = Vector3.Normalize(v1);
            var n = Vector3.Normalize(Vector3.Cross(v1, v2));
            var v2n = Vector3.Cross(n, v1n);
            lcs = new Vector3[3] { n, v1n, v2n };
        }

        public override Vector3 emitingRadiance(HitPoint position, Vector3 direction)
        {
            return new Vector3(0,0,0); 
        }

        public override Vector3[] GetLocalCoordinateSystem(HitPoint surfacePoint)
        {
            return lcs;
        }

        public override HitPoint Intersect(Ray r)
        {
            Vector3 rhs = r.origin - this.p1;
            float d = determinant(v1, v2, -r.direction);
            float solution = determinant(v1, v2, rhs) / d;
            float p1 = determinant(v1, rhs, -r.direction) / d;
            float p2 = determinant(rhs, v2, -r.direction) / d;
            if (p1 < 0) return new HitPoint(float.NaN);
            if (p2 < 0) return new HitPoint(float.NaN);
            if ((p1+p2)>1) return new HitPoint(float.NaN);
            return new HitPoint(solution);
        }

        float determinant(Vector3 c1, Vector3 c2, Vector3 c3)
        {
            return c1.X * c2.Y * c3.Z + c1.Y * c2.Z * c3.X + c1.Z * c2.X * c3.Y - c1.Z * c2.Y * c3.X - c1.X * c2.Z * c3.Y - c1.Y * c2.X * c3.Z;
        }
    }
}
