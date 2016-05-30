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
        int halfDisplayWidth;
        public Raytracer(Scene rs, Camera rc, Surface surface)
        {
            scene = rs;
            camera = rc;
            display = surface;
            halfDisplayWidth = display.width / 2;
        }
        public void Render()
        {
            for (int x = 0; x < halfDisplayWidth; x++)
                for (int y = 0; y < display.height; y++)
                {
                    Ray ray = scene.ReturnClosestIntersection(camera.CreatePrimaryRay(x, y));
                    Vector3 color;
                    if (ray.nearestPrimitive != null)
                    {
                        if(ray.nearestPrimitive.isSpecular)
                         color = floatColorToInt(scene.Trace(ray)*ray.nearestPrimitive.specularity+scene.DirectIllumination(ray)*ray.nearestPrimitive.color*(1-ray.nearestPrimitive.specularity));
                        else { color = floatColorToInt(scene.Trace(ray)); }
                    }
                    else
                    {
                        color = Vector3.Zero;
                    }
                    if (x % 10 == 0 && y == display.height / 2)
                    {
                        if (ray.normalAtPoint == Vector3.Zero)
                        {
                            display.Line(ScreenCoordinatesX(camera.position.X + 5), ScreenCoordinatesZ(camera.position.Z), ScreenCoordinatesX((ray.point.X) / 20 + 5), ScreenCoordinatesZ(ray.point.Z / 20), 100 * 256 * 256 + 100 * 256);
                        }
                        else
                            display.Line(ScreenCoordinatesX(camera.position.X + 5), ScreenCoordinatesZ(camera.position.Z), ScreenCoordinatesX(ray.point.X + 5), ScreenCoordinatesZ(ray.point.Z), 255 * 256 * 256 + 255 * 256);
                    }
                    display.pixels[x + y * display.width] = CreateColor((int)color.X, (int)color.Y, (int)color.Z);
                }
        }
        Vector3 floatColorToInt(Vector3 floatColorValues)
        {
            return new Vector3(Math.Min(255, floatColorValues.X * 256.0f), Math.Min(255, floatColorValues.Y * 256.0f), Math.Min(255, floatColorValues.Z * 256.0f));
        }
        int CreateColor(int red, int green, int blue)
        {
            return (red << 16) + (green << 8) + blue;
        }

        public int ScreenCoordinatesX(double number)
        {
            return (int)(number * 50 + 512);
        }

        public int ScreenCoordinatesZ(double number)
        {
            return (int)((10 - number) * 50);
        }
    }
}
