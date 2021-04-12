using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommandLine;
using ComputerGraphics.PluginLoader;

namespace ComputerGraphics.ConsoleApp
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            await Parser.Default.ParseArguments<Options>(args)
                .MapResult(
                    RunOptions,
                    errs =>
                    {
                        Console.WriteLine("Unable to parse command-line arguments");
                        return Task.CompletedTask;
                    });
        }

        private static async Task RunOptions(Options opts)
        {
            try
            {
                string fileName = opts.FileName;
                string format = opts.Format;
                string output = opts.Output ?? Path.ChangeExtension(fileName, format);
                var pluginLoader = new ConverterPluginsLoader();
                var factory = pluginLoader.LoadConverters();
                var inputExtension = fileName.Split(".").LastOrDefault();
                if (inputExtension == null)
                {
                    throw new ArgumentException("File doesn't seem to have an extension");
                }

                var reader = factory.GetDecoder(inputExtension);
                var writer = factory.GetEncoder(format);
                var bytes = await File.ReadAllBytesAsync(fileName);
                var result = reader.Read(bytes);
                var encoded = writer.Encode(result);
                await File.WriteAllBytesAsync(output, encoded);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}