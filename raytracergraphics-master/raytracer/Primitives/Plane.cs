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
        public int textureWidth;
        public int textureHeight;
        public Vector3 color1,color2;

        public Plane(Vector3 n, float dToOrigin,Vector3 color1, Vector3 color2) :base(color1)
        {
            this.normal = n.Normalized();
            this.distanceToOrigin = dToOrigin;
            textureWidth = 5;
            textureHeight = 10;
            this.color1= color1;
            this.color2 = color2;

        }

        public override void Intersect(Ray ray)
        {
            float distance = -((Vector3.Dot(ray.Origin, this.normal) + this.distanceToOrigin) / Vector3.Dot(ray.Direction, this.normal));
            if (distance <= 0)
            {
                return;
            }
            if (distance < ray.distance)
            {
                ray.distance = distance;
                ray.nearestPrimitive = this;
            }
        }
        public override Vector3 getColor(Vector3 point)
        {
            Vector3 color = color1;
            if (point.X > -textureWidth && point.X < textureWidth && point.Z < textureHeight && point.Z > 0)
            {
                if ((point.X + textureWidth) % 2 >= 1 != (point.Z + textureWidth) % 2 > 1)
                    color = color1;
                else
                    color = color2;
            }
            else
                color = color2;
            return color;
        }
    }
}
