// Copyright (c) zhenlei520 All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Mvc;

namespace RequestLog.TestApi.Controllers
{
    /// <summary>
    ///
    /// </summary>
    public class CheckController : ControllerBase
    {
        private readonly IRequest _request;

        public CheckController(IRequest request)
        {
            _request = request;
        }

        /// <summary>
        /// 健康检查
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ContentResult Healthy()
        {
            string content = "当前时间为：" + DateTime.Now + "-" + new Random().Next(0, 10);
            this._request.SetExtend(content);
            return Content(content + "OK");
        }
    }
}
