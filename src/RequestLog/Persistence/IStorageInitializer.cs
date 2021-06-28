using System.Threading;
using System.Threading.Tasks;

namespace RequestLog.Persistence
{
    /// <summary>
    ///
    /// </summary>
    public interface IStorageInitializer
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task InitializeAsync(CancellationToken cancellationToken);

        /// <summary>
        /// 检查并且更新持续时间属性
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task CheckAndUpdate(CancellationToken cancellationToken);

        /// <summary>
        /// 获取表名
        /// </summary>
        /// <returns></returns>
        string GetTableName();

        /// <summary>
        /// 服务名
        /// </summary>
        string ServiceName { get; }
    }
}
