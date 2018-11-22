using System.IO;
using Tmpps.Infrastructure.Common.IO.Interfaces;

namespace Tmpps.Infrastructure.Common.IO
{
    /// <summary>
    /// パスを解決するためのオブジェクト
    /// </summary>
    public class PathResolver : IPathResolver
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PathResolver(string rootPath = null)
        {
            this.RootPath = rootPath;
        }
        /// <summary>
        /// RootPath
        /// </summary>
        public string RootPath { get; private set; }
        /// <inheritdoc />
        public string ResolveFilePath(string path)
        {
            if (File.Exists(path))
            {
                return path;
            }
            path = Path.Combine(this.RootPath, path);
            if (File.Exists(path))
            {
                return path;
            }
            return null;
        }
        /// <inheritdoc />
        public string ResolveDirectoryPath(string path)
        {
            if (Directory.Exists(path))
            {
                return path;
            }
            path = Path.Combine(this.RootPath, path);
            if (Directory.Exists(path))
            {
                return path;
            }
            return null;
        }
        /// <inheritdoc />
        public bool ExistsFilePath(string path)
        {
            if (File.Exists(path))
            {
                return true;
            }
            path = Path.Combine(this.RootPath, path);
            return File.Exists(path);
        }
        /// <inheritdoc />
        public bool ExistsDirectoryPath(string path)
        {
            if (Directory.Exists(path))
            {
                return true;
            }
            path = Path.Combine(this.RootPath, path);
            return Directory.Exists(path);
        }
    }
}