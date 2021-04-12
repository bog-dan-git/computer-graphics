using System.Collections.Generic;
using System.Numerics;

namespace ComputerGraphics.RayTracing.Core.Interfaces
{
    public interface IScene
    {
        Vector3[,] Render(IEnumerable<IHittable> hittables);
    }
}