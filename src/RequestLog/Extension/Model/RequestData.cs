// Copyright (c) zhenlei520 All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using RequestLog.Configuration;

namespace RequestLog.Extension.Model
{
    /// <summary>
    /// 请求数据
    /// </summary>
    public class RequestData
    {
        /// <summary>
        /// 
        /// </summary>
        public RequestData()
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="unique"></param>
        /// <param name="requestUrl"></param>
        /// <param name="headers"></param>
        /// <param name="protocol"></param>
        /// <param name="scheme"></param>
        /// <param name="message"></param>
        /// <param name="timeFormat"></param>
        public RequestData(string unique, string requestUrl, object headers,
            string protocol,
            string scheme,
            object message,
            TimeFormat timeFormat) : this()
        {
            this.Unique = unique;
            this.RequestUrl = requestUrl;
            this.Headers = headers;
            this.Protocol = protocol;
            this.Scheme = scheme;
            this.Message = message;
            if (timeFormat == TimeFormat.Timestamp)
            {
                this.Timestamp = DateTime.Now.ToUnixTimestamp().ToString();
            }
            else if (timeFormat == TimeFormat.GeneralTime)
            {
                this.Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ms");
            }
            else
            {
                this.Timestamp = DateTime.Now.ToTicks().ToString();
            }
        }

        /// <summary>
        /// 唯一标识
        /// </summary>
        public string Unique { get; }

        /// <summary>
        /// 服务名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 请求Url
        /// </summary>
        public string RequestUrl { get; }

        /// <summary>
        /// 请求头信息
        /// </summary>
        public object Headers { get; }

        /// <summary>
        /// 数据
        /// </summary>
        public object Message { get; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public string Timestamp { get; }

        /// <summary>
        /// Protocol
        /// </summary>
        public string Protocol { get; }

        /// <summary>
        /// 协议内容
        /// </summary>
        public string Scheme { get; }

        /// <summary>
        /// 响应值
        /// </summary>
        public object Response { get; private set; }

        /// <summary>
        /// 是否异常
        /// </summary>
        public bool IsError { get; private set; }

        /// <summary>
        /// 耗费时间 ms
        /// </summary>
        public double Duration { get; private set; }

        /// <summary>
        /// 扩展信息
        /// </summary>
        public object Extend { get; private set; }

        /// <summary>
        /// 设置响应值
        /// </summary>
        /// <param name="isError">是否异常</param>
        /// <param name="name"></param>
        /// <param name="response">响应信息</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="extendData">扩展信息</param>
        internal void SetResponse(bool isError, string name, object response, DateTime startTime, object extendData)
        {
            IsError = isError;
            Name = name;
            Response = response;
            Duration = Math.Round((DateTime.Now - startTime).TotalMilliseconds);
            Extend = extendData;
        }
    }
}
