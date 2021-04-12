using System.Numerics;

namespace ComputerGraphics.RayTracing.Core.Interfaces
{
    public interface ITransformationMatrixBuilder
    {
        ITransformationMatrixBuilder RotateX(float rad);
        ITransformationMatrixBuilder RotateY(float rad);
        ITransformationMatrixBuilder RotateZ(float rad);
        ITransformationMatrixBuilder MoveX(float offset);
        ITransformationMatrixBuilder MoveY(float offset);
        ITransformationMatrixBuilder MoveZ(float offset);
        ITransformationMatrixBuilder ScaleX(float coef);
        ITransformationMatrixBuilder ScaleY(float coef);
        ITransformationMatrixBuilder ScaleZ(float coef);

        Matrix4x4 Build();
    }
}