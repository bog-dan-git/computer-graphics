using ComputerGraphics.RayTracing.Core.Entities;
using ComputerGraphics.RayTracing.Core.Entities.Cameras;
using ComputerGraphics.RayTracing.Core.Interfaces;

namespace ComputerGraphics.RayTracing.Implementation.Services
{
    internal class CameraRayProvider : IRayProvider
    {
        private readonly IScreenProvider _screenProvider;
        private readonly Camera _camera;

        public CameraRayProvider(Camera camera, IScreenProvider screenProvider) => (_camera, _screenProvider) = (camera, screenProvider);

        public Ray GetRay(float x, float y)
        {
            return _camera.GetRay(
                x / (_screenProvider.Width - 1),
                (_screenProvider.Height - y) / (_screenProvider.Height - 1)
                );
        }
    }
}