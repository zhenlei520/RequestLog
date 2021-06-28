// Copyright (c) zhenlei520 All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.ComponentModel;

namespace RequestLog.MySql
{
    /// <summary>
    /// 分组规则
    /// </summary>
    public enum Rules
    {
        /// <summary>
        /// 不归档
        /// </summary>
        [Description("None")] None = 0,

        /// <summary>
        /// 每年一个记录表
        /// </summary>
        [Description("Year")] Year = 1,

        /// <summary>
        /// 每月一个记录表
        /// </summary>
        [Description("Month")] Month = 2,

        /// <summary>
        /// 每天一个记录表
        /// </summary>
        [Description("Day")] Day = 3
    }
}