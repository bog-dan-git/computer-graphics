using System;

namespace ComputerGraphics.Converters.Sdk
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ImageWriterAttribute : Attribute
    {
        public string Format { get; }

        public ImageWriterAttribute(string format)
        {
            Format = format;
        }
    }
}