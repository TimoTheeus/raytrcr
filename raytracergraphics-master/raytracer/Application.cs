using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Input;
using OpenTK;

namespace Template
{
    //Handle input 
    class Application
    {
        protected Raytracer raytracer;

        public Application(Raytracer raytracer)
        {
            this.raytracer = raytracer;
        }
        public void Visualize()
        {
            raytracer.Render();
            HandleInput();
        }
        public void HandleInput()
        {
            var state = Keyboard.GetState();
            if (state.IsKeyDown(Key.Right))
            {
                raytracer.camera.RotateRight();
            }
            if (state.IsKeyDown(Key.Up))
            {
                raytracer.camera.RotateUp();
            }
            if (state.IsKeyDown(Key.Down))
            {
                raytracer.camera.RotateDown();
            }
            if (state.IsKeyDown(Key.Left))
            {
                raytracer.camera.RotateLeft();
            }
        }
    }
}
