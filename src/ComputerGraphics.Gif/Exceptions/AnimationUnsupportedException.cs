using System;

namespace ComputerGraphics.GifReader
{
    public class AnimationUnsupportedException : Exception
    {
        public AnimationUnsupportedException()
        {
        }

        public AnimationUnsupportedException(string message)
            : base(message)
        {
        }

        public AnimationUnsupportedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}