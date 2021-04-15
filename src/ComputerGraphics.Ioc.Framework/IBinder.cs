using System.Collections.Generic;

namespace ComputerGraphics.Ioc.Framework
{
    public interface IBinder<TInterface>
    {
        void To<TImplementation>() where TImplementation : class, TInterface;

        public void ToSingleton<TImplementation>() where TImplementation : class, TInterface;

    }

    internal class Binder<TInterface> : IBinder<TInterface>
    {
        private readonly ICollection<ServiceDescription> _serviceDescriptions;

        public Binder(ICollection<ServiceDescription> serviceDescriptions)
        {
            _serviceDescriptions = serviceDescriptions;
        }

        public void To<TImplementation>() where TImplementation : class, TInterface
        {
            _serviceDescriptions.Add(new ServiceDescription(typeof(TInterface), typeof(TImplementation),
                DependencyLifetime.Transient));
        }

        public void ToSingleton<TImplementation>() where TImplementation : class, TInterface
        {
            _serviceDescriptions.Add(new ServiceDescription(typeof(TInterface), typeof(TImplementation),
                DependencyLifetime.Singleton));
        }
    }
}