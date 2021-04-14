using System;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using ComputerGraphics.Converters.Sdk.Model;
using ComputerGraphics.PluginLoader;
using ComputerGraphics.RayTracing.Core.Entities;
using ComputerGraphics.RayTracing.Core.Interfaces;
using ComputerGraphics.RayTracing.Entities.Entities;

namespace ComputerGraphics.ConsoleApp
{
    internal interface IConsoleRunner
    {
        Task RunAsync(string inputFileName, string outputFileName);
    }

    internal class ConsoleRunner : IConsoleRunner
    {
        private readonly IRayTracer _rayTracer;
        private readonly IConverterPluginsLoader _pluginsLoader;

        public ConsoleRunner(IRayTracer rayTracer, IConverterPluginsLoader pluginsLoader) =>
            (_rayTracer, _pluginsLoader) = (rayTracer, pluginsLoader);

        public async Task RunAsync(string inputFileName, string outputFileName)
        {
            if (!File.Exists(inputFileName))
            {
                Console.WriteLine("File does not exist");
            }

            var outputExtension = Path.GetExtension(outputFileName).TrimStart('.');
            var startTime = DateTime.Now;
            var file = new ObjLoader.ObjLoader().Load(inputFileName);
            Console.WriteLine("Started creating mesh");
            var mesh = new Mesh(file.Faces.Select(_ => new Triangle(_.A, _.B, _.C)).ToArray());
            Console.WriteLine("Finished creating mesh");
            var scene = new Scene(new[] {mesh});
            Console.WriteLine("Started tracing");
            var traced = _rayTracer.Trace(scene);
            Console.WriteLine("Finished tracing");
            var rgbs = ConvertToRgb(traced);
            var writer = _pluginsLoader.LoadConverters();
            var encoder = writer.GetEncoder(outputExtension);
            var imageEncoded = encoder.Encode(rgbs);
            await File.WriteAllBytesAsync(outputFileName, imageEncoded);
            var endTime = DateTime.Now;
            var diffMs = endTime.Subtract(startTime).TotalMilliseconds;
            Console.WriteLine($"Finished in {(int) diffMs} ms");
        }

        private RgbColor[,] ConvertToRgb(Vector3[,] traced)
        {
            int width = traced.GetLength(0);
            int height = traced.GetLength(1);
            var result = new RgbColor[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    var current = traced[i, j];
                    result[i, j] = new RgbColor()
                    {
                        R = ToRgbComponent(current.X),
                        G = ToRgbComponent(current.Y),
                        B = ToRgbComponent(current.Z)
                    };
                }
            }

            return result;
        }

        private static byte ToRgbComponent(float v) => (byte) Math.Clamp(v * 255, 0, 255);
    }
}