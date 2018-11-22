using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tmpps.Infrastructure.Common.ValueObjects
{
    /// <summary>
    /// paging list
    /// </summary>
    public class PagedList<T>
    {
        /// <summary>
        /// トータル レコード数
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// データ
        /// </summary>
        public IEnumerable<T> Data { get; set; }
    }
}