using System;

namespace ComputerGraphics.Converters.Sdk
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ImageReaderAttribute : Attribute
    {
        public string Format { get; }

        public ImageReaderAttribute(string format)
        {
            Format = format;
        }
    }
}