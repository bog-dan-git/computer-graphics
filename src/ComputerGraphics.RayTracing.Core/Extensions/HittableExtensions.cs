using System.Collections.Generic;
using ComputerGraphics.RayTracing.Core.Entities;
using ComputerGraphics.RayTracing.Core.Interfaces;

namespace ComputerGraphics.RayTracing.Core.Extensions
{
    public static class HittableExtensions
    {
        public static HitResult? Hit<T>(this IEnumerable<T> hittables, Ray ray) where T : IHittable
        {
            float closestSoFar = float.MaxValue;
            HitResult? hit = null;
            foreach (var hittable in hittables)
            {
                var hitResult = hittable.Hit(ray);
                if (hitResult is { } result && result.T < closestSoFar)
                {
                    closestSoFar = hitResult.Value.T;
                    hit = hitResult;
                }
            }

            return hit;
        }
    }
}