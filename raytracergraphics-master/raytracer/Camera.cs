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
        public Vector3 viewDirection;
        Vector3 left, right, up, down;
        public double AppliedFieldOfView
        {
            get {
                return 1 / Math.Tan(fieldOfView / 2);
            }
        }
        public Vector3 Left
        {
            get { return Vector3.Normalize(-Vector3.Cross(viewDirection, up)); }
        }
        public Vector3 Up
        {
            get { return Vector3.Cross(viewDirection, left); }
        }
        public Vector3 Right
        {
            get { return -Left; }
        }
        public Vector3 Down
        {
            get { return -Up; }
        }
        public double fieldOfView;
        float displayWidth;
        float displayHeight;

        public Camera(Vector3 pos, Vector3 dir, float screenWidth, float screenHeight)
        {
            position = pos;
            direction = dir;
            viewDirection = Vector3.Normalize(direction - position);
            fieldOfView = Math.PI*0.5;
            displayWidth = screenWidth;
            displayHeight = screenHeight;
            left = new Vector3(-1, 0, 0);
            right = new Vector3(1, 0, 0);
            up = new Vector3(0, -1, 0);
            down = new Vector3(0, 1, 0);
            
        }
        public Vector3 Upperleft
        {
            get { return (Center +Up+Left); }
        }
        public Vector3 Upperright
        {
            get { return (Center + Up+Right); }
        }
        public Vector3 Downleft
        {
            get { return (Center + Down+Left); }
        }
        public Vector3 Center
        {
            get { return (position + (float)AppliedFieldOfView * viewDirection); }
        }
        public Ray CreatePrimaryRay(int x, int y)
        {
            float u = x * displayWidth*2;
            float v = y * displayHeight;
            Vector3 point = Upperleft + u * (Upperright - Upperleft) + v * (Downleft - Upperleft);
            Vector3 direction = point - this.position;
            Ray r = new Ray(this.position, direction,100f);
            return r;
        }
        public void RotateRight()
        {
            Vector3 target = position + viewDirection;
            target += Vector3.Cross(Center,Up);
            viewDirection = Vector3.Normalize(target - position);
        }
        public void RotateUp()
        {
            Vector3 target = position + viewDirection;
            target += Vector3.Cross(Center, Left);
            viewDirection = Vector3.Normalize(target - position);
        }
        public void RotateLeft()
        {
            Vector3 target = position + viewDirection;
            target += Vector3.Cross(Center, Down);
            viewDirection = Vector3.Normalize(target - position);
        }
        public void RotateDown()
        {
            Vector3 target = position + viewDirection;
            target += Vector3.Cross(Center, Right);
            viewDirection = Vector3.Normalize(target - position);
        }
    }
}
