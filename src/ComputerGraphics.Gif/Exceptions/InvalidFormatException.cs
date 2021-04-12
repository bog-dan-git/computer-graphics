using System;
using System.Runtime.Serialization;

namespace ComputerGraphics.Gif.Exceptions
{
    [Serializable]
    public class InvalidFormatException : Exception
    {
        public InvalidFormatException() : base("Invalid format!")
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

        protected InvalidFormatException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}