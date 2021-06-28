using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RequestLog.Extension.Model;

namespace RequestLog
{
    /// <summary>
    ///
    /// </summary>
    public interface IRequestLogRepository
    {
        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="requestData"></param>
        Task RecordAsync(RequestData requestData);

        /// <summary>
        /// 批量记录日志
        /// </summary>
        /// <param name="requestDatas"></param>
        /// <returns></returns>
        Task RecordMultAsync(IEnumerable<RequestData> requestDatas);
    }
}
