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

        //comparers for sorting along x, y and z axes
        int compX(int a, int b)
        {
            float avgA = tris[a].p1.X + tris[a].p2.X + tris[a].p3.X;
            float avgB = tris[b].p1.X + tris[b].p2.X + tris[b].p3.X;
            return (avgA.CompareTo(avgB));
        }

        private int compY(int a, int b)
        {
            float avgA = tris[a].p1.Y + tris[a].p2.Y + tris[a].p3.Y;
            float avgB = tris[b].p1.Y + tris[b].p2.Y + tris[b].p3.Y;
            return (avgA.CompareTo(avgB));
        }

        private int compZ(int a, int b)
        {
            float avgA = tris[a].p1.Z + tris[a].p2.Z + tris[a].p3.Z;
            float avgB = tris[b].p1.Z + tris[b].p2.Z + tris[b].p3.Z;
            return (avgA.CompareTo(avgB));
        }

        public BVHNode(Triangle[] triangles, int[] activeTris)
        {
            this.tris = triangles;
            this.nodeTris = activeTris;

            // compute aabb
            aabbMin = tris[nodeTris[0]].p1;
            aabbMax = tris[nodeTris[0]].p1;

            for (int i = 0;i<nodeTris.Length;i++)
            {
                Triangle triangle = tris[nodeTris[i]];
                aabbMin = Vector3.Min(aabbMin, triangle.p1);
                aabbMin = Vector3.Min(aabbMin, triangle.p2);
                aabbMin = Vector3.Min(aabbMin, triangle.p3);

                aabbMax = Vector3.Max(aabbMax, triangle.p1);
                aabbMax = Vector3.Max(aabbMax, triangle.p2);
                aabbMax = Vector3.Max(aabbMax, triangle.p3);
            }

            // create child nodes if the number of triangles is large
            if (nodeTris.Length > 4)
            {
                Vector3 span = aabbMax - aabbMin;
                if ((span.X > span.Y) && (span.X > span.Z))
                    Array.Sort(nodeTris, compX);
                else if ((span.Y > span.X) && (span.Y > span.Z))
                    Array.Sort(nodeTris, compY);
                else Array.Sort(nodeTris, compZ);

                int[] left = new int[nodeTris.Length/2];
                int[] right = new int[nodeTris.Length - nodeTris.Length/2];
                for (int i = 0; i < nodeTris.Length; i++)
                {
                    if (i<left.Length)
                        left[i] = nodeTris[i];
                    else
                        right[i-left.Length] = nodeTris[i];
                }
                this.left = new BVHNode(tris, left);
                this.right = new BVHNode(tris, right);
            }
        }

        

        public MeshHitPoint Intersect(Ray r)
        {
            // parameters of intersection in x, y and z directions
            Vector3 minP = (aabbMin - r.origin) / r.direction;
            Vector3 maxP = (aabbMax - r.origin) / r.direction;

            // parameters of first intersection along ray (enter)
            Vector3 mip = Vector3.Min(minP, maxP);

            // parameters of last intersection along ray (exit)
            Vector3 map = Vector3.Max(minP, maxP);

            // intersection of parameter intervals
            float min = Math.Max(mip.X, Math.Max(mip.Y, mip.Z)); // last entry parameter
            if (min < 0)
                min = 0;

            float max = Math.Min(map.X, Math.Min(map.Y, map.Z)); //first exit parameter

            if (max < min)
                return null; // no intersection

            // bounding box is intersected. Check childnodes if we have some
            if (left!=null)
            {
                MeshHitPoint d1 = left.Intersect(r);
                MeshHitPoint d2 = right.Intersect(r);
                if (d1 == null)
                {
                    if (d2 == null)
                        return null; // none of the children intersected
                    return d2;
                }
                else
                {
                    if (d2 == null)
                        return d1;

                    // both children intesected
                    if (d1.d < d2.d)
                        return d1;
                    return d2;
                }
            }

            // leaf node, check all triangles
            float minD = float.MaxValue;
            int hit = -1;
            for (int i = 0; i < nodeTris.Length; i++)
            {
                HitPoint hp = tris[nodeTris[i]].Intersect(r);
                if (hp!=null)
                {
                    if (hp.d < minD)
                    {
                        minD = hp.d;
                        hit = nodeTris[i];
                    }
                }
            }
            if (hit < 0)
                return null;
            else
                return new MeshHitPoint(minD, hit);
        }
    }
    public class MeshWithBVH:Mesh
    {
        BVHNode root;
        public MeshWithBVH(string fileName, Vector3 translation, float scale) : base(fileName, translation, scale)
        {
            // triangle indices for the root node: all triangles.
            int[] rootTris = new int[triangles.Length];
            for (int i = 0;i<triangles.Length;i++)
                rootTris[i] = i;

            root = new BVHNode(this.triangles, rootTris);
        }

        public override HitPoint Intersect(Ray r)
        {
            return root.Intersect(r);
        }
    }
}
