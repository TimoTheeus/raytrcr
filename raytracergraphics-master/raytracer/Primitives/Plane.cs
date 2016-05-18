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
        public Vector3 normal;
        public float distanceToOrigin;

        public Plane(Vector3 n, float dToOrigin,Vector3 color):base(color)
        {
            normal = n.Normalized();
            distanceToOrigin = dToOrigin;
            this.color = new Vector3(255, 0, 0);
        }
        public override bool Intersect(Ray r,out Intersection intersection)
        {
            float distance = (Vector3.Dot(r.Origin, this.normal) + this.distanceToOrigin) / Vector3.Dot(r.Direction, this.normal);
            if(distance<=0)
            {
                intersection = null;
                return false;
            }
            Vector3 point = r.Origin + (distance * r.Direction);
            intersection = new Intersection(point,distance,this,this.normal);
            return true;
        }
    }
}
