using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using ComputerGraphics.Ioc.Framework.Exceptions;

namespace ComputerGraphics.Ioc.Framework
{
    public class CustomServiceCollection
    {
        private readonly ImmutableDictionary<Type, ServiceDescription> _services;
        private readonly HashSet<Type> _empty = new();
        private readonly Dictionary<Type, object> _singletons = new();

        internal CustomServiceCollection(ImmutableDictionary<Type, ServiceDescription> services) =>
            _services = services;

        public T GetService<T>()
        {
            var type = typeof(T);
            return (T) GetService(type);
        }

        private object GetService(Type type)
        {
            if (!_services.TryGetValue(type, out var implementation)) throw new DependencyNotFoundException(type);
            switch (implementation.Lifetime)
            {
                case DependencyLifetime.Transient:
                    return ResolveService(implementation.ImplementationType);
                case DependencyLifetime.Singleton when _singletons.TryGetValue(type, out var imp):
                    return imp;
                case DependencyLifetime.Singleton:
                {
                    var service = ResolveService(implementation.ImplementationType);
                    _singletons[type] = service;
                    return service;
                }
                default:
                    throw new DependencyNotFoundException();
            }
        }

        private object ResolveService(Type type)
        {
            if (_empty.Contains(type))
            {
                return Activator.CreateInstance(type);
            }

            var ctor = type.GetConstructors().FirstOrDefault(_ => _.IsPublic);
            if (ctor is null)
            {
                _empty.Add(type);
                return Activator.CreateInstance(type);
            }

            var ctorParamsInfo = ctor.GetParameters();
            var ctorParams = new List<object>(ctorParamsInfo.Length);
            ctorParams.AddRange(ctorParamsInfo.Select(param => GetService(param.ParameterType)));

            var result = ctor.Invoke(ctorParams.ToArray());
            return result;
        }
    }
}