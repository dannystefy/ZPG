using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

namespace cv12
{
    public class BSplineCubic : AbstractLine
    {

        protected Vector2[] P = new Vector2[4];


        public BSplineCubic(Vector2 P0, Vector2 P1, Vector2 P2, Vector2 P3, int segments)
        {
            this.P[0] = P0;
            this.P[1] = P1;
            this.P[2] = P2;
            this.P[3] = P3;
            Segments = segments;


            Initialize();
        }


        public override List<Vector2> GetControlPoints()
        {
            return P.ToList();
        }

        protected override void GeneratePoints(List<Vector2> output)
        {
            output.Clear();
            Matrix4 Bbase = new Matrix4(
                -1, 3, -3, 1,
                3, -6, 3, 0,
                -3, 0, 3, 0,
                1, 4, 1, 0
            );

            Vector4 Px = new Vector4(P[0].X, P[1].X, P[2].X, P[3].X);

            Vector4 Py = new Vector4(P[0].Y, P[1].Y, P[2].Y, P[3].Y);


            var BX = Bbase * Px / 6;
            var BY = Bbase * Py / 6;

            for (var i = 0; i <= Segments; i++)
            {
                float p = i / (float)Segments;
                Vector4 ps = new Vector4(p * p * p, p * p, p, 1);


                Vector2 point = new Vector2(Vector4.Dot(ps, BX), Vector4.Dot(ps, BY));

                output.Add(point);
            }
        }
    }
}
