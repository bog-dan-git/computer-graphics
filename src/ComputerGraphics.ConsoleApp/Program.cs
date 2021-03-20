using System;
using System.Collections;
using System.Threading.Tasks;

namespace ComputerGraphics.ConsoleApp
{
    public static class Program
    {
        public static async Task Main(string[] args)
        { 
            var gifReader = new GifReader.GifReader();
            await gifReader.ReadAsync("verylarge.gif");
        }
    }
}