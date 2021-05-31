using System;
using System.Numerics;
using ComputerGraphics.RayTracing.Core.Entities;
using ComputerGraphics.RayTracing.Core.Interfaces;

namespace ComputerGraphics.RayTracing.Implementation.Services
{
    internal class CameraRayProvider : IRayProvider
    {
        private readonly IScreenProvider _screenProvider;
        private readonly ICameraProvider _cameraProvider;

        public CameraRayProvider(ICameraProvider cameraProvider, IScreenProvider screenProvider) =>
            (_cameraProvider, _screenProvider) = (cameraProvider, screenProvider);

        public Ray GetRay(float x, float y)
        {
            int width = _screenProvider.Width;
            int height = _screenProvider.Height;
            var scale = MathF.Tan(_cameraProvider.AngleDeg * MathF.PI / 2 / 180);
            var aspectRatio = (float) width / height;
            var px = (2 * (x + .5f) / width - 1.0f) * scale * aspectRatio;
            var py = (1 - 2 * (y + .5f) / height) * scale;
            var direction = _cameraProvider.Direction + new Vector3(px, py, 0) - _cameraProvider.Origin;
            var ray = new Ray(_cameraProvider.Origin, Vector3.Normalize(direction));
            return ray;
        }
    }
}