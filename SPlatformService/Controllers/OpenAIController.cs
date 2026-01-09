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
using System.Security.Policy;
using Aop.Api.Domain;
using TencentCloud.Tci.V20190318.Models;

namespace SPlatformService.Controllers
{
    [RoutePrefix("SPWebAPI/OpenAI")]
    [TokenProjector]
    public class OpenAIController : ApiController
    {
        /// <summary>
        /// 获取我的名片
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getText"), HttpGet]
        public ResultObject getText(string Prompt, string token)
        {
            //UserProfile uProfile = CacheManager.GetUserProfile(token);
            //CustomerProfile cProfile = uProfile as CustomerProfile;
            //int customerId = cProfile.CustomerId;
            //CardBO CardBO = new CardBO(new CustomerProfile());
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
        /// 测试视频jobid
        /// </summary>
        /// <param name="voddata">视频参数</param>
        /// <returns></returns>
        [Route("postvodJobid"), HttpPost, Anonymous]
        public ResultObject postvodJobid(dynamic voddata)
        {
            try
            {
                string text = voddata.text;
                var imageurl = voddata.imageurl;
                JsonSerializer serializer = new JsonSerializer();

                string url = "https://aip.baidubce.com/rpc/2.0/brain/creative/ttv/material?access_token=24.70daab2be2d702d169091a3a1b51cdd8.2592000.1715152214.282335-55225680";
                List<structsModel> structsList = new List<structsModel>();
                structsList.Add(new structsModel
                {
                    type = "text",
                    text = text
                });
                if (string.IsNullOrEmpty(imageurl.ToString()))
                {

                    return new ResultObject() { Flag = -1, Message = "图片不能为空" };

                }
                string[] imageurlStr = JsonConvert.DeserializeObject<string[]>(imageurl.ToString());
                if (imageurlStr.Count() < 1)
                {

                    return new ResultObject() { Flag = -1, Message = "图片不能为空" };

                }
                foreach (var item in imageurlStr)
                {

                    structsList.Add(new structsModel
                    {
                        type = "image",
                        mediaSource = new mediaSourceModel()
                        {
                            type = "3",
                            url = item
                        } 
                    });
                }
                
             

                int[] resolutionint = new int[] { 1280, 720 };
                var json = new
                {
                    source = new sourceModel
                    {
                        structs = structsList
                    },
                    config = new configModel
                    {
                        productType = "video",
                        duration = -1,
                        resolution = resolutionint
                    }
                };
                BDMode req = JsonConvert.DeserializeObject<BDMode>(HttpPost(url, JsonConvert.SerializeObject(json)));
                string jobid = req.data.jobId;
                return new ResultObject() { Flag = 0, Message = "生成成功", Result = jobid };

            }
            catch (Exception e)
            {
                return new ResultObject() { Flag = -1, Message = "AI正在开小差,请再试一下" };
            }
        }

        /// <summary>
        /// HttpPost请求
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string HttpPost(string uri, string data)
        {
            var dataStr = data;
            HttpWebRequest request = HttpWebRequest.Create(uri) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/json";
            Stream reqStream;
            byte[] postData = Encoding.UTF8.GetBytes(dataStr);
            reqStream = request.GetRequestStream();
            reqStream.Write(postData, 0, postData.Length);
            var response = (HttpWebResponse)request.GetResponse();

            StringBuilder respBody = new StringBuilder();
            byte[] buffer = new byte[8192];
            Stream stream;
            stream = response.GetResponseStream();
            int count = 0;
            do
            {
                count = stream.Read(buffer, 0, buffer.Length);
                if (count != 0)
                    respBody.Append(Encoding.UTF8.GetString(buffer, 0, count));
            }
            while (count > 0);
            string responseText = respBody.ToString();

            return responseText;
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
    public class configModel
    {
        public string productType { get; set; }
        public int duration { get; set; }
        public int[] resolution { get; set; }

    }

    public class sourceModel
    {
        public List<structsModel> structs { get; set; }
    }

    public class structsModel
    {
        public string type { get; set; }
        public string text { get; set; }
        public mediaSourceModel mediaSource { get; set; }

    }
    public class mediaSourceModel
    {
        public string type { get; set; }
        public string url { get; set; }

    }

    public class BDMode
    {
        public BDDataMode data { get; set; }
        public string log_id { get; set; }

    }
    public class BDDataMode
    {
        public string jobId { get; set; }
        public string id { get; set; }

    }


}
