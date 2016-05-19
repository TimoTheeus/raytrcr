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
        public Vector3 normal;
     
        public Primitive(Vector3 c)
        {
            color = c;
        }
        public virtual void Intersect(Ray ray)
        {
            
        }
    }
}
