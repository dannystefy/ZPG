using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZPG03
{
    public class Vector3
    {
        public double X { get; set; }
        public double Y { get; set; }

        public double Z { get; set; }


        public Vector3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static double Distance(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            return Math.Sqrt((v1.X - v2.X) * (v1.X - v2.X) + (v1.Y - v2.Y) * (v1.Y - v2.Y));
        }
    }
}
 
