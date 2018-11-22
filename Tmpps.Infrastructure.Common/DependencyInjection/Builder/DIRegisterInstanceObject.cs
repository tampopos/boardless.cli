using System;
using System.Collections.Generic;

namespace Tmpps.Infrastructure.Common.DependencyInjection.Builder
{
    public class DIRegisterInstanceObject<TRegister> : DIRegisterObject
    where TRegister : class
    {
        public DIRegisterInstanceObject(
            TRegister instance,
            IEnumerable<Type> aliasTypes = null) : base(aliasTypes)
        {
            this.Instance = instance;
        }
        public TRegister Instance { get; }
        public DIRegisterInstanceObject<TRegister> As(Type aliasType)
        {
            this.AliasTypes.Add(aliasType);
            return this;
        }
        public DIRegisterInstanceObject<TRegister> As<TAlias>() => this.As(typeof(TAlias));
    }
}