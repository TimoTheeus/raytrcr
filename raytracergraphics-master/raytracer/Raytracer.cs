using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;

namespace Template
{
    class Raytracer
    {
        public Scene scene;
        public Camera camera;
        public Surface display;
        List<Intersection> intersectionList;
        int halfDisplayWidth;
        float maxRayDistance;
        float epsilon;
        public Raytracer(Scene rs, Camera rc, Surface surface)
        {
            scene = rs;
            camera = rc;
            display = surface;
            intersectionList = new List<Intersection>();
            halfDisplayWidth = display.width / 2;
            maxRayDistance = 100f;
            epsilon = 0.0001f;
        }
        public void Render()
        {
            Intersection k;
            for (int x = 0; x < halfDisplayWidth; x++)
                for (int y = 0; y < display.height; y++)
                {
                    Ray ray = camera.CreatePrimaryRay(x, y);
                    k = scene.ReturnClosestIntersection(ray);
                    //if the ray distance actually decreased
                    if (ray.distance < maxRayDistance)
                    {
                        Vector3 intensity = Vector3.Zero;
                        //make shadowrays for all lightsources and intersect them
                        foreach (Light l in scene.lightsources)
                        {
                            Vector3 direction = l.location - k.point;
                            float nDotL = Vector3.Dot(direction.Normalized(), k.normal);
                            //if nDotL <0 dont make and intersect a shadowray
                            if (nDotL > 0)
                            {
                                Ray shadowray = new Ray(k.point + epsilon * direction, Vector3.Normalize(direction), direction.Length - 2 * (epsilon * direction).Length);

                                if (!scene.IntersectShadowRay(shadowray))
                                {
                                    //distance attenuation and N dot L clamped to 0
                                    intensity += l.intensity * (float)(1 / (Math.PI * 4 * Math.Pow(direction.Length, 2))) * Math.Max(0, nDotL);
                                }
                            }
                        }
                        if (k.nearestPrimitive != null)
                        {
                            Vector3 color = floatColorToInt(k.color * intensity);
                            display.pixels[x + y * display.width] = CreateColor((int)color.X, (int)color.Y, (int)color.Z);
                        }
                    }
                }
        }
        Vector3 floatColorToInt(Vector3 floatColorValues)
        {
            return new Vector3(Math.Min(255,floatColorValues.X*256.0f), Math.Min(255, floatColorValues.Y * 256.0f), Math.Min(255, floatColorValues.Z * 256.0f));
        }
        int CreateColor(int red, int green, int blue)
        {
            return (red << 16) + (green << 8) + blue;
        }
    }
}
