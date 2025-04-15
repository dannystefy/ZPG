using System.Numerics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PathTracer
{
    class BVHNode
    {
        // axis-aligned bounding box
        public Vector3 aabbMin, aabbMax;

        // reference to a list of all triangles
        public Triangle[] tris;

        // list of triangle indices active in the given node
        public int[] nodeTris;

        // children
        BVHNode left, right;

        public BVHNode(Triangle[] triangles, int[] activeTris)
        {
            this.tris = triangles;
            this.nodeTris = activeTris;

            // compute aabb

            aabbMin = tris[nodeTris[0]].p1;
            aabbMax = tris[nodeTris[0]].p1;




            for (var i = 0; i < activeTris.Length; i++)
            {
                var triangle = tris[activeTris[i]];
                aabbMin = Vector3.Min(aabbMin, triangle.p1);
                aabbMin = Vector3.Min(aabbMin, triangle.p2);
                aabbMin = Vector3.Min(aabbMin, triangle.p3);


                aabbMax = Vector3.Max(aabbMax, triangle.p1);
                aabbMax = Vector3.Max(aabbMax, triangle.p2);
                aabbMax = Vector3.Max(aabbMax, triangle.p3);

            }


            // create child nodes if the number of triangles is large
            if (nodeTris.Length > 5)
            {
                var span = aabbMax - aabbMin;
                /*  if (span.X > span.Y && span.X > span.Z)
                  {
                      // split along x-axis
                      Array.Sort(nodeTris, compX);
                  }
                  else if (span.Y > span.X && span.Y > span.Z)
                  {
                      // split along y-axis
                      Array.Sort(nodeTris, compY);
                  }
                  else
                  {
                      // split along z-axis
                      Array.Sort(nodeTris, compZ);
                  }
                */

                if (span.X > span.Y && span.X > span.Z)
                    Array.Sort(nodeTris, compCentroidX);
                else if (span.Y > span.X && span.Y > span.Z)
                    Array.Sort(nodeTris, compCentroidY);
                else
                    Array.Sort(nodeTris, compCentroidZ);


                int[] left = new int[nodeTris.Length / 2];
                int[] right = new int[nodeTris.Length - left.Length];

                for (var i = 0; i < nodeTris.Length; i++)
                {
                    if (i < left.Length)
                    {
                        left[i] = nodeTris[i];
                    }
                    else
                    {
                        right[i - left.Length] = nodeTris[i];
                    }
                }

                this.left = new BVHNode(tris, left);
                this.right = new BVHNode(tris, right);
            }

        }

        int compX(int a, int b)
        {
            return tris[a].p1.X.CompareTo(tris[b].p1.X);
        }

        int compY(int a, int b)
        {
            return tris[a].p1.Y.CompareTo(tris[b].p1.Y);
        }

        int compZ(int a, int b)
        {
            return tris[a].p1.Z.CompareTo(tris[b].p1.Z);
        }


        int compCentroidX(int a, int b)
        {
            var ca = (tris[a].p1 + tris[a].p2 + tris[a].p3) / 3;
            var cb = (tris[b].p1 + tris[b].p2 + tris[b].p3) / 3;
            return ca.X.CompareTo(cb.X);
        }
        int compCentroidY(int a, int b)
        {
            var ca = (tris[a].p1 + tris[a].p2 + tris[a].p3) / 3;
            var cb = (tris[b].p1 + tris[b].p2 + tris[b].p3) / 3;
            return ca.Y.CompareTo(cb.Y);
        }
        int compCentroidZ(int a, int b)
        {
            var ca = (tris[a].p1 + tris[a].p2 + tris[a].p3) / 3;
            var cb = (tris[b].p1 + tris[b].p2 + tris[b].p3) / 3;
            return ca.Z.CompareTo(cb.Z);
        }


        public MeshHitPoint Intersect(Ray r)
        {
            Vector3 minP = (aabbMin - r.origin) / r.direction;
            Vector3 maxP = (aabbMax - r.origin) / r.direction;

            Vector3 mip = Vector3.Min(minP, maxP);
            Vector3 map = Vector3.Max(maxP, minP);

            float min = Math.Max(mip.X, Math.Max(mip.Y, mip.Z));
            if (min < 0) min = 0;
            float max = Math.Min(map.X, Math.Min(map.Y, map.Z));
            // check if aabb intersected by ray

            // if not, return null
            if (min > max)
                return null;


            // else if there are child nodes, check them for intersections
            if (left != null)
            {
                MeshHitPoint d1 = left.Intersect(r);
                MeshHitPoint d2 = right.Intersect(r);

                if (d1 == null)
                    return d2;
                else
                {
                    if (d2 == null)
                        return d1;
                    if (d1.d < d2.d)
                        return d1;
                    return d2;
                }
            }

            float minD = float.PositiveInfinity;
            int hit = -1;

            for (int i = 0; i < nodeTris.Length; i++)
            {
                var hp = tris[nodeTris[i]].Intersect(r);
                if (hp != null)
                {
                    if (hp.d < minD)
                    {
                        minD = hp.d;
                        hit = nodeTris[i];
                    }
                }
            }

            if (hit != -1)
            {
                return new MeshHitPoint(minD, hit);
            }

            return null;

            // if just one intersecterd, return the intersection

            // if both intersected, return the nearest intersection

            // if there are no child nodes, then check the triangles of the current node, one by one

            // return closest intersection

        }
    }
    public class MeshWithBVH : Mesh
    {
        BVHNode root;
        public MeshWithBVH(string fileName, Vector3 translation, float scale) : base(fileName, translation, scale)
        {
            // triangle indices for the root node: all triangles.
            int[] rootTris = new int[triangles.Length];
            for (int i = 0; i < triangles.Length; i++)
                rootTris[i] = i;

            root = new BVHNode(this.triangles, rootTris);
        }

        public override HitPoint Intersect(Ray r)
        {
            return root.Intersect(r);
        }
    }
}