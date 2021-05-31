using System;
using System.Numerics;

namespace ComputerGraphics.RayTracing.Core.Entities.Pdfs
{
    public class OrthonormalBasis
    {
        public Vector3[] Axis { get; set; } = new Vector3[3];

        public Vector3 U => Axis[0];
        public Vector3 V => Axis[1];
        public Vector3 W => Axis[2];

        public Vector3 Local(Vector3 vec) => vec.X * U + vec.Y * V + vec.Z * W;

        public static OrthonormalBasis FromW(Vector3 w)
        {
            var basis = new OrthonormalBasis {Axis = {[2] = Vector3.Normalize(w)}};
            var a = (MathF.Abs(basis.W.X) > .9f) ? Vector3.UnitY : Vector3.UnitX;
            basis.Axis[1] = Vector3.Normalize(Vector3.Cross(basis.W, a));
            basis.Axis[0] = Vector3.Cross(basis.W, basis.V);
            return basis;
        }
    }
}