using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Tmpps.Infrastructure.Common.Foundation.Extensions;
using Tmpps.Infrastructure.Common.Foundation.Interfaces;
using Tmpps.Infrastructure.Common.ValueObjects;

namespace Tmpps.Infrastructure.Common.Foundation
{
    /// <summary>
    /// Mapper
    /// </summary>
    public class Mapper : IMapper
    {
        private ConcurrentDictionary<MapKey, Action<object, object>> cache = new ConcurrentDictionary<MapKey, Action<object, object>>();
        private Dictionary<MapKey, Action<object, object>> map = new Dictionary<MapKey, Action<object, object>>();
        private IMapRegister mapRegister;

        public Mapper(IMapRegister mapRegister = null)
        {
            this.mapRegister = mapRegister;
            this.map = mapRegister?.CreateMap();
        }
        /// <summary>
        /// プロパティ値をコピーする
        /// </summary>
        /// <typeparam name="TResult">戻り値の型</typeparam>
        /// <param name="source">コピー元のオブジェクト</param>
        /// <param name="generator">戻り値のインスタンスの生成方法を指定する。デフォルトではActivator.CreateInstanceを行う</param>
        /// <returns>生成したオブジェクト</returns>
        public TResult Map<TResult>(object source, Func<TResult> generator = null) where TResult : class
        {
            if (source == null)
            {
                return default(TResult);
            }
            var result = generator != null ? generator() : Activator.CreateInstance<TResult>();
            return Map(source, result);
        }
        /// <summary>
        /// リストのプロパティ値をコピーする
        /// </summary>
        /// <typeparam name="TResult">列挙体の格納型</typeparam>
        /// <param name="source">コピー元のオブジェクト</param>
        /// <param name="generator">戻り値のインスタンスの生成方法を指定する。デフォルトではActivator.CreateInstanceを行う</param>
        /// <returns>生成したオブジェクトの列挙体</returns>
        public IEnumerable<TResult> Map<TResult>(IEnumerable source, Func<TResult> generator = null) where TResult : class
        {
            foreach (var item in source)
            {
                if (item != null)
                {
                    yield return Map(item, generator);
                }
            }
        }
        /// <summary>
        /// プロパティ値をコピーする
        /// </summary>
        /// <typeparam name="TDestination">コピー先のオブジェクトの型</typeparam>
        /// <param name="source">コピー元のオブジェクト</param>
        /// <param name="destination">コピー先のオブジェクト</param>
        /// <returns>コピー先のオブジェクト</returns>
        public TDestination Map<TDestination>(object source, TDestination destination) where TDestination : class
        {
            if (source == null || destination == null)
            {
                return destination;
            }
            var sType = source.GetType();
            var convert = cache.GetOrAdd(new MapKey(sType, typeof(TDestination)), key =>
            {
                if (this.map == null)
                {
                    return (s, d) => this.MapDefault<TDestination>(s, d as TDestination);
                }
                if (this.map.TryGetValue(key, out var action))
                {
                    return action;
                }
                return this.map
                    .Where(x => x.Key.IsAssignableFrom(key))
                    .Select(x => x.Value)
                    .FirstOrDefault() ?? new Action<object, object>((s, d) => this.MapDefault<TDestination>(s, d as TDestination));
            });
            convert(source, destination);
            return destination;
        }

        private TDestination MapDefault<TDestination>(object source, TDestination destination) where TDestination : class
        {
            var sProps = source.GetType().GetProperties();
            var dProps = typeof(TDestination).GetProperties();
            foreach (var sProp in sProps)
            {
                var dProp = dProps.SingleOrDefault(x => x.Name.ToLower() == sProp.Name.ToLower());
                if (dProp == null || !dProp.PropertyType.IsAssignableFrom(sProp.PropertyType))
                {
                    continue;
                }
                var sVal = source.Get(sProp);
                destination.Set(dProp, sVal);
            }
            return destination;
        }
    }
}