using System;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Template
{

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
            s = new Scene(primitiveList, lightList);
            cam = new Camera(new Vector3(0, 0, 0), new Vector3(0, 0, 1), 1 / ((float)screen.width), 1 / ((float)screen.height));
            r = new Raytracer(s, cam, screen);
            app = new Application(r);
            AddPrimitives();

        }
        // tick: renders one frame
        public void Tick()
        {
            screen.Clear(0);
            app.Visualize(primitiveList);

        }
        //initialize all primitives
        void AddPrimitives()
        {
            //Specular bollen met een kleur, waarvan er eentje ook dielectric is.
            Sphere specularSphere = new Sphere(new Vector3(2.5f, 0, 4f), 1f, new Vector3(1f, 1f, 1f));
            Sphere specularSphere2 = new Sphere(new Vector3(-2.5f, 0, 4f), 1f, new Vector3(0, 1f, 0));
            Sphere specularSphere3 = new Sphere(new Vector3(-1, 0, 2f), 1f, new Vector3(1f, 1f, 1f));
            specularSphere.isSpecular = true;
            specularSphere2.isSpecular = true;
            specularSphere3.isDielectric = true;
            specularSphere.specularity = 1f;
            specularSphere2.specularity = 0.5f;
            specularSphere3.specularity = 1f;
            s.AddPrimitive(specularSphere);
            s.AddPrimitive(specularSphere2);
            s.AddPrimitive(specularSphere3);

            //floor plane
            Plane checkerBoardFloor = new Plane(new Vector3(0, -1, 0), 1f, new Vector3(1f, 1f, 1f), new Vector3(0.5f, 0, 0f));
            checkerBoardFloor.specularity = 0.5f;
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
            light1.spotlightAngle = (float)(Math.PI * 0.09);
            light1.spotLightDirection = new Vector3(0, 1, 0).Normalized();
            s.AddLightSource(light1);
            s.AddLightSource(new Light(new Vector3(-1, -1.5f, 0f), new Vector3(80f, 80f, 80f)));
            s.AddLightSource(new Light(new Vector3(1, -1, 0), new Vector3(70f, 70f, 70f)));
        }

    }

} // namespace Template