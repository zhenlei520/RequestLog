// Copyright (c) zhenlei520 All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;
using RequestLog.Configuration;

namespace RequestLog.Filters
{
    /// <summary>
    /// 请求日志
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class RequestLogAttribute : ActionFilterAttribute
    {
        /// <summary>
        ///
        /// </summary>
        private readonly RequestLogOptions _requestLogOptions;

        /// <summary>
        ///
        /// </summary>
        public RequestLogAttribute()
        {
            base.Order = 0;
            this.Ignore = false;
            this._requestLogOptions = ServiceCollectionExtensions.RequestLogOptions;
        }

        /// <summary>
        /// 唯一标识
        /// </summary>
        public string Name;

        /// <summary>
        ///
        /// </summary>
        public bool Ignore;

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        public override void OnResultExecuted(ResultExecutedContext context)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (_requestLogOptions != null)
            {
                if (context.HttpContext.Items == null)
                {
                    context.HttpContext.Items = new Dictionary<object, object>();
                }

                SetItem(context, $"{_requestLogOptions.Pre}.Ignore", Ignore ? "1" : "0");
                SetItem(context, $"{_requestLogOptions.Pre}.Name",
                    string.IsNullOrEmpty(Name) ? GetName(context) : Name);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private void SetItem(ActionExecutingContext context, string key, object value)
        {
            if (!context.HttpContext.Items.ContainsKey(key))
            {
                context.HttpContext.Items[key] = value;
            }
            else
            {
                context.HttpContext.Items.Add(key, value);
            }
        }

        #region 获取ActionName

        /// <summary>
        /// 获取ActionName
        /// </summary>
        /// <param name="context"></param>
        private string GetName(ActionExecutingContext context)
        {
            string controllerName = context.RouteData.Values.Where(x => x.Key == "controller").Select(x => x.Value)
                .FirstOrDefault()?.ToString() ?? "";
            string actionName = context.RouteData.Values.Where(x => x.Key == "action").Select(x => x.Value)
                .FirstOrDefault()?.ToString() ?? "";
            return $"{controllerName}.{actionName}";
        }

        #endregion
    }
}
