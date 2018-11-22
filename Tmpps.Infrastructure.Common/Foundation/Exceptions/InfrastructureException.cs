using System.Collections.Generic;
using Tmpps.Infrastructure.Common.Foundation.Exceptions.Bases;

namespace Tmpps.Infrastructure.Common.Foundation.Exceptions
{
    public class InfrastructureException : TmppsException
    {
        /// <summary>
        /// ビジネスロジック例外
        /// </summary>
        /// <param name="msg">エラー メッセージ</param>
        public InfrastructureException(string msg) : base(msg) { }
        /// <summary>
        /// ビジネスロジック例外
        /// </summary>
        /// <param name="msgs">エラー メッセージ</param>
        /// <param name="sep">セパレータ</param>
        public InfrastructureException(IEnumerable<string> msgs, string sep = ",") : base(string.Join(sep, msgs)) { }
    }
}