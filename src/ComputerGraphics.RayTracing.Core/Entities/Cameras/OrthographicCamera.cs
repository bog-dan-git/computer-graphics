using System.Numerics;
using ComputerGraphics.Common;

namespace ComputerGraphics.RayTracing.Core.Entities.Cameras
{
    public class OrthographicCamera : Camera
    {
        private Matrix4x4? _rotationMatrix;
        private Matrix4x4? _transformationMatrix;

        private readonly double _viewportWidth;
        private readonly double _viewportHeight;
        private readonly Vector3 _lowerLeftCorner;
        
        public OrthographicCamera(float aspectRatio)
        {
            _viewportHeight = 1;
            _viewportWidth = _viewportHeight * aspectRatio;
            _lowerLeftCorner = Origin - aspectRatio * Vector3.UnitX / 2 - Vector3.UnitY / 2;
        }

        public override Ray GetRay(float x, float y)
        {
            var dir = Vector3.Normalize(Vector3.Transform(Direction, GetRotationMatrix()));
            var origin = Vector3.Transform(new Vector3(
                    (float) (_lowerLeftCorner.X + x * _viewportWidth),
                    (float) (_lowerLeftCorner.Y + y * _viewportHeight), 0),
                GetTransformationMatrix());
            
            return new Ray(origin, dir);
        }
        
        private Matrix4x4 GetRotationMatrix()
        {
            _rotationMatrix ??= new TransposedTransformationMatrixBuilder()
                .Rotate(Transform.Rotation)
                .Build();

            return _rotationMatrix.Value;
        }

        private Matrix4x4 GetTransformationMatrix()
        {
            _transformationMatrix ??= new TransposedTransformationMatrixBuilder()
                .Move(Transform.Position)
                .Rotate(Transform.Rotation)
                .Scale(Transform.Scale)
                .Build();

            return _transformationMatrix.Value;
        }
        
    }
}