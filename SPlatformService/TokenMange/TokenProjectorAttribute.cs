
using CoreFramework.VO;
using SPlatformService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace SPlatformService.TokenMange
{
    /// <summary>
    /// 用户令牌验证/// </summary>
    public class TokenProjectorAttribute : ActionFilterAttribute
    {
        private const string UserToken = "token";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            
            // 匿名访问验证
            var anonymousAction = actionContext.ActionDescriptor.GetCustomAttributes<AnonymousAttribute>();
            if (!anonymousAction.Any())
            {
                // 验证token
                var token = TokenVerification(actionContext);
            }

            base.OnActionExecuting(actionContext);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            //actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            //actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Headers", "*");
            //actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE");
            base.OnActionExecuted(actionExecutedContext);
        }

        /// <summary>
        /// 身份令牌验证
        /// </summary>
        /// <param name="actionContext"></param>
        protected virtual string TokenVerification(HttpActionContext actionContext)
        {
            // 获取token
            var token = GetToken(actionContext.ActionArguments, actionContext.Request.Method);

            // 判断token是否有效
            if (!CacheManager.TokenIsExist(token))
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, new ResultObject() { Flag = 401, Message = "Token 失效", Result = null });
                return "";
                //throw new Exception("Token已失效，请重新登陆!");
            }
            else
            {
                //IndependentPrincipal indeP = new IndependentPrincipal();
                //indeP.CompanyGroupId = CacheManager.GetCompanyGroupId(token);
            }

            // 判断用户是否被冻结
            //if (accountInfoService.Exist_User_IsForzen(AccountHelper.GetUUID(token)))
            //{
            //    CacheManager.RemoveToken(token);
            //    tempCacheService.Delete_OneTempCaches(new Guid(token));
            //    throw new UserLoginException("此用户已被冻结,请联系客服!");
            //}

            return token;
        }

        private string GetToken(Dictionary<string, object> actionArguments, HttpMethod type)
        {
            var token = "";

            if (type == HttpMethod.Post)
            {
                foreach (var value in actionArguments.Values)
                {
                    if (value == null)
                        continue;
                    token = value.GetType().GetProperty(UserToken) == null
                        ? GetToken(actionArguments, HttpMethod.Get)
                        : value.GetType().GetProperty(UserToken).GetValue(value).ToString();
                }
            }
            else if (type == HttpMethod.Get)
            {
                if (!actionArguments.ContainsKey(UserToken))
                {
                    //throw new Exception("未附带token!");
                }
                else if (actionArguments[UserToken] != null)
                {
                    token = actionArguments[UserToken].ToString();
                }
                else
                {
                    //throw new Exception("token不能为空!");
                }
            }
            else
            {
                //throw new Exception("暂未开放其它访问方式!");
            }

            return token;
        }
    }
}