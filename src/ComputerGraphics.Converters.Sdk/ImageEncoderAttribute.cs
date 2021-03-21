using System;
using System.Collections.Generic;

namespace ComputerGraphics.Converters.Sdk
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ImageEncoderAttribute : Attribute
    {
        public IEnumerable<string> Formats { get; }

        public ImageEncoderAttribute(string format)
        {
            Formats = new[] {format};
        }

        public ImageEncoderAttribute(string[] formats)
        {
            Formats = formats;
        }
    }
}