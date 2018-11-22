using System.Data;
using Tmpps.Infrastructure.Common.Data;
using Tmpps.Infrastructure.Common.Data.Interfaces;
using Npgsql;

namespace Tmpps.Infrastructure.Npgsql.Entity
{
    public class NpgsqlDbQuery : DbQuery
    {
        public NpgsqlDbQuery(string sql = null) : base(sql) { }

        public override IDataParameter CreateParameter(string name, object value)
        {
            return new NpgsqlParameter(name, value);
        }
    }
}