using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PathTracer
{
    public class Mesh : SceneObject
    {
        // set of triangles
        public Triangle[] triangles;

        public Mesh(string fileName, Vector3 translation, float scale)
        {
            (var verts, var ids) = LoadObj(fileName);
            triangles = new Triangle[ids.Count / 3];
            for (int i = 0; i < ids.Count; i+=3)
            {
                Vector3 v1 = verts[ids[i]]*scale+translation;
                Vector3 v2 = verts[ids[i+1]]*scale+translation;
                Vector3 v3 = verts[ids[i+2]]*scale+translation;
                triangles[i/3] = new Triangle(v1, v2, v3);
            }
        }
        public override Vector3[] GetLocalCoordinateSystem(HitPoint p)
        {
            MeshHitPoint mhp = (MeshHitPoint)p; // if it is not a MeshHitPoint, then something went very wrong
            return triangles[mhp.triangle].GetLocalCoordinateSystem(p);
        }

        public override HitPoint Intersect(Ray r)
        {
            float minD = float.MaxValue;
            int hit = -1;
            for (int i = 0;i<triangles.Length;i++)            
            {
                float d = triangles[i].Intersect(r).d; 
                if (!float.IsNaN(d))
                {
                    if (d<minD)
                    {
                        minD = d;
                        hit = i;
                    }
                }
            }
            if (hit<0)
                return null;
            return new MeshHitPoint(minD, hit);
        }

        public static (List<Vector3> verticecs, List<int> indices) LoadObj(string filename)
        {
            string[] lines = File.ReadAllLines(filename);
            List<Vector3> verts = new();
            List<int> indicesList = new();

            Dictionary<string, int> idx = new Dictionary<string, int>();

            foreach (string line in lines)
            {
                if (line.StartsWith("v "))
                {
                    verts.Add(ProcessV3(line));
                }
                else if (line.StartsWith("f "))
                {
                    var vertgroups = line.Split(' ');
                    for (int i = 1; i < 4; i++)
                    {
                        
                        var positions = vertgroups[i].Split('/');
                        int.TryParse(positions[0], out int v);
                        indicesList.Add(v - 1);
                    }
                }
            }
            return (verts, indicesList);
        }

        private static Vector3 ProcessV3(string line)
        {
            var tokens = line.Split(' ');
            float.TryParse(tokens[1], NumberStyles.Float, CultureInfo.InvariantCulture, out float x);
            float.TryParse(tokens[2], NumberStyles.Float, CultureInfo.InvariantCulture, out float y);
            float.TryParse(tokens[3], NumberStyles.Float, CultureInfo.InvariantCulture, out float z);
            return new Vector3(x, y, z);
        }
    }
}

