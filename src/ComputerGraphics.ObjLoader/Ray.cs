using System.Numerics;

namespace ComputerGraphics.ObjLoader
{
    public struct Ray
    {
        public Vector3 Origin { get; }
        public Vector3 Direction { get; }

        public Ray(Vector3 origin, Vector3 direction)
        {
            Origin = origin;
            Direction = direction;
        }

        public Vector3 PointAt(float t) => Origin * t + Direction;
    }
}