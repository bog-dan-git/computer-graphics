using ComputerGraphics.Ioc.Framework;

namespace ComputerGraphics.PluginLoader
{
    public class DependencyInjection : DependencyProvider
    {
        protected override void LoadDependencies()
        {
            Bind<IConverterPluginsLoader>().To<ConverterPluginsLoader>();
        }
    }
}