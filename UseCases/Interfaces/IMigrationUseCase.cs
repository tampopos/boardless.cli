using System.Threading.Tasks;

namespace UseCases
{
    public interface IMigrationUseCase
    {
        Task<MigrationState> ExecuteAsync();
        Task<MigrationState> GetStateAsync();
    }
}