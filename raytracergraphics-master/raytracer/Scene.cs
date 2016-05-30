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
        float epsilon;
        int recursionCounter;
        int recursionDepth;
        float maxRayDistance;
        public Scene(List<Primitive> pList, List<Light> lList)
        {
            primitives = pList;
            lightsources = lList;
            epsilon = 0.0001f;
            recursionCounter = 0;
            recursionDepth = 2;
            maxRayDistance = 100f;
        }

        public Ray ReturnClosestIntersection(Ray ray)
        {
            foreach (Primitive p in primitives)
            {
                p.Intersect(ray);
            }
            if (ray.nearestPrimitive != null)
            {
                Vector3 point = ray.Origin + (ray.distance * ray.Direction);
                ray.point = point;
                ray.normalAtPoint = ray.nearestPrimitive.normal;
            }
            return ray;

        }
        public Vector3 Trace(Ray ray)
        {
            ray = ReturnClosestIntersection(ray);
            Primitive p = ray.nearestPrimitive;
            if (ray.distance < maxRayDistance)
            {
                if (p.isSpecular)
                {
                    if (recursionCounter < recursionDepth)
                    {
                        Vector3 reflectedDirection = new Vector3(ray.Direction - 2 * (Vector3.Dot(ray.Direction, ray.normalAtPoint)) * ray.normalAtPoint);
                        return p.color * Trace(new Ray(ray.point + epsilon * reflectedDirection, reflectedDirection, maxRayDistance));
                    }
                    else { return Vector3.Zero; }
                }
                return DirectIllumination(ray) * p.color;
            }
            else return Vector3.Zero;
        }
        public Vector3 DirectIllumination(Ray ray)
        {
            //if the ray distance actually decreased
            Vector3 intensity = Vector3.Zero;
            //make shadowrays for all lightsources and intersect them
            foreach (Light l in lightsources)
            {
                Vector3 lightDirection = l.location - ray.point;
                float nDotL = Vector3.Dot(lightDirection.Normalized(), ray.normalAtPoint);
                //if nDotL>0, prevent shadowrays
                if (nDotL > 0)
                {
                    //For spotlights, Only create shadowrays and thus (potentialliy)increase light intensity at that point
                    //if the angle between lightdirection and spotlightdirection<spotlightangle
                    float spotlightAngle = (float)Math.Acos(Vector3.Dot(l.spotLightDirection, -lightDirection.Normalized()));
                    if (((spotlightAngle < l.spotlightAngle&&spotlightAngle>0 )|| !l.isSpotlight))
                    {
                        Ray shadowray = new Ray(ray.point + epsilon * lightDirection, Vector3.Normalize(lightDirection), lightDirection.Length -
                            2 * (epsilon * lightDirection).Length);
                        if (!IntersectShadowRay(shadowray))
                        {
                            //distance attenuation and N dot L clamped to 0
                            intensity += l.intensity * (float)(1 / (Math.PI * 4 * Math.Pow(lightDirection.Length, 2))) * Math.Max(0, nDotL);
                        }
                    }
                }
            }
            return intensity;
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
