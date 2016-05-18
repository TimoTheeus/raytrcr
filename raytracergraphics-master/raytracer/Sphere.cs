using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Template
{
    //Defined by a position and a radius
    class Sphere:Primitive
    {
        protected float radius;
        protected Vector3 position;
        
        public Sphere(Vector3 pos, float r, Vector3 color):base(color)
        {
            radius = r;
            position = pos;

        }
    }
}
