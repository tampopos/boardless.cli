using System;
using System.Collections;
using System.Collections.Generic;

namespace Tmpps.Infrastructure.Common.Foundation.Interfaces
{
    /// <summary>
    /// ITypeHelper
    /// </summary>
    public interface ITypeHelper
    {
        Type GetType(Func<Type, bool> filter);
        IEnumerable<Type> GetTypes(Func<Type, bool> filter);
    }
}