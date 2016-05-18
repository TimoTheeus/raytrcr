using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Template
{
    //Defined by a position and a radius
    class Sphere:Primitive
    {
        public float radius;
        public Vector3 position;
        public float r2;
        
        public Sphere(Vector3 pos, float r, Vector3 color):base(color)
        {
            radius = r;
            position = pos;
            r2 = (float)Math.Pow(r, 2);

        }
        public override bool Intersect(Ray ray,out Intersection intersection)
        {
            float distance;
            Vector3 point;
            Vector3 c = this.position - ray.Origin;
            float t = Vector3.Dot(c, ray.Direction);
            Vector3 q = c - t * ray.Direction;
            float p2 = Vector3.Dot(q, q);
            if (p2 > this.r2)
            {
                intersection = null;
                return false;
            }
            t -= (float)Math.Sqrt(this.r2 -p2);
            distance = Math.Min( ray.distance, Math.Max( 0, t ) );
            point = ray.Origin + (distance * ray.Direction);
            intersection = new Intersection(point, distance, this, (-c.Normalized()));
            return true;
        }
    }
}
