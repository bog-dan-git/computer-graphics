using System.Numerics;

namespace ComputerGraphics.RayTracing.Core.Entities.Textures
{
    public class SolidColor : Texture
    {
        public Vector3 Color { get; }

        public SolidColor(Vector3 color) => Color = color;

        public override Vector3 ValueAt(float u, float v, Vector3 p) => Color;
    }
}