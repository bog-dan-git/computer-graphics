using System;
using System.Runtime.Serialization;

namespace ComputerGraphics.Ioc.Framework.Exceptions
{
    [Serializable]
    public class DependencyNotFoundException : Exception
    {
        public DependencyNotFoundException()
        {
        }

        public DependencyNotFoundException(Type type) : base($"Can't resolve service for type {type.Name}")
        {
        }

        public DependencyNotFoundException(string message) : base(message)
        {
        }

        public DependencyNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }

        protected DependencyNotFoundException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}