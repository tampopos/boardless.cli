namespace Tmpps.Infrastructure.Common.ValueObjects
{
    /// <summary>
    /// ソート
    /// </summary>
    public class Sort
    {
        /// <summary>コンストラクタ</summary>
        public Sort() { }
        /// <summary>コンストラクタ</summary>
        public Sort(string name) : this() => this.Name = name;
        /// <summary>コンストラクタ</summary>
        public Sort(string name, SortDirection direction) : this(name) => this.Direction = direction;
        /// <summary>
        /// 名前
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 向き
        /// </summary>
        public SortDirection Direction { get; set; }
    }
}