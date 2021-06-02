﻿using System;
using System.Numerics;

namespace ComputerGraphics.RayTracing.Core.Entities.Textures
{
    public class Checker : Texture
    {
        public Texture Even { get; }
        public Texture Odd { get; }

        public Checker(Texture even, Texture odd)
        {
            Even = even;
            Odd = odd;
        }
        public override Vector3 ValueAt(float u, float v, Vector3 p)
        {
            var sines = MathF.Sin(40 * u) * MathF.Sin(40 * v);
            if (sines < 0)
            {
                return Odd.ValueAt(u, v, p);
            }

            return Even.ValueAt(u, v, p);
        }
    }
}