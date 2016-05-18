using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Template
{
    class Intersection
    {
        //where the intersection occurred
        protected Vector3 point;

        /// The surface's normal at the intersection point
        protected Vector3 normal;
        
        protected Primitive nearestPrimitive;
        // The distance at which the intersection occurred.
        protected float distance;

        public Intersection(Vector3 p, float d, Primitive nearestP, Vector3 n)
        {
            point = p;
            nearestPrimitive = nearestP;
            normal = n;
            distance = d;
        }
    }
}
