using System.Collections.Generic;
using Tmpps.Infrastructure.Common.DependencyInjection.Builder;
using Tmpps.Infrastructure.Common.DependencyInjection.Builder.Interfaces;
using Tmpps.Infrastructure.Common.DependencyInjection.Interfaces;

namespace Tmpps.Infrastructure.Autofac
{
    public class AutofacDIModule : IDIModule
    {
        public void DefineModule(IDIBuilder builder)
        {
            builder.RegisterInstance(new AutofacScopeProvider(), x => x.As<IScopeProvider>());

            builder.RegisterBuildCallback(container =>
            {
                container.Resolve<IScopeProvider>().CurrentScope = container;
            });
        }
    }
}