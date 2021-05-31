using System;
using System.Numerics;
using System.Threading.Tasks;
using ComputerGraphics.RayTracing.Core.Entities;
using ComputerGraphics.RayTracing.Core.Extensions;
using ComputerGraphics.RayTracing.Core.Interfaces;

namespace ComputerGraphics.RayTracing.Implementation.Services
{
    public class MultiThreadedRayTracer : IRayTracer
    {
        private readonly IScreenProvider _screen;
        private readonly IRayProvider _rayProvider;
        private const int SamplesPerPixel = 100;

        public MultiThreadedRayTracer(IScreenProvider screen,
            IRayProvider rayProvider)
        {
            _screen = screen;
            _rayProvider = rayProvider;
        }

        public Vector3[,] Trace(Scene scene)
        {
            int width = _screen.Width;
            int height = _screen.Height;
            var result = new Vector3[width, height];
            var random = new Random();
            Parallel.For(0, width, i =>
            {
                Parallel.For(0, height, j =>
                {
                    var resultColor = new Vector3();

                    for (int s = 0; s < SamplesPerPixel; s++)
                    {
                        var ray = _rayProvider.GetRay(i + (float)random.Next(), j + (float)random.NextDouble());
                        var color = RayColor(ray, new Vector3(.1f, .1f, .1f), scene, 50);
                        resultColor += color;
                    }

                    resultColor /= SamplesPerPixel;
                    result[i, j] = resultColor;
                });
            });
            return result;
        }

        private Vector3 RayColor(in Ray r, in Vector3 background, Scene scene, int depth)
        {
            if (depth <= 0)
            {
                return Vector3.UnitZ;
            }

            var hitResult = scene.SceneObjects.Hit(r);

            if (hitResult is null)
            {
                return background;
            } 

            // var mesh = scene.SceneObjects.First(_ => _.Id == 2);
            // var meshHit = mesh.Hit(r);
            // if (meshHit.HasValue && meshHit.Value.Equals(hitResult.Value))
            // {
                // Debug.Assert(true);
                // return (Vector3.Normalize(meshHit.Value.Normal));
            // }

            var emitted = hitResult.Value.Material.Emitted();
            var scattered = hitResult.Value.Material.Scatter(r, hitResult.Value, out var scatter);
            if (!scattered)
            {
                return emitted;
            }

            if (scatter.IsSpecular)
            {
                return scatter.Attenuation * RayColor(scatter.SpecularRay, background, scene, depth - 1);
            }
            
            var p = scatter.Pdf;


            var scatteredRay = new Ray(hitResult.Value.P, p.Generate());
            var pdfVal = p.Value(scatteredRay.Direction);

            return emitted +
                   scatter.Attenuation * hitResult.Value.Material.ScatteringPdf(r, hitResult.Value, scatteredRay)
                                       * RayColor(scatteredRay, background, scene, depth - 1) / pdfVal;
        }
    }
}