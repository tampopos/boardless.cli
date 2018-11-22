using System;
using System.Collections.Generic;
using System.Linq;

namespace Tmpps.Infrastructure.Common.DependencyInjection.Builder
{
    public abstract class DIRegisterObject
    {
        public DIRegisterObject(IEnumerable<Type> aliasTypes)
        {
            this.AliasTypes = aliasTypes?.ToList() ?? new List<Type>();
        }
        public IList<Type> AliasTypes { get; }
    }
    public static class DIRegisterObjectExtensions
    {
        public static T As<T>(this T obj, Type aliasType) where T : DIRegisterObject
        {
            obj.AliasTypes.Add(aliasType);
            return obj;
        }
    }
}