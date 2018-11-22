using System;
using System.Collections.Generic;

namespace Tmpps.Infrastructure.Common.DependencyInjection.Builder
{
    public static class DIRegisterObjectFactory
    {
        public static DIRegisterTypeObject CreateTypeObject(
            Type registerType,
            IEnumerable<Type> aliasTypes = null,
            LifetimeType lifetimeType = LifetimeType.Dependency,
            IEnumerable<string> scopeTags = null)
        {
            return new DIRegisterTypeObject(registerType, aliasTypes, lifetimeType, scopeTags);
        }
        public static DIRegisterTypeObject<TRegister> CreateTypeObject<TRegister>(
            IEnumerable<Type> aliasTypes = null,
            LifetimeType lifetimeType = LifetimeType.Dependency,
            IEnumerable<string> scopeTags = null)
        {
            return new DIRegisterTypeObject<TRegister>(aliasTypes, lifetimeType, scopeTags);
        }
        public static DIRegisterInstanceObject<TRegister> CreateInstanceObject<TRegister>(
            TRegister instance,
            IEnumerable<Type> aliasTypes = null) where TRegister : class
        {
            return new DIRegisterInstanceObject<TRegister>(instance, aliasTypes);
        }
    }
}