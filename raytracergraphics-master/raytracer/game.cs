using System;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

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
            //Specular bollen met een kleur
            Sphere specularSphere = new Sphere(new Vector3(2.5f, 0, 4f), 1f, new Vector3(1f, 1f, 1f));
            Sphere specularSphere2 = new Sphere(new Vector3(-2.5f, 0, 4f), 1f, new Vector3(0, 1f, 0));
            Sphere specularSphere3 = new Sphere(new Vector3(0, 0, 3f), 1f, new Vector3(1f, 0, 0));
            specularSphere.isSpecular = true;
            specularSphere2.isSpecular = true;
            specularSphere3.isSpecular = true;
            specularSphere.specularity = 1f;
            specularSphere2.specularity = 0.5f;
            specularSphere3.specularity = 0.7f;
            s.AddPrimitive(specularSphere);
            s.AddPrimitive(specularSphere2);
            s.AddPrimitive(specularSphere3);

            //floor plane
            Plane checkerBoardFloor = new Plane(new Vector3(0, -1, 0), 1f, new Vector3(1f, 1f, 1f), new Vector3(0.5f, 0, 0f));
            checkerBoardFloor.specularity=0.5f;
            checkerBoardFloor.isSpecular = true;
            s.AddPrimitive(checkerBoardFloor);
            //ceiling plane
            s.AddPrimitive(new Plane(new Vector3(0, 1, 0), 4f, new Vector3(0.5f, 0.33f, 0.75f), new Vector3(0.5f, 0.33f, 0.75f)));
            //plane in the back and behind camera
            s.AddPrimitive(new Plane(new Vector3(0, 0, -1), 6f, new Vector3(0.5f, 0.33f, 0.75f), new Vector3(0.5f, 0.33f, 0.75f)));
            s.AddPrimitive(new Plane(new Vector3(0, 0, 1), 3f, new Vector3(0.5f, 0.33f, 0.75f), new Vector3(0.5f, 0.33f, 0.75f)));
            //left and right plane
            s.AddPrimitive(new Plane(new Vector3(1, 0, 0), 5f, new Vector3(0.5f, 0.33f, 0.75f), new Vector3(0.5f, 0.33f, 0.75f)));
            s.AddPrimitive(new Plane(new Vector3(-1, 0, 0), 5f, new Vector3(0.5f, 0.33f, 0.75f), new Vector3(0.5f, 0.33f, 0.75f)));
            //Lightsources
            Light light1 = new Light(new Vector3(0, -3f, 2f), new Vector3(200f, 200f, 200f));
            light1.isSpotlight = true;
            light1.spotlightAngle = (float)(Math.PI*0.09);
            light1.spotLightDirection = new Vector3(0,1,0).Normalized();
            s.AddLightSource(light1);
            s.AddLightSource(new Light(new Vector3(-1, -1.5f, 0f), new Vector3(80f, 80f, 80f)));
            s.AddLightSource(new Light(new Vector3(1, -1, 0), new Vector3(70f, 70f, 70f)));
        }

    }

} // namespace Template