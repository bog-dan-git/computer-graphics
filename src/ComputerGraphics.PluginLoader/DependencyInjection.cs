using ComputerGraphics.Ioc.Framework;

namespace ComputerGraphics.PluginLoader
{
    public class DependencyInjection : Provider
    {
        protected override void LoadDependencies()
        {
            Bind<IConverterPluginsLoader>().To<ConverterPluginsLoader>();
        }
    }
}