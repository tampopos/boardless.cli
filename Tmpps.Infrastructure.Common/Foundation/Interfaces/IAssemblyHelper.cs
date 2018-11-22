using System.Reflection;

namespace Tmpps.Infrastructure.Common.Foundation.Interfaces
{
    /// <summary>
    /// IAssemblyHelper
    /// </summary>
    public interface IAssemblyHelper
    {
        /// <summary>
        /// ExecuteAssemblyを取得する
        /// </summary>
        /// <returns>ExecuteAssembly</returns>
        Assembly GetExecuteAssembly();
    }
}