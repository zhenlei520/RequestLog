// Copyright (c) zhenlei520 All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace RequestLog.Extension.Content
{
    /// <summary>
    /// 
    /// </summary>
    public class GetRequestProvider : IRequestProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string GetQueryPath(HttpContext context)
        {
            return context.Request.Path;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task<object> Get(HttpContext context)
        {
            return Task.FromResult(Utils.GetData(context.Request.Query
                .Select(x => new KeyValuePair<string, string>(x.Key, x.Value))
                .ToList()));
        }
    }
}