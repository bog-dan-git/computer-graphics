using System;
using System.Numerics;
using ComputerGraphics.Common;

namespace ComputerGraphics.RayTracing.Core.Entities.Cameras
{
    public class PerspectiveCamera : Camera
    {
        private readonly Vector3 _lowerLeftCorner;
        private readonly double _viewportWidth;
        private readonly double _viewportHeight;

        public float HFov { get; }

        private Matrix4x4? _rotationMatrix;
        private Matrix4x4? _transitionMatrix;

        public PerspectiveCamera(float hFov, float aspectRatio)
        {
            HFov = hFov;
            var hFovRad = hFov * MathF.PI / 180;

            _viewportHeight = 2.0 * MathF.Tan(hFovRad / 2);
            _viewportWidth = aspectRatio * _viewportHeight;

            _lowerLeftCorner = Origin - new Vector3(0, (float) (_viewportHeight / 2), 0)
                                      - new Vector3((float) (_viewportWidth / 2), 0, 0)
                               + Direction;
        }

        public override Ray GetRay(float x, float y)
        {
            var rayDir = new Vector3(
                (float) (_lowerLeftCorner.X + x * _viewportWidth),
                (float) (_lowerLeftCorner.Y + y * _viewportHeight),
                0) + Direction;

            return new Ray(
                Vector3.Transform(Origin, GetTransitionMatrix()),
                Vector3.Normalize(Vector3.Transform(rayDir, GetRotationMatrix()))
            );
        }

        private Matrix4x4 GetRotationMatrix()
        {
            _rotationMatrix ??= new TransposedTransformationMatrixBuilder()
                .Rotate(Transform.Rotation)
                .Build();

            return _rotationMatrix.Value;
        }

        private Matrix4x4 GetTransitionMatrix()
        {
            _transitionMatrix ??= new TransposedTransformationMatrixBuilder()
                .Move(Transform.Position)
                .Build();

            return _transitionMatrix.Value;
        }
    }
}