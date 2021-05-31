using System;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.Intrinsics.X86;

namespace ComputerGraphics.Common
{
    public static class Vector3Extensions
    {
        private const float Diff = (float) 1.0e-8;

        public static bool NearZero(this Vector3 vector3) => MathF.Abs(vector3.X) < Diff
                                                             && MathF.Abs(vector3.Y) < Diff
                                                             && MathF.Abs(vector3.Z) < Diff;

        public static Vector3 RandomCosineDirection()
        {
            var rnd = new Random();
            var r1 = (float) rnd.NextDouble();
            var r2 = (float) rnd.NextDouble();
            var z = MathF.Sqrt(1 - r2);

            var phi = 2 * MathF.PI * r1;
            var x = MathF.Cos(phi) * MathF.Sqrt(r2);
            var y = MathF.Sin(phi) * MathF.Sqrt(r2);
            return new Vector3(x, y, z);
        }

        public static Vector3 RandomInUnitSphere()
        {
            var random = new Random();

            while (true)
            {
                var x = (float) random.NextDouble() * 2 - 1;
                var y = (float) random.NextDouble() * 2 - 1;
                var z = (float) random.NextDouble() * 2 - 1;
                var vector = new Vector3(x, y, z);
                if (vector.LengthSquared() > 1) continue;
                return Vector3.Normalize(vector);
            }
        }

        public static Vector3 RandomToSphere(float radius, float distanceSquared)
        {
            var rnd = new Random();
            var r1 = (float) rnd.NextDouble();
            var r2 = (float) rnd.NextDouble();
            var z = 1 + r2 * (MathF.Sqrt(1 - radius * radius / distanceSquared) - 1);

            var phi = 2f * MathF.PI * r1;
            var x = MathF.Cos(phi) * MathF.Sqrt(1 - z * z);
            var y = MathF.Sign(phi) * MathF.Sqrt(1 - z * z);

            return new Vector3(x, y, z);
        }

        public static Vector3 RandomInRange(float min, float max)
        {
            Debug.Assert(max > min);
            var range = max - min;
            var random = new Random();
            var result = new Vector3
            {
                X = (float) random.NextDouble() * range + min,
                Y = (float) random.NextDouble() * range + min,
                Z = (float) random.NextDouble() * range + min
            };
            return result;
        }

        /// <summary>
        /// An implementation of Snell's law
        /// </summary>
        /// <param name="unitDirection"></param>
        /// <param name="hitResultNormal"></param>
        /// <param name="refractionRatio">eta 1 over eta prime</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static Vector3 Refract(Vector3 unitDirection, Vector3 hitResultNormal, float refractionRatio)
        {
            var cosTheta = MathF.Min(Vector3.Dot(-unitDirection, hitResultNormal), 1.0f);
            var perpendicular = refractionRatio * (unitDirection + cosTheta * hitResultNormal);
            var parallel = -MathF.Sqrt(MathF.Abs(1f - perpendicular.LengthSquared())) * hitResultNormal;
            return perpendicular + parallel;
        }
    }
}