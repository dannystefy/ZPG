﻿using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zpg
{
    public class Light
    {
        public Vector4 position;
        public Vector3 color;
        public float intensity;

        public Light(Vector3 posOrDir, bool isDirectional) 
        {
            position = new Vector4(posOrDir, isDirectional ? 0f : 1f);
            color = Vector3.One;
            intensity = 1f;
        }
    }
}
