using ComputerGraphics.Ioc.Framework.Tests.Services;

namespace ComputerGraphics.Ioc.Framework.Tests
{
    public class TestProviders : Provider
    {
        protected override void LoadDependencies()
        {
            Bind<IHelloWorldService>().To<HelloWorldService>();
            Bind<IServiceWithHelloWorldDependency>().To<ServiceWithHelloWorldDependency>();
            Bind<IServiceWithUnregisteredDependency>().To<ServiceWithUnregisteredDependency>();
            Bind<ISomeSingletonService>().ToSingleton<SomeSingletonService>();
            Bind<IServiceChangingSingletonServiceState>().To<ServiceChangingSingletonServiceState>();
        }
    }
}