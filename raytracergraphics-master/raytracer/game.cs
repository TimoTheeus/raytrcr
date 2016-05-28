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
		    screen.Clear( 0 );
		    screen.Print( "hello world", 2, 2, 0xffffff );
            app.Visualize();
            screen.Box(ScreenCoordinatesX(cam.position.X + 5) - 1, ScreenCoordinatesZ(cam.position.Z), ScreenCoordinatesX(cam.position.X + 5) + 1, ScreenCoordinatesZ(cam.position.Z) + 2, 256*256*255 + 256 * 255);

            foreach (Primitive primitive in primitiveList)
            {
                if (primitive is Sphere)
                {
                    Sphere sphere = primitive as Sphere;
                    for (int i = 0; i < 100; i++)
                    {
                        screen.Line(ScreenCoordinatesX(Math.Cos(i / (2 * Math.PI)) + sphere.position.X + 5), ScreenCoordinatesZ(Math.Sin(i / (2 * Math.PI)) + sphere.position.Z), ScreenCoordinatesX(Math.Cos((i + 1)/ (2 * Math.PI)) + sphere.position.X + 5), ScreenCoordinatesZ(Math.Sin((i + 1)/(2 * Math.PI)) + sphere.position.Z), 255);
                    }
                }
            }

            Vector2 camcoordinates = new Vector2(ScreenCoordinatesX(cam.position.X + 5), ScreenCoordinatesZ(cam.position.Z));

            float length = 5f;

            for (int i = 0; i <= 10; i++)
            {
                screen.Line((int)camcoordinates.X, (int)camcoordinates.Y, ScreenCoordinatesX(cam.position.X + 5 + length * Math.Sin(-cam.fieldOfView + i * cam.fieldOfView/5)), ScreenCoordinatesX((cam.position.Z - length * Math.Cos(-cam.fieldOfView + i * cam.fieldOfView / 5))), Correct(new Vector2(ScreenCoordinatesX(cam.position.X + 5 + length * Math.Sin(-cam.fieldOfView + i * cam.fieldOfView / 5)), ScreenCoordinatesX((cam.position.Z - length * Math.Cos(-cam.fieldOfView + i * cam.fieldOfView / 5))))));
            }

        }

        public int ScreenCoordinatesX(double number)
        {
            return (int)(number * 50 + 512);
        }

        public int ScreenCoordinatesZ(double number)
        {
            return (int)((10 - number) * 50);
        }
        
        public Vector2 ScreenCoordinates(Vector2 vector)
        {
            return new Vector2((int)(vector.X * 50 + 512), (int)((10 - vector.Y) * 50));
        }

        public int Correct(Vector2 vector)
        {
            for (int i = 0; i < 20; i++)
            {
                if (Math.Sqrt(
                    Math.Pow((((ScreenCoordinates(new Vector2(0, 2.5f)) + ScreenCoordinates(new Vector2(cam.position.X + 5, cam.position.Z)) - Vector2.Multiply(Vector2.Divide(vector, 20), (i)))).X), 2) +
                    Math.Pow((((ScreenCoordinates(new Vector2(0, 2.5f)) + ScreenCoordinates(new Vector2(cam.position.X + 5, cam.position.Z)) - Vector2.Multiply(Vector2.Divide(vector, 20), (i)))).Y), 2))
                    < 1500)
                {
                    return 100;
                }
            }
            return 255;

        }

        void AddPrimitives()
        {
            s.AddPrimitive(new Sphere(new Vector3(0, 0, 3f), 1f, new Vector3(1f, 0, 0)));
            s.AddPrimitive(new Sphere(new Vector3(-2.5f, 0, 3f), 1f, new Vector3(0, 1f, 0)));
            s.AddPrimitive(new Sphere(new Vector3(2.5f, 0, 3f), 1f, new Vector3(0, 0, 1f)));
            s.AddPrimitive(new Plane(new Vector3(0, -1, 0), -1f, new Vector3(1f, 1f, 1f), new Vector3(0.2f, 0.2f, 0.2f)));
            s.AddLightSource(new Light(new Vector3(0, -1, 0), new Vector3(50f, 50f, 50f)));
            s.AddLightSource(new Light(new Vector3(1, -1, 0), new Vector3(30f, 30f, 30f)));
        }
    }

} // namespace Template