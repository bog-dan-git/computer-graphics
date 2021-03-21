using System.Threading.Tasks;
using ComputerGraphics.Converters.Sdk.Model;

namespace ComputerGraphics.Converters.Sdk.Interfaces
{
    public interface IImageWriter
    {
        Task WriteAsync(string path, RgbColor[,] colors);
    }
}