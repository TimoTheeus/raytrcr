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
        public Vector3 point;

        /// The surface's normal at the intersection point
        public Vector3 normal;

        public Primitive nearestPrimitive;
        // The distance at which the intersection occurred.
        public float distance;
        public Vector3 color;
        public float addedDistance;

        public Intersection(Vector3 p, float d, Primitive nearestP, Vector3 n,Vector3 color)
        {
            point = p;
            nearestPrimitive = nearestP;
            normal = n;
            distance = d;
            this.color = color;
        }
    }
}
