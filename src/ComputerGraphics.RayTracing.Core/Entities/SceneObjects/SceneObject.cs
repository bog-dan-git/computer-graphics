using System.Numerics;
using ComputerGraphics.RayTracing.Core.Entities.Materials;
using ComputerGraphics.RayTracing.Core.Interfaces;

namespace ComputerGraphics.RayTracing.Core.Entities.SceneObjects
{
    public abstract class SceneObject : IHittable
    {
        public int Id { get; set; }
        public Transform Transform { get; set; }
        public Material Material { get; set; }
        public abstract HitResult? Hit(Ray r);

        public virtual float PdfValue(in Vector3 o, in Vector3 v) => 0f;
        
        public virtual Vector3 Random(in Vector3 o) => Vector3.UnitX;
    }
}