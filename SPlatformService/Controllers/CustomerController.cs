using Aop.Api;
using Aop.Api.Domain;
using Aop.Api.Request;
using Aop.Api.Response;
using CoreFramework.DAO;
using CoreFramework.VO;
using SPlatformService.Models;
using SPlatformService.ThirdLogin;
using SPlatformService.TokenMange;
using SPLibrary.BussinessManagement.BO;
using SPLibrary.CoreFramework.BO;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.CustomerManagement.VO;
using SPLibrary.ProjectManagement.BO;
using SPLibrary.ProjectManagement.VO;
using SPLibrary.UserManagement.VO;
using SPLibrary.RequireManagement.BO;
using SPLibrary.RequireManagement.VO;
using SPLibrary.WebConfigInfo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using WebUI.Common;
using System.Data;
using System.Data.Odbc;
using System.Security.Cryptography;
using System.IO;
using BroadSky.WeChatAppDecrypt;
using System.Runtime.Serialization.Json;
using SPLibrary.CoreFramework;
using System.Drawing.Imaging;
using SPLibrary.CoreFramework.Logging.BO;
using SPLibrary.BusinessCardManagement.BO;
using SPLibrary.BusinessCardManagement.VO;
using Newtonsoft.Json.Linq;

namespace SPlatformService.Controllers
{
    /// <summary>
    /// 会员、销售、雇主 API
    /// </summary>
    [RoutePrefix("SPWebAPI/Customer")]
    [TokenProjector]
    public class CustomerController : ApiController
    {
        /// <summary>
        /// 验证账号密码，匿名
        /// </summary>
        /// <param name="loginName">账号</param>
        /// <param name="password">密码</param>
        /// <returns>Result Object，成功：Flag = 1</returns>
        [Route("ValidCustomerAccount"), HttpGet, Anonymous]
        public ResultObject ValidCustomerAccount(string loginName, string password)
        {
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            password = Utilities.GetMD5(password);
            CustomerVO uVO = uBO.FindCustomerByLoginInfo(loginName, password);

            CustomerLoginHistoryVO clHistoryVO = new CustomerLoginHistoryVO();
            clHistoryVO.LoginAt = DateTime.Now;
            clHistoryVO.Status = true;
            clHistoryVO.LoginOS = HttpContext.Current.Request.Browser.Platform;
            clHistoryVO.LoginIP = HttpContext.Current.Request.UserHostAddress;
            clHistoryVO.LoginBrowser = HttpContext.Current.Request.Browser.Browser;

            if (uVO != null)
            {
                CustomerViewVO uvVO = uBO.FindById(uVO.CustomerId);
                string token = CacheManager.TokenInsert(uvVO.CustomerId);
                CustomerLoginModel ulm = new CustomerLoginModel();
                ulm.Customer = uvVO;
                ulm.Token = token;

                //记录登录信息               
                clHistoryVO.CustomerId = uvVO.CustomerId;
                uBO.AddCustomerLoginHistory(clHistoryVO);

                //通过认证，IM注册，存在则不添加，不存在则添加
                try
                {
                    /*
                    IMBO imBO = new IMBO(new CustomerProfile());
                    imBO.RegisterIMUser(uvVO.CustomerId, uvVO.CustomerCode, "$" + uvVO.CustomerCode, uvVO.CustomerName);
                    */
                }
                catch
                {

                }
                

                return new ResultObject() { Flag = 1, Message = "验证成功!", Result = ulm };
            }
            else
            {
                CustomerVO vo = uBO.FindByParams("CustomerAccount = @CustomerAccount", new object[] { DbHelper.CreateParameter("@CustomerAccount", loginName) });
                if (vo != null)
                {
                    clHistoryVO.CustomerId = vo.CustomerId;
                    clHistoryVO.Status = false;
                    uBO.AddCustomerLoginHistory(clHistoryVO);
                }
                else
                {
                    vo = uBO.FindByParams("Phone = @Phone", new object[] { DbHelper.CreateParameter("@Phone", loginName) });
                    if (vo != null)
                    {
                        clHistoryVO.CustomerId = vo.CustomerId;
                        clHistoryVO.Status = false;
                        uBO.AddCustomerLoginHistory(clHistoryVO);
                    }
                    else
                    {
                        clHistoryVO.Status = false;
                        uBO.AddCustomerLoginHistory(clHistoryVO);
                    }
                }
                return new ResultObject() { Flag = 0, Message = "验证失败!", Result = null };
            }

        }

        /// <summary>
        /// 根据OpenID及第三方类型查询会员信息，有则返回会员信息，否则需要填写手机号码完成注册。
        /// </summary>
        /// <param name="openId">第三方授权返回的OpenID</param>
        /// <param name="loginType">授权类型</param>
        /// <returns></returns>
        [Route("GetCustomerByOpenID"), HttpGet, Anonymous]
        public ResultObject GetCustomerByOpenID(string openId, string loginType)
        {
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            CustomerViewVO customerVO = uBO.FindCustomerByOpenId(openId, loginType);
            if (customerVO != null)
            {
                CustomerLoginHistoryVO clHistoryVO = new CustomerLoginHistoryVO();
                clHistoryVO.LoginAt = DateTime.Now;
                clHistoryVO.Status = true;
                clHistoryVO.LoginOS = HttpContext.Current.Request.Browser.Platform;
                clHistoryVO.LoginIP = HttpContext.Current.Request.UserHostAddress;
                clHistoryVO.LoginBrowser = HttpContext.Current.Request.Browser.Browser;

                string token = CacheManager.TokenInsert(customerVO.CustomerId);
                CustomerLoginModel ulm = new CustomerLoginModel();
                ulm.Customer = customerVO;
                ulm.Token = token;

                //记录登录信息               
                clHistoryVO.CustomerId = customerVO.CustomerId;
                uBO.AddCustomerLoginHistory(clHistoryVO);

                return new ResultObject() { Flag = 1, Message = "验证成功!", Result = ulm };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "尚未注册!", Result = null };
            }
        }

