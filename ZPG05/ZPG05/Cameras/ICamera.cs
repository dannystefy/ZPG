using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

namespace ZPG05.Cameras
{
    public interface ICamera
    {
        Matrix4 View { get; }
        Matrix4 Projection { get; }
    }
}
