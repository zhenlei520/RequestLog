// Copyright (c) zhenlei520 All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Builder;

namespace RequestLog
{
    /// <summary>
    /// 
    /// </summary>
    public static class RequestLogBuilderExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseRequestLog(
            this IApplicationBuilder app)
        {
            app.UseMiddleware<RequestMiddleware>();
            return app;
        }
    }
}