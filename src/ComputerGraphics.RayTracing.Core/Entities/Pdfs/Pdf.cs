using System.Numerics;

namespace ComputerGraphics.RayTracing.Core.Entities.Pdfs
{
    public abstract class Pdf
    {
        public abstract float Value(Vector3 direction);

        public abstract Vector3 Generate();
    }
}