using System;

namespace ComputerGraphics.Jpeg
{
    internal interface IDiscreteCosineTransformer
    {
        /// <summary>
        /// Applies DC transform
        /// </summary>
        /// <param name="block">Represents flattened 8*8 block. Is changed after DCT (to increase performance and reduce memory allocation)</param>
        /// <param name="step">Meant to be either 1 or 8</param>
        void Transform(Span<float> block, int step);
    }

    internal class FastDiscreteCosineTransformer : IDiscreteCosineTransformer
    {
        private static readonly float SqrtHalfSqrt = 1.306562965f; // cos(pi/8) * sqrt(2)
        private static readonly float HalfSqrtSqrt = 0.382683432f; // cos(3pi/8)
        private static readonly float InvSqrt = 0.707106781f; //cos(2pi/8) == cos(pi/4) == 1/sqrt(2)
        private static readonly float InvSqrtSqrt = 0.541196100f; // cos(3pi/8) * sqrt(2)


        /// <summary>
        /// This implementation is highly inspired by https://dev.w3.org/Amaya/libjpeg/jfdctflt.c
        /// </summary>
        /// <param name="block"></param>
        /// <param name="step"></param>
        public void Transform(Span<float> block, int step)
        {
            var block0 = block[0 * step];
            var block1 = block[1 * step];
            var block2 = block[2 * step];
            var block3 = block[3 * step];
            var block4 = block[4 * step];
            var block5 = block[5 * step];
            var block6 = block[6 * step];
            var block7 = block[7 * step];

            var add07 = block0 + block7;
            var add16 = block1 + block6;
            var add25 = block2 + block5;
            var add34 = block3 + block4;

            var sub34 = block3 - block4;
            var sub07 = block0 - block7;
            var sub16 = block1 - block6;
            var sub25 = block2 - block5;

            var add0347 = add07 + add34;
            var add1256 = add16 + add25;

            var sub1625 = add16 - add25;
            var sub0734 = add07 - add34;

            block[0 * step] = add0347 + add1256;
            block[4 * step] = add0347 - add1256;

            var z1 = (sub1625 + sub0734) * InvSqrt;
            block[2 * step] = sub0734 + z1;
            block[6 * step] = sub0734 - z1;
            var sub2345 = sub25 + sub34;
            var sub1256 = sub16 + sub25;
            var sub0167 = sub16 + sub07;

            var z5 = (sub2345 - sub0167) * HalfSqrtSqrt;
            var z2 = sub2345 * InvSqrtSqrt + z5;
            var z3 = sub1256 * InvSqrt;
            var z4 = sub0167 * SqrtHalfSqrt + z5;
            var z6 = sub07 + z3;
            var z7 = sub07 - z3;
            block[1 * step] = z6 + z4;
            block[7 * step] = z6 - z4;
            block[5 * step] = z7 + z2;
            block[3 * step] = z7 - z2;
        }
    }
}