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
        public bool cameraIsMoving;
        //the world the debug output draws in
        float worldX;
        float worldY;
        //center of the debug output
        float centerX;
        float centerY;
        //for drawing the small dot at cameraposition, since the camera doesnt move.
        int cameraPositionX;
        int cameraPositionY;
        public Raytracer(Scene rs, Camera rc, Surface surface)
        {
            scene = rs;
            camera = rc;
            display = surface;
            halfDisplayWidth = display.width / 2;
            //6 by 6 box to draw the debug output in
            worldX = 6;
            worldY = 6;
            //draw it on the right side of the screen.
            centerX = halfDisplayWidth+(halfDisplayWidth/2);
            centerY = display.height / 2;
            cameraPositionX = TX(camera.position.X, centerX);
            cameraPositionY = TY(camera.position.Z, centerY);
        }
        public void Render()
        {
            for (int x = 0; x < halfDisplayWidth; x++)
                for (int y = 0; y < display.height; y++)
                {
                    if ((x % 3 == 0&&y%3==0) || !cameraIsMoving)
                    {
                        Ray ray = scene.ReturnClosestIntersection(camera.CreatePrimaryRay(x, y));
                        Vector3 color= returnColor(ray);
                        if (x % 10 == 0 && y == display.height / 2)
                        {
                            DrawLine(ray,CreateColor(255,0,0));
                            foreach (Ray r in scene.shadowRays)
                            {
                                DrawLine(r,CreateColor(0,0,255));
                            }
                            foreach (Ray r in scene.directIlluminationRays)
                            {
                                DrawLine(r, CreateColor(0, 255, 0));
                            }
                            foreach (Ray r in scene.reflectedRays)
                            {
                                DrawLine(r, CreateColor(0, 255, 255));
                            }
                        }
                        display.Line(TX(camera.Upperleft.X, centerX), TY(camera.Upperleft.Z, centerY), TX(camera.Upperright.X, centerX), TY(camera.Upperright.Z, centerY), CreateColor(255, 255, 255));
                        display.Box(cameraPositionX, cameraPositionY, cameraPositionX - 1, cameraPositionY + 1, CreateColor(255, 255, 0));
                        display.pixels[x + y * display.width] = CreateColor((int)color.X, (int)color.Y, (int)color.Z);
                    }
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
        public Vector3 returnColor(Ray ray)
        {
            Vector3 color;
            scene.reflectedRays.Clear();
            if (ray.nearestPrimitive != null)
            {
                if (ray.nearestPrimitive.isSpecular)
                {
                    color = floatColorToInt(scene.Trace(ray) * ray.nearestPrimitive.specularity + scene.DirectIllumination(ray)
                        * ray.nearestPrimitive.getColor(ray.point) * (1 - ray.nearestPrimitive.specularity));
                }
                else { color = floatColorToInt(scene.Trace(ray)); }
            }
            else
            {
                color = Vector3.Zero;
            }
            return color;
        }
        public void DrawLine(Ray ray,int color)
        {
            //if the points are outside of the screen, draw the line till the end of the screen;
            /* int pointX= TX(ray.point.X, centerX);
              int pointY = TY(ray.point.Z, centerY);
              if (pointX < 0) pointX = 1;
              else if (pointX >= display.width) pointX = display.width - 1;
              if (pointY < 0) pointY = 1;
              else if (pointY >= display.height) pointY = display.height - 1;*/
            //draw the line
            
            display.Line(TX(ray.Origin.X, centerX), TY(ray.Origin.Z, centerY), TX(ray.point.X, centerX), TY(ray.point.Z, centerY), color);
        }
        //transform x value based on the center location
        int TX(float x, float centerX)
        {
            return (int)((x + worldX) * (halfDisplayWidth / (2 * worldX)) - (-centerX + (float)halfDisplayWidth / 2));
        }
        //transform Y value based on the center location
        int TY(float y, float centerY)
        {
            return (int)((-y + worldY) * (display.height / (2 * worldY)) - (-centerY + ((float)display.height / 2)));
        }
    }
}
