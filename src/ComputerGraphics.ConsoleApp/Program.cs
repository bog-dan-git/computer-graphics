using System.Threading.Tasks;
using ComputerGraphics.Gif;

namespace ComputerGraphics.ConsoleApp
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var gifReader = new GifReader();
            await gifReader.ReadAsync("filename.gif");
        }
    }
}