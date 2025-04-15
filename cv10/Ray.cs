using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PathTracer
{
    public class Ray
    {
        public Vector3 origin;
        public Vector3 direction;
        public int depth;

        static Random rnd = new Random(0);

        static float intersectMinDist = 0.0001f;

        public Ray(Vector3 origin, Vector3 direction, int depth)
        {
            this.origin = origin;
            this.direction = Vector3.Normalize(direction);
            this.depth = depth;
        }

        public Vector3 TraceScene(Scene s)
        {
            if (depth > 2)
            {
                return (new Vector3(0, 0, 0));
            }

            SceneObject nearestObject = null;
            HitPoint nearestHit = new HitPoint(float.MaxValue);
            foreach (SceneObject o in s.objects)
            {
                HitPoint hp = o.Intersect(this);
                if (hp!=null)
                {
                    if (hp.d > intersectMinDist)
                    {
                        if (hp.d < nearestHit.d)
                        {
                            nearestHit = hp; nearestObject = o;
                        }
                    }
                }
            }
            if (nearestObject == null)
            {
                return new Vector3(0, 0, 0);
            }
            else
            {
                // found intersection
                Vector3 intersection = this.origin + nearestHit.d * this.direction;
                nearestHit.point = intersection;
                return nearestObject.GetRadiance(nearestHit, -direction, s, this.depth);
            }
        }
    }
}
