using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

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
            this.normal = Vector3.Zero;

        }

        //Calculate if a ray is intersected with a sphere, assign this primitive to the membervariable nearest primitive of the ray
        public override void Intersect(Ray ray)
        {
            if (Vector3.Dot(ray.Origin - this.position, ray.Origin - this.position) - r2 > 0)
            {
                Vector3 c = this.position - ray.Origin;
                float t = Vector3.Dot(c, ray.Direction);
                Vector3 q = c - t * ray.Direction;
                float p2 = Vector3.Dot(q, q);
                if (p2 > this.r2)
                {
                    return;
                }
                t -= (float)Math.Sqrt(this.r2 - p2);
                if ((t < ray.distance) && (t > 0))
                {
                    ray.distance = t;
                    ray.nearestPrimitive = this;
                    this.normal = new Vector3((ray.Origin + ray.distance * ray.Direction) - this.position).Normalized();
                    if (Vector3.Dot(normal, ray.Direction.Normalized()) > 0)
                    {
                        this.normal = -this.normal;
                    }
                }
            }
            else
            {
                float a = Vector3.Dot(ray.Direction, ray.Direction);
                float b = Vector3.Dot(2 * ray.Direction, ray.Origin - this.position);
                float c = Vector3.Dot(ray.Origin - this.position, ray.Origin - this.position) - r2;
                float D = (float)Math.Pow(b, 2) - 4 * a * c;
                if (D > 0)
                {
                    float t = (-b - (float)Math.Sqrt(D)) / (2 * a);
                    float j = (-b + (float)Math.Sqrt(D)) / (2 * a);
                    float distance = Math.Min(t, j);
                    if (j < 0)
                        distance = t;
                    else if (t < 0)
                        distance = j;
                    if ((distance < ray.distance) && (distance > 0))
                    {
                        ray.distance = distance;
                        ray.nearestPrimitive = this;
                        this.normal = new Vector3((ray.Origin + ray.distance * ray.Direction) - this.position).Normalized();
                        if (Vector3.Dot(normal, ray.Direction.Normalized()) > 0)
                        {
                            this.normal = -this.normal;
                        }
                    }
                }
            }
            //normal at the point is -c.Normalized
        }

        //Determine the color of a specific point on a sphere
        public override Vector3 getColor(Vector3 point)
        {
            return this.color;
        }

    }
}
