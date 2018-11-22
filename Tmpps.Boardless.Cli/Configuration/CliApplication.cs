using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Tmpps.Boardless.UseCases.Migration;

namespace Tmpps.Boardless.Cli.Configuration
{
    public class CliApplication
    {
        private CancellationTokenSource cancellationTokenSource;
        private CommandLineApplication application;
        private ILogger logger;
        private IMigrationUseCase migrationService;

        public CliApplication(
            CommandLineApplication application,
            CancellationTokenSource cancellationTokenSource,
            ILogger logger,
            IMigrationUseCase migrationService)
        {
            this.application = application;
            this.cancellationTokenSource = cancellationTokenSource;
            this.logger = logger;
            this.migrationService = migrationService;
        }

        public int Execute(string[] args)
        {
            this.application.Name = nameof(Boardless);
            this.application.Description = "CliSampleプロジェクト";
            this.application.HelpOption("-h|--help");

            this.application.Command("migration", command =>
            {
                command.Description = "database migration を行います。";
                command.HelpOption("-h|--help");

                command.OnExecute(async() =>
                {
                    return await this.ExecuteOnErrorHandleAsync("migration", async() =>
                    {
                        return await this.migrationService.ExecuteAsync();
                    });
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