using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Template
{
    class Primitive
    {
        public Vector3 color;
     
        public Primitive(Vector3 c)
        {
            color = c;
        }
        public virtual bool Intersect(Ray r, out Intersection intersection)
        {
            intersection = null;
            return true;
        }
    }
}
