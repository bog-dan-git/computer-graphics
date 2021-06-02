using ComputerGraphics.Ioc.Framework;

namespace ComputerGraphics.ConsoleApp
{
    public class DependencyInjection : DependencyProvider
    {
        protected override void LoadDependencies()
        {
            Bind<IConsoleRunner>().To<ConsoleRunner>();
        }
    }
}