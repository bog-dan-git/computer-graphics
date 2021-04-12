namespace ComputerGraphics.Ioc.Framework.Tests.Services
{
    public interface IServiceWithUnregisteredDependency
    {
    }

    internal class ServiceWithUnregisteredDependency : IServiceWithUnregisteredDependency
    {
        private readonly string _unresolvedDependency;
        public ServiceWithUnregisteredDependency(string unresolvedDependency)
        {
            _unresolvedDependency = unresolvedDependency;
        }
    }
}