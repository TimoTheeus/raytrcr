using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Template
{
    class Camera
    {
        //member variables
        public Vector3 position;
        public Vector3 direction;
        public float fieldOfView;
        float displayWidth;
        float displayHeight;

        public Camera(Vector3 pos, Vector3 dir, float screenWidth, float screenHeight)
        {
            position = pos;
            direction = dir;
            fieldOfView = 1;
            displayWidth = screenWidth;
            displayHeight = screenHeight;
        }
        public Vector3 Upperleft
        {
            get { return (Center + new Vector3(-1, -1, 0)); }
        }
        public Vector3 Upperright
        {
            get { return (Center + new Vector3(1, -1, 0)); }
        }
        public Vector3 Downleft
        {
            get { return (Center + new Vector3(-1, 1, 0)); }
        }
        public Vector3 Center
        {
            get { return (position + fieldOfView * direction); }
        }
        public Ray CreatePrimaryRay(int x, int y)
        {
            float u = x * displayWidth;
            float v = y * displayHeight;
            Vector3 point = Upperleft + u * (Upperright - Upperleft) + v * (Downleft - Upperleft);
            Vector3 direction = point - this.position;
            Ray r = new Ray(this.position, direction,100f);
            return r;
        }
    }
}
