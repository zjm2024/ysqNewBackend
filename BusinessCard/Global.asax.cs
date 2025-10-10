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
    }
}