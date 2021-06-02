using System.Numerics;
using ComputerGraphics.RayTracing.Core.Entities.Materials;

namespace ComputerGraphics.RayTracing.Core.Entities
{
    public struct  HitResult
    {
        /// <summary>
        /// Represents distance from ray to origin
        /// </summary>
        public float T { get; set; }
        /// <summary>
        /// Represents intersection point
        /// </summary>
        public Vector3 P { get; set; }
        /// <summary>
        /// Represents normal to surface
        /// </summary>
        public Vector3 Normal { get; set; }
        
        public Material Material { get; set; }
        
        public Vector2 TextureCoordinates { get; set; }
    }
}