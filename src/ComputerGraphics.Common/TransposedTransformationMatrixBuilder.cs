using System;
using System.Numerics;

namespace ComputerGraphics.Common
{
    public class TransposedTransformationMatrixBuilder
    {
        private Matrix4x4 _matrix4 = Matrix4x4.Identity;

        public TransposedTransformationMatrixBuilder RotateX(float rad)
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

        public TransposedTransformationMatrixBuilder RotateY(float rad)
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

        public TransposedTransformationMatrixBuilder RotateZ(float rad)
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

        public TransposedTransformationMatrixBuilder MoveX(float offset)
        {
            var matrix = Matrix4x4.Transpose(new Matrix4x4(1, 0, 0, offset,
                0, 1, 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1));
            _matrix4 *= matrix;
            return this;
        }

        public TransposedTransformationMatrixBuilder MoveY(float offset)
        {
            var matrix = Matrix4x4.Transpose(new Matrix4x4(1, 0, 0, 0,
                0, 1, 0, offset,
                0, 0, 1, 0,
                0, 0, 0, 1));
            _matrix4 *= matrix;
            return this;
        }

        public TransposedTransformationMatrixBuilder MoveZ(float offset)
        {
            var matrix = Matrix4x4.Transpose(new Matrix4x4(1, 0, 0, 0,
                0, 1, 0, 0,
                0, 0, 1, offset,
                0, 0, 0, 1));
            _matrix4 *= matrix;
            return this;
        }

        public TransposedTransformationMatrixBuilder ScaleX(float coef)
        {
            var matrix = Matrix4x4.Transpose(new Matrix4x4(coef, 0, 0, 0,
                0, 1, 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1));
            _matrix4 *= matrix;
            return this;
        }

        public TransposedTransformationMatrixBuilder ScaleY(float coef)
        {
            var matrix = Matrix4x4.Transpose(new Matrix4x4(1, 0, 0, 0,
                0, coef, 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1));
            _matrix4 *= matrix;
            return this;
        }

        public TransposedTransformationMatrixBuilder ScaleZ(float coef)
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