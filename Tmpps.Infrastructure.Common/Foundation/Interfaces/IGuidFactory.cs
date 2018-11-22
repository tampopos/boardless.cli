using System;

namespace Tmpps.Infrastructure.Common.Foundation.Interfaces
{
    /// <summary>
    /// Guidのfactory
    /// </summary>
    public interface IGuidFactory
    {
        /// <summary>
        /// Guidを生成する
        /// </summary>
        /// <returns>Guid</returns>
        Guid CreateNew();
    }
}