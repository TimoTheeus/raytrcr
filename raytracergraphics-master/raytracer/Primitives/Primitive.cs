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
        //absorption
        public Vector3 color;
        //the normal
        public Vector3 normal;
        //specular or not
        public bool isSpecular;
        //0-1f
        public float specularity;
     
        public Primitive(Vector3 c)
        {
            color = c;
            specularity = 0.7f;
        }
        public virtual void Intersect(Ray ray)
        {
            
        }
    }
}
