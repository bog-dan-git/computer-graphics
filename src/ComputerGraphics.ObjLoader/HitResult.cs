using System.Numerics;

namespace ComputerGraphics.ObjLoader
{
    public struct HitResult
    {
        public float T { get; set; }
        public Vector3 P { get; set; }
        public Vector3 Normal { get; set; }
    }
}