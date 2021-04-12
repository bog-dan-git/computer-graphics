using ComputerGraphics.Ioc.Framework.Exceptions;
using ComputerGraphics.Ioc.Framework.Tests.Services;
using Xunit;

namespace ComputerGraphics.Ioc.Framework.Tests
{
    public class ContainerTests
    {
        private readonly CustomServiceCollection _services =
            new Container(new[] {typeof(ContainerTests).Assembly}).Build();

        [Fact]
        public void GetService_HelloWorldService_ShouldResolve()
        {
            var bindHelloWorldService = _services.GetService<IHelloWorldService>();
            var expectedHelloWorldService = new HelloWorldService();
            Assert.Equal(expectedHelloWorldService.Message, bindHelloWorldService.Message);
            Assert.Equal(expectedHelloWorldService.GetMessage(), bindHelloWorldService.GetMessage());
        }

        [Fact]
        public void GetService_ServiceWithSingleDependency_ShouldResolve()
        {
            var bindService = _services.GetService<IServiceWithHelloWorldDependency>();
            var expectedHelloWorldService = new HelloWorldService();
            Assert.Equal(expectedHelloWorldService.Message, bindService.HelloWorldMessage);
            Assert.Equal(expectedHelloWorldService.GetMessage(), bindService.GetHelloWorldMessage());
        }

        [Fact]
        public void GetService_ServiceWithNonExistingDependency_ShouldThrow()
        {
            Assert.Throws<DependencyNotFoundException>(() =>
                _services.GetService<IServiceWithUnregisteredDependency>());
        }

        [Fact]
        public void GetService_SingletonService_ShouldReturnSameObjects()
        {
            var singletonService = _services.GetService<ISomeSingletonService>();
            var sameSingletonService = _services.GetService<ISomeSingletonService>();
            Assert.Equal(singletonService, sameSingletonService);
        }

        [Fact]
        public void ServiceChangingSingleton_ShouldChangeSingleton()
        {
            var singletonService = _services.GetService<ISomeSingletonService>();
            var serviceChangingSingleton = _services.GetService<IServiceChangingSingletonServiceState>();
            serviceChangingSingleton.IncreaseCounter();
            Assert.Equal(serviceChangingSingleton.Counter, singletonService.Counter);
        }
    }
}