using System;

namespace ComputerGraphics.GifReader
{
    public class InterlacedUnsupportedException : Exception
    {
        public InterlacedUnsupportedException()
        {
        }

        public InterlacedUnsupportedException(string message)
            : base(message)
        {
        }

        public InterlacedUnsupportedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}