using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Tmpps.Infrastructure.Common.Foundation.Extensions
{
    public static class MemberExtensions
    {
        private static ConcurrentDictionary<MemberInfo, Func<object, object>> getterCache = new ConcurrentDictionary<MemberInfo, Func<object, object>>();
        private static ConcurrentDictionary<MemberInfo, Action<object, object>> setterCache = new ConcurrentDictionary<MemberInfo, Action<object, object>>();

        public static PropertyInfo GetProperty<T, TProperty>(this T obj, Expression<Func<T, TProperty>> expression)
        {
            return typeof(T).GetProperty(((MemberExpression) expression.Body).Member.Name);
        }
        public static TResult Get<T, TResult>(this T obj, string propertyOrFieldName)
        {
            var memberInfo = GetMemberInfo(typeof(T), propertyOrFieldName);
            if (memberInfo is PropertyInfo pInfo)
            {
                return obj.Get<TResult>(pInfo);
            }
            else if (memberInfo is FieldInfo fInfo)
            {
                return obj.Get<TResult>(fInfo);
            }
            return default(TResult);
        }
        public static TResult Get<TResult>(this object obj, PropertyInfo propertyInfo)
        {
            return (TResult) Get(obj, propertyInfo);
        }
        public static TResult Get<TResult>(this object obj, FieldInfo fieldInfo)
        {
            return (TResult) Get(obj, fieldInfo);
        }
        /// <summary>
        /// 指定したプロパティから値を取得する
        /// </summary>
        public static object Get(this object obj, PropertyInfo propertyInfo)
        {
            var getter = getterCache.GetOrAdd(propertyInfo, p =>
            {
                return propertyInfo.CreateGetExpression(obj);
            });
            return getter(obj);
        }
        /// <summary>
        /// 指定したフィールドから値を取得する
        /// </summary>
        public static object Get(this object obj, FieldInfo fieldInfo)
        {
            var getter = getterCache.GetOrAdd(fieldInfo, p =>
            {
                return fieldInfo.CreateGetExpression(obj);
            });
            return getter(obj);
        }

        private static MemberInfo GetMemberInfo(this Type type, string propertyOrFieldName)
        {
            return type.GetProperty(propertyOrFieldName) as MemberInfo ?? type.GetField(propertyOrFieldName);
        }

        /// <summary>
        /// 指定したプロパティまたはフィールドに値を設定する
        /// </summary>
        public static void Set<T>(this T obj, string propertyOrFieldName, object value)
        {
            var memberInfo = GetMemberInfo(typeof(T), propertyOrFieldName);
            switch (memberInfo)
            {
                case PropertyInfo propertyInfo:
                    Set(obj, propertyInfo, value);
                    break;
                case FieldInfo fieldInfo:
                    Set(obj, fieldInfo, value);
                    break;
            }
        }
        /// <summary>
        /// 指定したプロパティに値を設定する
        /// </summary>
        public static void Set(this object obj, PropertyInfo pInfo, object value)
        {
            var setter = setterCache.GetOrAdd(pInfo, p =>
            {
                return pInfo.CreateSetExpression(obj, value);
            });
            setter(obj, value);
        }
        /// <summary>
        /// 指定したフィールドに値を設定する
        /// </summary>
        public static void Set(this object obj, FieldInfo fieldInfo, object value)
        {
            var setter = setterCache.GetOrAdd(fieldInfo, p =>
            {
                return fieldInfo.CreateSetExpression(obj, value);
            });
            setter(obj, value);
        }

        private static Func<object, object> CreateGetExpression(this PropertyInfo propertyInfo, object obj)
        {
            var objExp = Expression.Parameter(typeof(object), nameof(obj));
            var getterExp = Expression.Property(Expression.Convert(objExp, propertyInfo.DeclaringType), propertyInfo);
            return Expression.Lambda<Func<object, object>>(Expression.Convert(getterExp, typeof(object)), objExp).Compile();
        }
        private static Func<object, object> CreateGetExpression(this FieldInfo fieldInfo, object obj)
        {
            var objExp = Expression.Parameter(typeof(object), nameof(obj));
            var getterExp = Expression.Field(Expression.Convert(objExp, fieldInfo.DeclaringType), fieldInfo);
            return Expression.Lambda<Func<object, object>>(Expression.Convert(getterExp, typeof(object)), objExp).Compile();
        }
        private static Action<object, object> CreateSetExpression(this PropertyInfo pInfo, object obj, object value)
        {
            var objExp = Expression.Parameter(typeof(object), nameof(obj));
            var setterExp = Expression.Property(Expression.Convert(objExp, pInfo.DeclaringType), pInfo);
            var valueExp = Expression.Parameter(typeof(object), nameof(value));
            return Expression.Lambda<Action<object, object>>(Expression.Assign(setterExp, Expression.Convert(valueExp, pInfo.PropertyType)), objExp, valueExp).Compile();
        }
        private static Action<object, object> CreateSetExpression(this FieldInfo fInfo, object obj, object value)
        {
            var objExp = Expression.Parameter(typeof(object), nameof(obj));
            var setterExp = Expression.Field(Expression.Convert(objExp, fInfo.DeclaringType), fInfo);
            var valueExp = Expression.Parameter(typeof(object), nameof(value));
            return Expression.Lambda<Action<object, object>>(Expression.Assign(setterExp, Expression.Convert(valueExp, fInfo.FieldType)), objExp, valueExp).Compile();
        }
    }
}