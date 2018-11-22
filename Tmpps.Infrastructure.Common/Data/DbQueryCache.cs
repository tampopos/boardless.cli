using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tmpps.Infrastructure.Common.Data.Configuration.Interfaces;
using Tmpps.Infrastructure.Common.Data.Interfaces;
using Tmpps.Infrastructure.Common.IO.Interfaces;

namespace Tmpps.Infrastructure.Common.Data
{
    public class DbQueryCache : IDbQueryCache
    {
        private IPathResolver pathResolver;
        private IDbConfig config;
        private Lazy<Dictionary<string, string>> cache;
        public DbQueryCache(IDbConfig config, IPathResolver pathResolver)
        {
            this.pathResolver = pathResolver;
            this.config = config;
            this.cache = new Lazy<Dictionary<string, string>>(this.CreateCache, true);
        }
        private Dictionary<string, string> CreateCache()
        {
            var dir = this.pathResolver.ResolveDirectoryPath(this.config.SqlPoolPath);
            if (dir == null)
            {
                return new Dictionary<string, string>();
            }
            return Directory.GetFiles(dir, "*.sql", SearchOption.AllDirectories).ToDictionary(x => Path.GetFileNameWithoutExtension(x), x => File.ReadAllText(x));
        }
        public string GetSqlById(string sqlId)
        {
            if (sqlId != null && this.cache.Value.TryGetValue(sqlId, out var sql))
            {
                return sql;
            }
            return null;
        }
    }
}