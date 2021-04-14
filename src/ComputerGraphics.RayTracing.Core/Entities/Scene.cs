using System.Collections.Generic;
using System.Linq;
using ComputerGraphics.RayTracing.Core.Interfaces;

namespace ComputerGraphics.RayTracing.Core.Entities
{
    public class Scene
    {
        private readonly List<IHittable> _hittables;

        public IEnumerable<IHittable> Hittables => _hittables;

        public Scene(IEnumerable<IHittable> hittables) => _hittables = hittables.ToList();

        public void AddElement(IHittable hittable) => _hittables.Add(hittable);
    }
}