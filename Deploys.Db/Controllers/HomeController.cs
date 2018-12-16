using Microsoft.AspNetCore.Mvc;

namespace Deploys.Db.Controllers
{
    public class HomeController : Controller
    {
        public string Index()
        {
            return "deploys.db";
        }
    }
}