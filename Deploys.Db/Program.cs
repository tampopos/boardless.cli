using Deploys.Db.Configuration;

namespace Deploys.Db
{
    class Program
    {
        static int Main(string[] args)
        {
            return new Startup(args).Execute();
        }
    }
}