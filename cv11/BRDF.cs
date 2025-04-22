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
        public abstract Vector3 Value(Vector3 inDirectionLCS, Vector3 outDirectionLCS, Vector3 surfacePoint);
        
    }

    public class LambertianBRDF : BRDF
    {
        public Vector3 albedo = new Vector3(1,1,1);
        public override Vector3 Value(Vector3 inDirectionLCS, Vector3 outDirectionLCS, Vector3 surfacePoint)
        {
            return albedo / (float) Math.PI;
        }

        public LambertianBRDF(float r, float g, float b)
        {
            this.albedo = new Vector3(r, g, b);
        }
    }

    public class LambertianCheckerBoardBRDF : BRDF
    {
        public Vector3 albedoBright = new Vector3(1, 1, 1);
        public Vector3 albedoDark = new Vector3(0.1f, 0.1f, 0.1f);
        public override Vector3 Value(Vector3 inDirectionLCS, Vector3 outDirectionLCS, Vector3 surfacePoint)
        {
            int x = (int)Math.Round(surfacePoint.X);
            int z = (int)Math.Round(surfacePoint.Z);
            if ((x+z)%2 == 0)
                return albedoBright / (float)Math.PI;
            else
                return albedoDark / (float)Math.PI;
        }

        public LambertianCheckerBoardBRDF(float r, float g, float b, float r2, float g2, float b2)
        {
            this.albedoBright = new Vector3(r, g, b);
            this.albedoDark = new Vector3(r2, g2, b2);
        }
    }

    public class PhongBRDF : BRDF
    {
        public Vector3 albedo = new Vector3(1, 1, 1);
        float exp = 200;
        public override Vector3 Value(Vector3 inDirectionLCS, Vector3 outDirectionLCS, Vector3 surfacePoint)
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
