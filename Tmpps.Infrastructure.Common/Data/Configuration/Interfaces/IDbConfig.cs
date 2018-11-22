namespace Tmpps.Infrastructure.Common.Data.Configuration.Interfaces
{
    public interface IDbConfig
    {
        string SqlPoolPath { get; }
        string GetConnectionString(string name);
    }
}