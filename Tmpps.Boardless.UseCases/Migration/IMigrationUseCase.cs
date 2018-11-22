using System.Threading.Tasks;

namespace Tmpps.Boardless.UseCases.Migration
{
    public interface IMigrationUseCase
    {
        Task<int> ExecuteAsync();
    }
}