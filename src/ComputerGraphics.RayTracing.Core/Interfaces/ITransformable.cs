using System.Numerics;

namespace ComputerGraphics.RayTracing.Core.Interfaces
{
    public interface ITransformable
    {
        public void Transform(Matrix4x4 matrix4X4);
    }
}