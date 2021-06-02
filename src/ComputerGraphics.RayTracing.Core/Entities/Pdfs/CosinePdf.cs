using System;
using System.Numerics;
using ComputerGraphics.Common;

namespace ComputerGraphics.RayTracing.Core.Entities.Pdfs
{
    public class CosinePdf : Pdf
    {
        public OrthonormalBasis Basis { get; set; }
        public override float Value(Vector3 direction)
        {
            var cosine = Vector3.Dot(Vector3.Normalize(direction), Basis.W);
            return (cosine <= 0) ? 0 : cosine / MathF.PI;
        }

        public override Vector3 Generate()
        {
            return Basis.Local(Vector3Extensions.RandomCosineDirection());
        }
    }
}