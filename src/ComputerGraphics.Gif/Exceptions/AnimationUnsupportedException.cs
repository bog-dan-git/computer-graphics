using System;
using System.Runtime.Serialization;

namespace ComputerGraphics.Gif.Exceptions
{
    [Serializable]
    public class AnimationUnsupportedException : Exception
    {
        public AnimationUnsupportedException() : base("Animation is not supported")
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

        protected AnimationUnsupportedException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}