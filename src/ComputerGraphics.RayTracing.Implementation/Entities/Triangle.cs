using System.Numerics;
using ComputerGraphics.RayTracing.Core.Entities;
using ComputerGraphics.RayTracing.Core.Interfaces;

namespace ComputerGraphics.RayTracing.Implementation.Entities
{
    public class Triangle : IHittable, ITransformable
    {
        public Triangle(Vector3 a, Vector3 b, Vector3 c)
        {
            A = a;
            B = b;
            C = c;
        }

        public Vector3 A { get; private set; }
        public Vector3 B { get; private set; }
        public Vector3 C { get; private set; }

        public HitResult? Hit(Ray r, float min, float max)
        {
            var e1 = B - A;
            var e2 = C - A;
            var pvec = Vector3.Cross(r.Direction, e2);
            var det = Vector3.Dot(e1, pvec);
            if (det < 1e-8 && det > -1e-8)
            {
                return null;
            }

            var invDet = 1 / det;
            var tvec = r.Origin - A;
            var u = Vector3.Dot(tvec, pvec) * invDet;
            if (u < 0 || u > 1)
            {
                return null;
            }

            var qvec = Vector3.Cross(tvec, e1);
            var v = Vector3.Dot(r.Direction, qvec) * invDet;
            if (v < 0 || u + v > 1)
            {
                return null;
            }

            var f = Vector3.Dot(e2, qvec) * invDet;
            return new HitResult()
            {
                P = r.PointAt(f),
                Normal = Vector3.Cross(e2, e1),
                T = f
            };
        }

        public void Transform(Matrix4x4 matrix4X4)
        {
            A = Vector3.Transform(A, matrix4X4);
            B = Vector3.Transform(B, matrix4X4);
            C = Vector3.Transform(C, matrix4X4);
        }
    }
}