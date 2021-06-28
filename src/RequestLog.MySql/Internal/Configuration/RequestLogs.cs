// Copyright (c) zhenlei520 All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using RequestLog.Internal;

namespace RequestLog.MySql.Internal.Configuration
{
    /// <summary>
    /// 日志
    /// </summary>
    public class RequestLogs
    {
        /// <summary>
        ///
        /// </summary>
        public RequestLogs()
        {
            this.Id = SnowflakeId.Default().NextId().ToString();
        }

        /// <summary>
        /// 日志序列号
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 唯一标识
        /// </summary>
        public string Unique { get; set; }

        /// <summary>
        /// 服务名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 请求Url地址
        /// </summary>
        public string RequestUrl { get; set; }

        /// <summary>
        /// 请求Header
        /// </summary>
        public string Headers { get; set; }

        /// <summary>
        /// Protocol
        /// </summary>
        public string Protocol { get; set; }

        /// <summary>
        /// Scheme
        /// </summary>
        public string Scheme { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public string Timestamp { get; set; }

        /// <summary>
        /// 是否出错
        /// </summary>
        public short State { get; set; }

        /// <summary>
        /// 响应信息
        /// </summary>
        public string Response { get; set; }

        /// <summary>
        /// 耗费时间 ms
        /// </summary>
        public double Duration { get; set; }

        /// <summary>
        /// 扩展信息
        /// </summary>
        public object Extend { get; set; }
    }
}
