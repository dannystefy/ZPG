using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PathTracer
{
    public abstract class SceneObject
    {
        public BRDF brdf;

        // number of samples used for incomming radiance integration
        static int sampleCount = 10;

        // shared random number generator
        static Random r = new Random(0);
        abstract public HitPoint Intersect(Ray r);

        /// <summary>
        /// returns radiance emitted from a point in a direction.
        /// </summary>
        /// <param name="hitPoint">point on surface</param>
        /// <param name="direction">direction from the surface</param>
        /// <param name="s">scene to track incomming radiance</param>
        /// <param name="depth">depth of ray. Used for early termination</param>
        /// <returns></returns>
        virtual public Vector3 GetRadiance(HitPoint hitPoint, Vector3 direction, Scene s, int depth)
        {
            // outgoing direction must be converted to local coordinate system
            var lcs = GetLocalCoordinateSystem(hitPoint);
            Vector3 outDirectionLCS = new Vector3(Vector3.Dot(lcs[0], direction),
                Vector3.Dot(lcs[1], direction),
                Vector3.Dot(lcs[2], direction));

            // uniform random sampling of hemisphere
            Vector3[] outRadiance = new Vector3[sampleCount];
            for (int i = 0; i < sampleCount; i++)
            {
                Vector3 inDirectionLCS = RandomDirection(); // random direction in local coordinates

                // convert to world coordinates
                Vector3 inDirectionWCS = lcs[0] * inDirectionLCS.X + lcs[1] * inDirectionLCS.Y + lcs[2] * inDirectionLCS.Z;

                // trace incomming ray
                Ray r = new Ray(hitPoint.point, inDirectionWCS, depth+1);
                Vector3 inRadiance = r.TraceScene(s);

                // ask brdf what is the reflectance from inDirection towards outDirection (three components, RGB)
                Vector3 reflectance = brdf.Value(inDirectionLCS, outDirectionLCS);

                // rendering equation. Thank you Mr. Kajiya
                outRadiance[i] = reflectance * Vector3.Dot(lcs[0], inDirectionWCS) * inRadiance;
            }

            Vector3 totalOutRadiance = new Vector3(0,0,0);
            for (int i = 0;i<sampleCount;i++)
                {
                    totalOutRadiance += outRadiance[i];
                }

            Vector3 ownRadiance = emitingRadiance(hitPoint, outDirectionLCS);

            totalOutRadiance += ownRadiance;

            return totalOutRadiance / sampleCount;
        }

        /// <summary>
        /// random direction in hemishpere with X>0. Samples a hemicube, discards points outside of hemispehere, normalizes.
        /// </summary>
        /// <returns></returns>
        private Vector3 RandomDirection()
        {
            Vector3 v = new Vector3(r.NextSingle(), r.NextSingle() * 2 - 1, r.NextSingle() * 2 - 1);
            while(v.LengthSquared()>1)
                v = new Vector3(r.NextSingle(), r.NextSingle() * 2 - 1, r.NextSingle() * 2 - 1);
            return Vector3.Normalize(v);
        }

        /// <summary>
        /// returns local coordinate system at surface point: normal and two tangents
        /// </summary>
        /// <param name="surfacePoint"></param>
        /// <returns></returns>
        abstract public Vector3[] GetLocalCoordinateSystem(HitPoint hitPoint);

        public virtual Vector3 emitingRadiance(HitPoint position, Vector3 direction)
        {
            return new Vector3(0, 0, 0);
        }
    }
}
