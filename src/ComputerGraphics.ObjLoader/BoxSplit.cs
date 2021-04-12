using System.Numerics;

namespace ComputerGraphics.ObjLoader
{
    public struct BoxSplit
    {
        public Vector3 LBoxMax { get; set; }
        public Vector3 RBoxMin { get; set; }
        public float SplitValue { get; set; }
    }
}