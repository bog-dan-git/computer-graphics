using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ComputerGraphics.Converters.Sdk;
using ComputerGraphics.Converters.Sdk.Interfaces;
using ComputerGraphics.Converters.Sdk.Model;

namespace ComputerGraphics.JpegWriter
{
    [ImageWriter("jpg")]
    public class JpegWriter : IImageWriter
    {
        public async Task WriteAsync(List<Color> colors, string outputFile)
        {
            var result = new List<byte>();
            await File.WriteAllBytesAsync(outputFile, result.ToArray());
        }
    }
}