using System;
using System.Numerics;

namespace ComputerGraphics.RayTracing.Core.Entities.Textures
{
    public class Noise : Texture
    {
        private readonly Perlin _noiseGenerator = new Perlin();

        public Noise(float scale) => Scale = scale;

        public float Scale { get; }

        public override Vector3 ValueAt(float u, float v, Vector3 p)
        {
            return Vector3.One * .5f *
                   (1 + MathF.Sin(Scale * p.Z) + 10 * _noiseGenerator.Turbulence(p, 7));
        }
    }
}