using System;
using System.Numerics;
using ComputerGraphics.Common;
using ComputerGraphics.RayTracing.Core.Entities.Pdfs;
using ComputerGraphics.RayTracing.Core.Entities.Textures;

namespace ComputerGraphics.RayTracing.Core.Entities.Materials
{
    public class LambertReflectionMaterial : Material
    {
        public Texture Albedo { get; set; }
        public Vector3 Color { get; set; }

        public sealed override bool Scatter(Ray inRay, HitResult result, out Vector3 attenuation, out Ray scattered)
        {
            var scatterDirection = result.Normal + Vector3Extensions.RandomInUnitSphere();
            if (scatterDirection.NearZero())
            {
                scatterDirection = result.Normal;
            }

            scattered = new Ray(result.P, scatterDirection);
            attenuation = Color;
            return true;
        }


        public sealed override bool Scatter(in Ray inRay, in HitResult hitResult, out ScatterResult scatter)
        {
            scatter = new ScatterResult
            {
                IsSpecular = false,
                Attenuation = Albedo.ValueAt(hitResult.TextureCoordinates.X, hitResult.TextureCoordinates.Y,
                    hitResult.P),
                Pdf = new CosinePdf() {Basis = OrthonormalBasis.FromW(hitResult.Normal)}
            };
            return true;
        }

        public override float ScatteringPdf(in Ray ray, in HitResult hitResult, in Ray scattered)
        {
            var cosine = Vector3.Dot(hitResult.Normal, Vector3.Normalize(scattered.Direction));
            return cosine < 0 ? 0 : cosine / MathF.PI;
        }
    }
}