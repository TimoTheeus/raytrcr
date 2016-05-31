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
        public List<Ray> reflectedRays;
        public List<Ray> shadowRays;
        public List<Ray> directIlluminationRays;
        public List<Ray> refractionRays;
        public Scene(List<Primitive> pList, List<Light> lList)
        {
            primitives = pList;
            lightsources = lList;
            epsilon = 0.0001f;
            recursionCounter = 0;
            recursionDepth = 3;
            maxRayDistance = 100f;
            reflectedRays = new List<Ray>();
            shadowRays = new List<Ray>();
            directIlluminationRays = new List<Ray>();
            refractionRays = new List<Ray>();
        }
        //return the first intersection, and set the point of intersection and the normal at this point.
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
        //trace where a ray goes in case of reflection or refraction
        public Vector3 Trace(Ray ray)
        {
            ray = ReturnClosestIntersection(ray);
            directIlluminationRays.Clear();
            if (ray.distance < maxRayDistance&&ray.distance>0)
            {
                Primitive p = ray.nearestPrimitive;
                Vector3 color = p.getColor(ray.point);
                if (p.isSpecular)
                {//mirror the ray
                    if (recursionCounter < recursionDepth)
                    {
                        return color * Trace(reflect(ray));
                    }
                    else {  return Vector3.Zero; }
                }
                else if (p.isDielectric)
                {//refract the ray
                    if (recursionCounter < recursionDepth)
                    {
                        Sphere sphere = ray.nearestPrimitive as Sphere;
                        Vector3 pMinC = ray.Origin - sphere.position;
                        float f;
                        if ((Vector3.Dot(pMinC, pMinC) - Math.Pow(sphere.radius, 2)) > 0)
                        {
                            f = Fresnel(ray, true);
                            return (f * Trace(reflect(ray)) + (1 - f) * Trace(refraction(ray, true))) * color;
                        }
                        else
                        {
                            f = Fresnel(ray, false);
                            return (f * Trace(reflect(ray)) + (1 - f) * Trace(refraction(ray, false))) * color;
                        }
                    }
                    else
                    {
                        recursionCounter = 0;
                        return Vector3.Zero;
                    }
                        
                }
                return DirectIllumination(ray) * p.getColor(ray.point);
            }
            else {
                recursionCounter = 0;
                return Vector3.Zero;
            }
        }
        public float Fresnel(Ray ray, bool outsideSphere)
        {//Fresnel's law in code form
            float cos1 = Vector3.Dot(ray.normalAtPoint.Normalized(), -ray.Direction.Normalized());
            if (outsideSphere)
            {
                return (ray.nearestPrimitive.fresnelR + (1 - ray.nearestPrimitive.fresnelR) * (float)Math.Pow((1 - cos1), 5));
            }
            else
            {
                return (ray.nearestPrimitive.fresnelRInverted + (1 - ray.nearestPrimitive.fresnelRInverted) * (float)Math.Pow((1 - cos1), 5));
            }
        }
        public Ray refraction(Ray ray,bool outsideSphere)
        {//refract the ray - only works for spheres
            float k;
            float cos1 = Vector3.Dot(ray.normalAtPoint.Normalized(), -ray.Direction.Normalized());
            Ray rayOut;
            float dividedindexes;
            //if ray origin in outside sphere(then its air to glass)
            if(outsideSphere)
            {
                k = 1 - (float)(Math.Pow(ray.nearestPrimitive.dividedIndexes, 2) * (1 - Math.Pow(cos1, 2)));
                dividedindexes = ray.nearestPrimitive.dividedIndexes;
            }
            else
            {
                k = 1 - (float)(Math.Pow(ray.nearestPrimitive.dividedIndexesInverted, 2) * (1 - Math.Pow(cos1, 2)));
                dividedindexes = ray.nearestPrimitive.dividedIndexesInverted;
            }
            if (k < 0)
            {
                //total reflection
                rayOut = reflect(ray);
            }
            else
            {
                Vector3 refractedDirection = ((dividedindexes * ray.Direction) + ray.normalAtPoint * (float)((dividedindexes * cos1) - Math.Sqrt(k)));
                rayOut = new Ray(ray.point + epsilon * refractedDirection, refractedDirection, maxRayDistance);
                refractionRays.Add(rayOut);
            }
            return rayOut;
        }
        public Ray reflect(Ray ray)
        {//reflect the ray with respect to the normal
            if (recursionCounter > 0)
                reflectedRays.Add(ray);
            recursionCounter += 1;
            Vector3 reflectedDirection = new Vector3(ray.Direction - 2 * (Vector3.Dot(ray.Direction, ray.normalAtPoint)) * ray.normalAtPoint);
            Ray reflectedRay = new Ray(ray.point + epsilon * reflectedDirection, reflectedDirection, maxRayDistance);
            return reflectedRay;
        }
        public Vector3 DirectIllumination(Ray ray)
        {
            shadowRays.Clear();
            if(recursionCounter>0)
            directIlluminationRays.Add(ray);
            //if the ray distance actually decreased
            Vector3 intensity = Vector3.Zero;
            //make shadowrays for all lightsources and intersect them
            foreach (Light l in lightsources)
            {
                Vector3 lightDirection = l.location - ray.point;
                float nDotL = Vector3.Dot(lightDirection.Normalized(), ray.normalAtPoint);
                //if nDotL>0, prevent shadowrays
                if (nDotL > 0||ray.nearestPrimitive.isDielectric)
                {
                    //For spotlights, Only create shadowrays and thus (potentialliy)increase light intensity at that point
                    //if the angle between lightdirection and spotlightdirection<spotlightangle
                    float spotlightAngle = (float)Math.Acos(Vector3.Dot(l.spotLightDirection, -lightDirection.Normalized()));
                    if (((spotlightAngle < l.spotlightAngle&&spotlightAngle>0 )|| !l.isSpotlight))
                    {
                        Ray shadowray = new Ray(ray.point + epsilon * lightDirection, Vector3.Normalize(lightDirection), lightDirection.Length -
                            2 * (epsilon * lightDirection).Length);
                        if (!IntersectShadowRay(shadowray)||ray.nearestPrimitive.isDielectric)
                        {
                            //distance attenuation and N dot L clamped to 0
                            intensity += l.intensity * (float)(1 / (Math.PI * 4 * Math.Pow(lightDirection.Length, 2))) * Math.Max(0, nDotL);
                        }
                    }
                }
            }
            recursionCounter = 0;
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
                
                r.point = r.Origin + r.distance * r.Direction;
                shadowRays.Add(r);
                return true;
            }
            else if (r.distance < 0)
            {
                return false;
            }
            //else false
            else {
                r.point = r.Origin + r.distance * r.Direction;
                shadowRays.Add(r);
                return false; }
        }
    }
}
