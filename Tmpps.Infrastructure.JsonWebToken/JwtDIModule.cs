using Tmpps.Infrastructure.Common.Claims.Interfaces;
using Tmpps.Infrastructure.Common.DependencyInjection.Builder.Interfaces;

namespace Tmpps.Infrastructure.JsonWebToken
{
    public class JwtDIModule : IDIModule
    {
        public void DefineModule(IDIBuilder builder)
        {
            builder.RegisterType<JwtFactory>(x => x.As<IJwtFactory>().InstancePerLifetimeScope());
        }
    }
}