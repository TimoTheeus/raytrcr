using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
namespace Template
{
    class Light
    {
        //member variables
        protected Vector3 location;
        protected Vector3 intensity;

        //constructor
        public Light(Vector3 loc, Vector3 i)
        {
            location = loc;
            intensity = i;
        }
    }
}
