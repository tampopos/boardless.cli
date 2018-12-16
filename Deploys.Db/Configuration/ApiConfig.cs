using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Tmpps.Infrastructure.Data.Migration.Interfaces;

namespace Deploys.Db.Configuration
{
    public class ApiConfig : IMigrationConfig
    {
        private IConfigurationRoot configurationRoot;
        public ApiConfig(IConfigurationRoot configurationRoot)
        {
            this.configurationRoot = configurationRoot;
            this.Database = this.configurationRoot.GetValue<string>(nameof(this.Database));
            this.Path = this.configurationRoot.GetValue<string>("Directory");
            this.RootConnectionString = this.GetConnectionString("Root");
        }

        public string Database { get; }
        public string Path { get; }
        public string RootConnectionString { get; }
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