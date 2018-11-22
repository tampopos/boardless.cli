using Tmpps.Infrastructure.Common.Configuration;
using Tmpps.Infrastructure.Common.Configuration.Interfaces;
using Tmpps.Infrastructure.Common.Cryptography;
using Tmpps.Infrastructure.Common.Cryptography.Interfaces;
using Tmpps.Infrastructure.Common.DependencyInjection.Builder.Interfaces;
using Tmpps.Infrastructure.Common.Foundation;
using Tmpps.Infrastructure.Common.Foundation.Interfaces;
using Tmpps.Infrastructure.Common.IO;
using Tmpps.Infrastructure.Common.IO.Interfaces;
using Tmpps.Infrastructure.Common.Logging.Extensions;
using Microsoft.Extensions.Logging;

namespace Tmpps.Infrastructure.Autofac.Configuration
{
    public class CommonDIModule : IDIModule
    {
        private System.Reflection.Assembly executeAssembly;
        private string rootPath;
        private ILoggerFactory loggerFactory;

        public CommonDIModule(System.Reflection.Assembly executeAssembly, string rootPath, ILoggerFactory loggerFactory)
        {
            this.executeAssembly = executeAssembly;
            this.rootPath = rootPath;
            this.loggerFactory = loggerFactory;
        }

        public void DefineModule(IDIBuilder builder)
        {
            builder.RegisterInstance(this.loggerFactory, x => x.As<ILoggerFactory>());
            builder.RegisterInstance(this.loggerFactory.GetDefaultLogger(), x => x.As<ILogger>());
            builder.RegisterInstance(new AssemblyHelper(this.executeAssembly), x => x.As<IAssemblyHelper>());
            builder.RegisterType<GuidFactory>(x => x.As<IGuidFactory>().SingleInstance());
            builder.RegisterType<Mapper>(x => x.As<IMapper>().SingleInstance());
            builder.RegisterType<SystemClock>(x => x.As<ISystemClock>().SingleInstance());
            builder.RegisterType<TypeHelper>(x => x.As<ITypeHelper>().SingleInstance());
            builder.RegisterInstance(new PathResolver(this.rootPath), x => x.As<IPathResolver>());
            builder.RegisterType<MD5HashComputer>(x => x.As<IHashComputer>().SingleInstance());

            // 登録されてない型もコンテナで作成する
            builder.CanResolveNotAlreadyRegisteredSource = true;
        }
    }
}