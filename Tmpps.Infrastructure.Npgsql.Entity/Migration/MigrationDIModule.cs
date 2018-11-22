using Tmpps.Infrastructure.Common.Data.Migration.Interfaces;
using Tmpps.Infrastructure.Common.DependencyInjection.Builder.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Tmpps.Infrastructure.Npgsql.Entity.Migration
{
    public class MigrationDIModule : IDIModule
    {
        private string connectionString;

        public MigrationDIModule(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public void DefineModule(IDIBuilder builder)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MigrationDbContext>();
            if (!string.IsNullOrEmpty(this.connectionString))
            {
                optionsBuilder.UseNpgsql(this.connectionString);
            }
            builder.RegisterInstance(optionsBuilder.Options);
            builder.RegisterType<MigrationDbContext>(x => x.InstancePerLifetimeScope());
            builder.RegisterType<MigrationDbContextWrapper>(x => x.As<IMigrationDbContext>());
            builder.RegisterType<MigrationRepository>(x => x.As<IMigrationRepository<MigrationHistory>>());
            builder.RegisterType<MigrationHelper>(x => x.As<IMigrationHelper>());
        }
    }
}