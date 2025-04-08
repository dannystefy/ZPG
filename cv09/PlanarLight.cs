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

        public override Vector3 emitingRadiance(Vector3 position, Vector3 direction)
        {
            return radiance; 
        }

        public override Vector3[] GetLocalCoordinateSystem(Vector3 surfacePoint)
        {
            return new Vector3[] { Vector3.Cross(v1, v2), v1, v2 };
        }

        public override float Intersect(Ray r)
        {
            Vector3 rns = r.origin - this.point;
            float solution = MathUtils.Determinant(v1, v2, rns) / MathUtils.Determinant(v1, v2, -r.direction);
            float t1 = MathUtils.Determinant(v1, rns, -r.direction) / MathUtils.Determinant(v1, v2, -r.direction);
            float t2 = MathUtils.Determinant(rns, v2, -r.direction) / MathUtils.Determinant(v1, v2, -r.direction);


            if (t1 < 0 || t1 > 1 || t2 < 0 || t2 > 1) return float.NaN;
         
            return solution;
            
        }
    }
}
