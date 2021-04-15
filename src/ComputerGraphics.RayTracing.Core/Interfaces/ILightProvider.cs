using System.Numerics;

namespace ComputerGraphics.RayTracing.Core.Interfaces
{
    public interface ILightProvider
    {
        Vector3 Origin { get; }
    }
}