using System.Numerics;
using ComputerGraphics.RayTracing.Core.Interfaces;

namespace ComputerGraphics.RayTracing.Core.Entities
{
    public class Plane : IHittable
    {
        public Vector3 A { get; }
        public Vector3 B { get; }
        public Vector3 C { get; }


        public Plane(Vector3 a, Vector3 b, Vector3 c) => (A, B, C) = (a, b, c);

        public HitResult? Hit(Ray r, float min, float max)
        {
            var normal = Vector3.Cross(A - B, B - C);
            var denom = Vector3.Dot(r.Direction, normal);
            if (denom > 1e-6)
            {
                var p0l0 = A - r.Origin;
                var t = Vector3.Dot(p0l0, normal) / denom;
                if (t >= 0)
                {
                    return new HitResult()
                    {
                        Normal = normal,
                        T = t,
                        P = r.PointAt(t)
                    };
                }
            }

            return null;
        }
    }
}