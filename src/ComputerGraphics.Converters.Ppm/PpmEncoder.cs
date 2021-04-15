using System.Text;
using ComputerGraphics.Converters.Sdk;
using ComputerGraphics.Converters.Sdk.Interfaces;
using ComputerGraphics.Converters.Sdk.Model;

namespace ComputerGraphics.Converters.Ppm
{
    [ImageEncoder("ppm")]
    public class PpmEncoder : IImageEncoder
    {
        public byte[] Encode(RgbColor[,] colors)
        {
            var result = new StringBuilder("P3\n");
            int width = colors.GetLength(0);
            int height = colors.GetLength(1);
            result.Append(width).Append(' ')
                .Append(height)
                .Append('\n');
            result.Append("255\n");
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    var color = colors[j, i];
                    result
                        .Append(color.R).Append(' ')
                        .Append(color.G).Append(' ')
                        .Append(color.B)
                        .Append('\n');
                }
            }

            return Encoding.ASCII.GetBytes(result.ToString().TrimEnd('\n'));
        }
    }
}