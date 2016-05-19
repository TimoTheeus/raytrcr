using System;
using System.IO;
using OpenTK;
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
	    }
        void AddPrimitives()
        {
            s.AddPrimitive(new Sphere(new Vector3(0, 0, 2.5f), 1f, new Vector3(255, 0, 0)));
            s.AddPrimitive(new Sphere(new Vector3(-1, 0, 3), 1f, new Vector3(0, 255, 0)));
            s.AddPrimitive(new Sphere(new Vector3(1, 0, 3), 1f, new Vector3(0, 0, 255)));
            s.AddPrimitive(new Plane(new Vector3(0, 1, 0), 2f, new Vector3(0, 255, 255)));
        }
    }

} // namespace Template