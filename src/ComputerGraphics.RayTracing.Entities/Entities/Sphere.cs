using System;
using System.Numerics;
using ComputerGraphics.Common;
using ComputerGraphics.RayTracing.Core.Entities;
using ComputerGraphics.RayTracing.Core.Entities.Pdfs;
using ComputerGraphics.RayTracing.Core.Entities.SceneObjects;

namespace ComputerGraphics.RayTracing.Entities.Entities
{
    public class Sphere : SceneObject
    {
        // To not to spend whole night debugging
        public float Radius { get; set; } = 1;

        public override HitResult? Hit(Ray r)
        {
            Transform ??= new Transform();
            var oc = r.Origin - Transform.Position;
            var a = r.Direction.LengthSquared();
            var halfB = Vector3.Dot(oc, r.Direction);
            var c = oc.LengthSquared() - Radius * Radius;
            var discriminant = halfB * halfB - a * c;
            if (discriminant < 0) return null;
            var sqrtD = MathF.Sqrt(discriminant);
            var root = (-halfB - sqrtD) / a;
            if (root < 0.00001 || 999999 < root)
            {
                root = (-halfB + sqrtD) / a;
                if (root < 0.00001 || 999999 < root)
                {
                    return null;
                }
            }

            var res = new HitResult()
            {
                T = root,
                P = r.PointAt(root),
                Material = Material
            };
            var outwardNormal = (res.P - Transform.Position)/ Radius;
            var frontFace = Vector3.Dot(r.Direction, outwardNormal) < 0;
            res.Normal = frontFace ? outwardNormal : -outwardNormal;
            // res.Normal = outwardNormal;
            res.TextureCoordinates = GetSphereTextureCoordinates(outwardNormal);
            return res;

            // var k = r.Origin - Transform.Position;
            // var b = Vector3.Dot(k, r.Direction);
            // var c = Vector3.Dot(k, k) - Radius * Radius;
            // var d = b * b - c;
            //
            // if (d >= 0)
            // {
            //     var sqrt = MathF.Sqrt(d);
            //     var t1 = -b + sqrt;
            //     var t2 = -b - sqrt;
            //
            //     var minT = MathF.Min(t1, t2);
            //     var maxT = MathF.Max(t1, t2);
            //
            //     var t = (minT >= 0) ? minT : maxT;
            //
            //     if (!(t > 0)) return null;
            //     
            //     var intersectionPoint = r.Direction * t + r.Origin;
            //     var normal = intersectionPoint - Transform.Position;
            //         
            //     var hitResult = new HitResult();
            //     hitResult.T = t;
            //     hitResult.P = intersectionPoint;
            //     hitResult.Normal = normal / Radius;
            //     hitResult.Material = Material;
            //     return hitResult;
            // }
            //
            // return null;
        }

        public override Vector3 Random(in Vector3 o)
        {
            var direction = Transform.Position - o;
            var distance = direction.LengthSquared();
            var onb = OrthonormalBasis.FromW(direction);
            return onb.Local(Vector3Extensions.RandomToSphere(Radius, distance));
        }

        public override float PdfValue(in Vector3 o, in Vector3 v)
        {
            var hitResult = Hit(new Ray(o, v));
            if (hitResult is null)
            {
                return 0;
            }

            var cosThetaMax = MathF.Sqrt(1 - (Radius * Radius) / (Transform.Position - o).LengthSquared());
            var solidAnge = 2 * MathF.PI * (1 - cosThetaMax);
            return 1 / solidAnge;
        }


        private static Vector2 GetSphereTextureCoordinates(in Vector3 point)
        {
            var theta = MathF.Acos(-point.Y);
            var phi = MathF.Atan2(-point.Z, point.X) + MathF.PI;
            var result = new Vector2 {X = phi / (2 * MathF.PI), Y = theta / MathF.PI};
            return result;
        }
    }
}