using System;
using System.Collections.Generic;
using Tmpps.Infrastructure.Common.DependencyInjection.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Tmpps.Infrastructure.Common.DependencyInjection.Builder.Interfaces
{
    public interface IDIBuilder
    {
        bool CanResolveNotAlreadyRegisteredSource { get; set; }
        void RegisterModule(IDIModule module);
        void RegisterType(Type type, Action<DIRegisterTypeObject> action = null);
        void RegisterType<T>(Action<DIRegisterTypeObject<T>> action = null);
        void RegisterInstance<TRegister>(TRegister instance, Action<DIRegisterInstanceObject<TRegister>> action = null) where TRegister : class;
        void RegisterBuildCallback(Action<IScope> callback);
        void Populate(IServiceCollection services);
        IScope Build();
        IServiceProvider CreateServiceProvider();
    }
}