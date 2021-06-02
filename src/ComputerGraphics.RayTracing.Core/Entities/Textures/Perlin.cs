using System;
using System.Linq;
using System.Numerics;
using ComputerGraphics.Common;

namespace ComputerGraphics.RayTracing.Core.Entities.Textures
{
    // TODO: debug
    public class Perlin
    {
        private Vector3[] RanVec { get; set; }
        private const int AmountOfPoints = 256;
        private int[] _xPerms;
        private int[] _yPerms;
        private int[] _zPerms;

        public Perlin()
        {
            RanVec = new Vector3[AmountOfPoints];
            for (int i = 0; i < AmountOfPoints; i++)
            {
                RanVec[i] = Vector3.Normalize(Vector3Extensions.RandomInRange(-1, 1));
                _xPerms = GeneratePerm();
                _yPerms = GeneratePerm();
                _zPerms = GeneratePerm();
            }
        }

        public float Noise(in Vector3 p)
        {
            var u = p.X - MathF.Floor(p.X);
            var v = p.Y - MathF.Floor(p.Y);
            var w = p.Z - MathF.Floor(p.Z);
            var c = new Vector3[2, 2, 2];

            for (int a = 0; a < 2; a++)
            {
                for (int b = 0; b < 2; b++)
                {
                    for (int d = 0; d < 2; d++)
                    {
                        c[a, b, d] = RanVec[_xPerms[(a + (int) u) & 255] ^
                                            _yPerms[(b + (int) v) & 255] ^
                                            _zPerms[(d + (int) d & 255)]];
                    }
                }
            }

            return Interpolate(c, u, v, w);
        }

        public float Turbulence(Vector3 p, int depth)
        {
            var acc = 0f;
            var weight = 1f;
            for (int i = 0; i < depth; i++)
            {
                acc += weight * Noise(p);
                weight *= .5f;
                p *= 2;
            }

            return MathF.Abs(acc);
        }

        private float Interpolate(Vector3[,,] c, float u, float v, float w)
        {
            var uu = u * u * u;
            var vv = v * v * v;
            var ww = w * w * w;
            var accum = 0f;

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        var weight = new Vector3(u - i, v - j, w - k);
                        accum += (i * uu + (1 - i) * (1 - uu)) *
                                 (j * vv + (1 - j) * (1 - vv)) *
                                 (k * ww + (1 - k) * (1 - ww))
                                 * Vector3.Dot(c[i, j, k], weight);
                    }
                }
            }

            return accum;
        }

        private int[] GeneratePerm()
        {
            var p = Enumerable.Range(0, AmountOfPoints).ToArray();
            var random = new Random();
            for (int i = AmountOfPoints - 1; i > 0; i--)
            {
                int target = random.Next(0, i);
                (p[i], p[target]) = (p[target], p[i]);
            }

            return p;
        }
    }
}