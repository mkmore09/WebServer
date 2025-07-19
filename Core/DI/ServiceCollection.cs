using System;
using System.Collections.Generic;

namespace WebServer.Core.DI
{
    public class ServiceCollection
    {
        private readonly List<ServiceDescriptor> _descriptors = new();

        public void AddSingleton<TService>(TService implementationInstance)
        {
            _descriptors.Add(new ServiceDescriptor(typeof(TService), implementationInstance));
        }

        

        public void AddSingleton<TService>(Func<ServiceProvider, object> factory)
        {
            _descriptors.Add(new ServiceDescriptor(typeof(TService), factory, Lifetime.Singleton));
        }

        public void AddScoped<TService, TImplementation>()
        {
            _descriptors.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), Lifetime.Scoped));
        }

        public void AddScoped<TService>(Func<ServiceProvider, object> factory)
        {
            _descriptors.Add(new ServiceDescriptor(typeof(TService), factory, Lifetime.Scoped));
        }

        public void AddTransient<TService, TImplementation>()
        {
            _descriptors.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), Lifetime.Transient));
        }

        public void AddTransient<TService>(Func<ServiceProvider, object> factory)
        {
            _descriptors.Add(new ServiceDescriptor(typeof(TService), factory, Lifetime.Transient));
        }

        public List<ServiceDescriptor> GetDescriptors() => _descriptors;
    }
}
