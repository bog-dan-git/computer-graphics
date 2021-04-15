using System.Numerics;
using ComputerGraphics.RayTracing.Core.Interfaces;

namespace ComputerGraphics.RayTracing.Implementation.Services
{
    internal class StaticCameraProvider : ICameraProvider
    {
        public Vector3 Origin => Vector3.Zero;
        public Vector3 Direction => Vector3.UnitZ;

        public int AngleDeg => 90;
    }
}