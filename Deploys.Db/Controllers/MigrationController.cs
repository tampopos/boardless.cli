using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tmpps.Infrastructure.Data.Migration.Interfaces;

namespace Deploys.Db.Controllers
{
    public class MigrationController : Controller
    {
        private IMigrationUseCase migrationUseCase;

        public MigrationController(IMigrationUseCase migrationUseCase)
        {
            this.migrationUseCase = migrationUseCase;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<int> Index()
        {
            return await this.migrationUseCase.ExecuteAsync();
        }
    }
}