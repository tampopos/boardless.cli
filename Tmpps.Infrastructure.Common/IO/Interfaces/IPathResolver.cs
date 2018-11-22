namespace Tmpps.Infrastructure.Common.IO.Interfaces
{
    /// <summary>
    /// パス解決の機能を提供する
    /// </summary>
    public interface IPathResolver
    {
        /// <summary>
        /// RootPath
        /// </summary>
        string RootPath { get; }
        /// <summary>
        /// ファイルパスの解決
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        string ResolveFilePath(string path);
        /// <summary>
        /// ディレクトリパスの解決
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        string ResolveDirectoryPath(string path);
        /// <summary>
        /// ファイルの存在チェック
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        bool ExistsFilePath(string path);
        /// <summary>
        /// ディレクトリの存在チェック
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        bool ExistsDirectoryPath(string path);
    }
}