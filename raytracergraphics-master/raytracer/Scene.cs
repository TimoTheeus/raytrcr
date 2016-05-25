using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Template
{
    class Scene
    {
        public List<Primitive> primitives;
        public List<Light> lightsources;
        public Scene(List<Primitive> pList, List<Light> lList)
        {
            primitives = pList;
            lightsources = lList;
        }

        public Intersection ReturnClosestIntersection(Ray ray)
        {
            foreach (Primitive p in primitives)
            {
                p.Intersect(ray);
            }
            Vector3 point = ray.Origin + (ray.distance * ray.Direction);
            if (ray.distance < 100f)
                return new Intersection(point, ray.distance, ray.nearestPrimitive, ray.nearestPrimitive.normal);
            else
            {
                return new Intersection(point, ray.distance, ray.nearestPrimitive, Vector3.Zero);
            }
        }
        public void AddPrimitive(Primitive p)
        {
            primitives.Add(p);
        }
        public void AddLightSource(Light l)
        {
            lightsources.Add(l);
        }
        public bool IntersectShadowRay(Ray r)
        {
            //store the maxdistance (magnitude of vector (light.pos-I.pos)
            float maxDistance = r.distance;
            foreach (Primitive p in primitives)
            {
                p.Intersect(r);
            }
            //if the new distance is between the max distance and 0, return true
            if (r.distance < maxDistance && r.distance > 0)
            {
                return true;
            }
            //else false
            else { return false; }
        }
    }
}
