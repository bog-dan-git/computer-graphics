using System;
using System.Collections.Generic;

namespace ComputerGraphics.Converters.Sdk
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ImageDecoderAttribute : Attribute
    {
        public IEnumerable<string> Formats { get; }
        

        public ImageDecoderAttribute(string format)
        {
            Formats = new[] {format};
        }

        public ImageDecoderAttribute(string[] formats)
        {
            Formats = formats;
        }
    }
}