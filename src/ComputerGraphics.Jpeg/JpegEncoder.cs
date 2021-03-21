using System;
using System.Collections.Generic;
using ComputerGraphics.Converters.Sdk;
using ComputerGraphics.Converters.Sdk.Interfaces;
using ComputerGraphics.Converters.Sdk.Model;

namespace ComputerGraphics.Jpeg
{
    [ImageEncoder(new[]{"jpeg", "jpg"})]
    public class JpegEncoder : IImageEncoder
    {
        public byte[] Encode(RgbColor[,] colors)
        {
            var result = new List<byte>();
            var output = new Action<byte>((byte b) => { result.Add(b); });
            var writer = new JpegEncoderInternal(new FastDiscreteCosineTransformer());
            writer.WriteJpeg(output, colors);
            return result.ToArray();
        }
    }
}