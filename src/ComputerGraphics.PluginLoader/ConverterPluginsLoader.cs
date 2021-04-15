using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ComputerGraphics.Converters.Sdk;
using ComputerGraphics.Converters.Sdk.Interfaces;
using ComputerGraphics.PluginLoader.Utils;

namespace ComputerGraphics.PluginLoader
{
    public interface IConverterPluginsLoader
    {
        IConverterFactory LoadConverters();
    }

    internal class ConverterPluginsLoader : IConverterPluginsLoader
    {
        private readonly Assembly[] _initialAssemblies =
        {
            typeof(ComputerGraphics.Converters.Ppm.PpmEncoder).Assembly,
            typeof(ComputerGraphics.Jpeg.JpegEncoder).Assembly
        };

        public IConverterFactory LoadConverters()
        {
            var decoders = new IgnoreCaseDictionary<IImageDecoder>();
            var encoders = new IgnoreCaseDictionary<IImageEncoder>();
            var pluginAssemblies = LoadPlugins();
            foreach (var assembly in pluginAssemblies)
            {
                AddDecodersFromAssembly(assembly, decoders);
                AddEncodersFromAssembly(assembly, encoders);
            }

            return new ConverterFactory(decoders, encoders);
        }

        private Assembly[] LoadPlugins()
        {
            var result = new List<Assembly>(_initialAssemblies);
            var path = Path.Join(AppContext.BaseDirectory, "Plugins");
            if (Directory.Exists(path))
            {
                var dlls = Directory.GetFiles(path, "*.dll");
                foreach (var el in dlls)
                {
                    try
                    {
                        var loadContext = new PluginLoadContext(el);
                        result.Add(loadContext.LoadFromAssemblyName(
                            new AssemblyName(Path.GetFileNameWithoutExtension(el))));
                    }
                    // Just ignore if we can't
                    catch
                    {
                    }
                }
            }

            return result.ToArray();
        }

        private void AddDecodersFromAssembly(Assembly assembly,
            Dictionary<string, IImageDecoder> result)
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (type.GetInterfaces().Contains(typeof(IImageDecoder)))
                {
                    var attribute = type.GetCustomAttribute<ImageDecoderAttribute>();
                    if (attribute is null)
                    {
                        Console.WriteLine(
                            $"WARNING. Type {type} extends IImageDecoder, but doesn't have required ImageReader attribute, so it can't be registered");
                        continue;
                    }

                    var instance = Activator.CreateInstance(type) as IImageDecoder;
                    foreach (var format in attribute.Formats)
                    {
                        if (!result.TryAdd(format, instance))
                        {
                            Console.WriteLine($"Reader for {format} is already added and will not be replaced");
                        }
                    }
                }
            }
        }


        private void AddEncodersFromAssembly(Assembly assembly,
            Dictionary<string, IImageEncoder> result)
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (type.GetInterfaces().Contains(typeof(IImageEncoder)))
                {
                    var attribute = type.GetCustomAttribute<ImageEncoderAttribute>();
                    if (attribute is null)
                    {
                        Console.WriteLine(
                            $"WARNING. Type {type} extends IImageEncoder, but doesn't have required ImageWriter attribute, so it can't be registered");
                        continue;
                    }

                    var instance = Activator.CreateInstance(type) as IImageEncoder;
                    foreach (var format in attribute.Formats)
                    {
                        if (!result.TryAdd(format, instance))
                        {
                            Console.WriteLine($"Reader for {format} is already added and will not be replaced");
                        }
                    }
                }
            }
        }
    }
}