// Copyright (c) zhenlei520 All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace RequestLog.Extension.Content
{
    /// <summary>
    /// 
    /// </summary>
    public class PostRequestProvider : IRequestProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string GetQueryPath(HttpContext context)
        {
            return context.Request.Path + context.Request.QueryString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<object> Get(HttpContext context)
        {
            var contextType = context.Request.ContentType;
            if (contextType.Contains("application/x-www-form-urlencoded") ||
                contextType.Contains("multipart/form-data"))
            {
                return GetByFormXWwwUrlencoded(context);
            }

            if (contextType.Contains("application/json") ||
                contextType.Contains("text/plain") ||
                contextType.Contains("text/html") ||
                contextType.Contains("application/xml"))
            {
                object data= await GetByBody(context);
                return data;
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private object GetByFormXWwwUrlencoded(HttpContext context)
        {
            return Utils.GetData(context.Request.Form.Select(x => new KeyValuePair<string, string>(x.Key, x.Value))
                .ToList());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private async Task<object> GetByBody(HttpContext context)
        {
            context.Request.EnableBuffering();
            var requestReader = new StreamReader(context.Request.Body);
            if (requestReader.BaseStream.CanRead)
            {
                context.Request.Body.Seek(0, SeekOrigin.Begin);
                var requestContent = await requestReader.ReadToEndAsync();
                context.Request.Body.Seek(0, SeekOrigin.Begin);
                return requestContent;
            }

            return null;
        }
    }
}