using System;
using System.Numerics;

namespace ComputerGraphics.RayTracing.Core.Entities.Pdfs
{
    public class GgxPdf : Pdf
    {
        private readonly OrthonormalBasis _onb;
        private readonly Vector3 _in;
        private readonly float _roughness;

        public GgxPdf(Vector3 w, Vector3 inDirection, float roughness)
        {
            _onb = OrthonormalBasis.FromW(w);
            _in = inDirection;
            _roughness = roughness;
        }

        public override float Value(Vector3 direction)
        {
            var h = Vector3.Normalize(direction - _in);
            return Ggx1(_onb.W, h, _roughness) * MathF.Abs(Vector3.Dot(h, _onb.W));
        }

        public override Vector3 Generate()
        {
            return _onb.Local(RandomGgx());
        }

        private Vector3 RandomGgx()
        {
            var random = new Random();
            var r1 = (float) random.NextDouble();
            var r2 = (float) random.NextDouble();
            var phi = 2 * MathF.PI * r1;
            var theta = MathF.Atan(_roughness * MathF.Sqrt(r2) / MathF.Sqrt(1 - r2));
            var x = MathF.Cos(phi) * MathF.Sin(theta);
            var y = MathF.Sin(phi) * MathF.Sin(theta);
            var z = MathF.Cos(theta);
            return new(x, y, z);
        }

        public static float Ggx1(Vector3 n, Vector3 h, float roughness)
        {
            var cosine = Vector3.Dot(n, h);
            var r2 = roughness * roughness;

            return r2 * (cosine > 0 ? 1 : 0) /
                   (MathF.PI * MathF.Pow(cosine, 4) * MathF.Pow(r2 + 1.0f / r2 - 1.0f, 2.0f));
        }
    }
}