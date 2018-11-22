using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Tmpps.Infrastructure.Common.Data.Interfaces;

namespace Tmpps.Infrastructure.Common.Data
{
    /// <summary>
    /// Sql構築クラス
    /// </summary>
    public abstract class DbQuery : IDbQuery
    {
        #region constructor
        /// <summary>
        /// Sql構築クラス
        /// </summary>
        /// <param name="sql">sql</param>
        public DbQuery(string sql = null)
        {
            this.Sql = new StringBuilder(sql ?? string.Empty);
        }
        #endregion

        #region props
        private StringBuilder Sql { get; }
        private StringBuilder WhereSql { get; } = new StringBuilder();
        private StringBuilder OrderSql { get; } = new StringBuilder();
        private string OffsetSql { get; set; }
        private List<IDataParameter> Parameters { get; } = new List<IDataParameter>();
        /// <summary>
        /// Where句の置換用文字列
        /// 初期値は/*where*/
        /// </summary>
        public string WhereReplacer { get; set; } = "/*where*/";
        /// <summary>
        /// OrderBy句の置換用文字列
        /// 初期値は/*order*/
        /// </summary>
        public string OrderByReplacer { get; set; } = "/*order*/";
        /// <summary>
        /// Offset句の置換用文字列
        /// </summary>
        public string OffsetReplacer { get; set; }
        #endregion

        #region publics
        /// <summary>
        /// クエリ実行時のSql変数を追加する
        /// </summary>
        /// <param name="parameters">変数</param>
        /// <returns>IDbQuery</returns>
        public IDbQuery AddParameters(params(string name, object value) [] parameters)
        {
            this.Parameters.AddRange(parameters.Select(x => this.CreateParameter(x.name, x.value)));
            return this;
        }
        /// <summary>
        /// クエリ実行時のSql変数を追加する
        /// </summary>
        /// <param name="parameters">変数</param>
        /// <returns>IDbQuery</returns>
        public IDbQuery AddParameters(params IDataParameter[] parameters)
        {
            this.Parameters.AddRange(parameters);
            return this;
        }
        /// <summary>
        /// 末尾にSqlを追加する
        /// </summary>
        /// <param name="sql">Sql</param>
        /// <param name="parameters">変数</param>
        /// <returns>IDbQuery</returns>
        public IDbQuery Append(string sql, params IDataParameter[] parameters)
        {
            this.Sql.AppendLine(sql);
            return this.AddParameters(parameters);
        }
        /// <summary>
        /// Offset句に値を設定する
        /// </summary>
        /// <param name="offset">スキップする件数</param>
        /// <param name="fetch">取得する件数</param>
        /// <returns>IDbQuery</returns>
        public virtual IDbQuery SetToOffset(int offset, int? fetch = null)
        {
            this.AddParameters(this.CreateParameter(nameof(offset), offset));
            var q = new StringBuilder(" offset @{} rows ");
            if (fetch.HasValue)
            {
                this.AddParameters(this.CreateParameter(nameof(fetch), fetch));
                q.Append(" fetch next @{next} rows only ");
            }
            this.OffsetSql = q.ToString();
            return this;
        }
        /// <summary>
        /// OrderBy句に値を追加する
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="parameters">変数</param>
        /// <returns>IDbQuery</returns>
        public IDbQuery AppendToOrderBy(string condition, params IDataParameter[] parameters)
        {
            this.OrderSql.AppendLine(condition);
            return this.AddParameters(parameters);
        }
        /// <summary>
        /// OrderBy句に値を追加する
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="parameters">変数</param>
        /// <returns>IDbQuery</returns>
        public IDbQuery AppendToOrderBy(string condition, params(string name, object value) [] parameters)
        {
            this.OrderSql.AppendLine(condition);
            return this.AddParameters(parameters);
        }
        /// <summary>
        /// Where句に値を追加する
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="parameters">変数</param>
        /// <returns>IDbQuery</returns>
        public IDbQuery AppendToWhere(string condition, params IDataParameter[] parameters)
        {
            this.WhereSql.AppendLine(condition);
            return this.AddParameters(parameters);
        }
        /// <summary>
        /// Where句に値を追加する
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="parameters">変数</param>
        /// <returns>IDbQuery</returns>
        public IDbQuery AppendToWhere(string condition, params(string name, object value) [] parameters)
        {
            this.WhereSql.AppendLine(condition);
            return this.AddParameters(parameters);
        }
        /// <summary>
        /// Sql変数を生成する
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="value">値</param>
        /// <returns>Sql変数</returns>
        public abstract IDataParameter CreateParameter(string name, object value);
        /// <summary>
        /// 設定されているパラメータを取得する
        /// </summary>
        /// <returns>パラメータ</returns>
        public IEnumerable<IDataParameter> GetParameters()
        {
            return this.Parameters;
        }
        /// <summary>
        /// Sqlを置換する
        /// </summary>
        /// <param name="oldValue">oldValue</param>
        /// <param name="newValue">newValue</param>
        /// <param name="parameters">変数</param>
        /// <returns>IDbQuery</returns>
        public IDbQuery Replace(string oldValue, string newValue, params IDataParameter[] parameters)
        {
            this.Sql.Replace(oldValue, newValue);
            return this.AddParameters(parameters);
        }
        /// <summary>
        /// Sqlを置換する
        /// </summary>
        /// <param name="oldValue">oldValue</param>
        /// <param name="newValue">newValue</param>
        /// <param name="parameters">変数</param>
        /// <returns>IDbQuery</returns>
        public IDbQuery Replace(string oldValue, string newValue, params(string name, object value) [] parameters)
        {
            this.Sql.Replace(oldValue, newValue);
            return this.AddParameters(parameters);
        }
        /// <summary>
        /// Sqlを取得する
        /// </summary>
        /// <returns>Sql</returns>
        public override string ToString()
        {
            var sql = this.Sql.ToString();
            sql.Replace(this.WhereReplacer, this.WhereSql.ToString());
            sql.Replace(this.OrderByReplacer, this.OrderSql.ToString());

            if (!string.IsNullOrEmpty(this.OffsetSql))
            {
                if (string.IsNullOrEmpty(this.OffsetReplacer))
                {
                    sql += this.OffsetSql;
                }
                else
                {
                    sql.Replace(this.OffsetReplacer, this.OffsetSql);
                }
            }
            return sql;
        }
        #endregion
    }
}