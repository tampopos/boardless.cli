using System;
using System.Collections.Generic;

namespace Tmpps.Infrastructure.Common.DependencyInjection.Builder.Interfaces
{
    public interface IDIModule
    {
        void DefineModule(IDIBuilder builder);
    }
}