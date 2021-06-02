using System;
using System.Numerics;
using ComputerGraphics.Common;

namespace ComputerGraphics.RayTracing.Core.Entities.Materials
{
    public class DielectricMaterial : Material
    {
        public float RefractionIndex { get; set; }

        public override bool Scatter(in Ray inRay, in HitResult hitResult, out ScatterResult scatter)
        {
            scatter = new ScatterResult {IsSpecular = true, Attenuation = new Vector3(1, 1, 1)};
            var refractionRatio = hitResult.Normal.Z > 0 ? (1f / RefractionIndex) : RefractionIndex;

            var unitDirection = Vector3.Normalize(inRay.Direction);
            var cosTheta = MathF.Min(Vector3.Dot(-unitDirection, hitResult.Normal), 1f);
            var sinTheta = MathF.Sqrt(1f - cosTheta * cosTheta);

            bool cannotRefract = refractionRatio * sinTheta > 1f;
            Vector3 direction;
            var random = new Random();
            if (cannotRefract || Reflectance(cosTheta, refractionRatio) > (float) random.NextDouble())
            {
                direction = Vector3.Reflect(unitDirection, hitResult.Normal);
            }
            else
            {
                direction = Vector3Extensions.Refract(unitDirection, hitResult.Normal, refractionRatio);
            }

            scatter.SpecularRay = new Ray(hitResult.P, direction);
            return true;
        }

        /// <summary>
        /// Uses Schlick's approximation
        /// </summary>
        /// <param name="cosine"></param>
        /// <param name="refIndex"></param>
        /// <returns></returns>
        private static float Reflectance(float cosine, float refIndex)
        {
            var r0 = (1f - refIndex) / (1f + refIndex);
            r0 *= r0;
            return r0 + (1 - r0) * MathF.Pow((1 - cosine), 5);
        }
    }
}