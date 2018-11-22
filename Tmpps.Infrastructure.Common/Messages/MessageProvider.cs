using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Tmpps.Infrastructure.Common.Messages.Interfaces;

namespace Tmpps.Infrastructure.Common.Messages
{
    /// <inheritDoc/>
    public class MessageProvider<TMessageSet> : IMessageProvider<TMessageSet>
    {
        private readonly TMessageSet messageSet;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="messageSet">メッセージ定義クラス</param>
        public MessageProvider(TMessageSet messageSet)
        {
            this.messageSet = messageSet;
        }

        /// <inheritDoc/>
        public string Get(Func<TMessageSet, string> selector, params object[] parameters)
        {
            return string.Format(selector(this.messageSet), parameters);
        }
    }
}