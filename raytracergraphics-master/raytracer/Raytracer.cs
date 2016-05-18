using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

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
            for (int x = 0; x < display.width; x++)
                for (int y = 0; y < display.height; y++)
                    for (int z = 0; z < scene.primitives.Count; z++)
                    {
                        if (scene.primitives[z].Intersect(camera.CreatePrimaryRay(x, y), out k))
                        {
                            intersectionList.Add(k);
                            display.pixels[x + y * display.width] = CreateColor((int)k.nearestPrimitive.color.X, (int)k.nearestPrimitive.color.Y, (int)k.nearestPrimitive.color.Z);
                        }
                    }
        }
        int CreateColor(int red, int green, int blue)
        {
            return (red << 16) + (green << 8) + blue;
        }
    }
}
