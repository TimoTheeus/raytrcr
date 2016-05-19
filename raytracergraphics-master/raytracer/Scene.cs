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
            for (int i = 0; i < primitives.Count; i++)
            {
                primitives[i].Intersect(ray);
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
    }
}
