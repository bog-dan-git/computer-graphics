using ComputerGraphics.RayTracing.Core.Entities;
using ComputerGraphics.RayTracing.Core.Entities.SceneObjects;

namespace ComputerGraphics.RayTracing.Entities.Entities
{
    /// <summary>
    /// <see cref="Mesh"/>
    /// </summary>
    public class MeshObject : SceneObject
    {
        public string Reference { get; set; }
        public override HitResult? Hit(Ray r)
        {
            throw new System.NotImplementedException();
        }
    }
}