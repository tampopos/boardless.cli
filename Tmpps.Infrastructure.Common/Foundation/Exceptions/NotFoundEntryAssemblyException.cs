using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tmpps.Infrastructure.Common.Foundation.Exceptions.Bases;

namespace Tmpps.Infrastructure.Common.Foundation.Exceptions
{
    /// <summary>
    /// システムアセンブリの取得例外
    /// </summary>
    public class NotFoundEntryAssemblyException : TmppsException
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public NotFoundEntryAssemblyException() : base("エントリーアセンブリが見つかりません。エントリーアセンブリを手動で登録してください。")
        {

        }
    }
}