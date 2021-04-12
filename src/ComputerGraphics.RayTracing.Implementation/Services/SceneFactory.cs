using System.Collections.Generic;
using ComputerGraphics.RayTracing.Core.Entities;
using ComputerGraphics.RayTracing.Core.Interfaces;

namespace ComputerGraphics.RayTracing.Implementation.Services
{
    internal class SceneFactory : ISceneFactory
    {
        private readonly ICamera _camera;
        private readonly ILightProvider _lightProvider;

        public SceneFactory(ICamera camera, ILightProvider lightProvider)
        {
            _camera = camera;
            _lightProvider = lightProvider;
        }

        public Scene CreateDefaultScene(IEnumerable<IHittable> hittables) =>
            new(_camera, hittables, _lightProvider);
    }
}