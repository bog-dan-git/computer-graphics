using System.Numerics;
using ComputerGraphics.Common;
using ComputerGraphics.RayTracing.Core.Entities;
using ComputerGraphics.RayTracing.Core.Entities.SceneObjects;
using ComputerGraphics.RayTracing.Core.Extensions;

namespace ComputerGraphics.RayTracing.Entities.Entities
{
    public class Pyramid : SceneObject
    {
        private readonly Triangle[] _triangles = new Triangle[4];

        public Pyramid(float length, float height, Transform transform)
        {
            var baseVertices = new[]
            {
                new Vector3(-.5f, 0, .5f) * length,
                new Vector3(-.5f, 0, -.5f) * length,
                new Vector3(.5f, 0, -.5f) * length,
                new Vector3(.5f, 0, .5f) * length
            };

            var zenith = new Vector3(0,height,0);
            _triangles[0] = new Triangle(baseVertices[0], zenith, baseVertices[3]);
            _triangles[1] = new Triangle(baseVertices[3], zenith, baseVertices[2]);
            _triangles[2] = new Triangle(baseVertices[2], zenith, baseVertices[1]);
            _triangles[3] = new Triangle(baseVertices[1], zenith, baseVertices[0]);
            Transform = transform;
            ApplyDefaultTransform();
        }

        public override HitResult? Hit(Ray r)
        {
            var result = _triangles.Hit(r);
            if (result is { } hitResult)
            {
                hitResult.Material = Material;
                return hitResult;
            }

            return null;
        }

        private void ApplyDefaultTransform()
        {
            foreach (var triangle in _triangles)
            {
                triangle.Transform(new TransposedTransformationMatrixBuilder()
                    .Move(Transform.Position)
                    .Rotate(Transform.Rotation)
                    .Scale(Transform.Scale)
                    .Build());
            }
        }
    }
}