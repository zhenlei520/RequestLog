using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace RequestLog.Internal
{
    /// <summary>
    /// 
    /// </summary>
    public interface IClient : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        Task ExecuteAsync(HttpContext context, RequestDelegate next);
    }
}