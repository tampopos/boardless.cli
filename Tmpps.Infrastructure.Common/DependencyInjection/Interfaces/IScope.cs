using System;
using Tmpps.Infrastructure.Common.ValueObjects;

namespace Tmpps.Infrastructure.Common.DependencyInjection.Interfaces
{
    /// <summary>
    /// Scope
    /// </summary>
    public interface IScope : IDisposable
    {
        /// <summary>
        /// オブジェクトの呼び出しを解決する
        /// </summary>
        object Resolve(Type type, params TypeValuePair[] parameters);
        /// <summary>
        /// オブジェクトの呼び出しを解決する
        /// </summary>
        T Resolve<T>(params TypeValuePair[] parameters);
        /// <summary>
        /// 子スコープを作成する
        /// </summary>
        IScope BeginLifetimeScope(params TypeValuePair[] inheritances);
        /// <summary>
        /// 子スコープを作成する
        /// </summary>
        IScope BeginLifetimeScope(object tag, params TypeValuePair[] inheritances);
    }
}