using System;
using System.Numerics;
using ComputerGraphics.RayTracing.Core.Entities;
using ComputerGraphics.RayTracing.Core.Interfaces;
using ComputerGraphics.RayTracing.Core.Services;
using ComputerGraphics.RayTracing.Implementation.Builders;

namespace ComputerGraphics.RayTracing.Implementation.Entities
{
    internal class StaticCamera : ICamera
    {
        private Vector3 _origin;
        private Vector3 _direction;
        private readonly IScreenProvider _screen;
        private readonly float _angleDeg;

        public StaticCamera(ICameraProvider cameraProvider, IScreenProvider screen)
        {
            _screen = screen;
            _origin = cameraProvider.Origin;
            _direction = cameraProvider.Direction;
            Transform(new TransposedTransformationMatrixBuilder().Build());
            _angleDeg = cameraProvider.AngleDeg;
        }

        public Ray LookAt(int x, int y)
        {
            var scale = MathF.Tan(_angleDeg * MathF.PI / 2 / 180);
            var aspectRatio = (float) _screen.Width / _screen.Height;
            var px = (2 * (x + .5f) / _screen.Width - 1.0f) * scale * aspectRatio;
            var py = (1 - 2 * (y + .5f) / _screen.Height) * scale;
            var direction = _direction + new Vector3(px, py, 0) - _origin;
            var ray = new Ray(_origin, Vector3.Normalize(direction));
            return ray;
        }

        public void Transform(Matrix4x4 matrix4X4)
        {
            _direction = Vector3.Transform(_direction, matrix4X4);
            _origin = Vector3.Transform(_origin, matrix4X4);
        }
    }
}