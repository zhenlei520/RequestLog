// Copyright (c) zhenlei520 All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Mvc;
using RequestLog.Filters;

namespace RequestLog.TestApi.Controllers
{
    /// <summary>
    ///
    /// </summary>
    public class TestController : ControllerBase
    {
        private readonly IRequest _request;

        public TestController(IRequest request)
        {
            _request = request;
        }

        /// <summary>
        /// GetContent
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [RequestLog(Ignore = true)]
        public ContentResult GetContent()
        {
            return Content(DateTime.Now.ToString("yyyy-MM-dd hh:MM:ss"));
        }
    }
}
