using System.Collections.Generic;
using System.Numerics;
using ComputerGraphics.RayTracing.Core.Entities.SceneObjects;
using ComputerGraphics.RayTracing.Core.Extensions;

namespace ComputerGraphics.RayTracing.Core.Entities.Pdfs
{
    public class Hittable : Pdf
    {
        public Vector3 O { get; set; }
        public IEnumerable<SceneObject> Lights { get; set; }

        public Hittable(Vector3 o, IEnumerable<SceneObject> lights)
        {
            O = o;
            Lights = lights;
        }
        public override float Value(Vector3 direction)
        {
            return Lights.PdfValue(O, direction);
        }

        public override Vector3 Generate()
        {
            return Lights.Random(O);
        }
    }
}