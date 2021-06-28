// Copyright (c) zhenlei520 All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using RequestLog.Configuration;

namespace RequestLog.Internal
{
    /// <summary>
    /// 设置扩展信息
    /// </summary>
    public class RequestDefault : IRequest
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _key;

        /// <summary>
        ///
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <param name="requestLogOptions"></param>
        public RequestDefault(IHttpContextAccessor httpContextAccessor, RequestLogOptions requestLogOptions)
        {
            this._httpContextAccessor = httpContextAccessor;
            this._key = $"{requestLogOptions.Pre}.extend";
        }

        #region 设置扩展信息

        /// <summary>
        /// 设置扩展信息
        /// </summary>
        /// <param name="data">扩展信息</param>
        /// <returns></returns>
        public void SetExtend(object data)
        {
            if (!this._httpContextAccessor.HttpContext.Items.ContainsKey(this._key))
            {
                this._httpContextAccessor.HttpContext.Items.Add(this._key, data);
            }
        }

        #endregion

        #region 得到扩展信息

        /// <summary>
        /// 得到扩展信息
        /// </summary>
        /// <returns></returns>
        public object GetExtend()
        {
            this._httpContextAccessor.HttpContext.Items.TryGetValue(this._key, out var extend);
            return extend;
        }

        #endregion
    }
}
