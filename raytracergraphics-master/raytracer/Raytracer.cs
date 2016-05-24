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
                    k=scene.ReturnClosestIntersection(ray);
                    //if the ray distance actually decreased
                    if (ray.distance < maxRayDistance)
                    {
                        Vector3 intensity = Vector3.Zero;
                        //make shadowrays for all lightsources and intersect them
                        for (int i = 0; i < scene.lightsources.Count; i++)
                        {
                            Vector3 direction = scene.lightsources[i].location - k.point;
                            Ray shadowray = new Ray(k.point+epsilon*direction, Vector3.Normalize(direction), direction.Length-2*(epsilon*direction).Length);

                            if (!scene.IntersectShadowRay(shadowray))
                            {
                                intensity += scene.lightsources[i].intensity*(float)(1/(Math.PI*4*k.distance));
                            }
                        }
                        display.pixels[x + y * display.width] = CreateColor((int)(k.nearestPrimitive.color.X*intensity.X), (int)(k.nearestPrimitive.color.Y*intensity.Y), (int)(k.nearestPrimitive.color.Z*intensity.Z));
                    }
                }
        }
        int CreateColor(int red, int green, int blue)
        {
            return (red << 16) + (green << 8) + blue;
        }
    }
}
