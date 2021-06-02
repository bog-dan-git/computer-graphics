using System.Numerics;

namespace ComputerGraphics.RayTracing.Core.Entities.Materials
{
    public class DiffuseLightMaterial : Material
    {
        public Vector3 Color { get; set; }

        public override Vector3 Emitted() => Color;
    }
}