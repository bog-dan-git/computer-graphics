using System;

namespace ComputerGraphics.GifReader
{
    public class InvalidFormatException : Exception
    {
        public InvalidFormatException()
        {
        }

        public InvalidFormatException(string message)
            : base(message)
        {
        }

        public InvalidFormatException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}