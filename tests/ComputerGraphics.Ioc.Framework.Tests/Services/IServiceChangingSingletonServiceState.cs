namespace ComputerGraphics.Ioc.Framework.Tests.Services
{
    public interface IServiceChangingSingletonServiceState
    {
        int Counter { get; }

        void IncreaseCounter();
    }

    internal class ServiceChangingSingletonServiceState : IServiceChangingSingletonServiceState
    {
        private readonly ISomeSingletonService _someSingletonService;

        public ServiceChangingSingletonServiceState(ISomeSingletonService someSingletonService) =>
            _someSingletonService = someSingletonService;

        public int Counter => _someSingletonService.Counter;

        public void IncreaseCounter() => _someSingletonService.IncreaseCounter();
    }
}