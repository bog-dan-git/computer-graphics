using System;
using System.Drawing;
using System.Numerics;
using ComputerGraphics.RayTracing.Core.Entities.Pdfs;

namespace ComputerGraphics.RayTracing.Core.Entities.Materials
{
    public class NormalMaterial : Material
    {
        public override bool Scatter(Ray inRay, HitResult result, out Vector3 attenuation, out Ray scattered)
        {
            throw new NotImplementedException();
        }
        
        public override bool Scatter(in Ray inRay, in HitResult hitResult, out ScatterResult scatter)
        {
            scatter = new ScatterResult
            {
                Attenuation = hitResult.Normal,
                IsSpecular = false,
                Pdf = new CosinePdf() {Basis = OrthonormalBasis.FromW(hitResult.Normal)},
                SpecularRay = new Ray(Vector3.Zero, Vector3.Zero)
            };
            return false;
        }

        public override float ScatteringPdf(in Ray ray, in HitResult hitResult, in Ray scattered)
        {
            var cosine = Vector3.Dot(hitResult.Normal, Vector3.Normalize(scattered.Direction));
            return cosine < 0 ? 0 : cosine / MathF.PI;
        }
    }
}