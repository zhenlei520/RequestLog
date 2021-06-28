// Copyright (c) zhenlei520 All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace RequestLog
{
    /// <summary>
    ///
    /// </summary>
    public interface IRequest
    {
        /// <summary>
        /// 设置扩展信息
        /// </summary>
        /// <param name="data">扩展信息</param>
        /// <returns></returns>
        void SetExtend(object data);

        /// <summary>
        /// 得到扩展信息
        /// </summary>
        /// <returns></returns>
        object GetExtend();
    }
}
