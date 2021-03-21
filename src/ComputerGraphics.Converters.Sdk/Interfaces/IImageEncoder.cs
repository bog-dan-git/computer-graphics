using ComputerGraphics.Converters.Sdk.Model;

namespace ComputerGraphics.Converters.Sdk.Interfaces
{
    public interface IImageEncoder
    {
        byte[] Encode(RgbColor[,] colors);
    }
}