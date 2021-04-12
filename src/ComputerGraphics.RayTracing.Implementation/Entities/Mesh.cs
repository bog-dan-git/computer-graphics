using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using ComputerGraphics.RayTracing.Core.Entities;
using ComputerGraphics.RayTracing.Core.Interfaces;
using ComputerGraphics.RayTracing.Implementation.Builders;

namespace ComputerGraphics.RayTracing.Implementation.Entities
{
    public class Mesh : IHittable, ITransformable
    {
        private readonly IEnumerable<Triangle> _triangles;

        public Mesh(IEnumerable<Triangle> triangles)
        {
            _triangles = triangles;
            ResetToDefault();
        }

        public void Transform(Matrix4x4 matrix4X4)
        {
            foreach (var triangle in _triangles)
            {
                triangle.Transform(matrix4X4);
            }
        }

        public HitResult? Hit(Ray r, float min, float max)
        {
            HitResult? hitResult = null;
            foreach (var triangle in _triangles)
            {
                var hit = triangle.Hit(r, min, max);
                if (!hit.HasValue) continue;
                if (!hitResult.HasValue)
                {
                    hitResult = hit;
                }
                else
                {
                    if (hitResult.Value.T <
                        hit.Value.T)
                    {
                        hitResult = hit;
                    }
                }
            }

            return hitResult;
        }

        private void ResetToDefault()
        {
            var xs = _triangles.Select(_ => new[] {_.A.X, _.B.X, _.C.X})
                .SelectMany(_ => _);
            var ys = _triangles.Select(_ => new[] {_.A.Y, _.B.Y, _.C.Y})
                .SelectMany(_ => _);
            var zs = _triangles.Select(_ => new[] {_.A.X, _.B.X, _.C.X})
                .SelectMany(_ => _);
            var averageX = xs.Average();
            var averageY = ys.Average();
            var averageZ = zs.Average();
            var maxX = xs.Select(Math.Abs).Max();
            var maxY = ys.Select(Math.Abs).Max();
            var maxZ = zs.Select(Math.Abs).Max();
            var maxValue = new[] {maxX, maxY, maxZ}.Max();
            var scale = 10 / maxValue;
            Transform(new TransposedTransformationMatrixBuilder()
                .MoveX(-averageX)
                .MoveY(-averageY)
                .MoveZ(-averageZ)
                .ScaleX(scale)
                .ScaleY(scale)
                .ScaleZ(scale)
                .Build());
        }
    }
}