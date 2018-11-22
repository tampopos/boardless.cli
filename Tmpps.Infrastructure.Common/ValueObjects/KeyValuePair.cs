using System.Collections.Generic;

namespace Tmpps.Infrastructure.Common.ValueObjects
{
    /// <summary>
    /// KeyValuePair
    /// </summary>
    public class KeyValuePair
    {
        /// <summary>コンストラクタ</summary>
        public KeyValuePair() { }
        /// <summary>コンストラクタ</summary>
        public KeyValuePair(string name) : this() => this.Name = name;
        /// <summary>コンストラクタ</summary>
        public KeyValuePair(string name, object value) : this(name) => this.Value = value;
        /// <summary>
        /// 名前
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 値
        /// </summary>
        /// <returns></returns>
        public object Value { get; set; }
    }
}