using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Tmpps.Infrastructure.Common.DependencyInjection.Interfaces;
using Tmpps.Infrastructure.Data.Migration.Interfaces;

namespace Deploys.Db.Configuration
{
    public class Application
    {
        private CommandLineApplication application;
        private ILogger logger;
        private IScopeProvider scopeProvider;
        private Config config;

        public Application(
            CommandLineApplication application,
            ILogger logger,
            IScopeProvider scopeProvider,
            Config config
        )
        {
            this.application = application;
            this.logger = logger;
            this.scopeProvider = scopeProvider;
            this.config = config;
        }

        public int Execute(string[] args)
        {
            this.application.Name = "Boardless.Deploys.SQS";
            this.application.Description = "Boardless Db の展開アプリ";
            this.application.HelpOption("-h|--help");
            this.application.OnExecute(async() =>
            {
                try
                {
                    this.logger.LogInformation($"Start migration");
                    var res = await this.MigrationAsync();
                    this.logger?.LogInformation($"End migration");
                    return res;
                }
                catch (OperationCanceledException ex)
                {
                    this.logger?.LogWarning(ex.Message);
                }
                catch (Exception ex)
                {
                    this.logger?.LogError(ex, $"Error migration");
                }
                return 1;
            });

            return this.application.Execute(args);
        }

        private async Task<int> MigrationAsync()
        {
            var stopwatch = new Stopwatch();
            return await MigrationAsyncInner() ? 0 : 1;

            async Task<bool> MigrationAsyncInner()
            {
                using(var childScope = this.scopeProvider.BeginLifetimeScope())
                {
                    var useCase = childScope.Resolve<IMigrationUseCase>();
                    try
                    {
                        var r = await useCase.ExecuteAsync();
                        if (r != 0)
                        {
                            throw new Exception("Failure migration");
                        }
                    }
                    catch (Exception ex)
                    {
                        this.logger.LogWarning(ex, "Error migration");
                        if (this.config.Timeout <= stopwatch.ElapsedMilliseconds * 1000)
                        {
                            return false;
                        }
                        this.logger.LogInformation("Wait 5 sec...");
                        await Task.Delay(5000);
                        this.logger.LogInformation("Retry migration");
                        return await MigrationAsyncInner();
                    }
                    return true;
                }
            }
        }
    }
}