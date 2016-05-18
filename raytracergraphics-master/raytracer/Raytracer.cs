using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Template
{
    class Raytracer
    {
        protected Scene scene;
        protected Camera camera;
        protected Surface display;
        
        public Raytracer(Scene rs, Camera rc,Surface surface)
        {
            scene = rs;
            camera = rc;
            display = surface;
        }
        public void Render()
        {
          
        }
    }
}
