﻿using System.Numerics;
using ComputerGraphics.Common;

namespace ComputerGraphics.RayTracing.Core.Entities.Materials
{
    public class MetalMaterial : Material
    {
        public float Fuzz { get; set; } = 0f;
        
        public override bool Scatter(in Ray inRay, in HitResult hitResult, out ScatterResult scatter)
        {
            var reflected = Vector3.Reflect(Vector3.Normalize(inRay.Direction), hitResult.Normal);
            var scatterResult = new ScatterResult
            {
                SpecularRay = new Ray(hitResult.P, reflected + Fuzz * Vector3Extensions.RandomInUnitSphere()),
                Attenuation = Color, IsSpecular = true, Pdf = null
            };
            scatter = scatterResult;
            return true;
        }

        public Vector3 Color { get; set; }
    }
}