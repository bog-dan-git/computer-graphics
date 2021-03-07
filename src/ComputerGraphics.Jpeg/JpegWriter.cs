using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ComputerGraphics.Converters.Sdk;
using ComputerGraphics.Converters.Sdk.Interfaces;
using ComputerGraphics.Converters.Sdk.Model;

namespace ComputerGraphics.Jpeg
{
    [ImageWriter("jpg")]
    public class JpegWriter : IImageWriter
    {
        public async Task WriteAsync(string path, List<RgbColor> colors)
        {
            var result = new List<byte>();
            
            // TODO: implementation
            
            await File.WriteAllBytesAsync(path, result.ToArray());
        }
    }
}