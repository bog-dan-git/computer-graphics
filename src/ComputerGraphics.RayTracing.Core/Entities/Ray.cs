using System.Numerics;

namespace ComputerGraphics.RayTracing.Core.Entities
{
    public struct Ray
    {
        public Vector3 Origin { get; }
        public Vector3 Direction { get; }

        public Ray(Vector3 origin, Vector3 direction) => (Origin, Direction) = (origin, direction);


        public Vector3 PointAt(float t) => Origin + Direction * t;
    }
}