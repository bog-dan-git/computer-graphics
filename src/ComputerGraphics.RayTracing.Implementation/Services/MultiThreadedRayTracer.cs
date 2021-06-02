using System;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using ComputerGraphics.RayTracing.Core.Entities;
using ComputerGraphics.RayTracing.Core.Extensions;
using ComputerGraphics.RayTracing.Core.Interfaces;

namespace ComputerGraphics.RayTracing.Implementation.Services
{
    internal class MultiThreadedRayTracer : IRayTracer
    {
        private const int SamplesPerPixel = 100;
        private const int Depth = 50;

        public Vector3[,] Trace(Scene scene)
        {
            // var camera = scene.Cameras.First(camera1 => camera1.Id == scene.RenderOptions.CameraId);
            var camera = scene.Cameras.First();
            var provider = new CameraRayProvider(camera, new DefaultScreenProvider());
            int width = scene.RenderOptions.Width;
            int height = scene.RenderOptions.Height;
            var result = new Vector3[width, height];
            var random = new Random();
            Parallel.For(0, width, i =>
            {
                Parallel.For(0, height, j =>
                {
                    var resultColor = new Vector3();

                    for (int s = 0; s < SamplesPerPixel; s++)
                    {
                        var ray = provider.GetRay(i + (float) random.Next(), j + (float) random.NextDouble());
                        var color = RayColor(ray, scene.BackgroundColor, scene, Depth);
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
                return Vector3.Zero;
            }

            var hitResult = scene.SceneObjects.Hit(r);

            if (hitResult is null)
            {
                return background;
            }

            var emitted = hitResult.Value.Material.Emitted();
            var scattered = hitResult.Value.Material.Scatter(r, hitResult.Value, out var scatter);
            if (!scattered)
            {
                // we need to normalize color for light sources (as is does not fit in [0,0,0] - [1,1,1] range
                if (depth == Depth)
                {
                    return Vector3.Normalize(emitted);
                }

                return emitted;
            }

            if (scatter.IsSpecular)
            {
                return scatter.Attenuation * RayColor(scatter.SpecularRay, background, scene, depth - 1);
            }

            // TODO: use mixed pdf here
            var p = scatter.Pdf;


            var scatteredRay = new Ray(hitResult.Value.P, p.Generate());
            var pdfVal = p.Value(scatteredRay.Direction);

            return emitted +
                   scatter.Attenuation * hitResult.Value.Material.ScatteringPdf(r, hitResult.Value, scatteredRay)
                                       * RayColor(scatteredRay, background, scene, depth - 1) / pdfVal;
        }
    }
}