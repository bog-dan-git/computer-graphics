using System;
using System.Numerics;
using ComputerGraphics.RayTracing.Core.Entities;
using ComputerGraphics.RayTracing.Core.Entities.SceneObjects;
using ComputerGraphics.RayTracing.Core.Interfaces;

namespace ComputerGraphics.RayTracing.Entities.Entities
{
    public class Triangle : SceneObject, ITransformable
    {
        private Vector3 _boxMax;
        private Vector3 _boxMin;

        private const float Epsilon = 1e-8f;
        private bool _doubleSided = false;
        public Vector3 NormalA { get; set; }
        public Vector3 NormalB { get; set; }
        public Vector3 NormalC { get; set; }

        public Triangle(Vector3 a, Vector3 b, Vector3 c)
        {
            A = a;
            B = b;
            C = c;
        }

        public Triangle(Vector3 a, Vector3 b, Vector3 c, Vector3 normalA, Vector3 normalB, Vector3 normalC)
        {
            NormalA = normalA;
            NormalB = normalB;
            NormalC = normalC;
            A = a;
            B = b;
            C = c;
        }

        public Vector3 A { get; private set; }
        public Vector3 B { get; private set; }
        public Vector3 C { get; private set; }

        // public override HitResult? Hit(Ray r)
        // {
        //     var v0v1 = B - A;
        //     var v0v2 = C - A;
        //     var pvec = Vector3.Cross(r.Direction, v0v2);
        //     var det = Vector3.Dot(v0v1, pvec);
        //     if (det is < Epsilon and > -Epsilon)
        //     {
        //         return null;
        //     }
        //
        //     var invDet = 1f / det;
        //     var tvec = r.Origin - A;
        //     var u = Vector3.Dot(tvec, pvec) * invDet;
        //     if (u is < 0 or > 1)
        //     {
        //         return null;
        //     }
        //
        //     var qvec = Vector3.Cross(tvec, v0v1);
        //     var v = Vector3.Dot(r.Direction, qvec) * invDet;
        //     if (v < 0 || u + v > 1)
        //     {
        //         return null;
        //     }
        //
        //     var f = Vector3.Dot(v0v2, qvec) * invDet;
        //     var normal = Vector3.Normalize(Vector3.Cross(v0v2, v0v1));
        //     var intersectionPoint = r.PointAt(f);
        //     
        //     if (!NormalA.Equals(Vector3.Zero) && !NormalB.Equals(Vector3.Zero) && !NormalC.Equals(Vector3.Zero))
        //     {
        //         normal = CalculateBarycentricNormal(intersectionPoint);
        //     }
        //
        //     return new HitResult()
        //     {
        //         P = r.PointAt(f),
        //         Normal = normal,
        //         T = f,
        //         Material = Material
        //     };
        // }
        public override HitResult? Hit(Ray r)
        {
            var tMin = 0.00001;
            var tMax = 999999;
            var normal = Vector3.Normalize(Vector3.Cross(B - A, C - B));
            var nDotDir = Vector3.Dot(r.Direction, normal);
            if (nDotDir > 0 && !_doubleSided) return null;
            if (MathF.Abs(nDotDir) < 0.00001) return null;

            var d = -Vector3.Dot(normal, A);
            var t = -(Vector3.Dot(normal, r.Origin) + d) / nDotDir;

            if (t < tMin || t > tMax)
            {
                if (_doubleSided)
                {
                    t = -t;
                    if (t < tMin || t > tMax)
                    {
                        return null;
                    } 
                }

                return null;
            }

            var p = r.PointAt(t);
            var s1 = B - A;
            var vToP = p - A;
            if (Vector3.Dot(Vector3.Cross(s1, vToP), normal) < 0) return null;
            var s2 = C - B;
            vToP = p - B;
            if (Vector3.Dot(Vector3.Cross(s2, vToP), normal) < 0) return null;
            var s3 = A - C;
            vToP = p - C;
            if (Vector3.Dot(Vector3.Cross(s3, vToP), normal) < 0) return null;
            var hitResult = new HitResult();
            hitResult.T = t;
            hitResult.P = p;
            if (nDotDir > 0)
            {
                hitResult.Normal = -normal;
            }
            else
            {
                hitResult.Normal = normal;
            }

            hitResult.Material = Material;
            return hitResult;

        }

        private Vector3 CalculateBarycentricNormal(Vector3 point)
        {
            var v0 = B - A;
            var v1 = C - A;
            var v2 = point - A;
            var d00 = Vector3.Dot(v0, v0);
            var d01 = Vector3.Dot(v0, v1);
            var d11 = Vector3.Dot(v1, v1);
            var d20 = Vector3.Dot(v2, v0);
            var d21 = Vector3.Dot(v2, v1);
            var denom = d00 * d11 - d01 * d01;
            var v = (d11 * d20 - d01 * d21) / denom;
            var w = (d00 * d21 - d01 * d20) / denom;
            var u = 1.0f - v - w;
            var barNormal = u * NormalA + v * NormalB + w * NormalC;

            return barNormal;
        }

        public void Transform(Matrix4x4 matrix4X4)
        {
            A = Vector3.Transform(A, matrix4X4);
            B = Vector3.Transform(B, matrix4X4);
            C = Vector3.Transform(C, matrix4X4);
        }

        #region KDTree

        /// <summary>
        /// Internally used by KD-tree
        /// </summary>
        internal Vector3 BoxMax
        {
            get
            {
                if (_boxMax != Vector3.Zero) return _boxMax;

                var maxX = Math.Max(A.X, Math.Max(B.X, C.X));
                var maxY = Math.Max(A.Y, Math.Max(B.Y, C.Y));
                var maxZ = Math.Max(A.Z, Math.Max(B.Z, C.Z));

                _boxMax = new Vector3(maxX, maxY, maxZ);
                return _boxMax;
            }
        }

        /// <summary>
        /// Internally used by KD-tree
        /// </summary>
        internal Vector3 BoxMin
        {
            get
            {
                if (_boxMin != Vector3.Zero) return _boxMin;

                var minX = Math.Min(A.X, Math.Min(B.X, C.X));
                var minY = Math.Min(A.Y, Math.Min(B.Y, C.Y));
                var minZ = Math.Min(A.Z, Math.Min(B.Z, C.Z));

                _boxMin = new Vector3(minX, minY, minZ);
                return _boxMin;
            }
        }

        #endregion
    }
}