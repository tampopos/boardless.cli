using Autofac;
using Tmpps.Infrastructure.Common.ValueObjects;

namespace Tmpps.Infrastructure.Autofac.Extensions
{
    public static class TypeValuePairExtensions
    {
        public static TypedParameter Convert(this TypeValuePair pair) => new TypedParameter(pair.Type, pair.Value);
    }
}