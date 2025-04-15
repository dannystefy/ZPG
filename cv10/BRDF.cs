using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PathTracer
{
    public abstract class BRDF
    {
        /// <summary>
        /// returns reflectance from indirection to outdirection. Should obey laws, i.e. energy conservation, positivity, Helmholz reciprocity
        /// </summary>
        /// <param name="inDirectionLCS"></param>
        /// <param name="outDirectionLCS"></param>
        /// <returns></returns>
        public abstract Vector3 Value(Vector3 inDirectionLCS, Vector3 outDirectionLCS);
        
    }

    public class LambertianBRDF : BRDF
    {
        public Vector3 albedo = new Vector3(1,1,1);
        public override Vector3 Value(Vector3 inDirectionLCS, Vector3 outDirectionLCS)
        {
            return albedo / (float) Math.PI;
        }
    }

    public class PhongBRDF : BRDF
    {
        public Vector3 albedo = new Vector3(1, 1, 1);
        float exp = 200;
        public override Vector3 Value(Vector3 inDirectionLCS, Vector3 outDirectionLCS)
        {
            Vector3 refDirectionLCS = -inDirectionLCS;
            refDirectionLCS[0] = inDirectionLCS[0];
            float dot = Vector3.Dot(refDirectionLCS, outDirectionLCS);
            if (dot < 0)
                dot = 0;
            dot = (float)Math.Pow(dot, exp);
            return albedo*dot;
        }
    }
}
