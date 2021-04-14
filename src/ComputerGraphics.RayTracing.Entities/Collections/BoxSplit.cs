using System.Numerics;

namespace ComputerGraphics.RayTracing.Entities.Collections
{
    internal struct BoxSplit
    {
        public Vector3 LBoxMax { get; set; }
        public Vector3 RBoxMin { get; set; }
        public float SplitValue { get; set; }
    }
}