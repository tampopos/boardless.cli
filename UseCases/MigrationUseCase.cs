using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using Tmpps.Infrastructure.Common.DependencyInjection.Interfaces;
using Tmpps.Infrastructure.Common.Foundation.Exceptions;
using Tmpps.Infrastructure.Common.IO.Interfaces;
using Tmpps.Infrastructure.Common.ValueObjects;
using Tmpps.Infrastructure.Data.Migration.Interfaces;
using Tmpps.Infrastructure.Npgsql.Entity.Migration;
using UseCases.Interfaces;

namespace UseCases
{
    public class MigrationUseCase : IMigrationUseCase
    {
        private IConfig config;
        private ILogger logger;
        private IPathResolver pathResolver;
        private IScopeProvider scopeProvider;
        private IMigrationStore migrationStore;

        public MigrationUseCase(
            IConfig config,
            ILogger logger,
            IPathResolver pathResolver,
            IScopeProvider scopeProvider,
            IMigrationStore migrationStore)
        {
            this.config = config;
            this.logger = logger;
            this.pathResolver = pathResolver;
            this.scopeProvider = scopeProvider;
            this.migrationStore = migrationStore;
        }

        public async Task<MigrationState> ExecuteAsync()
        {
            var stopwatch = Stopwatch.StartNew();
            this.migrationStore.State = MigrationState.InProgress;
            try
            {
                this.logger.LogInformation($"Start migration");
                await ExecuteInnerAsync();
                this.logger?.LogInformation($"End migration");
            }
            catch (OperationCanceledException ex)
            {
                this.logger?.LogWarning(ex.Message);
            }
            catch (Exception ex)
            {
                this.logger?.LogError(ex, $"Error migration");
            }
            return this.migrationStore.State;

            async Task ExecuteInnerAsync()
            {
                try
                {
                    var r = await this.MigrationAsync();
                    if (r != 0)
                    {
                        throw new Exception("Failure migration");
                    }
                    this.migrationStore.State = MigrationState.Success;
                    return;
                }
                catch (Exception ex)
                {
                    this.logger.LogWarning(ex, "Error migration");
                    if (this.config.Timeout <= stopwatch.ElapsedMilliseconds / 1000)
                    {
                        this.migrationStore.State = MigrationState.Failure;
                        return;
                    }
                    this.logger.LogInformation("Wait 5 sec...");
                    await Task.Delay(5000);
                    this.logger.LogInformation("Retry migration");
                    await ExecuteInnerAsync();
                }
            }
        }

        private async Task<int> MigrationAsync()
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

        public async Task<MigrationState> GetStateAsync() => await Task.FromResult(this.migrationStore.State);
    }
}