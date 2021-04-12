using System.Collections.Generic;
using ComputerGraphics.RayTracing.Core.Entities;

namespace ComputerGraphics.RayTracing.Core.Interfaces
{
    public interface ISceneFactory
    {
        Scene CreateDefaultScene(IEnumerable<IHittable> hittables);
    }
}