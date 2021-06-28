// Copyright (c) zhenlei520 All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RequestLog.Configuration;
using RequestLog.Extension.Model;

namespace RequestLog.Internal
{
    /// <summary>
    ///
    /// </summary>
    public class RequestLogBuilderDefault : IRequestLogBuilder
    {
        private readonly IEnumerable<IRequestLogRepository> _requestLogProviders;

        private readonly BatchCommon<RequestData> _batchCommon;

        /// <summary>
        ///
        /// </summary>
        /// <param name="requestLogProviders"></param>
        /// <param name="requestLogOptions"></param>
        public RequestLogBuilderDefault(IEnumerable<IRequestLogRepository> requestLogProviders,
            RequestLogOptions requestLogOptions)
        {
            this._requestLogProviders = requestLogProviders;
            RequestLogOptions options = requestLogOptions;
            this._batchCommon =
                new BatchCommon<RequestData>(options.BatchRowNum, options.TimeInterval, ExecuteRecords);
        }

        #region 记录日志

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        public Task ExecuteAsync(RequestData requestData)
        {
            this._batchCommon.AddJob(requestData);
            return Task.CompletedTask;
        }

        #endregion

        #region 记录日志

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="list"></param>
        private async Task ExecuteRecords(List<RequestData> list)
        {
            foreach (var requestLogRepository in _requestLogProviders)
            {
                await requestLogRepository.RecordMultAsync(list);
            }
        }

        #endregion
    }
}
