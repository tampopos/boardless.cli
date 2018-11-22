using System;
using System.Collections.Generic;
using Tmpps.Infrastructure.Common.ValueObjects;

namespace Tmpps.Infrastructure.Common.Foundation.Interfaces
{
    public interface IMapRegister
    {
        Dictionary<MapKey, Action<object, object>> CreateMap();
        void Register<TSource, TDestination>(Action<TSource, TDestination> action)
        where TSource : class
        where TDestination : class;
        void RegisterDefinition<TSource, TDestination>(IMapDefinition<TSource, TDestination> mapDefinition)
        where TSource : class
        where TDestination : class;
    }
}