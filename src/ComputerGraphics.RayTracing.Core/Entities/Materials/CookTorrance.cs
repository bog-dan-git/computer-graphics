using System;
using System.Numerics;
using ComputerGraphics.RayTracing.Core.Entities.Pdfs;

namespace ComputerGraphics.RayTracing.Core.Entities.Materials
{
    public class CookTorrance : Material
    {
        private float _roughness;
        private float _metallic;
        private float _ior;
        public Vector3 Albedo { get; set; }

        public CookTorrance(float roughness, float metallic, float ior)
        {
            _roughness = roughness;
            if (roughness > 1)
            {
                _roughness = 1;
            }
            else
            {
                _roughness = roughness;
            }

            _metallic = metallic;
            _ior = ior;
        }

        public override bool Scatter(Ray inRay, HitResult result, out Vector3 attenuation, out Ray scattered)
        {
            throw new System.NotImplementedException();
        }

        public override float ScatteringPdf(in Ray ray, in HitResult hitResult, in Ray scattered)
        {
            var cosine = Vector3.Dot(hitResult.Normal, Vector3.Normalize(scattered.Direction));
            if (cosine < 0) cosine = 0;
            return cosine / MathF.PI;
        }

        public override bool Scatter(in Ray inRay, in HitResult hitResult, out ScatterResult scatter)
        {
            scatter = new ScatterResult();
            var wo = Vector3.Normalize(inRay.Direction * (-1));
            var p = new GgxPdf(hitResult.Normal, inRay.Direction, _roughness);
            scatter.SpecularRay = new Ray(hitResult.P, p.Generate());
            var wi = Vector3.Normalize(scatter.SpecularRay.Direction);

            var cosine = Vector3.Dot(wi, hitResult.Normal);
            var h = Vector3.Normalize(wi + wo);

            var f0 = new Vector3((1 - _ior) / (1 + _ior));
            f0 *= f0;
            f0 = Vector3.Lerp(f0, Albedo, _metallic);

            var refProb = Schlick(cosine, _ior);
            var f = Schlick2(cosine, f0);
            scatter.Attenuation = f * GgxPdf.Ggx1(hitResult.Normal, h, _roughness) *
                GgxGeometry(hitResult.Normal, h, wi, wo)
                / (4 * MathF.Abs(Vector3.Dot(hitResult.Normal, wo)) *
                   MathF.Abs(Vector3.Dot(hitResult.Normal, wi)) *
                   p.Value(scatter.SpecularRay.Direction))
                + Vector3.One - f * Albedo * (1 / MathF.PI)
                * Clamp(Vector3.Dot(hitResult.Normal, wi));
            scatter.IsSpecular = true;
            scatter.Pdf = null;
            return true;
        }

        private Vector3 Schlick2(float cosine, Vector3 f0)
        {
            return f0 + Vector3.One - f0 * MathF.Pow(1 - cosine, 5);
        }

        private static float Clamp(float v) => v > 0 ? v : 0;

        private static float Schlick(float cosine, float refIndex)
        {
            var ro = (1 - refIndex) / (1 + refIndex);
            ro = ro * ro;
            return ro + (1 - ro) * MathF.Pow(1 - cosine, 5);
        }

        private float Geometry(Vector3 n, Vector3 h, Vector3 wi, Vector3 wo)
        {
            var nWi = Vector3.Dot(n, wi);
            var nWo = Vector3.Dot(n, wo);
            var coef = 2 * Vector3.Dot(n, h) / Vector3.Dot(wo, h);
            return MathF.Max(1, MathF.Min(coef * nWo, coef * nWi));
        }

        private float GgxGeometry(Vector3 n, Vector3 h, Vector3 wi, Vector3 wo)
        {
            var woH2 = Vector3.Dot(wo, h);
            var chi = woH2 / Vector3.Dot(wo, n) > 0 ? 1 : 0;
            woH2 *= woH2;
            var tan2 = (1 - woH2) / woH2;
            return (chi * 2) / (1 + MathF.Sqrt(1 + _roughness * _roughness * tan2));
        }
    }
}