using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Template
{
    //Defined by a normalized normal and a distance to the origin
    class Plane:Primitive
    {
        public float distanceToOrigin;

        public Plane(Vector3 n, float dToOrigin,Vector3 color):base(color)
        {
            this.normal = n.Normalized();
            this.distanceToOrigin = dToOrigin;
        }
        public override void Intersect(Ray ray)
        {
            float distance = (Vector3.Dot(ray.Origin, this.normal) + this.distanceToOrigin) / Vector3.Dot(ray.Direction, this.normal);
            if(distance<=0)
            {
                return;
            }
            if (distance < ray.distance)
            {
                ray.distance = distance;
                ray.nearestPrimitive = this;
            }
        }
    }
}
