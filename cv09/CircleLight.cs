using System;
using System.Numerics;

namespace PathTracer
{
    public class CircularLight : SceneObject
    {
        private Vector3 center;
        private Vector3 normal;
        private Vector3 v1;  
        private Vector3 v2;  
        private float radius;
        public Vector3 radiance = new Vector3(1, 1, 1);

        public CircularLight(Vector3 center, Vector3 normal, float radius)
        {
            this.center = center;
            this.normal = Vector3.Normalize(normal);
            this.radius = radius;

        }
        public override Vector3 emitingRadiance(Vector3 position, Vector3 direction)
        {
            return radiance;
        }
        public override Vector3[] GetLocalCoordinateSystem(Vector3 surfacePoint)
        {
            return new Vector3[] { normal, v1, v2 };
        }

        public override float Intersect(Ray r)
        {
            
            float denom = Vector3.Dot(normal, r.direction);
            if (Math.Abs(denom) < 1e-6f) return float.NaN;

            float t = Vector3.Dot(center - r.origin, normal) / denom;
            if (t < 0) return float.NaN;

            Vector3 hit = r.origin + t * r.direction;
            Vector3 d = hit - center;

            
            if (d.LengthSquared() <= radius * radius)
                return t;

            return float.NaN;
        }

    }
}
