using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Template
{
    class Ray
    {
        public readonly Vector3 Origin;
        public readonly Vector3 Direction;
        public readonly float Distance;

        public Ray(Vector3 start, Vector3 direction, float distance)
        {
            this.Origin = start;
            this.Direction = direction;
            this.Distance = distance;
        }
    }
}
