using System;

namespace Tmpps.Infrastructure.Common.Messages.Interfaces
{
    /// <summary>
    /// メッセージを提供するクラス
    /// </summary>
    public interface IMessageProvider<TMessageSet>
    {
        /// <summary>
        /// メッセージを取得
        /// </summary>
        /// <returns>メッセージ</returns>
        string Get(Func<TMessageSet, string> selector, params object[] parameters);
    }
}