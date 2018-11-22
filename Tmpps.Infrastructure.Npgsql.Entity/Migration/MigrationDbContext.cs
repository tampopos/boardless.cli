using Tmpps.Infrastructure.Common.Data.Interfaces;
using Tmpps.Infrastructure.Common.Data.Migration.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Tmpps.Infrastructure.Npgsql.Entity.Migration
{
    public class MigrationDbContext : NpgsqlDbContext
    {
        public MigrationDbContext(DbContextOptions<MigrationDbContext> options) : base(options) { }
        public DbSet<MigrationHistory> MigrationHistories { get; set; }
    }
}