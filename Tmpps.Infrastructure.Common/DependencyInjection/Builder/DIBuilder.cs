using System;
using System.Collections.Generic;
using System.Linq;
using Tmpps.Infrastructure.Common.DependencyInjection.Builder.Interfaces;
using Tmpps.Infrastructure.Common.DependencyInjection.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Tmpps.Infrastructure.Common.DependencyInjection.Builder
{
    public abstract class DIBuilder : IDIBuilder
    {
        protected virtual IList<DIRegisterObject> RegisterObjects { get; } = new List<DIRegisterObject>();
        public virtual bool CanResolveNotAlreadyRegisteredSource { get; set; }
        public abstract void RegisterModule(IDIModule module);
        public abstract void RegisterBuildCallback(Action<IScope> callback);
        public abstract void Populate(IServiceCollection services);
        public abstract IScope Build();
        public abstract IServiceProvider CreateServiceProvider();
        public abstract void RegisterType(Type type, Action<DIRegisterTypeObject> action = null);
        public abstract void RegisterType<T>(Action<DIRegisterTypeObject<T>> action = null);
        public abstract void RegisterInstance<TRegister>(TRegister instance, Action<DIRegisterInstanceObject<TRegister>> action = null) where TRegister : class;
    }
}