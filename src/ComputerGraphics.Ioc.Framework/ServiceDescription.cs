using System;

namespace ComputerGraphics.Ioc.Framework
{
    internal class ServiceDescription
    {
        public Type InterfaceType { get; }
        public Type ImplementationType { get; }
        public DependencyLifetime Lifetime { get; }
        public ServiceDescription(Type interfaceType, Type implementationType, DependencyLifetime lifetime)
        {
            InterfaceType = interfaceType;
            ImplementationType = implementationType;
            Lifetime = lifetime;
        }
    }
}