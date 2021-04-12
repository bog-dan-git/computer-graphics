using System;
using System.Runtime.Serialization;

namespace ComputerGraphics.PluginLoader.Exceptions
{
    [Serializable]
    public class ConverterNotFoundException : Exception
    {
        public string ConverterName { get; }
        public string ConverterType { get; }
        public ConverterNotFoundException()
        {
        }

        public ConverterNotFoundException(string converterName, string converterType) : base($"Couldn't find {converterName} {converterType}")
        {
            ConverterName = converterName;
            ConverterType = converterName;
        }

        public ConverterNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ConverterNotFoundException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}