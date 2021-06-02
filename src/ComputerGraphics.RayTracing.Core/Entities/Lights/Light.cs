using System.Numerics;

namespace ComputerGraphics.RayTracing.Core.Entities.Lights
{
    public class Light
    {
        public Transform Transform { get; set; }
        public Vector3 Color { get; set; }
    }
}