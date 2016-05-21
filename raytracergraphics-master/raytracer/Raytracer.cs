using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
namespace Template
{
    class Raytracer
    {
        protected Scene scene;
        protected Camera camera;
        protected Surface display;
        List<Intersection> intersectionList;

        public Raytracer(Scene rs, Camera rc, Surface surface)
        {
            scene = rs;
            camera = rc;
            display = surface;
            intersectionList = new List<Intersection>();
            
        }
        public void Render()
        {
            Intersection k;
            for (int x = 0; x < display.width/2; x++)
                for (int y = 0; y < display.height; y++)
                {
                    Ray ray = camera.CreatePrimaryRay(x, y);
                    float distance = ray.distance;
                    k=scene.ReturnClosestIntersection(ray);
                    if(ray.distance<distance)
                    display.pixels[x + y * display.width] = CreateColor((int)k.nearestPrimitive.color.X, (int)k.nearestPrimitive.color.Y, (int)k.nearestPrimitive.color.Z);
                }
        }
        int CreateColor(int red, int green, int blue)
        {
            return (red << 16) + (green << 8) + blue;
        }
    }
}
