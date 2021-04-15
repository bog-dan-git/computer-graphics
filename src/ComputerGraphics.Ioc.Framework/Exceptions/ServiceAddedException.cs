using System;
using System.Runtime.Serialization;

namespace ComputerGraphics.Ioc.Framework.Exceptions
{
    [Serializable]
    public class ServiceAddedException : Exception
    {
        public ServiceAddedException()
        {
        }

        public ServiceAddedException(Type interfaceType, Type existingImplementation, Type wantedImplementation) :
            base(
                $"Implementation of service for type {interfaceType.Name} is already bind to {existingImplementation.Name}. However, an attempt was made to bind to {wantedImplementation.Name}")
        {
        }

        public ServiceAddedException(string message) : base(message)
        {
        }

        public ServiceAddedException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ServiceAddedException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}