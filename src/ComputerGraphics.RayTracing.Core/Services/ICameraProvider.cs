using System.Numerics;

namespace ComputerGraphics.RayTracing.Core.Services
{
    public interface ICameraProvider
    {
        Vector3 Origin { get; }
        Vector3 Direction { get; }
        
        int AngleDeg { get; }
    }
}