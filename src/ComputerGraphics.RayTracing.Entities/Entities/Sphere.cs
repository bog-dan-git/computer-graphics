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
            res.TextureCoordinates = GetSphereTextureCoordinates(res.Normal);
            return res;
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


        private Vector2 GetSphereTextureCoordinates(in Vector3 point)
        {
            var theta = MathF.Acos(-point.Y);
            var phi = MathF.Atan2(-point.Z, point.X) + MathF.PI;
            var result = new Vector2 {X = phi / (2 * MathF.PI), Y = theta / MathF.PI};
            return result;
        }
    }
}