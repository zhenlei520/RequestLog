# RequestLog

[![NuGet](https://img.shields.io/nuget/v/RequestLog.svg?style=flat-square)](https://www.nuget.org/packages/RequestLog)
[![NuGet Download](https://img.shields.io/nuget/dt/RequestLog.svg?style=flat-square)](https://www.nuget.org/packages/RequestLog)

<a class="ide" href="https://www.jetbrains.com/?from=RequestLog">
    <p>Thanks for the sponsorship of jetbrains products，A useful development tool</p>
    <img src="./jetbrains.png" width="50" height="50">
</a>

RequestLog是一个基于.NET Standard2.0、.NET Standard2.1 的 C#库，帮助我们收集在NetCore下记录请求的类库，具体使用办法查看以下文档


## Getting Started

你可以运行以下下命令在你的项目中安装 RequestLog

    PM> Install-Package RequestLog

RequestLog目前仅支持Mysql，你可以按需选择下面的包进行安装

    PM> Install-Package RequestLog.MySql

## Configuration
首先配置RequestLog到 Startup.cs 文件中，如下：

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddRequestLog(options =>
        {
            options.Headers = "Host";//需要记录的请求头信息,如需记录多个请求头，则中间以,分割，* 记录所有的请求头
            options.HitHeaders = "Content-Type";//排除的请求头信息，最后保存的请求头为headers去除hitHeaders的值

            options.UseMySql(x =>
            {
                x.ConnectionString =
                    "Server=localhost;port=3306;database=system.request.log;uid=root;pwd=rootroot;Convert Zero Datetime=True;";
                
                 //x.Rules = Rules.None;//当前请求不归档,所有请求在一个表中
                 x.Rules = Rules.Year;//当前请求以年归档（默认）
                 //x.Rules = Rules.Month;//当前请求以月归档
                 //x.Rules = Rules.Day;//当前请求以日归档
            });//如果你使用的Mysql，根据数据库选择进行配置：
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseRequestLog();//启用日志记录，不启用是不生效的（放到最上方）
            
        ......
    }


在 Controller 中记录请求

    //整个控制器都记录日志(除了被忽略的)
    [RequestLog]
    public class CheckController : Controller
    {
        //记录日志
        [HttpGet]
        public IActionResult Healthy()
        {
            return Content("healthy");
        }

        //当前方法忽略，不记录日志
        [HttpGet]
        [RequestLog(Ignore = true)]
        public IActionResult Healthy2()
        {
            return Content("healthy2");
        }
    }

    public class Check2Controller : Controller
    {
        
        //仅当前方法记录日志，并且为name赋值为Healthy
        [HttpGet]
        [RequestLog(Name = "Healthy")]
        public IActionResult Healthy()
        {
            return Content("healthy");
        }

        //记录日志
        [HttpGet]
        public IActionResult Healthy()
        {
            return Content("healthy");
        }
    }


## 高级教程


1. 希望在记录中增加请求额外的参数，可通过注入IRequest，调用其SetExtend方法记录。


        例如：错误中间件中，记录本次实际错误的信息，例子如下：

        /// <summary>
        /// 错误异常
        /// </summary>
        public class ErrorHandlingMiddleware
        {
            private readonly RequestDelegate _next;
            private readonly IRequest _request;

            /// <summary>
            ///
            /// </summary>
            /// <param name="next"></param>
            public ErrorHandlingMiddleware(RequestDelegate next, IRequest request)
            {
                _next = next;
                _request = request;
            }

            /// <summary>
            ///
            /// </summary>
            /// <param name="context"></param>
            /// <returns></returns>
            public async Task Invoke(HttpContext context)
            {
                try
                {
                    await _next(context);
                }
                catch (System.Exception ex)
                {
                    _request.SetExtend(ex.Message);
                    await HandleExceptionAsync(context, msg);
                }
            }

            private Task HandleExceptionAsync(HttpContext context, string msg)
            {
                context.Response.ContentType = "application/json;charset=utf-8";
                return context.Response.WriteAsync(msg);
            }
        }

在Startup中Configure中添加
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
              app.UseRequestLog();//启用日志记录，不启用是不生效的（放到最上方）

              app.UseMiddleware<ErrorHandlingMiddleware>();//全局异常中间件

               ......
        }
        
Tip：
	最新版启用日志记录默认后会对所有的请求增加日志，针对上传文件类以及不希望记录的请按照方法或者控制器头部增加[RequestLog(Ignore = true)]，以免影响性能
