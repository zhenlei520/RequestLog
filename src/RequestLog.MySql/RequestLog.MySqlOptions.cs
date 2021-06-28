namespace RequestLog.MySql
{
    /// <summary>
    /// 请求日志
    /// </summary>
    public class MySqlOptions : EfOptions
    {
        /// <summary>
        ///
        /// </summary>
        public MySqlOptions()
        {
            Rules = Rules.Year;
        }

        /// <summary>
        /// Gets or sets the database's connection string that will be used to store database entities.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 分组规则
        /// </summary>
        public Rules Rules { get; set; }
    }

    // internal class ConfigureMySqlOptions : IConfigureOptions<MySqlOptions>
    // {
    //     private readonly IServiceScopeFactory _serviceScopeFactory;
    //
    //     public ConfigureMySqlOptions(IServiceScopeFactory serviceScopeFactory)
    //     {
    //         _serviceScopeFactory = serviceScopeFactory;
    //     }
    //
    //     public void Configure(MySqlOptions options)
    //     {
    //         if (options.DbContextType != null)
    //         {
    //             using (var scope = _serviceScopeFactory.CreateScope())
    //             {
    //                 var provider = scope.ServiceProvider;
    //                 using (var dbContext = (DbContext) provider.GetRequiredService(options.DbContextType))
    //                 {
    //                     options.ConnectionString = dbContext.Database.GetDbConnection().ConnectionString;
    //                 }
    //             }
    //         }
    //     }
    // }
}
