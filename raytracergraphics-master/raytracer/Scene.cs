using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Template
{
    class Scene
    {
        public List<Primitive> primitives;
        public List<Light> lightsources;
        public Scene(List<Primitive> pList, List<Light> lList)
        {
            primitives = pList;
            lightsources = lList;
        }

        public Vector3 Intersection(Ray ray)
        {
            Primitive k;
            foreach (Primitive p in primitives)
            {

            }
            return new Vector3(0, 0, 0);
        }
    }
}
