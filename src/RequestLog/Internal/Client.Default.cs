using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using RequestLog.Configuration;
using RequestLog.Extension;
using RequestLog.Extension.Content;
using RequestLog.Extension.Model;

namespace RequestLog.Internal
{
    /// <summary>
    ///
    /// </summary>
    public class ClientDefault : IClient
    {
        private readonly RequestLogOptions _options;
        private readonly IRequest _request;
        private readonly IRequestLogBuilder _requestLogBuilder;

        private readonly List<KeyValuePair<string, IRequestProvider>> _maps =
            new List<KeyValuePair<string, IRequestProvider>>()
            {
                new KeyValuePair<string, IRequestProvider>("POST", new PostRequestProvider()),
                new KeyValuePair<string, IRequestProvider>("GET", new GetRequestProvider())
            };

        /// <summary>
        ///
        /// </summary>
        /// <param name="requestLogOptions"></param>
        /// <param name="requestLogBuilder"></param>
        /// <param name="request"></param>
        public ClientDefault(RequestLogOptions requestLogOptions,
            IRequestLogBuilder requestLogBuilder,
            IRequest request)
        {
            this._options = requestLogOptions;
            this._requestLogBuilder = requestLogBuilder;
            this._request = request;
        }

        /// <summary>
        ///
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task ExecuteAsync(HttpContext context, RequestDelegate next)
        {
            DateTime dateStartTime = DateTime.Now;
            object response = null;
            bool isError = false;
            var originalBodyStream = context.Response.Body;
            try
            {
                //替换request 流
                using (var responseMemoryStream = new MemoryStream())
                {
                    //替换response流
                    context.Response.Body = responseMemoryStream;

                    //进入action
                    await next(context);

                    //读取action返回的结果
                    using (var streamReader = new StreamReader(responseMemoryStream))
                    {
                        context.Response.Body.Seek(0, SeekOrigin.Begin);
                        response = await streamReader.ReadToEndAsync();
                        context.Response.Body.Seek(0, SeekOrigin.Begin);
                        await responseMemoryStream.CopyToAsync(originalBodyStream);
                    }
                }
            }
            catch (Exception ex)
            {
                response = new
                {
                    message = ex.Message,
                    innerException = ex.InnerException?.Message ?? ""
                };
                isError = true;
                throw;
            }
            finally
            {
                context.Response.Body = originalBodyStream;
                if (IsRecord(context))
                {
                    RequestData requestData = await GetRequestData(context);
                    if (requestData != null)
                    {
                        if (response == null)
                        {
                            requestData.SetResponse(false, context.Items[$"{_options.Pre}.Name"]?.ToString() ?? "",
                                "", dateStartTime, _request.GetExtend());
                        }
                        else
                        {
                            requestData.SetResponse(isError,
                                context.Items[$"{_options.Pre}.Name"]?.ToString() ??
                                "", response, dateStartTime, _request.GetExtend());
                        }

                        if (this._requestLogBuilder!=null)
                        {
                            await this._requestLogBuilder.ExecuteAsync(requestData);
                        }
                    }
                }
            }
        }

        #region 得到请求信息

        /// <summary>
        /// 得到请求信息
        /// </summary>
        /// <returns></returns>
        private async Task<RequestData> GetRequestData(HttpContext context)
        {
            try
            {
                var requestId = Guid.NewGuid().ToString();

                var provider = _maps.Where(x => x.Key == context.Request.Method.ToUpper()).Select(x => x.Value)
                    .FirstOrDefault();
                if (provider != null)
                {
                    var data = await provider.Get(context.Request.HttpContext);
                    return new RequestData(requestId,
                        provider.GetQueryPath(context),
                        Utils.GetData(_options.Get(context.Request.Headers
                            .Select(x => new KeyValuePair<string, string>(x.Key, x.Value)).ToList())),
                        context.Request.Protocol,
                        context.Request.Scheme,
                        data??new {}, _options.TimeFormat);
                }

                return null;
            }
#pragma warning disable 168
            catch (Exception ex)
#pragma warning restore 168
            {
                return null;
            }
        }

        #endregion

        #region 得到是否需要记录日志

        /// <summary>
        /// 得到是否需要记录日志
        /// </summary>
        /// <returns></returns>
        private bool IsRecord(HttpContext context)
        {
            context.Items.TryGetValue($"{_options.Pre}.Ignore", out object ignore);
            return (ignore?.ToString() ?? "0") == "0";
        }

        #endregion
    }
}
