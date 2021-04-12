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
            var lightAbs = Vector3.Dot(hitResult.Normal, lightDirection);
            return new Vector3(lightAbs, lightAbs, lightAbs);
        }
    }
}