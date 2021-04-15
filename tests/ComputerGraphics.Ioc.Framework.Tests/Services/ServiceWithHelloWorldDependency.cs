namespace ComputerGraphics.Ioc.Framework.Tests.Services
{
    public interface IServiceWithHelloWorldDependency
    {
        string GetHelloWorldMessage();
        string HelloWorldMessage { get; }
    }

    internal class ServiceWithHelloWorldDependency : IServiceWithHelloWorldDependency
    {
        private readonly IHelloWorldService _helloWorldService;

        public ServiceWithHelloWorldDependency(IHelloWorldService helloWorldService)
        {
            _helloWorldService = helloWorldService;
        }

        public string GetHelloWorldMessage() => _helloWorldService.GetMessage();

        public string HelloWorldMessage => _helloWorldService.Message;
    }
}