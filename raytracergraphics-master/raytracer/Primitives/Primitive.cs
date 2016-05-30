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
            specularity = 0.5f;
        }
        public virtual void Intersect(Ray ray)
        {
            
        }
        public virtual Vector3 getColor(Vector3 point)
        {
            return Vector3.Zero;
        }
    }
}
