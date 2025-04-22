using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PathTracer
{
    public class HitPoint
    {
        public float d;

        public Vector3 point;

        public HitPoint(float d)
        {
            this.d = d;
        }
    }

    public class MeshHitPoint:HitPoint
    {
        public int triangle;

        public MeshHitPoint(float d, int t):base(d)
        {
            triangle = t;
        }
    }
}
