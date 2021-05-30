using System;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
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
            var normal = Vector3.Normalize(Vector3.Cross(e2, e1));
            var intersectionPoint = r.PointAt(f);
            
            if (!NormalA.Equals(Vector3.Zero) && !NormalB.Equals(Vector3.Zero) && !NormalC.Equals(Vector3.Zero))
            {
                normal = CalculateBarycentricNormal(intersectionPoint);
            }
            
            return new HitResult()
            {
                P = intersectionPoint,
                Normal = normal,
                T = f
            };
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