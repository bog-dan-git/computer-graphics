using System;
using System.Numerics;

namespace ComputerGraphics.ObjLoader.Models
{
    public class Triangle
    {
        public Vector3 A { get; set; }
        public Vector3 B { get; set; }
        public Vector3 C { get; set; }

        private Vector3 _boxMax;
        public Vector3 BoxMax
        {
            get
            {
                if (_boxMax != Vector3.Zero) return _boxMax;
                
                var maxX = Math.Max(A.X, Math.Max(B.X, C.X));
                var maxY = Math.Max(A.Y, Math.Max(B.Y, C.Y));
                var maxZ = Math.Max(A.Z, Math.Max(B.Z, C.Z));
            
                _boxMax = new Vector3(maxX, maxY, maxZ);
                return _boxMax;
            }
        }
        
        private Vector3 _boxMin;
        public Vector3 BoxMin
        {
            get
            {
                if (_boxMin != Vector3.Zero) return _boxMin;
                
                var minX = Math.Min(A.X, Math.Min(B.X, C.X));
                var minY = Math.Min(A.Y, Math.Min(B.Y, C.Y));
                var minZ = Math.Min(A.Z, Math.Min(B.Z, C.Z));

                _boxMin = new Vector3(minX, minY, minZ);
                return _boxMin;
            }
        }
    }
}