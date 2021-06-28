using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using RequestLog.Persistence;

namespace RequestLog.MySql
{
    /// <summary>
    ///
    /// </summary>
    internal class MySqlOptionsExtension : IRequestLogOptionsExtension
    {
        private readonly Action<MySqlOptions> _configure;

        /// <summary>
        ///
        /// </summary>
        public static DateTime LastExecTime;

        /// <summary>
        ///
        /// </summary>
        /// <param name="configure"></param>
        public MySqlOptionsExtension(Action<MySqlOptions> configure)
        {
            _configure = configure;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="services"></param>
        public void AddServices(IServiceCollection services)
        {
            services.TryAddEnumerable(new ServiceDescriptor(typeof(IStorageInitializer), typeof(MySqlStorageInitializer),
                ServiceLifetime.Singleton));
            services.TryAddEnumerable(new ServiceDescriptor(typeof(IRequestLogRepository), typeof(RequestLogRepository),
                ServiceLifetime.Singleton));

            //Add MySqlOptions
            services.Configure(_configure);
            // services.AddSingleton<IConfigureOptions<MySqlOptions>, ConfigureMySqlOptions>();
        }
    }
}
