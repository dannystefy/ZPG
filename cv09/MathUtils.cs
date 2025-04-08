using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PathTracer
{
    public class MathUtils
    {
        public static float Determinant(Vector3 c1, Vector3 c2, Vector3 c3)
        {
            return  c1.X *c2.Y * c3.Z
                  + c1.Y * c2.Z * c3.X
                  + c1.Z * c2.X * c3.Y 
                  - c1.Z * c2.Y * c3.X 
                  - c1.Y * c2.X * c3.Z
                  - c1.X * c2.Z * c3.Y;
        }
    }
}
