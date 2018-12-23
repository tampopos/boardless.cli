using Tmpps.Infrastructure.Data.Migration.Interfaces;

namespace UseCases.Interfaces
{
    public interface IConfig : IMigrationConfig
    {
        int Timeout { get; }
    }
}