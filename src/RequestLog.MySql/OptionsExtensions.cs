using System;
using RequestLog.Configuration;

namespace RequestLog.MySql
{
    /// <summary>
    ///
    /// </summary>
    public static class OptionsExtensions
    {
        /// <summary>
        /// MySql实现
        /// </summary>
        internal static string ServiceName => "MySql";

        /// <summary>
        ///
        /// </summary>
        /// <param name="options"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static RequestLogOptions UseMySql(this RequestLogOptions options, string connectionString)
        {
            return options.UseMySql(opt => { opt.ConnectionString = connectionString; });
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="options"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static RequestLogOptions UseMySql(this RequestLogOptions options, Action<MySqlOptions> configure)
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            options.RegisterExtension(new MySqlOptionsExtension(configure));

            return options;
        }
    }
}
