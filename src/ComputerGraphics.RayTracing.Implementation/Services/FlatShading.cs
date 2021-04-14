using System;
using System.Numerics;
using ComputerGraphics.RayTracing.Core.Entities;
using ComputerGraphics.RayTracing.Core.Interfaces;

namespace ComputerGraphics.RayTracing.Implementation.Services
{
    internal class FlatShading : ILightingStrategy
    {
        public Vector3 Illuminate(ILightProvider lightProvider, HitResult hitResult)
        {
            var lightOrigin = lightProvider.Origin;
            var lightDirection = Vector3.Normalize(lightOrigin - hitResult.P);
            var lightAbs = Math.Clamp(MathF.Abs(Vector3.Dot(Vector3.Normalize(hitResult.Normal), lightDirection)), 0, 1);
            return new Vector3(205f * lightAbs / 255f, 40f * lightAbs / 255f, 40f * lightAbs / 255f);
        }
    }
}