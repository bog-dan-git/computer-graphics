using System.Numerics;
using System.Threading.Tasks;
using ComputerGraphics.RayTracing.Core.Entities;
using ComputerGraphics.RayTracing.Core.Interfaces;

namespace ComputerGraphics.RayTracing.Implementation.Services
{
    public class MultiThreadedRayTracer : IRayTracer
    {
        private readonly IScreenProvider _screen;
        private readonly ILightingStrategy _lightingStrategy;
        private readonly IRayProvider _rayProvider;
        private readonly ILightProvider _lightProvider;

        public MultiThreadedRayTracer(IScreenProvider screen, ILightingStrategy lightingStrategy,
            IRayProvider rayProvider, ILightProvider lightProvider)
        {
            _screen = screen;
            _lightingStrategy = lightingStrategy;
            _rayProvider = rayProvider;
            _lightProvider = lightProvider;
        }

        public Vector3[,] Trace(Scene scene)
        {
            int width = _screen.Width;
            int height = _screen.Height;
            var result = new Vector3[width, height];

            var bgcolor = new Vector3(216 / 255f, 243 / 255f, 255 / 255f);
            Parallel.For(0, width, i =>
            {
                Parallel.For(0, height, j =>
                {
                    var ray = _rayProvider.GetRay(i, j);
                    foreach (var hittable in scene.SceneObjects)
                    {
                        var hitResult = hittable.Hit(ray);

                        if (hitResult.HasValue)
                        {
                            result[i, j] = _lightingStrategy.Illuminate(_lightProvider, hitResult.Value);
                        }
                        else if (result[i, j] == Vector3.Zero)
                        {
                            result[i, j] = bgcolor;
                        }
                    }
                });
            });
            return result;
        }
    }
}