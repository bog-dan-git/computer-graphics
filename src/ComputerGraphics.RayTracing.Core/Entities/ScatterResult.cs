using System.Numerics;
using ComputerGraphics.RayTracing.Core.Entities.Pdfs;

namespace ComputerGraphics.RayTracing.Core.Entities
{
    public struct ScatterResult
    {
        public Ray SpecularRay { get; set; }
        public bool IsSpecular { get; set; }
        public Vector3 Attenuation { get; set; }
        
        public Pdf Pdf { get; set; }
    }
}