using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using static SPlatformService.WebApiApplication;
using CoreFramework;
using SPLibrary.CoreFramework;

namespace BusinessCard
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // 在应用程序启动时运行的代码
            RouteConfig.RegisterRoutes(RouteTable.Routes);  // 注册路由
            GlobalConfiguration.Configure(WebApiConfig.Register);

            DBConfig.DBConnectionTimeOut = ConfigInfo.Instance.DBConnectionTimeOut;
            DBConfig.DbName = ConfigInfo.Instance.DefaultConnectionString;
            DBConfig.ProviderType = EProviderType.MySQL;
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");
            // 允许所有域名，生产环境建议指定具体域名
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Authorization, Accept");

            // 处理预检请求（OPTIONS）
            if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
            {
                HttpContext.Current.Response.StatusCode = 200;
                HttpContext.Current.Response.End();
            }
        }
    }
}