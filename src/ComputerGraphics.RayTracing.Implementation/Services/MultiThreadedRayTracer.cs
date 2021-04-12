using System.Numerics;
using System.Threading.Tasks;
using ComputerGraphics.RayTracing.Core.Entities;
using ComputerGraphics.RayTracing.Core.Interfaces;
using ComputerGraphics.RayTracing.Implementation.Builders;

namespace ComputerGraphics.RayTracing.Implementation.Services
{
    public class MultiThreadedRayTracer : IRayTracer
    {
        private readonly IScreenProvider _screen;
        private readonly ILightingStrategy _lightingStrategy;

        public MultiThreadedRayTracer(IScreenProvider screen, ILightingStrategy lightingStrategy)
        {
            _screen = screen;
            _lightingStrategy = lightingStrategy;
        }

        public Vector3[,] Trace(Scene scene)
        {
            scene.Camera.Transform(new TransposedTransformationMatrixBuilder().MoveZ(-20).Build());
            int width = _screen.Width;
            int height = _screen.Height;

            var result = new Vector3[width, height];

            Parallel.ForEach(scene.Hittables, hittable =>
            {
                Parallel.For(0, width, i =>
                {
                    Parallel.For(0, height, j =>
                    {
                        var ray = scene.Camera.LookAt(i, j);
                        var hitResult = hittable.Hit(ray, 0, 10000);

                        if (hitResult.HasValue)
                        {
                            result[i, j] = _lightingStrategy.Illuminate(scene.LightProvider, hitResult.Value);
                        }
                    });
                });
            });
            return result;
        }
    }
}