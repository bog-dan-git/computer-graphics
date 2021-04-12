using System;
using System.Numerics;
using ComputerGraphics.RayTracing.Core.Entities;
using ComputerGraphics.RayTracing.Core.Interfaces;
using ComputerGraphics.RayTracing.Core.Services;

namespace ComputerGraphics.RayTracing.Implementation.Services
{
    internal class CameraRayProvider : IRayProvider
    {
        private readonly IScreenProvider _screenProvider;
        private readonly ICameraProvider _cameraProvider;
        private static readonly float _angle = 0;
        private readonly float _cos = MathF.Cos(_angle);
        private readonly float _sin = MathF.Sin(_angle);

        public CameraRayProvider(ICameraProvider cameraProvider, IScreenProvider screenProvider) =>
            (_cameraProvider, _screenProvider) = (cameraProvider, screenProvider);

        public Ray GetRay(int x, int y)
        {
            var scale = MathF.Tan(_cameraProvider.AngleDeg * MathF.PI / 2 / 180);
            var aspectRatio = (float) _screenProvider.Width / _screenProvider.Height;
            var px = (2 * (x + .5f) / _screenProvider.Width - 1.0f) * scale * aspectRatio;
            var py = (1 - 2 * (y + .5f) / _screenProvider.Height) * scale;
            var direction = _cameraProvider.Direction + new Vector3(px, py, 0) - _cameraProvider.Origin;
            var translationMatrix = Matrix4x4.Transpose(new Matrix4x4(
                1, 0, 0, 0,
                0, 1, 0, 400,
                0, 0, 1, -1000,
                0, 0, 0, 1));
            var rotationMatrix = Matrix4x4.Transpose(new Matrix4x4(
                1, 0,0, 0,
                0, _cos, -_sin, 0,
                0, _sin, _cos, 0,
                0, 0, 0, 1));
            var transformedOrigin = Vector3.Transform(_cameraProvider.Origin, translationMatrix);
            var ray = new Ray(transformedOrigin, Vector3.Normalize(Vector3.Transform(direction, rotationMatrix)));
            return ray;
        }
    }
}