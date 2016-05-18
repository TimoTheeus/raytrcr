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
        protected Vector3 position;
        protected Vector3 direction;
        protected float fieldOfView;

        public Camera(Vector3 pos, Vector3 dir)
        {
            position = pos;
            direction = dir;
            fieldOfView = 1;
        }
        public Vector3 upperleft
        {
            get { return (Center + new Vector3(-1, -1, 0)); }
        }
        public Vector3 upperright
        {
            get { return (Center + new Vector3(1, -1, 0)); }
        }
        public Vector3 downleft
        {
            get { return (Center + new Vector3(-1, 1, 0)); }
        }
        public Vector3 Center
        {
            get { return (position + fieldOfView * direction); }
        }
    }
}
