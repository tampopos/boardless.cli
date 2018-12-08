using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Tmpps.Infrastructure.Common.IO.Interfaces;
using Tmpps.Infrastructure.Data.Migration.Interfaces;

namespace Deploys.Db.Configuration
{
    public class Application
    {
        private CancellationTokenSource cancellationTokenSource;
        private CommandLineApplication application;
        private ILogger logger;
        private IMigrationConfig config;
        private IPathResolver pathResolver;
        private IMigrationUseCase migrationUseCase;

        public Application(
            CommandLineApplication application,
            CancellationTokenSource cancellationTokenSource,
            ILogger logger,
            IMigrationConfig config,
            IPathResolver pathResolver,
            IMigrationUseCase migrationUseCase)
        {
            this.application = application;
            this.cancellationTokenSource = cancellationTokenSource;
            this.logger = logger;
            this.config = config;
            this.pathResolver = pathResolver;
            this.migrationUseCase = migrationUseCase;
        }

        public int Execute(string[] args)
        {
            this.application.Name = "Boardless.Deploys.Db";
            this.application.Description = "Boardless Db の展開アプリ";
            this.application.HelpOption("-h|--help");
            this.application.OnExecute(async() =>
            {
                return await this.ExecuteOnErrorHandleAsync("migration", async() =>
                {
                    var dir = this.pathResolver.ResolveDirectoryPath(this.config.Path);
                    var path = Path.Combine(Directory.GetCurrentDirectory(), this.config.Path);
                    var fullpath = Path.GetFullPath(path);

                    return await this.migrationUseCase.ExecuteAsync();
                });
            });

            return this.application.Execute(args);
        }

        private async Task<int> ExecuteOnErrorHandleAsync(string commandName, Func<Task<int>> func)
        {
            try
            {
                this.logger?.LogInformation($"Start {commandName}");
                var res = await func();
                this.logger?.LogInformation($"End {commandName}");
                return res;
            }
            catch (OperationCanceledException ex)
            {
                this.logger?.LogWarning(ex.Message);
            }
            catch (Exception ex)
            {
                this.logger?.LogError(ex, $"Error {commandName}");
            }
            return 1;
        }
    }
}