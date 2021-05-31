using System;
using System.Numerics;

namespace ComputerGraphics.RayTracing.Core.Entities.Pdfs
{
    public class MixedPdf : Pdf
    {
        public Pdf Pdf0 { get; set; }
        public Pdf Pdf1 { get; set; }

        public MixedPdf(Pdf pdf0, Pdf pdf1)
        {
            Pdf0 = pdf0;
            Pdf1 = pdf1;
        }


        public override float Value(Vector3 direction)
        {
            return .5f * Pdf0.Value(direction) + .5f * Pdf1.Value(direction);
        }

        public override Vector3 Generate()
        {
            var random = new Random();
            return random.NextDouble() < .5 ? Pdf0.Generate() : Pdf1.Generate();
        }
    }
}