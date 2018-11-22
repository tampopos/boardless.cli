using System;
using System.Linq;
using Autofac;
using Autofac.Builder;
using Autofac.Extensions.DependencyInjection;
using Autofac.Features.ResolveAnything;
using Tmpps.Infrastructure.Common.DependencyInjection.Builder;
using Tmpps.Infrastructure.Common.DependencyInjection.Builder.Interfaces;
using Tmpps.Infrastructure.Common.DependencyInjection.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Tmpps.Infrastructure.Autofac.Builder
{
    public class AutofacBuilder : DIBuilder
    {
        private IContainer container;
        private volatile object lockObject = new object();
        private ContainerBuilder builder = new ContainerBuilder();

        public override void Populate(IServiceCollection services)
        {
            builder.Populate(services);
        }

        public override IServiceProvider CreateServiceProvider()
        {
            this.Build();
            return new AutofacServiceProvider(this.container);
        }

        public override IScope Build()
        {
            if (this.container == null)
            {
                lock(this.lockObject)
                {
                    if (this.container == null)
                    {
                        if (this.CanResolveNotAlreadyRegisteredSource)
                        {
                            // 登録されてない型もコンテナで作成する
                            builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());
                        }
                        this.container = this.builder.Build();
                    }
                }
            }
            return new AutofacScopeWrapper(this.container);
        }
        public override void RegisterModule(IDIModule module)
        {
            module.DefineModule(this);
        }

        public override void RegisterBuildCallback(Action<IScope> callback)
        {
            this.builder.RegisterBuildCallback(container =>
            {
                callback?.Invoke(new AutofacScopeWrapper(container));
            });
        }

        public override void RegisterType<T>(Action<DIRegisterTypeObject<T>> action = null)
        {
            var obj = DIRegisterObjectFactory.CreateTypeObject<T>();
            action?.Invoke(obj);
            this.RegisterType(obj);
        }
        public override void RegisterType(Type type, Action<DIRegisterTypeObject> action = null)
        {
            var obj = DIRegisterObjectFactory.CreateTypeObject(type);
            action?.Invoke(obj);
            this.RegisterType(obj);
        }
        private void RegisterType(DIRegisterTypeObject obj)
        {
            var b = this.Register(this.builder.RegisterType(obj.RegisterType), obj);
            switch (obj.LifetimeType)
            {
                case LifetimeType.Singleton:
                    b.SingleInstance();
                    break;
                case LifetimeType.Scoped:
                    if (obj.ScopeTags?.Count > 0)
                    {
                        b.InstancePerMatchingLifetimeScope(obj.ScopeTags.ToArray());
                        break;
                    }
                    b.InstancePerLifetimeScope();
                    break;
            }
        }

        private IRegistrationBuilder<T1, T2, SingleRegistrationStyle> Register<T1, T2>(IRegistrationBuilder<T1, T2, SingleRegistrationStyle> builder, DIRegisterObject obj)
        where T2 : IConcreteActivatorData
        {
            foreach (var aliasType in obj.AliasTypes)
            {
                builder = builder.As(aliasType);
            }
            builder.AsSelf();
            return builder;
        }

        public override void RegisterInstance<TRegister>(TRegister instance, Action<DIRegisterInstanceObject<TRegister>> action = null)
        {
            var obj = DIRegisterObjectFactory.CreateInstanceObject(instance);
            action?.Invoke(obj);
            this.Register(this.builder.RegisterInstance(obj.Instance), obj);
        }
    }
}