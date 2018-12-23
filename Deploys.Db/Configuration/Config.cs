using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Tmpps.Infrastructure.Data.Migration.Interfaces;
using UseCases.Interfaces;

namespace Deploys.Db.Configuration
{
    public class Config : IConfig
    {
        private IConfigurationRoot configurationRoot;
        public Config(IConfigurationRoot configurationRoot)
        {
            this.configurationRoot = configurationRoot;
            this.Database = this.configurationRoot.GetValue<string>(nameof(this.Database));
            this.Path = this.configurationRoot.GetValue<string>("Directory");
            this.RootConnectionString = this.GetConnectionString("Root");
            this.SkipInitialMigration = this.configurationRoot.GetValue<bool>(nameof(this.SkipInitialMigration));
            this.Timeout = this.configurationRoot.GetValue<int>(nameof(this.Timeout), 60);
        }

        public string Database { get; }
        public string Path { get; }
        public string RootConnectionString { get; }
        public bool SkipInitialMigration { get; }
        public int Timeout { get; }

        public string GetConnectionString(string name)
        {
            return this.configurationRoot.GetConnectionString(name);
        }
        public void CreateMvcConfigureRoutes(IRouteBuilder routes)
        {
            routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
        }
    }
}