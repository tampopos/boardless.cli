using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Tmpps.Infrastructure.Common.Foundation.Extensions
{
    public static class TypeExtensions
    {
        private static ConcurrentDictionary<Type, Func<IEnumerable<object>, IEnumerable>> casterCache = new ConcurrentDictionary<Type, Func<IEnumerable<object>, IEnumerable>>();
        public static bool IsNullableType(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
        public static Type GetNullableTypeArguments(this Type type)
        {
            return IsNullableType(type) ? type.GenericTypeArguments.First() : null;
        }
        public static bool IsEnumerable(this Type type, params Type[] exclude)
        {
            if (exclude.Any(e => e == type))
            {
                return false;
            }
            return IsEnumerable(type) || type.GetInterfaces().Any(x => IsEnumerable(x));

            bool IsEnumerable(Type t)
            {
                return t == typeof(IEnumerable) || (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>));
            }
        }
        /// <summary>
        /// IEnumerableジェネリック型の格納された型の取得
        /// </summary>
        /// <param name="type">IEnumerableジェネリック型</param>
        /// <returns>格納された型</returns>
        public static Type GetEnumerableTypeArguments(this Type type)
        {
            if (!IsEnumerable(type))
            {
                return null;
            }
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                return type.GenericTypeArguments.First();
            }

            var t2 = type.GetInterfaces().FirstOrDefault(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>));
            if (t2 != null)
            {
                return t2.GenericTypeArguments.First();
            }

            return typeof(object);
        }
        /// <summary>
        /// 型変換を行う
        /// </summary>
        /// <remarks>
        /// IConvertibleな型のみ相互変換可能。Null許容型にも対応済み。
        /// </remarks>
        /// <param name="type">変換後の型</param>
        /// <param name="value">変換対象の値</param>
        /// <returns>objectにボクシングされた変換後の値</returns>
        public static object ChangeTypeNullable(this Type type, object value)
        {
            if (value == null)
            {
                return null;
            }
            if (IsNullableType(type))
            {
                type = GetNullableTypeArguments(type);
            }
            return Convert.ChangeType(value, type);
        }
        /// <summary>
        /// 型変換を行う
        /// </summary>
        /// <remarks>
        /// IConvertibleな型のみ相互変換可能。Null許容型にも対応済み。
        /// </remarks>
        /// <typeparam name="T">変換後の型</typeparam>
        /// <param name="value">変換対象の値</param>
        /// <returns>変換後の値</returns>
        public static T ChangeTypeNullable<T>(this IConvertible value)
        {
            return (T) ChangeTypeNullable(typeof(T), value);
        }
        /// <summary>
        /// 動的に取得した Type にCastする
        /// </summary>
        /// <param name="values">列挙体</param>
        /// <param name="type">Castする型</param>
        /// <returns>キャストされた列挙体</returns>
        public static IEnumerable Cast(this IEnumerable<object> values, Type type)
        {
            var caster = casterCache.GetOrAdd(type, p =>
            {
                var paramExp = Expression.Parameter(typeof(IEnumerable<object>), nameof(values));
                var method = typeof(Enumerable).GetMethod(nameof(Enumerable.Cast));
                return Expression.Lambda<Func<IEnumerable<object>, IEnumerable>>(
                    Expression.Convert(
                        Expression.Call(typeof(Enumerable), nameof(Enumerable.Cast), new [] { type }, paramExp),
                        typeof(IEnumerable)
                    ), paramExp).Compile();
            });
            return caster(values);
        }
    }
}