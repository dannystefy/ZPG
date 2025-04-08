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
}
