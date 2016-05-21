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
        public float width = 10;
        public float length = 10;

        public Plane(Vector3 n, float dToOrigin,Vector3 color):base(color)
        {
            this.normal = n.Normalized();
            this.distanceToOrigin = dToOrigin;
        }

        public override void Intersect(Ray ray)
        {
            float distance = (Vector3.Dot(ray.Origin, this.normal) + this.distanceToOrigin) / Vector3.Dot(ray.Direction, this.normal);
            if (distance <= 0)
            {
                return;
            }
            if (distance < ray.distance)
            {
                ray.distance = distance;

                Vector3 point = ray.Origin + (ray.distance * ray.Direction);

                if (point.X > -5 && point.X < 5 && point.Z < 10 && point.Z > 0)
                {
                    if ((point.X + 5) % 2 >= 1 != (point.Z + 5) % 2 > 1)
                        color = new Vector3(255, 255, 255);
                    else
                        color = new Vector3(50, 50, 50);
                }
                else
                    color = Vector3.Zero;


                ray.nearestPrimitive = this;
            }
        }
    }
}
