using System.Threading.Tasks;
using ComputerGraphics.Converters.Sdk.Model;

namespace ComputerGraphics.Converters.Sdk.Interfaces
{
    public interface IImageReader
    {
        public Task<RgbColor[,]> ReadAsync(string filename);
    }
}