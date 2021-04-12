using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using ComputerGraphics.RayTracing.Core.Entities;
using ComputerGraphics.RayTracing.Core.Interfaces;

namespace ComputerGraphics.RayTracing.Implementation.Entities
{
    internal class Scene : IScene
    {
        private readonly IScreenProvider _screenProvider;
        private readonly ICamera _camera;
        private readonly ITransformationMatrixBuilder _transformationMatrixBuilder;
        private readonly ILightProvider _lightProvider;

        public Scene(IScreenProvider screenProvider,
            ICamera camera,
            ITransformationMatrixBuilder transformationMatrixBuilder,
            ILightProvider lightProvider)
        {
            _screenProvider = screenProvider;
            _camera = camera;
            _transformationMatrixBuilder = transformationMatrixBuilder;
            _lightProvider = lightProvider;
        }

        public Vector3[,] Render(IEnumerable<IHittable> hittables)
        {
            _camera.Transform(_transformationMatrixBuilder.MoveZ(-20).Build());
            int width = _screenProvider.Width;
            int height = _screenProvider.Height;
            var result = new Vector3[width, height];

            Parallel.ForEach(hittables, hittable =>
            {
                Parallel.For(0, width, i =>
                {
                    Parallel.For(0, height, j =>
                    {
                        var ray = _camera.LookAt(i, j);
                        var hitResult = hittable.Hit(ray, 0, 10000);

                        if (hitResult.HasValue)
                        {
                            result[i, j] = GetColor(hitResult.Value);
                        }
                    });
                });
            });

            return result;
        }

        private Vector3 GetColor(HitResult hitResult)
        {
            var lightOrigin = _lightProvider.Origin;
            var lightDirection = Vector3.Normalize(lightOrigin - hitResult.P);
            var lightAbs = Vector3.Dot(hitResult.Normal, lightDirection);
            return new Vector3(lightAbs, lightAbs, lightAbs);
        }
    }
}