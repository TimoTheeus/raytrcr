using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Template
{
    class Primitive
    {
        //absorption
        public Vector3 color;
        //the normal
        public Vector3 normal;
        //specular or not
        public bool isSpecular;
        //0-1f
        public float specularity;
        //Dielectric or not
        public bool isDielectric;
        //Elements of dielectirc primitives
        public float airToGlassIndex;
        public float glassToAirIndex;
        public float dividedIndexes;
        public float dividedIndexesInverted;
        public float fresnelR;
        public float fresnelRInverted;

        public Primitive(Vector3 c)
        {
            color = c;
            specularity = 0.5f;
            airToGlassIndex = 1.5f;
            glassToAirIndex = 1 / airToGlassIndex;
            dividedIndexes = airToGlassIndex / glassToAirIndex;
            dividedIndexesInverted = glassToAirIndex / airToGlassIndex;
            //Calculate R0 and inverted R0 with Fresnel laws
            fresnelR = (float)Math.Pow(((airToGlassIndex - glassToAirIndex) / (airToGlassIndex + glassToAirIndex)),2);
            fresnelRInverted = (float)Math.Pow(((glassToAirIndex-airToGlassIndex) / (airToGlassIndex + glassToAirIndex)),2);
        }
        public virtual void Intersect(Ray ray)
        {
            
        }
        public virtual Vector3 getColor(Vector3 point)
        {
            return Vector3.Zero;
        }
    }
}
