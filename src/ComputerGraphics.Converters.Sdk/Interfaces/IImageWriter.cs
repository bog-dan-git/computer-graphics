using System.Collections.Generic;
using System.Threading.Tasks;
using ComputerGraphics.Converters.Sdk.Model;

namespace ComputerGraphics.Converters.Sdk.Interfaces
{
    public interface IImageWriter
    {
        Task WriteAsync(string path, List<RgbColor> colors);
    }
}