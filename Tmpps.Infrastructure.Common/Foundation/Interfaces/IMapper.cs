using System;
using System.Collections;
using System.Collections.Generic;

namespace Tmpps.Infrastructure.Common.Foundation.Interfaces
{
    /// <summary>
    /// IMapper
    /// </summary>
    public interface IMapper
    {
        /// <summary>
        /// プロパティ値をコピーする
        /// </summary>
        /// <typeparam name="TResult">戻り値の型</typeparam>
        /// <param name="source">コピー元のオブジェクト</param>
        /// <param name="generator">戻り値のインスタンスの生成方法を指定する。デフォルトではActivator.CreateInstanceを行う</param>
        /// <returns>生成したオブジェクト</returns>
        TResult Map<TResult>(object source, Func<TResult> generator = null) where TResult : class;
        /// <summary>
        /// リストのプロパティ値をコピーする
        /// </summary>
        /// <typeparam name="TResult">列挙体の格納型</typeparam>
        /// <param name="source">コピー元のオブジェクト</param>
        /// <param name="generator">戻り値のインスタンスの生成方法を指定する。デフォルトではActivator.CreateInstanceを行う</param>
        /// <returns>生成したオブジェクトの列挙体</returns>
        IEnumerable<TResult> Map<TResult>(IEnumerable source, Func<TResult> generator = null) where TResult : class;
        /// <summary>
        /// プロパティ値をコピーする
        /// </summary>
        /// <typeparam name="TDestination">コピー先のオブジェクトの型</typeparam>
        /// <param name="source">コピー元のオブジェクト</param>
        /// <param name="destination">コピー先のオブジェクト</param>
        /// <returns>コピー先のオブジェクト</returns>
        TDestination Map<TDestination>(object source, TDestination destination) where TDestination : class;
    }
}