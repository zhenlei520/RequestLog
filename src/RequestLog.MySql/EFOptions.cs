using System;

namespace RequestLog.MySql
{
    /// <summary>
    /// 
    /// </summary>
    public class EfOptions
    {
        /// <summary>
        /// Gets or sets the table name prefix to use when creating database objects.
        /// </summary>
        public string TableNamePrefix { private get; set; }

        /// <summary>
        /// 得到表名前缀
        /// </summary>
        /// <returns></returns>
        internal string GetTableNamePrefix()
        {
            if (string.IsNullOrEmpty(TableNamePrefix))
            {
                return "rq_log";
            }

            return TableNamePrefix;
        }

        /// <summary>
        /// EF db context type.
        /// </summary>
        internal Type DbContextType { get; set; }
    }
}