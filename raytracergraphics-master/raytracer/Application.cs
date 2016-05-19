using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template
{
    //Handle input 
    class Application
    {
        protected Raytracer raytracer;

        public Application(Raytracer raytracer)
        {
            this.raytracer = raytracer;
        }
        public void Visualize()
        {
            raytracer.Render();
        }
    }
}
