// Copyright (c) zhenlei520 All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using RequestLog.Internal;

namespace RequestLog
{
    /// <summary>
    /// 
    /// </summary>
    public class RequestMiddleware
    {
        private readonly IClient _client;
        private readonly RequestDelegate _next;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        /// <param name="client"></param>
        public RequestMiddleware(RequestDelegate next, IClient client)
        {
            this._next = next;
            this._client = client;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            await _client.ExecuteAsync(context, _next);
        }
    }
}