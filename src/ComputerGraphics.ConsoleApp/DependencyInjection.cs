using ComputerGraphics.Ioc.Framework;

namespace ComputerGraphics.ConsoleApp
{
    public class DependencyInjection : Provider
    {
        protected override void LoadDependencies()
        {
            Bind<IConsoleRunner>().To<ConsoleRunner>();
        }
    }
}