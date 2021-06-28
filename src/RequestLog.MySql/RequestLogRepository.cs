using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using RequestLog.Extension.Model;
using RequestLog.MySql.Internal.Configuration;
using RequestLog.Persistence;
using RequestLog.Serialize;

namespace RequestLog.MySql
{
    /// <summary>
    ///
    /// </summary>
    public class RequestLogRepository : IRequestLogRepository
    {
        private readonly MySqlOptions _options;
        private IJsonProvider _jsonProvider;
        private readonly IStorageInitializer _storageInitializer;

        /// <summary>
        ///
        /// </summary>
        /// <param name="storageInitializers"></param>
        /// <param name="options"></param>
        /// <param name="jsonProvider"></param>
        public RequestLogRepository(IEnumerable<IStorageInitializer> storageInitializers, IOptions<MySqlOptions> options,
            IJsonProvider jsonProvider)
        {
            this._storageInitializer = storageInitializers.FirstOrDefault(x => x.ServiceName==OptionsExtensions.ServiceName);
            this._options = options.Value;
            this._jsonProvider = jsonProvider;
        }

        #region 记录日志

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        public async Task RecordAsync(RequestData requestData)
        {
            await this.RecordMultAsync(new List<RequestData>()
            {
                requestData
            });
        }

        #endregion

        #region 批量记录日志

        /// <summary>
        /// 批量记录日志
        /// </summary>
        /// <param name="requestDatas"></param>
        /// <returns></returns>
        public async Task RecordMultAsync(IEnumerable<RequestData> requestDatas)
        {
            await CheckAndCreateTable();
            var sql =
                $"INSERT INTO `{_storageInitializer.GetTableName()}`(`id`,`unique`,`name`,`request_url`,`headers`,`protocol`,`scheme`,`message`,`timestamp`,`state`,`response`,`duration`,`extend`) VALUES(@Id,@Unique,@Name,@RequestUrl,@Headers,@Protocol,@Scheme,@Message,@Timestamp,@State,@Response,@Duration,@Extend);";
            using (var connection = new MySqlConnection(_options.ConnectionString))
            {
                try
                {
                    List<RequestLogs> requestDataList = new List<RequestLogs>();
                    foreach (var request in requestDatas)
                    {
                        requestDataList.Add(new RequestLogs()
                        {
                            Unique = request.Unique,
                            Name = request.Name,
                            RequestUrl = request.RequestUrl,
                            Headers = GetString(request.Headers),
                            Protocol = request.Protocol,
                            Scheme = request.Scheme,
                            Message = GetString(request.Message),
                            Timestamp = request.Timestamp,
                            State = request.IsError ? (short) 0 : (short) 1,
                            Response = GetString(request.Response),
                            Duration = request.Duration,
                            Extend = GetString(request.Extend)
                        });
                    }

                    await connection.ExecuteAsync(sql, requestDataList);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        #endregion

        #region 得到Json

        /// <summary>
        /// 得到Json
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string GetString(object data)
        {
            if (data == null)
            {
                return "";
            }

            if (data.GetType().IsClass && !(data is string))
            {
                return this._jsonProvider.Serializer(data);
            }

            return data.ToString();
        }

        #endregion

        #region 检查表是否存在

        /// <summary>
        /// 检查表是否存在
        /// </summary>
        /// <returns></returns>
        private async Task CheckAndCreateTable()
        {
            if ((_options.Rules == Rules.Year && MySqlOptionsExtension.LastExecTime.Date.Year == DateTime.Now.Year) ||
                (_options.Rules == Rules.Month &&
                 MySqlOptionsExtension.LastExecTime.Date.Month == DateTime.Now.Month) ||
                (_options.Rules == Rules.Day && MySqlOptionsExtension.LastExecTime.Date.Day == DateTime.Now.Day))
            {
                return;
            }

            await _storageInitializer.InitializeAsync(CancellationToken.None);
        }

        #endregion
    }
}
