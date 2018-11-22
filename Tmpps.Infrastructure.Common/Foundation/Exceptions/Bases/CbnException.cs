using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tmpps.Infrastructure.Common.Foundation.Exceptions.Bases
{
    /// <summary>
    /// TmppsException
    /// </summary>
    public abstract class TmppsException : Exception
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="msg"></param>
        public TmppsException(string msg) : base(msg)
        {

        }
    }
}