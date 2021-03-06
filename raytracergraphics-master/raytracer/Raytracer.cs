﻿using System;
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
        public void Render(List<Primitive> primitiveList)
        {
            foreach (Primitive primitive in primitiveList)
            {//Draw the speres on the debug output
                if (primitive is Sphere)
                {
                    Sphere sphere = primitive as Sphere;
                    for (int i = 0; i < 100; i++)
                    {
                        display.Line(TX((float)(Math.Cos(i / (2 * Math.PI)) + sphere.position.X), centerX), TY((float)(Math.Sin(i / (2 * Math.PI)) + sphere.position.Z), centerY), TX((float)(Math.Cos((i + 1) / (2 * Math.PI)) + sphere.position.X), centerX), TY((float)(Math.Sin((i + 1) / (2 * Math.PI)) + sphere.position.Z), centerY), ConvertToColor(sphere.color));
                    }
                }
            }
            //for every pixel, cast a ray to determine the color that should be drawn.
            for (int x = 0; x < halfDisplayWidth; x++)
                for (int y = 0; y < display.height; y++)
                {
                    if ((x % 3 == 0&&y%3==0) || !cameraIsMoving)
                    {
                        Ray ray = scene.ReturnClosestIntersection(camera.CreatePrimaryRay(x, y));
                        Vector3 color= AverageColor(x, y);
                        if (x % 10 == 0 && y == display.height / 2)
                        {
                            DrawLine(ray,CreateColor(255,0,0));
                            //shadow rays are blue
                            foreach (Ray r in scene.shadowRays)
                            {
                                DrawLine(r,CreateColor(0,0,255));
                            }
                            //directilluminationrays are green
                            foreach (Ray r in scene.directIlluminationRays)
                            {
                                DrawLine(r, CreateColor(0, 255, 0));
                            }
                            //reflectedrays are yellow
                            foreach (Ray r in scene.reflectedRays)
                            {
                                DrawLine(r, CreateColor(255, 255, 0));
                            }
                            //refraction rays are white
                            foreach (Ray r in scene.refractionRays)
                            {
                                DrawLine(r, CreateColor(255, 255, 255));
                            }
                        }//Draw the camera and screen on the debug output
                        display.Line(TX(camera.Upperleft.X, centerX), TY(camera.Upperleft.Z, centerY), TX(camera.Upperright.X, centerX), TY(camera.Upperright.Z, centerY), CreateColor(255, 255, 255));
                        display.Box(cameraPositionX, cameraPositionY, cameraPositionX - 1, cameraPositionY + 1, CreateColor(255, 255, 0));
                        display.pixels[x + y * display.width] = CreateColor((int)color.X, (int)color.Y, (int)color.Z);
                    }
                }
        }
        Vector3 floatColorToInt(Vector3 floatColorValues)
        {//get a vector for the color from floats
            return new Vector3(Math.Min(255, floatColorValues.X * 256.0f), Math.Min(255, floatColorValues.Y * 256.0f), Math.Min(255, floatColorValues.Z * 256.0f));
        }
        int CreateColor(int red, int green, int blue)
        {//bitshift colors so you get a good int
            return (red << 16) + (green << 8) + blue;
        }
        
        //Determine what color should be drawn for each pixel
        public Vector3 returnColor(Ray ray)
        {
            Vector3 color;
            scene.reflectedRays.Clear();
            scene.refractionRays.Clear();
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
        //draw a line on the debug screen for a ray
        public void DrawLine(Ray ray,int color)
        {
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

        public int ConvertToColor(Vector3 vector)
        {//convert a vector3 with colors to an int.
            float number = 0;
            number += vector.X * 256 * 256;
            number += vector.Y * 256;
            number += vector.Z;
            return (int)number * 255;
        }

        public Vector3 AverageColor(int x, int y)
        {//determine the average color for the anti aliasing
            Vector3 color = Vector3.Zero;
            Random rng = new Random();
            float p,q;
            int nmRays = 5;

            for(int i = 0; i < nmRays; i++)
            {
                p = (float)rng.NextDouble();
                q = (float)rng.NextDouble();
                Ray ray1 = scene.ReturnClosestIntersection(camera.CreatePrimaryRay(x + p , y + q));
                color += returnColor(ray1);
            }

            color = color / nmRays;
            
            return color;
        }
    }
}
