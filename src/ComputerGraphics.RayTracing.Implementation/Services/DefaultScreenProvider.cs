using ComputerGraphics.RayTracing.Core.Interfaces;

namespace ComputerGraphics.RayTracing.Implementation.Services
{
    internal class DefaultScreenProvider : IScreenProvider
    {
        public int Width => 1920;
        public int Height => 1080;
    }
}