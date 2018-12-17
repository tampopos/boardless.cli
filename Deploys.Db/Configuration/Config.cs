using Microsoft.Extensions.Configuration;
using Tmpps.Infrastructure.Data.Migration.Interfaces;

namespace Deploys.Db.Configuration
{
    public class Config : IMigrationConfig
    {
        private IConfigurationRoot configurationRoot;
        public Config(IConfigurationRoot configurationRoot)
        {
            this.configurationRoot = configurationRoot;
            this.Database = this.configurationRoot.GetValue<string>(nameof(this.Database));
            this.Path = this.configurationRoot.GetValue<string>("Directory");
            this.RootConnectionString = this.GetConnectionString("Root");
            this.Timeout = this.configurationRoot.GetValue<int>(nameof(this.Timeout), 60);
        }

        public string Database { get; }
        public string Path { get; }
        public string RootConnectionString { get; }
        public bool InitialMigration { get; }
        public int Timeout { get; }

        public string GetConnectionString(string name)
        {
            return this.configurationRoot.GetConnectionString(name);
        }
    }
}