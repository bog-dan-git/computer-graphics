using ComputerGraphics.RayTracing.Core.Entities;
using ComputerGraphics.RayTracing.Core.Entities.SceneObjects;

namespace ComputerGraphics.RayTracing.Entities.Entities
{
    public class Sphere : SceneObject
    {
        public float Radius { get; set; }
        public override HitResult? Hit(Ray r)
        {
            throw new System.NotImplementedException();
        }
    }
}