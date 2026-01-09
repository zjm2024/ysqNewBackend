using CoreFramework;
using SPLibrary.CoreFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using System.Web.Routing;
using System.Web.SessionState;
using System.Web.Http.WebHost;

namespace WebUI
{
    public class Global : System.Web.HttpApplication
    {
        public override void Init()
        {
            this.PostAuthenticateRequest += (s, e) => HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);  
            base.Init();
        }
        protected void Application_Start(object sender, EventArgs e)
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);  // 注册路由
            GlobalConfiguration.Configure(WebApiConfig.Register);

            DBConfig.DBConnectionTimeOut = ConfigInfo.Instance.DBConnectionTimeOut;
            DBConfig.DbName = ConfigInfo.Instance.DefaultConnectionString;
            DBConfig.ProviderType = EProviderType.MySQL;
        }

        public class SessionableControllerHandler : HttpControllerHandler, IRequiresSessionState
        {
            public SessionableControllerHandler(RouteData routeData) : base(routeData) { }
        }

        public class SessionStateRouteHandler : IRouteHandler
        {
            IHttpHandler IRouteHandler.GetHttpHandler(RequestContext requestContext)
            {
                return new SessionableControllerHandler(requestContext.RouteData);
            }
        }

        public class RouteConfig
        {
            public static void RegisterRoutes(RouteCollection routes)
            {
                routes.MapHttpRoute(
                    name: "WebApiRoute1",
                    routeTemplate: "api/{controller}/{id}",
                    defaults: new { id = RouteParameter.Optional }
                ).RouteHandler = new SessionStateRouteHandler();  // 使用Session  


                routes.MapHttpRoute(
                  name: "WebApiRoute2",
                  routeTemplate: "api/{controller}/{id}/{id2}",
                  defaults: new { id = RouteParameter.Optional }
              );  // 不使用Session  
            }
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}