using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace WebServer.Core.DI
{
    public class ServiceProvider
    {
        private readonly List<ServiceDescriptor> _serviceDescriptors;
        private readonly ConcurrentDictionary<Type, object> _singletonInstances;
        private readonly Dictionary<Type, object> _scopedInstances;

        public ServiceProvider(
            List<ServiceDescriptor> serviceDescriptors,
            ConcurrentDictionary<Type, object>? singletonInstances = null,
            Dictionary<Type, object>? scopedInstances = null)
        {
            _serviceDescriptors = serviceDescriptors;
            _singletonInstances = singletonInstances ?? new ConcurrentDictionary<Type, object>();
            _scopedInstances = scopedInstances ?? new Dictionary<Type, object>();
        }

        public T GetService<T>() => (T)GetService(typeof(T));

        public object GetService(Type serviceType)
        {
            var descriptor = _serviceDescriptors.SingleOrDefault(d => d.ServiceType == serviceType);

            if (descriptor == null)
                throw new Exception($"Service of type {serviceType.Name} not registered");

            if (descriptor.ImplementationInstance != null)
                return descriptor.ImplementationInstance;

            if (descriptor.ImplementationFactory != null)
                return descriptor.ImplementationFactory(this);

            if (descriptor.ImplementationType != null)
            {
                var actualType = descriptor.ImplementationType;
                var ctor = actualType.GetConstructors().First();
                var parameters = ctor.GetParameters()
                    .Select(p => GetService(p.ParameterType))
                    .ToArray();

                var implementation = Activator.CreateInstance(actualType, parameters)!;

                return descriptor.Lifetime switch
                {
                    Lifetime.Singleton => _singletonInstances.GetOrAdd(serviceType, implementation),
                    Lifetime.Scoped => _scopedInstances.TryAdd(serviceType, implementation) ? implementation : _scopedInstances[serviceType],
                    Lifetime.Transient => implementation,
                    _ => throw new Exception("Unknown lifetime")
                };
            }

            throw new Exception("Could not resolve service");
        }

        public ServiceProvider CreateScope()
        {
            return new ServiceProvider(_serviceDescriptors, _singletonInstances, new Dictionary<Type, object>());
        }
    }
}
