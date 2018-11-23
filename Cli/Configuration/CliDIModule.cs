using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Tmpps.Infrastructure.Autofac;
using Tmpps.Infrastructure.Autofac.Configuration;
using Tmpps.Infrastructure.Common.DependencyInjection.Builder;
using Tmpps.Infrastructure.Common.DependencyInjection.Builder.Interfaces;
using Tmpps.Infrastructure.Common.Foundation;
using Tmpps.Infrastructure.Common.Foundation.Interfaces;
using Tmpps.Infrastructure.Data.Configuration.Interfaces;
using Tmpps.Infrastructure.Data.Migration.Interfaces;
using Tmpps.Infrastructure.JsonWebToken;
using Tmpps.Infrastructure.JsonWebToken.Interfaces;
using Tmpps.Infrastructure.Npgsql.Entity.Migration;
using UseCases;

namespace Cli.Configuration
{
    public class CliDIModule : IDIModule
    {
        private Assembly executeAssembly;
        private string rootPath;
        private IConfigurationRoot configurationRoot;
        private ILoggerFactory loggerFactory;
        private CommonDIModule commonAutofacModule;

        public CliDIModule(Assembly executeAssembly, string rootPath, IConfigurationRoot configurationRoot, ILoggerFactory loggerFactory)
        {
            this.executeAssembly = executeAssembly;
            this.rootPath = rootPath;
            this.configurationRoot = configurationRoot;
            this.loggerFactory = loggerFactory;
            this.commonAutofacModule = new CommonDIModule(executeAssembly, rootPath, loggerFactory);
        }

        public void DefineModule(IDIBuilder builder)
        {
            var mapRegister = new MapRegister();
            builder.RegisterInstance(mapRegister, x => x.As<IMapRegister>());
            builder.RegisterModule(this.commonAutofacModule);
            builder.RegisterModule(new AutofacDIModule());
            builder.RegisterModule(new JwtDIModule());
            builder.RegisterModule(new UseCasesDIModule());
            builder.RegisterModule(new MigrationDIModule(this.configurationRoot.GetConnectionString("MigrationConnection")));
            builder.RegisterInstance(this.configurationRoot, x => x.As<IConfigurationRoot>());
            builder.RegisterType<CliConfig>(x =>
                x.As<IDbConfig>()
                .As<IJwtConfig>()
                .As<IMigrationConfig>()
                .SingleInstance());
        }
    }
}