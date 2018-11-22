using Microsoft.Extensions.Configuration;
using Tmpps.Infrastructure.Common.Claims.Interfaces;
using Tmpps.Infrastructure.Common.Data.Configuration.Interfaces;
using Tmpps.Infrastructure.Common.Data.Migration.Interfaces;

namespace Tmpps.Boardless.Cli.Configuration
{
    public class CliConfig : IDbConfig, IJwtConfig, IMigrationConfig
    {
        private IConfigurationRoot configurationRoot;
        public CliConfig(IConfigurationRoot configurationRoot)
        {
            this.configurationRoot = configurationRoot;
            this.SqlPoolPath = this.configurationRoot.GetValue<string>(nameof(this.SqlPoolPath));
            this.JwtSecret = this.configurationRoot.GetValue<string>(nameof(this.JwtSecret));
            this.JwtExpiresDate = this.configurationRoot.GetValue<int>(nameof(this.JwtExpiresDate));
            this.JwtAudience = this.configurationRoot.GetValue<string>(nameof(this.JwtAudience));
            this.JwtIssuer = this.configurationRoot.GetValue<string>(nameof(this.JwtIssuer));
            this.Database = this.configurationRoot.GetValue<string>(nameof(this.Database));
            this.AdminConnectionString = this.GetConnectionString("AdminConnection");
            this.AwsAccessKeyId = this.configurationRoot.GetValue<string>(nameof(this.AwsAccessKeyId));
            this.AwsSecretAccessKey = this.configurationRoot.GetValue<string>(nameof(this.AwsSecretAccessKey));
            this.ServiceURL = this.configurationRoot.GetValue<string>(nameof(this.ServiceURL));
            this.MaxConcurrencyReceive = this.configurationRoot.GetValue<int>(nameof(this.MaxConcurrencyReceive));
        }

        public string SqlPoolPath { get; }
        public string JwtSecret { get; }
        public int JwtExpiresDate { get; }
        public string JwtAudience { get; }
        public string JwtIssuer { get; }
        public string Database { get; }
        public string AdminConnectionString { get; }
        public string AwsAccessKeyId { get; }
        public string AwsSecretAccessKey { get; }
        public string ServiceURL { get; }

        public int MaxConcurrencyReceive { get; }

        public string GetConnectionString(string name)
        {
            return this.configurationRoot.GetConnectionString(name);
        }
    }
}