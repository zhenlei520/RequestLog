// Copyright (c) zhenlei520 All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RequestLog.Configuration;
using RequestLog.Internal;
using RequestLog.Serialize;

namespace RequestLog
{
    /// <summary>
    ///
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        ///
        /// </summary>
        internal static IServiceCollection ServiceCollection;

        /// <summary>
        ///
        /// </summary>
        internal static RequestLogOptions RequestLogOptions;

        /// <summary>
        ///
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setupAction"></param>
        public static void AddRequestLog(this IServiceCollection services, Action<RequestLogOptions> setupAction)
        {
            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            ServiceCollection = services;
            services.AddHttpContextAccessor();
            services.TryAddSingleton<IClient, ClientDefault>();
            services.TryAddSingleton<IRequestLogBuilder, RequestLogBuilderDefault>();
            services.TryAddSingleton<IJsonProvider, NewtonsoftJsonProvider>();
            services.TryAddSingleton<IRequest, RequestDefault>();
            RequestLogOptions = new RequestLogOptions();
            setupAction.Invoke(RequestLogOptions);
            foreach (var serviceExtension in RequestLogOptions.Extensions)
            {
                serviceExtension.AddServices(services);
            }
            services.TryAddSingleton(RequestLogOptions);
            services.AddScoped<Filters.RequestLogAttribute>();
            services.AddHostedService<Bootstrapper>();
        }
    }
}
