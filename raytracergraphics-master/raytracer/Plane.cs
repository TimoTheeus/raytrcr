using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Template
{
    //Defined by a normal and a distance to the origin
    class Plane:Primitive
    {
        protected Vector3 normal;
        protected float distanceToOrigin;

        public Plane(Vector3 n, float dToOrigin,Vector3 color):base(color)
        {
            normal = n;
            distanceToOrigin = dToOrigin;
            this.color = new Vector3(255, 0, 0);
        }
    }
}
