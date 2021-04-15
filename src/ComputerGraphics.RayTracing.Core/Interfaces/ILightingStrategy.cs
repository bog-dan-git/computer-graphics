using System.Numerics;
using ComputerGraphics.RayTracing.Core.Entities;

namespace ComputerGraphics.RayTracing.Core.Interfaces
{
    public interface ILightingStrategy
    {
        Vector3 Illuminate(ILightProvider lightProvider, HitResult hitResult);
    }
}