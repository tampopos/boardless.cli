using Microsoft.Extensions.Configuration;
using Tmpps.Infrastructure.Data.Configuration.Interfaces;
using Tmpps.Infrastructure.Data.Migration.Interfaces;
using Tmpps.Infrastructure.JsonWebToken.Interfaces;

namespace Cli.Configuration
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
            this.MaxConcurrencyReceive = this.configurationRoot.GetValue<int>(nameof(this.MaxConcurrencyReceive));
        }

        public string SqlPoolPath { get; }
        public string JwtSecret { get; }
        public int JwtExpiresDate { get; }
        public string JwtAudience { get; }
        public string JwtIssuer { get; }
        public string Database { get; }
        public string AdminConnectionString { get; }

        public int MaxConcurrencyReceive { get; }

        public string GetConnectionString(string name)
        {
            return this.configurationRoot.GetConnectionString(name);
        }
    }
}