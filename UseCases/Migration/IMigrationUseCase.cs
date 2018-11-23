using System.Threading.Tasks;

namespace UseCases.Migration
{
    public interface IMigrationUseCase
    {
        Task<int> ExecuteAsync();
    }
}