using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Tmpps.Infrastructure.Common.Foundation.Interfaces;

namespace Tmpps.Infrastructure.Common.Foundation
{
    /// <summary>
    /// TypeHelper
    /// </summary>
    public class TypeHelper : ITypeHelper
    {
        private readonly IAssemblyHelper assemblyHelper;
        private Lazy<Type[]> typeCache;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TypeHelper(IAssemblyHelper assemblyHelper)
        {
            this.assemblyHelper = assemblyHelper;
            this.typeCache = new Lazy<Type[]>(() => GetAllTypeArray(), true);
        }
        /// <summary>
        /// SystemAssemblyから辿ることのできる型情報を取得する
        /// </summary>
        /// <param name="filter">型フィルター</param>
        /// <returns>型情報</returns>
        public Type GetType(Func<Type, bool> filter)
        {
            return typeCache.Value.FirstOrDefault(filter);
        }

        /// <summary>
        /// SystemAssemblyから辿ることのできる型情報を取得する
        /// </summary>
        /// <param name="filter">型フィルター</param>
        /// <returns>型情報</returns>
        public IEnumerable<Type> GetTypes(Func<Type, bool> filter)
        {
            return typeCache.Value.Where(filter);
        }

        private Type[] GetAllTypeArray()
        {
            var hashSet = new HashSet<string>();
            return GetAllTypes(this.assemblyHelper.GetExecuteAssembly()).Distinct().ToArray();

            IEnumerable<Type> GetAllTypes(Assembly assembly)
            {
                foreach (var type in assembly.GetTypes())
                {
                    yield return type;
                }
                var referencedAssemblies = assembly.GetReferencedAssemblies().Where(x => !hashSet.Contains(x.FullName));
                foreach (var referencedAssembly in referencedAssemblies)
                {
                    hashSet.Add(referencedAssembly.FullName);
                    foreach (var type in GetAllTypes(Assembly.Load(referencedAssembly)))
                    {
                        yield return type;
                    }
                }
            }
        }
    }
}