using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseCases;

namespace Deploys.Db.Controllers
{
    [AllowAnonymous]
    public class MigrationController : Controller
    {
        private IMigrationUseCase migrationUseCase;

        public MigrationController(IMigrationUseCase migrationUseCase)
        {
            this.migrationUseCase = migrationUseCase;
        }
        public async Task<string> State()
        {
            return (await this.migrationUseCase.GetStateAsync()).ToString();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<string> Index()
        {
            return (await this.migrationUseCase.ExecuteAsync()).ToString();
        }
    }
}