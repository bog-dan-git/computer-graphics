using System.Numerics;
using ComputerGraphics.RayTracing.Core.Entities;
using ComputerGraphics.RayTracing.Core.Entities.SceneObjects;

namespace ComputerGraphics.RayTracing.Entities.Entities
{
    public class Cube : SceneObject
    {
        public Vector3 Size { get; set; }
        public override HitResult? Hit(Ray r)
        {
            throw new System.NotImplementedException();
        }
    }
}