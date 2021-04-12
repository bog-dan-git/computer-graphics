using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using ComputerGraphics.Ioc.Framework.Exceptions;

namespace ComputerGraphics.Ioc.Framework
{
    public sealed class Container
    {
        private readonly IEnumerable<Assembly> _assemblies;

        public Container(IEnumerable<Assembly> assemblies)
        {
            _assemblies = assemblies;
        }

        public CustomServiceCollection Build()
        {
            var descriptors = new Dictionary<Type, ServiceDescription>();
            foreach (var assembly in _assemblies)
            {
                var types = assembly.GetTypes().Where(_ => _.BaseType == typeof(Provider));
                foreach (var type in types)
                {
                    var instance = Activator.CreateInstance(type) as Provider;
                    instance.LoadDependencies();
                    AddDependenciesToDictionary(descriptors, instance.ServiceDescriptions);
                }
            }

            return new CustomServiceCollection(descriptors.ToImmutableDictionary());
        }

        private void AddDependenciesToDictionary(Dictionary<Type, ServiceDescription> dict,
            ICollection<ServiceDescription> descriptions)
        {
            foreach (var description in descriptions)
            {
                if (!dict.TryAdd(description.InterfaceType, description))
                {
                    throw new ServiceAddedException(description.InterfaceType,
                        dict[description.InterfaceType].ImplementationType, description.ImplementationType);
                }
            }
        }
    }
}