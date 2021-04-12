using System;
using System.Numerics;
using ComputerGraphics.RayTracing.Core.Interfaces;

namespace ComputerGraphics.RayTracing.Implementation.Builders
{
    internal class TransposedTransformationMatrixBuilder : ITransformationMatrixBuilder
    {
        private Matrix4x4 _matrix4 = Matrix4x4.Identity;

        public ITransformationMatrixBuilder RotateX(float rad)
        {
            var cos = MathF.Cos(rad);
            var sin = MathF.Sin(rad);
            var matrix = Matrix4x4.Transpose(new Matrix4x4(1, 0, 0, 0,
                0, cos, -sin, 0,
                0, sin, cos, 0,
                0, 0, 0, 1));
            _matrix4 *= matrix;
            return this;
        }

        public ITransformationMatrixBuilder RotateY(float rad)
        {
            var cos = MathF.Cos(rad);
            var sin = MathF.Sin(rad);
            var matrix = Matrix4x4.Transpose(new Matrix4x4(cos, 0, sin, 0,
                0, 1, 0, 0,
                -sin, 0, cos, 0,
                0, 0, 0, 1));
            _matrix4 *= matrix;
            return this;
        }

        public ITransformationMatrixBuilder RotateZ(float rad)
        {
            var cos = MathF.Cos(rad);
            var sin = MathF.Sin(rad);
            var matrix = Matrix4x4.Transpose(new Matrix4x4(cos, -sin, 0, 0,
                sin, cos, 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1));
            _matrix4 *= matrix;
            return this;
        }

        public ITransformationMatrixBuilder MoveX(float offset)
        {
            var matrix = Matrix4x4.Transpose(new Matrix4x4(1, 0, 0, offset,
                0, 1, 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1));
            _matrix4 *= matrix;
            return this;
        }

        public ITransformationMatrixBuilder MoveY(float offset)
        {
            var matrix = Matrix4x4.Transpose(new Matrix4x4(1, 0, 0, 0,
                0, 1, 0, offset,
                0, 0, 1, 0,
                0, 0, 0, 1));
            _matrix4 *= matrix;
            return this;
        }

        public ITransformationMatrixBuilder MoveZ(float offset)
        {
            var matrix = Matrix4x4.Transpose(new Matrix4x4(1, 0, 0, 0,
                0, 1, 0, 0,
                0, 0, 1, offset,
                0, 0, 0, 1));
            _matrix4 *= matrix;
            return this;
        }

        public ITransformationMatrixBuilder ScaleX(float coef)
        {
            var matrix = Matrix4x4.Transpose(new Matrix4x4(coef, 0, 0, 0,
                0, 1, 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1));
            _matrix4 *= matrix;
            return this;
        }

        public ITransformationMatrixBuilder ScaleY(float coef)
        {
            var matrix = Matrix4x4.Transpose(new Matrix4x4(1, 0, 0, 0,
                0, coef, 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1));
            _matrix4 *= matrix;
            return this;
        }

        public ITransformationMatrixBuilder ScaleZ(float coef)
        {
            var matrix = Matrix4x4.Transpose(new Matrix4x4(1, 0, 0, 0,
                0, 1, 0, 0,
                0, 0, coef, 0,
                0, 0, 0, 1));
            _matrix4 *= matrix;
            return this;
        }

        public Matrix4x4 Build() => _matrix4;
    }
}