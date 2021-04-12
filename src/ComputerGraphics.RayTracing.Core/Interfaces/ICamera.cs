using ComputerGraphics.RayTracing.Core.Entities;

namespace ComputerGraphics.RayTracing.Core.Interfaces
{
    public interface ICamera : ITransformable
    {
        Ray LookAt(int x, int y);
    }
}