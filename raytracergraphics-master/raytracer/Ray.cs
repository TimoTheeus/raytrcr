using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Template
{
    //Class for rays, direction is normalized in constructor
    class Ray
    {
        public Vector3 Origin;
        public Vector3 Direction;
        public float distance;
        public Primitive nearestPrimitive;

        public Ray(Vector3 start, Vector3 direction,float distance)
        {
            this.Origin = start;
            this.Direction = direction.Normalized();
            this.distance = distance;
        }
    }
}
