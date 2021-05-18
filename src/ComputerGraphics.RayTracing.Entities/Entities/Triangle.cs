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

        public Triangle(Vector3 a, Vector3 b, Vector3 c)
        {
            A = a;
            B = b;
            C = c;
        }

        public Vector3 A { get; private set; }
        public Vector3 B { get; private set; }
        public Vector3 C { get; private set; }

        public override HitResult? Hit(Ray r)
        {
            var e1 = B - A;
            var e2 = C - A;
            var pvec = Vector3.Cross(r.Direction, e2);
            var det = Vector3.Dot(e1, pvec);
            if (det < float.Epsilon && det > -float.Epsilon)
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