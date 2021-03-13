using System;

namespace ComputerGraphics.ConsoleApp
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var gifReader = new GifReader.GifReader();
            gifReader.ReadAsync("screen2.gif");
        }
    }
}