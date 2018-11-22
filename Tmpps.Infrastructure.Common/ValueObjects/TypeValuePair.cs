using System;
using System.Collections.Generic;

namespace Tmpps.Infrastructure.Common.ValueObjects
{
    /// <summary>
    /// TypeValuePair
    /// </summary>
    public class TypeValuePair
    {
        /// <summary>コンストラクタ</summary>
        public TypeValuePair(object value) : this(value, value?.GetType()) { }
        /// <summary>コンストラクタ</summary>
        public TypeValuePair(object value, Type type)
        {
            this.Type = type;
            this.Value = value;
        }
        /// <summary>
        /// 型
        /// </summary>
        public Type Type { get; set; }
        /// <summary>
        /// 値
        /// </summary>
        /// <returns></returns>
        public object Value { get; set; }
    }
    /// <summary>
    /// TypeValuePair
    /// </summary>
    public class TypeValuePair<T> : TypeValuePair
    {
        /// <summary>コンストラクタ</summary>
        public TypeValuePair(T value) : base(value, typeof(T)) { }
    }

}