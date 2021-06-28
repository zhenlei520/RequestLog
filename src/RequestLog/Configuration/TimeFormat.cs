// Copyright (c) zhenlei520 All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.ComponentModel;

namespace RequestLog.Configuration
{
    /// <summary>
    /// 时间格式
    /// </summary>
    public enum TimeFormat
    {
        /// <summary>
        /// 纳秒 最小时间
        /// </summary>
        [Description("ticks")] Ticks = 0,

        /// <summary>
        /// 时间戳 13位
        /// </summary>
        [Description("timestamp")] Timestamp = 1,

        /// <summary>
        /// 普通时间
        /// </summary>
        [Description("generaltime")] GeneralTime = 2
    }
}
