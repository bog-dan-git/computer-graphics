using System;
using System.Numerics;
using ComputerGraphics.RayTracing.Core.Entities;
using ComputerGraphics.RayTracing.Core.Entities.SceneObjects;

namespace ComputerGraphics.RayTracing.Entities.Entities
{
    public class Sphere : SceneObject
    {
        public float Radius { get; set; }
        public override HitResult? Hit(Ray r)
        {
            Transform ??= new Transform();
            
            var k = r.Origin - Transform.Position;
            var b = Vector3.Dot(k, r.Direction);
            var c = Vector3.Dot(k, k) - Radius * Radius;
            var d = b * b - c;

            if (d >= 0)
            {
                var sqrt = MathF.Sqrt(d);
                var t1 = -b + sqrt;
                var t2 = -b - sqrt;

                var minT = MathF.Min(t1, t2);
                var maxT = MathF.Max(t1, t2);

                var t = (minT >= 0) ? minT : maxT;

                if (!(t > 0)) return null;
                
                var intersectionPoint = r.Direction * t + r.Origin;
                var normal = intersectionPoint - Transform.Position;
                    
                var hitResult = new HitResult();
                hitResult.T = t;
                hitResult.P = intersectionPoint;
                hitResult.Normal = normal;
                return hitResult;
            }

            return null;
        }
    }
}