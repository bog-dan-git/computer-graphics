using System.Numerics;

namespace ComputerGraphics.RayTracing.Core.Entities.Materials
{
    public class DiffuseLightMaterial : Material
    {
        public Vector3 Color { get; set; }
        public override bool Scatter(Ray inRay, HitResult result, out Vector3 attenuation, out Ray scattered)
        {
            attenuation = Vector3.Zero;
            scattered = new Ray(Vector3.Zero, Vector3.Zero);
            return false;
        }

        public override Vector3 Emitted() => Color;
    }
}