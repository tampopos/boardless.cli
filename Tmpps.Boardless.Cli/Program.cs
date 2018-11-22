using System;
using System.Threading.Tasks;
using Tmpps.Boardless.Cli.Configuration;

namespace Tmpps.Boardless.Cli
{
    class Program
    {
        static int Main(string[] args)
        {
            return new Startup(args).Execute();
        }
    }
}