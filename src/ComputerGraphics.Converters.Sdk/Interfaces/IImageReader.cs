using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ComputerGraphics.Converters.Sdk.Model;

namespace ComputerGraphics.Converters.Sdk.Interfaces
{
    public interface IImageReader
    {
        public Task<List<Color>> ReadAsync(string filename);
    }
}