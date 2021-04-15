using ComputerGraphics.RayTracing.Core.Entities;

namespace ComputerGraphics.RayTracing.Core.Interfaces
{
    public interface IHittable
    {
        /// <summary>
        /// Determines if ray hits a hittable object
        /// </summary>
        /// <param name="r"></param>
        /// <returns>Null if ray doesn't hit object. Otherwise, <see cref="HitResult"/></returns>
        HitResult? Hit(Ray r);
    }
}