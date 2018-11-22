using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Tmpps.Infrastructure.Common.Foundation.Extensions
{
    public static class AttributeExtensions
    {
        private static ConcurrentDictionary<MemberInfo, IEnumerable<Attribute>> attributeCache = new ConcurrentDictionary<MemberInfo, IEnumerable<Attribute>>();

        private static IEnumerable<Attribute> GetAttributes(this MemberInfo memberInfo)
        {
            return attributeCache.GetOrAdd(memberInfo, m =>
            {
                return m.GetCustomAttributes(true).Cast<Attribute>();
            });
        }
        /// <summary>
        /// 属性を取得する
        /// </summary>
        /// <typeparam name="TAttribute">属性の型</typeparam>
        /// <param name="memberInfo">属性を持つメンバー</param>
        /// <returns>属性</returns>
        public static TAttribute GetAttribute<TAttribute>(this MemberInfo memberInfo)
        where TAttribute : Attribute
        {
            return GetAttributes(memberInfo).FirstOrDefault(x => x is TAttribute) as TAttribute;
        }
        /// <summary>
        /// 属性から値を取得する
        /// </summary>
        /// <typeparam name="TAttribute">属性の型</typeparam>
        /// <typeparam name="TResult">値の型</typeparam>
        /// <param name="memberInfo">属性を持つメンバー</param>
        /// <param name="selector">取得する値の選択</param>
        /// <returns>値</returns>
        public static TResult GetAttributeValue<TAttribute, TResult>(this MemberInfo memberInfo, Func<TAttribute, TResult> selector)
        where TAttribute : Attribute
        {
            var attr = GetAttribute<TAttribute>(memberInfo);
            if (attr == null)
            {
                return default(TResult);
            }
            return selector(attr);
        }
    }
}