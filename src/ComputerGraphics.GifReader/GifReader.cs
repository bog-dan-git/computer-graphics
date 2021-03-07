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
        public async Task<List<Color>> ReadAsync(string filename)
        {
            byte[] bytes = await File.ReadAllBytesAsync(filename);            
            return new();
        }
    }
}