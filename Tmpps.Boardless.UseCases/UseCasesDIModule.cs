using Tmpps.Boardless.UseCases.Migration;
using Tmpps.Infrastructure.Common.DependencyInjection.Builder.Interfaces;

namespace Tmpps.Boardless.UseCases
{
    public class UseCasesDIModule : IDIModule
    {
        public void DefineModule(IDIBuilder builder)
        {
            builder.RegisterType<MigrationUseCase>(x => x.As<IMigrationUseCase>());
        }
    }
}