        /// <summary>
        /// 根据OpenID及第三方类型查询会员信息，有则返回会员信息，否则需要填写手机号码完成注册。
        /// </summary>
        /// <param name="openId">第三方授权返回的OpenID</param>
        /// <param name="UnionID">微信开放平台唯一标识</param>
        /// <param name="loginType">授权类型</param>
        /// <returns></returns>
        [Route("GetCustomerByOpenID"), HttpGet, Anonymous]
        public ResultObject GetCustomerByOpenID(string openId, string UnionID, string loginType)
        {
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            if (UnionID == "undefined") {
                UnionID = "";
            }
            CustomerViewVO customerVO = uBO.FindCustomerByOpenId(openId, UnionID, loginType);
            if (customerVO != null)
            {
                CustomerLoginHistoryVO clHistoryVO = new CustomerLoginHistoryVO();
                clHistoryVO.LoginAt = DateTime.Now;
                clHistoryVO.Status = true;
                clHistoryVO.LoginOS = HttpContext.Current.Request.Browser.Platform;
                clHistoryVO.LoginIP = HttpContext.Current.Request.UserHostAddress;
                clHistoryVO.LoginBrowser = HttpContext.Current.Request.Browser.Browser;

                string token = CacheManager.TokenInsert(customerVO.CustomerId);
                CustomerLoginModel ulm = new CustomerLoginModel();
                ulm.Customer = customerVO;
                ulm.Token = token;

                //记录登录信息               
                clHistoryVO.CustomerId = customerVO.CustomerId;
                uBO.AddCustomerLoginHistory(clHistoryVO);

                return new ResultObject() { Flag = 1, Message = "验证成功!", Result = ulm };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "尚未注册!", Result = null };
            }
        }

        /// <summary>
        /// 添加第三方及会员匹配信息
        /// </summary>
        /// <param name="customerMatchVO">匹配VO</param>
        /// <returns></returns>
        [Route("UpdateCustomerMatch"), HttpPost, Anonymous]
        public ResultObject UpdateCustomerMatch([FromBody] CustomerMatchVO customerMatchVO)
        {
            CustomerProfile uProfile = new CustomerProfile();
            if (customerMatchVO != null)
            {
                if(customerMatchVO.UnionID == "undefined") {
                    customerMatchVO.UnionID = "";
                }

                CustomerBO uBO = new CustomerBO(uProfile);
                if (customerMatchVO.CustomerId < 1)
                {
                    return new ResultObject() { Flag = 0, Message = "CustomerId不存在!", Result = null };
                }
                int customerId = uBO.AddCustomerMatch(customerMatchVO);
                if (customerId > 0)
                {
                    //发放乐币奖励
                    //if(CacheSystemConfig.GetSystemConfig().zxbRegistered>0)
                        //uBO.ZXBAddrequire(customerId, CacheSystemConfig.GetSystemConfig().zxbRegistered, CacheSystemConfig.GetSystemConfig().zxbRegistered_text,1);
                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = customerId };
                }
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };

            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
        }

        /// <summary>
        /// 获取第三方URL
        /// </summary>
        /// <param name="returnURL">回调URL</param>
        /// <param name="loginType">授权类型</param>
        /// <returns></returns>
        [Route("GetThirdPartURL"), HttpGet, Anonymous]
        public ResultObject GetThirdPartURL(string loginType, string returnURL)
        {
            string url = "";
            switch (loginType)
            {
                case "1":
                    url = new QQLogin().Authorize(returnURL);
                    break;
                case "2":
                    url = new WXLogin().GetWeiXinCode(1,returnURL);
                    break;
                case "3":
                    url = new WXLogin().GetWeiXinCode(2,returnURL);
                    break;
            }
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = url };
        }

        /// <summary>
        /// 获取微信订阅号里面的文章
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="pagecount">每页总数</param>
        /// <param name="type">从哪个公众号获取：1：社交资源联盟 2：乐聊名片</param>
        /// <returns></returns>
        [Route("GetwxDYH"), HttpGet, Anonymous]
        public ResultObject GetwxDYH(int page,int pagecount,int type=1)
        {
            string url;
            string appid = new WXLogin().AppID_gzh;
            string secret = new WXLogin().AppSecret_gzh;

            if (type == 2)
            {
                appid = new WXLogin().AppID_leliao;
                secret = new WXLogin().AppSecret_leliao;
            }

            url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid="+ appid + "&secret="+ secret;

            string jsonStr = HttpHelper.HtmlFromUrlGet(url);
            var result = new WeiXinAccessTokenResultDYH();
            if (jsonStr.Contains("errcode"))
            {
                var errorResult = JsonConvert.DeserializeObject<WeiXinHelper.WeiXinErrorMsg>(jsonStr);
                result.ErrorResult = errorResult;
                result.Result = false;
            }
            else
            {
                var model = JsonConvert.DeserializeObject<WeiXinAccessTokenModelDYH>(jsonStr);
                result.SuccessResult = model;
                result.Result = true;
            }

            string data ="{\"type\":\"news\",\"offset\":"+ page + ",\"count\":"+ pagecount + "}";

            string Str = HttpHelper.HtmlFromUrlPost("https://api.weixin.qq.com/cgi-bin/material/batchget_material?access_token=" + result.SuccessResult.access_token, data);
            var Strmodel = JsonConvert.DeserializeObject<WeiXinbatchget_material>(Str);
            for (int i=0;i< Strmodel.item.Length;i++)
            {
                for (int j = 0; j < Strmodel.item[i].content.news_item.Length; j++) {
                    Strmodel.item[i].content.news_item[j].content = "";
                }
            }
            

            if (result.Result)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = Strmodel };
            }
            else {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 根据id获取微信订阅号里面的文章
        /// </summary>
        /// <param name="media_id">media_id</param>
        /// <param name="type">从哪个公众号获取：1：社交资源联盟 2：乐聊名片</param>
        /// <returns></returns>
        [Route("get_material"), HttpGet, Anonymous]
        public ResultObject get_material(string media_id, int type = 1)
        {
            string url;
            string appid = new WXLogin().AppID_gzh;
            string secret = new WXLogin().AppSecret_gzh;

            if (type == 2)
            {
                appid = new WXLogin().AppID_leliao;
                secret = new WXLogin().AppSecret_leliao;
            }

            url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid="+ appid + "&secret="+ secret;

            string jsonStr = HttpHelper.HtmlFromUrlGet(url);
            var result = new WeiXinAccessTokenResultDYH();
            if (jsonStr.Contains("errcode"))
            {
                var errorResult = JsonConvert.DeserializeObject<WeiXinHelper.WeiXinErrorMsg>(jsonStr);
                result.ErrorResult = errorResult;
                result.Result = false;
            }
            else
            {
                var model = JsonConvert.DeserializeObject<WeiXinAccessTokenModelDYH>(jsonStr);
                result.SuccessResult = model;
                result.Result = true;
            }

            string data = "{\"media_id\":\""+ media_id + "\"}";

            string Str = HttpHelper.HtmlFromUrlPost("https://api.weixin.qq.com/cgi-bin/material/get_material?access_token=" + result.SuccessResult.access_token, data);
            var Strmodel = JsonConvert.DeserializeObject<WeiXinbatchget_material_text>(Str);
            if (result.Result)
            {
                CardBO cBO = new CardBO(new CustomerProfile());
                try
                {
                    Strmodel.news_item[0].thumb_url = cBO.downloadImages(Strmodel.news_item[0].thumb_url);
                    Strmodel.news_item[0].content = cBO.downloadwxImages(Strmodel.news_item[0].content);
                }
                catch
                {

                }


                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = Strmodel };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取推广网站文章
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="pagecount">每页总数</param>
        /// <returns></returns>
        [Route("get_WebNews"), HttpGet, Anonymous]
        public ResultObject get_WebNews(int page, int pagecount)
        {
            string Str = HttpHelper.HtmlFromUrlGet("http://zhongxiaole.ltd/news_wxapp.asp?QType=News&page="+ page + "&ItemCount="+ pagecount+ "&sortid=33");
            var Strmodel = JsonConvert.DeserializeObject<WeiXinbatchget_material>(Str);
            for (int i = 0; i < Strmodel.item.Length; i++)
            {
                for (int j = 0; j < Strmodel.item[i].content.news_item.Length; j++)
                {
                    string thumb_url = Strmodel.item[i].content.news_item[j].thumb_url;
                    Strmodel.item[i].content.news_item[j].thumb_url = thumb_url.Replace("/UpFile", "http://zhongxiaole.ltd/UpFile");
                }
            }
            if (Strmodel.item.Length>0)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = Strmodel };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }


        /// <summary>
        /// 根据id获取推广网站文章
        /// </summary>
        /// <param name="media_id">media_id</param>
        /// <returns></returns>
        [Route("get_WebNewsShow"), HttpGet, Anonymous]
        public ResultObject get_WebNewsShow(string media_id)
        {
            string Str = HttpHelper.HtmlFromUrlGet("http://zhongxiaole.ltd/news_show_wxapp.asp?id=" + media_id);
            if (Str!="")
            {
                var Strmodel = JsonConvert.DeserializeObject<WeiXinbatchget_material_text>(Str);

                string content = Strmodel.news_item[0].content;
                content = content.Replace("../../../UpFile", "http://zhongxiaole.ltd/UpFile");
                content = content.Replace("<img/>", "");
                Strmodel.news_item[0].content = content.Replace("<img />", "");
                string thumb_url = Strmodel.news_item[0].thumb_url;
                Strmodel.news_item[0].thumb_url= thumb_url.Replace("/UpFile", "http://zhongxiaole.ltd/UpFile");
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = Strmodel };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取第三方用户信息
        /// </summary>
        /// <param name="code">第一次拉手返回Code</param>
        /// <param name="loginType">授权类型</param>
        /// <param name="returnURL">回调URL</param>
        /// <param name="state"></param>
        /// <returns></returns>
        [Route("GetThirdPartUserInfo"), HttpGet, Anonymous]
        public ResultObject GetThirdPartUserInfo(string loginType, string code, string returnURL,string state)
        {
            User_info ui = new User_info();
            switch (loginType)
            {
                case "1":
                    ui = new QQLogin().Get_User(code, returnURL);
                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = ui };
                    
                case "2":
                    {
                        WeiXinAccessTokenResult modelResult = new WeiXinAccessTokenResult();
                        WeiXinHelper.WeiXinUserInfoResult userInfo = new WeiXinHelper.WeiXinUserInfoResult();
                        try
                        {
                            var weixinOAuth = new WXLogin();
                            //获取微信的Access_Token（第二次微信握手）
                            modelResult = weixinOAuth.GetWeiXinAccessToken(Convert.ToInt32(state),code);
                            //获取微信的用户信息(第三次微信握手)
                            userInfo = weixinOAuth.GetWeiXinUserInfo(modelResult.SuccessResult.access_token, modelResult.SuccessResult.openid);
                            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = userInfo };
                        }
                        catch (Exception err)
                        {
                            LogBO _log = new LogBO(typeof(CustomerController));
                            string strErrorMsg = "Message:" + err.Message.ToString() + "\r\n  Stack :" + err.StackTrace + " \r\n Source :" + err.Source + " \r\n modelResult :" + Newtonsoft.Json.JsonConvert.SerializeObject(modelResult);
                            _log.Error(strErrorMsg);
                            throw err;
                        }
                    }                  
            }
            return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
        }


        /// <summary>
        /// 小程序获取微信用户信息(众销乐)
        /// </summary>
        /// <param name="code">第一次拉手返回Code</param>
        /// <returns></returns>
        [Route("GetMiniprogramUserInfo"), HttpGet, Anonymous]
        public ResultObject GetMiniprogramUserInfo(string code)
        {
            try
            {
                string url = "https://api.weixin.qq.com/sns/jscode2session?appid=wxd90e86e2ec343eae&secret=58ce21e763870071474c8ee531013e7e&js_code="+ code + "&grant_type=authorization_code";
                string jsonStr = HttpHelper.HtmlFromUrlGet(url);
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = jsonStr };
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }
        
        /// <summary>
        /// 小程序获取微信用户信息(销售总监)
        /// </summary>
        /// <param name="code">第一次拉手返回Code</param>
        /// <returns></returns>
        [Route("GetMiniprogramUserInfo2"), HttpGet, Anonymous]
        public ResultObject GetMiniprogramUserInfo2(string code)
        {
            try
            {
                string url = "https://api.weixin.qq.com/sns/jscode2session?appid=wxa1f8655e6c7863ee&secret=c472eb8e0306a9603f7b0df16c1e8548&js_code=" + code + "&grant_type=authorization_code";
                string jsonStr = HttpHelper.HtmlFromUrlGet(url);
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = jsonStr };
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 小程序获取微信用户信息(省情)
        /// </summary>
        /// <param name="code">第一次拉手返回Code</param>
        /// <returns></returns>
        [Route("GetMiniprogramUserInfo3"), HttpGet, Anonymous]
        public ResultObject GetMiniprogramUserInfo3(string code)
        {
            try
            {
                string url = "https://api.weixin.qq.com/sns/jscode2session?appid=wxdad6b6abb8f6386b&secret=5624f292d4246bebda328d69b176dfba&js_code=" + code + "&grant_type=authorization_code";
                string jsonStr = HttpHelper.HtmlFromUrlGet(url);
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = jsonStr };
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 小程序获取微信用户信息(名片小程序)
        /// </summary>
        /// <param name="wxUserInfoVO">微信小程序用户信息</param>
        /// <param name="code">第一次拉手返回Code</param>
        /// <returns></returns>
        [Route("GetMiniprogramUserInfoCard"), HttpPost, Anonymous]
        public ResultObject GetMiniprogramUserInfoCard([FromBody] wxUserInfoVO wxUserInfoVO, string code, string originType = "", int originID = 0, int isNewLogin = 0)
        {
            try
            {
                string url = "https://api.weixin.qq.com/sns/jscode2session?appid=wx584477316879d7e9&secret=1e08e16c76895ff8584b591d447b754e&js_code=" + code + "&grant_type=authorization_code";
                string jsonStr = HttpHelper.HtmlFromUrlGet(url);
                var serializer = new DataContractJsonSerializer(typeof(OpenIdAndSessionKey));
                var mStream = new MemoryStream(Encoding.Default.GetBytes(jsonStr));
                OpenIdAndSessionKey readConfig = (OpenIdAndSessionKey)serializer.ReadObject(mStream);

                WeChatAppDecrypt un = new WeChatAppDecrypt("wx584477316879d7e9", "1e08e16c76895ff8584b591d447b754e");


                wxUserInfo wxUser = new wxUserInfo();
                string openId = readConfig.openid;
                string unionId = readConfig.unionid;
                if (isNewLogin == 0)
                {
                    if (un.VaildateUserInfo(wxUserInfoVO.rawData, wxUserInfoVO.signature, readConfig.session_key))
                    {
                        WechatUserInfo wui = un.Decrypt(wxUserInfoVO.encryptedData, wxUserInfoVO.iv, readConfig.session_key);
                        wxUser.nickName = wui.nickName;
                        //wxUser.gender = wui.gender == "1"?1:0;
                        wxUser.avatarUrl = wui.avatarUrl;

                        openId = wui.openId;
                        unionId = wui.unionId;
                    }
                    else
                    {
                        return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
                    }
                }else
                {
                    wxUser = wxUserInfoVO.userInfo;
                }

                CardBO CardBO = new CardBO(new CustomerProfile());
                CustomerBO uBO = new CustomerBO(new CustomerProfile());
                CustomerViewVO customerVO = uBO.FindCustomerByOpenId(openId, unionId, "2");
                if (customerVO != null)
                {
                    CustomerLoginHistoryVO clHistoryVO = new CustomerLoginHistoryVO();
                    clHistoryVO.LoginAt = DateTime.Now;
                    clHistoryVO.Status = true;
                    clHistoryVO.LoginOS = HttpContext.Current.Request.Browser.Platform;
                    clHistoryVO.LoginIP = HttpContext.Current.Request.UserHostAddress;
                    clHistoryVO.LoginBrowser = "乐聊名片小程序";

                    /*
                   try
                   {

                       string jsonstr = HttpHelper.HtmlFromUrlGet("http://apis.map.qq.com/ws/location/v1/ip?key=QHBBZ-ZDAWI-TRVGE-5GWEO-PHEKO-Q5BXC&ip=" + clHistoryVO.LoginIP);
                       JObject jo = (JObject)JsonConvert.DeserializeObject(jsonstr);
                       clHistoryVO.Nation = jo["result"]["ad_info"]["nation"].ToString();
                       clHistoryVO.Province = jo["result"]["ad_info"]["province"].ToString();
                       clHistoryVO.City = jo["result"]["ad_info"]["city"].ToString();
                       clHistoryVO.District = jo["result"]["ad_info"]["district"].ToString();

                    }
                    catch
                    {

                    } */
                    //判断会员VIP是否过期
                    if (customerVO.isVip)
                    {
                        if (customerVO.ExpirationAt <= DateTime.Now)
                        {
                            CustomerVO cVO = new CustomerVO();
                            cVO.CustomerId = customerVO.CustomerId;
                            cVO.isVip = false;
                            cVO.ExpirationSendStatus = 0;
                            uBO.Update(cVO);
                            customerVO.isVip = false;

                            //发送到期提醒
                            CardBO.AddCardMessage("您的乐聊名片会员特权已到期", customerVO.CustomerId, "会员特权", "/pages/MyCenter/VipCenter/VipCenter");
                        }
                    }
                    customerVO.isVip = CardBO.isVIP(customerVO.CustomerId);

                    string token = CacheManager.TokenInsert(customerVO.CustomerId);
                    CustomerLoginModel ulm = new CustomerLoginModel();
                    customerVO.Password = "";
                    ulm.Customer = customerVO;
                    ulm.Token = token;

                    
                    BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
                    //判断是否有企业名片的个人信息
                    PersonalVO pVO = cBO.FindPersonalByCustomerId(customerVO.CustomerId);
                    if (pVO != null)
                    {
                        
                        List<CardDataVO> cVO = CardBO.FindCardByCustomerId(customerVO.CustomerId);
                        if (cVO.Count <= 0)
                        {
                            CardDataVO CardDataVO = new CardDataVO();

                            CardDataVO.CardID = 0;
                            CardDataVO.Name = pVO.Name;
                            CardDataVO.Headimg = pVO.Headimg;
                            CardDataVO.Phone = pVO.Phone;
                            CardDataVO.Position = pVO.Position;
                            CardDataVO.WeChat = pVO.WeChat;
                            CardDataVO.Email = pVO.Email;
                            CardDataVO.Details = pVO.Details;
                            CardDataVO.Address = pVO.Address;
                            CardDataVO.Business = pVO.Business;
                            CardDataVO.latitude = pVO.latitude;
                            CardDataVO.longitude = pVO.longitude;
                            CardDataVO.Tel = pVO.Tel;

                            CardDataVO.CreatedAt = DateTime.Now;
                            CardDataVO.Status = 1;//0:禁用，1:启用
                            CardDataVO.CustomerId = customerVO.CustomerId;
                            CardDataVO.Collection = 0;
                            CardDataVO.ReadCount = 0;
                            CardDataVO.Forward = 0;
                            CardDataVO.isBusinessCard = 1;

                            if (pVO.BusinessID > 0)
                            {
                                CardDataVO.CorporateName = cBO.FindBusinessCardById(pVO.BusinessID).BusinessName;
                            }

                            int CardId = CardBO.AddCard(CardDataVO);
                        }
                    }

                    //记录登录信息               
                    clHistoryVO.CustomerId = customerVO.CustomerId;
                    uBO.AddCustomerLoginHistory(clHistoryVO);

                    //更新以往访问记录
                    CardBO CdBO = new CardBO(new CustomerProfile());
                    CdBO.UpdateAccessrecordsCustomerId(customerVO.CustomerId, openId);

                    return new ResultObject() { Flag = 1, Message = "登录成功!", Result = ulm, Subsidiary = openId };
                }
                else if (openId != "" || unionId != "")
                {

                    CustomerVO cVO = new CustomerVO();
                    cVO.CustomerCode = uBO.GetCustomerCode();
                    string password = Utilities.MakePassword(8);
                    cVO.Password = Utilities.GetMD5(password);
                    cVO.Status = 1;
                    cVO.CreatedAt = DateTime.Now;
                    if (wxUser.nickName.Length > 0)
                    {
                        cVO.CustomerName = wxUser.nickName;
                    }
                    cVO.Sex = wxUser.gender == 1;
                    cVO.Leliao = true;
                    cVO.originType = originType;
                    cVO.originID = originID;

                    //识别来源会员

                    //新用户赠送一个月会员
                    /*
                    cVO.isVip = true;
                    cVO.ExpirationAt = DateTime.Now.AddMonths(1);
                    */
                    try
                    {
                        if (cVO.CustomerName != "微信用户")
                        {
                            //下载头像
                            string folder = "/UploadFolder/Image/" + DateTime.Now.ToString("yyyyMM") + "/";
                            string imgPath = "";
                            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".jpg";
                            //可以修改为网络路径
                            string localPath = ConfigInfo.Instance.UploadFolder + folder;
                            if (!Directory.Exists(localPath))
                            {
                                Directory.CreateDirectory(localPath);
                            }
                            string PhysicalPath = localPath + newFileName;
                            imgPath = ConfigInfo.Instance.APIURL + folder + newFileName;
                            WebRequest wreq = WebRequest.Create(wxUser.avatarUrl);
                            HttpWebResponse wresp = (HttpWebResponse)wreq.GetResponse();
                            Stream s = wresp.GetResponseStream();
                            System.Drawing.Image img;
                            img = System.Drawing.Image.FromStream(s);
                            img.Save(PhysicalPath, ImageFormat.Jpeg);   //保存

                            cVO.HeaderLogo = imgPath;
                        }
                    }
                    catch
                    {

                    }


                    int customerId = uBO.Add(cVO);
                    if (customerId > 0)
                    {
                        CustomerMatchVO customerMatchVO = new CustomerMatchVO();
                        customerMatchVO.OpenId = openId;
                        customerMatchVO.UnionID = unionId;
                        customerMatchVO.CustomerId = customerId;
                        customerMatchVO.MatchType = "2";
                        int customerId2 = uBO.AddCustomerMatch(customerMatchVO);
                        if (customerId2 > 0)
                        {
                            CustomerViewVO customerVO2 = uBO.FindCustomerByOpenId(openId, unionId, "2");
                            if (customerVO2 != null)
                            {
                                CustomerLoginHistoryVO clHistoryVO = new CustomerLoginHistoryVO();
                                clHistoryVO.LoginAt = DateTime.Now;
                                clHistoryVO.Status = true;
                                clHistoryVO.LoginOS = HttpContext.Current.Request.Browser.Platform;
                                clHistoryVO.LoginIP = HttpContext.Current.Request.UserHostAddress;
                                clHistoryVO.LoginBrowser = "乐聊名片小程序";
                                /*
                                try
                                {
                                    string jsonstr = HttpHelper.HtmlFromUrlGet("http://apis.map.qq.com/ws/location/v1/ip?key=QHBBZ-ZDAWI-TRVGE-5GWEO-PHEKO-Q5BXC&ip=" + clHistoryVO.LoginIP);
                                    JObject jo = (JObject)JsonConvert.DeserializeObject(jsonstr);
                                    clHistoryVO.Nation = jo["result"]["ad_info"]["nation"].ToString();
                                    clHistoryVO.Province = jo["result"]["ad_info"]["province"].ToString();
                                    clHistoryVO.City = jo["result"]["ad_info"]["city"].ToString();
                                    clHistoryVO.District = jo["result"]["ad_info"]["district"].ToString();
                                }
                                catch
                                {

                                }*/

                                string token = CacheManager.TokenInsert(customerVO2.CustomerId);
                                CustomerLoginModel ulm = new CustomerLoginModel();
                                customerVO2.Password = "";
                                ulm.Customer = customerVO2;
                                ulm.Token = token;

                                //注册一张个人版名片
                                /*
                                CardBO CardBO = new CardBO(new CustomerProfile());
                                List<CardDataVO> cVOList = CardBO.FindCardByCustomerId(customerVO2.CustomerId);
                                if (cVOList.Count <= 0)
                                {
                                    CardDataVO CardDataVO = new CardDataVO();

                                    CardDataVO.CardID = 0;
                                    CardDataVO.Name = customerVO2.CustomerName;
                                    CardDataVO.Headimg = customerVO2.HeaderLogo;
                                    CardDataVO.CreatedAt = DateTime.Now;
                                    CardDataVO.CustomerId = customerVO2.CustomerId;
                                    CardDataVO.Collection = 0;
                                    CardDataVO.ReadCount = 0;
                                    CardDataVO.Forward = 0;

                                    int CardId = CardBO.AddCard(CardDataVO);
                                }*/

                                //记录登录信息               
                                clHistoryVO.CustomerId = customerVO2.CustomerId;
                                uBO.AddCustomerLoginHistory(clHistoryVO);

                                //更新以往访问记录
                                CardBO cBO = new CardBO(new CustomerProfile());




                                cBO.UpdateAccessrecordsCustomerId(customerVO2.CustomerId,openId);

                                return new ResultObject() { Flag = 1, Message = "注册成功!", Result = ulm, Subsidiary = openId };
                            }
                        }
                    }

                    return new ResultObject() { Flag = 0, Message = "注册失败!", Result = null };
                }else
                {
                    return new ResultObject() { Flag = 0, Message = "注册失败!", Result = null };
                }
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CustomerController));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 小程序获取微信用户信息(引流王)
        /// </summary>
        /// <param name="wxUserInfoVO">微信小程序用户信息</param>
        /// <param name="code">第一次拉手返回Code</param>
        /// <returns></returns>
        [Route("GetMiniprogramUserInfoByYLW"), HttpPost, Anonymous]
        public ResultObject GetMiniprogramUserInfoByYLW([FromBody] wxUserInfoVO wxUserInfoVO, string code, string originType = "", int originID = 0, int isNewLogin = 0)
        {
            try
            {
                string url = "https://api.weixin.qq.com/sns/jscode2session?appid=wxbe6347ce9f00fd0b&secret=936b0905c776a207174039336a217bcb&js_code=" + code + "&grant_type=authorization_code";
                string jsonStr = HttpHelper.HtmlFromUrlGet(url);
                var serializer = new DataContractJsonSerializer(typeof(OpenIdAndSessionKey));
                var mStream = new MemoryStream(Encoding.Default.GetBytes(jsonStr));
                OpenIdAndSessionKey readConfig = (OpenIdAndSessionKey)serializer.ReadObject(mStream);

                WeChatAppDecrypt un = new WeChatAppDecrypt("wxbe6347ce9f00fd0b", "936b0905c776a207174039336a217bcb");

                wxUserInfo wxUser = new wxUserInfo();
                string openId = readConfig.openid;
                string unionId = readConfig.unionid;
                if (isNewLogin == 0)
                {
                    if (un.VaildateUserInfo(wxUserInfoVO.rawData, wxUserInfoVO.signature, readConfig.session_key))
                    {
                        WechatUserInfo wui = un.Decrypt(wxUserInfoVO.encryptedData, wxUserInfoVO.iv, readConfig.session_key);
                        wxUser.nickName = wui.nickName;
                        //wxUser.gender = wui.gender == "1" ? 1 : 0;
                        wxUser.avatarUrl = wui.avatarUrl;

                        openId = wui.openId;
                        unionId = wui.unionId;
                    }
                    else
                    {
                        return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
                    }
                }
                else
                {
                    wxUser = wxUserInfoVO.userInfo;
                }
                CardBO CardBO = new CardBO(new CustomerProfile());
                CustomerBO uBO = new CustomerBO(new CustomerProfile());
                CustomerViewVO customerVO = uBO.FindCustomerByOpenId(openId, unionId, "2");
                if (customerVO != null)
                {
                    CustomerLoginHistoryVO clHistoryVO = new CustomerLoginHistoryVO();
                    clHistoryVO.LoginAt = DateTime.Now;
                    clHistoryVO.Status = true;
                    clHistoryVO.LoginOS = HttpContext.Current.Request.Browser.Platform;
                    clHistoryVO.LoginIP = HttpContext.Current.Request.UserHostAddress;
                    clHistoryVO.LoginBrowser = "引流王";

                    /*
                   try
                   {

                       string jsonstr = HttpHelper.HtmlFromUrlGet("http://apis.map.qq.com/ws/location/v1/ip?key=QHBBZ-ZDAWI-TRVGE-5GWEO-PHEKO-Q5BXC&ip=" + clHistoryVO.LoginIP);
                       JObject jo = (JObject)JsonConvert.DeserializeObject(jsonstr);
                       clHistoryVO.Nation = jo["result"]["ad_info"]["nation"].ToString();
                       clHistoryVO.Province = jo["result"]["ad_info"]["province"].ToString();
                       clHistoryVO.City = jo["result"]["ad_info"]["city"].ToString();
                       clHistoryVO.District = jo["result"]["ad_info"]["district"].ToString();

                    }
                    catch
                    {

                    } */
                    //判断会员VIP是否过期
                    if (customerVO.isVip)
                    {
                        if (customerVO.ExpirationAt <= DateTime.Now)
                        {
                            CustomerVO cVO = new CustomerVO();
                            cVO.CustomerId = customerVO.CustomerId;
                            cVO.isVip = false;
                            cVO.ExpirationSendStatus = 0;
                            uBO.Update(cVO);
                            customerVO.isVip = false;

                            //发送到期提醒
                            CardBO.AddCardMessage("您的乐聊名片会员特权已到期", customerVO.CustomerId, "会员特权", "/pages/MyCenter/VipCenter/VipCenter");
                        }
                    }
                    customerVO.isVip = CardBO.isVIP(customerVO.CustomerId);

                    string token = CacheManager.TokenInsert(customerVO.CustomerId);
                    CustomerLoginModel ulm = new CustomerLoginModel();
                    customerVO.Password = "";
                    ulm.Customer = customerVO;
                    ulm.Token = token;

                    BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
                    //判断是否有企业名片的个人信息
                    PersonalVO pVO = cBO.FindPersonalByCustomerId(customerVO.CustomerId);
                    if (pVO != null)
                    {
                        List<CardDataVO> cVO = CardBO.FindCardByCustomerId(customerVO.CustomerId);
                        if (cVO.Count <= 0)
                        {
                            CardDataVO CardDataVO = new CardDataVO();

                            CardDataVO.CardID = 0;
                            CardDataVO.Name = pVO.Name;
                            CardDataVO.Headimg = pVO.Headimg;
                            CardDataVO.Phone = pVO.Phone;
                            CardDataVO.Position = pVO.Position;
                            CardDataVO.WeChat = pVO.WeChat;
                            CardDataVO.Email = pVO.Email;
                            CardDataVO.Details = pVO.Details;
                            CardDataVO.Address = pVO.Address;
                            CardDataVO.Business = pVO.Business;
                            CardDataVO.latitude = pVO.latitude;
                            CardDataVO.longitude = pVO.longitude;
                            CardDataVO.Tel = pVO.Tel;

                            CardDataVO.CreatedAt = DateTime.Now;
                            CardDataVO.Status = 1;//0:禁用，1:启用
                            CardDataVO.CustomerId = customerVO.CustomerId;
                            CardDataVO.Collection = 0;
                            CardDataVO.ReadCount = 0;
                            CardDataVO.Forward = 0;
                            CardDataVO.isBusinessCard = 1;

                            if (pVO.BusinessID > 0)
                            {
                                CardDataVO.CorporateName = cBO.FindBusinessCardById(pVO.BusinessID).BusinessName;
                            }

                            int CardId = CardBO.AddCard(CardDataVO);
                        }
                    }

                    //记录登录信息               
                    clHistoryVO.CustomerId = customerVO.CustomerId;
                    uBO.AddCustomerLoginHistory(clHistoryVO);

                    //更新以往访问记录
                    CardBO.UpdateAccessrecordsCustomerId(customerVO.CustomerId, openId);

                    return new ResultObject() { Flag = 1, Message = "登录成功!", Result = ulm, Subsidiary = openId };
                }
                else if (openId != "" || unionId != "")
                {

                    CustomerVO cVO = new CustomerVO();
                    cVO.CustomerCode = uBO.GetCustomerCode();
                    string password = Utilities.MakePassword(8);
                    cVO.Password = Utilities.GetMD5(password);
                    cVO.Status = 1;
                    cVO.CreatedAt = DateTime.Now;
                    if (wxUser.nickName.Length > 0)
                    {
                        cVO.CustomerName = wxUser.nickName;
                    }
                    cVO.Sex = wxUser.gender == 1;
                    cVO.Leliao = true;
                    cVO.originType = originType;
                    cVO.originID = originID;

                    //新用户赠送一个月会员
                    /*
                    cVO.isVip = true;
                    cVO.ExpirationAt = DateTime.Now.AddMonths(1);
                    */
                    try
                    {
                        if (cVO.CustomerName != "微信用户")
                        {
                            //下载头像
                            string folder = "/UploadFolder/Image/" + DateTime.Now.ToString("yyyyMM") + "/";
                            string imgPath = "";
                            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".jpg";
                            //可以修改为网络路径
                            string localPath = ConfigInfo.Instance.UploadFolder + folder;
                            if (!Directory.Exists(localPath))
                            {
                                Directory.CreateDirectory(localPath);
                            }
                            string PhysicalPath = localPath + newFileName;
                            imgPath = ConfigInfo.Instance.APIURL + folder + newFileName;
                            WebRequest wreq = WebRequest.Create(wxUser.avatarUrl);
                            HttpWebResponse wresp = (HttpWebResponse)wreq.GetResponse();
                            Stream s = wresp.GetResponseStream();
                            System.Drawing.Image img;
                            img = System.Drawing.Image.FromStream(s);
                            img.Save(PhysicalPath, ImageFormat.Jpeg);   //保存

                            cVO.HeaderLogo = imgPath;
                        }
                    }
                    catch
                    {

                    }


                    int customerId = uBO.Add(cVO);
                    if (customerId > 0)
                    {
                        CustomerMatchVO customerMatchVO = new CustomerMatchVO();
                        customerMatchVO.OpenId = openId;
                        customerMatchVO.UnionID = unionId;
                        customerMatchVO.CustomerId = customerId;
                        customerMatchVO.MatchType = "2";
                        int customerId2 = uBO.AddCustomerMatch(customerMatchVO);
                        if (customerId2 > 0)
                        {
                            CustomerViewVO customerVO2 = uBO.FindCustomerByOpenId(openId, unionId, "2");
                            if (customerVO2 != null)
                            {
                                CustomerLoginHistoryVO clHistoryVO = new CustomerLoginHistoryVO();
                                clHistoryVO.LoginAt = DateTime.Now;
                                clHistoryVO.Status = true;
                                clHistoryVO.LoginOS = HttpContext.Current.Request.Browser.Platform;
                                clHistoryVO.LoginIP = HttpContext.Current.Request.UserHostAddress;
                                clHistoryVO.LoginBrowser = "引流王";
                                /*
                                try
                                {
                                    string jsonstr = HttpHelper.HtmlFromUrlGet("http://apis.map.qq.com/ws/location/v1/ip?key=QHBBZ-ZDAWI-TRVGE-5GWEO-PHEKO-Q5BXC&ip=" + clHistoryVO.LoginIP);
                                    JObject jo = (JObject)JsonConvert.DeserializeObject(jsonstr);
                                    clHistoryVO.Nation = jo["result"]["ad_info"]["nation"].ToString();
                                    clHistoryVO.Province = jo["result"]["ad_info"]["province"].ToString();
                                    clHistoryVO.City = jo["result"]["ad_info"]["city"].ToString();
                                    clHistoryVO.District = jo["result"]["ad_info"]["district"].ToString();
                                }
                                catch
                                {

                                }*/

                                string token = CacheManager.TokenInsert(customerVO2.CustomerId);
                                CustomerLoginModel ulm = new CustomerLoginModel();
                                customerVO2.Password = "";
                                ulm.Customer = customerVO2;
                                ulm.Token = token;

                                //注册一张个人版名片
                                /*
                                CardBO CardBO = new CardBO(new CustomerProfile());
                                List<CardDataVO> cVOList = CardBO.FindCardByCustomerId(customerVO2.CustomerId);
                                if (cVOList.Count <= 0)
                                {
                                    CardDataVO CardDataVO = new CardDataVO();

                                    CardDataVO.CardID = 0;
                                    CardDataVO.Name = customerVO2.CustomerName;
                                    CardDataVO.Headimg = customerVO2.HeaderLogo;
                                    CardDataVO.CreatedAt = DateTime.Now;
                                    CardDataVO.CustomerId = customerVO2.CustomerId;
                                    CardDataVO.Collection = 0;
                                    CardDataVO.ReadCount = 0;
                                    CardDataVO.Forward = 0;

                                    int CardId = CardBO.AddCard(CardDataVO);
                                }*/

                                //记录登录信息               
                                clHistoryVO.CustomerId = customerVO2.CustomerId;
                                uBO.AddCustomerLoginHistory(clHistoryVO);

                                //更新以往访问记录
                                CardBO cBO = new CardBO(new CustomerProfile());
                                cBO.UpdateAccessrecordsCustomerId(customerVO2.CustomerId, openId);

                                return new ResultObject() { Flag = 1, Message = "注册成功!", Result = ulm, Subsidiary = openId };
                            }
                        }
                    }

                    return new ResultObject() { Flag = 0, Message = "注册失败!", Result = null };
                }else
                {
                    return new ResultObject() { Flag = 0, Message = "注册失败!", Result = null };
                }
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CustomerController));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 小程序获取微信用户信息(粤省情小程序)
        /// </summary>
        /// <param name="wxUserInfoVO">微信小程序用户信息</param>
        /// <param name="code">第一次拉手返回Code</param>
        /// <returns></returns>
        [Route("GetMiniprogramUserInfoByYSQ"), HttpPost, Anonymous]
        public ResultObject GetMiniprogramUserInfoByYSQ([FromBody] wxUserInfoVO wxUserInfoVO, string code, string originType = "", int originID = 0, int isNewLogin = 0)
        {
            try
            {
                string url = "https://api.weixin.qq.com/sns/jscode2session?appid=wxdad6b6abb8f6386b&secret=5624f292d4246bebda328d69b176dfba&js_code=" + code + "&grant_type=authorization_code";
                string jsonStr = HttpHelper.HtmlFromUrlGet(url);
                var serializer = new DataContractJsonSerializer(typeof(OpenIdAndSessionKey));
                var mStream = new MemoryStream(Encoding.Default.GetBytes(jsonStr));
                OpenIdAndSessionKey readConfig = (OpenIdAndSessionKey)serializer.ReadObject(mStream);

                WeChatAppDecrypt un = new WeChatAppDecrypt("wxdad6b6abb8f6386b", "5624f292d4246bebda328d69b176dfba");


                wxUserInfo wxUser = new wxUserInfo();
                string openId = readConfig.openid;
                string unionId = readConfig.unionid;
                if (isNewLogin == 0)
                {
                    if (un.VaildateUserInfo(wxUserInfoVO.rawData, wxUserInfoVO.signature, readConfig.session_key))
                    {
                        WechatUserInfo wui = un.Decrypt(wxUserInfoVO.encryptedData, wxUserInfoVO.iv, readConfig.session_key);
                        wxUser.nickName = wui.nickName;
                        //wxUser.gender = wui.gender == "1"?1:0;
                        wxUser.avatarUrl = wui.avatarUrl;

                        openId = wui.openId;
                        unionId = wui.unionId;
                    }
                    else
                    {
                        return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
                    }
                }
                else
                {
                    wxUser = wxUserInfoVO.userInfo;
                }

                CardBO CardBO = new CardBO(new CustomerProfile());
                CustomerBO uBO = new CustomerBO(new CustomerProfile());
                CustomerViewVO customerVO = uBO.FindCustomerByOpenId(openId, unionId, "2");
                if (customerVO != null)
                {
                    CustomerLoginHistoryVO clHistoryVO = new CustomerLoginHistoryVO();
                    clHistoryVO.LoginAt = DateTime.Now;
                    clHistoryVO.Status = true;
                    clHistoryVO.LoginOS = HttpContext.Current.Request.Browser.Platform;
                    clHistoryVO.LoginIP = HttpContext.Current.Request.UserHostAddress;
                    clHistoryVO.LoginBrowser = "多彩乡村 粤省情小程序";

                    /*
                   try
                   {

                       string jsonstr = HttpHelper.HtmlFromUrlGet("http://apis.map.qq.com/ws/location/v1/ip?key=QHBBZ-ZDAWI-TRVGE-5GWEO-PHEKO-Q5BXC&ip=" + clHistoryVO.LoginIP);
                       JObject jo = (JObject)JsonConvert.DeserializeObject(jsonstr);
                       clHistoryVO.Nation = jo["result"]["ad_info"]["nation"].ToString();
                       clHistoryVO.Province = jo["result"]["ad_info"]["province"].ToString();
                       clHistoryVO.City = jo["result"]["ad_info"]["city"].ToString();
                       clHistoryVO.District = jo["result"]["ad_info"]["district"].ToString();

                    }
                    catch
                    {

                    } */
                    //判断会员VIP是否过期
                    if (customerVO.isVip)
                    {
                        if (customerVO.ExpirationAt <= DateTime.Now)
                        {
                            CustomerVO cVO = new CustomerVO();
                            cVO.CustomerId = customerVO.CustomerId;
                            cVO.isVip = false;
                            cVO.ExpirationSendStatus = 0;
                            uBO.Update(cVO);
                            customerVO.isVip = false;

                            //发送到期提醒
                            CardBO.AddCardMessage("您的多彩乡村 粤省情会员特权已到期", customerVO.CustomerId, "会员特权", "/pages/MyCenter/VipCenter/VipCenter");
                        }
                    }
                    customerVO.isVip = CardBO.isVIP(customerVO.CustomerId);

                    string token = CacheManager.TokenInsert(customerVO.CustomerId);
                    CustomerLoginModel ulm = new CustomerLoginModel();
                    customerVO.Password = "";
                    ulm.Customer = customerVO;
                    ulm.Token = token;


                    BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
                    //判断是否有企业名片的个人信息
                    PersonalVO pVO = cBO.FindPersonalByCustomerId(customerVO.CustomerId);
                    if (pVO != null)
                    {

                        List<CardDataVO> cVO = CardBO.FindCardByCustomerId(customerVO.CustomerId);
                        if (cVO.Count <= 0)
                        {
                            CardDataVO CardDataVO = new CardDataVO();

                            CardDataVO.CardID = 0;
                            CardDataVO.Name = pVO.Name;
                            CardDataVO.Headimg = pVO.Headimg;
                            CardDataVO.Phone = pVO.Phone;
                            CardDataVO.Position = pVO.Position;
                            CardDataVO.WeChat = pVO.WeChat;
                            CardDataVO.Email = pVO.Email;
                            CardDataVO.Details = pVO.Details;
                            CardDataVO.Address = pVO.Address;
                            CardDataVO.Business = pVO.Business;
                            CardDataVO.latitude = pVO.latitude;
                            CardDataVO.longitude = pVO.longitude;
                            CardDataVO.Tel = pVO.Tel;

                            CardDataVO.CreatedAt = DateTime.Now;
                            CardDataVO.Status = 1;//0:禁用，1:启用
                            CardDataVO.CustomerId = customerVO.CustomerId;
                            CardDataVO.Collection = 0;
                            CardDataVO.ReadCount = 0;
                            CardDataVO.Forward = 0;
                            CardDataVO.isBusinessCard = 1;

                            if (pVO.BusinessID > 0)
                            {
                                CardDataVO.CorporateName = cBO.FindBusinessCardById(pVO.BusinessID).BusinessName;
                            }

                            int CardId = CardBO.AddCard(CardDataVO);
                        }
                    }

                    //记录登录信息               
                    clHistoryVO.CustomerId = customerVO.CustomerId;
                    uBO.AddCustomerLoginHistory(clHistoryVO);

                    //更新以往访问记录
                    CardBO CdBO = new CardBO(new CustomerProfile());
                    CdBO.UpdateAccessrecordsCustomerId(customerVO.CustomerId, openId);

                    return new ResultObject() { Flag = 1, Message = "登录成功!", Result = ulm, Subsidiary = openId };
                }
                else if (openId != "" || unionId != "")
                {

                    CustomerVO cVO = new CustomerVO();
                    cVO.CustomerCode = uBO.GetCustomerCode();
                    string password = Utilities.MakePassword(8);
                    cVO.Password = Utilities.GetMD5(password);
                    cVO.Status = 1;
                    cVO.CreatedAt = DateTime.Now;
                    if (wxUser.nickName.Length > 0)
                    {
                        cVO.CustomerName = wxUser.nickName;
                    }
                    cVO.Sex = wxUser.gender == 1;
                    cVO.Leliao = true;
                    cVO.originType = originType;
                    cVO.originID = originID;

                    //识别来源会员

                    //新用户赠送一个月会员
                    /*
                    cVO.isVip = true;
                    cVO.ExpirationAt = DateTime.Now.AddMonths(1);
                    */
                    try
                    {
                        if (cVO.CustomerName != "微信用户")
                        {
                            //下载头像
                            string folder = "/UploadFolder/Image/" + DateTime.Now.ToString("yyyyMM") + "/";
                            string imgPath = "";
                            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".jpg";
                            //可以修改为网络路径
                            string localPath = ConfigInfo.Instance.UploadFolder + folder;
                            if (!Directory.Exists(localPath))
                            {
                                Directory.CreateDirectory(localPath);
                            }
                            string PhysicalPath = localPath + newFileName;
                            imgPath = ConfigInfo.Instance.APIURL + folder + newFileName;
                            WebRequest wreq = WebRequest.Create(wxUser.avatarUrl);
                            HttpWebResponse wresp = (HttpWebResponse)wreq.GetResponse();
                            Stream s = wresp.GetResponseStream();
                            System.Drawing.Image img;
                            img = System.Drawing.Image.FromStream(s);
                            img.Save(PhysicalPath, ImageFormat.Jpeg);   //保存

                            cVO.HeaderLogo = imgPath;
                        }
                    }
                    catch
                    {

                    }


                    int customerId = uBO.Add(cVO);
                    if (customerId > 0)
                    {
                        CustomerMatchVO customerMatchVO = new CustomerMatchVO();
                        customerMatchVO.OpenId = openId;
                        customerMatchVO.UnionID = unionId;
                        customerMatchVO.CustomerId = customerId;
                        customerMatchVO.MatchType = "2";
                        int customerId2 = uBO.AddCustomerMatch(customerMatchVO);
                        if (customerId2 > 0)
                        {
                            CustomerViewVO customerVO2 = uBO.FindCustomerByOpenId(openId, unionId, "2");
                            if (customerVO2 != null)
                            {
                                CustomerLoginHistoryVO clHistoryVO = new CustomerLoginHistoryVO();
                                clHistoryVO.LoginAt = DateTime.Now;
                                clHistoryVO.Status = true;
                                clHistoryVO.LoginOS = HttpContext.Current.Request.Browser.Platform;
                                clHistoryVO.LoginIP = HttpContext.Current.Request.UserHostAddress;
                                clHistoryVO.LoginBrowser = "多彩乡村 粤省情小程序";
                                /*
                                try
                                {
                                    string jsonstr = HttpHelper.HtmlFromUrlGet("http://apis.map.qq.com/ws/location/v1/ip?key=QHBBZ-ZDAWI-TRVGE-5GWEO-PHEKO-Q5BXC&ip=" + clHistoryVO.LoginIP);
                                    JObject jo = (JObject)JsonConvert.DeserializeObject(jsonstr);
                                    clHistoryVO.Nation = jo["result"]["ad_info"]["nation"].ToString();
                                    clHistoryVO.Province = jo["result"]["ad_info"]["province"].ToString();
                                    clHistoryVO.City = jo["result"]["ad_info"]["city"].ToString();
                                    clHistoryVO.District = jo["result"]["ad_info"]["district"].ToString();
                                }
                                catch
                                {

                                }*/

                                string token = CacheManager.TokenInsert(customerVO2.CustomerId);
                                CustomerLoginModel ulm = new CustomerLoginModel();
                                customerVO2.Password = "";
                                ulm.Customer = customerVO2;
                                ulm.Token = token;

                                //注册一张个人版名片
                                /*
                                CardBO CardBO = new CardBO(new CustomerProfile());
                                List<CardDataVO> cVOList = CardBO.FindCardByCustomerId(customerVO2.CustomerId);
                                if (cVOList.Count <= 0)
                                {
                                    CardDataVO CardDataVO = new CardDataVO();

                                    CardDataVO.CardID = 0;
                                    CardDataVO.Name = customerVO2.CustomerName;
                                    CardDataVO.Headimg = customerVO2.HeaderLogo;
                                    CardDataVO.CreatedAt = DateTime.Now;
                                    CardDataVO.CustomerId = customerVO2.CustomerId;
                                    CardDataVO.Collection = 0;
                                    CardDataVO.ReadCount = 0;
                                    CardDataVO.Forward = 0;

                                    int CardId = CardBO.AddCard(CardDataVO);
                                }*/

                                //记录登录信息               
                                clHistoryVO.CustomerId = customerVO2.CustomerId;
                                uBO.AddCustomerLoginHistory(clHistoryVO);

                                //更新以往访问记录
                                CardBO cBO = new CardBO(new CustomerProfile());




                                cBO.UpdateAccessrecordsCustomerId(customerVO2.CustomerId, openId);

                                return new ResultObject() { Flag = 1, Message = "注册成功!", Result = ulm, Subsidiary = openId };
                            }
                        }
                    }

                    return new ResultObject() { Flag = 0, Message = "注册失败!", Result = null };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "注册失败!", Result = null };
                }
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CustomerController));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 小程序获取微信用户信息(微云智推)
        /// </summary>
        /// <param name="wxUserInfoVO">微信小程序用户信息</param>
        /// <param name="code">第一次拉手返回Code</param>
        /// <returns></returns>
        [Route("GetMiniprogramUserInfoByDZB"), HttpPost, Anonymous]
        public ResultObject GetMiniprogramUserInfoByDZB([FromBody] wxUserInfoVO wxUserInfoVO, string code, string originType = "", int originID = 0)
        {
            try
            {
                string url = "https://api.weixin.qq.com/sns/jscode2session?appid=wx2c0ce94e903bea9a&secret=a642ce61f3ce2e2cddbcfb8b116f0579&js_code=" + code + "&grant_type=authorization_code";
                string jsonStr = HttpHelper.HtmlFromUrlGet(url);
                var serializer = new DataContractJsonSerializer(typeof(OpenIdAndSessionKey));
                var mStream = new MemoryStream(Encoding.Default.GetBytes(jsonStr));
                OpenIdAndSessionKey readConfig = (OpenIdAndSessionKey)serializer.ReadObject(mStream);

                WeChatAppDecrypt un = new WeChatAppDecrypt("wx2c0ce94e903bea9a", "a642ce61f3ce2e2cddbcfb8b116f0579");
                if (un.VaildateUserInfo(wxUserInfoVO.rawData, wxUserInfoVO.signature, readConfig.session_key))
                {
                    WechatUserInfo wui = un.Decrypt(wxUserInfoVO.encryptedData, wxUserInfoVO.iv, readConfig.session_key);

                    CustomerBO uBO = new CustomerBO(new CustomerProfile());
                    CustomerViewVO customerVO = uBO.FindCustomerByOpenId(wui.openId, wui.unionId, "2",3);
                    if (customerVO != null)
                    {
                        CustomerLoginHistoryVO clHistoryVO = new CustomerLoginHistoryVO();
                        clHistoryVO.LoginAt = DateTime.Now;
                        clHistoryVO.Status = true;
                        clHistoryVO.LoginOS = HttpContext.Current.Request.Browser.Platform;
                        clHistoryVO.LoginIP = HttpContext.Current.Request.UserHostAddress;
                        clHistoryVO.LoginBrowser = "微云智推";

                        //判断会员VIP是否过期
                        if (customerVO.isVip)
                        {
                            if (customerVO.ExpirationAt <= DateTime.Now)
                            {
                                CustomerVO cVO = new CustomerVO();
                                cVO.CustomerId = customerVO.CustomerId;
                                cVO.isVip = false;
                                cVO.ExpirationSendStatus = 0;
                                uBO.Update(cVO);
                                customerVO.isVip = false;

                                CardBO CardBO = new CardBO(new CustomerProfile(),3);
                                //发送到期提醒
                                CardBO.AddCardMessage("您的微云智推名片会员特权已到期", customerVO.CustomerId, "会员特权", "/pages/MyCenter/VipCenter/VipCenter");
                            }
                        }

                        string token = CacheManager.TokenInsert(customerVO.CustomerId);
                        CustomerLoginModel ulm = new CustomerLoginModel();
                        customerVO.Password = "";
                        ulm.Customer = customerVO;
                        ulm.Token = token;

                        BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
                        //判断是否有企业名片的个人信息
                        PersonalVO pVO = cBO.FindPersonalByCustomerId(customerVO.CustomerId);
                        if (pVO != null)
                        {
                            CardBO CardBO = new CardBO(new CustomerProfile(),3);
                            List<CardDataVO> cVO = CardBO.FindCardByCustomerId(customerVO.CustomerId);
                            if (cVO.Count <= 0)
                            {
                                CardDataVO CardDataVO = new CardDataVO();

                                CardDataVO.CardID = 0;
                                CardDataVO.Name = pVO.Name;
                                CardDataVO.Headimg = pVO.Headimg;
                                CardDataVO.Phone = pVO.Phone;
                                CardDataVO.Position = pVO.Position;
                                CardDataVO.WeChat = pVO.WeChat;
                                CardDataVO.Email = pVO.Email;
                                CardDataVO.Details = pVO.Details;
                                CardDataVO.Address = pVO.Address;
                                CardDataVO.Business = pVO.Business;
                                CardDataVO.latitude = pVO.latitude;
                                CardDataVO.longitude = pVO.longitude;
                                CardDataVO.Tel = pVO.Tel;

                                CardDataVO.CreatedAt = DateTime.Now;
                                CardDataVO.Status = 1;//0:禁用，1:启用
                                CardDataVO.CustomerId = customerVO.CustomerId;
                                CardDataVO.Collection = 0;
                                CardDataVO.ReadCount = 0;
                                CardDataVO.Forward = 0;
                                CardDataVO.isBusinessCard = 1;
                                CardDataVO.AppType = 3;

                                if (pVO.BusinessID > 0)
                                {
                                    CardDataVO.CorporateName = cBO.FindBusinessCardById(pVO.BusinessID).BusinessName;
                                }

                                int CardId = CardBO.AddCard(CardDataVO);
                            }
                        }

                        //记录登录信息               
                        clHistoryVO.CustomerId = customerVO.CustomerId;
                        uBO.AddCustomerLoginHistory(clHistoryVO);

                        //更新以往访问记录
                        CardBO CdBO = new CardBO(new CustomerProfile(),3);
                        CdBO.UpdateAccessrecordsCustomerId(customerVO.CustomerId, wui.openId);

                        return new ResultObject() { Flag = 1, Message = "登录成功!", Result = ulm, Subsidiary = wui.openId };
                    }
                    else
                    {

                        CustomerVO cVO = new CustomerVO();
                        cVO.CustomerCode = uBO.GetCustomerCode();
                        string password = Utilities.MakePassword(8);
                        cVO.Password = Utilities.GetMD5(password);
                        cVO.Status = 1;
                        cVO.AppType = 3;
                        cVO.CreatedAt = DateTime.Now;
                        if (wui.nickName.Length > 0)
                        {
                            cVO.CustomerName = wui.nickName;
                        }
                        //cVO.Sex = wui.gender == "1";
                        cVO.Leliao = true;
                        cVO.originType = originType;
                        cVO.originID = originID;
                        try
                        {
                            if (cVO.CustomerName != "微信用户")
                            {
                                //下载头像
                                string folder = "/UploadFolder/Image/" + DateTime.Now.ToString("yyyyMM") + "/";
                                string imgPath = "";
                                string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".jpg";
                                //可以修改为网络路径
                                string localPath = ConfigInfo.Instance.UploadFolder + folder;
                                if (!Directory.Exists(localPath))
                                {
                                    Directory.CreateDirectory(localPath);
                                }
                                string PhysicalPath = localPath + newFileName;
                                imgPath = ConfigInfo.Instance.APIURL + folder + newFileName;
                                WebRequest wreq = WebRequest.Create(wui.avatarUrl);
                                HttpWebResponse wresp = (HttpWebResponse)wreq.GetResponse();
                                Stream s = wresp.GetResponseStream();
                                System.Drawing.Image img;
                                img = System.Drawing.Image.FromStream(s);
                                img.Save(PhysicalPath, ImageFormat.Jpeg);   //保存

                                cVO.HeaderLogo = imgPath;
                            }
                        }
                        catch
                        {

                        }


                        int customerId = uBO.Add(cVO);
                        if (customerId > 0)
                        {
                            CustomerMatchVO customerMatchVO = new CustomerMatchVO();
                            customerMatchVO.OpenId = wui.openId;
                            customerMatchVO.UnionID = wui.unionId;
                            customerMatchVO.CustomerId = customerId;
                            customerMatchVO.MatchType = "2";
                            customerMatchVO.AppType = 3;
                            int customerId2 = uBO.AddCustomerMatch(customerMatchVO);
                            if (customerId2 > 0)
                            {
                                CustomerViewVO customerVO2 = uBO.FindCustomerByOpenId(wui.openId, wui.unionId, "2",3);
                                if (customerVO2 != null)
                                {
                                    CustomerLoginHistoryVO clHistoryVO = new CustomerLoginHistoryVO();
                                    clHistoryVO.LoginAt = DateTime.Now;
                                    clHistoryVO.Status = true;
                                    clHistoryVO.LoginOS = HttpContext.Current.Request.Browser.Platform;
                                    clHistoryVO.LoginIP = HttpContext.Current.Request.UserHostAddress;
                                    clHistoryVO.LoginBrowser = "微云智推";

                                    string token = CacheManager.TokenInsert(customerVO2.CustomerId);
                                    CustomerLoginModel ulm = new CustomerLoginModel();
                                    customerVO2.Password = "";
                                    ulm.Customer = customerVO2;
                                    ulm.Token = token;

                                    //注册一张个人版名片
                                    /*
                                    CardBO CardBO = new CardBO(new CustomerProfile());
                                    List<CardDataVO> cVOList = CardBO.FindCardByCustomerId(customerVO2.CustomerId);
                                    if (cVOList.Count <= 0)
                                    {
                                        CardDataVO CardDataVO = new CardDataVO();

                                        CardDataVO.CardID = 0;
                                        CardDataVO.Name = customerVO2.CustomerName;
                                        CardDataVO.Headimg = customerVO2.HeaderLogo;
                                        CardDataVO.CreatedAt = DateTime.Now;
                                        CardDataVO.CustomerId = customerVO2.CustomerId;
                                        CardDataVO.Collection = 0;
                                        CardDataVO.ReadCount = 0;
                                        CardDataVO.Forward = 0;

                                        int CardId = CardBO.AddCard(CardDataVO);
                                    }*/

                                    //记录登录信息               
                                    clHistoryVO.CustomerId = customerVO2.CustomerId;
                                    uBO.AddCustomerLoginHistory(clHistoryVO);

                                    //更新以往访问记录
                                    CardBO cBO = new CardBO(new CustomerProfile(),3);
                                    cBO.UpdateAccessrecordsCustomerId(customerVO2.CustomerId, wui.openId);

                                    return new ResultObject() { Flag = 1, Message = "注册成功!", Result = ulm, Subsidiary = wui.openId };
                                }
                            }
                        }

                        return new ResultObject() { Flag = 0, Message = "注册失败!", Result = null };
                    }
                }

                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CustomerController));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 小程序获取微信用户信息(活动星选)
        /// </summary>
        /// <param name="wxUserInfoVO">微信小程序用户信息</param>
        /// <param name="code">第一次拉手返回Code</param>
        /// <returns></returns>
        [Route("GetMiniprogramUserInfoByHD"), HttpPost, Anonymous]
        public ResultObject GetMiniprogramUserInfoByHD([FromBody] wxUserInfoVO wxUserInfoVO, string code, string originType = "", int originID = 0, int isNewLogin = 0)
        {
            try
            {
                string url = "https://api.weixin.qq.com/sns/jscode2session?appid=wx83bf84d3847abf2f&secret=dcdddcd1f79943500e2fb210b1684185&js_code=" + code + "&grant_type=authorization_code";
                string jsonStr = HttpHelper.HtmlFromUrlGet(url);
                var serializer = new DataContractJsonSerializer(typeof(OpenIdAndSessionKey));
                var mStream = new MemoryStream(Encoding.Default.GetBytes(jsonStr));
                OpenIdAndSessionKey readConfig = (OpenIdAndSessionKey)serializer.ReadObject(mStream);

                WeChatAppDecrypt un = new WeChatAppDecrypt("wx83bf84d3847abf2f", "dcdddcd1f79943500e2fb210b1684185");

                wxUserInfo wxUser = new wxUserInfo();
                string openId = readConfig.openid;
                string unionId = readConfig.unionid;
                if (isNewLogin == 0)
                {
                    if (un.VaildateUserInfo(wxUserInfoVO.rawData, wxUserInfoVO.signature, readConfig.session_key))
                    {
                        WechatUserInfo wui = un.Decrypt(wxUserInfoVO.encryptedData, wxUserInfoVO.iv, readConfig.session_key);
                        wxUser.nickName = wui.nickName;
                        //wxUser.gender = wui.gender == "1" ? 1 : 0;
                        wxUser.avatarUrl = wui.avatarUrl;

                        openId = wui.openId;
                        unionId = wui.unionId;
                    }
                    else
                    {
                        return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
                    }
                }
                else
                {
                    wxUser = wxUserInfoVO.userInfo;
                }
                CardBO CardBO = new CardBO(new CustomerProfile());
                CustomerBO uBO = new CustomerBO(new CustomerProfile());
                CustomerViewVO customerVO = uBO.FindCustomerByOpenId(openId, unionId, "2");
                if (customerVO != null)
                {
                    CustomerLoginHistoryVO clHistoryVO = new CustomerLoginHistoryVO();
                    clHistoryVO.LoginAt = DateTime.Now;
                    clHistoryVO.Status = true;
                    clHistoryVO.LoginOS = HttpContext.Current.Request.Browser.Platform;
                    clHistoryVO.LoginIP = HttpContext.Current.Request.UserHostAddress;
                    clHistoryVO.LoginBrowser = "活动星选";

                    /*
                   try
                   {

                       string jsonstr = HttpHelper.HtmlFromUrlGet("http://apis.map.qq.com/ws/location/v1/ip?key=QHBBZ-ZDAWI-TRVGE-5GWEO-PHEKO-Q5BXC&ip=" + clHistoryVO.LoginIP);
                       JObject jo = (JObject)JsonConvert.DeserializeObject(jsonstr);
                       clHistoryVO.Nation = jo["result"]["ad_info"]["nation"].ToString();
                       clHistoryVO.Province = jo["result"]["ad_info"]["province"].ToString();
                       clHistoryVO.City = jo["result"]["ad_info"]["city"].ToString();
                       clHistoryVO.District = jo["result"]["ad_info"]["district"].ToString();

                    }
                    catch
                    {

                    } */
                    //判断会员VIP是否过期
                    if (customerVO.isVip)
                    {
                        if (customerVO.ExpirationAt <= DateTime.Now)
                        {
                            CustomerVO cVO = new CustomerVO();
                            cVO.CustomerId = customerVO.CustomerId;
                            cVO.isVip = false;
                            cVO.ExpirationSendStatus = 0;
                            uBO.Update(cVO);
                            customerVO.isVip = false;

                            //发送到期提醒
                            CardBO.AddCardMessage("您的活动星选会员特权已到期", customerVO.CustomerId, "会员特权", "/pages/MyCenter/VipCenter/VipCenter");
                        }
                    }
                    customerVO.isVip = CardBO.isVIP(customerVO.CustomerId);

                    string token = CacheManager.TokenInsert(customerVO.CustomerId);
                    CustomerLoginModel ulm = new CustomerLoginModel();
                    customerVO.Password = "";
                    ulm.Customer = customerVO;
                    ulm.Token = token;

                    BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
                    //判断是否有企业名片的个人信息
                    PersonalVO pVO = cBO.FindPersonalByCustomerId(customerVO.CustomerId);
                    if (pVO != null)
                    {
                        List<CardDataVO> cVO = CardBO.FindCardByCustomerId(customerVO.CustomerId);
                        if (cVO.Count <= 0)
                        {
                            CardDataVO CardDataVO = new CardDataVO();

                            CardDataVO.CardID = 0;
                            CardDataVO.Name = pVO.Name;
                            CardDataVO.Headimg = pVO.Headimg;
                            CardDataVO.Phone = pVO.Phone;
                            CardDataVO.Position = pVO.Position;
                            CardDataVO.WeChat = pVO.WeChat;
                            CardDataVO.Email = pVO.Email;
                            CardDataVO.Details = pVO.Details;
                            CardDataVO.Address = pVO.Address;
                            CardDataVO.Business = pVO.Business;
                            CardDataVO.latitude = pVO.latitude;
                            CardDataVO.longitude = pVO.longitude;
                            CardDataVO.Tel = pVO.Tel;

                            CardDataVO.CreatedAt = DateTime.Now;
                            CardDataVO.Status = 1;//0:禁用，1:启用
                            CardDataVO.CustomerId = customerVO.CustomerId;
                            CardDataVO.Collection = 0;
                            CardDataVO.ReadCount = 0;
                            CardDataVO.Forward = 0;
                            CardDataVO.isBusinessCard = 1;

                            if (pVO.BusinessID > 0)
                            {
                                CardDataVO.CorporateName = cBO.FindBusinessCardById(pVO.BusinessID).BusinessName;
                            }

                            int CardId = CardBO.AddCard(CardDataVO);
                        }
                    }

                    //记录登录信息               
                    clHistoryVO.CustomerId = customerVO.CustomerId;
                    uBO.AddCustomerLoginHistory(clHistoryVO);

                    //更新以往访问记录
                    CardBO CdBO = new CardBO(new CustomerProfile());
                    CdBO.UpdateAccessrecordsCustomerId(customerVO.CustomerId, openId);

                    return new ResultObject() { Flag = 1, Message = "登录成功!", Result = ulm, Subsidiary = openId };
                }
                else if (openId != "" || unionId != "")
                {

                    CustomerVO cVO = new CustomerVO();
                    cVO.CustomerCode = uBO.GetCustomerCode();
                    string password = Utilities.MakePassword(8);
                    cVO.Password = Utilities.GetMD5(password);
                    cVO.Status = 1;
                    cVO.CreatedAt = DateTime.Now;
                    if (wxUser.nickName.Length > 0)
                    {
                        cVO.CustomerName = wxUser.nickName;
                    }
                    cVO.Sex = wxUser.gender == 1;
                    cVO.Leliao = true;
                    cVO.originType = originType;
                    cVO.originID = originID;

                    //新用户赠送一个月会员
                    /*
                    cVO.isVip = true;
                    cVO.ExpirationAt = DateTime.Now.AddMonths(1);
                    */
                    try
                    {
                        if (cVO.CustomerName != "微信用户")
                        {
                            //下载头像
                            string folder = "/UploadFolder/Image/" + DateTime.Now.ToString("yyyyMM") + "/";
                            string imgPath = "";
                            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".jpg";
                            //可以修改为网络路径
                            string localPath = ConfigInfo.Instance.UploadFolder + folder;
                            if (!Directory.Exists(localPath))
                            {
                                Directory.CreateDirectory(localPath);
                            }
                            string PhysicalPath = localPath + newFileName;
                            imgPath = ConfigInfo.Instance.APIURL + folder + newFileName;
                            WebRequest wreq = WebRequest.Create(wxUser.avatarUrl);
                            HttpWebResponse wresp = (HttpWebResponse)wreq.GetResponse();
                            Stream s = wresp.GetResponseStream();
                            System.Drawing.Image img;
                            img = System.Drawing.Image.FromStream(s);
                            img.Save(PhysicalPath, ImageFormat.Jpeg);   //保存

                            cVO.HeaderLogo = imgPath;
                        }
                    }
                    catch
                    {

                    }


                    int customerId = uBO.Add(cVO);
                    if (customerId > 0)
                    {
                        CustomerMatchVO customerMatchVO = new CustomerMatchVO();
                        customerMatchVO.OpenId = openId;
                        customerMatchVO.UnionID = unionId;
                        customerMatchVO.CustomerId = customerId;
                        customerMatchVO.MatchType = "2";
                        int customerId2 = uBO.AddCustomerMatch(customerMatchVO);
                        if (customerId2 > 0)
                        {
                            CustomerViewVO customerVO2 = uBO.FindCustomerByOpenId(openId, unionId, "2");
                            if (customerVO2 != null)
                            {
                                CustomerLoginHistoryVO clHistoryVO = new CustomerLoginHistoryVO();
                                clHistoryVO.LoginAt = DateTime.Now;
                                clHistoryVO.Status = true;
                                clHistoryVO.LoginOS = HttpContext.Current.Request.Browser.Platform;
                                clHistoryVO.LoginIP = HttpContext.Current.Request.UserHostAddress;
                                clHistoryVO.LoginBrowser = "活动星选";
                                /*
                                try
                                {
                                    string jsonstr = HttpHelper.HtmlFromUrlGet("http://apis.map.qq.com/ws/location/v1/ip?key=QHBBZ-ZDAWI-TRVGE-5GWEO-PHEKO-Q5BXC&ip=" + clHistoryVO.LoginIP);
                                    JObject jo = (JObject)JsonConvert.DeserializeObject(jsonstr);
                                    clHistoryVO.Nation = jo["result"]["ad_info"]["nation"].ToString();
                                    clHistoryVO.Province = jo["result"]["ad_info"]["province"].ToString();
                                    clHistoryVO.City = jo["result"]["ad_info"]["city"].ToString();
                                    clHistoryVO.District = jo["result"]["ad_info"]["district"].ToString();
                                }
                                catch
                                {

                                }*/

                                string token = CacheManager.TokenInsert(customerVO2.CustomerId);
                                CustomerLoginModel ulm = new CustomerLoginModel();
                                customerVO2.Password = "";
                                ulm.Customer = customerVO2;
                                ulm.Token = token;

                                //注册一张个人版名片
                                /*
                                CardBO CardBO = new CardBO(new CustomerProfile());
                                List<CardDataVO> cVOList = CardBO.FindCardByCustomerId(customerVO2.CustomerId);
                                if (cVOList.Count <= 0)
                                {
                                    CardDataVO CardDataVO = new CardDataVO();

                                    CardDataVO.CardID = 0;
                                    CardDataVO.Name = customerVO2.CustomerName;
                                    CardDataVO.Headimg = customerVO2.HeaderLogo;
                                    CardDataVO.CreatedAt = DateTime.Now;
                                    CardDataVO.CustomerId = customerVO2.CustomerId;
                                    CardDataVO.Collection = 0;
                                    CardDataVO.ReadCount = 0;
                                    CardDataVO.Forward = 0;

                                    int CardId = CardBO.AddCard(CardDataVO);
                                }*/

                                //记录登录信息               
                                clHistoryVO.CustomerId = customerVO2.CustomerId;
                                uBO.AddCustomerLoginHistory(clHistoryVO);

                                //更新以往访问记录
                                CardBO cBO = new CardBO(new CustomerProfile());
                                cBO.UpdateAccessrecordsCustomerId(customerVO2.CustomerId, openId);

                                return new ResultObject() { Flag = 1, Message = "注册成功!", Result = ulm, Subsidiary = openId };
                            }
                        }
                    }

                    return new ResultObject() { Flag = 0, Message = "注册失败!", Result = null };
                }else
                {
                    return new ResultObject() { Flag = 0, Message = "注册失败!", Result = null };
                }
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CustomerController));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }


        /// <summary>
        /// 小程序获取微信用户信息(搜客)
        /// </summary>
        /// <param name="wxUserInfoVO">微信小程序用户信息</param>
        /// <param name="code">第一次拉手返回Code</param>
        /// <returns></returns>
        [Route("GetMiniprogramUserInfoBySK"), HttpPost, Anonymous]
        public ResultObject GetMiniprogramUserInfoBySK([FromBody] wxUserInfoVO wxUserInfoVO, string code, string originType = "", int originID = 0)
        {
            try
            {
                string url = "https://api.weixin.qq.com/sns/jscode2session?appid=wxdc35ba3b02a97c92&secret=b6163ee939ec6614ea9c3e8b9917a983&js_code=" + code + "&grant_type=authorization_code";
                string jsonStr = HttpHelper.HtmlFromUrlGet(url);
                var serializer = new DataContractJsonSerializer(typeof(OpenIdAndSessionKey));
                var mStream = new MemoryStream(Encoding.Default.GetBytes(jsonStr));
                OpenIdAndSessionKey readConfig = (OpenIdAndSessionKey)serializer.ReadObject(mStream);
                CardBO CardBO = new CardBO(new CustomerProfile());
                WeChatAppDecrypt un = new WeChatAppDecrypt("wxdc35ba3b02a97c92", "b6163ee939ec6614ea9c3e8b9917a983");
                if (un.VaildateUserInfo(wxUserInfoVO.rawData, wxUserInfoVO.signature, readConfig.session_key))
                {
                    WechatUserInfo wui = un.Decrypt(wxUserInfoVO.encryptedData, wxUserInfoVO.iv, readConfig.session_key);

                    CustomerBO uBO = new CustomerBO(new CustomerProfile());
                    CustomerViewVO customerVO = uBO.FindCustomerByOpenId(wui.openId, wui.unionId, "2");
                    if (customerVO != null)
                    {
                        CustomerLoginHistoryVO clHistoryVO = new CustomerLoginHistoryVO();
                        clHistoryVO.LoginAt = DateTime.Now;
                        clHistoryVO.Status = true;
                        clHistoryVO.LoginOS = HttpContext.Current.Request.Browser.Platform;
                        clHistoryVO.LoginIP = HttpContext.Current.Request.UserHostAddress;
                        clHistoryVO.LoginBrowser = "搜客";

                        //判断会员VIP是否过期
                        if (customerVO.isVip)
                        {
                            if (customerVO.ExpirationAt <= DateTime.Now)
                            {
                                CustomerVO cVO = new CustomerVO();
                                cVO.CustomerId = customerVO.CustomerId;
                                cVO.isVip = false;
                                cVO.ExpirationSendStatus = 0;
                                uBO.Update(cVO);
                                customerVO.isVip = false;
                            }
                        }
                        customerVO.isVip = CardBO.isVIP(customerVO.CustomerId);

                        string token = CacheManager.TokenInsert(customerVO.CustomerId);
                        CustomerLoginModel ulm = new CustomerLoginModel();
                        customerVO.Password = "";
                        ulm.Customer = customerVO;
                        ulm.Token = token;

                        //记录登录信息               
                        clHistoryVO.CustomerId = customerVO.CustomerId;
                        uBO.AddCustomerLoginHistory(clHistoryVO);

                        //更新以往访问记录
                        CardBO.UpdateAccessrecordsCustomerId(customerVO.CustomerId, wui.openId);

                        return new ResultObject() { Flag = 1, Message = "登录成功!", Result = ulm, Subsidiary = wui.openId };
                    }
                    else if (wui.openId != "" || wui.unionId != "")
                    {

                        CustomerVO cVO = new CustomerVO();
                        cVO.CustomerCode = uBO.GetCustomerCode();
                        string password = Utilities.MakePassword(8);
                        cVO.Password = Utilities.GetMD5(password);
                        cVO.Status = 1;
                        cVO.CreatedAt = DateTime.Now;
                        if (wui.nickName.Length > 0)
                        {
                            cVO.CustomerName = wui.nickName;
                        }
                        //cVO.Sex = wui.gender == "1";
                        cVO.SouKe = true;
                        cVO.originType = originType;
                        cVO.originID = originID;
                        try
                        {
                            if (cVO.CustomerName != "微信用户")
                            {
                                //下载头像
                                string folder = "/UploadFolder/Image/" + DateTime.Now.ToString("yyyyMM") + "/";
                                string imgPath = "";
                                string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".jpg";
                                //可以修改为网络路径
                                string localPath = ConfigInfo.Instance.UploadFolder + folder;
                                if (!Directory.Exists(localPath))
                                {
                                    Directory.CreateDirectory(localPath);
                                }
                                string PhysicalPath = localPath + newFileName;
                                imgPath = ConfigInfo.Instance.APIURL + folder + newFileName;
                                WebRequest wreq = WebRequest.Create(wui.avatarUrl);
                                HttpWebResponse wresp = (HttpWebResponse)wreq.GetResponse();
                                Stream s = wresp.GetResponseStream();
                                System.Drawing.Image img;
                                img = System.Drawing.Image.FromStream(s);
                                img.Save(PhysicalPath, ImageFormat.Jpeg);   //保存

                                cVO.HeaderLogo = imgPath;
                            }
                        }
                        catch
                        {

                        }


                        int customerId = uBO.Add(cVO);
                        if (customerId > 0)
                        {
                            CustomerMatchVO customerMatchVO = new CustomerMatchVO();
                            customerMatchVO.OpenId = wui.openId;
                            customerMatchVO.UnionID = wui.unionId;
                            customerMatchVO.CustomerId = customerId;
                            customerMatchVO.MatchType = "2";
                            int customerId2 = uBO.AddCustomerMatch(customerMatchVO);
                            if (customerId2 > 0)
                            {
                                CustomerViewVO customerVO2 = uBO.FindCustomerByOpenId(wui.openId, wui.unionId, "2");
                                if (customerVO2 != null)
                                {
                                    CustomerLoginHistoryVO clHistoryVO = new CustomerLoginHistoryVO();
                                    clHistoryVO.LoginAt = DateTime.Now;
                                    clHistoryVO.Status = true;
                                    clHistoryVO.LoginOS = HttpContext.Current.Request.Browser.Platform;
                                    clHistoryVO.LoginIP = HttpContext.Current.Request.UserHostAddress;
                                    clHistoryVO.LoginBrowser = "搜客";
                                    /*
                                    try
                                    {
                                        string jsonstr = HttpHelper.HtmlFromUrlGet("http://apis.map.qq.com/ws/location/v1/ip?key=QHBBZ-ZDAWI-TRVGE-5GWEO-PHEKO-Q5BXC&ip=" + clHistoryVO.LoginIP);
                                        JObject jo = (JObject)JsonConvert.DeserializeObject(jsonstr);
                                        clHistoryVO.Nation = jo["result"]["ad_info"]["nation"].ToString();
                                        clHistoryVO.Province = jo["result"]["ad_info"]["province"].ToString();
                                        clHistoryVO.City = jo["result"]["ad_info"]["city"].ToString();
                                        clHistoryVO.District = jo["result"]["ad_info"]["district"].ToString();
                                    }
                                    catch
                                    {

                                    }*/

                                    string token = CacheManager.TokenInsert(customerVO2.CustomerId);
                                    CustomerLoginModel ulm = new CustomerLoginModel();
                                    customerVO2.Password = "";
                                    ulm.Customer = customerVO2;
                                    ulm.Token = token;

                                    //注册一张个人版名片
                                    /*
                                    CardBO CardBO = new CardBO(new CustomerProfile());
                                    List<CardDataVO> cVOList = CardBO.FindCardByCustomerId(customerVO2.CustomerId);
                                    if (cVOList.Count <= 0)
                                    {
                                        CardDataVO CardDataVO = new CardDataVO();

                                        CardDataVO.CardID = 0;
                                        CardDataVO.Name = customerVO2.CustomerName;
                                        CardDataVO.Headimg = customerVO2.HeaderLogo;
                                        CardDataVO.CreatedAt = DateTime.Now;
                                        CardDataVO.CustomerId = customerVO2.CustomerId;
                                        CardDataVO.Collection = 0;
                                        CardDataVO.ReadCount = 0;
                                        CardDataVO.Forward = 0;

                                        int CardId = CardBO.AddCard(CardDataVO);
                                    }*/

                                    //记录登录信息               
                                    clHistoryVO.CustomerId = customerVO2.CustomerId;
                                    uBO.AddCustomerLoginHistory(clHistoryVO);

                                    //更新以往访问记录
                                    CardBO cBO = new CardBO(new CustomerProfile());
                                    cBO.UpdateAccessrecordsCustomerId(customerVO2.CustomerId, wui.openId);

                                    return new ResultObject() { Flag = 1, Message = "注册成功!", Result = ulm, Subsidiary = wui.openId };
                                }
                            }
                        }

                        return new ResultObject() { Flag = 0, Message = "注册失败!", Result = null };
                    }else
                    {
                        return new ResultObject() { Flag = 0, Message = "注册失败!", Result = null };
                    }
                }

                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CustomerController));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// h5获取微信用户信息
        /// </summary>
        /// <param name="code">第一次拉手返回Code</param>
        /// <returns></returns>
        [Route("GetMiniprogramUser"), HttpGet, Anonymous]
        public ResultObject GetMiniprogramUser(string code)
        {
            try
            { 
                ////获取微信的用户信息
                CustomerController customerCon = new CustomerController();
                ResultObject result = customerCon.GetThirdPartUserInfo("2", code, "", "2");
                WeiXinHelper.WeiXinUserInfoResult userInfo = result.Result as WeiXinHelper.WeiXinUserInfoResult;
                //用户信息（判断是否已经获取到用户的微信用户信息）
                if (userInfo.Result && userInfo.UserInfo.openid != "" && userInfo.UserInfo.unionid != "")
                {
                    //_bo.Info("userInfo.UserInfo.openid = " + userInfo.UserInfo.openid);
                    string OpenId = userInfo.UserInfo.openid;
                    string UnionID = userInfo.UserInfo.unionid;
                    //根据OpenID判断是否已经注册，有则直接跳过。否则需要输入手机号码才能正常使用                    
                    CustomerBO uBO = new CustomerBO(new CustomerProfile());
                    CardBO CardBO = new CardBO(new CustomerProfile());
                    CustomerViewVO customerVO = uBO.FindCustomerByOpenId(OpenId, UnionID, "2");
                    if (customerVO != null)
                    {
                        CustomerLoginHistoryVO clHistoryVO = new CustomerLoginHistoryVO();
                        clHistoryVO.LoginAt = DateTime.Now;
                        clHistoryVO.Status = true;
                        clHistoryVO.LoginOS = HttpContext.Current.Request.Browser.Platform;
                        clHistoryVO.LoginIP = HttpContext.Current.Request.UserHostAddress;
                        clHistoryVO.LoginBrowser = HttpContext.Current.Request.Browser.Browser;

                        //判断会员VIP是否过期
                        if (customerVO.isVip)
                        {
                            if (customerVO.ExpirationAt <= DateTime.Now)
                            {
                                CustomerVO cVO = new CustomerVO();
                                cVO.CustomerId = customerVO.CustomerId;
                                cVO.isVip = false;
                                cVO.ExpirationSendStatus = 0;
                                uBO.Update(cVO);
                                customerVO.isVip = false;

                                //发送到期提醒
                                CardBO.AddCardMessage("您的乐聊名片会员特权已到期", customerVO.CustomerId, "会员特权", "/pages/MyCenter/VipCenter/VipCenter");
                            }
                        }
                        customerVO.isVip = CardBO.isVIP(customerVO.CustomerId);

                        //登录
                        string token = CacheManager.TokenInsert(customerVO.CustomerId);

                        CustomerLoginModel ulm = new CustomerLoginModel();
                        ulm.Customer = customerVO;
                        ulm.Token = token;

                        BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
                        //判断是否有企业名片的个人信息
                        PersonalVO pVO = cBO.FindPersonalByCustomerId(customerVO.CustomerId);
                        if (pVO != null)
                        {
                            
                            List<CardDataVO> cVO = CardBO.FindCardByCustomerId(customerVO.CustomerId);
                            if (cVO.Count <= 0)
                            {
                                CardDataVO CardDataVO = new CardDataVO();

                                CardDataVO.CardID = 0;
                                CardDataVO.Name = pVO.Name;
                                CardDataVO.Headimg = pVO.Headimg;
                                CardDataVO.Phone = pVO.Phone;
                                CardDataVO.Position = pVO.Position;
                                CardDataVO.WeChat = pVO.WeChat;
                                CardDataVO.Email = pVO.Email;
                                CardDataVO.Details = pVO.Details;
                                CardDataVO.Address = pVO.Address;
                                CardDataVO.Business = pVO.Business;
                                CardDataVO.latitude = pVO.latitude;
                                CardDataVO.longitude = pVO.longitude;
                                CardDataVO.Tel = pVO.Tel;

                                CardDataVO.CreatedAt = DateTime.Now;
                                CardDataVO.Status = 1;//0:禁用，1:启用
                                CardDataVO.CustomerId = customerVO.CustomerId;
                                CardDataVO.Collection = 0;
                                CardDataVO.ReadCount = 0;
                                CardDataVO.Forward = 0;

                                if (pVO.BusinessID > 0)
                                {
                                    CardDataVO.CorporateName = cBO.FindBusinessCardById(pVO.BusinessID).BusinessName;
                                }

                                int CardId = CardBO.AddCard(CardDataVO);
                            }
                        }

                        //记录登录信息               
                        clHistoryVO.CustomerId = customerVO.CustomerId;
                        uBO.AddCustomerLoginHistory(clHistoryVO);

                        return new ResultObject() { Flag = 1, Message = "登录成功!", Result = ulm };
                        
                    }
                    else if(OpenId!=""|| UnionID!="")
                    {

                        CustomerVO cVO = new CustomerVO();
                        cVO.CustomerCode = uBO.GetCustomerCode();
                        string password = Utilities.MakePassword(8);
                        cVO.Password = Utilities.GetMD5(password);
                        cVO.Status = 1;
                        cVO.CreatedAt = DateTime.Now;


                        if (userInfo.UserInfo.nickname.Length > 0)
                        {
                            cVO.CustomerName = userInfo.UserInfo.nickname;
                        }

                        cVO.Sex = userInfo.UserInfo.sex == "1";
                        cVO.Leliao = true;

                        //新用户赠送一个月会员
                        /*
                        cVO.isVip = true;
                        cVO.ExpirationAt = DateTime.Now.AddMonths(1);
                        */
                        try
                        {
                            if (cVO.CustomerName != "微信用户")
                            {
                                //下载头像
                                string folder = "/UploadFolder/Image/" + DateTime.Now.ToString("yyyyMM") + "/";
                                string imgPath = "";
                                string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".jpg";
                                //可以修改为网络路径
                                string localPath = ConfigInfo.Instance.UploadFolder + folder;
                                if (!Directory.Exists(localPath))
                                {
                                    Directory.CreateDirectory(localPath);
                                }
                                string PhysicalPath = localPath + newFileName;
                                imgPath = ConfigInfo.Instance.APIURL + folder + newFileName;
                                WebRequest wreq = WebRequest.Create(userInfo.UserInfo.headimgurl);
                                HttpWebResponse wresp = (HttpWebResponse)wreq.GetResponse();
                                Stream s = wresp.GetResponseStream();
                                System.Drawing.Image img;
                                img = System.Drawing.Image.FromStream(s);
                                img.Save(PhysicalPath, ImageFormat.Jpeg);   //保存

                                cVO.HeaderLogo = imgPath;
                            }
                        }
                        catch
                        {

                        }


                        int customerId = uBO.Add(cVO);
                        if (customerId > 0)
                        {
                            CustomerMatchVO customerMatchVO = new CustomerMatchVO();
                            customerMatchVO.OpenId = OpenId;
                            customerMatchVO.UnionID = UnionID;
                            customerMatchVO.CustomerId = customerId;
                            customerMatchVO.MatchType = "2";
                            int customerId2 = uBO.AddCustomerMatch(customerMatchVO);
                            if (customerId2 > 0)
                            {
                                CustomerViewVO customerVO2 = uBO.FindCustomerByOpenId(OpenId, UnionID, "2");
                                if (customerVO2 != null)
                                {
                                    CustomerLoginHistoryVO clHistoryVO = new CustomerLoginHistoryVO();
                                    clHistoryVO.LoginAt = DateTime.Now;
                                    clHistoryVO.Status = true;
                                    clHistoryVO.LoginOS = HttpContext.Current.Request.Browser.Platform;
                                    clHistoryVO.LoginIP = HttpContext.Current.Request.UserHostAddress;
                                    clHistoryVO.LoginBrowser = HttpContext.Current.Request.Browser.Browser;

                                    //注册一张个人版名片
                                    /*
                                    CardBO CardBO = new CardBO(new CustomerProfile());
                                    List<CardDataVO> cVOList = CardBO.FindCardByCustomerId(customerVO2.CustomerId);
                                    if (cVOList.Count <= 0)
                                    {
                                        CardDataVO CardDataVO = new CardDataVO();

                                        CardDataVO.CardID = 0;
                                        CardDataVO.Name = customerVO2.CustomerName;
                                        CardDataVO.Headimg = customerVO2.HeaderLogo;
                                        CardDataVO.CreatedAt = DateTime.Now;
                                        CardDataVO.CustomerId = customerVO2.CustomerId;
                                        CardDataVO.Collection = 0;
                                        CardDataVO.ReadCount = 0;
                                        CardDataVO.Forward = 0;

                                        int CardId = CardBO.AddCard(CardDataVO);
                                    }*/

                                    //登录
                                    string token = CacheManager.TokenInsert(customerVO2.CustomerId);
                                    CustomerLoginModel ulm = new CustomerLoginModel();
                                    ulm.Customer = customerVO;
                                    ulm.Token = token;

                                    //记录登录信息               
                                    clHistoryVO.CustomerId = customerVO2.CustomerId;
                                    uBO.AddCustomerLoginHistory(clHistoryVO);

                                    return new ResultObject() { Flag = 1, Message = "注册成功!", Result = ulm };
                                }
                            }
                        }
                    }else
                    {
                        return new ResultObject() { Flag = 0, Message = "注册失败!", Result = null };
                    }

                    return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
                }
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CustomerController));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 小程序获取微信用户信息(众销乐)
        /// </summary>
        /// <param name="wxUserInfoVO">微信小程序用户信息</param>
        /// <param name="code">第一次拉手返回Code</param>
        /// <returns></returns>
        [Route("GetMiniprogramUserInfoZXL"), HttpPost, Anonymous]
        public ResultObject GetMiniprogramUserInfoZXL([FromBody] wxUserInfoVO wxUserInfoVO, string code)
        {
            try
            {
                string url = "https://api.weixin.qq.com/sns/jscode2session?appid=wxd90e86e2ec343eae&secret=58ce21e763870071474c8ee531013e7e&js_code=" + code + "&grant_type=authorization_code";
                string jsonStr = HttpHelper.HtmlFromUrlGet(url);
                var serializer = new DataContractJsonSerializer(typeof(OpenIdAndSessionKey));
                var mStream = new MemoryStream(Encoding.Default.GetBytes(jsonStr));
                OpenIdAndSessionKey readConfig = (OpenIdAndSessionKey)serializer.ReadObject(mStream);

                WeChatAppDecrypt un = new WeChatAppDecrypt("wxd90e86e2ec343eae", "58ce21e763870071474c8ee531013e7e");
                if (un.VaildateUserInfo(wxUserInfoVO.rawData, wxUserInfoVO.signature, readConfig.session_key))
                {
                    WechatUserInfo wui = un.Decrypt(wxUserInfoVO.encryptedData, wxUserInfoVO.iv, readConfig.session_key);

                    CustomerBO uBO = new CustomerBO(new CustomerProfile());
                    CustomerViewVO customerVO = uBO.FindCustomerByOpenId(wui.openId, wui.unionId, "2");
                    if (customerVO != null)
                    {
                        CustomerLoginHistoryVO clHistoryVO = new CustomerLoginHistoryVO();
                        clHistoryVO.LoginAt = DateTime.Now;
                        clHistoryVO.Status = true;
                        clHistoryVO.LoginOS = HttpContext.Current.Request.Browser.Platform;
                        clHistoryVO.LoginIP = HttpContext.Current.Request.UserHostAddress;
                        clHistoryVO.LoginBrowser = HttpContext.Current.Request.Browser.Browser;

                        string token = CacheManager.TokenInsert(customerVO.CustomerId);
                        CustomerLoginModel ulm = new CustomerLoginModel();
                        customerVO.Password = "";
                        ulm.Customer = customerVO;
                        ulm.Token = token;

                        //记录登录信息               
                        clHistoryVO.CustomerId = customerVO.CustomerId;
                        uBO.AddCustomerLoginHistory(clHistoryVO);

                        return new ResultObject() { Flag = 1, Message = "登录成功!", Result = ulm };
                    }
                    else
                    {
                        return new ResultObject() { Flag = 0, Message = "尚未注册!", Result = wui };
                    }
                }

                return new ResultObject() { Flag = 0, Message = "尚未注册!", Result = null };
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CustomerController));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 删除会员账号
        /// </summary>
        /// <param name="customerId">会员ID，多个会员用逗号分隔</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DeleteCustomer"), HttpPost]
        public ResultObject DeleteCustomer(string customerId, string token)
        {
            try
            {
                if (!string.IsNullOrEmpty(customerId))
                {
                    string[] customerIdArr = customerId.Split(',');
                    bool isAllDelete = true;
                    CustomerBO uBO = new CustomerBO((CustomerProfile)CacheManager.GetUserProfile(token));
                    for (int i = 0; i < customerIdArr.Length; i++)
                    {
                        try
                        {
                            uBO.Delete(Convert.ToInt32(customerIdArr[i]));
                        }
                        catch
                        {
                            isAllDelete = false;
                        }
                    }
                    if (isAllDelete)
                    {
                        return new ResultObject() { Flag = 1, Message = "删除成功!", Result = null };
                    }
                    else
                    {
                        return new ResultObject() { Flag = 1, Message = "部分删除成功!", Result = null };
                    }

                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
                }
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
            }
        }

        /// <summary>
        /// 根据会员ID获取会员信息
        /// </summary>
        /// <param name="customerId">会员ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetCustomer"), HttpGet]
        public ResultObject GetCustomer(int customerId, string token)
        {
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            CustomerViewVO uVO = uBO.FindById(customerId);
            if (uVO != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 根据会员ID获取会员信息,匿名
        /// </summary>
        /// <param name="customerId">会员ID</param>
        /// <returns></returns>
        [Route("GetCustomer"), HttpGet, Anonymous]
        public ResultObject GetCustomer(int customerId)
        {
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            CustomerViewVO uVO = uBO.FindById(customerId);
            if (uVO != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取会员数量,匿名
        /// </summary>
        /// <returns></returns>
        [Route("GetCustomerCount"), HttpGet, Anonymous]
        public ResultObject GetCustomerCount()
        {
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            int Count = uBO.FindCustomerCount();
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = Count };
        }

        /// <summary>
        /// 根据ID获取乐币奖励信息
        /// </summary>
        /// <param name="ZxbConfigID">ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetZxbConfig"), HttpGet]
        public ResultObject GetZxbConfig(int ZxbConfigID, string token)
        {
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            ZxbConfigVO uVO = uBO.FindZxbConfigById(ZxbConfigID);
            if (uVO != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 手动发放乐币奖励，后台使用
        /// </summary>
        /// <param name="customerId">会员ID</param>
        /// <param name="Cost">数额</param>
        /// <param name="Purpose">说明</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("admZXBAddrequire"), HttpGet]
        public ResultObject admZXBAddrequire(int customerId,decimal Cost,string Purpose, string token)
        {
            //只能平台账号才能发放乐币
            UserProfile uProfile = CacheManager.GetUserProfile(token);

            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            if (uBO.ZXBAddrequire(customerId, Cost, Purpose,7))
            {
                return new ResultObject() { Flag = 1, Message = "发放成功!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "发放失败!", Result = null };
            }
        }

        public ResultObject GetBankListByCustomerId(int customerId, string token)
        {
            CustomerBO uBO = new CustomerBO((CustomerProfile)CacheManager.GetUserProfile(token));
            List<BankAccountVO> uVO = uBO.GetBankListByCustomerId(customerId);
            if (uVO != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 根据会员余额信息
        /// </summary>
        /// <param name="customerId">会员ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetCustomerBalance"), HttpGet]
        public ResultObject GetCustomerBalance(int customerId, string token)
        {
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            BalanceVO uVO = uBO.FindBalanceByCustomerId(customerId);
            if (uVO != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO };
            }
            else
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = new BalanceVO() { CustomerId = customerId, Balance = 0 } };
            }
        }

        /// <summary>
        /// 根据乐币余额信息
        /// </summary>
        /// <param name="customerId">会员ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetZXBBalance"), HttpGet]
        public ResultObject GetZXBBalance(int customerId, string token)
        {
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            BalanceVO uVO = uBO.FindZXBBalanceByCustomerId(customerId);
            if (uVO != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO };
            }
            else
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = new BalanceVO() { CustomerId = customerId, Balance = 0 } };
            }
        }

        /// <summary>
        /// 添加或更新会员信息
        /// </summary>
        /// <param name="customerVO">会员VO，根据CustomerID判断是新增还是更新</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateCustomer"), HttpPost]
        public ResultObject UpdateCustomer([FromBody] CustomerVO customerVO, string token)
        {
            CustomerProfile uProfile = new CustomerProfile();
            if (customerVO != null)
            {
                CustomerBO uBO = new CustomerBO(uProfile);
                //判断LoginName 和 CustomerName是否重复
                /*
                if (uBO.IsCustomerExist(customerVO))
                {
                    return new ResultObject() { Flag = 0, Message = "登录名称已存在!", Result = null };
                }*/

                if (customerVO.CustomerId < 1)
                {
                    customerVO.CreatedAt = DateTime.Now;
                    customerVO.CustomerCode = uBO.GetCustomerCode();
                    customerVO.Password = Utilities.GetMD5(customerVO.Password);

                    int customerId = uBO.Add(customerVO);
                    if (customerId > 0)
                    {
                        //发放乐币奖励
                        //if (CacheSystemConfig.GetSystemConfig().zxbRegistered > 0)
                            //uBO.ZXBAddrequire(customerId, CacheSystemConfig.GetSystemConfig().zxbRegistered, CacheSystemConfig.GetSystemConfig().zxbRegistered_text,1);
                        return new ResultObject() { Flag = 1, Message = "添加成功!", Result = customerId };
                    }
                        
                    else
                        return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
                }
                else
                {
                    customerVO.Restore("Password");
                    bool isSuccess = uBO.Update(customerVO);
                    if (isSuccess)
                        return new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
                    else
                        return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
        }

        /// <summary>
        /// 更新会员状态
        /// </summary>
        /// <param name="customerId">会员ID</param>
        /// <param name="status">状态</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateCustomerStatus"), HttpPost]
        public ResultObject UpdateCustomerStatus(string customerId, int status, string token)
        {
            //只能平台账号才能禁用或者启用会员账号
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            if (uProfile.UserId > 0)
            {
                try
                {
                    CustomerBO uBO = new CustomerBO(new CustomerProfile());
                    if (!string.IsNullOrEmpty(customerId))
                    {
                        string[] customerIdArr = customerId.Split(',');
                        bool isAllUpdate = true;

                        for (int i = 0; i < customerIdArr.Length; i++)
                        {
                            try
                            {
                                CustomerVO customerVO = new CustomerVO();
                                customerVO.CustomerId = Convert.ToInt32(customerIdArr[i]);
                                customerVO.Status = status;
                                bool isSuccess = uBO.Update(customerVO);
                            }
                            catch
                            {
                                isAllUpdate = false;
                            }
                        }
                        if (isAllUpdate)
                        {
                            return new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
                        }
                        else
                        {
                            return new ResultObject() { Flag = 1, Message = "部分更新成功!", Result = null };
                        }

                    }
                    else
                    {
                        return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
                    }
                }
                catch
                {
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "没有权限执行此操作!", Result = null };
            }
        }

        /// <summary>
        /// 会员快速注册，匿名
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        [Route("RegisterCustomer"), HttpPost, Anonymous]
        public ResultObject RegisterCustomer(string account, string password)
        {
            CustomerProfile uProfile = new CustomerProfile();
            if (!string.IsNullOrEmpty(account) && !string.IsNullOrEmpty(password))
            {
                CustomerBO uBO = new CustomerBO(uProfile);
                //判断LoginName 和 CustomerName是否重复
                CustomerVO customerVO = new CustomerVO();
                customerVO.CustomerCode = uBO.GetCustomerCode();
                customerVO.CustomerAccount = account;
                //customerVO.CustomerName = account.Substring(0,7)+"****";
                customerVO.CustomerName = account;
                customerVO.Phone = account;
                customerVO.Password = Utilities.GetMD5(password);
                customerVO.Status = 1;
                customerVO.CreatedAt = DateTime.Now;
                if (uBO.IsCustomerExist(customerVO))
                {
                    CustomerVO vo= uBO.FindByParams("CustomerAccount="+ account+ " or Phone="+ account);
                    return new ResultObject() { Flag = 0, Message = "登录名称已存在!", Result = vo.CustomerId };
                }
                int customerId = uBO.Add(customerVO);
                if (customerId > 0)
                {
                    //发放乐币奖励
                    //if (CacheSystemConfig.GetSystemConfig().zxbRegistered > 0)
                    //uBO.ZXBAddrequire(customerId, CacheSystemConfig.GetSystemConfig().zxbRegistered, CacheSystemConfig.GetSystemConfig().zxbRegistered_text,1);

                    //通过认证，IM注册，存在则不添加，不存在则添加
                    try
                    {
                        /*
                        IMBO imBO = new IMBO(new CustomerProfile());
                        imBO.RegisterIMUser(customerId, customerVO.CustomerCode, "$" + customerVO.CustomerCode, customerVO.CustomerName);
                        */
                    }
                    catch
                    {

                    }
                    

                    return new ResultObject() { Flag = 1, Message = "账号注册成功!", Result = customerId };

                }
                else
                    return new ResultObject() { Flag = 0, Message = "账号注册失败!", Result = null };

            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "请输入账号和密码!", Result = null };
            }
        }

        /// <summary>
        /// 会员修改自己密码
        /// </summary>
        /// <param name="customerId">会员ID</param>
        /// <param name="password">旧密码</param>
        /// <param name="newPassword">新密码</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("ChangePassword"), HttpPost]
        public ResultObject ChangePassword(int customerId, string password, string newPassword, string token)
        {
            CustomerBO uBO = new CustomerBO((CustomerProfile)CacheManager.GetUserProfile(token));
            bool result = uBO.ChangePassword(customerId, Utilities.GetMD5(password), Utilities.GetMD5(newPassword));
            if (result)
                return new ResultObject() { Flag = 1, Message = "修改成功!", Result = null };
            else
                return new ResultObject() { Flag = 0, Message = "密码错误，请重新输入!", Result = null };
        }

        /// <summary>
        /// 会员重置密码
        /// </summary>
        /// <param name="phone">会员手机号码</param>
        /// <param name="newPassword">新密码</param>
        /// <returns></returns>
        [Route("ResetPassword"), HttpPost, Anonymous]
        public ResultObject ResetPassword(string phone, string newPassword)
        {
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            CustomerVO cVO = uBO.FindByParams("Phone = '" + phone + "'");
            if (cVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "手机号码不存在!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "旧版本找回密码功能关闭，请前往官网www.zhongxiaole.net修改!", Result = null };
                bool result = uBO.ChangeCustomerPassword(cVO.CustomerId, Utilities.GetMD5(newPassword));
                if (result)
                    return new ResultObject() { Flag = 1, Message = "修改成功!", Result = null };
                else
                    return new ResultObject() { Flag = 0, Message = "修改失败!", Result = null };
            }
        }

        /// <summary>
        /// 平台修改会员密码
        /// </summary>
        /// <param name="customerId">会员ID</param>
        /// <param name="newPassword">新密码</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("ChangeCustomerPassword"), HttpPost]
        public ResultObject ChangeCustomerPassword(int customerId, string newPassword, string token)
        {
            CustomerBO uBO = new CustomerBO((CustomerProfile)CacheManager.GetUserProfile(token));
            bool result = uBO.ChangeCustomerPassword(customerId, Utilities.GetMD5(newPassword));
            if (result)
                return new ResultObject() { Flag = 1, Message = "修改成功!", Result = null };
            else
                return new ResultObject() { Flag = 0, Message = "密码错误，请重新输入!", Result = null };
        }

        /// <summary>
        /// 添加或更新乐币奖励
        /// </summary>
        /// <param name="zxbconfigVO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateZxbConfig"), HttpPost]
        public ResultObject UpdateZxbConfig([FromBody] ZxbConfigVO zxbconfigVO, string token)
        {
            CustomerProfile uProfile = new CustomerProfile();
            if (zxbconfigVO != null)
            {
                CustomerBO uBO = new CustomerBO(new CustomerProfile());
                bool isSuccess = uBO.UpdateZxbConfig(zxbconfigVO);
                if (isSuccess)
                {
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
        }

        /// <summary>
        /// 更新乐币奖励状态
        /// </summary>
        /// <param name="ZxbConfigID">会员ID</param>
        /// <param name="status">状态</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateZxbConfigStatus"), HttpPost]
        public ResultObject UpdateZxbConfigStatus(string ZxbConfigID, int status, string token)
        {
            //只能平台账号才能禁用或者启用会员账号
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            if (uProfile.UserId > 0)
            {
                try
                {
                    CustomerBO uBO = new CustomerBO(new CustomerProfile());
                    if (!string.IsNullOrEmpty(ZxbConfigID))
                    {
                        string[] customerIdArr = ZxbConfigID.Split(',');
                        bool isAllUpdate = true;

                        for (int i = 0; i < customerIdArr.Length; i++)
                        {
                            try
                            {
                                ZxbConfigVO zxbconfigVO = new ZxbConfigVO();
                                zxbconfigVO.ZxbConfigID = Convert.ToInt32(customerIdArr[i]);
                                zxbconfigVO.Status = status;
                                bool isSuccess = uBO.UpdateZxbConfig(zxbconfigVO);
                            }
                            catch
                            {
                                isAllUpdate = false;
                            }
                        }
                        if (isAllUpdate)
                        {
                            return new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
                        }
                        else
                        {
                            return new ResultObject() { Flag = 1, Message = "部分更新成功!", Result = null };
                        }

                    }
                    else
                    {
                        return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
                    }
                }
                catch
                {
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "没有权限执行此操作!", Result = null };
            }
        }

        /// <summary>
        /// 添加或更新雇主信息
        /// </summary>
        /// <param name="businessModelVO">雇主VO，根据BusinessID确定新增还是更新</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateBusiness"), HttpPost]
        public ResultObject UpdateBusiness([FromBody] BusinessModel businessModelVO, string token)
        {
            if (businessModelVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            CustomerProfile uProfile = new CustomerProfile();

            BusinessVO businessVO = businessModelVO.Business;
            List<BusinessCategoryVO> businessCategoryVOList = businessModelVO.BusinessCategory;
            List<TargetCategoryVO> targetCategoryVOList = businessModelVO.TargetCategory;
            List<TargetCityVO> targetCityVOList = businessModelVO.TargetCity;
            List<BusinessIdcardVO> businessIdcardVOList = businessModelVO.BusinessIdCard;

            if (businessVO != null)
            {
                BusinessBO uBO = new BusinessBO(uProfile);
                CustomerBO _bo = new CustomerBO(new CustomerProfile());
                if (businessVO.BusinessId < 1)
                {
                    businessVO.CreatedAt = DateTime.Now;
                    businessVO.RealNameStatus = 0;
                    int businessId = uBO.Add(businessVO, businessCategoryVOList, targetCategoryVOList, targetCityVOList, businessIdcardVOList);
                    if (businessId > 0)
                    {
                        return new ResultObject() { Flag = 1, Message = "添加成功!", Result = businessId };
                    }
                    else
                        return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
                }
                else
                {
                    BusinessViewVO bVO = uBO.FindBusinessById(businessVO.BusinessId);
                    if (bVO.RealNameStatus==1) {
                        businessVO.BusinessLicense = bVO.BusinessLicense;
                        businessVO.BusinessLicenseImg = bVO.BusinessLicenseImg;

                        List<BusinessIdcardVO> IdcardVO = uBO.FindBusinessIdcardByBusiness(businessVO.BusinessId);
                        businessIdcardVOList = IdcardVO;
                    }
                    businessVO.RealNameStatus = bVO.RealNameStatus;
                    bool isSuccess = uBO.Update(businessVO, businessCategoryVOList, targetCategoryVOList, targetCityVOList, businessIdcardVOList);
                    if (isSuccess)
                        return new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
                    else
                        return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
        }
        /// <summary>
        /// 获取关于我们详情
        /// </summary>
        /// <param name="TypeName">标题</param>
        /// <returns></returns>
        [Route("GetAboutus"), HttpGet,Anonymous]
        public ResultObject GetAboutus(string TypeName)
        {
            string html= SiteCommon.GetHelpDocByTypeName(TypeName);
            if (html.Length>0)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = html };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }
        /// <summary>
        /// 获取众销乐规则列表
        /// </summary>
        /// <param name="parentHelpDocTypeId">ID</param>
        /// <returns></returns>
        [Route("GetHelpDocTypeList"), HttpGet, Anonymous]
        public ResultObject GetHelpDocTypeList(int parentHelpDocTypeId)
        {
            List<HelpDocTypeVO> helpTypeList = SiteCommon.GetHelpDocTypeList(parentHelpDocTypeId);
            if (helpTypeList != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = helpTypeList};
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }
        /// <summary>
        /// 获取众销乐规则详情
        /// </summary>
        /// <param name="HelpDocTypeId">ID</param>
        /// <returns></returns>
        [Route("GetHelpDocTypeRules"), HttpGet, Anonymous]
        public ResultObject GetHelpDocTypeRules(int HelpDocTypeId)
        {
            if (HelpDocTypeId > 0)
            {
                HelpDocViewVO vo = SiteCommon.GetHelpDocByTypeId(HelpDocTypeId);
                if (vo != null)
                {
                    return new ResultObject() { Flag = 1, Message = vo.Title, Result = vo.Description};
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
                }
            }else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
            
        }
        /// <summary>
        /// 获取雇主信息
        /// </summary>
        /// <param name="businessId">雇主ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetBusiness"), HttpGet]
        public ResultObject GetBusiness(int businessId, string token)
        {
            BusinessBO uBO = new BusinessBO(new CustomerProfile());
            BusinessViewVO uVO = uBO.FindBusinessById(businessId);
            if (uVO != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取雇主资料完成度
        /// </summary>
        /// <param name="businessId">雇主ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetBusinessCompleted"), HttpGet]
        public ResultObject GetBusinessCompleted(int businessId, string token)
        {
            BusinessBO uBO = new BusinessBO(new CustomerProfile());
            BusinessViewVO uVO = uBO.FindBusinessById(businessId);
            if (uVO != null)
            {
                double Completed = 0;

                if (uVO.CompanyName != "")
                {
                    Completed += 12;
                }
                if (uVO.CityName != "")
                {
                    Completed += 8;
                }
                if (uVO.CategoryNames != "")
                {
                    Completed += 8;
                }
                if (uVO.Address != "")
                {
                    Completed += 4.4;
                }
                if (uVO.CompanySite != "")
                {
                    Completed += 4.4;
                }
                /*if (uVO.Description != "")
                {
                    Completed += 4.4;
                }*/
                if (uVO.MainProducts != "")
                {
                    Completed += 8.8;
                }
                if (uVO.ProductDescription != "")
                {
                    Completed += 4.4;
                }
                if (uVO.CompanyLogo != "")
                {
                    Completed += 12;
                }
                if (uVO.CompanyType != "")
                {
                    Completed += 4.4;
                }
                if (uVO.CompanyTel != "")
                {
                    Completed += 4.4;
                }
                if (uVO.BusinessLicense != "")
                {
                    Completed += 8;
                }
                if (uVO.EntrustImgPath != "")
                {
                    Completed += 4.4;
                }
                /*
                List<BusinessIdcardVO> IdcardVO = uBO.FindBusinessIdcardByBusiness(businessId);
                if (IdcardVO!=null&& IdcardVO.Count>=2) {
                    Completed += 8;
                }
                if (uVO.BusinessLicenseImg != "")
                {
                    Completed += 8;
                }*/
                List<TargetCategoryViewVO> CategoryVO = uBO.FindTargetCategoryByBusiness(businessId);
                if (CategoryVO != null && CategoryVO.Count != 0)
                {
                    Completed += 4.4;
                }
                List<TargetCityViewVO> CityVO = uBO.FindTargetCityByBusiness(businessId);
                if (CityVO != null && CityVO.Count != 0)
                {
                    Completed += 4.4;
                }
                
                CustomerBO _bo = new CustomerBO(new CustomerProfile());
                List<ZxbConfigVO> zVO = _bo.ZXBAddrequirebyCode("雇主认证");

                decimal tBalance = _bo.ZXBFindBalanceByCondition("CustomerId=" + uVO.CustomerId + " and " + "type="+ zVO[0].ZxbConfigID);//已发放雇主认证奖励金额
                decimal nBalance = (zVO[0].Cost / 100) * decimal.Parse(Completed.ToString());//需要发放的金额

                if (nBalance > tBalance)
                {
                    //发放乐币奖励
                    _bo.ZXBAddrequire(uVO.CustomerId, nBalance - tBalance, zVO[0].Purpose, zVO[0].ZxbConfigID);
                }

                //如果所有必填项都填了就自动认证通过
                if (uVO.CompanyName != "" && uVO.CompanyLogo != "" && uVO.MainProducts != "")
                {       
                    int status = 1;
                    try
                    {
                        BusinessVO bVO = new BusinessVO();
                        bVO.BusinessId = Convert.ToInt32(businessId);
                        bVO.Status = status;

                        //获取当前雇主数据
                        BusinessViewVO bViewVO = uBO.FindBusinessById(bVO.BusinessId);

                        bool isSuccess = uBO.Update(bVO);
                        BusinessApproveHistoryVO baHisVO = new BusinessApproveHistoryVO();
                        baHisVO.BusinessId = Convert.ToInt32(businessId);
                        baHisVO.ApproveStatus = status;
                        baHisVO.ApproveDate = DateTime.Now;
                        baHisVO.ApproveComment = "";

                        uBO.AddApproveHistory(baHisVO);

                        //发送站内信息,状态有变化的才发送
                        MessageBO mBO = new MessageBO(new CustomerProfile());
                        if (status != bViewVO.Status)
                        {
                            if (status == 1)
                            {
                                mBO.SendMessage("雇主认证", "  " + bViewVO.CustomerName + ":您的雇主身份已通过！", bViewVO.CustomerId, MessageType.RZSQ);
                                //逻辑修改，将转移到会员注册时添加
                                //通过认证，IM注册，存在则不添加，不存在则添加
                                //IMBO imBO = new IMBO(new CustomerProfile());
                                //imBO.RegisterIMUser(bViewVO.CustomerId, bViewVO.CustomerCode, "$" + bViewVO.CustomerCode, bViewVO.CustomerName);
                            }
                            else if (status == 2)
                            {
                                mBO.SendMessage("雇主认证", "  " + bViewVO.CustomerName + ":您的雇主身份被驳回,请重新上传资料！", bViewVO.CustomerId, MessageType.RZSQ);
                            }
                        }
                    }
                    catch
                    {

                    }
                }
                else {
                    int status = 0;
                    try
                    {
                        BusinessVO bVO = new BusinessVO();
                        bVO.BusinessId = Convert.ToInt32(businessId);
                        bVO.Status = status;

                        //获取当前雇主数据
                        BusinessViewVO bViewVO = uBO.FindBusinessById(bVO.BusinessId);

                        bool isSuccess = uBO.Update(bVO);

                        BusinessApproveHistoryVO baHisVO = new BusinessApproveHistoryVO();
                        baHisVO.BusinessId = Convert.ToInt32(businessId);
                        baHisVO.ApproveStatus = status;
                        baHisVO.ApproveDate = DateTime.Now;
                        baHisVO.ApproveComment = "";

                        uBO.AddApproveHistory(baHisVO);
                    }
                    catch
                    {

                    }
                }
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = Convert.ToInt32(Completed)};
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }
        /// <summary>
        /// 获取雇主份证照片信息
        /// </summary>
        /// <param name="businessId">雇主ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetBusinessIdCardByBusiness"), HttpGet]
        public ResultObject GetBusinessIdCardByBusiness(int businessId, string token)
        {
            BusinessBO uBO = new BusinessBO(new CustomerProfile());
            List<BusinessIdcardVO> uVO = uBO.FindBusinessIdcardByBusiness(businessId);
            if (uVO != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }
        /// <summary>
        /// 获取雇主行业列表
        /// </summary>
        /// <param name="businessId">雇主ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetBusinessCategoryByBusiness"), HttpGet]
        public ResultObject GetBusinessCategoryByBusiness(int businessId, string token)
        {
            BusinessBO uBO = new BusinessBO(new CustomerProfile());
            List<BusinessCategoryViewVO> uVO = uBO.FindBusinessCategoryByBusiness(businessId);
            if (uVO != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取雇主目标行业列表
        /// </summary>
        /// <param name="businessId">雇主ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetTargetCategoryByBusiness"), HttpGet]
        public ResultObject GetTargetCategoryByBusiness(int businessId, string token)
        {
            BusinessBO uBO = new BusinessBO(new CustomerProfile());
            List<TargetCategoryViewVO> uVO = uBO.FindTargetCategoryByBusiness(businessId);
            if (uVO != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取雇主目标城市列表
        /// </summary>
        /// <param name="businessId">雇主ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetTargetCityByBusiness"), HttpGet]
        public ResultObject GetTargetCityByBusiness(int businessId, string token)
        {
            BusinessBO uBO = new BusinessBO(new CustomerProfile());
            List<TargetCityViewVO> uVO = uBO.FindTargetCityByBusiness(businessId);
            if (uVO != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 更新雇主状态，平台审核时使用
        /// </summary>
        /// <param name="businessId">雇主ID</param>
        /// <param name="status">状态</param>
        /// <param name="type">模式：A，资料认证。B，实名认证</param>
        /// <param name="approveComment">原因</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateBusinessStatus"), HttpPost]
        public ResultObject UpdateBusinessStatus(string businessId, int status, string type, string approveComment, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            if (uProfile.UserId > 0)
            {
                try
                {
                    BusinessBO uBO = new BusinessBO(new CustomerProfile());
                    MessageBO mBO = new MessageBO(new CustomerProfile());
                    if (!string.IsNullOrEmpty(businessId))
                    {
                        string[] bIdArr = businessId.Split(',');
                        bool isAllUpdate = true;

                        for (int i = 0; i < bIdArr.Length; i++)
                        {
                            try
                            {
                                BusinessVO bVO = new BusinessVO();
                                bVO.BusinessId = Convert.ToInt32(bIdArr[i]);
                                if (type == "A")
                                {
                                    bVO.Status = status;
                                }else {
                                    bVO.RealNameStatus = status;
                                }

                                //获取当前雇主数据
                                BusinessViewVO bViewVO = uBO.FindBusinessById(bVO.BusinessId);

                                bool isSuccess = uBO.Update(bVO);
                                if (type != "A")
                                {
                                    BusinessApproveHistoryVO baHisVO = new BusinessApproveHistoryVO();
                                    baHisVO.BusinessId = Convert.ToInt32(bIdArr[i]);
                                    baHisVO.ApproveStatus = status;
                                    baHisVO.ApproveDate = DateTime.Now;
                                    baHisVO.ApproveComment = approveComment;
                                    uBO.AddApproveHistory(baHisVO);
                                }
                                   

                                //发送站内信息,状态有变化的才发送
                                if (status != bViewVO.RealNameStatus&& type != "A")
                                {
                                    if (status == 1)
                                    {
                                        mBO.SendMessage("雇主实名认证", "  " + bViewVO.CustomerName + ":您的实名认证已通过！", bViewVO.CustomerId, MessageType.RZSQ);
                                        //逻辑修改，将转移到会员注册时添加
                                        //通过认证，IM注册，存在则不添加，不存在则添加
                                        //IMBO imBO = new IMBO(new CustomerProfile());
                                        //imBO.RegisterIMUser(bViewVO.CustomerId, bViewVO.CustomerCode, "$" + bViewVO.CustomerCode, bViewVO.CustomerName);
                                    }
                                    else if (status == 2)
                                    {
                                        mBO.SendMessage("雇主实名认证", "  " + bViewVO.CustomerName + ":您的实名认证申请被驳回,请重新上传资料！", bViewVO.CustomerId, MessageType.RZSQ);
                                    }
                                }
                            }
                            catch
                            {
                                isAllUpdate = false;
                            }
                        }
                        if (isAllUpdate)
                        {
                            return new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
                        }
                        else
                        {
                            return new ResultObject() { Flag = 1, Message = "部分更新成功!", Result = null };
                        }

                    }
                    else
                    {
                        return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
                    }
                }
                catch
                {
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "没有权限执行此操作!", Result = null };
            }
        }


        /// <summary>
        /// 雇主提交实名认证审核
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateBusinessRealNameStatus"), HttpPost]
        public ResultObject UpdateBusinessRealNameStatus(string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            try
            {
                CustomerProfile cProfile = uProfile as CustomerProfile;  
                int customerId = cProfile.CustomerId;

                CustomerBO cBO = new CustomerBO(new CustomerProfile());
                CustomerViewVO cVO = cBO.FindById(cProfile.CustomerId);
                BusinessBO bBO = new BusinessBO(new CustomerProfile());
                BusinessViewVO bVVO = bBO.FindBusinessById(cVO.BusinessId);

                if (bVVO == null)
                {
                    return new ResultObject() { Flag = 0, Message = "请先上传资料!", Result = null };
                }

                if (bVVO.RealNameStatus == 1) {
                    return new ResultObject() { Flag = 0, Message = "您的实名认证已经通过，不能再次提交审核!", Result = null };
                }
                if (bVVO.BusinessLicense == "")
                {
                    return new ResultObject() { Flag = 0, Message = "请填写您的营业执照号!", Result = null };
                }
                if (bVVO.BusinessLicenseImg == "")
                {
                    return new ResultObject() { Flag = 0, Message = "请上传您的营业执照!", Result = null };
                }
                List<BusinessIdcardVO> IdcardVO = bBO.FindBusinessIdcardByBusiness(cVO.BusinessId);
                /*
                if (IdcardVO == null || IdcardVO.Count < 2)
                {
                    return new ResultObject() { Flag = 0, Message = "请上传您的身份证正反面照片!", Result = null };
                }
                if (IdcardVO.Count >= 2) {
                    if(IdcardVO[0].IDCardImg==""|| IdcardVO[1].IDCardImg=="")
                        return new ResultObject() { Flag = 0, Message = "请上传您的身份证正反面照片!", Result = null };
                }*/
                BusinessVO bVO = new BusinessVO();
                bVO.BusinessId = Convert.ToInt32(cVO.BusinessId);
                bVO.RealNameStatus = 3;

                bool isSuccess = bBO.Update(bVO);

                if (isSuccess) {
                    return new ResultObject() { Flag = 1, Message = "提交成功!", Result = null };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "提交失败!", Result = null };
                }
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "提交失败!", Result = null };
            }
        }

        /// <summary>
        /// 取消雇主实名认证，平台审核时使用
        /// </summary>
        /// <param name="businessId">雇主ID</param>
        /// <param name="approveComment">原因</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("CancelBusinessRZ"), HttpPost]
        public ResultObject CancelBusinessRZ(string businessId, string approveComment, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            if (uProfile.UserId > 0)
            {
                try
                {
                    int status = 2;
                    BusinessBO uBO = new BusinessBO(new CustomerProfile());
                    MessageBO mBO = new MessageBO(new CustomerProfile());
                    if (!string.IsNullOrEmpty(businessId))
                    {
                        string[] bIdArr = businessId.Split(',');
                        bool isAllUpdate = true;

                        for (int i = 0; i < bIdArr.Length; i++)
                        {
                            try
                            {
                                BusinessVO bVO = new BusinessVO();
                                bVO.BusinessId = Convert.ToInt32(bIdArr[i]);
                                bVO.RealNameStatus = status;

                                //获取当前雇主数据
                                BusinessViewVO bViewVO = uBO.FindBusinessById(bVO.BusinessId);

                                //认证通过的才能取消
                                if (bViewVO.RealNameStatus == 1)
                                {
                                    bool isSuccess = uBO.Update(bVO);

                                    BusinessApproveHistoryVO baHisVO = new BusinessApproveHistoryVO();
                                    baHisVO.BusinessId = Convert.ToInt32(bIdArr[i]);
                                    baHisVO.ApproveStatus = status;
                                    baHisVO.ApproveDate = DateTime.Now;
                                    baHisVO.ApproveComment = approveComment;

                                    uBO.AddApproveHistory(baHisVO);

                                    //发送站内信息,状态有变化的才发送
                                    if (status != bViewVO.Status)
                                    {
                                        mBO.SendMessage("认证取消", "  " + bViewVO.CustomerName + ":您的实名认证被取消,请重新上传资料！", bViewVO.CustomerId, MessageType.RZSQ);
                                    }
                                }
                            }
                            catch
                            {
                                isAllUpdate = false;
                            }
                        }
                        if (isAllUpdate)
                        {
                            return new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
                        }
                        else
                        {
                            return new ResultObject() { Flag = 1, Message = "部分更新成功!", Result = null };
                        }

                    }
                    else
                    {
                        return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
                    }
                }
                catch
                {
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "没有权限执行此操作!", Result = null };
            }
        }

        /// <summary>
        /// 添加审核历史记录
        /// </summary>
        /// <param name="businessApproveHistoryVO">审核历史VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("ApproveBusiness"), HttpPost]
        public ResultObject ApproveBusiness([FromBody] BusinessApproveHistoryVO businessApproveHistoryVO, string token)
        {
            CustomerProfile uProfile = new CustomerProfile();
            if (businessApproveHistoryVO != null)
            {
                BusinessBO uBO = new BusinessBO(uProfile);

                int historyId = uBO.AddApproveHistory(businessApproveHistoryVO);
                if (historyId > 0)
                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = historyId };
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
        }

        /// <summary>
        /// 获取雇主最新的审核历史
        /// </summary>
        /// <param name="businessId">雇主ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetBusinessApproveInfo"), HttpGet]
        public ResultObject GetBusinessApproveInfo(int businessId, string token)
        {
            BusinessBO uBO = new BusinessBO(new CustomerProfile());
            BusinessApproveHistoryVO uVO = uBO.FindApproveHistoryByBusiness(businessId);
            if (uVO != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 添加或更新销售信息
        /// </summary>
        /// <param name="agencyModelVO">销售信息VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateAgency"), HttpPost]
        public ResultObject UpdateAgency([FromBody] AgencyModel agencyModelVO, string token)
        {
            if (agencyModelVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            CustomerProfile uProfile = new CustomerProfile();

            AgencyVO agencyVO = agencyModelVO.Agency;
            List<AgencyCategoryVO> agencyCategoryVOList = agencyModelVO.AgencyCategory;
            List<AgencyCityVO> agencyCityVOList = agencyModelVO.AgencyCity;
            List<AgencyIdCardVO> agencyIdCardVOList = agencyModelVO.AgencyIdCard;
            List<AgencyTechnicalVO> agencyTechnicalVOList = agencyModelVO.AgencyTechnical;
            List<AgencySuperClientVO> agencySuperClientVOList = agencyModelVO.AgencySuperClient;
            List<AgencySolutionVO> agencySolutionVOList = agencyModelVO.AgencySolution;

            if (agencyVO != null)
            {
                AgencyBO uBO = new AgencyBO(uProfile);
                if (agencyVO.AgencyId < 1)
                {
                    agencyVO.CreatedAt = DateTime.Now;
                    agencyVO.RealNameStatus = 0;
                    int agencyId = uBO.Add(agencyVO, agencyCategoryVOList, agencyCityVOList, agencyIdCardVOList, agencyTechnicalVOList, agencySuperClientVOList, agencySolutionVOList);
                    if (agencyId > 0)
                    {
                        return new ResultObject() { Flag = 1, Message = "添加成功!", Result = agencyId };
                    }
                    else
                        return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
                }
                else
                {
                    AgencyViewVO aVO = uBO.FindAgencyById(agencyVO.AgencyId);
                    if (aVO.RealNameStatus == 1)
                    {
                        agencyVO.IDCard = aVO.IDCard;

                        List<AgencyIdCardVO> IdCardVO = uBO.FindAgencyIdCardByAgency(aVO.AgencyId);
                        agencyIdCardVOList = IdCardVO;
                    }
                    agencyVO.RealNameStatus = aVO.RealNameStatus;
                    bool isSuccess = uBO.Update(agencyVO, agencyCategoryVOList, agencyCityVOList, agencyIdCardVOList, agencyTechnicalVOList, agencySuperClientVOList, agencySolutionVOList);
                    if (isSuccess)
                        return new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
                    else
                        return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
        }

        /// <summary>
        /// 获取销售信息
        /// </summary>
        /// <param name="agencyId">销售ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetAgency"), HttpGet]
        public ResultObject GetAgency(int agencyId, string token)
        {
            AgencyBO uBO = new AgencyBO(new CustomerProfile());
            AgencyViewVO uVO = uBO.FindAgencyById(agencyId);
            if (uVO != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取销售资料完成度
        /// </summary>
        /// <param name="agencyId">销售ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetAgencyCompleted"), HttpGet]
        public ResultObject GetAgencyCompleted(int agencyId, string token)
        {
            AgencyBO uBO = new AgencyBO(new CustomerProfile());
            AgencyViewVO uVO = uBO.FindAgencyById(agencyId);
            MessageBO mBO = new MessageBO(new CustomerProfile());
            if (uVO != null)
            {
                double Completed = 0;

                if (uVO.AgencyName != "")
                {
                    Completed += 16;
                }
                if (uVO.CityName != "")
                {
                    Completed += 8;
                }
                if (uVO.School != "")
                {
                    Completed += 4.4;
                }
                if (uVO.FamiliarProduct != "")
                {
                    Completed += 4.4;
                }
                if (uVO.Specialty != "")
                {
                    Completed += 4.4;
                }
                if (uVO.Feature != "")
                {
                    Completed += 4.4;
                }
                if (uVO.Company != "")
                {
                    Completed += 4.4;
                }
                if (uVO.Position != "")
                {
                    Completed += 4.4;
                }
                if (uVO.PersonalCard != "")
                {
                    Completed += 16;
                }
                if (uVO.Description != "")
                {
                    Completed += 4.4;
                }
                if (uVO.ShortDescription != "")
                {
                    Completed += 4.4;
                }
                /*
                if (uVO.IDCard != "")
                {
                    Completed += 8;
                }
                List<AgencyIdCardVO> IdCardVO = uBO.FindAgencyIdCardByAgency(agencyId);
                if (IdCardVO != null && IdCardVO.Count >= 2)
                {
                    Completed += 8;
                }
                */
                List<AgencyCityViewVO> CityVO = uBO.FindAgencyCityByAgency(agencyId);
                if (CityVO != null && CityVO.Count > 0)
                {
                    Completed += 8;
                }
                List<AgencyCategoryViewVO> CategoryVO = uBO.FindAgencyCategoryByAgency(agencyId);
                if (CategoryVO != null && CategoryVO.Count > 0)
                {
                    Completed += 8;
                }
                List<AgencyExperienceViewVO> ExperienceVO = uBO.FindAgencyExperienceByAgency(agencyId);
                if (ExperienceVO != null && ExperienceVO.Count > 0)
                {
                    Completed += 4.4;
                }
                List<AgencySolutionVO> SolutionVO = uBO.FindAgencySolutionByAgency(agencyId);
                if (SolutionVO != null && SolutionVO.Count > 0)
                {
                    Completed += 4.4;
                }

                CustomerBO _bo = new CustomerBO(new CustomerProfile());
                List<ZxbConfigVO> zVO = _bo.ZXBAddrequirebyCode("销售认证");

                decimal tBalance=_bo.ZXBFindBalanceByCondition("CustomerId="+ uVO.CustomerId+" and "+ "type="+ zVO[0].ZxbConfigID);//已发放销售认证奖励金额
                decimal nBalance = (zVO[0].Cost / 100) * decimal.Parse(Completed.ToString());//需要发放的金额
                if (nBalance > tBalance) {
                    //发放乐币奖励
                    _bo.ZXBAddrequire(uVO.CustomerId, nBalance-tBalance, zVO[0].Purpose, zVO[0].ZxbConfigID);
                }


                //如果所有必填项都填了就自动认证通过
                if (uVO.AgencyName != "" && uVO.PersonalCard != "" && uVO.FamiliarProduct != "" &&(uVO.Description != ""|| uVO.ShortDescription != ""))
                {
                    int status = 1;
                    try
                    {
                        AgencyVO bVO = new AgencyVO();
                        bVO.AgencyId = Convert.ToInt32(agencyId);
                        bVO.Status = status;

                        //获取当前销售数据
                        AgencyViewVO bViewVO = uBO.FindAgencyById(bVO.AgencyId);

                        bool isSuccess = uBO.Update(bVO);

                        AgencyApproveHistoryVO baHisVO = new AgencyApproveHistoryVO();
                        baHisVO.AgencyId = Convert.ToInt32(agencyId);
                        baHisVO.ApproveStatus = status;
                        baHisVO.ApproveDate = DateTime.Now;

                        uBO.AddApproveHistory(baHisVO);

                        //发送站内信息,状态有变化的才发送
                        if (status != bViewVO.Status)
                        {

                            if (status == 1)
                            {
                                mBO.SendMessage("销售认证申请", "  " + bViewVO.CustomerName + ":您的申请已通过！", bViewVO.CustomerId, MessageType.RZSQ);
                                //逻辑修改，将转移到会员注册时添加
                                //通过认证，IM注册，存在则不添加，不存在则添加
                                //IMBO imBO = new IMBO(new CustomerProfile());
                                //imBO.RegisterIMUser(bViewVO.CustomerId, bViewVO.CustomerCode, "$" + bViewVO.CustomerCode, bViewVO.CustomerName);
                            }
                            else if (status == 2)
                                mBO.SendMessage("销售认证申请", "  " + bViewVO.CustomerName + ":您的申请被驳回,请重新上传资料！", bViewVO.CustomerId, MessageType.RZSQ);
                        }
                    }
                    catch
                    {

                    }
                }
                else {
                    int status = 0;
                    try
                    {
                        AgencyVO bVO = new AgencyVO();
                        bVO.AgencyId = Convert.ToInt32(agencyId);
                        bVO.Status = status;

                        //获取当前销售数据
                        AgencyViewVO bViewVO = uBO.FindAgencyById(bVO.AgencyId);

                        bool isSuccess = uBO.Update(bVO);

                        AgencyApproveHistoryVO baHisVO = new AgencyApproveHistoryVO();
                        baHisVO.AgencyId = Convert.ToInt32(agencyId);
                        baHisVO.ApproveStatus = status;
                        baHisVO.ApproveDate = DateTime.Now;

                        uBO.AddApproveHistory(baHisVO);
                    }
                    catch
                    {

                    }
                }
                
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = Convert.ToInt32(Completed) };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取销售行业列表
        /// </summary>
        /// <param name="agencyId">销售ID</param> 
        /// <param name="token">口令</param>     
        /// <returns></returns>
        [Route("GetAgencyCategoryByAgency"), HttpGet]
        public ResultObject GetAgencyCategoryByAgency(int agencyId, string token)
        {
            AgencyBO uBO = new AgencyBO(new CustomerProfile());
            List<AgencyCategoryViewVO> uVO = uBO.FindAgencyCategoryByAgency(agencyId);
            if (uVO != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取销售行业列表 匿名
        /// </summary>
        /// <param name="agencyId">销售ID</param>       
        /// <returns></returns>
        [Route("GetAgencyCategoryByAgencySite"), HttpGet, Anonymous]
        public ResultObject GetAgencyCategoryByAgencySite(int agencyId)
        {
            AgencyBO uBO = new AgencyBO(new CustomerProfile());
            List<AgencyCategoryViewVO> uVO = uBO.FindAgencyCategoryByAgency(agencyId);
            if (uVO != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取销售城市列表
        /// </summary>
        /// <param name="agencyId">销售ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetAgencyCityByAgency"), HttpGet]
        public ResultObject GetAgencyCityByAgency(int agencyId, string token)
        {
            AgencyBO uBO = new AgencyBO(new CustomerProfile());
            List<AgencyCityViewVO> uVO = uBO.FindAgencyCityByAgency(agencyId);
            if (uVO != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取销售城市列表 匿名
        /// </summary>
        /// <param name="agencyId">销售ID</param>     
        /// <returns></returns>
        [Route("GetAgencyCityByAgencySite"), HttpGet, Anonymous]
        public ResultObject GetAgencyCityByAgencySite(int agencyId)
        {
            AgencyBO uBO = new AgencyBO(new CustomerProfile());
            List<AgencyCityViewVO> uVO = uBO.FindAgencyCityByAgency(agencyId);
            if (uVO != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 销售提交实名认证审核
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateAgencyRealNameStatus"), HttpPost]
        public ResultObject UpdateAgencyRealNameStatus(string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            try
            {
                CustomerProfile cProfile = uProfile as CustomerProfile;
                int customerId = cProfile.CustomerId;

                CustomerBO cBO = new CustomerBO(new CustomerProfile());
                CustomerViewVO cVO = cBO.FindById(cProfile.CustomerId);

                AgencyBO aBO=new AgencyBO(new CustomerProfile());
                AgencyViewVO aVVO = aBO.FindAgencyById(cVO.AgencyId);

                if (aVVO == null) {
                    return new ResultObject() { Flag = 0, Message = "请先上传资料!", Result = null };
                }
                if (aVVO.RealNameStatus == 1)
                {
                    return new ResultObject() { Flag = 0, Message = "您的实名认证已经通过，不能再次提交审核!", Result = null };
                }
                if (aVVO.IDCard == "")
                {
                    return new ResultObject() { Flag = 0, Message = "请填写您的身份证号码!", Result = null };
                }
                List<AgencyIdCardVO> IdCardVO = aBO.FindAgencyIdCardByAgency(cVO.AgencyId);
                if (IdCardVO == null || IdCardVO.Count < 2)
                {
                    return new ResultObject() { Flag = 0, Message = "请上传您的身份证正反面照片!", Result = null };
                }
                if (IdCardVO.Count >= 2)
                {
                    if (IdCardVO[0].IDCardImg == "" || IdCardVO[1].IDCardImg == "")
                    return new ResultObject() { Flag = 0, Message = "请上传您的身份证正反面照片!", Result = null };
                }
                
                BusinessVO bVO = new BusinessVO();
                AgencyVO aVO = new AgencyVO();
                aVO.AgencyId = Convert.ToInt32(cVO.AgencyId);
                aVO.RealNameStatus = 3;

                bool isSuccess = aBO.Update(aVO);
                if (isSuccess)
                {
                    return new ResultObject() { Flag = 1, Message = "提交成功!", Result = null };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "提交失败!", Result = null };
                }
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "提交失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取销售身份证照片信息
        /// </summary>
        /// <param name="agencyId">销售ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetAgencyIdCardByAgency"), HttpGet]
        public ResultObject GetAgencyIdCardByAgency(int agencyId, string token)
        {
            AgencyBO uBO = new AgencyBO(new CustomerProfile());
            List<AgencyIdCardVO> uVO = uBO.FindAgencyIdCardByAgency(agencyId);
            if (uVO != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取销售技能照片信息
        /// </summary>
        /// <param name="agencyId">销售ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetAgencyTechnicalByAgency"), HttpGet]
        public ResultObject GetAgencyTechnicalByAgency(int agencyId, string token)
        {
            AgencyBO uBO = new AgencyBO(new CustomerProfile());
            List<AgencyTechnicalVO> uVO = uBO.FindAgencyTechnicalByAgency(agencyId);
            if (uVO != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 取消销售认证，平台使用
        /// </summary>
        /// <param name="agencyId">销售ID</param>
        /// <param name="approveComment">原因</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("CancelAgencyRZ"), HttpPost]
        public ResultObject CancelAgencyRZ(string agencyId, string approveComment, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            if (uProfile.UserId > 0)
            {
                try
                {
                    int status = 2;
                    AgencyBO uBO = new AgencyBO(new CustomerProfile());
                    MessageBO mBO = new MessageBO(new CustomerProfile());
                    if (!string.IsNullOrEmpty(agencyId))
                    {
                        string[] bIdArr = agencyId.Split(',');
                        bool isAllUpdate = true;

                        for (int i = 0; i < bIdArr.Length; i++)
                        {
                            try
                            {
                                AgencyVO bVO = new AgencyVO();
                                bVO.AgencyId = Convert.ToInt32(bIdArr[i]);
                                bVO.RealNameStatus = status;

                                //获取当前销售数据
                                AgencyViewVO bViewVO = uBO.FindAgencyById(bVO.AgencyId);

                                //认证通过的才能取消
                                if (bViewVO.RealNameStatus == 1)
                                {
                                    bool isSuccess = uBO.Update(bVO);

                                    AgencyApproveHistoryVO baHisVO = new AgencyApproveHistoryVO();
                                    baHisVO.AgencyId = Convert.ToInt32(bIdArr[i]);
                                    baHisVO.ApproveStatus = status;
                                    baHisVO.ApproveDate = DateTime.Now;
                                    baHisVO.ApproveComment = approveComment;

                                    uBO.AddApproveHistory(baHisVO);

                                    //发送站内信息,状态有变化的才发送
                                    if (status != bViewVO.RealNameStatus)
                                    {
                                        mBO.SendMessage("销售实名认证取消", "  " + bViewVO.CustomerName + ":您的实名认证被取消,请重新上传资料！", bViewVO.CustomerId, MessageType.RZSQ);
                                    }
                                }
                            }
                            catch
                            {
                                isAllUpdate = false;
                            }
                        }
                        if (isAllUpdate)
                        {
                            return new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
                        }
                        else
                        {
                            return new ResultObject() { Flag = 1, Message = "部分更新成功!", Result = null };
                        }

                    }
                    else
                    {
                        return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
                    }
                }
                catch
                {
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "没有权限执行此操作!", Result = null };
            }
        }

        /// <summary>
        /// 销售状态修改，平台使用
        /// </summary>
        /// <param name="agencyId">销售ID</param>
        /// <param name="status">状态</param>
        /// <param name="type">模式：A，资料认证。B，实名认证</param>
        /// <param name="approveComment">原因</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateAgencyStatus"), HttpPost]
        public ResultObject UpdateAgencyStatus(string agencyId, int status,string type, string approveComment, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            if (uProfile.UserId > 0)
            {
                try
                {
                    AgencyBO uBO = new AgencyBO(new CustomerProfile());
                    MessageBO mBO = new MessageBO(new CustomerProfile());
                    if (!string.IsNullOrEmpty(agencyId))
                    {
                        string[] bIdArr = agencyId.Split(',');
                        bool isAllUpdate = true;

                        for (int i = 0; i < bIdArr.Length; i++)
                        {
                            try
                            {
                                AgencyVO bVO = new AgencyVO();
                                bVO.AgencyId = Convert.ToInt32(bIdArr[i]);
                                if (type == "A")
                                {
                                    bVO.Status = status;
                                }
                                else {
                                    bVO.RealNameStatus = status;
                                }
                                

                                //获取当前销售数据
                                AgencyViewVO bViewVO = uBO.FindAgencyById(bVO.AgencyId);

                                bool isSuccess = uBO.Update(bVO);
                                if (type != "A")
                                {
                                    AgencyApproveHistoryVO baHisVO = new AgencyApproveHistoryVO();
                                    baHisVO.AgencyId = Convert.ToInt32(bIdArr[i]);
                                    baHisVO.ApproveStatus = status;
                                    baHisVO.ApproveDate = DateTime.Now;
                                    baHisVO.ApproveComment = approveComment;

                                    uBO.AddApproveHistory(baHisVO);
                                }
                                //发送站内信息,状态有变化的才发送
                                if (status != bViewVO.RealNameStatus && type != "A")
                                {

                                    if (status == 1)
                                    {
                                        mBO.SendMessage("销售实名认证", "  " + bViewVO.CustomerName + ":您的实名认证申请已通过！", bViewVO.CustomerId, MessageType.RZSQ);
                                        //逻辑修改，将转移到会员注册时添加
                                        //通过认证，IM注册，存在则不添加，不存在则添加
                                        //IMBO imBO = new IMBO(new CustomerProfile());
                                        //imBO.RegisterIMUser(bViewVO.CustomerId, bViewVO.CustomerCode, "$" + bViewVO.CustomerCode, bViewVO.CustomerName);
                                    }
                                    else if (status == 2)
                                        mBO.SendMessage("销售实名认证", "  " + bViewVO.CustomerName + ":您的实名认证申请被驳回,请重新上传资料！", bViewVO.CustomerId, MessageType.RZSQ);
                                }
                            }
                            catch
                            {
                                isAllUpdate = false;
                            }
                        }
                        if (isAllUpdate)
                        {
                            return new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
                        }
                        else
                        {
                            return new ResultObject() { Flag = 1, Message = "部分更新成功!", Result = null };
                        }

                    }
                    else
                    {
                        return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
                    }
                }
                catch
                {
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "没有权限执行此操作!", Result = null };
            }
        }

        /// <summary>
        /// 添加销售审核历史
        /// </summary>
        /// <param name="agencyApproveHistoryVO">审核历史VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("ApproveAgency"), HttpPost]
        public ResultObject ApproveAgency([FromBody] AgencyApproveHistoryVO agencyApproveHistoryVO, string token)
        {
            CustomerProfile uProfile = new CustomerProfile();
            if (agencyApproveHistoryVO != null)
            {
                AgencyBO uBO = new AgencyBO(uProfile);

                int historyId = uBO.AddApproveHistory(agencyApproveHistoryVO);
                if (historyId > 0)
                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = historyId };
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
        }

        /// <summary>
        /// 获取销售最新的审核历史
        /// </summary>
        /// <param name="agencyId">销售ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetAgencyApproveInfo"), HttpGet]
        public ResultObject GetAgencyApproveInfo(int agencyId, string token)
        {
            AgencyBO uBO = new AgencyBO(new CustomerProfile());
            AgencyApproveHistoryVO uVO = uBO.FindApproveHistoryByAgency(agencyId);
            if (uVO != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 添加或更新销售项目经验
        /// </summary>
        /// <param name="agencyExperienceVO">销售经验VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateAgencyExperience"), HttpPost]
        public ResultObject UpdateAgencyExperience([FromBody] AgencyExperienceVO agencyExperienceVO, string token)
        {
            if (agencyExperienceVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            CustomerProfile uProfile = CacheManager.GetUserProfile(token) as CustomerProfile;



            AgencyBO uBO = new AgencyBO(uProfile);

            if (agencyExperienceVO.AgencyExperienceId < 1)
            {
                int customerId = uProfile.CustomerId;
                AgencyViewVO tVO = uBO.FindAgencyByCustomerId(customerId);
                if (tVO != null)
                    agencyExperienceVO.AgencyId = tVO.AgencyId;
                int agencyExperienceId = uBO.AddAgencyExperience(agencyExperienceVO, agencyExperienceVO.AgencyExperienceImageList);
                if (agencyExperienceId > 0)
                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = agencyExperienceId };
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
            else
            {
                bool isSuccess = uBO.UpdateAgencyExperience(agencyExperienceVO, agencyExperienceVO.AgencyExperienceImageList);
                if (isSuccess)
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
                else
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }

        }

        /// <summary>
        /// 获取销售项目经验列表
        /// </summary>
        /// <param name="agencyId">销售ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetAgencyExperienceByAgency"), HttpGet]
        public ResultObject GetAgencyExperienceByAgency(int agencyId, string token)
        {
            AgencyBO uBO = new AgencyBO(new CustomerProfile());
            List<AgencyExperienceViewVO> uVO = uBO.FindAgencyExperienceByAgency(agencyId);
            if (uVO != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取销售项目经验
        /// </summary>
        /// <param name="agencyExperienceId">销售项目经验ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetAgencyExperience"), HttpGet]
        public ResultObject GetAgencyExperience(int agencyExperienceId, string token)
        {
            AgencyBO uBO = new AgencyBO(new CustomerProfile());
            AgencyExperienceViewVO uVO = uBO.FindAgencyExperienceById(agencyExperienceId);
            if (uVO != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 删除销售项目经验
        /// </summary>
        /// <param name="agencyExperienceId">销售项目经验ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DeleteAgencyExperience"), HttpPost]
        public ResultObject DeleteAgencyExperience(string agencyExperienceId, string token)
        {
            try
            {
                if (!string.IsNullOrEmpty(agencyExperienceId))
                {
                    string[] aeIdArr = agencyExperienceId.Split(',');
                    bool isAllDelete = true;
                    AgencyBO uBO = new AgencyBO(new CustomerProfile());
                    for (int i = 0; i < aeIdArr.Length; i++)
                    {
                        try
                        {
                            uBO.DeleteAgencyExperience(Convert.ToInt32(aeIdArr[i]));
                        }
                        catch
                        {
                            isAllDelete = false;
                        }
                    }
                    if (isAllDelete)
                    {
                        return new ResultObject() { Flag = 1, Message = "删除成功!", Result = null };
                    }
                    else
                    {
                        return new ResultObject() { Flag = 1, Message = "部分删除成功!", Result = null };
                    }

                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
                }
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
            }
        }

        /// <summary>
        /// 取消销售项目经验审核，平台使用
        /// </summary>
        /// <param name="agencyExperienceId">销售项目经验ID</param>
        /// <param name="approveComment">原因</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("CancelAgencyExperience"), HttpPost]
        public ResultObject CancelAgencyExperience(string agencyExperienceId, string approveComment, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            if (uProfile.UserId > 0)
            {
                try
                {
                    int status = 2;
                    AgencyBO uBO = new AgencyBO(new CustomerProfile());
                    MessageBO mBO = new MessageBO(new CustomerProfile());
                    if (!string.IsNullOrEmpty(agencyExperienceId))
                    {
                        string[] bIdArr = agencyExperienceId.Split(',');
                        bool isAllUpdate = true;

                        for (int i = 0; i < bIdArr.Length; i++)
                        {
                            try
                            {
                                AgencyExperienceVO bVO = new AgencyExperienceVO();
                                bVO.AgencyExperienceId = Convert.ToInt32(bIdArr[i]);
                                bVO.Status = status;

                                //获取当前销售数据
                                AgencyExperienceViewVO aVO = uBO.FindAgencyExperienceById(bVO.AgencyExperienceId);
                                AgencyViewVO bViewVO = uBO.FindAgencyById(aVO.AgencyId);



                                //认证通过的才能取消
                                if (aVO.Status == 1)
                                {
                                    bool isSuccess = uBO.UpdateAgencyExperience(bVO);

                                    AgencyExperienceApproveHistoryVO baHisVO = new AgencyExperienceApproveHistoryVO();
                                    baHisVO.AgencyExperienceId = Convert.ToInt32(bIdArr[i]);
                                    baHisVO.ApproveStatus = status;
                                    baHisVO.ApproveDate = DateTime.Now;
                                    baHisVO.ApproveComment = approveComment;

                                    uBO.AddExperienceApproveHistory(baHisVO);

                                    //发送站内信息,状态有变化的才发送
                                    if (status != bViewVO.Status)
                                    {
                                        mBO.SendMessage("销售案例申请取消", "  " + bViewVO.CustomerName + ":您的销售案例被取消,请重新上传资料！", bViewVO.CustomerId, MessageType.RZSQ);
                                    }
                                }
                            }
                            catch
                            {
                                isAllUpdate = false;
                            }
                        }
                        if (isAllUpdate)
                        {
                            return new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
                        }
                        else
                        {
                            return new ResultObject() { Flag = 1, Message = "部分更新成功!", Result = null };
                        }

                    }
                    else
                    {
                        return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
                    }
                }
                catch
                {
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "没有权限执行此操作!", Result = null };
            }
        }

        /// <summary>
        /// 销售项目经验状态修改，平台使用
        /// </summary>
        /// <param name="agencyExperienceId">销售项目经验ID</param>
        /// <param name="status">状态</param>
        /// <param name="approveComment">原因</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateAgencyExperienceStatus"), HttpPost]
        public ResultObject UpdateAgencyExperienceStatus(string agencyExperienceId, int status, string approveComment, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            if (uProfile.UserId > 0)
            {
                try
                {
                    AgencyBO uBO = new AgencyBO(new CustomerProfile());
                    MessageBO mBO = new MessageBO(new CustomerProfile());
                    if (!string.IsNullOrEmpty(agencyExperienceId))
                    {
                        string[] bIdArr = agencyExperienceId.Split(',');
                        bool isAllUpdate = true;

                        for (int i = 0; i < bIdArr.Length; i++)
                        {
                            try
                            {
                                AgencyExperienceVO bVO = new AgencyExperienceVO();
                                bVO.AgencyExperienceId = Convert.ToInt32(bIdArr[i]);
                                bVO.Status = status;

                                //获取当前销售数据
                                AgencyExperienceViewVO aVO = uBO.FindAgencyExperienceById(bVO.AgencyExperienceId);
                                AgencyViewVO bViewVO = uBO.FindAgencyById(aVO.AgencyId);

                                bool isSuccess = uBO.UpdateAgencyExperience(bVO);

                                AgencyExperienceApproveHistoryVO baHisVO = new AgencyExperienceApproveHistoryVO();
                                baHisVO.AgencyExperienceId = Convert.ToInt32(bIdArr[i]);
                                baHisVO.ApproveStatus = status;
                                baHisVO.ApproveDate = DateTime.Now;
                                baHisVO.ApproveComment = approveComment;

                                uBO.AddExperienceApproveHistory(baHisVO);

                                //发送站内信息,状态有变化的才发送
                                if (status != aVO.Status)
                                {

                                    if (status == 1)
                                    {
                                        mBO.SendMessage("销售案例审核", "  " + bViewVO.CustomerName + ":您的申请已通过！", bViewVO.CustomerId, MessageType.RZSQ);
                                    }
                                    else if (status == 2)
                                        mBO.SendMessage("销售案例审核", "  " + bViewVO.CustomerName + ":您的申请被驳回,请重新上传资料！", bViewVO.CustomerId, MessageType.RZSQ);
                                }
                            }
                            catch
                            {
                                isAllUpdate = false;
                            }
                        }
                        if (isAllUpdate)
                        {
                            return new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
                        }
                        else
                        {
                            return new ResultObject() { Flag = 1, Message = "部分更新成功!", Result = null };
                        }

                    }
                    else
                    {
                        return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
                    }
                }
                catch
                {
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "没有权限执行此操作!", Result = null };
            }
        }

        /// <summary>
        /// 添加销售项目经验审核历史
        /// </summary>
        /// <param name="agencyExperienceApproveHistoryVO">审核历史VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("ApproveAgencyExperience"), HttpPost]
        public ResultObject ApproveAgencyExperience([FromBody] AgencyExperienceApproveHistoryVO agencyExperienceApproveHistoryVO, string token)
        {
            CustomerProfile uProfile = new CustomerProfile();
            if (agencyExperienceApproveHistoryVO != null)
            {
                AgencyBO uBO = new AgencyBO(uProfile);

                int historyId = uBO.AddExperienceApproveHistory(agencyExperienceApproveHistoryVO);
                if (historyId > 0)
                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = historyId };
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
        }

        /// <summary>
        /// 获取销售案例最新的审核历史
        /// </summary>
        /// <param name="agencyExperienceId">销售案例ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetAgencyExperienceApproveInfo"), HttpGet]
        public ResultObject GetAgencyExperienceApproveInfo(int agencyExperienceId, string token)
        {
            AgencyBO uBO = new AgencyBO(new CustomerProfile());
            AgencyExperienceApproveHistoryVO uVO = uBO.FindExperienceApproveHistoryByAgencyExperience(agencyExperienceId);
            if (uVO != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取雇主对销售的评价
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="type">1.雇主 2销售</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetAgencyReviewList"), HttpPost]
        public ResultObject GetAgencyReviewList([FromBody] ConditionModel condition,int type, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            if (condition == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            else if (condition.Filter == null || condition.PageInfo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            string conditionStr = "";
            if (type == 1)
            {
                conditionStr = " BusinessCustomerId = " + cProfile.CustomerId;
            }else
            {
                conditionStr = " AgencyCustomerId = " + cProfile.CustomerId;
            }
            conditionStr += " and " + condition.Filter.Result();
            Paging pageInfo = condition.PageInfo;
            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            List<AgencyReviewViewVO> list = uBO.FindAgencyReviewByPageIndex(conditionStr, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list };
        }

        /// <summary>
        /// 获取销售对雇主的评价
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="type">1.雇主 2销售</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetBusinessReviewList"), HttpPost]
        public ResultObject GetBusinessReviewList([FromBody] ConditionModel condition, int type, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            if (condition == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            else if (condition.Filter == null || condition.PageInfo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            string conditionStr = "";
            if (type == 1)
            {
                conditionStr = " BusinessCustomerId = " + cProfile.CustomerId;
            }
            else
            {
                conditionStr = " AgencyCustomerId = " + cProfile.CustomerId;
            }
            conditionStr += " and " + condition.Filter.Result();
            Paging pageInfo = condition.PageInfo;
            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            List<BusinessReviewViewVO> list = uBO.FindBusinessReviewByPageIndex(conditionStr, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list };
        }

        /// <summary>
        /// 添加关注
        /// </summary>
        /// <param name="markVO">关注VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateMark"), HttpPost]
        public ResultObject UpdateMark([FromBody] MarkVO markVO, string token)
        {
            if (markVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            CustomerProfile uProfile = CacheManager.GetUserProfile(token) as CustomerProfile;

            CustomerBO uBO = new CustomerBO(uProfile);

            markVO.CustomerId = uProfile.CustomerId;
            //判断是否存在，存在则不添加
            MarkVO mVO = uBO.FindMark(markVO.MarkObjectId, markVO.MarkType, markVO.CustomerId);
            int markId = 1;
            if (mVO == null)
            {
                markId = uBO.AddMark(markVO);
            }
            if (markId > 0)
                return new ResultObject() { Flag = 1, Message = "添加成功!", Result = markId };
            else
                return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };


        }

        /// <summary>
        /// 取消关注
        /// </summary>
        /// <param name="markId">关注ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DeleteMark"), HttpPost]
        public ResultObject DeleteMark(int markId, string token)
        {
            CustomerProfile uProfile = CacheManager.GetUserProfile(token) as CustomerProfile;

            CustomerBO uBO = new CustomerBO(uProfile);
            MarkVO markVO = new MarkVO();
            markVO.MarkId = markId;

            if (uBO.DeleteMark(markVO))
                return new ResultObject() { Flag = 1, Message = "取消成功!", Result = null };
            else
                return new ResultObject() { Flag = 0, Message = "取消失败!", Result = null };

        }

        /// <summary>
        /// 判断是否已经关注
        /// </summary>
        /// <param name="markObjectId">关注ID</param>
        /// <param name="markType">关注类型</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("IsMarked"), HttpPost]
        public ResultObject IsMarked(int markObjectId, int markType, string token)
        {
            CustomerProfile uProfile = CacheManager.GetUserProfile(token) as CustomerProfile;

            CustomerBO uBO = new CustomerBO(uProfile);

            MarkVO markVO = uBO.FindMark(markObjectId, markType, uProfile.CustomerId);

            if (markVO != null)
                return new ResultObject() { Flag = 1, Message = "已经关注!", Result = markVO.MarkId };
            else
                return new ResultObject() { Flag = 0, Message = "未关注!", Result = null };

        }

        /// <summary>
        /// 添加APP别名,每个使用APP的都需要添加
        /// </summary>
        /// <param name="aliasVO">别名VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("AddAlias"), HttpPost, Anonymous]
        public ResultObject AddAlias([FromBody] AliasVO aliasVO)
        {
            CustomerProfile uProfile = new CustomerProfile();
            MessageBO uBO = new MessageBO(uProfile);
            int aliasId = uBO.AddAlias(aliasVO);
            if (aliasId > 0)
                return new ResultObject() { Flag = 1, Message = "添加成功!", Result = aliasId };
            else
                return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };

        }

        /// <summary>
        /// 添加APP和会员的关联，登录之后添加
        /// </summary>
        /// <param name="alias">别名</param>
        /// <param name="customerId">会员ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("AddAliasMapping"), HttpPost]
        public ResultObject AddAliasMapping(string alias, int customerId, string token)
        {
            CustomerProfile uProfile = CacheManager.GetUserProfile(token) as CustomerProfile;

            MessageBO uBO = new MessageBO(uProfile);

            int aliasId = uBO.AddAliasMapping(alias, customerId);
            if (aliasId > 0)
                return new ResultObject() { Flag = 1, Message = "添加成功!", Result = aliasId };
            else
                return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };

        }

        /// <summary>
        /// 删除APP和会员的关联，注销之后删除
        /// </summary>
        /// <param name="alias">别名</param>
        /// <param name="customerId">会员ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DeleteAliasMapping"), HttpPost]
        public ResultObject DeleteAliasMapping(string alias, int customerId, string token)
        {
            CustomerProfile uProfile = CacheManager.GetUserProfile(token) as CustomerProfile;

            MessageBO uBO = new MessageBO(uProfile);

            if (uBO.DeleteAliasMapping(alias, customerId))
                return new ResultObject() { Flag = 1, Message = "删除成功!", Result = null };
            else
                return new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };

        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="messageVO">消息VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("SendMessage"), HttpPost]
        public ResultObject SendMessage([FromBody] MessageVO messageVO, string token)
        {
            CustomerProfile uProfile = CacheManager.GetUserProfile(token) as CustomerProfile;

            MessageBO uBO = new MessageBO(uProfile);

            int messageId = uBO.AddMessage(messageVO);
            if (messageId > 0)
                return new ResultObject() { Flag = 1, Message = "发送成功!", Result = messageId };
            else
                return new ResultObject() { Flag = 0, Message = "发送失败!", Result = null };

        }

        /// <summary>
        /// 获取消息具体信息
        /// </summary>
        /// <param name="messageId">消息ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetMessage"), HttpGet]
        public ResultObject GetMessage(int messageId, string token)
        {
            CustomerProfile uProfile = CacheManager.GetUserProfile(token) as CustomerProfile;

            MessageBO uBO = new MessageBO(uProfile);

            MessageViewVO vo = uBO.FindMessageId(messageId);
            if (vo != null)
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = vo };
            else
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };

        }

        /// <summary>
        /// 获取银行信息信息
        /// </summary>
        /// <param name="BankAccountId">银行ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetBankInfoByBankAccountId"), HttpGet]
        public ResultObject GetBankInfoByBankAccountId(int BankAccountId, string token)
        {
            CustomerProfile uProfile = CacheManager.GetUserProfile(token) as CustomerProfile;

            CustomerBO uBO = new CustomerBO(uProfile);

            BankAccountVO vo = uBO.FindBankAccountById(BankAccountId);
            if (vo != null)
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = vo };
            else
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };

        }


        /// <summary>
        /// 添加银行账户信息
        /// </summary>
        /// <param name="bankAccountVO">账户信息</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("AddBankAccount"), HttpPost]
        public ResultObject AddBankAccount([FromBody] BankAccountVO bankAccountVO, string token)
        {
            CustomerProfile uProfile = CacheManager.GetUserProfile(token) as CustomerProfile;
            CustomerBO uBO = new CustomerBO(uProfile);
            int bankAccountId = uBO.AddBankAccount(bankAccountVO);
            if (bankAccountId > 0)
                return new ResultObject() { Flag = 1, Message = "添加成功!", Result = bankAccountId };
            else
                return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };

        }


        /// <summary>
        /// 添加提现记录信息
        /// </summary>
        /// <param name="payoutHistoryVO">账户信息</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("AddPayoutHistory"), HttpPost]
        public ResultObject AddPayoutHistory([FromBody] PayoutHistoryVO payoutHistoryVO, string token)
        {
            CustomerProfile uProfile = CacheManager.GetUserProfile(token) as CustomerProfile;
            CustomerBO uBO = new CustomerBO(uProfile);

            BalanceVO uVO = uBO.FindBalanceByCustomerId(uProfile.CustomerId);
            if (uVO.Balance < payoutHistoryVO.Cost) {
                return new ResultObject() { Flag = 0, Message = "余额不足!", Result = null };
            }

            payoutHistoryVO.PayOutDate = DateTime.Now;
            int payoutHistoryId = uBO.AddPayoutHistoryVO(payoutHistoryVO);
            if (payoutHistoryId > 0)
                return new ResultObject() { Flag = 1, Message = "添加成功!", Result = payoutHistoryId };
            else
                return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
        }

        /// <summary>
        /// 每日分享
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DailySharing"), HttpPost]
        public ResultObject DailySharing(string token)
        {
            CustomerProfile uProfile = CacheManager.GetUserProfile(token) as CustomerProfile;
            CustomerBO uBO = new CustomerBO(uProfile);

            List<ZxbConfigVO> zVO = uBO.ZXBAddrequirebyCode("每日分享");
            if (uBO.ZXBFindRequireCount("CustomerId = " + uProfile.CustomerId + " and type=" + zVO[0].ZxbConfigID + " and date(Date) = curdate()") == 0)
            {
                //发放乐币奖励
                if (uBO.ZXBAddrequire(uProfile.CustomerId, zVO[0].Cost, zVO[0].Purpose, zVO[0].ZxbConfigID))
                {
                    return new ResultObject() { Flag = 1, Message = "分享奖励，乐币 + "+ zVO[0].Cost + "!", Result = null };
                }
                else {
                    return new ResultObject() { Flag = 0, Message = "奖励发放失败!", Result = null };
                }
            }
            else {
                return new ResultObject() { Flag = 0, Message = "每天分享只能奖励一次!", Result = null };
            }
        }

        /// <summary>
        /// 删除消息
        /// </summary>
        /// <param name="messageId">消息ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DeleteMessage"), HttpGet]
        public ResultObject DeleteMessage(string messageId, string token)
        {
            CustomerProfile uProfile = CacheManager.GetUserProfile(token) as CustomerProfile;

            MessageBO uBO = new MessageBO(uProfile);

            try
            {
                if (!string.IsNullOrEmpty(messageId))
                {
                    string[] messageIdArr = messageId.Split(',');
                    bool isAllDelete = true;
                    for (int i = 0; i < messageIdArr.Length; i++)
                    {
                        try
                        {
                            uBO.DeleteMessage(Convert.ToInt32(messageIdArr[i]));
                        }
                        catch
                        {
                            isAllDelete = false;
                        }
                    }
                    if (isAllDelete)
                    {
                        return new ResultObject() { Flag = 1, Message = "删除成功!", Result = null };
                    }
                    else
                    {
                        return new ResultObject() { Flag = 1, Message = "部分删除成功!", Result = null };
                    }

                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
                }
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
            }

        }

        /// <summary>
        /// 设置邀请人
        /// </summary>
        /// <param name="InvitationTel">邀请人号码</param>
        /// <param name="CustomerId">会员ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("SetInvitationTel"), HttpGet]
        public ResultObject SetInvitationTel(string InvitationTel, int CustomerId, string token)
        {
            CustomerProfile uProfile = CacheManager.GetUserProfile(token) as CustomerProfile;

            try
            {
                if (!string.IsNullOrEmpty(InvitationTel))
                {
                    CustomerBO _bo = new CustomerBO(new CustomerProfile());
                    CustomerViewVO uVO = _bo.FindById(CustomerId);
                    if (uVO.InvitationCustomerID == 0)
                    {
                        List<CustomerViewVO> lVO = _bo.FindByPhone(InvitationTel);
                        if (lVO.Count != 0)
                        {
                            if (_bo.ChangeCustomerInvitationCustomerID(CustomerId, lVO[0].CustomerId))
                            {
                                //发放乐币奖励
                                if (CacheSystemConfig.GetSystemConfig().zxbShare > 0) {
                                    List<ZxbConfigVO> zVO = _bo.ZXBAddrequirebyCode("设置邀请");
                                    List<ZxbConfigVO> zVO2 = _bo.ZXBAddrequirebyCode("被设为邀请");
                                    _bo.ZXBAddrequire(CustomerId, zVO[0].Cost, zVO[0].Purpose, zVO[0].ZxbConfigID);
                                    _bo.ZXBAddrequire(lVO[0].CustomerId, zVO2[0].Cost, zVO2[0].Purpose, zVO2[0].ZxbConfigID);
                                }  
                                return new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
                            }
                            else {
                                return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                            };
                        }
                        else {
                            return new ResultObject() { Flag = 0, Message = "该号码不是众销乐会员!", Result = null };
                        }
                    }
                    else {
                        return new ResultObject() { Flag = 0, Message = "你之前已经填写了邀请人!", Result = null };
                    }
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
                }
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }

        }

        /// <summary>
        /// 领取奖励
        /// </summary>
        /// <param name="ZXBrequireId">消息ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("ReceiveZXBrequire"), HttpGet]
        public ResultObject ReceiveZXBrequire(string ZXBrequireId, string token)
        {
            CustomerProfile uProfile = CacheManager.GetUserProfile(token) as CustomerProfile;

            MessageBO uBO = new MessageBO(uProfile);

            try
            {
                if (!string.IsNullOrEmpty(ZXBrequireId))
                {
                    CustomerBO _bo = new CustomerBO(new CustomerProfile());
                    if (_bo.ZXBReceiveRequire(Convert.ToInt32(ZXBrequireId)))
                    {
                        return new ResultObject() { Flag = 1, Message = "领取成功!", Result = null };
                    }
                    else
                    {
                        return new ResultObject() { Flag = 0, Message = "领取失败!", Result = null };
                    }

                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
                }
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "领取失败!", Result = null };
            }

        }

        /// <summary>
        /// 领取所有奖励
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("ReceiveZXBrequire"), HttpGet]
        public ResultObject ReceiveZXBrequire(string token)
        {
            CustomerProfile uProfile = CacheManager.GetUserProfile(token) as CustomerProfile;

            MessageBO uBO = new MessageBO(uProfile);
            try
            {
                CustomerBO _bo = new CustomerBO(new CustomerProfile());
                List<zxbRequireVO> zVO = _bo.ZXBFindRequireAllByPageIndex("Status = 0 and CustomerId="+uProfile.CustomerId);
                bool isAllDelete = true;
                for (int i = 0; i < zVO.Count; i++)
                {
                    if (!_bo.ZXBReceiveRequire(Convert.ToInt32(zVO[i].ZXBrequireId)))
                    {
                        isAllDelete = false;
                    }
                }
                if (isAllDelete)
                {
                    return new ResultObject() { Flag = 1, Message = "领取成功!", Result = null };
                }
                else
                {
                    return new ResultObject() { Flag = 1, Message = "部分领取成功!", Result = null };
                }
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "领取失败!", Result = null };
            }

        }

        /// <summary>
        /// 撤销奖励
        /// </summary>
        /// <param name="ZXBrequireId">消息ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DelZXBrequire"), HttpGet]
        public ResultObject DelZXBrequire(string ZXBrequireId, string token)
        {
            CustomerProfile uProfile = CacheManager.GetUserProfile(token) as CustomerProfile;

            MessageBO uBO = new MessageBO(uProfile);

            try
            {
                if (!string.IsNullOrEmpty(ZXBrequireId))
                {
                    CustomerBO _bo = new CustomerBO(new CustomerProfile());
                    if (_bo.ZXBdelRequire(Convert.ToInt32(ZXBrequireId)))
                    {
                        return new ResultObject() { Flag = 1, Message = "撤销成功!", Result = null };
                    }
                    else
                    {
                        return new ResultObject() { Flag = 1, Message = "撤销失败!", Result = null };
                    }

                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
                }
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "撤销失败!", Result = null };
            }

        }

        /// <summary>
        /// 获取未读消息数量
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetUnRedMessageCount"), HttpGet]
        public ResultObject GetUnRedMessageCount(string token)
        {
            CustomerProfile uProfile = CacheManager.GetUserProfile(token) as CustomerProfile;

            MessageBO uBO = new MessageBO(uProfile);
            if (uProfile == null || uProfile.CustomerId < 0)
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = 0 };

            int count = uBO.FindMessageTotalCount("SendTo = " + uProfile.CustomerId + " AND Status = 0 ");

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = count };
        }

        /// <summary>
        /// 更新消息状态，未读变为已读
        /// </summary>
        /// <param name="messageId">消息ID</param>
        /// <param name="status">状态</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateMessageStatus"), HttpGet]
        public ResultObject UpdateMessageStatus(int messageId, int status, string token)
        {
            CustomerProfile uProfile = CacheManager.GetUserProfile(token) as CustomerProfile;
            MessageBO uBO = new MessageBO(uProfile);
            MessageVO vo = new MessageVO();
            vo.MessageId = messageId;
            vo.Status = status;

            if (uBO.UpdateMessage(vo))
                return new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
            else
                return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
        }

        /// <summary>
        /// 更新消息状态，未读变为已读(根据消息分类)
        /// </summary>
        /// <param name="MessageTypeId">消息分类ID</param>
        /// <param name="status">状态</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateMessageStatusByMessageTypeId"), HttpGet]
        public ResultObject UpdateMessageStatusByMessageTypeId(int MessageTypeId, string token)
        {
            CustomerProfile uProfile = CacheManager.GetUserProfile(token) as CustomerProfile;
            MessageBO uBO = new MessageBO(uProfile);

            if (uBO.UpdateMessageStatusByMessageTypeId(MessageTypeId, uProfile.CustomerId))
                return new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
            else
                return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
        }

        /// <summary>
        /// 批量未读变为已读
        /// </summary>
        /// <param name="messageId">消息ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("readAllMessage"), HttpGet]
        public ResultObject readAllMessage(string messageId, string token)
        {
            CustomerProfile uProfile = CacheManager.GetUserProfile(token) as CustomerProfile;

            MessageBO uBO = new MessageBO(uProfile);
            
            try
            {
                if (!string.IsNullOrEmpty(messageId))
                {
                    string[] messageIdArr = messageId.Split(',');
                    bool isAllDelete = true;
                    for (int i = 0; i < messageIdArr.Length; i++)
                    {
                        try
                        {
                            MessageVO vo = new MessageVO();
                            vo.MessageId = Convert.ToInt32(messageIdArr[i]);
                            vo.Status = 1;
                            uBO.UpdateMessage(vo);
                        }
                        catch
                        {
                            isAllDelete = false;
                        }
                    }
                    if (isAllDelete)
                    {
                        return new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
                    }
                    else
                    {
                        return new ResultObject() { Flag = 1, Message = "部分更新成功!", Result = null };
                    }

                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
                }
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }

        }

        /// <summary>
        /// 获取我的关注销售列表
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetMarkAgencyList"), HttpPost]
        public ResultObject GetMarkAgencyList([FromBody] ConditionModel condition, string token)
        {
            // CustomerProfile uProfile = CacheManager.GetUserProfile(token) as CustomerProfile;
            CustomerProfile cProfile = (CustomerProfile)CacheManager.GetUserProfile(token);
            if (condition == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            else if (condition.Filter == null || condition.PageInfo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            string conditionStr = " MarkCustomerId = " + cProfile.CustomerId + " and " + condition.Filter.Result();
            Paging pageInfo = condition.PageInfo;
            CustomerBO uBO = new CustomerBO(cProfile);
            List<MarkAgencyViewVO> list = uBO.FindMarkAgencyAllByPageIndex(conditionStr, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list };

        }

        /// <summary>
        /// 获取我的关注雇主列表
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetMarkBusinessList"), HttpPost]
        public ResultObject GetMarkBusinessList([FromBody] ConditionModel condition, string token)
        {
            // CustomerProfile uProfile = CacheManager.GetUserProfile(token) as CustomerProfile;
            CustomerProfile cProfile = (CustomerProfile)CacheManager.GetUserProfile(token);
            if (condition == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            else if (condition.Filter == null || condition.PageInfo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            string conditionStr = " MarkCustomerId = " + cProfile.CustomerId + " and " + condition.Filter.Result();
            Paging pageInfo = condition.PageInfo;
            CustomerBO uBO = new CustomerBO(cProfile);
            List<MarkBusinessViewVO> list = uBO.FindMarkBusinessAllByPageIndex(conditionStr, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list };

        }


        /// <summary>
        /// 获取我的关注的销售数量
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetMarkAgencyListCount"), HttpPost]
        public ResultObject GetMarkAgencyListCount([FromBody] ConditionModel condition, string token)
        {
            CustomerProfile cProfile = (CustomerProfile)CacheManager.GetUserProfile(token);
            if (condition == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            else if (condition.Filter == null || condition.PageInfo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            string conditionStr = " MarkCustomerId = " + cProfile.CustomerId + " and " + condition.Filter.Result();
            Paging pageInfo = condition.PageInfo;
            CustomerBO uBO = new CustomerBO(cProfile);
            int count = uBO.FindMarkAgencyTotalCount(conditionStr);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = count };
        }

        /// <summary>
        /// 获取我的关注的案例列表
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetMarkProjectList"), HttpPost]
        public ResultObject GetMarkProjectList([FromBody] ConditionModel condition, string token)
        {
            // CustomerProfile uProfile = CacheManager.GetUserProfile(token) as CustomerProfile;
            CustomerProfile cProfile = (CustomerProfile)CacheManager.GetUserProfile(token);
            if (condition == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            else if (condition.Filter == null || condition.PageInfo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            string conditionStr = " MarkCustomerId = " + cProfile.CustomerId + " and " + condition.Filter.Result();
            Paging pageInfo = condition.PageInfo;
            CustomerBO uBO = new CustomerBO(cProfile);
            List<MarkProjectViewVO> list = uBO.FindMarkProjectAllByPageIndex(conditionStr, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list };

        }


        /// <summary>
        /// 获取我的关注的案例数量
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetMarkProjectListCount"), HttpPost]
        public ResultObject GetMarkProjectListCount([FromBody] ConditionModel condition, string token)
        {
            CustomerProfile cProfile = (CustomerProfile)CacheManager.GetUserProfile(token);
            if (condition == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            else if (condition.Filter == null || condition.PageInfo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            string conditionStr = " MarkCustomerId = " + cProfile.CustomerId + " and " + condition.Filter.Result();
            Paging pageInfo = condition.PageInfo;
            CustomerBO uBO = new CustomerBO(cProfile);
            int count = uBO.FindMarkProjectTotalCount(conditionStr);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = count };
        }

        /// <summary>
        /// 获取我的关注的任务列表
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetMarkRequireList"), HttpPost]
        public ResultObject GetMarkRequireList([FromBody] ConditionModel condition, string token)
        {
            // CustomerProfile uProfile = CacheManager.GetUserProfile(token) as CustomerProfile;
            CustomerProfile cProfile = (CustomerProfile)CacheManager.GetUserProfile(token);
            if (condition == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            else if (condition.Filter == null || condition.PageInfo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            string conditionStr = " MarkCustomerId = " + cProfile.CustomerId + " and " + condition.Filter.Result();
            Paging pageInfo = condition.PageInfo;
            CustomerBO uBO = new CustomerBO(cProfile);
            List<MarkRequireViewVO> list = uBO.FindMarkRequireAllByPageIndex(conditionStr, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list };

        }

        /// <summary>
        /// 获取我的关注的任务数量
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetMarkRequireListCount"), HttpPost]
        public ResultObject GetMarkRequireListCount([FromBody] ConditionModel condition, string token)
        {
            CustomerProfile cProfile = (CustomerProfile)CacheManager.GetUserProfile(token);
            if (condition == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            else if (condition.Filter == null || condition.PageInfo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            string conditionStr = " MarkCustomerId = " + cProfile.CustomerId + " and " + condition.Filter.Result();
            Paging pageInfo = condition.PageInfo;
            CustomerBO uBO = new CustomerBO(cProfile);
            int count = uBO.FindMarkRequireTotalCount(conditionStr);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = count };
        }





        /// <summary>
        /// 获取我的服务列表
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetMyServicesList"), HttpPost]
        public ResultObject GetMyServicesList([FromBody] ConditionModel condition, string token)
        {
            // CustomerProfile uProfile = CacheManager.GetUserProfile(token) as CustomerProfile;
            CustomerProfile cProfile = (CustomerProfile)CacheManager.GetUserProfile(token);
            if (condition == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            else if (condition.Filter == null || condition.PageInfo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            string conditionStr = " CustomerId = " + cProfile.CustomerId + " and " + condition.Filter.Result();
            Paging pageInfo = condition.PageInfo;
            ServicesBO uBO = new ServicesBO(cProfile);
            List<ServicesViewVO> list = uBO.FindServicesAllByPageIndex(conditionStr, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list };

        }
        /// <summary>
        /// 获取我的服务数量
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetMyServicesListCount"), HttpPost]
        public ResultObject GetMyServicesListCount([FromBody] ConditionModel condition, string token)
        {
            CustomerProfile cProfile = (CustomerProfile)CacheManager.GetUserProfile(token);
            if (condition == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            else if (condition.Filter == null || condition.PageInfo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            string conditionStr = " CustomerId = " + cProfile.CustomerId + " and " + condition.Filter.Result();
            Paging pageInfo = condition.PageInfo;
            ServicesBO uBO = new ServicesBO(cProfile);
            int count = uBO.FindServicesTotalCount(conditionStr);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = count };
        }

        /// <summary>
        /// 获取我的奖励列表
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetMyzxbRequireList"), HttpPost]
        public ResultObject GetMyzxbRequireList([FromBody] ConditionModel condition, string token)
        {
            // CustomerProfile uProfile = CacheManager.GetUserProfile(token) as CustomerProfile;
            CustomerProfile cProfile = (CustomerProfile)CacheManager.GetUserProfile(token);
            if (condition == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            else if (condition.Filter == null || condition.PageInfo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            string conditionStr = " CustomerId = " + cProfile.CustomerId + " and " + condition.Filter.Result();
            Paging pageInfo = condition.PageInfo;
            CustomerBO _bo = new CustomerBO(new CustomerProfile());
            List<zxbRequireVO> list = _bo.ZXBFindRequireAllByPageIndex(conditionStr, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list };

        }

        /// <summary>
        /// 获取我的任务列表
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetMyRequireList"), HttpPost]
        public ResultObject GetMyRequireList([FromBody] ConditionModel condition, string token)
        {
            // CustomerProfile uProfile = CacheManager.GetUserProfile(token) as CustomerProfile;
            CustomerProfile cProfile = (CustomerProfile)CacheManager.GetUserProfile(token);
            if (condition == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            else if (condition.Filter == null || condition.PageInfo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            string conditionStr = " CustomerId = " + cProfile.CustomerId + " and " + condition.Filter.Result();
            Paging pageInfo = condition.PageInfo;
            RequireBO uBO = new RequireBO(cProfile);
            List<RequirementViewVO> list = uBO.FindRequireAllByPageIndex(conditionStr, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);
            int count = uBO.FindRequireTotalCount(conditionStr);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list,Count = count };

        }

        /// <summary>
        /// 获取我的商机需求
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetMyDemandList"), HttpPost]
        public ResultObject GetMyDemandList([FromBody] ConditionModel condition, string token)
        {
            // CustomerProfile uProfile = CacheManager.GetUserProfile(token) as CustomerProfile;
            CustomerProfile cProfile = (CustomerProfile)CacheManager.GetUserProfile(token);
            if (condition == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            else if (condition.Filter == null || condition.PageInfo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            string conditionStr = " CustomerId = " + cProfile.CustomerId + " and " + condition.Filter.Result();
            Paging pageInfo = condition.PageInfo;
            DemandBO uBO = new DemandBO(cProfile);
            List<DemandViewVO> list = uBO.FindDemandAllByPageIndex(conditionStr, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list };
        }
        /// <summary>
        /// 获取我的任务数量
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetMyRequireListCount"), HttpPost]
        public ResultObject GetMyRequireListCount([FromBody] ConditionModel condition, string token)
        {
            CustomerProfile cProfile = (CustomerProfile)CacheManager.GetUserProfile(token);
            if (condition == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            else if (condition.Filter == null || condition.PageInfo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            string conditionStr = " CustomerId = " + cProfile.CustomerId + " and " + condition.Filter.Result();
            Paging pageInfo = condition.PageInfo;
            RequireBO uBO = new RequireBO(cProfile);
            int count = uBO.FindRequireTotalCount(conditionStr);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = count };
        }



        /// <summary>
        /// 获取我的邀请列表
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetMyTenderList"), HttpPost]
        public ResultObject GetMyTenderList([FromBody] ConditionModel condition, string token)
        {
            // CustomerProfile uProfile = CacheManager.GetUserProfile(token) as CustomerProfile;
            CustomerProfile cProfile = (CustomerProfile)CacheManager.GetUserProfile(token);
            if (condition == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            else if (condition.Filter == null || condition.PageInfo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            string conditionStr = " TenderCustomerId = " + cProfile.CustomerId + " and " + condition.Filter.Result();
            Paging pageInfo = condition.PageInfo;
            RequireBO uBO = new RequireBO(cProfile);
            List<TenderInfoRequirementViewVO> list = uBO.FindTenderInfoRequirementAllByPageIndex(conditionStr, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list };

        }
        /// <summary>
        /// 获取我的邀请数量
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetMyTenderListCount"), HttpPost]
        public ResultObject GetMyTenderListCount([FromBody] ConditionModel condition, string token)
        {
            CustomerProfile cProfile = (CustomerProfile)CacheManager.GetUserProfile(token);
            if (condition == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            else if (condition.Filter == null || condition.PageInfo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            string conditionStr = " TenderCustomerId = " + cProfile.CustomerId + " and " + condition.Filter.Result();
            Paging pageInfo = condition.PageInfo;
            RequireBO uBO = new RequireBO(cProfile);
            int count = uBO.FindTenderInfoRequirementTotalCount(conditionStr);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = count };
        }

        /// <summary>
        /// 获取我的投标列表
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetMyTenderInviteList"), HttpPost]
        public ResultObject GetMyTenderInviteList([FromBody] ConditionModel condition, string token)
        {
            // CustomerProfile uProfile = CacheManager.GetUserProfile(token) as CustomerProfile;
            CustomerProfile cProfile = (CustomerProfile)CacheManager.GetUserProfile(token);
            if (condition == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            else if (condition.Filter == null || condition.PageInfo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            string conditionStr = " CustomerId = " + cProfile.CustomerId + " and " + condition.Filter.Result();
            Paging pageInfo = condition.PageInfo;
            RequireBO uBO = new RequireBO(cProfile);
            List<TenderInviteRequirementViewVO> list = uBO.FindTenderInviteRequirementAllByPageIndex(conditionStr, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list };

        }
        /// <summary>
        /// 获取我的投标数量
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetMyTenderListCount"), HttpPost]
        public ResultObject GetMyTenderInviteListCount([FromBody] ConditionModel condition, string token)
        {
            CustomerProfile cProfile = (CustomerProfile)CacheManager.GetUserProfile(token);
            if (condition == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            else if (condition.Filter == null || condition.PageInfo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            string conditionStr = " CustomerId = " + cProfile.CustomerId + " and " + condition.Filter.Result();
            Paging pageInfo = condition.PageInfo;
            RequireBO uBO = new RequireBO(cProfile);
            int count = uBO.FindTenderInviteRequirementTotalCount(conditionStr);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = count };
        }



        /// <summary>
        /// 获取我的项目经验列表
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetMyAgencyExperienceList"), HttpPost]
        public ResultObject GetMyAgencyExperienceList([FromBody] ConditionModel condition, string token)
        {
            // CustomerProfile uProfile = CacheManager.GetUserProfile(token) as CustomerProfile;
            CustomerProfile cProfile = (CustomerProfile)CacheManager.GetUserProfile(token);
            if (condition == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            else if (condition.Filter == null || condition.PageInfo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            string conditionStr = " CustomerId = " + cProfile.CustomerId + " and " + condition.Filter.Result();
            Paging pageInfo = condition.PageInfo;
            AgencyBO uBO = new AgencyBO(cProfile);
            List<AgencyExperienceViewVO> list = uBO.FindAllExperienceByPageIndex(conditionStr, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list };

        }


        /// <summary>
        /// 获取我的项目经验数量
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetMyAgencyExperienceListCount"), HttpPost]
        public ResultObject GetMyAgencyExperienceListCount([FromBody] ConditionModel condition, string token)
        {
            CustomerProfile cProfile = (CustomerProfile)CacheManager.GetUserProfile(token);
            if (condition == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            else if (condition.Filter == null || condition.PageInfo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            string conditionStr = " CustomerId = " + cProfile.CustomerId + " and " + condition.Filter.Result();
            Paging pageInfo = condition.PageInfo;
            AgencyBO uBO = new AgencyBO(cProfile);
            int count = uBO.FindExperienceTotalCount(conditionStr);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = count };
        }



        /// <summary>
        /// 获取我的项目经验列表
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetAgencyExperienceList"), HttpPost]
        public ResultObject GetAgencyExperienceList([FromBody] ConditionModel condition, string token)
        {
            // CustomerProfile uProfile = CacheManager.GetUserProfile(token) as CustomerProfile;
            CustomerProfile cProfile = (CustomerProfile)CacheManager.GetUserProfile(token);
            if (condition == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            else if (condition.Filter == null || condition.PageInfo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            string conditionStr = " CustomerId = " + cProfile.CustomerId + " and " + condition.Filter.Result();
            Paging pageInfo = condition.PageInfo;
            AgencyBO uBO = new AgencyBO(cProfile);
            List<AgencyExperienceViewVO> list = uBO.FindAllExperienceByPageIndex(conditionStr, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list };

        }


        /// <summary>
        /// 获取我的项目经验数量
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetMyAgencyExperienceListCount"), HttpPost]
        public ResultObject GetAgencyExperienceListCount([FromBody] ConditionModel condition, string token)
        {
            CustomerProfile cProfile = (CustomerProfile)CacheManager.GetUserProfile(token);
            if (condition == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            else if (condition.Filter == null || condition.PageInfo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            string conditionStr = " CustomerId = " + cProfile.CustomerId + " and " + condition.Filter.Result();
            Paging pageInfo = condition.PageInfo;
            AgencyBO uBO = new AgencyBO(cProfile);
            int count = uBO.FindExperienceTotalCount(conditionStr);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = count };
        }


        /// <summary>
        /// 获取我的消息列表
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetMyMessageList"), HttpPost]
        public ResultObject GetMyMessageList([FromBody] ConditionModel condition, string token)
        {
            // CustomerProfile uProfile = CacheManager.GetUserProfile(token) as CustomerProfile;
            CustomerProfile cProfile = (CustomerProfile)CacheManager.GetUserProfile(token);
            if (condition == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            else if (condition.Filter == null || condition.PageInfo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            string conditionStr = " SendTo = " + cProfile.CustomerId + " and " + condition.Filter.Result();
            Paging pageInfo = condition.PageInfo;
            MessageBO uBO = new MessageBO(cProfile);
            List<MessageViewVO> list = uBO.FindAllMessageByPageIndex(conditionStr, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list };

        }
        /// <summary>
        /// 获取我的消息数量
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetMyMessageListCount"), HttpPost]
        public ResultObject GetMyMessageListCount([FromBody] ConditionModel condition, string token)
        {
            CustomerProfile cProfile = (CustomerProfile)CacheManager.GetUserProfile(token);
            if (condition == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            else if (condition.Filter == null || condition.PageInfo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            string conditionStr = " CustomerId = " + cProfile.CustomerId + " and " + condition.Filter.Result();
            Paging pageInfo = condition.PageInfo;
            MessageBO uBO = new MessageBO(cProfile);
            int count = uBO.FindMessageTotalCount(conditionStr);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = count };
        }


        /// <summary>
        /// 添加或更新销售优势客户
        /// </summary>
        /// <param name="agencySuperClientVO">销售优势客户VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateAgencySuperClient"), HttpPost]
        public ResultObject UpdateAgencySuperClient([FromBody] AgencySuperClientVO agencySuperClientVO, string token)
        {
            if (agencySuperClientVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            CustomerProfile uProfile = CacheManager.GetUserProfile(token) as CustomerProfile;



            AgencyBO uBO = new AgencyBO(uProfile);

            if (agencySuperClientVO.AgencySuperClientId < 1)
            {
                int customerId = uProfile.CustomerId;
                AgencyViewVO tVO = uBO.FindAgencyByCustomerId(customerId);
                if (tVO != null)
                    agencySuperClientVO.AgencyId = tVO.AgencyId;
                int agencySuperClientId = uBO.AddAgencySuperClient(agencySuperClientVO);
                if (agencySuperClientId > 0)
                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = agencySuperClientId };
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
            else
            {
                bool isSuccess = uBO.UpdateAgencySuperClient(agencySuperClientVO);
                if (isSuccess)
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
                else
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }

        }

        /// <summary>
        /// 获取销售优势客户列表
        /// </summary>
        /// <param name="agencyId">销售ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetAgencySuperClientByAgency"), HttpGet]
        public ResultObject GetAgencySuperClientByAgency(int agencyId, string token)
        {
            AgencyBO uBO = new AgencyBO(new CustomerProfile());
            List<AgencySuperClientVO> uVO = uBO.FindAgencySuperClientByAgency(agencyId);
            if (uVO != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取销售优势客户
        /// </summary>
        /// <param name="agencySuperClientId">销售优势客户ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetAgencySuperClient"), HttpGet]
        public ResultObject GetAgencySuperClient(int agencySuperClientId, string token)
        {
            AgencyBO uBO = new AgencyBO(new CustomerProfile());
            AgencySuperClientVO uVO = uBO.FindAgencySuperClientById(agencySuperClientId);
            if (uVO != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 删除销售优势客户
        /// </summary>
        /// <param name="agencySuperClientId">销售优势客户ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DeleteAgencySuperClient"), HttpPost]
        public ResultObject DeleteAgencySuperClient(string agencySuperClientId, string token)
        {
            try
            {
                if (!string.IsNullOrEmpty(agencySuperClientId))
                {
                    string[] aeIdArr = agencySuperClientId.Split(',');
                    bool isAllDelete = true;
                    AgencyBO uBO = new AgencyBO(new CustomerProfile());
                    for (int i = 0; i < aeIdArr.Length; i++)
                    {
                        try
                        {
                            uBO.DeleteAgencySuperClient(Convert.ToInt32(aeIdArr[i]));
                        }
                        catch
                        {
                            isAllDelete = false;
                        }
                    }
                    if (isAllDelete)
                    {
                        return new ResultObject() { Flag = 1, Message = "删除成功!", Result = null };
                    }
                    else
                    {
                        return new ResultObject() { Flag = 1, Message = "部分删除成功!", Result = null };
                    }

                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
                }
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
            }
        }


        /// <summary>
        /// 添加或更新销售典型案例
        /// </summary>
        /// <param name="agencySolutionVO">销售典型案例VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateAgencySolution"), HttpPost]
        public ResultObject UpdateAgencySolution([FromBody] AgencySolutionVO agencySolutionVO, string token)
        {
            if (agencySolutionVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            CustomerProfile uProfile = CacheManager.GetUserProfile(token) as CustomerProfile;



            AgencyBO uBO = new AgencyBO(uProfile);

            if (agencySolutionVO.AgencySolutionId < 1)
            {
                int customerId = uProfile.CustomerId;
                AgencyViewVO tVO = uBO.FindAgencyByCustomerId(customerId);
                if (tVO != null)
                    agencySolutionVO.AgencyId = tVO.AgencyId;
                int agencySolutionId = uBO.AddAgencySolution(agencySolutionVO, agencySolutionVO.AgencySolutionFileList);
                if (agencySolutionId > 0)
                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = agencySolutionId };
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
            else
            {
                bool isSuccess = uBO.UpdateAgencySolution(agencySolutionVO, agencySolutionVO.AgencySolutionFileList);
                if (isSuccess)
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
                else
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }

        }

        /// <summary>
        /// 获取销售项目典型案例
        /// </summary>
        /// <param name="agencyId">销售ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetAgencySolutionByAgency"), HttpGet]
        public ResultObject GetAgencySolutionByAgency(int agencyId, string token)
        {
            AgencyBO uBO = new AgencyBO(new CustomerProfile());
            List<AgencySolutionVO> uVO = uBO.FindAgencySolutionByAgency(agencyId);
            if (uVO != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取销售项目典型案例 匿名
        /// </summary>
        /// <param name="agencyId">销售ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetAgencySolutionByAgencyAnonymous"), HttpGet,Anonymous]
        public ResultObject GetAgencySolutionByAgencyAnonymous(int agencyId)
        {
            AgencyBO uBO = new AgencyBO(new CustomerProfile());
            List<AgencySolutionVO> uVO = uBO.FindAgencySolutionByAgency(agencyId);
            if (uVO != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取销售典型案例
        /// </summary>
        /// <param name="agencySolutionId">销售典型案例ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetAgencySolution"), HttpGet]
        public ResultObject GetAgencySolution(int agencySolutionId, string token)
        {
            AgencyBO uBO = new AgencyBO(new CustomerProfile());
            AgencySolutionVO uVO = uBO.FindAgencySolutionById(agencySolutionId);
            if (uVO != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 删除销售项目经验
        /// </summary>
        /// <param name="agencySolutionId">销售项目经验ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DeleteAgencySolution"), HttpPost]
        public ResultObject DeleteAgencySolution(string agencySolutionId, string token)
        {
            try
            {
                if (!string.IsNullOrEmpty(agencySolutionId))
                {
                    string[] aeIdArr = agencySolutionId.Split(',');
                    bool isAllDelete = true;
                    AgencyBO uBO = new AgencyBO(new CustomerProfile());
                    for (int i = 0; i < aeIdArr.Length; i++)
                    {
                        try
                        {
                            uBO.DeleteAgencySolution(Convert.ToInt32(aeIdArr[i]));
                        }
                        catch
                        {
                            isAllDelete = false;
                        }
                    }
                    if (isAllDelete)
                    {
                        return new ResultObject() { Flag = 1, Message = "删除成功!", Result = null };
                    }
                    else
                    {
                        return new ResultObject() { Flag = 1, Message = "部分删除成功!", Result = null };
                    }

                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
                }
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
            }
        }


        /// <summary>
        /// 添加或更新雇主目标客户
        /// </summary>
        /// <param name="businessClientVO">雇主目标客户VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateBusinessClient"), HttpPost]
        public ResultObject UpdateBusinessClient([FromBody] BusinessClientVO businessClientVO, string token)
        {
            if (businessClientVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            CustomerProfile uProfile = CacheManager.GetUserProfile(token) as CustomerProfile;



            BusinessBO uBO = new BusinessBO(uProfile);

            if (businessClientVO.BusinessClientId < 1)
            {
                int customerId = uProfile.CustomerId;
                BusinessViewVO tVO = uBO.FindBusinessByCustomerId(customerId);
                if (tVO != null)
                    businessClientVO.BusinessId = tVO.BusinessId;
                int businessClientId = uBO.AddBusinessClient(businessClientVO);
                if (businessClientId > 0)
                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = businessClientId };
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
            else
            {
                bool isSuccess = uBO.UpdateBusinessClient(businessClientVO);
                if (isSuccess)
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
                else
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }

        }

        /// <summary>
        /// 获取雇主目标客户列表
        /// </summary>
        /// <param name="businessId">雇主目标客户ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetBusinessClientByBusiness"), HttpGet]
        public ResultObject GetBusinessClientByBusiness(int businessId, string token)
        {
            BusinessBO uBO = new BusinessBO(new CustomerProfile());
            List<BusinessClientVO> uVO = uBO.FindBusinessClientByBusiness(businessId);
            if (uVO != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取雇主目标客户列表
        /// </summary>
        /// <param name="businessId">雇主目标客户ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetBusinessClientByBusinessAnonymous"), HttpGet, Anonymous]
        public ResultObject GetBusinessClientByBusinessAnonymous(int businessId)
        {
            BusinessBO uBO = new BusinessBO(new CustomerProfile());
            List<BusinessClientVO> uVO = uBO.FindBusinessClientByBusiness(businessId);
            if (uVO != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取雇主目标客户
        /// </summary>
        /// <param name="businessClientId">雇主目标客户ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetBusinessClient"), HttpGet]
        public ResultObject GetBusinessClient(int businessClientId, string token)
        {
            BusinessBO uBO = new BusinessBO(new CustomerProfile());
            BusinessClientVO uVO = uBO.FindBusinessClientById(businessClientId);
            if (uVO != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 删除销售优势客户
        /// </summary>
        /// <param name="businessClientId">销售优势客户ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DeleteBusinessClient"), HttpPost]
        public ResultObject DeleteBusinessClient(string businessClientId, string token)
        {
            try
            {
                if (!string.IsNullOrEmpty(businessClientId))
                {
                    string[] aeIdArr = businessClientId.Split(',');
                    bool isAllDelete = true;
                    BusinessBO uBO = new BusinessBO(new CustomerProfile());
                    for (int i = 0; i < aeIdArr.Length; i++)
                    {
                        try
                        {
                            uBO.DeleteBusinessClient(Convert.ToInt32(aeIdArr[i]));
                        }
                        catch
                        {
                            isAllDelete = false;
                        }
                    }
                    if (isAllDelete)
                    {
                        return new ResultObject() { Flag = 1, Message = "删除成功!", Result = null };
                    }
                    else
                    {
                        return new ResultObject() { Flag = 1, Message = "部分删除成功!", Result = null };
                    }

                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
                }
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
            }
        }

        /// <summary>
        /// 查询销售匹配
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="requireId">需要匹配的任务</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetMatchAgencyList"), HttpPost]
        public ResultObject GetMatchAgencyList([FromBody] ConditionModel condition, int requireId, string token)
        {
            CustomerProfile cProfile = (CustomerProfile)CacheManager.GetUserProfile(token);
            if (condition == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            else if (condition.Filter == null || condition.PageInfo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            //
            RequireBO rBO = new RequireBO(cProfile);

            string conditionStr = condition.Filter.Result();
            Paging pageInfo = condition.PageInfo;
            AgencyBO uBO = new AgencyBO(new CustomerProfile());
            if (requireId > 0)
            {
                List<AgencyViewVO> list = uBO.FindMatchAgencyByPageIndex(requireId, conditionStr, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list };
            }
            else
            {
                //获取当前账号所有的任务，全部查询出来
                List<RequirementViewVO> requireList = rBO.FindRequireByCustomerId(cProfile.CustomerId, 1);
                List<AgencyViewVO> list = new List<AgencyViewVO>();
                foreach (RequirementViewVO vo in requireList)
                {
                    List<AgencyViewVO> listTmp = uBO.FindMatchAgencyByPageIndex(requireList[0].RequirementId, conditionStr, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);
                    if (listTmp != null)
                    {
                        foreach (AgencyViewVO nVO in listTmp)
                        {
                            bool iscontain = false;
                            foreach (AgencyViewVO oVO in list)
                            {
                                if (nVO.AgencyId == oVO.AgencyId)
                                {
                                    iscontain = true;
                                    break;                                    
                                }

                            }
                            if (!iscontain)
                                list.Add(nVO);
                        }
                    }
                }
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list };
            }
        }


        /// <summary>
        /// 查询会员提现记录
        /// </summary>
        /// <param name="condition">查询条件</param>

        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetPayoutHistoryList"), HttpPost]
        public ResultObject GetPayoutHistoryList([FromBody] ConditionModel condition, string token)
        {
            CustomerProfile cProfile = (CustomerProfile)CacheManager.GetUserProfile(token);
            if (condition == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            else if (condition.Filter == null || condition.PageInfo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }


            string conditionStr = " CustomerId = " + cProfile.CustomerId + " and " + condition.Filter.Result();
            Paging pageInfo = condition.PageInfo;
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            List<CustomerPayOutHistoryViewVO> list = uBO.FindCustomerPayoutHistoryViewAllByPageIndex(conditionStr, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list };
        }


        /// <summary>
        /// 查询会员收入记录
        /// </summary>
        /// <param name="condition">查询条件</param>

        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetEarnHistoryList"), HttpPost]
        public ResultObject GetEarnHistoryList([FromBody] ConditionModel condition, string token)
        {
            CustomerProfile cProfile = (CustomerProfile)CacheManager.GetUserProfile(token);
            if (condition == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            else if (condition.Filter == null || condition.PageInfo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }


            string conditionStr = " CustomerId = " + cProfile.CustomerId + " and " + condition.Filter.Result();
            Paging pageInfo = condition.PageInfo;
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            List<CommissionIncomeViewVO> list = uBO.FindCommissionIncomeAllByPageIndex(conditionStr, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list };
        }
        /// <summary>
        /// 查询会员托管记录
        /// </summary>
        /// <param name="condition">查询条件</param>

        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetCommissionHistoryList"), HttpPost]
        public ResultObject GetCommissionHistoryList([FromBody] ConditionModel condition, string token)
        {
            CustomerProfile cProfile = (CustomerProfile)CacheManager.GetUserProfile(token);
            if (condition == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            else if (condition.Filter == null || condition.PageInfo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            string conditionStr = " CustomerId = " + cProfile.CustomerId + " and " + condition.Filter.Result();
            Paging pageInfo = condition.PageInfo;
            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            List<CommissionDelegationViewVO> list = uBO.FindCommissionDelegationViewAllByPageIndex(conditionStr, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list };
        }
        /// <summary>
        /// 获取会员提现记录数量
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetMyPayoutHistoryListCount"), HttpPost]
        public ResultObject GetMyPayoutHistoryListCount([FromBody] ConditionModel condition, string token)
        {
            CustomerProfile cProfile = (CustomerProfile)CacheManager.GetUserProfile(token);
            if (condition == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            else if (condition.Filter == null || condition.PageInfo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            string conditionStr = " CustomerId = " + cProfile.CustomerId + " and " + condition.Filter.Result();
            Paging pageInfo = condition.PageInfo;
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            int count = uBO.FindCustomerPayoutHistoryViewTotalCount(conditionStr);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = count };
        }


        /// <summary>
        /// 查询会员充值记录
        /// </summary>
        /// <param name="condition">查询条件</param>       
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetPayInHistoryList"), HttpPost]
        public ResultObject GetPayInHistoryList([FromBody] ConditionModel condition, string token)
        {
            CustomerProfile cProfile = (CustomerProfile)CacheManager.GetUserProfile(token);
            if (condition == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            else if (condition.Filter == null || condition.PageInfo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }


            string conditionStr = " CustomerId = " + cProfile.CustomerId + "  and PayInStatus = 1 and " + condition.Filter.Result();
            Paging pageInfo = condition.PageInfo;
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            List<CustomerPayInHistoryViewVO> list = uBO.FindCustomerPayInHistoryViewAllByPageIndex(conditionStr, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list };
        }

        /// <summary>
        /// 获取会员充值记录数量
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetMyPayInHistoryListCount"), HttpPost]
        public ResultObject GetMyPayInHistoryListCount([FromBody] ConditionModel condition, string token)
        {
            CustomerProfile cProfile = (CustomerProfile)CacheManager.GetUserProfile(token);
            if (condition == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            else if (condition.Filter == null || condition.PageInfo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            string conditionStr = " CustomerId = " + cProfile.CustomerId + "  and PayInStatus = 1 and " + condition.Filter.Result();
            Paging pageInfo = condition.PageInfo;
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            int count = uBO.FindCustomerPayInHistoryViewTotalCount(conditionStr);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = count };
        }


        /// <summary>
        /// 处理会员提现操作
        /// </summary>
        /// <param name="payoutHistoryVO">提现记录</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("HandleCustomerPayOut"), HttpPost]
        public ResultObject HandleCustomerPayOut([FromBody] PayoutHistoryVO payoutHistoryVO, string token)
        {
            UserProfile cProfile = CacheManager.GetUserProfile(token);

            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            int result = uBO.HandleCustomerPayOut(payoutHistoryVO, cProfile.UserId);
            if (result > 0)
                return new ResultObject() { Flag = 1, Message = "处理成功!", Result = result };
            else
                return new ResultObject() { Flag = 0, Message = "处理失败!", Result = result };
        }
        /// <summary>
        /// 获取会员提现View
        /// </summary>
        /// <param name="payoutHistoryId">提现记录</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetCustomerPayOutHistory"), HttpGet]
        public ResultObject GetCustomerPayOutHistory(int payoutHistoryId, string token)
        {
            UserProfile cProfile = CacheManager.GetUserProfile(token);

            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            CustomerPayOutHistoryViewVO vo = uBO.GetCustomerPayOutHistoryView(payoutHistoryId);
            if (vo !=null)
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = vo };
            else
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = vo };
        }

        /// <summary>
        /// 查看销售联系方式
        /// </summary>
        /// <param name="agencyCustomerId">销售会员ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetAgencyPhone"), HttpGet]
        public ResultObject GetAgencyPhone(int agencyCustomerId, string token)
        {
            CustomerProfile cProfile = CacheManager.GetUserProfile(token) as CustomerProfile;

            CustomerBO uBO = new CustomerBO(cProfile);
            string phone = uBO.ViewAgencyPhone(cProfile.CustomerId, agencyCustomerId);
            if (phone != "-1")
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = phone };
            else
                return new ResultObject() { Flag = 0, Message = "您的可查看数已经用完，无法查看！", Result = null };
        }

        /// <summary>
        /// 查看雇主联系方式
        /// </summary>
        /// <param name="businessCustomerId">雇主会员ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetBusinessPhone"), HttpGet]
        public ResultObject GetBusinessPhone(int businessCustomerId, string token)
        {
            CustomerProfile cProfile = CacheManager.GetUserProfile(token) as CustomerProfile;

            CustomerBO uBO = new CustomerBO(cProfile);
            string phone = uBO.ViewBusinessPhone(cProfile.CustomerId, businessCustomerId);
            if (phone != "-1")
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = phone };
            else
                return new ResultObject() { Flag = 0, Message = "您的可查看数已经用完，无法查看！", Result = null };
        }

        /// <summary>
        /// 获取雇主发布中的任务
        /// </summary>
        /// <param name="CustomerId">雇主会员ID</param>
        /// <returns></returns>
        [Route("GetBusinessRequire"), HttpGet, Anonymous]
        public ResultObject GetBusinessRequire(int CustomerId)
        {
            RequireBO rBO = new RequireBO(new CustomerProfile());
            List<RequirementViewVO> RequirementVOList = rBO.FindRequireByCustomerId(CustomerId, 1);

            if (RequirementVOList != null)
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = RequirementVOList };
            else
                return new ResultObject() { Flag = 0, Message = "获取失败", Result = null };
        }


        #region tool File

        /// <summary>
        /// 添加或更新客户文件
        /// </summary>
        /// <param name="ToolFileVO">文件VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateToolFile"), HttpPost]
        public ResultObject UpdateToolFile([FromBody] ToolFileVO ToolFileVO, string token)
        {
            if (ToolFileVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerBO uBO = new CustomerBO(new CustomerProfile());

            if (ToolFileVO.ToolFileId < 1)
            {
                ToolFileVO.CreatedDate = DateTime.Now;
                int fileId = uBO.AddToolFile(ToolFileVO);
                if (fileId > 0)
                {
                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = fileId };
                }
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
            else
            {
                bool isSuccess = uBO.UpdateToolFile(ToolFileVO);
                if (isSuccess)
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
                else
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }

        }



        /// <summary>
        /// 获取客户文件信息
        /// </summary>
        /// <param name="ToolFileId">客户文件ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetToolFile"), HttpGet]
        public ResultObject GetToolFile(int ToolFileId, string token)
        {
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            ToolFileVO uVO = uBO.FindToolFileById(ToolFileId);
            if (uVO != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取客户文件列表
        /// </summary>
        /// <param name="CustomerId">客户ID</param>
        /// <param name="typeId">1为合同，2为工具</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetToolFileByCustomer"), HttpGet]
        public ResultObject GetToolFileByCustomer(int CustomerId, int typeId, string token)
        {
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            List<ToolFileVO> uVO = uBO.FindToolFileByCustomerId(CustomerId, typeId);
            if (uVO != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 批量删除客户文件
        /// </summary>
        /// <param name="toolFileIds">客户文件ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DeleteToolFile"), HttpGet]
        public ResultObject DeleteToolFile(string toolFileIds, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerBO uBO = new CustomerBO((CustomerProfile)uProfile);
            try
            {
                if (!string.IsNullOrEmpty(toolFileIds))
                {
                    string[] bIdArr = toolFileIds.Split(',');
                    bool isAllUpdate = true;

                    for (int i = 0; i < bIdArr.Length; i++)
                    {
                        try
                        {
                            bool result = uBO.DeleteToolFile(Convert.ToInt32(bIdArr[i]));

                        }
                        catch
                        {
                            isAllUpdate = false;
                        }
                    }
                    if (isAllUpdate)
                    {
                        return new ResultObject() { Flag = 1, Message = "删除成功!", Result = null };
                    }
                    else
                    {
                        return new ResultObject() { Flag = 1, Message = "部分删除成功!", Result = null };
                    }

                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
                }
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
            }
        }
        #endregion
        /// <summary>
        /// prepay
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [Route("getAppPayInfo"), HttpGet]
        public ResultObject getPayInfo(string amount, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            string out_trade_no = "0";
            out_trade_no = GenerateOutTradeNo("Ali");
            //string token = HttpContext.Current.Session["#Session#TOKEN"].ToString();
            //CustomerProfile up = (CustomerProfile)CacheManager.GetUserProfile(token);
            CustomerBO _bo = new CustomerBO(new CustomerProfile());
            PayinHistoryVO vo = new PayinHistoryVO();
            vo.CustomerId = ((CustomerProfile)uProfile).CustomerId;
            vo.Cost = Convert.ToDecimal(amount.Trim());
            vo.PayInOrder = out_trade_no;
            vo.PayInStatus = 0;
            vo.PayInDate = DateTime.Now;
            vo.Purpose = "APP钱包充值";       


            DefaultAopClient client = new DefaultAopClient(config.gatewayUrl, config.app_id, config.private_key, "json", "1.0", config.sign_type, config.alipay1_public_key, config.charset, false);


            AlipayTradeAppPayModel model = new AlipayTradeAppPayModel();
            model.Body = "众销乐APP-资源共享众包销售平台客户钱包充值";
            model.Subject = "众销乐APP-资源共享众包销售平台客户钱包充值";
            model.TotalAmount = amount;
            model.OutTradeNo = out_trade_no;
            model.ProductCode = "FAST_INSTANT_TRADE_PAY";


            AlipayTradeAppPayRequest request = new AlipayTradeAppPayRequest();
            // 设置同步回调地址
            request.SetReturnUrl(config.ReturnUrl);
            // 设置异步通知接收地址
            request.SetNotifyUrl(config.NotifyUrl);
            // 将业务model载入到request
            request.SetBizModel(model);

            AlipayTradeAppPayResponse response = null;

            Dictionary<string, string> sArray = new Dictionary<string, string>();
            try
            {
                response = client.pageExecute(request, null, "post");

                //string s=@"< form id = 'alipaysubmit' name = 'alipaysubmit'
                //          action = 'https://openapi.alipay.com/gateway.do?charset=UTF-8' 
                //          method = 'post' >
                //          < input  name = 'app_id' value = '2017102609537389' />
                //          < input  name = 'biz_content' value = '{"body":"众销乐-资源共享众包销售平台客户钱包充值","out_trade_no":"Ali20171205154406241","product_code":"FAST_INSTANT_TRADE_PAY","qrcode_width":0,"subject":"众销乐-资源共享众包销售平台客户钱包充值","total_amount":"0.01"}' />
                //          < input  name = 'charset' value = 'UTF-8' />
                //          < input  name = 'format' value = 'json' />
                //          < input  name = 'method' value = 'alipay.trade.page.pay' />
                //          < input  name = 'notify_url' value = 'http://www.zhongxiaole.net/Pay/Notify_url.aspx' />
                //          < input  name = 'return_url' value = 'http://www.zhongxiaole.net/Pay/Return_url.aspx' />
                //          < input  name = 'sign_type' value = 'RSA2' />
                //          < input  name = 'timestamp' value = '2017-12-05 15:44:10' />
                //          < input  name = 'version' value = '1.0' />
                //          < input  name = 'sign' value = 'b6GaFYqTxEXHSDsKNtnacQ2vWYyA+iS2o8drdY8fl7VIjF0EDY5ij3JJEbb0qCTA1Wmo16E1FFAlFjsYueuf1XzER8umUQAqInE+rPRxzSHPi4FaAl2wqBBTJx8vCxUPHOfg01CDGgYhnM/XQ++4QYT+S/eyNbPjFRqmrBl07MlTa9gGiJ5wTVxS2jpXo+gxR4CTGXdtBmyxAw9kxFOjTapd9d488cU+DoffoyCDtRpgvoKhTEXoUXAyuyXL3bL9zkjOv/9i/BzmbEWuh28Nl0yI3UJBTZIvX7YWudJ2qi/ggooKxkQ6N5pGRrt+2AuzL8xiMQWTfUrTnuTaFJbT6w==' />
                //          < input type = 'submit' value = 'post' style = 'display:none;' >
                //          </ form >< script > document.forms['alipaysubmit'].submit();</ script >";
                string body = response.Body;
                HtmlAgilityPack.HtmlDocument htmlDoc2 = new HtmlAgilityPack.HtmlDocument();
                htmlDoc2.LoadHtml(body);
                HtmlAgilityPack.HtmlNodeCollection nodeList = htmlDoc2.DocumentNode.SelectNodes("//input");
                for (int i = 0; i < nodeList.Count; i++)
                {
                    HtmlAgilityPack.HtmlNode rnode = nodeList[i];
                    if (rnode.GetAttributeValue("name", "") != "")
                    {
                        sArray.Add(rnode.GetAttributeValue("name", ""), (HttpUtility.UrlEncode(rnode.GetAttributeValue("value", ""))).Replace(" +", " %20"));
                    }
                }

                _bo.InsertPayinHistory(vo);
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = sArray };

            }
            catch (Exception exp)
            {


            }
            return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
        }
        private static string GenerateOutTradeNo(string type)
        {
            var ran = new Random();

            return string.Format("{0}{1}{2}", type, DateTime.Now.ToString("yyyyMMddHHmmss"), ran.Next(999));
        }

        /// <summary>
        /// 签到
        /// </summary>
        /// <param name="CustomerId">会员ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("AddSignIn"), HttpPost]
        public ResultObject AddSignIn(int CustomerId,string token)
        {
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            if (uBO.FindSignInCount("CustomerId = " + CustomerId + " and TO_DAYS(NOW()) - TO_DAYS(SignDate) = 1 and (Status = 0 or Status = 1)") == 0) {
                uBO.DeleteSignInBycondition("CustomerId = " + CustomerId + " and date(SignDate) <> curdate()");
            }
            if (uBO.FindSignInCount("CustomerId = " + CustomerId + " and date(SignDate) = curdate()") == 0)
            {
                SignInVO signinVO = new SignInVO();
                signinVO.Status = 0;
                signinVO.SignDate = DateTime.Now;
                signinVO.CustomerId = CustomerId;

                int SignInID = uBO.AddSignIn(signinVO);
                if (SignInID > 0)
                {
                    signinVO.SignInID = SignInID;

                    decimal balance = 0;
                    string str = "每日签到";
                    List<ZxbConfigVO> zVO = uBO.ZXBAddrequirebyCode("每日签到");
                    List<ZxbConfigVO> zVO2 = uBO.ZXBAddrequirebyCode("连续签到7天");
                    List<ZxbConfigVO> zVO3 = uBO.ZXBAddrequirebyCode("连续签到30天");

                    if (uBO.FindSignInCount("CustomerId = " + CustomerId + " and (Status = 0 or Status = 1)") >= 30)
                    {
                        signinVO.Status = 2;
                        uBO.UpdateSignIn(signinVO);

                        uBO.DeleteSignInBycondition("CustomerId = " + CustomerId + " and (Status = 0 or Status = 1)");

                        balance = zVO3[0].Cost;
                        str = "连续签到30天";
                        uBO.ZXBPlusBalance(CustomerId, zVO3[0].Cost, "连续签到30天");
                    }
                    else if (uBO.FindSignInCount("CustomerId = " + CustomerId + " and Status = 0") >= 7)
                    {
                        balance = zVO2[0].Cost;
                        str = "连续签到7天";
                        uBO.UpdateSignInBycondition("Status = 1", "CustomerId = " + CustomerId + " and Status = 0");
                        uBO.ZXBPlusBalance(CustomerId, zVO2[0].Cost, "连续签到7天");
                    }
                    else {
                        balance = zVO[0].Cost;
                        str = "每日签到";
                        uBO.ZXBPlusBalance(CustomerId, zVO[0].Cost, "每日签到");
                    }
                    
                    return new ResultObject() { Flag = 1, Message = str+",乐币 + " + balance+"!", Result = null };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "签到失败!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "每天只能签到一次!", Result = null };
            }
        }

        /// <summary>
        /// 判断今天是否签到，Result是返回已经连续签到多少天
        /// </summary>
        /// <param name="CustomerId">会员ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getisSignIn"), HttpGet]
        public ResultObject getisSignIn(int CustomerId, string token)
        {
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            int count = 0;
            bool istoday = uBO.FindSignInCount("CustomerId = " + CustomerId + " and date(SignDate) = curdate()") == 0;
            if (uBO.FindSignInCount("CustomerId = " + CustomerId + " and TO_DAYS(NOW()) - TO_DAYS(SignDate) = 1 and (Status = 0 or Status = 1)") == 0)
            {
                if(istoday)
                count = 0;
                else
                count = 1;
            }
            else {
                count = uBO.FindSignInCount("CustomerId = " + CustomerId + " and (Status = 0 or Status = 1)");
            }

            if (istoday)
            {
                return new ResultObject() { Flag = 1, Message = "今天没有签到", Result = count };
            }   
            else
            {
                return new ResultObject() { Flag = 0, Message = "今天已经签到过了", Result = count };
            }
        }

        /// <summary>
        /// 小程序首页通知消息
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getNotificationMessage"), HttpGet]
        public ResultObject getNotificationMessage(string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            if (cProfile != null)
            {
                NotificationMessage nVO = new NotificationMessage();
                nVO.contract = new List<ContractMessage>();
                nVO.project = new List<ProjectMessage>();
                nVO.notice = new List<NoticeMessage>();

                int CustomerId = cProfile.CustomerId;
                ProjectBO uBO = new ProjectBO(new CustomerProfile());
                RequireBO rBO = new RequireBO(new CustomerProfile());
                DemandBO dBO = new DemandBO(new CustomerProfile());
                CustomerBO cBO = new CustomerBO(new CustomerProfile());
                string condition = "";
                //合同
                condition = " Status = 1 and (BusinessCustomerId = " + CustomerId + " or AgencyCustomerId=" + CustomerId + ") and (AgencyStatus=0 or BusinessStatus=0)";
                List<ContractViewVO> list = uBO.FindContractAllByPageIndex(condition, 1, 5, "CreatedAt", "desc");
                foreach (ContractViewVO vo in list)
                {
                    ContractMessage con= new ContractMessage();
                    con.contractid = vo.ContractId;
                    con.projectName = vo.ProjectName;
                    con.commission = vo.Commission;
                    con.agencyName = vo.AgencyName;
                    con.agencyStatus = vo.AgencyStatus;
                    con.companyName = vo.CompanyName;
                    con.businessStatus = vo.BusinessStatus;
                    if (vo.AgencyCustomerId == CustomerId)
                    {
                        con.identity = 2;
                    }
                    else {
                        con.identity = 1;
                    }

                    nVO.contract.Add(con);
                }
                //项目
                condition = "Status<>2 and Status<>5 and Status<>6 and(BusinessCustomerId=" + CustomerId + " or " + "AgencyCustomerId=" + CustomerId + ")";
                List<ProjectViewVO> list2 = uBO.FindProjectAllByPageIndex(condition, 1, 5, "CreatedAt", "desc");
                foreach (ProjectViewVO vo in list2)
                {
                    ProjectMessage pro = new ProjectMessage();
                    pro.projectid = vo.ProjectId;
                    pro.projectName = vo.ProjectName;
                    pro.commission = vo.Commission;
                    pro.status = vo.Status;
                    pro.agencyName = vo.AgencyName;
                    pro.businessname = vo.BusinessName;

                    List<ProjectActionViewVO> uVO = uBO.FindProjectActionByProject(vo.ProjectId);//工作进度
                    if (uVO.Count > 0)
                    {
                        pro.progress = uVO[uVO.Count - 1].Description;
                    }
                    else {
                        pro.progress = "";
                    }
                    pro.applymessage = "";
                    if (vo.AgencyCustomerId == CustomerId)
                    {
                        List<ProjectChangeVO> pVO2 = uBO.FindProjectChangeByProject(vo.ProjectId, "Status=0");
                        if (pVO2.Count > 0) {
                            pro.applymessage = "雇主申请将总酬金更改为：" + pVO2[0].Commission + "元";
                        }
                        List<ProjectRefundVO> pVO = uBO.FindProjectRefundByProject(vo.ProjectId, "Status=0");//申请关闭项目
                        if (pVO.Count > 0)
                        {
                            DateTime startTime = pVO[0].CreatedAt;
                            DateTime endTime = DateTime.Now;
                            TimeSpan ts = endTime - startTime;
                            pro.applymessage = "雇主申请关闭项目，如果你"+ ts.Days + "天之内没有同意或拒绝操作，雇主将有权单方面关闭项目。";
                        }
                        pro.identity = 2;
                    }
                    else {
                        ProjectCommissionViewVO pcVO = uBO.FindLatestProjectCommission(vo.ProjectId);//付款申请
                        if (pcVO != null) {
                            pro.applymessage = "销售申请阶段付款：" + pcVO.Commission + "元";
                        }
                        if (vo.Status == 3)
                        {
                            pro.applymessage = "销售申请项目完工";
                        }
                        pro.identity = 1;
                    } 
                    nVO.project.Add(pro);
                }
                //任务
                condition = "RequirementCustomerId=" + CustomerId;
                List<TenderInviteRequirementViewVO> rVO = rBO.FindTenderInviteRequirementAllByPageIndex(condition, 1, 1, "InviteDate", "desc");//最新投递
                if (rVO.Count > 0) {
                    NoticeMessage not = new NoticeMessage();
                    not.RID = rVO[0].RequirementId;
                    not.AID = rVO[0].TenderInviteId;
                    not.image = rBO.getRequireIMG(rVO[0].RequirementId);
                    not.title = rVO[0].Title;
                    not.typename = "任务";
                    not.type = "Requirement";
                    not.Date = rVO[0].InviteDate;
                    not.content = rVO[0].AgencyName + "投递了简历";
                    nVO.notice.Add(not);
                }
                //商机
                condition = "BCustomerId=" + CustomerId;
                List<DemandOfferViewVO> dVO = dBO.FindDemandOfferViewAllByPageIndex(condition, 1, 1, "CreatedAt", "desc");//商机需求留言列表
                if (dVO.Count > 0)
                {
                    NoticeMessage not = new NoticeMessage();
                    not.RID = dVO[0].DemandId;
                    not.AID = dVO[0].OfferId;
                    not.image = cBO.GetCustomerIMG(dVO[0].CustomerId);
                    not.title = dVO[0].Name;
                    not.typename = "商机需求";
                    not.type = "Demand";
                    not.Date = dVO[0].CreatedAt;
                    not.content = dVO[0].OfferDescription;
                    nVO.notice.Add(not);
                }
                //面试邀请
                condition = "TenderCustomerId=" + CustomerId;
                List<TenderInfoRequirementViewVO> tVO = rBO.FindTenderInfoRequirementAllByPageIndex(condition, 1, 1, "TenderDate", "desc");//面试邀请
                if (tVO.Count > 0)
                {
                    NoticeMessage not = new NoticeMessage();
                    not.RID = tVO[0].RequirementId;
                    not.AID = tVO[0].TenderInfoId;
                    not.image = rBO.getRequireIMG(tVO[0].RequirementId);
                    not.title = tVO[0].Title;
                    not.typename = "面试邀请";
                    not.type = "Tender";
                    not.Date = tVO[0].TenderDate;
                    not.content = tVO[0].CompanyName+"邀请您投递简历";
                    nVO.notice.Add(not);
                }
                //钱包
                BalanceVO bVO = cBO.FindBalanceByCustomerId(CustomerId);
                decimal balance = 0;//余额
                if (bVO != null)
                {
                    balance = bVO.Balance;
                }
                NoticeMessage Walletnot = new NoticeMessage();
                Walletnot.typename = "钱包";
                Walletnot.type = "Wallet";
                Walletnot.balance = balance;
                Walletnot.RID = 0;
                Walletnot.AID = 0;
                Walletnot.image = "";
                //充值记录
                condition = " CustomerId = " + CustomerId + " and PayInStatus = 1";
                List<CustomerPayInHistoryViewVO> PayInVO = cBO.FindCustomerPayInHistoryViewAllByPageIndex(condition, 1, 1, "PayInDate", "desc");
                if (PayInVO.Count > 0) {
                    Walletnot.AID = PayInVO[0].PayInHistoryId;
                    Walletnot.Date = PayInVO[0].PayInDate;
                    Walletnot.title = "充值";
                    Walletnot.content = PayInVO[0].Purpose + "：" + PayInVO[0].Cost + "元";
                }
                //提现记录
                condition = " CustomerId = " + CustomerId;
                List<PayoutHistoryVO> PayoutVO = cBO.FindPayoutHistoryAllByPageIndex(condition, 1, 1, "PayOutDate", "desc");
                if (PayoutVO.Count > 0)
                {
                    if (PayoutVO[0].PayOutDate > Walletnot.Date) {
                        Walletnot.AID = PayoutVO[0].PayOutHistoryId;
                        Walletnot.Date = PayoutVO[0].PayOutDate;
                        Walletnot.title = "提现";
                        if (PayoutVO[0].PayOutStatus == 0)
                            Walletnot.content = "申请提现";
                        else if (PayoutVO[0].PayOutStatus == -1)
                            Walletnot.content = "未提交申请";
                        else if (PayoutVO[0].PayOutStatus == -2)
                            Walletnot.content = "提现失败";
                        else if (PayoutVO[0].PayOutStatus == 1)
                            Walletnot.content = "提现成功";
                    }
                }
                //收入记录
                condition = " CustomerId = " + CustomerId;
                List<CommissionIncomeViewVO> IncomeVO = cBO.FindCommissionIncomeAllByPageIndex(condition, 1, 1, "PayDate", "desc");
                if (IncomeVO.Count > 0)
                {
                    if (IncomeVO[0].PayDate > Walletnot.Date)
                    {
                        Walletnot.AID = IncomeVO[0].commissionInComeId;
                        Walletnot.Date = IncomeVO[0].PayDate;
                        Walletnot.title = "收入";
                        Walletnot.content = IncomeVO[0].Purpose + "：" + IncomeVO[0].Commission + "元"; ;
                    }
                }
                //项目托管记录
                condition = " CustomerId = " + CustomerId;
                List<CommissionDelegationViewVO> DelegationVO = uBO.FindCommissionDelegationViewAllByPageIndex(condition, 1, 1, "PayDate", "desc");
                if (DelegationVO.Count > 0)
                {
                    if (DelegationVO[0].PayDate > Walletnot.Date)
                    {
                        Walletnot.AID = DelegationVO[0].CommissionDelegationId;
                        Walletnot.Date = DelegationVO[0].PayDate;
                        Walletnot.title = "项目托管";
                        Walletnot.content = DelegationVO[0].Purpose + "：" + DelegationVO[0].Commission + "元"; ;
                    }
                }
                //任务托管记录
                condition = " CustomerId = " + CustomerId;
                List<RequireCommissionDelegationviewVO> RequireCommissionVO = rBO.FindRequireDelegateCommisiondelegationAllByPageIndex(condition, 1, 1, "DelegationDate", "desc");
                if (RequireCommissionVO.Count > 0)
                {
                    if (RequireCommissionVO[0].DelegationDate > Walletnot.Date)
                    {
                        Walletnot.AID = RequireCommissionVO[0].RequireCommissionDelegationId;
                        Walletnot.Date = RequireCommissionVO[0].DelegationDate;
                        Walletnot.title = "任务托管";
                        Walletnot.content ="任务酬金委托：" + RequireCommissionVO[0].Commission + "元";
                    }
                }

                if (Walletnot.AID > 0) {
                    nVO.notice.Add(Walletnot);
                }
                return new ResultObject() { Flag = 1, Message = "获取成功", Result = nVO };
            }
            return new ResultObject() { Flag = 0, Message = "获取失败", Result = null };
        }
    }
}
