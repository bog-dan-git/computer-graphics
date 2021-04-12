using ComputerGraphics.RayTracing.Core.Entities;

namespace ComputerGraphics.RayTracing.Core.Interfaces
{
    public interface IHittable
    {
        HitResult? Hit(Ray r, float min, float max);
    }
}