﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Input;
using OpenTK;
using System.Diagnostics;

namespace Template
{
    //Handle input 
    class Application
    {
        protected Raytracer raytracer;
        Stopwatch t;
        double oneThousandth;

        public Application(Raytracer raytracer)
        {
            this.raytracer = raytracer;
            t = new Stopwatch();
            oneThousandth = (double)(1m / 1000m);
        }
        public void Visualize()
        {
            t.Reset();
            t.Start();
            raytracer.Render();
            double timeElapsedMilliseconds = t.Elapsed.TotalMilliseconds;
            t.Stop();
            HandleInput(timeElapsedMilliseconds);
        }
        public void HandleInput(double timeElapsedMilliseconds)
        {
            double timeElapsed = (timeElapsedMilliseconds* oneThousandth);
            var state = Keyboard.GetState();
            if (state.IsKeyDown(Key.Right))
            {
                raytracer.camera.RotateRight(timeElapsed);
            }
            if (state.IsKeyDown(Key.Up))
            {
                raytracer.camera.RotateUp(timeElapsed);
            }
            if (state.IsKeyDown(Key.Down))
            {
                raytracer.camera.RotateDown(timeElapsed);
            }
            if (state.IsKeyDown(Key.Left))
            {
                raytracer.camera.RotateLeft(timeElapsed);
            }
        }
    }
}
