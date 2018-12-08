using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using Tmpps.Infrastructure.Autofac;
using Tmpps.Infrastructure.Autofac.Configuration;
using Tmpps.Infrastructure.Common.DependencyInjection.Builder;
using Tmpps.Infrastructure.Common.DependencyInjection.Builder.Interfaces;
using Tmpps.Infrastructure.Common.DependencyInjection.Interfaces;
using Tmpps.Infrastructure.Common.Foundation;
using Tmpps.Infrastructure.Common.Foundation.Exceptions;
using Tmpps.Infrastructure.Common.Foundation.Interfaces;
using Tmpps.Infrastructure.Common.IO.Interfaces;
using Tmpps.Infrastructure.Common.ValueObjects;
using Tmpps.Infrastructure.Data.Configuration.Interfaces;
using Tmpps.Infrastructure.Data.Migration.Interfaces;
using Tmpps.Infrastructure.Npgsql.Entity.Migration;

namespace Deploys.Db.Configuration
{
    public class DIModule : IDIModule
    {
        private Assembly executeAssembly;
        private string rootPath;
        private IConfigurationRoot configurationRoot;
        private ILoggerFactory loggerFactory;
        private CommonDIModule commonAutofacModule;

        public DIModule(Assembly executeAssembly, string rootPath, IConfigurationRoot configurationRoot, ILoggerFactory loggerFactory)
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
            builder.RegisterModule(new MigrationDIModule());
            builder.RegisterInstance(this.configurationRoot, x => x.As<IConfigurationRoot>());
            builder.RegisterType<Config>(x =>
                x.As<IMigrationConfig>()
                .SingleInstance());
            builder.RegisterType<MigrationUseCase>(x =>
                x.As<IMigrationUseCase>());
        }
    }
    internal class MigrationUseCase : IMigrationUseCase
    {
        private IMigrationConfig config;
        private IPathResolver pathResolver;
        private IScopeProvider scopeProvider;

        public MigrationUseCase(
            IMigrationConfig config,
            IPathResolver pathResolver,
            IScopeProvider scopeProvider)
        {
            this.config = config;
            this.pathResolver = pathResolver;
            this.scopeProvider = scopeProvider;
        }

        public async Task<int> ExecuteAsync()
        {
            var dir = this.pathResolver.ResolveDirectoryPath(this.config.Path);
            if (string.IsNullOrEmpty(dir))
            {
                throw new BizLogicException($"指定のディレクトリーは存在しません。(path:{this.config.Path})");
            }
            var databases = Directory.GetDirectories(dir).Select(Path.GetFileName).Where(x => string.IsNullOrEmpty(this.config.Database) || this.config.Database == x);
            foreach (var database in databases)
            {
                var files = Directory.GetFiles(Path.Combine(dir, database), "*.sql", SearchOption.AllDirectories)
                    .OrderBy(x => x)
                    .GroupBy(x => Path.GetFileNameWithoutExtension(x))
                    .ToArray();
                var duplicate = files.Where(x => x.Count() > 1).SelectMany(x => x).ToArray();
                if (duplicate.Length > 0)
                {
                    throw new BizLogicException($"ファイル名に重複があります。{Environment.NewLine}{string.Join(Environment.NewLine, duplicate)}");
                }

                var optionsBuilder = new DbContextOptionsBuilder<MigrationDbContext>();
                var connectionStringBuilder = new NpgsqlConnectionStringBuilder(this.config.RootConnectionString);
                connectionStringBuilder.Database = database;
                optionsBuilder.UseNpgsql(connectionStringBuilder.ConnectionString);
                var options = new TypeValuePair<DbContextOptions<MigrationDbContext>>(optionsBuilder.Options);

                using(var scope = this.scopeProvider.BeginLifetimeScope(options))
                {
                    var migrationHelper = scope.Resolve<IMigrationHelper>();
                    await migrationHelper.InitializeDatabaseAsync(database);
                    await migrationHelper.InitializeAsync();
                }
                foreach (var file in files.Select(x => new { Id = x.Key, Path = x.First() }))
                {
                    using(var scope = this.scopeProvider.BeginLifetimeScope(options))
                    {
                        await scope.Resolve<IMigrationHelper>().MigrationAsync(file.Id, file.Path);
                    }
                }
            }

            return 0;
        }
    }

}