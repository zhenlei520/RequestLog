using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RequestLog.MySql;

namespace RequestLog.TestApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRequestLog(options =>
            {
                options.Headers = "Host";//需要记录的请求头信息,如需记录多个请求头，则中间以,分割，* 记录所有的请求头
                options.HitHeaders = "Content-Type";//排除的请求头信息，最后保存的请求头为headers去除hitHeaders的值
                options.TimeInterval = 1000;
                options.UseMySql(x =>
                {
                    x.ConnectionString =
                        "Server=localhost;port=3306;database=system.request.log;uid=root;pwd=root;Convert Zero Datetime=True;";
                    x.Rules = Rules.Year;//当前请求以年归档（默认）
                });//如果你使用的Mysql，根据数据库选择进行配置：
            });
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRequestLog();//启用日志记录，不启用是不生效的（放到最上方）
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
