using System;

namespace WebServer.Core.DI
{
    public class ServiceDescriptor
    {
        public Type ServiceType { get; }
        public Type ImplementationType { get; }
        public object ImplementationInstance { get; internal set; }
        public Func<ServiceProvider, object> ImplementationFactory { get; }
        public Lifetime Lifetime { get; }

        public ServiceDescriptor(Type serviceType, Type implementationType, Lifetime lifetime)
        {
            ServiceType = serviceType;
            ImplementationType = implementationType;
            Lifetime = lifetime;
        }

        public ServiceDescriptor(Type serviceType, Func<ServiceProvider, object> factory, Lifetime lifetime)
        {
            ServiceType = serviceType;
            ImplementationFactory = factory;
            Lifetime = lifetime;
        }

        public ServiceDescriptor(Type serviceType, object instance)
        {
            ServiceType = serviceType;
            ImplementationInstance = instance;
            Lifetime = Lifetime.Singleton;
        }
    }
}
