using System.Collections.Generic;
using Tmpps.Infrastructure.Common.Foundation.Exceptions.Bases;

namespace Tmpps.Infrastructure.Common.Foundation.Exceptions
{
    /// <summary>
    /// ビジネスロジック例外
    /// </summary>
    public class BizLogicException : TmppsException
    {
        /// <summary>
        /// ビジネスロジック例外
        /// </summary>
        /// <param name="msg">エラー メッセージ</param>
        public BizLogicException(string msg) : base(msg) { }
        /// <summary>
        /// ビジネスロジック例外
        /// </summary>
        /// <param name="msgs">エラー メッセージ</param>
        /// <param name="sep">セパレータ</param>
        public BizLogicException(IEnumerable<string> msgs, string sep = ",") : base(string.Join(sep, msgs)) { }
    }
}