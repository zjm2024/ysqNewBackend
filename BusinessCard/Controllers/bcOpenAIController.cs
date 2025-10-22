using BroadSky.WeChatAppDecrypt;
using CoreFramework.VO;
using SPlatformService;
using SPlatformService.Controllers;
using SPlatformService.Models;
using SPlatformService.TokenMange;
using SPLibrary.BusinessCardManagement.BO;
using SPLibrary.BusinessCardManagement.VO;
using SPLibrary.CoreFramework;
using SPLibrary.CoreFramework.BO;
using SPLibrary.CoreFramework.Logging.BO;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.CustomerManagement.VO;
using SPLibrary.WebConfigInfo;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using MySqlX.XDevAPI.Common;
using MySql.Data.MySqlClient;

namespace BusinessCard.Controllers
{
    [RoutePrefix("SPWebAPI/bcOpenAI")]
    [TokenProjector]
    public class bcOpenAIController : ApiController
    {
        /// <summary>
        /// 获取我的名片
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getText"), HttpGet]
        public ResultObject getText(string Prompt, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            CardBO CardBO = new CardBO(new CustomerProfile());
            /*
            if (!CardBO.isVIP(customerId)) {
                return new ResultObject() { Flag = 0, Message = "乐ChatGPT测试体验已结束！\r\n即日起至乐Chat机器人正式上线期免费赠送1年使用权限（预售定价699元/年），仅限活动期间开通/续费的五星会员用户有效！", Result = null };
            };*/

            if (CacheSystemConfig.GetSystemConfig(true).MessageNotiCount > 0)
            {
                return new ResultObject() { Flag = 0, Message = "正在维护升级中，敬请期待！！！", Result = null };
            }

            BaiduAIBO baiduAI = new BaiduAIBO();
            string bdmsg = baiduAI.GetChat(Prompt);
            if (bdmsg != "")
            {
                return new ResultObject() { Flag = 1, Message = "获取成功", Result = bdmsg };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "AI正在开小差,请再试一下", Result = null };
            }

            /* OpenAi接口
            string MessageAPI = CacheSystemConfig.GetSystemConfig(true).MessageAPI;
            LogBO _log = new LogBO(typeof(CustomerController));
            try
            {
                //ConfigInfo.Instance.OpenApiKey 配置文件的链接
                
                string url = MessageAPI + "&Prompt=" + Prompt;
                string jsonStr = HttpHelper.HtmlFromUrlGet(url);
                ResultObject Result = JsonConvert.DeserializeObject<ResultObject>(jsonStr);
                if (Result.Flag == 1)
                {
                    string msg = Result.Result.ToString();
                    if (msg.StartsWith("\n")) {
                        msg = msg.Substring(2);
                    };
                    if (msg.StartsWith("\n"))
                    {
                        msg = msg.Substring(2);
                    };
                    Result.Result = msg;
                    string strErrorMsg = "OpenAI_Message0:Prompt=" + Prompt+ ",msg="+ msg+",url="+ MessageAPI;
                    _log.Error(strErrorMsg);
                }
                else
                {
                    Result.Message = "AI正在开小差,请稍后再试一下";
                    
                    string strErrorMsg = "OpenAI_Message1:Prompt=" + Prompt + ",Result=" + Result.Result + ",url=" + MessageAPI;
                    _log.Error(strErrorMsg);
                }
                return Result;
            }
            catch(Exception ex)
            {
                string strErrorMsg = "OpenAI_Message2:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source + ",url=" + MessageAPI;
                _log.Error(strErrorMsg);
                return new ResultObject() { Flag = 0, Message = "AI正在开小差,请再试一下", Result = ex };
            }*/
        }
        /// <summary>
        /// 测试
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getTest"), HttpGet, Anonymous]
        public ResultObject getTest()
        {
            BaiduAIBO baiduAI = new BaiduAIBO();

            return new ResultObject() { Flag = 0, Message = "", Result = baiduAI.GetChat("用js写一个排序函数") };
        }

    }
}
