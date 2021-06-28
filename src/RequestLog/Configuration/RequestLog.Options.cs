// Copyright (c) zhenlei520 All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace RequestLog.Configuration
{
    /// <summary>
    /// 请求日志配置
    /// </summary>
    public class RequestLogOptions
    {
        /// <summary>
        ///
        /// </summary>
        public RequestLogOptions()
        {
            Extensions = new List<IRequestLogOptionsExtension>();
            HitHeaders = "";
            Headers = "*";
            BatchRowNum = 300;
            TimeInterval = 1000;
            TimeFormat = TimeFormat.GeneralTime;
        }

        /// <summary>
        ///
        /// </summary>
        public string Pre = "RLog";

        /// <summary>
        /// 允许的头信息，*代表所有的
        /// </summary>
        public string Headers { get; set; }

        /// <summary>
        /// 禁止的头信息
        /// 中间以,分割
        /// 默认不排除
        /// </summary>
        public string HitHeaders { get; set; }

        /// <summary>
        /// 批处理数量，满足指定条数后自动执行记录，默认300条
        /// </summary>
        public int BatchRowNum { get; set; }

        /// <summary>
        /// 时间间隔，默认1000ms
        /// </summary>
        public int TimeInterval { get; set; }

        /// <summary>
        /// 时间类型
        /// </summary>
        public TimeFormat TimeFormat { get; set; }

        /// <summary>
        ///
        /// </summary>
        internal IList<IRequestLogOptionsExtension> Extensions { get; }

        /// <summary>
        /// Registers an extension that will be executed when building services.
        /// </summary>
        /// <param name="extension"></param>
        public void RegisterExtension(IRequestLogOptionsExtension extension)
        {
            if (extension == null)
            {
                throw new ArgumentNullException(nameof(extension));
            }

            Extensions.Add(extension);
        }

        /// <summary>
        /// 得到数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal List<KeyValuePair<string, string>> Get(List<KeyValuePair<string, string>> data)
        {
            if (data == null)
            {
                return new List<KeyValuePair<string, string>>();
            }

            if (Headers != "*")
            {
                data = data.Where(x => Headers.Split(',').ToList().Contains(x.Key)).ToList();
            }

            var hidHeaders = HitHeaders.Split(',').ToList();
            return data.Where(x => !hidHeaders.Contains(x.Key)).ToList();
        }
    }
}
