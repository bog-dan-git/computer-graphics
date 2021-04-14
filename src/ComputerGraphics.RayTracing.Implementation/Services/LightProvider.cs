using System.Numerics;
using ComputerGraphics.RayTracing.Core.Interfaces;

namespace ComputerGraphics.RayTracing.Implementation.Services
{
    internal class LightProvider : ILightProvider
    {
        public Vector3 Origin => new(0, 0, 0);
    }
}