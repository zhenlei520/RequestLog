// Copyright (c) zhenlei520 All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Threading.Tasks;
using RequestLog.Extension.Model;

namespace RequestLog
{
    /// <summary>
    ///
    /// </summary>
    public interface IRequestLogBuilder
    {
        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        Task ExecuteAsync(RequestData requestData);
    }
}
