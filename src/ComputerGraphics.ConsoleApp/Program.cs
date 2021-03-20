using System.Threading.Tasks;

namespace ComputerGraphics.ConsoleApp
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var gifReader = new Gif.GifReader();
            await gifReader.ReadAsync("ph.gif");
        }
    }
}