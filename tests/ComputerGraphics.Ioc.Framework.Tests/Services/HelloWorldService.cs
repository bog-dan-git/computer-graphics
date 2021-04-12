namespace ComputerGraphics.Ioc.Framework.Tests.Services
{
    public interface IHelloWorldService
    {
        string Message { get; }
        string GetMessage();
    }

    internal class HelloWorldService : IHelloWorldService
    {
        public string Message => "Hello, world";
        public string GetMessage()
        {
            return "Hello, world";
        }
    }
}