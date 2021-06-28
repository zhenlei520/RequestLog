﻿// Copyright (c) zhenlei520 All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

 using System.Threading.Tasks;
 using Microsoft.AspNetCore.Http;

 namespace RequestLog.Extension.Content
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRequestProvider
    {
        /// <summary>
        /// 得到请求地址
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        string GetQueryPath(HttpContext context);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task<object> Get(HttpContext context);
    }
}