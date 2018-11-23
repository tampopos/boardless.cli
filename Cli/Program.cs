using System;
using System.Threading.Tasks;
using Cli.Configuration;

namespace Cli
{
    class Program
    {
        static int Main(string[] args)
        {
            return new Startup(args).Execute();
        }
    }
}