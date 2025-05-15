using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using ZPG01;

namespace ZPG03
{
    public class Camera
    {
        public Viewport viewport;


        public float scale = 1f;
        public float x = 0;
        public float y = 0;

        public float rx = 0;
        public float ry = 0;




        public Camera(Viewport viewport) 
        { 
            this.viewport = viewport;
        } 

        public void SetProjection()
        {
            float ratio = (float)((viewport.Width * viewport.Control.Width) / (viewport.Height* viewport.Control.Height));
            var matrix = Matrix4.CreateOrthographic(2 * scale, 2 * ratio * scale, -10, 10);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref matrix);
        }

        public void Zoom(float coeff)
        {
            scale *= coeff; 
        }

        public void Move(float x, float y)
        {
            this.x -= x;
            this.y -= y;
        }


        public void SetView()
        {
            Matrix4 view = Matrix4.Identity;
            view *= Matrix4.CreateRotationY(ry);
            view *= Matrix4.CreateRotationX(rx);
            view *= Matrix4.CreateTranslation (x,y,0);
            
            
            
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref view);
        }


        
    }
}
