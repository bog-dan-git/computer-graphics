using ComputerGraphics.RayTracing.Core.Entities;

namespace ComputerGraphics.RayTracing.Core.Interfaces
{
    public interface IRayProvider
    {
        Ray GetRay(int x, int y);
    }
}