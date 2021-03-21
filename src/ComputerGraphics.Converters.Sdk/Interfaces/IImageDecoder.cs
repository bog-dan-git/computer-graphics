using ComputerGraphics.Converters.Sdk.Model;

namespace ComputerGraphics.Converters.Sdk.Interfaces
{
    public interface IImageDecoder
    {
        public RgbColor[,] Read(byte[] bytes);
    }
}