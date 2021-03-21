using System;
using ComputerGraphics.Converters.Sdk.Interfaces;
using ComputerGraphics.PluginLoader.Exceptions;
using ComputerGraphics.PluginLoader.Utils;

namespace ComputerGraphics.PluginLoader
{
    public interface IConverterFactory
    {
        /// <summary>
        /// Gets image decoder based on extension
        /// </summary>
        /// <param name="extension"></param>
        /// <exception cref="ConverterNotFoundException">Converter not found</exception>
        /// <returns>Decoder</returns>
        IImageDecoder GetDecoder(string extension);

        /// <summary>
        /// Gets image encoder based on extension
        /// </summary>
        /// <param name="extension"></param>
        /// <exception cref="ConverterNotFoundException">Converter not found</exception>
        /// <returns>Encoder</returns>
        IImageEncoder GetEncoder(string extension);
    }

    internal class ConverterFactory : IConverterFactory
    {
        private readonly IgnoreCaseDictionary<IImageDecoder> _decoders;
        private readonly IgnoreCaseDictionary<IImageEncoder> _encoders;

        public ConverterFactory(IgnoreCaseDictionary<IImageDecoder> decoders,
            IgnoreCaseDictionary<IImageEncoder> encoders)
        {
            Console.WriteLine(
                $"Loaded {decoders.Count} decoder{(decoders.Count != 1 ? "s" : "")} and {encoders.Count} encoder{(encoders.Count != 1 ? "s" : "")}");
            _decoders = decoders;
            _encoders = encoders;
        }


        public IImageDecoder GetDecoder(string extension)
        {
            if (_decoders.TryGetValue(extension, out var reader))
            {
                return reader;
            }

            throw new ConverterNotFoundException(extension, "decoder");
        }

        public IImageEncoder GetEncoder(string extension)
        {
            if (_encoders.TryGetValue(extension, out var writer))
            {
                return writer;
            }

            throw new ConverterNotFoundException(extension, "encoder");
        }
    }
}