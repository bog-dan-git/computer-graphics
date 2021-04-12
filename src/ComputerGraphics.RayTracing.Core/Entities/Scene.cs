using System.Collections.Generic;
using System.Linq;
using ComputerGraphics.RayTracing.Core.Interfaces;

namespace ComputerGraphics.RayTracing.Core.Entities
{
    public class Scene
    {
        private readonly List<IHittable> _hittables;
        public ICamera Camera { get; }

        public IEnumerable<IHittable> Hittables => _hittables;
        public ILightProvider LightProvider { get; }

        public Scene(ICamera camera, IEnumerable<IHittable> hittables, ILightProvider lightProvider)
        {
            Camera = camera;
            LightProvider = lightProvider;
            _hittables = hittables.ToList();
        }

        public void AddElement(IHittable hittable) => _hittables.Add(hittable);
    }
}