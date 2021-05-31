using System.Numerics;
using ComputerGraphics.Common;
using ComputerGraphics.RayTracing.Core.Entities;
using ComputerGraphics.RayTracing.Core.Entities.SceneObjects;

namespace ComputerGraphics.RayTracing.Entities.Entities
{
    public class Plane : SceneObject
    {
        private const float Epsilon = 0.00001f;

        public override HitResult? Hit(Ray r)
        {
            var transformationMatrix = new TransposedTransformationMatrixBuilder()
                .Move(Transform.Position)
                .Rotate(Transform.Rotation)
                .Build();

            var normal = new Vector3(transformationMatrix.M21, transformationMatrix.M22, transformationMatrix.M23);
            var planePosition = transformationMatrix.Translation;

            var denom = Vector3.Dot(normal, r.Direction);
            if (denom > Epsilon)
            {
                return null;
            }

            var t = Vector3.Dot(planePosition - r.Origin, normal) / denom;
            if (t < 0)
            {
                return null;
            }

            var intersectionPoint = r.Origin + r.Direction * t;

            return new HitResult
            {
                Normal = normal, P = intersectionPoint, T = t, Material = Material
            };
        }
    }
}