using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ComputerGraphics.Converters.Sdk;
using ComputerGraphics.Converters.Sdk.Interfaces;
using ComputerGraphics.Converters.Sdk.Model;

namespace ComputerGraphics.GifReader
{
    [ImageReader("gif")]
    public class GifReader : IImageReader
    {
        public async Task<List<RgbColor>> ReadAsync(string filename)
        {
            var bytes = await File.ReadAllBytesAsync(filename);
            // TODO: implementation
            return new List<RgbColor>();
        }
    }
}