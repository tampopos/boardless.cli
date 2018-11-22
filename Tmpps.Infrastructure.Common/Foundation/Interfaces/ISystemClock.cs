using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tmpps.Infrastructure.Common.Foundation.Interfaces
{
    /// <summary>
    /// 時間取得のためのインターフェイス
    /// </summary>
    public interface ISystemClock
    {
        /// <summary>
        /// Now
        /// </summary>
        /// <returns>DateTime</returns>
        DateTime Now { get; }
        /// <summary>
        /// OffsetNow
        /// </summary>
        /// <returns>DateTimeOffset</returns>
        DateTimeOffset OffsetNow { get; }
    }
}