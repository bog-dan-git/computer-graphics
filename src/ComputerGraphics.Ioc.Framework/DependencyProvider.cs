using System.Collections.Generic;

namespace ComputerGraphics.Ioc.Framework
{
    public abstract class DependencyProvider
    {
        internal readonly ICollection<ServiceDescription> ServiceDescriptions = new List<ServiceDescription>();
        protected IBinder<T> Bind<T>()
        {
            var binder = new Binder<T>(ServiceDescriptions);
            return binder;
        }

        protected internal abstract void LoadDependencies();
    }
}