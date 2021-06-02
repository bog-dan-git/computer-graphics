using ComputerGraphics.RayTracing.Core.Interfaces;

namespace ComputerGraphics.RayTracing.Implementation.Services
{
    internal class DefaultScreenProvider : IScreenProvider
    {
        public int Width => 1280;
        public int Height => 720;
    }
}