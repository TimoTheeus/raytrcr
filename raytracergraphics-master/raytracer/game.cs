using System;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace Template {

class Game
{
	    // member variables
	    public Surface screen;
        public Raytracer r;
        protected Scene s;
        protected Camera cam;
        protected List<Primitive> primitiveList;
        protected List<Light> lightList;
        Application app;
        // initialize
        public void Init()
	    {
            lightList = new List<Light>();
            primitiveList = new List<Primitive>();
            s = new Scene(primitiveList,lightList);
            cam = new Camera(new Vector3(0, 0, 0), new Vector3(0, 0, 1),1/((float)screen.width),1/((float)screen.height));
            r = new Raytracer(s,cam,screen);
            app = new Application(r);
            AddPrimitives();
	    }
        // tick: renders one frame
        public void Tick()
        {
            screen.Clear(0);
            screen.Print("hello world", 2, 2, 0xffffff);
            app.Visualize();
            screen.Box(ScreenCoordinatesX(cam.position.X + 5) - 1, ScreenCoordinatesZ(cam.position.Z), ScreenCoordinatesX(cam.position.X + 5) + 1, ScreenCoordinatesZ(cam.position.Z) + 2, 256 * 256 * 255 + 256 * 255);

            foreach (Primitive primitive in primitiveList)
            {
                if (primitive is Sphere)
                {
                    Sphere sphere = primitive as Sphere;
                    for (int i = 0; i < 100; i++)
                    {
                        screen.Line(ScreenCoordinatesX(Math.Cos(i / (2 * Math.PI)) + sphere.position.X + 5), ScreenCoordinatesZ(Math.Sin(i / (2 * Math.PI)) + sphere.position.Z), ScreenCoordinatesX(Math.Cos((i + 1) / (2 * Math.PI)) + sphere.position.X + 5), ScreenCoordinatesZ(Math.Sin((i + 1) / (2 * Math.PI)) + sphere.position.Z), ConvertToColor(sphere.color));
                    }
                }
            }
            Console.WriteLine(cam.viewDirection);
            screen.Line(ScreenCoordinatesX(cam.position.X + 5 + cam.viewDirection.X - cam.viewDirection.Z),
                ScreenCoordinatesZ(cam.position.Z + cam.viewDirection.Z + cam.viewDirection.X / Math.Sqrt(Math.Pow(cam.viewDirection.X, 2) + Math.Pow(cam.viewDirection.Z, 2))),
                ScreenCoordinatesX(cam.position.X + 5 + cam.viewDirection.X + cam.viewDirection.Z),
                ScreenCoordinatesZ(cam.position.Z + cam.viewDirection.Z - cam.viewDirection.X / Math.Sqrt(Math.Pow(cam.viewDirection.X, 2) + Math.Pow(cam.viewDirection.Z, 2))),
                255*256*256 + 255*256 + 255);
        }

        public int ScreenCoordinatesX(double number)
        {
            return (int)(number * 50 + 512);
        }

        public int ScreenCoordinatesZ(double number)
        {
            return (int)((10 - number) * 50);
        }
        
        public int ConvertToColor(Vector3 vector)
        {
            float number = 0;
            number += vector.X * 256 * 256;
            number += vector.Y * 256;
            number += vector.Z;
            return (int)number;
        }

        void AddPrimitives()
        {
            s.AddPrimitive(new Sphere(new Vector3(0, 0, 3f), 1f, new Vector3(255, 0, 0)));
            s.AddPrimitive(new Sphere(new Vector3(-2.5f, 0, 3f), 1f, new Vector3(0, 255, 0)));
            s.AddPrimitive(new Sphere(new Vector3(2.5f, 0, 3f), 1f, new Vector3(0, 0, 255)));
            s.AddPrimitive(new Plane(new Vector3(0, 1, 0), 1f, new Vector3(0, 255, 255)));
            s.AddLightSource(new Light(new Vector3(0, -1, 0), new Vector3(50f, 50f, 50f)));
        }
    }

} // namespace Template