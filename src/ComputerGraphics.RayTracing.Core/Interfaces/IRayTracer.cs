using System.Numerics;
using ComputerGraphics.RayTracing.Core.Entities;

namespace ComputerGraphics.RayTracing.Core.Interfaces
{
    public interface IRayTracer
    {
        Vector3[,] Trace(Scene scene);
    }
}