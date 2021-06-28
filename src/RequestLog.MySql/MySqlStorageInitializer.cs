using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using RequestLog.Persistence;

namespace RequestLog.MySql
{
    /// <summary>
    ///
    /// </summary>
    public class MySqlStorageInitializer : IStorageInitializer
    {
        private readonly MySqlOptions _options;

        /// <summary>
        ///
        /// </summary>
        /// <param name="options"></param>
        public MySqlStorageInitializer(IOptions<MySqlOptions> options)
        {
            _options = options.Value;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task InitializeAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            var sql = CreateDbTablesScript();
            using (var connection = new MySqlConnection(_options.ConnectionString))
            {
                await connection.ExecuteAsync(sql);
            }

            MySqlOptionsExtension.LastExecTime = DateTime.Now;
        }

        /// <summary>
        /// 检查并且更新持续时间属性
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task CheckAndUpdate(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }


            using (var connection = new MySqlConnection(_options.ConnectionString))
            {
                await CheckAndUpdateDuration(connection);
                await CheckAndUpdateExtend(connection);
            }
        }

        /// <summary>
        /// 得到表名
        /// </summary>
        /// <returns></returns>
        public string GetTableName()
        {
            string alias = "";
            switch (this._options.Rules)
            {
                case Rules.Year:
                    alias = DateTime.Now.Year.ToString();
                    break;
                case Rules.Month:
                    alias = DateTime.Now.Month.ToString("00");
                    break;
                case Rules.Day:
                    alias = DateTime.Now.Day.ToString("00");
                    break;
                case Rules.None:
                    alias = "";
                    break;
            }

            return string.IsNullOrEmpty(alias)
                ? $"{string.IsNullOrEmpty(_options.GetTableNamePrefix())}.request_log"
                : $"{_options.GetTableNamePrefix()}.request_log.{alias}";
        }

        /// <summary>
        /// 服务名
        /// </summary>
        public string ServiceName => OptionsExtensions.ServiceName;

        #region private methods

        #region 创建记录表

        /// <summary>
        /// 创建记录表
        /// </summary>
        /// <returns></returns>
        private string CreateDbTablesScript()
        {
            var batchSql = $@"
CREATE TABLE IF NOT EXISTS `{GetTableName()}` (
  `id` bigint NOT NULL,
  `name` varchar(400) NOT NULL COMMENT '方法标识：默认 控制器名.方法名',
  `unique` varchar(200) DEFAULT NULL COMMENT '唯一标识',
  `request_url` longtext COMMENT '请求地址',
  `headers` longtext COMMENT '请求头',
  `protocol` varchar(50) COMMENT '请求协议',
  `scheme` varchar(50) COMMENT '请求方式',
  `message` longtext COMMENT '请求内容',
  `timestamp` varchar(50) NOT NULL COMMENT '时间戳',
  `state` bit NOT NULL default 1 COMMENT '执行状态 异常：0，正常：1',
  `duration` bigint NOT NULL default 0 COMMENT '持续时间',
  `response` longtext default NULL COMMENT '响应信息',
  `extend` longtext default NULL COMMENT '扩展信息',
  PRIMARY KEY (`Id`),
  INDEX `IX_timestamp`(`timestamp`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;";
            return batchSql;
        }

        #endregion

        #region 增加持续时间属性

        /// <summary>
        /// 增加持续时间属性
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        private async Task CheckAndUpdateDuration(MySqlConnection connection)
        {
            var sql =
                $"select * from information_schema.columns t  where table_name = '{GetTableName()}' and TABLE_SCHEMA='{connection.Database}' and column_name = 'duration'";
            var result = await connection.QueryAsync(sql);
            if (result == null || !result.Any())
            {
                sql =
                    $"ALTER TABLE `{GetTableName()}` ADD `duration` bigint NOT NULL default 0 COMMENT '持续时间 ms' after `state`;";
                await connection.ExecuteAsync(sql);
            }
        }

        #endregion

        #region 增加扩展属性

        /// <summary>
        /// 增加扩展属性
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        private async Task CheckAndUpdateExtend(MySqlConnection connection)
        {
            var sql =
                $"select * from information_schema.columns t  where table_name = '{GetTableName()}' and TABLE_SCHEMA='{connection.Database}' and column_name = 'extend'";
            var result = await connection.QueryAsync(sql);
            if (result == null || !result.Any())
            {
                sql =
                    $"ALTER TABLE `{GetTableName()}` ADD `extend` longtext default NULL COMMENT '扩展信息' after `response`;";

                await connection.ExecuteAsync(sql);
            }
        }

        #endregion

        #endregion
    }
}
