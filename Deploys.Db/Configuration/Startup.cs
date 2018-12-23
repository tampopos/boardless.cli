using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading;
using Deploys.Db.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Tmpps.Infrastructure.AspNetCore.Extensions;
using Tmpps.Infrastructure.AspNetCore.Middlewares.Extensions;
using Tmpps.Infrastructure.Autofac.Builder;
using Tmpps.Infrastructure.Common.Configuration;
using Tmpps.Infrastructure.Common.DependencyInjection.Interfaces;
using UseCases;

namespace Deploys.Db.Configuration
{
    public class Startup
    {
        private IHostingEnvironment hostingEnvironment;
        private ILoggerFactory loggerFactory;
        private IConfiguration configuration;
        private IConfigurationRoot configurationRoot;
        private Assembly executeAssembly;
        private string rootPath;
        private Config config;

        public Startup(IHostingEnvironment hostingEnvironment, ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            this.hostingEnvironment = hostingEnvironment;
            this.loggerFactory = loggerFactory;
            this.configuration = configuration;
            this.executeAssembly = Assembly.GetEntryAssembly();
            this.rootPath = Directory.GetCurrentDirectory();
            this.configurationRoot = this.CreateConfigurationRoot();
        }

        private IConfigurationRoot CreateConfigurationRoot()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(hostingEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional : true, reloadOnChange : true)
                .AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", optional : true)
                .AddEnvironmentVariables()
                .AddJsonFile($"appsettings.user.json", optional : true);
            return builder.Build();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            this.config = new Config(this.configurationRoot);
            services.AddMvc().AddControllersAsServices();

            var builder = new AutofacBuilder();
            builder.Populate(services);
            builder.RegisterModule(new DIModule(this.executeAssembly, this.rootPath, this.configurationRoot, this.loggerFactory));
            var scope = builder.Build();
            this.Migration(scope);
            return builder.CreateServiceProvider();
        }

        private void Migration(IScope scope)
        {
            if (this.config.SkipInitialMigration)
            {
                return;
            }
            var useCase = scope.Resolve<IMigrationUseCase>();
            var result = useCase.ExecuteAsync().GetAwaiter().GetResult();
            if (result != MigrationState.Success)
            {
                throw new Exception("Migrationに失敗しました。");
            }
        }

        public void Configure(IApplicationBuilder builder)
        {
            builder.UseDeveloperExceptionPage();
            builder.UseMvc(this.config.CreateMvcConfigureRoutes);
        }
    }
}