using System;
using System.Runtime.Serialization;

namespace ComputerGraphics.Gif.Exceptions
{
    [Serializable]
    public class InterlacedUnsupportedException : Exception
    {
        public InterlacedUnsupportedException() : base("Interlace is not supported")
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
        
        protected InterlacedUnsupportedException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}