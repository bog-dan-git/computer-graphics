using ComputerGraphics.RayTracing.Core.Entities;

namespace ComputerGraphics.RayTracing.Core.Interfaces
{
    public interface IRayProvider
    {
        Ray GetRay(float x, float y);
    }
}