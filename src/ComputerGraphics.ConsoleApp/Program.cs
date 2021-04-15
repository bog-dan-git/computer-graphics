using System;
using System.Reflection;
using System.Threading.Tasks;
using CommandLine;
using ComputerGraphics.Ioc.Framework;

namespace ComputerGraphics.ConsoleApp
{
    public static class Program
    {
        private static readonly Assembly[] ModuleAssemblies =
        {
            typeof(RayTracing.Implementation.RayTracingImplementationProvider).Assembly,
            typeof(Program).Assembly,
            typeof(ComputerGraphics.PluginLoader.DependencyInjection).Assembly,
        };

        public static async Task Main(string[] args)
        {
            var container = new Container(ModuleAssemblies);
            var serviceCollection = container.Build();
            var runner = serviceCollection.GetService<IConsoleRunner>();
            await Parser.Default.ParseArguments<Options>(args)
                .WithParsedAsync(async _ =>
                {
                    try
                    {
                        await runner.RunAsync(_.FileName, _.Output);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                });

        }
    }
}