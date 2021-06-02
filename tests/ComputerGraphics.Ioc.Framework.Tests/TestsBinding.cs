using ComputerGraphics.Ioc.Framework.Tests.Services;

namespace ComputerGraphics.Ioc.Framework.Tests
{
    public class TestDependencyProviders : DependencyProvider
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