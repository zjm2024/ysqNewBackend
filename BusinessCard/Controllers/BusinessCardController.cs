using AlibabaCloud.SDK.CCC20200701.Models;
using AlibabaCloud.SDK.Sample;
using Aop.Api.Domain;
using BroadSky.WeChatAppDecrypt;
using BusinessCard.GenerateIMG;
using BusinessCard.Models;
using CoreFramework.VO;
using MySqlX.XDevAPI.Relational;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPOI.OpenXmlFormats.Shared;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using SPlatformService;
using SPlatformService.Controllers;
using SPlatformService.Models;
using SPlatformService.TokenMange;
using SPLibrary.BusinessCardManagement.BO;
using SPLibrary.BusinessCardManagement.DAO;
using SPLibrary.BusinessCardManagement.VO;
using SPLibrary.BussinessManagement.BO;
using SPLibrary.CoreFramework;
using SPLibrary.CoreFramework.BO;
using SPLibrary.CoreFramework.Logging.BO;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.CustomerManagement.VO;
using SPLibrary.LuckyDrawManagement.BO;
using SPLibrary.RequireManagement.BO;
using SPLibrary.RequireManagement.VO;
using SPLibrary.UserManagement.DAO;
using SPLibrary.UserManagement.VO;
using SPLibrary.WebConfigInfo;
using SPLibrary.WxEcommerce;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using Tencent;
//using Ubiety.Dns.Core;

namespace BusinessCard.Controllers
{
    [RoutePrefix("SPWebAPI/BusinessCard")]
    [TokenProjector]
    public class BusinessCardController : ApiController
    {
        /// <summary>
        /// 小程序获取微信用户信息(企业名片)
        /// </summary>
        /// <param name="wxUserInfoVO">微信小程序用户信息</param>
        /// <param name="code">第一次拉手返回Code</param>
        /// <returns></returns>
        [Route("GetMiniprogramUserInfoCard"), HttpPost, Anonymous]
        public ResultObject GetMiniprogramUserInfoCard([FromBody] wxUserInfoVO wxUserInfoVO, string code, string originType = "", int originID = 0, int AppType = 0)
        {
            AppVO AppVO = AppBO.GetApp(AppType);
            try
            {
                string url = "";
                url = "https://api.weixin.qq.com/sns/jscode2session?appid=" + AppVO.AppId + "&secret=" + AppVO.Secret + "&js_code=" + code + "&grant_type=authorization_code";
                string jsonStr = HttpHelper.HtmlFromUrlGet(url);

                var serializer = new DataContractJsonSerializer(typeof(OpenIdAndSessionKey));
                var mStream = new MemoryStream(Encoding.Default.GetBytes(jsonStr));
                OpenIdAndSessionKey readConfig = (OpenIdAndSessionKey)serializer.ReadObject(mStream);

                WeChatAppDecrypt un = new WeChatAppDecrypt(AppVO.AppId, AppVO.Secret);
                if (un.VaildateUserInfo(wxUserInfoVO.rawData, wxUserInfoVO.signature, readConfig.session_key))
                {
                    WechatUserInfo wui = new WechatUserInfo();
                    wui = un.Decrypt(wxUserInfoVO.encryptedData, wxUserInfoVO.iv, readConfig.session_key);

                    BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
                    CustomerBO uBO = new CustomerBO(new CustomerProfile());
                    CustomerViewVO customerVO = uBO.FindCustomerByOpenId(wui.openId, wui.unionId, "2", AppVO.AppType);

                    if (customerVO != null)
                    {
                        CustomerLoginHistoryVO clHistoryVO = new CustomerLoginHistoryVO();
                        clHistoryVO.LoginAt = DateTime.Now;
                        clHistoryVO.Status = true;
                        clHistoryVO.LoginOS = HttpContext.Current.Request.Browser.Platform;
                        clHistoryVO.LoginIP = HttpContext.Current.Request.UserHostAddress;
                        clHistoryVO.LoginBrowser = AppVO.AppName;
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

                        string token = CacheManager.TokenInsert(customerVO.CustomerId);
                        CustomerLoginModel ulm = new CustomerLoginModel();
                        customerVO.Password = "";
                        ulm.Customer = customerVO;
                        ulm.Token = token;

                        //判断是否有企业名片的个人信息
                        if (cBO.FindPersonalByCustomerId(customerVO.CustomerId) == null)
                        {
                            CardBO CardBO = new CardBO(new CustomerProfile());
                            PersonalVO pVO = new PersonalVO();

                            List<CardDataVO> cVO = CardBO.FindCardByCustomerId(customerVO.CustomerId);
                            if (cVO.Count > 0)
                            {
                                pVO.CustomerId = customerVO.CustomerId;
                                pVO.Name = cVO[0].Name;
                                pVO.Headimg = cVO[0].Headimg;
                                pVO.Phone = cVO[0].Phone;
                                pVO.Position = cVO[0].Position;
                                pVO.WeChat = cVO[0].WeChat;
                                pVO.Email = cVO[0].Email;
                                pVO.Details = cVO[0].Details;
                                pVO.Address = cVO[0].Address;
                                pVO.Business = cVO[0].Business;
                                pVO.latitude = cVO[0].latitude;
                                pVO.longitude = cVO[0].longitude;
                                pVO.Tel = cVO[0].Tel;

                            }
                            else
                            {
                                pVO.CustomerId = customerVO.CustomerId;
                                pVO.Name = customerVO.CustomerName;
                                pVO.Headimg = customerVO.HeaderLogo;
                            }
                            pVO.AppType = AppVO.AppType;
                            pVO.CreatedAt = DateTime.Now;
                            cBO.AddPersonal(pVO);
                        }


                        //记录登录信息               
                        clHistoryVO.CustomerId = customerVO.CustomerId;
                        uBO.AddCustomerLoginHistory(clHistoryVO);

                        //记录名片信息
                        ulm.Personal = cBO.FindPersonalByCustomerId(customerVO.CustomerId);

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
                        cVO.BusinessCard = true;
                        cVO.originType = originType;
                        cVO.originID = originID;
                        cVO.AppType = AppVO.AppType;

                        //新用户赠送一个月会员
                        /*
                        cVO.isVip = true;
                        cVO.ExpirationAt = DateTime.Now.AddMonths(1);
                        */
                        if (originType == "S_Personal" && originID > 0)
                        {
                            PersonalVO pVO = cBO.FindPersonalById(originID);
                            if (pVO != null)
                            {
                                cVO.originCustomerId = pVO.CustomerId;
                            }
                        }
                        if (originType == "S_JoinBusiness" && originID > 0)
                        {
                            cVO.originCustomerId = cBO.FindJurisdiction(originID);
                        }
                        if (originType == "S_SubsidiaryJoin" && originID > 0)
                        {
                            cVO.originCustomerId = cBO.FindJurisdiction(originID);
                        }
                        if (originType == "S_GreetingCard" && originID > 0)
                        {
                            GreetingCardVO gvo = cBO.FindGreetingCardById(originID);
                            if (gvo != null)
                            {
                                PersonalVO pVO = cBO.FindPersonalById(gvo.PersonalID);
                                if (pVO != null)
                                {
                                    cVO.originCustomerId = pVO.CustomerId;
                                }
                            }
                        }

                        if ((originType == "S_News" || originType == "S_Product" || originType == "S_Video" || originType == "S_Info") && originID > 0)
                        {
                            InfoVO gvo = cBO.FindInfoById(originID);
                            if (gvo != null)
                            {
                                PersonalVO pVO = cBO.FindPersonalById(gvo.PersonalID);
                                if (pVO != null)
                                {
                                    cVO.originCustomerId = pVO.CustomerId;
                                }
                            }
                        }


                        try
                        {
                            if (cVO.CustomerName != "微信用户")
                            {
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
                            customerMatchVO.AppType = AppVO.AppType;
                            int customerId2 = uBO.AddCustomerMatch(customerMatchVO);
                            if (customerId2 > 0)
                            {
                                CustomerViewVO customerVO2 = uBO.FindCustomerByOpenId(wui.openId, wui.unionId, "2", AppVO.AppType);
                                if (customerVO2 != null)
                                {
                                    CustomerLoginHistoryVO clHistoryVO = new CustomerLoginHistoryVO();
                                    clHistoryVO.LoginAt = DateTime.Now;
                                    clHistoryVO.Status = true;
                                    clHistoryVO.LoginOS = HttpContext.Current.Request.Browser.Platform;
                                    clHistoryVO.LoginIP = HttpContext.Current.Request.UserHostAddress;
                                    clHistoryVO.LoginBrowser = AppVO.AppName;

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

                                    //添加企业名片个人信息
                                    PersonalVO pVO = new PersonalVO();
                                    pVO.CustomerId = customerVO2.CustomerId;
                                    pVO.Name = customerVO2.CustomerName;
                                    pVO.Headimg = customerVO2.HeaderLogo;
                                    pVO.CreatedAt = DateTime.Now;
                                    pVO.AppType = AppVO.AppType;
                                    if (AppType == 30)
                                    {
                                        pVO.BusinessID = 330;
                                    }
                                    cBO.AddPersonal(pVO);

                                    //记录登录信息               
                                    clHistoryVO.CustomerId = customerVO2.CustomerId;
                                    uBO.AddCustomerLoginHistory(clHistoryVO);

                                    //记录名片信息
                                    ulm.Personal = cBO.FindPersonalByCustomerId(customerVO2.CustomerId);

                                    return new ResultObject() { Flag = 1, Message = "注册成功!", Result = ulm, Subsidiary = wui.openId };
                                }
                            }
                        }

                        return new ResultObject() { Flag = 0, Message = "注册失败!", Result = cVO };
                    }
                    else
                    {
                        return new ResultObject() { Flag = 0, Message = "注册失败!", Result = null };
                    }
                }

                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = un };
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CustomerController));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = ex };
            }
        }

        /// <summary>
        /// 小程序获取微信用户信息(企业名片)
        /// </summary>
        /// <param name="wxUserInfoVO">微信小程序用户信息</param>
        /// <param name="code">第一次拉手返回Code</param>
        /// <returns></returns>
        [Route("NewGetMiniprogramUserInfoCard"), HttpPost, Anonymous]
        public ResultObject NewGetMiniprogramUserInfoCard([FromBody] wxUserInfo wxUserInfo, string code, string originType = "", int originID = 0, int AppType = 0)
        {
            string jsonStr = "";
            try
            {
                AppVO AppVO = AppBO.GetApp(AppType);
                string url = "";
                url = "https://api.weixin.qq.com/sns/jscode2session?appid=" + AppVO.AppId + "&secret=" + AppVO.Secret + "&js_code=" + code + "&grant_type=authorization_code";
                jsonStr = HttpHelper.HtmlFromUrlGet(url);
                var serializer = new DataContractJsonSerializer(typeof(OpenIdAndSessionKey));
                var mStream = new MemoryStream(Encoding.Default.GetBytes(jsonStr));
                OpenIdAndSessionKey readConfig = (OpenIdAndSessionKey)serializer.ReadObject(mStream);

                if ((readConfig.unionid != null && readConfig.unionid != "") || (readConfig.openid != null && readConfig.openid != ""))
                {
                    BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
                    CustomerBO uBO = new CustomerBO(new CustomerProfile());
                    CustomerViewVO customerVO = uBO.FindCustomerByOpenId(readConfig.openid, readConfig.unionid, "2", AppVO.AppType);

                    if (customerVO != null)
                    {
                        CustomerLoginHistoryVO clHistoryVO = new CustomerLoginHistoryVO();
                        clHistoryVO.LoginAt = DateTime.Now;
                        clHistoryVO.Status = true;
                        clHistoryVO.LoginOS = HttpContext.Current.Request.Browser.Platform;
                        clHistoryVO.LoginIP = HttpContext.Current.Request.UserHostAddress;
                        clHistoryVO.LoginBrowser = AppVO.AppName;
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

                        string token = CacheManager.TokenInsert(customerVO.CustomerId);
                        CustomerLoginModel ulm = new CustomerLoginModel();
                        customerVO.Password = "";
                        ulm.Customer = customerVO;
                        ulm.Token = token;

                        //判断是否有企业名片的个人信息
                        if (cBO.FindPersonalByCustomerId(customerVO.CustomerId) == null)
                        {
                            CardBO CardBO = new CardBO(new CustomerProfile());
                            PersonalVO pVO = new PersonalVO();

                            List<CardDataVO> cVO = CardBO.FindCardByCustomerId(customerVO.CustomerId);
                            if (cVO.Count > 0)
                            {
                                pVO.CustomerId = customerVO.CustomerId;
                                pVO.Name = cVO[0].Name;
                                pVO.Headimg = cVO[0].Headimg;
                                pVO.Phone = cVO[0].Phone;
                                pVO.Position = cVO[0].Position;
                                pVO.WeChat = cVO[0].WeChat;
                                pVO.Email = cVO[0].Email;
                                pVO.Details = cVO[0].Details;
                                pVO.Address = cVO[0].Address;
                                pVO.Business = cVO[0].Business;
                                pVO.latitude = cVO[0].latitude;
                                pVO.longitude = cVO[0].longitude;
                                pVO.Tel = cVO[0].Tel;
                            }
                            else
                            {
                                pVO.CustomerId = customerVO.CustomerId;
                                pVO.Name = customerVO.CustomerName;
                                pVO.Headimg = customerVO.HeaderLogo;
                            }
                            pVO.AppType = AppVO.AppType;
                            pVO.CreatedAt = DateTime.Now;
                            cBO.AddPersonal(pVO);
                        }


                        //记录登录信息               
                        clHistoryVO.CustomerId = customerVO.CustomerId;
                        uBO.AddCustomerLoginHistory(clHistoryVO);

                        //记录名片信息
                        ulm.Personal = cBO.FindPersonalByCustomerId(customerVO.CustomerId);

                        return new ResultObject() { Flag = 1, Message = "登录成功!", Result = ulm, Subsidiary = readConfig.openid };
                    }
                    else if (readConfig.openid != "" || readConfig.unionid != "")
                    {
                        CustomerVO cVO = new CustomerVO();
                        cVO.CustomerCode = uBO.GetCustomerCode();
                        string password = Utilities.MakePassword(8);
                        cVO.Password = Utilities.GetMD5(password);
                        cVO.Status = 1;
                        cVO.CreatedAt = DateTime.Now;
                        if (wxUserInfo.nickName.Length > 0)
                        {
                            cVO.CustomerName = wxUserInfo.nickName;
                        }
                        cVO.Sex = true;
                        cVO.BusinessCard = true;
                        cVO.originType = originType;
                        cVO.originID = originID;
                        cVO.AppType = AppVO.AppType;

                        //新用户赠送一个月会员
                        /*
                   cVO.isVip = true;
                   cVO.ExpirationAt = DateTime.Now.AddMonths(1);
                   */
                        if (originType == "S_Personal" && originID > 0)
                        {
                            PersonalVO pVO = cBO.FindPersonalById(originID);
                            if (pVO != null)
                            {
                                cVO.originCustomerId = pVO.CustomerId;
                            }
                        }
                        if (originType == "S_JoinBusiness" && originID > 0)
                        {
                            cVO.originCustomerId = cBO.FindJurisdiction(originID);
                        }
                        if (originType == "S_SubsidiaryJoin" && originID > 0)
                        {
                            cVO.originCustomerId = cBO.FindJurisdiction(originID);
                        }
                        if (originType == "S_GreetingCard" && originID > 0)
                        {
                            GreetingCardVO gvo = cBO.FindGreetingCardById(originID);
                            if (gvo != null)
                            {
                                PersonalVO pVO = cBO.FindPersonalById(gvo.PersonalID);
                                if (pVO != null)
                                {
                                    cVO.originCustomerId = pVO.CustomerId;
                                }
                            }
                        }

                        if ((originType == "S_News" || originType == "S_Product" || originType == "S_Video" || originType == "S_Info") && originID > 0)
                        {
                            InfoVO gvo = cBO.FindInfoById(originID);
                            if (gvo != null)
                            {
                                PersonalVO pVO = cBO.FindPersonalById(gvo.PersonalID);
                                if (pVO != null)
                                {
                                    cVO.originCustomerId = pVO.CustomerId;
                                }
                            }
                        }


                        try
                        {
                            if (cVO.CustomerName != "微信用户")
                            {
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
                                WebRequest wreq = WebRequest.Create(wxUserInfo.avatarUrl);
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
                            customerMatchVO.OpenId = readConfig.openid;
                            customerMatchVO.UnionID = readConfig.unionid;
                            customerMatchVO.CustomerId = customerId;
                            customerMatchVO.MatchType = "2";
                            customerMatchVO.AppType = AppVO.AppType;
                            int customerId2 = uBO.AddCustomerMatch(customerMatchVO);
                            if (customerId2 > 0)
                            {
                                CustomerViewVO customerVO2 = uBO.FindCustomerByOpenId(readConfig.openid, readConfig.unionid, "2", AppVO.AppType);
                                if (customerVO2 != null)
                                {
                                    try
                                    {
                                        CustomerLoginHistoryVO clHistoryVO = new CustomerLoginHistoryVO();
                                        clHistoryVO.LoginAt = DateTime.Now;
                                        clHistoryVO.Status = true;
                                        clHistoryVO.LoginOS = HttpContext.Current.Request.Browser.Platform;
                                        clHistoryVO.LoginIP = HttpContext.Current.Request.UserHostAddress;
                                        clHistoryVO.LoginBrowser = AppVO.AppName;
                                        //记录登录信息               
                                        clHistoryVO.CustomerId = customerVO2.CustomerId;
                                        uBO.AddCustomerLoginHistory(clHistoryVO);
                                    }
                                    catch
                                    {

                                    }


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

                                    //添加企业名片个人信息
                                    PersonalVO pVO = new PersonalVO();
                                    pVO.CustomerId = customerVO2.CustomerId;
                                    pVO.Name = customerVO2.CustomerName;
                                    pVO.Headimg = customerVO2.HeaderLogo;
                                    pVO.CreatedAt = DateTime.Now;
                                    pVO.AppType = AppVO.AppType;
                                    if (AppType == 30)
                                    {
                                        pVO.BusinessID = 330;
                                    }
                                    cBO.AddPersonal(pVO);


                                    //记录名片信息
                                    ulm.Personal = cBO.FindPersonalByCustomerId(customerVO2.CustomerId);

                                    return new ResultObject() { Flag = 1, Message = "注册成功!", Result = ulm, Subsidiary = readConfig.openid };
                                }
                            }
                        }

                        return new ResultObject() { Flag = 0, Message = "注册失败!", Result = cVO };
                    }
                    else
                    {
                        return new ResultObject() { Flag = 0, Message = "注册失败!", Result = null };
                    }
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "登录失败!", Result = readConfig };
                }
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CustomerController));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return new ResultObject() { Flag = 0, Message = "登录失败!", Result = ex, Subsidiary = jsonStr };
            }

        }

        /// <summary>
        /// 手机解密并保存
        /// </summary>
        /// <param name="wxPhoneVO">手机VO</param>
        /// <param name="code">第一次拉手返回Code</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetPhone"), HttpPost]
        public ResultObject GetPhone([FromBody] wxPhoneVO wxPhoneVO, string code, string token, int AppType = 0)
        {
            AppVO AppVO = AppBO.GetApp(AppType);
            string url = "";
            url = "https://api.weixin.qq.com/sns/jscode2session?appid=" + AppVO.AppId + "&secret=" + AppVO.Secret + "&js_code=" + code + "&grant_type=authorization_code";

            string jsonStr = HttpHelper.HtmlFromUrlGet(url);
            try
            {

                var serializer = new DataContractJsonSerializer(typeof(OpenIdAndSessionKey));
                var mStream = new MemoryStream(Encoding.Default.GetBytes(jsonStr));
                OpenIdAndSessionKey readConfig = (OpenIdAndSessionKey)serializer.ReadObject(mStream);

                WeChatAppDecrypt un;
                un = new WeChatAppDecrypt(AppVO.AppId, AppVO.Secret);

                WechatPhoneData wui = un.DecryptByPhone(wxPhoneVO.encryptedData, wxPhoneVO.iv, readConfig.session_key);

                UserProfile uProfile = CacheManager.GetUserProfile(token);
                CustomerProfile cProfile = uProfile as CustomerProfile;
                int customerId = cProfile.CustomerId;

                CustomerBO cBO = new CustomerBO(new CustomerProfile());

                if (cBO.FindById(customerId).Phone == "")
                {
                    CustomerVO cVO = new CustomerVO();
                    cVO.CustomerId = customerId;
                    cVO.Phone = wui.phoneNumber;
                    cVO.CustomerAccount = wui.phoneNumber;
                    cBO.Update(cVO);
                }
                BusinessCardBO bBO = new BusinessCardBO(new CustomerProfile());
                PersonalVO pVO = bBO.FindPersonalByCustomerId(customerId);
                pVO.Phone = wui.phoneNumber;
                bBO.UpdatePersonal(pVO);

                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = wui.phoneNumber };
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = jsonStr };
            }
        }

        /// <summary>
        /// 获取我的名片在首页
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getMyCardByIndex"), HttpGet]
        public ResultObject getMyCardByIndex(string token, int AppType = 0)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            CrmBO CrmBO = new CrmBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);

            try
            {
                if (pVO != null)
                {
                    BusinessCardVO bVO = new BusinessCardVO();
                    ThemeVO ThemeVO = new ThemeVO();
                    ThemeVO.setThemeVO();
                    if (pVO.BusinessID == 0)
                    {
                        bVO = null;
                    }
                    else
                    {
                        bVO = cBO.FindBusinessCardById(pVO.BusinessID);
                        //清除营业执照等保密信息
                        bVO.BusinessLicenseImg = "";
                        if (bVO.ThemeID > 0)
                        {
                            ThemeVO = cBO.FindThemeById(bVO.ThemeID);
                        }
                    }

                    if (pVO.QRimg == "")
                    {
                        pVO.QRimg = cBO.GetQRImgByHeadimg(pVO.PersonalID, AppType);
                    }

                    if (pVO.PosterImg3 == "")
                    {
                        pVO.PosterImg3 = cBO.GetPosterCardIMG(pVO.PersonalID, pVO.BusinessID);
                    }

                    if (pVO.PosterImg == "")
                    {
                        pVO.PosterImg = cBO.GetPersonalIMG(pVO.PersonalID);
                    }

                    int ReadNum = 0, toReadNum = 0, todayReadNum = 0, notUsedOrderCount = 0, UsedOrderCount = 0, SecondBusinessCount = 0, ReturnCardCount = 0, UnreadCount = 0, TenderInviteCount = 0;
                    List<AccessrecordsViewVO> aVO = new List<AccessrecordsViewVO>();
                    List<AccessrecordsViewByRespondentsVO> ToaVO = new List<AccessrecordsViewByRespondentsVO>();
                    List<AccessrecordsViewVO> ReturnCardVO = new List<AccessrecordsViewVO>();
                    decimal Balance = 0;
                    decimal Balance2 = 0;

                    try
                    {
                        ReadNum = cBO.FindNumberOfVisitors("Personal", pVO.PersonalID);
                        toReadNum = cBO.FindNumberOfVisitors("Personal", pVO.PersonalID, 1);
                        todayReadNum = cBO.FindNumberOfVisitors("Personal", pVO.PersonalID, 1, 0);
                        aVO = cBO.FindAccessrecordsListOfMonth(null, pVO.PersonalID, 0, 1, false);
                        ToaVO = cBO.FindAccessrecordsViewByRespondentsGroupAllByPageIndex("PersonalID=" + pVO.PersonalID + " and ToPersonalID<>" + pVO.PersonalID + " and ToPersonalID<>1557", 1, 12, "AccessAt", "desc");

                        notUsedOrderCount = cBO.FindOrderViewCount("PersonalID=" + pVO.PersonalID + " and Status=1 and isUsed=0 and OfficialProducts IS NOT NULL");
                        UsedOrderCount = cBO.FindOrderByCondition("PersonalID=" + pVO.PersonalID + " and Status=1 and isUsed>0");
                        SecondBusinessCount = cBO.FindSecondBusinessByPersonalID(pVO.PersonalID).Count;

                        ReturnCardVO = cBO.FindAccessrecordsViewByCondtion("Type = 'ReturnCard' and ToPersonalID=" + pVO.PersonalID + " GROUP BY PersonalID ORDER BY AccessAt DESC LIMIT 1");
                        ReturnCardCount = cBO.FindReturnCardCount(pVO.PersonalID);

                        UnreadCount = CrmBO.FindCommentViewCount(pVO.PersonalID, pVO.BusinessID) + CrmBO.FindApprovalCount(pVO.PersonalID, pVO.BusinessID);

                        RequireBO rBO = new RequireBO(new CustomerProfile());
                        TenderInviteCount = rBO.FindRequireSumByCondtion("TenderInviteCount", "CustomerId=" + pVO.CustomerId);

                        //可提现余额
                        Balance = cBO.getMyRebateCost(customerId, 1);

                        //累计奖金
                        Balance2 = cBO.getMyRebateCost(customerId, 0);
                    }
                    catch
                    {

                    }

                    //直播信息
                    bool isline = false;
                    int lineNo = 3;
                    string lineTitle = "订阅直播";
                    string lineDesc = "7月30号晚上8点准时开播，现场抽奖";
                    List<InfoVO> banner = new List<InfoVO>();
                    if (AppType == 30)
                    {
                        banner = cBO.FindInfoByCondtion("Type IN ('Banner','MyBanner','MallBanner') AND BusinessID=330");
                    }
                    BusinessCard_JurisdictionVO B_Jurisdiction = cBO.getBusinessCard_Jurisdiction(pVO.BusinessID);
                    JurisdictionViewVO jVO = cBO.FindJurisdictionView(pVO.PersonalID, pVO.BusinessID);
                    object res = new { Personal = pVO, BusinessCard = bVO, Jurisdiction = jVO, Banner = banner, B_Jurisdiction = B_Jurisdiction, Theme = ThemeVO, ReadNum = ReadNum, toReadNum = toReadNum, todayReadNum = todayReadNum, AccessList = aVO, ToAccessList = ToaVO, notUsedOrderCount = notUsedOrderCount, UsedOrderCount = UsedOrderCount, ReturnCard = ReturnCardVO, ReturnCardCount = ReturnCardCount, SecondBusinessCount = SecondBusinessCount, UnreadCount = UnreadCount, TenderInviteCount = TenderInviteCount, Balance = Balance, Balance2 = Balance2, isline, lineNo, lineTitle, lineDesc };
                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = res };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
                }
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = ex };
            }

        }

        /// <summary>
        /// 获取我的名片
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getMyCard"), HttpGet]
        public ResultObject getMyCard(string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            CrmBO CrmBO = new CrmBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);

            try
            {
                if (pVO != null)
                {

                    BusinessCardVO bVO = new BusinessCardVO();
                    if (pVO.BusinessID == 0)
                    {
                        bVO = null;
                    }
                    else
                    {
                        bVO = cBO.FindBusinessCardById(pVO.BusinessID);
                    }
                    object res = new { Personal = pVO, BusinessCard = bVO };
                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = res };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
                }
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = ex };
            }

        }

        /// <summary>
        /// 获取我的足迹列表
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        [Route("GetAccessrecordsByRespondentsList"), HttpPost]
        public ResultObject GetAccessrecordsByRespondentsList([FromBody] ConditionModel condition, string token)
        {
            if (condition == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            else if (condition.Filter == null || condition.PageInfo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);

            string conditionStr = "PersonalID = " + pVO.PersonalID + " and ToPersonalID<>" + pVO.PersonalID + " and ToPersonalID<>1557 and (" + condition.Filter.Result() + ")";
            Paging pageInfo = condition.PageInfo;
            List<AccessrecordsViewByRespondentsVO> list = cBO.FindAccessrecordsViewByRespondentsGroupAllByPageIndex(conditionStr, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);
            int count = cBO.FindAccessrecordsViewByRespondentsGroupCount(conditionStr);

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list, Count = count };
        }

        /// <summary>
        /// 获取我的名片数据（扫码页面专用）
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetQRImgByHeadimg"), HttpGet]
        public ResultObject GetQRImgByHeadimg(string token, int AppType = 0)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);

            if (pVO != null)
            {
                pVO.QRimg = cBO.GetQRImgByHeadimg(pVO.PersonalID, AppType);
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = pVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 生成分享海报
        /// </summary>
        /// <returns></returns>
        [Route("GetPosterIMG"), HttpGet, Anonymous]
        public ResultObject GetPosterIMG(int PersonalID, string url, int AppType = 0, int BusinessID = 0, string Code = "")
        {
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalById(PersonalID);

            if (pVO != null)
            {
                string PosterImg2 = cBO.GetPosterIMG(pVO.PersonalID, url, BusinessID, Code, AppType);
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = PosterImg2 };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 生成电子彩页海报
        /// </summary>
        /// <returns></returns>
        [Route("getColorPageQR"), HttpGet, Anonymous]
        public ResultObject getColorPageQR(int PersonalID, int BusinessID, int AppType = 0, string Code = "")
        {
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalById(PersonalID);
            AppVO AppVO = AppBO.GetApp(AppType);
            if (pVO != null)
            {
                string PosterImg2 = cBO.GetColorPageIMG(pVO.PersonalID, BusinessID, AppType, Code);
                if (PosterImg2 != "")
                {
                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = PosterImg2 };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "电子彩页二维码即将上线，敬请期待！！！", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 生成产品海报
        /// </summary>
        /// <returns></returns>
        [Route("getProductImgQR"), HttpGet, Anonymous]
        public ResultObject getProductImgQR(int PersonalID, int InfoID, int AppType = 0, string Code = "", int isPoster = 1)
        {
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalById(PersonalID);
            InfoVO InfoVO = cBO.FindInfoById(InfoID);
            AppVO AppVO = AppBO.GetApp(AppType);
            if (pVO != null && InfoVO != null)
            {
                string PosterImg2 = cBO.GetProductIMG(pVO.PersonalID, InfoID, AppType, Code, isPoster);
                if (PosterImg2 != "")
                {
                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = PosterImg2 };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "生成失败!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 生成加入拼团二维码
        /// </summary>
        /// <returns></returns>
        [Route("getGroupBuyQR"), HttpGet, Anonymous]
        public ResultObject getGroupBuyQR(int GroupBuyID, int AppType = 0)
        {
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            GroupBuyVO gVO = cBO.FindGroupBuyById(GroupBuyID);
            AppVO AppVO = AppBO.GetApp(AppType);
            if (gVO != null)
            {
                string PosterImg2 = cBO.GetJoinGroupBuyQR(GroupBuyID, AppType);
                if (PosterImg2 != "")
                {
                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = PosterImg2 };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "生成失败!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取加入集团二维码
        /// </summary>
        /// <returns></returns>
        [Route("getJoinGroupQR"), HttpGet, Anonymous]
        public ResultObject getJoinGroupQR(int BusinessID, int AppType = 0)
        {
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            BusinessCardVO bVO = cBO.FindBusinessCardById(BusinessID);
            AppVO AppVO = AppBO.GetApp(AppType);
            if (bVO == null || bVO.isGroup == 0)
            {
                return new ResultObject() { Flag = 0, Message = "该企业不是集团版名片!", Result = null };
            }

            string PosterImg2 = cBO.GetJoinGroupQR(BusinessID, AppType);
            if (PosterImg2 != "")
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = PosterImg2 };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取二维码
        /// </summary>
        /// <returns></returns>
        [Route("getQR"), HttpGet, Anonymous]
        public ResultObject getQR(int PersonalID, int BusinessID, int currentIndex)
        {
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            string scene = PersonalID + "," + BusinessID + ",qymp," + currentIndex;

            string QRimg = cBO.GetQRcode(scene, 640, "pages/CardShow/CardShow/CardShow", "/UploadFolder/ProductFile/", 0);
            if (QRimg != "")
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = QRimg };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 更新个人信息
        /// </summary>
        /// <param name="PersonalVO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdatePersonal"), HttpPost]
        public ResultObject UpdatePersonal([FromBody] PersonalVO PersonalVO, string token, int AppType = 0)
        {
            if (PersonalVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO = CustomerBO.FindCustomenById(customerId);

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());

            /*审核文本是否合法*/
            if (!cBO.msg_sec_check(PersonalVO))
            {
                return new ResultObject() { Flag = 0, Message = "有非法关键词，请重新填写!", Result = null };
            }
            /*审核文本是否合法*/


            if (PersonalVO.PersonalID > 0)
            {
                PersonalVO cVO = cBO.FindPersonalById(PersonalVO.PersonalID);
                if (cVO.CustomerId != customerId)
                {
                    return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
                }

                /*只允许更新部分内容*/
                cVO.Name = PersonalVO.Name;
                cVO.Headimg = PersonalVO.Headimg;
                cVO.Position = PersonalVO.Position;
                cVO.Business = PersonalVO.Business;
                cVO.Address = PersonalVO.Address;
                cVO.latitude = PersonalVO.latitude;
                cVO.longitude = PersonalVO.longitude;
                cVO.Phone = PersonalVO.Phone;
                cVO.Tel = PersonalVO.Tel;
                cVO.Email = PersonalVO.Email;
                cVO.WeChat = PersonalVO.WeChat;
                cVO.Details = PersonalVO.Details;
                cVO.CardBack = PersonalVO.CardBack;

                if ((cVO.latitude == 0 || cVO.longitude == 0) && cVO.Address != "")
                {
                    CardBO ccBO = new CardBO(new CustomerProfile());
                    WeiXinGeocoder Geocoder = ccBO.getLatitudeAndLongitude(cVO.Address);
                    if (Geocoder != null)
                    {
                        cVO.latitude = Geocoder.result.location.lat;
                        cVO.longitude = Geocoder.result.location.lng;
                    }
                }

                if (CustomerVO.CustomerName == "微信用户")
                {
                    CustomerVO.CustomerName = PersonalVO.Name;
                    CustomerVO.HeaderLogo = PersonalVO.Headimg;
                    CustomerBO.Update(CustomerVO);
                }
                if ((CustomerVO.HeaderLogo == "" || CustomerVO.HeaderLogo == "undefined") && PersonalVO.Headimg != "" && PersonalVO.Headimg != "undefined")
                {
                    CustomerVO.HeaderLogo = PersonalVO.Headimg;
                    CustomerBO.Update(CustomerVO);
                }

                if (cBO.UpdatePersonal(cVO))
                {
                    /*生成分享图片*/
                    try
                    {
                        PersonalVO pVO = new PersonalVO();
                        pVO.PersonalID = cVO.PersonalID;
                        pVO.PosterImg = cBO.GetPersonalIMG(pVO.PersonalID);
                        pVO.PosterImg3 = cBO.GetPosterCardIMG(pVO.PersonalID, pVO.BusinessID);
                        pVO.QRimg = cBO.GetQRImgByHeadimg(pVO.PersonalID, AppType);
                        cBO.UpdatePersonal(pVO);
                    }
                    catch
                    {

                    }
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = cVO };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }
            }
            return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
        }

        /// <summary>
        /// 生成分享海报
        /// </summary>
        /// <returns></returns>
        [Route("GetPosterIMG"), HttpGet, Anonymous]
        public ResultObject GetPosterIMG(int PersonalID)
        {
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalById(PersonalID);

            if (pVO != null)
            {
                string PosterImg2 = cBO.GetPersonalIMG(pVO.PersonalID);
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = PosterImg2 };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取我的官网数据
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getMyWeb"), HttpGet]
        public ResultObject getMyWeb(string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            if (pVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }

            BusinessCardVO bVO = cBO.FindBusinessCardById(pVO.BusinessID);
            //清除营业执照等保密信息
            bVO.BusinessLicenseImg = "";
            if (bVO != null)
            {
                WebVO WebVO = cBO.FindWebByBusinessID(bVO.BusinessID);
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = WebVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "请先开通企业名片，或加入同事已开通的企业名片!", Result = null };
            }
        }

        /// <summary>
        /// 更新官网数据
        /// </summary>
        /// <param name="WebVO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateWeb"), HttpPost]
        public ResultObject UpdateWeb([FromBody] WebVO WebVO, string token)
        {
            if (WebVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());

            /*审核文本是否合法*/
            /*
            if (!cBO.msg_sec_check(WebVO))
            {
                return new ResultObject() { Flag = 0, Message = "有非法关键词，请重新填写!", Result = null };
            }*/
            /*审核文本是否合法*/

            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);

            if (pVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }

            if (!cBO.FindJurisdiction(pVO.PersonalID, WebVO.BusinessID, "Web") && !cBO.FindJurisdiction(pVO.PersonalID, WebVO.BusinessID, "Admin"))
            {
                return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
            }

            BusinessCardVO bVO = cBO.FindBusinessCardById(WebVO.BusinessID);
            //清除营业执照等保密信息
            bVO.BusinessLicenseImg = "";
            if (bVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }
            if (bVO.Address != WebVO.Address)
            {
                //跟据地址识别出经纬度
                CardBO CardBO = new CardBO(new CustomerProfile());
                WeiXinGeocoder Geocoder = CardBO.getLatitudeAndLongitude(WebVO.Address);
                if (Geocoder != null)
                {
                    WebVO.latitude = Geocoder.result.location.lat;
                    WebVO.longitude = Geocoder.result.location.lng;
                }
            }

            if (cBO.UpdateWeb(WebVO))
            {
                WebVO = cBO.FindWebByBusinessID(WebVO.BusinessID);
                return new ResultObject() { Flag = 1, Message = "更新成功!", Result = WebVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取轮播图
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getMySwiper"), HttpGet]
        public ResultObject getMySwiper(string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            if (pVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }

            BusinessCardVO bVO = cBO.FindBusinessCardById(pVO.BusinessID);
            //清除营业执照等保密信息
            bVO.BusinessLicenseImg = "";
            if (bVO != null)
            {
                WebVO WebVO = new WebVO();
                WebVO.BusinessID = pVO.BusinessID;
                WebVO.banner = cBO.FindInfoByCondtion("Type IN ('Banner','MyBanner','MallBanner') AND BusinessID=" + bVO.BusinessID);
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = WebVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "请先开通企业名片，或加入同事已开通的企业名片!", Result = null };
            }
        }

        /// <summary>
        /// 更新轮播图
        /// </summary>
        /// <param name="WebVO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateSwiper"), HttpPost]
        public ResultObject UpdateSwiper([FromBody] WebVO WebVO, string token)
        {
            try
            {
                if (WebVO == null)
                {
                    return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
                }
                UserProfile uProfile = CacheManager.GetUserProfile(token);
                CustomerProfile cProfile = uProfile as CustomerProfile;
                int customerId = cProfile.CustomerId;

                BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());

                PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);

                if (pVO == null)
                {
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }

                //if ( !cBO.FindJurisdiction(pVO.PersonalID, WebVO.BusinessID, "Admin"))
                //{
                //    return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
                //}

                BusinessCardVO bVO = cBO.FindBusinessCardById(WebVO.BusinessID);
                if (bVO == null)
                {
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }

                if (cBO.UpdateSwiper(WebVO))
                {
                    WebVO.banner = cBO.FindInfoByCondtion("Type IN ('Banner','MyBanner','MallBanner') AND BusinessID=" + bVO.BusinessID);
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = WebVO };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }
            }
            catch (Exception ex)
            {

                return new ResultObject() { Flag = 0, Message = "更新失败!", Result = ex };
            }

        }

        /// <summary>
        /// 更新自定义栏目名称
        /// </summary>
        /// <param name="WebVO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateCustomColumn"), HttpGet]
        public ResultObject UpdateCustomColumn(string CustomColumn, string token)
        {
            if (CustomColumn == "")
            {
                return new ResultObject() { Flag = 0, Message = "请输入内容!", Result = null };
            }
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());

            /*审核文本是否合法*/
            /*
            if (!cBO.msg_sec_check(WebVO))
            {
                return new ResultObject() { Flag = 0, Message = "有非法关键词，请重新填写!", Result = null };
            }*/
            /*审核文本是否合法*/

            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);

            if (pVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }

            if (!cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Web") && !cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Admin"))
            {
                return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
            }

            BusinessCardVO bVO = cBO.FindBusinessCardById(pVO.BusinessID);
            if (bVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }

            bVO.CustomColumn = CustomColumn;

            if (cBO.UpdateBusinessCard(bVO))
            {
                return new ResultObject() { Flag = 1, Message = "更新成功!", Result = bVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }
        }

        /// <summary>
        /// 更新门店栏目名称
        /// </summary>
        /// <param name="WebVO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateProductColumn"), HttpGet]
        public ResultObject UpdateProductColumn(string ProductColumn, string token)
        {
            if (ProductColumn == "")
            {
                return new ResultObject() { Flag = 0, Message = "请输入内容!", Result = null };
            }
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());

            /*审核文本是否合法*/
            /*
            if (!cBO.msg_sec_check(WebVO))
            {
                return new ResultObject() { Flag = 0, Message = "有非法关键词，请重新填写!", Result = null };
            }*/
            /*审核文本是否合法*/

            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);

            if (pVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }

            if (!cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Web") && !cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Admin"))
            {
                return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
            }

            BusinessCardVO bVO = cBO.FindBusinessCardById(pVO.BusinessID);
            if (bVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }

            bVO.ProductColumn = ProductColumn;

            if (cBO.UpdateBusinessCard(bVO))
            {
                return new ResultObject() { Flag = 1, Message = "更新成功!", Result = bVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取导航
        /// </summary>
        /// <param name="SortID"></param>
        /// <returns></returns>
        [Route("GetNavigation"), HttpGet]
        public ResultObject GetNavigation(string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);

            if (pVO != null)
            {
                List<InfoSortVO> sVO = cBO.FindNavigationList(pVO.BusinessID);
                for (int i = 0; i < sVO.Count; i++)
                {
                    sVO[i].InfoSortlist = cBO.FindNavigationList(pVO.BusinessID, sVO[i].SortID);
                }
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = sVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="VO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DelInfoSort"), HttpGet]
        public ResultObject DelInfoSort(int SortID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            InfoSortVO InfoSortVO = cBO.FindInfoSortById(SortID);

            if (InfoSortVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
            }
            if (!cBO.FindJurisdiction(pVO.PersonalID, InfoSortVO.BusinessID, "Web") && !cBO.FindJurisdiction(pVO.PersonalID, InfoSortVO.BusinessID, "Admin"))
            {
                return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
            }



            if (cBO.DeleteInfoSortById(SortID) > 0)
            {
                //删除子分类
                cBO.DeleteInfoSortByToid(SortID);
                return new ResultObject() { Flag = 1, Message = "删除成功!" };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取板块详情，匿名
        /// </summary>
        /// <param name="SortID"></param>
        /// <returns></returns>
        [Route("GetInfoSortSite"), HttpGet, Anonymous]
        public ResultObject GetInfoSortSite(int SortID)
        {
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            InfoSortVO sVO = cBO.FindInfoSortById(SortID);

            if (sVO != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = sVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 更新板块详情
        /// </summary>
        /// <param name="SortID"></param>
        /// <returns></returns>
        [Route("UpdateInfoSortSite"), HttpPost]
        public ResultObject UpdateInfoSortSite([FromBody] InfoSortVO InfoSortVO, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);

            /*审核文本是否合法*/
            /*
            if (!cBO.msg_sec_check(InfoSortVO))
            {
                return new ResultObject() { Flag = 0, Message = "有非法关键词，请重新填写!", Result = null };
            }*/
            /*审核文本是否合法*/

            if (pVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }

            if (!cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Web") && !cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Admin"))
            {
                return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
            }

            InfoSortVO sVO = new InfoSortVO();
            sVO.SortID = InfoSortVO.SortID;
            sVO.Content = InfoSortVO.Content;

            if (cBO.UpdateInfoSort(sVO))
            {
                return new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }
        }

        /// <summary>
        /// 更新板块详情
        /// </summary>
        /// <param name="SortID"></param>
        /// <returns></returns>
        [Route("UpdateInfoSort"), HttpPost]
        public ResultObject UpdateInfoSort([FromBody] InfoSortVO InfoSortVO, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);

            /*审核文本是否合法*/
            /*
            if (!cBO.msg_sec_check(InfoSortVO.SortName))
            {
                return new ResultObject() { Flag = 0, Message = "有非法关键词，请重新填写!", Result = null };
            }*/
            /*审核文本是否合法*/

            if (pVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }

            if (!cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Web") && !cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Admin"))
            {
                return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
            }
            if (InfoSortVO.SortID > 0)
            {
                if (InfoSortVO.BusinessID != pVO.BusinessID && InfoSortVO.SortID > 0)
                {
                    return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
                }
                if (cBO.UpdateInfoSort(InfoSortVO))
                {
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = InfoSortVO };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }
            }
            else
            {
                InfoSortVO.BusinessID = pVO.BusinessID;
                InfoSortVO.CreatedAt = DateTime.Now;
                int SortID = cBO.AddInfoSort(InfoSortVO);
                if (SortID > 0)
                {
                    InfoSortVO.SortID = SortID;
                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = InfoSortVO };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
                }
            }
        }

        /// <summary>
        /// 获取信息详情，匿名
        /// </summary>
        /// <param name="SortID"></param>
        /// <returns></returns>
        [Route("GetInfoSite"), HttpGet, Anonymous]
        public ResultObject GetInfoSite(int InfoID, int AppType = 0)
        {
            try
            {
                BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
                InfoViewVO sVO = cBO.FindInfoViewById(InfoID);
                AppVO AppVO = AppBO.GetApp(AppType);
                if (sVO.InfoQR == "")
                {
                    //sVO.InfoQR = cBO.GetInfoQRimg(InfoID, sVO.Title, AppType);
                }

                if (sVO != null)
                {
                    BusinessCardVO bVO = cBO.FindBusinessCardById(sVO.BusinessID);
                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = sVO, Subsidiary = bVO };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
                }
            }
            catch (Exception ex)
            {

                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = ex };
            }

        }

        /// <summary>
        /// 获取信息详情
        /// </summary>
        /// <param name="SortID"></param>
        /// <returns></returns>
        [Route("GetInfoSite"), HttpGet]
        public ResultObject GetInfoSite(int InfoID, string token, int AppType = 0)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            InfoViewVO sVO = cBO.FindInfoViewById(InfoID);
            AppVO AppVO = AppBO.GetApp(AppType);
            if (sVO != null)
            {
                if (sVO.InfoQR == "")
                {
                    sVO.InfoQR = cBO.GetInfoQRimg(InfoID, sVO.Title, AppType);
                }

                bool isJurisdiction = false;
                if (cBO.FindJurisdiction(pVO.PersonalID, sVO.BusinessID, "Web") || cBO.FindJurisdiction(pVO.PersonalID, sVO.BusinessID, "Admin"))
                {
                    isJurisdiction = true;
                }

                bool isStaff = false;
                if (pVO.BusinessID == sVO.BusinessID)
                {
                    isStaff = true;
                }

                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = new { Info = sVO, isJurisdiction = isJurisdiction, isStaff = isStaff } };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取产品详情，匿名
        /// </summary>
        /// <param name="SortID"></param>
        /// <returns></returns>
        [Route("GetProductSite"), HttpGet, Anonymous]
        public ResultObject GetProductSite(int InfoID, string token = "", int PersonalID = 0, int AppType = 0)
        {
            try
            {
                BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
                InfoViewVO sVO = cBO.FindInfoViewById(InfoID);

                if (sVO != null)
                {
                    bool CustomerisGroupBuy = false;
                    int Discount = 100;
                    decimal DiscountCost = sVO.Cost;
                    decimal SeckillDiscountCost = sVO.Cost;
                    List<InfoCostVO> InfoCostList = cBO.FindInfoCostList("Status=1 and InfoID=" + sVO.InfoID + " group by CostName");
                    if (token != "" && sVO.isGroupBuy == 1)
                    {
                        Discount = sVO.GroupBuyDiscount;
                        DiscountCost = sVO.Cost * sVO.GroupBuyDiscount / 100;
                        UserProfile uProfile = CacheManager.GetUserProfile(token);

                        if (uProfile != null)
                        {
                            CustomerProfile cProfile = uProfile as CustomerProfile;
                            int customerId = cProfile.CustomerId;
                            List<GroupBuyMemberVO> gVO = cBO.FindGroupBuyMemberList("CustomerId =" + customerId + " and BusinessID=" + sVO.BusinessID + " and InfoID=" + InfoID);
                            if (gVO.Count > 0)
                            {
                                CustomerisGroupBuy = true;
                                GroupBuyVO gbVO = cBO.FindGroupBuyById(gVO[0].GroupBuyID);
                                if (gbVO != null)
                                {
                                    Discount = gbVO.Discount;
                                    decimal Cost = sVO.Cost * gbVO.Discount / 100;
                                    DiscountCost = System.Convert.ToDecimal(Convert.ToInt32(Cost * 100).ToString()) / 100;
                                    foreach (InfoCostVO vo in InfoCostList)
                                    {
                                        decimal InfoCost = vo.Cost * gbVO.Discount / 100;
                                        vo.DiscountCost = Convert.ToDecimal(Convert.ToInt32(InfoCost * 100).ToString()) / 100;
                                    }
                                }

                            }
                        }
                    }

                    if (sVO.isSeckill == 1)
                    {
                        //秒杀时间截止自动关闭
                        if (sVO.SeckillEndTime < DateTime.Now)
                        {
                            InfoVO IVO = new InfoVO();
                            IVO.InfoID = sVO.InfoID;
                            IVO.isSeckill = 0;
                            cBO.UpdateInfo(IVO);
                            sVO.isSeckill = 0;
                        }
                        else
                        {
                            decimal Cost = sVO.Cost * sVO.SeckillDiscount / 100;
                            SeckillDiscountCost = Convert.ToDecimal(Convert.ToInt32(Cost * 100).ToString()) / 100;
                            foreach (InfoCostVO vo in InfoCostList)
                            {
                                decimal InfoCost = vo.Cost * sVO.SeckillDiscount / 100;
                                vo.SeckillDiscountCost = Convert.ToDecimal(Convert.ToInt32(InfoCost * 100).ToString()) / 100;
                            }
                        }
                    }

                    List<OrderByGroupbuyIdViewVO> OrderByGroupBuy = new List<OrderByGroupbuyIdViewVO>();
                    if (sVO.isGroupBuy == 1)
                    {
                        OrderByGroupBuy = cBO.FindOrderByGroupbuyIdViewList("InfoID=" + InfoID + " and Status=3 and  ExpireAt > now()");
                    }

                    List<InfoViewVO> sListVO = cBO.FindInfoViewByInfoID("Products", sVO.BusinessID, 0, "Order_info desc,CreatedAt desc", 10);
                    sVO.AgentlevelCostList = cBO.FindAgentByAgentlevelCostID(sVO.InfoID);

                    int ReadNum = sVO.ReadCount;
                    BusinessCardVO bVO = cBO.FindBusinessCardById(sVO.BusinessID);

                    string Phone = "";
                    if (PersonalID > 0)
                    {
                        PersonalVO pVO = cBO.FindPersonalById(PersonalID);
                        if (pVO != null)
                        {
                            Phone = pVO.Phone;
                        }

                    }
                    else
                    {
                        PersonalVO pVO = cBO.FindPersonalById(sVO.PersonalID);
                        if (pVO != null)
                        {
                            Phone = pVO.Phone;
                        }
                    }

                    //如果企业是二级商户，则关掉拼团和返佣显示
                    //判断是否为二级商户
                    EcommerceBO eBO = new EcommerceBO();
                    wxMerchantVO mVO = eBO.getBusinessMerchant(sVO.BusinessID);
                    if (mVO != null && AppType == 0)
                    {
                        sVO.isGroupBuy = 0;
                        sVO.isProfitsharing = 0;
                    }

                    bool isShareWindow = false;
                    if (sVO.isProfitsharing == 1 && sVO.isProfitsharingToVIP == 0 && (sVO.Profitsharing > 0 || sVO.TowProfitsharing > 0))
                    {
                        isShareWindow = true;
                    }
                    if (sVO.isProfitsharing == 1 && sVO.isProfitsharingToVIP == 1)
                    {
                        if (token != "")
                        {
                            UserProfile uProfile = CacheManager.GetUserProfile(token);
                            if (uProfile != null)
                            {
                                CustomerProfile cProfile = uProfile as CustomerProfile;
                                int customerId = cProfile.CustomerId;
                                PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
                                int pro = cBO.GetProfitsharing(pVO.PersonalID, sVO.InfoID);
                                if (pro > 0)
                                {
                                    isShareWindow = true;
                                    sVO.Profitsharing = pro;
                                }

                                int pro2 = cBO.GetTowProfitsharing(pVO.CustomerId, sVO.InfoID);
                                if (pro2 > 0)
                                {
                                    isShareWindow = true;
                                    sVO.TowProfitsharing = pro2;
                                }
                            }
                        }

                    }
                    bool flag = true;//判断是否存在与常规同名字规格
                    foreach (InfoCostVO vo in InfoCostList)
                    {
                        if (vo.CostName.Equals(sVO.CostName))
                        {
                            flag = false;
                        }
                    }
                    if (flag)
                    {
                        InfoCostVO vo = new InfoCostVO();
                        vo.CostID = 0;
                        vo.InfoID = sVO.InfoID;
                        vo.Cost = sVO.Cost;
                        vo.DiscountCost = DiscountCost;
                        vo.SeckillDiscountCost = SeckillDiscountCost;
                        vo.CostName = sVO.CostName;
                        vo.PerPersonLimit = sVO.PerPersonLimit;
                        vo.GiveIntegral = sVO.GiveIntegral;

                        InfoCostList.Reverse();
                        InfoCostList.Add(vo);
                        InfoCostList.Reverse();
                    }

                    object res = new { Info = sVO, List = sListVO, ReadNum = ReadNum, BusinessCardVO = bVO, CustomerisGroupBuy = CustomerisGroupBuy, Discount = Discount, DiscountCost = DiscountCost, InfoCostList = InfoCostList, SeckillDiscountCost = SeckillDiscountCost, ServicePhone = Phone, OrderByGroupBuy = OrderByGroupBuy, isShareWindow = isShareWindow };
                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = res };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
                }
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(BusinessCardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = ex };
            }


        }

        /// <summary>
        /// 获取产品规格尺寸
        /// </summary>
        /// <param name="InfoID"></param>
        /// <returns></returns>
        [Route("GetInfoAttribute"), HttpGet, Anonymous]
        public ResultObject GetInfoAttribute(int CostID, int InfoID, string token = "")
        {
            try
            {
                BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
                InfoCostVO InfoCost = cBO.FindInfoCostById(CostID);
                InfoViewVO sVO = cBO.FindInfoViewById(InfoID);
                List<InfoCostVO> InfoCostList = cBO.FindInfoCostList("Status=1 and InfoID=" + InfoID + " and CostName='" + InfoCost.CostName + "'");
                if (InfoCostList != null)
                {
                    foreach (InfoCostVO vo in InfoCostList)
                    {
                        if (string.IsNullOrEmpty(vo.Attribute))
                        {
                            vo.Attribute = "常规";
                        }


                        if (sVO.isSeckill == 1)
                        {
                            decimal Cost = vo.Cost * sVO.SeckillDiscount / 100;
                            vo.SeckillDiscountCost = Convert.ToDecimal(Convert.ToInt32(Cost * 100).ToString()) / 100;
                        }
                        if (sVO.isGroupBuy == 1)
                        {
                            vo.DiscountCost = vo.Cost * sVO.GroupBuyDiscount / 100;
                        }

                        if (token != "")
                        {
                            UserProfile uProfile = CacheManager.GetUserProfile(token);
                            CustomerProfile cProfile = uProfile as CustomerProfile;
                            int customerId = cProfile.CustomerId;
                            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);

                            BusinessCardVO bVO = cBO.FindBusinessCardById(sVO.BusinessID);
                            if (bVO.isAgent == 1)
                            {
                                List<AgentViewVO> AgentViewVO = cBO.FindAgentViewByPersonalID(sVO.BusinessID, pVO.PersonalID);
                                if (AgentViewVO.Count > 0)
                                {
                                    vo.isAgent = 1;
                                    vo.AgentCost = cBO.FindAgentlevelCostByPersonalID(pVO.PersonalID, sVO.InfoID, vo.CostID);
                                }
                            }

                            if (sVO.isVipDiscount == 1)
                            {
                                decimal VipCost = vo.Cost * cBO.GetVipDiscount(pVO.PersonalID, sVO.InfoID) / 100;
                                if (VipCost < vo.Cost)
                                {
                                    vo.isVipCost = 1;
                                    vo.VipCost = VipCost;
                                }
                            }
                        }

                    }
                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = InfoCostList };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
                }
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = ex };
            }
        }

        /// <summary>
        /// 获取产品列表，匿名
        /// </summary>
        /// <param name="SortID"></param>
        /// <returns></returns>
        [Route("GetProductList"), HttpGet, Anonymous]
        public ResultObject GetProductList()
        {
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            List<InfoViewVO> product = cBO.FindInfoViewByInfoID("Products", 1, 0, "Order_info desc,CreatedAt desc", 30);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = product };
        }

        /// <summary>
        /// 获取视频详情，匿名
        /// </summary>
        /// <param name="SortID"></param>
        /// <returns></returns>
        [Route("GetVideoSite"), HttpGet, Anonymous]
        public ResultObject GetVideoSite(int InfoID)
        {
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            InfoViewVO sVO = cBO.FindInfoViewById(InfoID);

            if (sVO != null)
            {
                List<InfoViewVO> sListVO = cBO.FindInfoViewByInfoID("Video", sVO.BusinessID, 0, "Order_info desc,CreatedAt desc", 10, "Video<>''");
                int ReadNum = sVO.ReadCount;
                BusinessCardVO bVO = cBO.FindBusinessCardById(sVO.BusinessID);

                object res = new { Info = sVO, List = sListVO, ReadNum = ReadNum, BusinessCardVO = bVO };
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = res };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }



        /// <summary>
        /// 添加或更新信息
        /// </summary>
        /// <param name="VO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateInfoSite"), HttpPost]
        public ResultObject UpdateInfoSite([FromBody] InfoVO InfoVO, string token)
        {
            if (InfoVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());

            /*审核文本是否合法*/

            if (!cBO.msg_sec_check(InfoVO))
            {
                return new ResultObject() { Flag = 0, Message = "有非法关键词，请重新填写!", Result = null };
            }
            /*审核文本是否合法*/

            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            int BusinessID = 0;
            if (InfoVO.InfoID > 0)
            {
                InfoVO iVO = cBO.FindInfoById(InfoVO.InfoID);
                BusinessID = iVO.BusinessID;
            }
            else
            {
                BusinessID = pVO.BusinessID;
            }


            if (InfoVO.Type == "Banner" || InfoVO.Type == "ModularList" || InfoVO.Type == "News" || InfoVO.Type == "Invoice" || InfoVO.Type == "Account" || InfoVO.Type == "Certificates" || InfoVO.Type == "knowledge" || InfoVO.Type == "OtherDocuments")
            {
                if (!cBO.FindJurisdiction(pVO.PersonalID, BusinessID, "Web") && !cBO.FindJurisdiction(pVO.PersonalID, BusinessID, "Admin"))
                {
                    return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
                }
            }
            if (InfoVO.Type == "Products")
            {
                if (!cBO.FindJurisdiction(pVO.PersonalID, BusinessID, "Product") && !cBO.FindJurisdiction(pVO.PersonalID, BusinessID, "Admin"))
                {
                    return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
                }
            }

            if (InfoVO.InfoID > 0)
            {
                InfoVO sVO = cBO.FindInfoById(InfoVO.InfoID);
                InfoVO.PersonalID = pVO.PersonalID;
                InfoVO.CreatedAt = sVO.CreatedAt;
                InfoVO.BusinessID = sVO.BusinessID;
                InfoVO.OfficialProducts = sVO.OfficialProducts;
                InfoVO.Video = sVO.Video;
                InfoVO.Duration = sVO.Duration;
                if (cBO.UpdateInfo(InfoVO))
                {
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = sVO.InfoID };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }
            }
            else
            {
                InfoVO.CreatedAt = DateTime.Now;
                InfoVO.PersonalID = pVO.PersonalID;
                InfoVO.BusinessID = pVO.BusinessID;
                InfoVO.OfficialProducts = null;

                int InfoID = cBO.AddInfo(InfoVO);
                if (InfoID > 0)
                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = InfoID };
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
        }

        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="VO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DelInfo"), HttpGet]
        public ResultObject DelInfo(int InfoID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            InfoVO InfoVO = cBO.FindInfoById(InfoID);

            if (InfoVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
            }

            if (InfoVO.OfficialProducts != null && InfoVO.OfficialProducts != "")
            {
                return new ResultObject() { Flag = 0, Message = "系统绑定产品不能删除!", Result = null };
            }

            if (InfoVO.Type == "Banner" || InfoVO.Type == "ModularList" || InfoVO.Type == "News" || InfoVO.Type == "Invoice" || InfoVO.Type == "Account" || InfoVO.Type == "Certificates" || InfoVO.Type == "knowledge" || InfoVO.Type == "OtherDocuments")
            {
                if (!cBO.FindJurisdiction(pVO.PersonalID, InfoVO.BusinessID, "Web") && !cBO.FindJurisdiction(pVO.PersonalID, InfoVO.BusinessID, "Admin"))
                {
                    return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
                }
            }
            if (InfoVO.Type == "Products")
            {
                if (!cBO.FindJurisdiction(pVO.PersonalID, InfoVO.BusinessID, "Product") && !cBO.FindJurisdiction(pVO.PersonalID, InfoVO.BusinessID, "Admin"))
                {
                    return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
                }
            }
            InfoVO.Status = -2;
            string Video = InfoVO.Video;
            InfoVO.Video = "";
            if (cBO.UpdateInfo(InfoVO))
            {
                try
                {//删除旧视频
                    if (Video != "")
                    {
                        //修改官网上的视频板块
                        List<InfoSortVO> sVO = cBO.FindInfoSortList("ModularVideo", InfoVO.BusinessID);
                        for (int i = 0; i < sVO.Count; i++)
                        {
                            if (sVO[i].Content == Video)
                            {
                                sVO[i].Content = "";
                                cBO.UpdateInfoSort(sVO[i]);
                            }
                        }

                        string FilePath = Video;
                        FilePath = FilePath.Replace(ConfigInfo.Instance.APIURL, "");
                        FilePath = ConfigInfo.Instance.UploadFolder + FilePath;
                        File.Delete(FilePath);
                    }
                    //删除官网导航

                    List<InfoSortVO> cVO = cBO.FindInfoSortList("Content like '%\"InfoID\":" + InfoVO.InfoID + ",%'");
                    for (int i = 0; i < cVO.Count; i++)
                    {
                        System.Text.RegularExpressions.Match m = Regex.Match(cVO[i].Content, @"[^\f\n\r\t\v{]*""InfoID"":" + InfoVO.InfoID + "[^\f\n\r\t\v}]*");
                        cVO[i].Content = cVO[i].Content.Replace(m.ToString(), "");
                        if (cVO[i].Content.Contains(",{}"))
                        {
                            cVO[i].Content = cVO[i].Content.Replace(",{}", "");
                        }
                        if (cVO[i].Content.Contains("{},"))
                        {
                            cVO[i].Content = cVO[i].Content.Replace("{},", "");
                        }
                        cVO[i].Content = cVO[i].Content.Replace("{}", "");
                        cBO.UpdateInfoSort(cVO[i]);
                    }
                }
                catch
                {

                }

                return new ResultObject() { Flag = 1, Message = "删除成功!", Result = InfoVO.InfoID };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
            }
        }
        /// <summary>
        /// 添加或更新产品价格
        /// </summary>
        /// <param name="VO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("test4"), HttpGet, Anonymous]
        public ResultObject test4()
        {
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            List<InfoSortVO> cVO = cBO.FindInfoSortList("Content like '%\"InfoID\":193,%'");

            for (int i = 0; i < cVO.Count; i++)
            {
                System.Text.RegularExpressions.Match m = Regex.Match(cVO[i].Content, @"[^\f\n\r\t\v{]*""InfoID"":193[^\f\n\r\t\v}]*");
                cVO[i].Content = cVO[i].Content.Replace(m.ToString(), "");
                cVO[i].Content = cVO[i].Content.Replace("{}", "");
                cVO[i].Content = cVO[i].Content.Replace(",,", ",");
            }

            return new ResultObject() { Flag = 0, Message = "参数为空!", Result = cVO };
        }


        /// <summary>
        /// 添加或更新产品价格
        /// </summary>
        /// <param name="VO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateInfoCost"), HttpPost]
        public ResultObject UpdateInfoCost([FromBody] InfoCostVO InfoCostVO, string token)
        {
            if (InfoCostVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());

            /*审核文本是否合法*/
            /*
            if (!cBO.msg_sec_check(InfoVO))
            {
                return new ResultObject() { Flag = 0, Message = "有非法关键词，请重新填写!", Result = null };
            }*/
            /*审核文本是否合法*/

            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            int BusinessID = 0;

            InfoVO InfoVO = cBO.FindInfoById(InfoCostVO.InfoID);
            if (InfoVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }
            BusinessID = InfoVO.BusinessID;


            if (InfoVO.Type == "Banner" || InfoVO.Type == "ModularList" || InfoVO.Type == "News" || InfoVO.Type == "Invoice" || InfoVO.Type == "Account" || InfoVO.Type == "Certificates" || InfoVO.Type == "knowledge" || InfoVO.Type == "OtherDocuments")
            {
                if (!cBO.FindJurisdiction(pVO.PersonalID, BusinessID, "Web") && !cBO.FindJurisdiction(pVO.PersonalID, BusinessID, "Admin"))
                {
                    return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
                }
            }
            if (InfoVO.Type == "Products")
            {
                if (!cBO.FindJurisdiction(pVO.PersonalID, BusinessID, "Product") && !cBO.FindJurisdiction(pVO.PersonalID, BusinessID, "Admin"))
                {
                    return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
                }
            }

            if (InfoCostVO.CostID > 0)
            {
                if (cBO.UpdateInfoCost(InfoCostVO))
                {
                    List<InfoCostVO> Costlist = cBO.FindInfoCostList("Status=1 and InfoID=" + InfoCostVO.InfoID);
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = Costlist };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }
            }
            else
            {
                int CostID = cBO.AddInfoCost(InfoCostVO);
                if (CostID > 0)
                {
                    List<InfoCostVO> Costlist = cBO.FindInfoCostList("Status=1 and InfoID=" + InfoCostVO.InfoID);
                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = Costlist };
                }
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
        }

        /// <summary>
        /// 删除产品规格
        /// </summary>
        /// <param name="VO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DelInfoCost"), HttpGet]
        public ResultObject DelInfoCost(int CostID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);

            InfoCostVO InfoCostVO = cBO.FindInfoCostById(CostID);

            if (InfoCostVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
            }

            InfoVO InfoVO = cBO.FindInfoById(InfoCostVO.InfoID);

            if (InfoVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
            }

            if (InfoVO.OfficialProducts != null && InfoVO.OfficialProducts != "")
            {
                return new ResultObject() { Flag = 0, Message = "系统绑定产品不能删除!", Result = null };
            }

            if (InfoVO.Type == "Banner" || InfoVO.Type == "ModularList" || InfoVO.Type == "News" || InfoVO.Type == "Invoice" || InfoVO.Type == "Account" || InfoVO.Type == "Certificates" || InfoVO.Type == "knowledge" || InfoVO.Type == "OtherDocuments")
            {
                if (!cBO.FindJurisdiction(pVO.PersonalID, InfoVO.BusinessID, "Web") && !cBO.FindJurisdiction(pVO.PersonalID, InfoVO.BusinessID, "Admin"))
                {
                    return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
                }
            }
            if (InfoVO.Type == "Products")
            {
                if (!cBO.FindJurisdiction(pVO.PersonalID, InfoVO.BusinessID, "Product") && !cBO.FindJurisdiction(pVO.PersonalID, InfoVO.BusinessID, "Admin"))
                {
                    return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
                }
            }
            InfoCostVO.Status = -2;
            if (cBO.UpdateInfoCost(InfoCostVO))
            {
                List<InfoCostVO> Costlist = cBO.FindInfoCostList("Status=1 and InfoID=" + InfoCostVO.InfoID);
                return new ResultObject() { Flag = 1, Message = "删除成功!", Result = Costlist };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        [Route("GetInfoList"), HttpPost]
        public ResultObject GetInfoList([FromBody] ConditionModel condition, string token)
        {
            if (condition == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            else if (condition.Filter == null || condition.PageInfo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);

            string conditionStr = "BusinessID = " + pVO.BusinessID + " and Status<>-2 and (" + condition.Filter.Result() + ")";
            Paging pageInfo = condition.PageInfo;
            List<InfoViewVO> list = cBO.FindInfoViewAllByPageIndex(conditionStr, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);
            int count = cBO.FindInfoViewCount(conditionStr);

            bool isJurisdiction = false;
            if (cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Web") || cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Admin"))
            {
                isJurisdiction = true;
            }
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Type == "Products")
                {
                    //获取产品规格列表
                    list[i].InfoCostList = cBO.FindInfoCostList("Status=1 and InfoID=" + list[i].InfoID);
                }
            }
            BusinessCardVO bVO = cBO.FindBusinessCardById(pVO.BusinessID);
            //清除营业执照等保密信息
            bVO.BusinessLicenseImg = "";

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list, Count = count, Subsidiary = new { isJurisdiction = isJurisdiction, BusinessName = bVO.BusinessName, BusinessID = bVO.BusinessID, CustomColumn = bVO.CustomColumn, ProductColumn = bVO.ProductColumn } };
        }

        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="AgentLevelID">代理级别ID</param>
        /// <returns></returns>
        [Route("GetInfoList"), HttpPost]
        public ResultObject GetInfoList([FromBody] ConditionModel condition, int AgentLevelID, string token)
        {
            if (condition == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            else if (condition.Filter == null || condition.PageInfo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);

            string conditionStr = "BusinessID = " + pVO.BusinessID + " and Status<>-2 and (" + condition.Filter.Result() + ")";
            Paging pageInfo = condition.PageInfo;
            List<InfoViewVO> list = cBO.FindInfoViewAllByPageIndex(conditionStr, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);
            int count = cBO.FindInfoViewCount(conditionStr);

            bool isJurisdiction = false;
            if (cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Web") || cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Admin"))
            {
                isJurisdiction = true;
            }

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Type == "Products")
                {
                    //获取代理价格列表
                    list[i].AgentlevelCostList = cBO.FindAgentByAgentlevelCostID(list[i].InfoID, AgentLevelID);
                }

            }

            BusinessCardVO bVO = cBO.FindBusinessCardById(pVO.BusinessID);
            //清除营业执照等保密信息
            bVO.BusinessLicenseImg = "";
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list, Count = count, Subsidiary = new { isJurisdiction = isJurisdiction, BusinessName = bVO.BusinessName, BusinessID = bVO.BusinessID } };
        }

        /// <summary>
        /// 获取信息列表,匿名
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        [Route("GetInfoList"), HttpPost, Anonymous]
        public ResultObject GetInfoList([FromBody] ConditionModel condition, int BusinessID)
        {
            if (condition == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            else if (condition.Filter == null || condition.PageInfo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());

            string conditionStr = "BusinessID = " + BusinessID + " and Status<>-2 and Status<>0 and (" + condition.Filter.Result() + ")";
            Paging pageInfo = condition.PageInfo;
            List<InfoViewVO> list = cBO.FindInfoViewAllByPageIndex(conditionStr, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);
            int count = cBO.FindInfoViewCount(conditionStr);
            BusinessCardVO bVO = cBO.FindBusinessCardById(BusinessID);

            List<InfoSortVO> InfoSort = cBO.FindInfoSortList("ProductSort", BusinessID);
            InfoSortVO SVO = new InfoSortVO();
            SVO.SortID = 0;
            SVO.SortName = "全部分类";
            InfoSort.Reverse();
            InfoSort.Add(SVO);
            InfoSort.Reverse();
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list, Count = count, Subsidiary = new { BusinessName = bVO.BusinessName, Logo = bVO.LogoImg, DisplayCard = bVO.DisplayCard, InfoSort = InfoSort }, Subsidiary2 = (" + condition.Filter.Result() + ") };
        }

        /// <summary>
        /// 获取分享页数据，匿名
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getCardByShare"), HttpGet, Anonymous]
        public ResultObject getCardByShare(int PersonalID, int SortID = 0, int OrderNO = 0, int BusinessID = 0, int AppType = 0)
        {
            try
            {
                BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
                if (PersonalID == 0)
                {
                    PersonalID = AppBO.GetApp(AppType).TPersonalID;
                }
                PersonalVO pVO = cBO.FindPersonalById(PersonalID);
                if (pVO != null)
                {
                    pVO.ReadNum = cBO.FindNumberOfVisitors("Personal", pVO.PersonalID);
                    pVO.todayReadNum = cBO.FindNumberOfVisitors("Personal", pVO.PersonalID, 1, 0);

                    if (pVO.QRimg == "")
                    {
                        pVO.QRimg = cBO.GetQRImgByHeadimg(pVO.PersonalID, AppType);
                    }

                    if (pVO.PosterImg3 == "" || BusinessID > 0)
                    {
                        pVO.PosterImg3 = cBO.GetPosterCardIMG(pVO.PersonalID, BusinessID);
                    }

                    WebVO WebVO = new WebVO();
                    List<InfoViewVO> product = new List<InfoViewVO>();
                    List<InfoViewVO> CaseList = new List<InfoViewVO>();

                    if (BusinessID == 0 || BusinessID == pVO.BusinessID)
                        BusinessID = pVO.BusinessID;
                    else
                    {
                        //判断是否是指定公司成员
                        List<SecondBusinessVO> sListVO = cBO.FindSecondBusinessByPersonalID(pVO.PersonalID, BusinessID);
                        if (sListVO.Count <= 0)
                        {
                            BusinessID = pVO.BusinessID;
                        }
                        else
                        {
                            pVO.Position = sListVO[0].Position;
                        }
                    }

                    ThemeVO ThemeVO = new ThemeVO();
                    ThemeVO.setThemeVO();
                    BusinessCardVO BusinessCardVO = null;
                    if (BusinessID > 0)
                    {
                        WebVO = cBO.FindWebByBusinessID(BusinessID);


                        string Order = "Order_info desc,CreatedAt desc";
                        if (OrderNO == 1)
                        {
                            Order = "CreatedAt desc";
                        }
                        if (OrderNO == 2)
                        {
                            Order = "ReadCount desc";
                        }
                        if (OrderNO == 3)
                        {
                            Order = "Cost asc";
                        }
                        product = cBO.FindInfoViewByInfoID("Products", BusinessID, SortID, Order, 29);
                        CaseList = cBO.FindInfoViewByInfoID("Case", BusinessID, 0, "Order_info desc,CreatedAt desc", 20);

                        BusinessCardVO bVO = cBO.FindBusinessCardById(BusinessID);
                        if (bVO != null)
                        {
                            if (bVO.ThemeID > 0)
                            {
                                ThemeVO = cBO.FindThemeById(bVO.ThemeID);
                            }
                            BusinessCardVO = bVO;
                        }

                    }
                    else
                    {
                        WebVO = null;
                    }

                    //获取导航
                    List<InfoSortVO> sVO = cBO.FindNavigationList(BusinessID);
                    for (int i = 0; i < sVO.Count; i++)
                    {
                        sVO[i].InfoSortlist = cBO.FindNavigationList(pVO.BusinessID, sVO[i].SortID);
                    }
                    int AccessNumber = cBO.FindNumberOfVisits("Personal", pVO.PersonalID, 1); //名片访问次数

                    string conditionStr = "ToPersonalID =" + pVO.PersonalID + " and PersonalID<>" + pVO.PersonalID;
                    List<AccessrecordsViewVO> Accesslist = new List<AccessrecordsViewVO>();
                    Accesslist = cBO.FindAccessrecordsViewGroupAllByPageIndex(conditionStr, 1, 10, "AccessAt", "desc");

                    //获取他的任务
                    string condition = " CustomerId = " + pVO.CustomerId + " and Status=1 and EffectiveEndDate > now() ";
                    RequireBO uBO = new RequireBO(new CustomerProfile());
                    List<RequirementViewVO> list = uBO.FindRequireAllByPageIndex(condition, 1, 10, "CreatedAt", "desc");
                    int count = uBO.FindRequireTotalCount(condition);

                    BusinessCard_JurisdictionVO B_Jurisdiction = cBO.getBusinessCard_Jurisdiction(BusinessID);

                    List<InfoSortVO> InfoSort = cBO.FindInfoSortList("ProductSort", BusinessID);

                    InfoSortVO SVO = new InfoSortVO();
                    SVO.SortID = 0;
                    SVO.SortName = "全部分类";
                    InfoSort.Reverse();
                    InfoSort.Add(SVO);
                    InfoSort.Reverse();

                    //首页显示活动
                    List<BCPartyVO> partyvo = cBO.FindBCPartyByCondtion("BusinessID=" + BusinessID + " AND AppType=" + pVO.AppType + " AND IsDisplayIndex=1 and EndTime > now()");

                    object res = new
                    {
                        Personal = pVO,
                        Web = WebVO,
                        PartyList = partyvo,
                        Theme = ThemeVO,
                        Navigation = sVO,
                        B_Jurisdiction = B_Jurisdiction,
                        Products = product,
                        AccessNumber = AccessNumber,
                        Accesslist = Accesslist,
                        RequirementList = list,
                        RequirementCount = count,
                        CaseList = CaseList,
                        BusinessCard = BusinessCardVO,
                        ProductSort = InfoSort
                    };
                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = res };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
                }
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = -1, Message = "获取失败!", Result = ex.ToString() };
            }

        }
        /// <summary>
        /// 获取名片数据，匿名
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getCard"), HttpGet, Anonymous]
        public ResultObject getCard(int PersonalID, int BusinessID = 0)
        {
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalById(PersonalID);
            if (pVO != null)
            {
                if (BusinessID == 0 || BusinessID == pVO.BusinessID)
                    BusinessID = pVO.BusinessID;
                else
                {
                    List<SecondBusinessVO> sListVO = cBO.FindSecondBusinessByPersonalID(pVO.PersonalID, BusinessID);
                    if (sListVO.Count <= 0)
                    {
                        BusinessID = pVO.BusinessID;
                    }
                    else
                    {
                        pVO.Position = sListVO[0].Position;
                    }
                }
                object res = new { Personal = pVO };
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = res };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }
        /// <summary>
        /// 获取名片贺卡数据，匿名
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getPersonal"), HttpGet, Anonymous]
        public ResultObject getPersonal(int PersonalID, int GreetingCardID, int BusinessID = 0)
        {
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalById(PersonalID);
            GreetingCardVO GreetingCardVO = cBO.FindGreetingCardById(GreetingCardID);
            if (pVO != null && GreetingCardVO != null)
            {
                BusinessCardVO bVO = new BusinessCardVO();

                if (BusinessID == 0 || BusinessID == pVO.BusinessID)
                    BusinessID = pVO.BusinessID;
                else
                {
                    List<SecondBusinessVO> sListVO = cBO.FindSecondBusinessByPersonalID(pVO.PersonalID, BusinessID);
                    if (sListVO.Count <= 0)
                    {
                        BusinessID = pVO.BusinessID;
                    }
                    else
                    {
                        pVO.Position = sListVO[0].Position;
                    }
                }
                if (BusinessID > 0)
                {
                    bVO = cBO.FindBusinessCardById(BusinessID);
                    //清除营业执照等保密信息
                    bVO.BusinessLicenseImg = "";
                }
                else
                {
                    bVO = null;
                }

                GreetingCardVO.ReadCount += 1;
                cBO.UpdateGreetingCard(GreetingCardVO);

                int Forward = cBO.FindNumberOfVisits("GreetingCard", GreetingCardVO.GreetingCardID);

                object res = new { Personal = pVO, Business = bVO, GreetingCard = GreetingCardVO, Forward = Forward };
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = res };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "短视频已被删除!", Result = null };
            }
        }

        /// <summary>
        /// 获取名片贺卡
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetGreetingCardSite"), HttpGet, Anonymous]
        public ResultObject GetGreetingCardSite(int GreetingCardID)
        {
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            GreetingCardVO GreetingCardVO = cBO.FindGreetingCardById(GreetingCardID);
            if (GreetingCardVO != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = GreetingCardVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取名片贺卡模板列表，匿名
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getGreetingCardList"), HttpGet, Anonymous]
        public ResultObject getGreetingCardList()
        {
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            List<GreetingCardVO> List = cBO.FindGreetingCard();
            if (List != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = List };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取名片贺卡模板列表
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getGreetingCardList"), HttpGet]
        public ResultObject getGreetingCardList(string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            List<GreetingCardVO> List = cBO.FindGreetingCard(pVO.PersonalID);
            if (List != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = List };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 添加或更新贺卡
        /// </summary>
        /// <param name="VO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateGreetingCardSite"), HttpPost]
        public ResultObject UpdateGreetingCardSite([FromBody] GreetingCardVO GreetingCardVO, string token)
        {
            if (GreetingCardVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());

            /*审核文本是否合法*/
            /*
            if (!cBO.msg_sec_check(GreetingCardVO))
            {
                return new ResultObject() { Flag = 0, Message = "有非法关键词，请重新填写!", Result = null };
            }*/
            /*审核文本是否合法*/

            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);

            if (GreetingCardVO.GreetingCardID > 0)
            {
                GreetingCardVO GVO = cBO.FindGreetingCardById(GreetingCardVO.GreetingCardID);

                GreetingCardVO.PersonalID = pVO.PersonalID;
                GreetingCardVO.CreatedAt = GVO.CreatedAt;
                GreetingCardVO.Video = GVO.Video;
                if (cBO.UpdateGreetingCard(GreetingCardVO))
                {
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = GreetingCardVO.GreetingCardID };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }
            }
            else
            {
                GreetingCardVO.CreatedAt = DateTime.Now;
                GreetingCardVO.PersonalID = pVO.PersonalID;

                int GreetingCardID = cBO.AddGreetingCard(GreetingCardVO);
                if (GreetingCardID > 0)
                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = GreetingCardID };
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
        }
        /// <summary>
        /// 删除贺卡
        /// </summary>
        /// <param name="VO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DelGreetingCard"), HttpGet]
        public ResultObject DelGreetingCard(int GreetingCardID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            GreetingCardVO GVO = cBO.FindGreetingCardById(GreetingCardID);

            if (GVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
            }


            if (GVO.PersonalID != pVO.PersonalID)
            {
                return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
            }

            try
            {
                //删除视频
                if (GVO.Video != "")
                {
                    string FilePath = GVO.Video;
                    FilePath = FilePath.Replace(ConfigInfo.Instance.APIURL, "");
                    FilePath = ConfigInfo.Instance.UploadFolder + FilePath;
                    File.Delete(FilePath);
                }

                //删除图片
                if (GVO.ShareImage != "")
                {
                    string FilePath = GVO.ShareImage;
                    FilePath = FilePath.Replace(ConfigInfo.Instance.APIURL, "");
                    FilePath = ConfigInfo.Instance.UploadFolder + FilePath;
                    File.Delete(FilePath);
                }
            }
            catch
            {

            }
            if (cBO.DeleteGreetingCardById(GreetingCardID) > 0)
            {
                return new ResultObject() { Flag = 1, Message = "删除成功!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
            }
        }

        /// <summary>
        /// 上传贺卡视频
        /// </summary>
        /// <returns></returns>
        [Route("UploadGreetingCardVideo"), HttpPost]
        public ResultObject UploadGreetingCardVideo(int GreetingCardID, string token, int duration)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            GreetingCardVO GVO = cBO.FindGreetingCardById(GreetingCardID);

            if (duration > 60)
            {
                return new ResultObject() { Flag = 0, Message = "最大只能上传1分钟长度的视频!", Result = null };
            }

            if (GVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
            }

            if (GVO.PersonalID != pVO.PersonalID)
            {
                return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
            }

            HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
            string folder = "/UploadFolder/Video/" + DateTime.Now.ToString("yyyyMM") + "/";
            string imgPath = "";
            if (hfc.Count > 0)
            {
                try
                {
                    FileInfo fi = new FileInfo(hfc[0].FileName);

                    string ext = fi.Extension.ToLower();
                    if (ext.IndexOf("asp") > -1 || ext.IndexOf("aspx") > -1 || ext.IndexOf("php") > -1)
                    {
                        return new ResultObject() { Flag = 0, Message = "文件类型错误", Result = null };
                    }

                    string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + fi.Extension;

                    //本地路径
                    string localPath = ConfigInfo.Instance.UploadFolder + folder;
                    if (!Directory.Exists(localPath))
                    {
                        Directory.CreateDirectory(localPath);
                    }
                    string PhysicalPath = localPath + newFileName;
                    imgPath = "~" + folder + newFileName;
                    hfc[0].SaveAs(PhysicalPath);

                    /*
                    string ThumbnailImg = "";
                    try
                    {
                        //封面路径
                        string ThumbnailImgfolder = "/UploadFolder/VideoThumbnail/";
                        string ThumbnailImglocalPath = ConfigInfo.Instance.UploadFolder + ThumbnailImgfolder;
                        string ThumbnailImgFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".jpg";

                        //截取封面
                        Thumbnail Thumbnail = new Thumbnail();
                        //Thumbnail.GenerateThumbnail(PhysicalPath, ThumbnailImglocalPath+ ThumbnailImgFileName);

                        TimeoutHelper timeHelper = new TimeoutHelper();
                        Thumbnail.originalFilePath = PhysicalPath;
                        Thumbnail.outputFilePath = ThumbnailImglocalPath + ThumbnailImgFileName;

                        timeHelper.Do = Thumbnail.GenerateThumbnail;
                        if (timeHelper.DoWithTimeout(new TimeSpan(0, 0, 0, 5)) == false)
                        {
                            ThumbnailImg = ConfigInfo.Instance.APIURL + ThumbnailImgfolder + ThumbnailImgFileName;
                        }
                        else
                        {
                            //若超时，则主动关闭进程
                            ThumbnailImg = "";
                        }
                    }

                    catch
                    {

                    }
                    */

                    //网络路径
                    string url = ConfigInfo.Instance.APIURL + folder + newFileName;

                    try
                    {//删除旧视频
                        if (GVO.Video != "")
                        {
                            string FilePath = GVO.Video;
                            FilePath = FilePath.Replace(ConfigInfo.Instance.APIURL, "");
                            FilePath = ConfigInfo.Instance.UploadFolder + FilePath;
                            File.Delete(FilePath);
                        }
                    }
                    catch
                    {

                    }



                    //保存连接
                    GVO.Video = url;
                    cBO.UpdateGreetingCard(GVO);

                    return new ResultObject() { Flag = 1, Message = "上传成功", Result = url, Subsidiary = null };
                }
                catch (Exception er)
                {
                    return new ResultObject() { Flag = 0, Message = "上传失败", Result = er.Message, Subsidiary = "" };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "上传失败", Result = "文件为空", Subsidiary = "" };
            }
        }

        /// <summary>
        /// 获取产品彩页，匿名
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getColorPage"), HttpGet, Anonymous]
        public ResultObject getColorPage(int BusinessID, int PersonalID)
        {
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            BusinessCardVO bVO = new BusinessCardVO();

            if (BusinessID == 0)
            {
                bVO = null;
            }
            else
            {
                bVO = cBO.FindBusinessCardById(BusinessID);
            }
            if (bVO != null)
            {
                //清除营业执照等保密信息
                bVO.BusinessLicenseImg = "";
                List<InfoViewVO> product = cBO.FindInfoViewByInfoID("Products", BusinessID, 0, "Order_info desc,CreatedAt desc", 100);
                PersonalVO pVO = cBO.FindPersonalById(PersonalID);

                object res = new { BusinessCard = bVO, Products = product, Personal = pVO };
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = res };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取公司信息，加入页面
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getJoinBusiness"), HttpGet]
        public ResultObject getJoinBusiness(int BusinessID, string token)
        {
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            BusinessCardVO bVO = cBO.FindBusinessCardById(BusinessID);

            if (bVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            //清除营业执照等保密信息
            bVO.BusinessLicenseImg = "";

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            if (pVO != null)
            {
                List<PersonalVO> Personal = cBO.FindPersonalByBusinessID(bVO.BusinessID);
                bool isjion = cBO.isJoinBusiness(pVO, bVO.BusinessID);
                //List<DepartmentVO> DVO = cBO.FindDepartmentList(bVO.BusinessID);
                BusinessCardVO HeadquartersVO = new BusinessCardVO();
                if (bVO.HeadquartersID > 0)
                {
                    HeadquartersVO = cBO.FindBusinessCardById(bVO.HeadquartersID);
                }
                JurisdictionViewVO jVO = cBO.FindJurisdictionView(pVO.PersonalID, bVO.BusinessID);
                BusinessCard_JurisdictionVO B_Jurisdiction = cBO.getBusinessCard_Jurisdiction(bVO.BusinessID);


                object res = new { BusinessCard = bVO, PersonalList = Personal, B_Jurisdiction = B_Jurisdiction, IsJion = isjion, HeadquartersVO = HeadquartersVO, Jurisdiction = jVO };
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = res };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取公司信息(后台专用)
        /// </summary>
        /// <param name="BusinessID">ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getBusinessCard"), HttpGet]
        public ResultObject getBusinessCard(int BusinessID, string token)
        {
            UserProfile cProfile = CacheManager.GetUserProfile(token);
            if (cProfile.UserId <= 0)
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());

            BusinessCardVO bVO = cBO.FindBusinessCardById(BusinessID);

            if (bVO != null)
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = bVO };
            else
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
        }

        /// <summary>
        /// 添加或更新公司(后台专用)
        /// </summary>
        /// <param name="BusinessCardVO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateBusinessCard"), HttpPost]
        public ResultObject UpdateBusinessCard([FromBody] BusinessCardVO BusinessCardVO, string token)
        {
            UserProfile cProfile = CacheManager.GetUserProfile(token);
            if (cProfile.UserId <= 0)
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
            if (BusinessCardVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());

            /*审核文本是否合法*/
            /*
            if (!cBO.msg_sec_check(BusinessCardVO))
            {
                return new ResultObject() { Flag = 0, Message = "有非法关键词，请重新填写!", Result = null };
            }*/
            /*审核文本是否合法*/

            if (BusinessCardVO.BusinessID > 0)
            {
                if (BusinessCardVO.JoinQR == "" || BusinessCardVO.JoinQR == null)
                {
                    BusinessCardVO.JoinQR = cBO.GetJoinQR(BusinessCardVO.BusinessID, 0);
                }
                if (cBO.UpdateBusinessCard(BusinessCardVO))
                {
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = BusinessCardVO.BusinessID };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }
            }
            else
            {
                BusinessCardVO.CreatedAt = DateTime.Now;

                if (cBO.FindBusinessCardTotalCount("BusinessName= '" + BusinessCardVO.BusinessName + "'") > 0)
                {
                    return new ResultObject() { Flag = 0, Message = "添加失败,该公司名已存在!", Result = null };
                }

                int BusinessID = cBO.AddBusinessCard(BusinessCardVO);
                if (BusinessID > 0)
                {
                    cBO.GetJoinQR(BusinessID, 0);
                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = BusinessID };
                }
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
        }

        /// <summary>
        /// 更新公司
        /// </summary>
        /// <param name="BusinessCardVO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateBusinessCardByBusinessAdmin"), HttpPost]
        public ResultObject UpdateBusinessCardByBusinessAdmin([FromBody] BusinessCardVO BusinessCardVO, string token)
        {


            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());

            /*审核文本是否合法*/
            /*
            if (!cBO.msg_sec_check(BusinessCardVO))
            {
                return new ResultObject() { Flag = 0, Message = "有非法关键词，请重新填写!", Result = null };
            }*/
            /*审核文本是否合法*/

            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);

            if (pVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }

            if (!cBO.FindJurisdiction(pVO.PersonalID, BusinessCardVO.BusinessID, "Web") && !cBO.FindJurisdiction(pVO.PersonalID, BusinessCardVO.BusinessID, "Admin"))
            {
                return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
            }

            if (BusinessCardVO.BusinessID > 0)
            {
                BusinessCardVO bVO = new BusinessCardVO();

                bVO.BusinessID = BusinessCardVO.BusinessID;
                bVO.BusinessName = BusinessCardVO.BusinessName;
                bVO.LogoImg = BusinessCardVO.LogoImg;
                bVO.Industry = BusinessCardVO.Industry;

                if (cBO.UpdateBusinessCard(bVO))
                {
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = bVO.BusinessID };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取退出当前公司
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("OutBusiness"), HttpGet]
        public ResultObject OutBusiness(string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            if (pVO != null)
            {
                if (cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Admin") && pVO.BusinessID != 73)
                {
                    return new ResultObject() { Flag = 0, Message = "您是管理员，请先移交管理员权限!", Result = null };
                }
                //清除所有权限
                cBO.DeleteJurisdiction(pVO.PersonalID, pVO.BusinessID);

                //如果有其他附属公司
                List<SecondBusinessVO> sVO = cBO.FindSecondBusinessByPersonalID(pVO.PersonalID);
                if (sVO.Count > 0)
                {
                    pVO.BusinessID = sVO[0].BusinessID;
                    pVO.DepartmentID = sVO[0].DepartmentID;
                    pVO.Position = sVO[0].Position;
                    pVO.isExternal = sVO[0].isExternal;
                    cBO.DeleteSecondBusinessById(sVO[0].PersonalID, sVO[0].BusinessID);
                }
                else
                {
                    pVO.BusinessID = 0;
                    pVO.DepartmentID = 0;
                    pVO.isExternal = false;
                }

                if (cBO.UpdatePersonal(pVO))
                {
                    return new ResultObject() { Flag = 1, Message = "退出成功!", Result = null };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "退出失败!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "退出失败!", Result = null };
            }
        }

        /// <summary>
        /// 加入公司
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("JoinBusiness"), HttpGet]
        public ResultObject JoinBusiness(int BusinessID, string token, int isExternal = 0, int AppType = 0)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            if (pVO != null)
            {
                BusinessCardVO bVO = cBO.FindBusinessCardById(BusinessID);
                if (bVO == null)
                {
                    return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
                }
                if (bVO.Status == 0)
                {
                    return new ResultObject() { Flag = 0, Message = "该公司已被删除，无法加入!", Result = null };
                }
                /*
                if (bVO.BusinessID == 73)
                {
                    return new ResultObject() { Flag = 0, Message = "体验版本正在调整中，暂不支持加入!", Result = null };
                }*/
                int PersonalCount = cBO.FindPersonalCountByBusinessID(BusinessID);
                if (PersonalCount >= bVO.Number)
                {
                    return new ResultObject() { Flag = 0, Message = "人数已满，无法加入!", Result = null };
                }
                bool bo = false;
                if (isExternal == 0)
                {
                    bo = cBO.JoinBusiness(pVO, BusinessID, AppType);
                }
                else
                {
                    bo = cBO.JoinBusiness(pVO, BusinessID, AppType, true);
                }
                if (bo)
                {
                    return new ResultObject() { Flag = 1, Message = "加入成功!", Result = null };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "加入失败!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "加入失败!", Result = null };
            }
        }

        /// <summary>
        /// 通过名片添加成员进入公司
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("AddPersonalofCard"), HttpGet]
        public ResultObject AddPersonalofCard(string CardID, string token, int AppType = 0)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            AppVO AppVO = AppBO.GetApp(AppType);
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            CardBO CardBO = new CardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            if (pVO != null && pVO.BusinessID > 0)
            {
                BusinessCardVO bVO = cBO.FindBusinessCardById(pVO.BusinessID);
                if (bVO == null)
                {
                    return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
                }
                int PersonalCount = cBO.FindPersonalCountByBusinessID(bVO.BusinessID);
                if (PersonalCount >= bVO.Number)
                {
                    return new ResultObject() { Flag = 0, Message = "人数已满!", Result = null };
                }
                try
                {
                    if (!string.IsNullOrEmpty(CardID))
                    {
                        string[] CardIDIdArr = CardID.Split(',');
                        bool isAllDelete = true;
                        for (int i = 0; i < CardIDIdArr.Length; i++)
                        {
                            try
                            {
                                CardDataVO cVO = CardBO.FindCardById(Convert.ToInt32(CardIDIdArr[i]));
                                if (cVO == null)
                                {
                                    isAllDelete = false;
                                    continue;
                                }

                                PersonalCount = cBO.FindPersonalCountByBusinessID(bVO.BusinessID);
                                if (PersonalCount >= bVO.Number)
                                {
                                    isAllDelete = false;
                                    break;
                                }

                                PersonalVO PersonalVO = cBO.FindPersonalByCustomerId(cVO.CustomerId);

                                //判断是否有企业名片的个人信息
                                if (PersonalVO == null)
                                {
                                    PersonalVO PlVO = new PersonalVO();
                                    PlVO.CustomerId = cVO.CustomerId;
                                    PlVO.Name = cVO.Name;
                                    PlVO.Headimg = cVO.Headimg;
                                    PlVO.Phone = cVO.Phone;
                                    PlVO.Position = cVO.Position;
                                    PlVO.WeChat = cVO.WeChat;
                                    PlVO.Email = cVO.Email;
                                    PlVO.Details = cVO.Details;
                                    PlVO.Address = cVO.Address;
                                    PlVO.Business = cVO.Business;
                                    PlVO.latitude = cVO.latitude;
                                    PlVO.longitude = cVO.longitude;
                                    PlVO.Tel = cVO.Tel;
                                    PlVO.CreatedAt = DateTime.Now;
                                    pVO.AppType = AppVO.AppType;

                                    PlVO.BusinessID = bVO.BusinessID;

                                    cBO.AddPersonal(PlVO);
                                }
                                else
                                {
                                    cBO.JoinBusiness(PersonalVO, bVO.BusinessID, AppType);
                                }
                            }
                            catch
                            {
                                isAllDelete = false;
                            }
                        }
                        if (isAllDelete)
                        {
                            return new ResultObject() { Flag = 1, Message = "添加成员成功!", Result = null };
                        }
                        else
                        {
                            return new ResultObject() { Flag = 1, Message = "部分添加成功!", Result = null };
                        }

                    }
                    else
                    {
                        return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
                    }
                }
                catch
                {
                    return new ResultObject() { Flag = 0, Message = "添加成员失败!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "添加成员失败!", Result = null };
            }
        }


        /// <summary>
        /// 通过企业版名片添加成员进入公司
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("AddPersonalofBusinessCard"), HttpGet]
        public ResultObject AddPersonalofBusinessCard(string PersonalID, string token, int AppType = 0)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            CardBO CardBO = new CardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            if (pVO != null && pVO.BusinessID > 0)
            {
                BusinessCardVO bVO = cBO.FindBusinessCardById(pVO.BusinessID);
                if (bVO == null)
                {
                    return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
                }
                int PersonalCount = cBO.FindPersonalCountByBusinessID(bVO.BusinessID);
                if (PersonalCount >= bVO.Number)
                {
                    return new ResultObject() { Flag = 0, Message = "人数已满!", Result = null };
                }
                try
                {
                    if (!string.IsNullOrEmpty(PersonalID))
                    {
                        string[] CardIDIdArr = PersonalID.Split(',');
                        bool isAllDelete = true;
                        for (int i = 0; i < CardIDIdArr.Length; i++)
                        {
                            try
                            {
                                PersonalVO PersonalVO = cBO.FindPersonalById(Convert.ToInt32(CardIDIdArr[i]));
                                if (PersonalVO == null)
                                {
                                    isAllDelete = false;
                                    continue;
                                }

                                PersonalCount = cBO.FindPersonalCountByBusinessID(bVO.BusinessID);
                                if (PersonalCount >= bVO.Number)
                                {
                                    isAllDelete = false;
                                    break;
                                }

                                cBO.JoinBusiness(PersonalVO, bVO.BusinessID, AppType);
                            }
                            catch
                            {
                                isAllDelete = false;
                            }
                        }
                        if (isAllDelete)
                        {
                            return new ResultObject() { Flag = 1, Message = "添加成员成功!", Result = null };
                        }
                        else
                        {
                            return new ResultObject() { Flag = 1, Message = "部分添加成功!", Result = null };
                        }

                    }
                    else
                    {
                        return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
                    }
                }
                catch
                {
                    return new ResultObject() { Flag = 0, Message = "添加成员失败!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "添加成员失败!", Result = null };
            }
        }

        /// <summary>
        /// 删除成员
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("OutBusiness"), HttpGet]
        public ResultObject OutBusiness(int PersonalID, string token, int AppType = 0)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            PersonalVO tpVO = cBO.FindPersonalById(PersonalID);
            if (pVO != null && tpVO != null)
            {

                if (cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Admin") || cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Personnel"))
                {
                    if (cBO.FindJurisdiction(tpVO.PersonalID, pVO.BusinessID, "Admin"))
                    {
                        return new ResultObject() { Flag = 0, Message = "他是管理员，无法删除!", Result = null };
                    }

                    //清除所有权限
                    cBO.DeleteJurisdiction(tpVO.PersonalID, pVO.BusinessID);

                    if (pVO.BusinessID == tpVO.BusinessID)
                    {
                        //如果有其他附属公司
                        List<SecondBusinessVO> sVO = cBO.FindSecondBusinessByPersonalID(tpVO.PersonalID);
                        if (sVO.Count > 0)
                        {
                            tpVO.BusinessID = sVO[0].BusinessID;
                            tpVO.DepartmentID = sVO[0].DepartmentID;
                            tpVO.Position = sVO[0].Position;
                            tpVO.isExternal = sVO[0].isExternal;
                            cBO.DeleteSecondBusinessById(sVO[0].PersonalID, sVO[0].BusinessID);
                        }
                        else
                        {
                            tpVO.BusinessID = 0;
                            tpVO.DepartmentID = 0;
                            tpVO.isExternal = false;
                        }
                        cBO.UpdatePersonal(tpVO);
                        pVO.QRimg = cBO.GetQRImgByHeadimg(pVO.PersonalID, AppType);
                    }
                    else
                    {
                        //删除绑定
                        cBO.DeleteSecondBusinessById(tpVO.PersonalID, pVO.BusinessID);
                    }

                    return new ResultObject() { Flag = 1, Message = "删除成功!", Result = null };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "您没有权限!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取通讯录
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getDirectories"), HttpGet]
        public ResultObject getDirectories(string token, int isExternal = 0)
        {
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            try
            {
                PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
                if (pVO != null)
                {
                    BusinessCardVO bVO = cBO.FindBusinessCardById(pVO.BusinessID);
                    if (bVO == null)
                    {
                        return new ResultObject() { Flag = 0, Message = "获取失败，您未绑定企业!", Result = null };
                    }
                    List<PersonalViewVO> Personal = cBO.FindPersonalViewByBusinessID(bVO.BusinessID, isExternal);

                    for (int i = 0; i < Personal.Count; i++)
                    {
                        try
                        {
                            Personal[i].Jurisdiction = cBO.FindJurisdictionView(Personal[i].PersonalID, bVO.BusinessID);
                        }
                        catch
                        {

                        }
                    }


                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = Personal };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
                }
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = ex };
            }

        }

        /// <summary>
        /// 获取我的团队
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getMyTeam"), HttpGet]
        public ResultObject getMyTeam(string token)
        {
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            try
            {
                PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
                if (pVO != null)
                {
                    BusinessCardVO bVO = cBO.FindBusinessCardById(pVO.BusinessID);
                    if (bVO == null)
                    {
                        return new ResultObject() { Flag = 0, Message = "获取失败，您未绑定企业!", Result = null };
                    }
                    List<PersonalVO> Personal = cBO.FindPersonalByPersonalID(bVO.BusinessID, pVO.PersonalID);
                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = Personal };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
                }
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = ex };
            }

        }

        /// <summary>
        ///  更改人员所属部门
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("ChangeDepartment"), HttpGet]
        public ResultObject ChangeDepartment(int PersonalID, int DepartmentID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            PersonalVO tpVO = cBO.FindPersonalById(PersonalID);
            if (pVO != null && tpVO != null)
            {
                if (cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Admin") || cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Personnel"))
                {
                    if (tpVO.BusinessID == pVO.BusinessID)
                    {
                        tpVO.DepartmentID = DepartmentID;
                        if (cBO.UpdatePersonal(tpVO))
                        {
                            return new ResultObject() { Flag = 1, Message = "更改成功!", Result = null };
                        }
                        else
                        {
                            return new ResultObject() { Flag = 0, Message = "更改失败!", Result = null };
                        }
                    }
                    else
                    {
                        List<SecondBusinessVO> sVO = cBO.FindSecondBusinessByPersonalID(tpVO.PersonalID, pVO.BusinessID);
                        if (sVO.Count > 0)
                        {
                            sVO[0].DepartmentID = DepartmentID;
                            if (cBO.UpdateSecondBusiness(sVO[0]))
                            {
                                return new ResultObject() { Flag = 1, Message = "更改成功!", Result = null };
                            }
                            else
                            {
                                return new ResultObject() { Flag = 0, Message = "更改失败!", Result = null };
                            }
                        }
                        else
                        {
                            return new ResultObject() { Flag = 0, Message = "更改失败!", Result = null };
                        }
                    }
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "您没有权限!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "更改失败!", Result = null };
            }
        }

        /// <summary>
        /// 添加权限
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("AddJurisdiction"), HttpGet]
        public ResultObject AddJurisdiction(int PersonalID, string Type, string token)
        {
            /*
            Type的值
            Admin 主管理员
            Web 官网修改权限
            Product 产品修改权限
            Clients 查看转移所有人的客户权限，默认只能查看转移自己与下属的客户
            Performance  查看所有人的业绩与合同权限，默认只能查看自己与下属的业绩
            Personnel  员工管理权限
            CloudCall 云呼权限
            */

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            if (pVO != null)
            {
                if (cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Admin") || cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Personnel"))
                {
                    if (Type != "Web" && Type != "Product" && Type != "Clients" && Type != "Performance" && Type != "Personnel" && Type != "Order" && Type != "CloudCall" && Type != "AddAgent")
                    {
                        return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
                    }
                    if (cBO.AddJurisdiction(PersonalID, pVO.BusinessID, Type) > 0)
                    {
                        return new ResultObject() { Flag = 1, Message = "添加权限成功!", Result = null };
                    }
                    else
                    {
                        return new ResultObject() { Flag = 0, Message = "添加权限失败!", Result = null };
                    }
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "您没有权限修改!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "添加权限失败!", Result = null };
            }
        }

        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DeleteJurisdiction"), HttpGet]
        public ResultObject DeleteJurisdiction(int PersonalID, string Type, string token)
        {
            /*
            Type的值
            Admin 主管理员
            Web 官网修改权限
            Product 产品修改权限
            Clients 查看转移所有人的客户权限，默认只能查看转移自己与下属的客户
            Performance  查看所有人的业绩与合同权限，默认只能查看自己与下属的业绩
            Personnel  员工管理权限
            */

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            if (pVO != null)
            {
                if (cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Admin") || cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Personnel"))
                {
                    if (Type != "Web" && Type != "Product" && Type != "Clients" && Type != "Performance" && Type != "Personnel" && Type != "Order" && Type != "CloudCall" && Type != "AddAgent")
                    {
                        return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
                    }

                    if (cBO.DeleteJurisdiction(PersonalID, pVO.BusinessID, Type) > 0)
                    {
                        return new ResultObject() { Flag = 1, Message = "删除权限成功!", Result = null };
                    }
                    else
                    {
                        return new ResultObject() { Flag = 0, Message = "删除权限失败!", Result = null };
                    }
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "您没有权限修改!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "删除权限失败!", Result = null };
            }
        }

        /// <summary>
        /// 移交管理员权限
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("toAdminJurisdiction"), HttpGet]
        public ResultObject toAdminJurisdiction(int PersonalID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            if (pVO != null)
            {
                if (cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Admin"))
                {
                    if (cBO.AddJurisdiction(PersonalID, pVO.BusinessID, "Admin") > 0)
                    {
                        cBO.DeleteJurisdiction(pVO.PersonalID, pVO.BusinessID, "Admin");
                        return new ResultObject() { Flag = 1, Message = "移交权限成功!", Result = null };
                    }
                    else
                    {
                        return new ResultObject() { Flag = 0, Message = "移交权限失败!", Result = null };
                    }
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "只有管理员可以移交权限!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "移交权限失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取权限
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("FindJurisdiction"), HttpGet]
        public ResultObject FindJurisdiction(string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            if (pVO != null)
            {
                /*
                Type的值
                Admin 主管理员
                Web 官网修改权限
                Product 产品修改权限
                Clients 查看转移所有人的客户权限，默认只能查看转移自己与下属的客户
                Performance  查看所有人的业绩与合同权限，默认只能查看自己与下属的业绩
                Personnel  员工管理权限
                */
                JurisdictionViewVO jVO = cBO.FindJurisdictionView(pVO.PersonalID, pVO.BusinessID);

                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = jVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取其他人权限
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("FindPersonalbyJurisdiction"), HttpGet]
        public ResultObject FindPersonalbyJurisdiction(int PersonalID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            PersonalViewVO tpVO = cBO.FindPersonalViewById(PersonalID);

            if (pVO != null && tpVO != null)
            {
                if (cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Admin") || cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Personnel"))
                {

                    JurisdictionViewVO jVO = cBO.FindJurisdictionView(tpVO.PersonalID, pVO.BusinessID);
                    List<SPLibrary.BusinessCardManagement.VO.DepartmentVO> dVO = cBO.FindDepartmentList(pVO.BusinessID);

                    if (tpVO.BusinessID != pVO.BusinessID)
                    {
                        List<SecondBusinessVO> sVO = cBO.FindSecondBusinessByPersonalID(tpVO.PersonalID, pVO.BusinessID);
                        if (sVO.Count > 0)
                        {
                            tpVO.DepartmentID = sVO[0].DepartmentID;
                        }
                    }

                    object res = new { Personal = tpVO, Jurisdiction = jVO, Department = dVO };
                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = res };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "您没有权限!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取管理页所需数据
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("FindSetUp"), HttpGet]
        public ResultObject FindSetUp(string token, int BusinessID = 0)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            CrmBO CrmBO = new CrmBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            if (pVO != null)
            {
                /*
                Type的值
                Admin 主管理员
                Web 官网修改权限
                Product 产品修改权限
                Clients 查看转移所有人的客户权限，默认只能查看转移自己与下属的客户
                Performance  查看所有人的业绩与合同权限，默认只能查看自己与下属的业绩
                Personnel  员工管理权限
                */

                int pBusinessID = pVO.BusinessID;

                if (BusinessID > 0)
                {
                    pBusinessID = BusinessID;
                }

                JurisdictionViewVO jVO = cBO.FindJurisdictionView(pVO.PersonalID, pBusinessID);
                List<InfoVO> banner = cBO.FindInfoByInfoID("Banner", pBusinessID);

                int ClientsUnread = CrmBO.FindCommentViewCount("Clients", pVO.PersonalID, pVO.BusinessID);
                int ClueUnread = CrmBO.FindCommentViewCount("Clue", pVO.PersonalID, pVO.BusinessID);
                int ChanceUnread = CrmBO.FindCommentViewCount("Chance", pVO.PersonalID, pVO.BusinessID);
                int ContractUnread = CrmBO.FindCommentViewCount("Contract", pVO.PersonalID, pVO.BusinessID);
                int GoOutUnread = CrmBO.FindCommentViewCount("GoOut", pVO.PersonalID, pVO.BusinessID);
                int DailyUnread = CrmBO.FindCommentViewCount("Daily", pVO.PersonalID, pVO.BusinessID);
                int WeeklyUnread = CrmBO.FindCommentViewCount("Weekly", pVO.PersonalID, pVO.BusinessID);
                int MonthlyUnread = CrmBO.FindCommentViewCount("Monthly", pVO.PersonalID, pVO.BusinessID);

                int qingjiaUnread = CrmBO.FindApprovalCount("qingjia", pVO.PersonalID, pVO.BusinessID) + CrmBO.FindCommentViewCount("qingjia", pVO.PersonalID, pVO.BusinessID);
                int baoxiaoUnread = CrmBO.FindApprovalCount("baoxiao", pVO.PersonalID, pVO.BusinessID) + CrmBO.FindCommentViewCount("baoxiao", pVO.PersonalID, pVO.BusinessID);
                int chuchaiUnread = CrmBO.FindApprovalCount("chuchai", pVO.PersonalID, pVO.BusinessID) + CrmBO.FindCommentViewCount("chuchai", pVO.PersonalID, pVO.BusinessID);
                int jiabanUnread = CrmBO.FindApprovalCount("jiaban", pVO.PersonalID, pVO.BusinessID) + CrmBO.FindCommentViewCount("jiaban", pVO.PersonalID, pVO.BusinessID);

                object UnreadCount = new
                {
                    ClientsUnread = ClientsUnread,
                    ClueUnread = ClueUnread,
                    ChanceUnread = ChanceUnread,
                    ContractUnread = ContractUnread,
                    GoOutUnread = GoOutUnread,
                    DailyUnread = DailyUnread,
                    WeeklyUnread = WeeklyUnread,
                    MonthlyUnread = MonthlyUnread,

                    qingjiaUnread = qingjiaUnread,
                    baoxiaoUnread = baoxiaoUnread,
                    chuchaiUnread = chuchaiUnread,
                    jiabanUnread = jiabanUnread,

                    Count = ClientsUnread + ClueUnread + ChanceUnread + ContractUnread + GoOutUnread + DailyUnread + WeeklyUnread + MonthlyUnread + qingjiaUnread + baoxiaoUnread + chuchaiUnread + jiabanUnread,
                };

                BusinessCard_JurisdictionVO B_Jurisdiction = cBO.getBusinessCard_Jurisdiction(pBusinessID);

                if (pVO.BusinessID != pBusinessID)
                {
                    B_Jurisdiction.isVip = false;
                }

                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = new { Jurisdiction = jVO, B_Jurisdiction = B_Jurisdiction, Banner = banner, UnreadCount = UnreadCount } };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取部门
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("FindDepartment"), HttpGet]
        public ResultObject FindDepartment(string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);

            List<SPLibrary.BusinessCardManagement.VO.DepartmentVO> dVO = cBO.FindDepartmentList(pVO.BusinessID);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = dVO };
        }

        /// <summary>
        /// 获取我管理的部门
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("FindDepartmentByDirectorPersonalID"), HttpGet]
        public ResultObject FindDepartmentByDirectorPersonalID(string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);

            List<SPLibrary.BusinessCardManagement.VO.DepartmentVO> dVO = cBO.FindDepartmentList(pVO.BusinessID, pVO.PersonalID);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = dVO };
        }

        /// <summary>
        /// 添加或更新部门
        /// </summary>
        /// <param name="VO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateDepartment"), HttpPost]
        public ResultObject UpdateDepartment([FromBody] SPLibrary.BusinessCardManagement.VO.DepartmentVO DepartmentVO, string token)
        {
            if (DepartmentVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);

            /*审核文本是否合法*/
            /*
            if (!cBO.msg_sec_check(DepartmentVO))
            {
                return new ResultObject() { Flag = 0, Message = "有非法关键词，请重新填写!", Result = null };
            }*/
            /*审核文本是否合法*/

            if (!cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Personnel") && !cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Admin"))
            {
                return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
            }

            if (DepartmentVO.DepartmentID > 0)
            {
                SPLibrary.BusinessCardManagement.VO.DepartmentVO sVO = cBO.FindDepartmentById(DepartmentVO.DepartmentID);
                DepartmentVO.BusinessID = sVO.BusinessID;
                if (cBO.UpdateDepartment(DepartmentVO))
                {
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = sVO.DepartmentID };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }
            }
            else
            {
                DepartmentVO.BusinessID = pVO.BusinessID;

                int DepartmentID = cBO.AddDepartment(DepartmentVO);
                if (DepartmentID > 0)
                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = DepartmentID };
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
        }

        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="VO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DelDepartment"), HttpGet]
        public ResultObject DelDepartment(int DepartmentID, string token)
        {
            if (DepartmentID <= 0)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);

            SPLibrary.BusinessCardManagement.VO.DepartmentVO sVO = cBO.FindDepartmentById(DepartmentID);
            if (sVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "该部门已被删除!", Result = null };
            }

            if (!cBO.FindJurisdiction(pVO.PersonalID, sVO.BusinessID, "Personnel") && !cBO.FindJurisdiction(pVO.PersonalID, sVO.BusinessID, "Admin"))
            {
                return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
            }

            List<PersonalVO> LpVO = cBO.FindPersonalByDepartmentID(DepartmentID);

            for (int i = 0; i < LpVO.Count; i++)
            {
                LpVO[i].DepartmentID = 0;
                try
                {
                    cBO.UpdatePersonal(LpVO[i]);
                }
                catch
                {

                }
            }

            if (cBO.DeleteDepartmentById(DepartmentID) > 0)
            {
                return new ResultObject() { Flag = 1, Message = "删除成功!", Result = sVO.DepartmentID };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
            }
        }

        /// <summary>
        /// 添加或更新Crm信息
        /// </summary>
        /// <param name="VO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateCrmSite"), HttpPost]
        public ResultObject UpdateCrmSite([FromBody] CrmVO CrmVO, string token)
        {
            if (CrmVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO bBO = new BusinessCardBO(new CustomerProfile());
            CrmBO cBO = new CrmBO(new CustomerProfile());

            PersonalVO pVO = bBO.FindPersonalByCustomerId(customerId);

            /*审核文本是否合法*/
            /*
            if (!bBO.msg_sec_check(CrmVO))
            {
                return new ResultObject() { Flag = 0, Message = "有非法关键词，请重新填写!", Result = null };
            }*/
            /*审核文本是否合法*/

            if (CrmVO.CrmID > 0)
            {
                //是否有修改权限
                if (!cBO.EntitledToEdit(CrmVO.CrmID, pVO.PersonalID))
                {
                    return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
                }

                CrmVO sVO = cBO.FindCrmById(CrmVO.CrmID);
                CrmVO.PersonalID = pVO.PersonalID;
                CrmVO.CreatedAt = sVO.CreatedAt;
                CrmVO.BusinessID = sVO.BusinessID;

                if (CrmVO.Type != "Daily" || CrmVO.Type != "Weekly" || CrmVO.Type != "Monthly" || CrmVO.Type != "qingjia" || CrmVO.Type != "baoxiao" || CrmVO.Type != "chuchai" || CrmVO.Type != "jiaban")
                {
                    CrmVO.ForPersonalID = null;
                }

                if (cBO.UpdateCrm(CrmVO))
                {
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = sVO.CrmID };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }
            }
            else
            {
                CrmVO.CreatedAt = DateTime.Now;
                CrmVO.PersonalID = pVO.PersonalID;
                CrmVO.BusinessID = pVO.BusinessID;

                int CrmID = cBO.AddCrm(CrmVO);
                if (CrmID > 0)
                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = CrmID };
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
        }

        /// <summary>
        /// 改变销售阶段
        /// </summary>
        /// <param name="VO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateCrmSitebyField4"), HttpGet]
        public ResultObject UpdateCrmSitebyField4(int CrmID, string Field4, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO bBO = new BusinessCardBO(new CustomerProfile());
            CrmBO cBO = new CrmBO(new CustomerProfile());

            PersonalVO pVO = bBO.FindPersonalByCustomerId(customerId);
            if (CrmID > 0)
            {
                //是否有修改权限
                if (!cBO.EntitledToEdit(CrmID, pVO.PersonalID))
                {
                    return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
                }

                CrmVO sVO = new CrmVO();
                sVO.CrmID = CrmID;
                sVO.Field4 = Field4;

                if (cBO.UpdateCrm(sVO))
                {
                    return new ResultObject() { Flag = 1, Message = "修改成功!", Result = sVO.CrmID };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "修改失败!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };

            }
        }

        /// <summary>
        /// 删除Crm信息
        /// </summary>
        /// <param name="VO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("delCrm"), HttpGet]
        public ResultObject delCrm(int CrmID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO bBO = new BusinessCardBO(new CustomerProfile());
            CrmBO cBO = new CrmBO(new CustomerProfile());

            PersonalVO pVO = bBO.FindPersonalByCustomerId(customerId);
            CrmVO CrmVO = cBO.FindCrmById(CrmID);

            if (CrmVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
            }
            /*
            if (CrmVO.Type == "Contract" && !cBO.EntitledToContractEdit(CrmVO.CrmID, pVO.PersonalID))
            {
                return new ResultObject() { Flag = 0, Message = "您没有删除合同的权限!", Result = null };
            }*/

            //是否有修改权限
            if (!cBO.EntitledToEdit(CrmVO.CrmID, pVO.PersonalID))
            {
                return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
            }

            CrmVO.Status = 0;

            if (cBO.UpdateCrm(CrmVO))
            {
                return new ResultObject() { Flag = 1, Message = "删除成功!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
            }
        }

        /// <summary>
        /// 批量删除Crm信息
        /// </summary>
        /// <param name="VO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("BatchdelCrm"), HttpGet]
        public ResultObject BatchdelCrm(string CrmID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO bBO = new BusinessCardBO(new CustomerProfile());
            CrmBO cBO = new CrmBO(new CustomerProfile());

            PersonalVO pVO = bBO.FindPersonalByCustomerId(customerId);
            try
            {
                if (!string.IsNullOrEmpty(CrmID))
                {
                    string[] CrmIDIdArr = CrmID.Split(',');
                    bool isAllDelete = true;
                    CustomerBO uBO = new CustomerBO((CustomerProfile)CacheManager.GetUserProfile(token));
                    for (int i = 0; i < CrmIDIdArr.Length; i++)
                    {
                        try
                        {
                            CrmVO CrmVO = cBO.FindCrmById(Convert.ToInt32(CrmIDIdArr[i]));
                            if (CrmVO == null)
                            {
                                isAllDelete = false;
                                continue;
                            }
                            //是否有修改权限
                            if (!cBO.EntitledToEdit(CrmVO.CrmID, pVO.PersonalID))
                            {
                                isAllDelete = false;
                                continue;
                            }
                            CrmVO.Status = 0;
                            if (!cBO.UpdateCrm(CrmVO))
                            {
                                isAllDelete = false;
                            }
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
        /// 将Crm信息丢入公海
        /// </summary>
        /// <param name="VO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("PublicCrm"), HttpGet]
        public ResultObject PublicCrm(int CrmID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO bBO = new BusinessCardBO(new CustomerProfile());
            CrmBO cBO = new CrmBO(new CustomerProfile());

            PersonalVO pVO = bBO.FindPersonalByCustomerId(customerId);
            CrmVO CrmVO = cBO.FindCrmById(CrmID);

            if (CrmVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "操作失败!", Result = null };
            }

            //是否有修改权限
            if (!cBO.EntitledToEdit(CrmVO.CrmID, pVO.PersonalID))
            {
                return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
            }

            CrmVO.PersonalID = 0;

            if (cBO.UpdateCrm(CrmVO))
            {
                return new ResultObject() { Flag = 1, Message = "丢入公海成功!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "操作失败!", Result = null };
            }
        }

        /// <summary>
        /// 批量将Crm信息丢入公海
        /// </summary>
        /// <param name="VO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("BatchPublicCrm"), HttpGet]
        public ResultObject BatchPublicCrm(string CrmID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO bBO = new BusinessCardBO(new CustomerProfile());
            CrmBO cBO = new CrmBO(new CustomerProfile());

            PersonalVO pVO = bBO.FindPersonalByCustomerId(customerId);
            try
            {
                if (!string.IsNullOrEmpty(CrmID))
                {
                    string[] CrmIDIdArr = CrmID.Split(',');
                    bool isAllDelete = true;
                    CustomerBO uBO = new CustomerBO((CustomerProfile)CacheManager.GetUserProfile(token));
                    for (int i = 0; i < CrmIDIdArr.Length; i++)
                    {
                        try
                        {
                            CrmVO CrmVO = cBO.FindCrmById(Convert.ToInt32(CrmIDIdArr[i]));
                            if (CrmVO == null)
                            {
                                isAllDelete = false;
                                continue;
                            }
                            //是否有修改权限
                            if (!cBO.EntitledToEdit(CrmVO.CrmID, pVO.PersonalID))
                            {
                                isAllDelete = false;
                                continue;
                            }
                            CrmVO.PersonalID = 0;
                            if (!cBO.UpdateCrm(CrmVO))
                            {
                                isAllDelete = false;
                            }
                        }
                        catch
                        {
                            isAllDelete = false;
                        }
                    }
                    if (isAllDelete)
                    {
                        return new ResultObject() { Flag = 1, Message = "丢入公海成功!", Result = null };
                    }
                    else
                    {
                        return new ResultObject() { Flag = 1, Message = "部分操作成功!", Result = null };
                    }

                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
                }
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "操作失败!", Result = null };
            }
        }

        /// <summary>
        /// 批量从公海领取Crm信息
        /// </summary>
        /// <param name="VO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("BatchGetPublicCrm"), HttpGet]
        public ResultObject BatchGetPublicCrm(string CrmID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO bBO = new BusinessCardBO(new CustomerProfile());
            CrmBO cBO = new CrmBO(new CustomerProfile());

            PersonalVO pVO = bBO.FindPersonalByCustomerId(customerId);
            try
            {
                if (!string.IsNullOrEmpty(CrmID))
                {
                    string[] CrmIDIdArr = CrmID.Split(',');
                    bool isAllDelete = true;
                    CustomerBO uBO = new CustomerBO((CustomerProfile)CacheManager.GetUserProfile(token));
                    for (int i = 0; i < CrmIDIdArr.Length; i++)
                    {
                        try
                        {
                            CrmVO CrmVO = cBO.FindCrmById(Convert.ToInt32(CrmIDIdArr[i]));
                            if (CrmVO == null)
                            {
                                isAllDelete = false;
                                continue;
                            }
                            //是否有修改权限
                            if (CrmVO.BusinessID != pVO.BusinessID)
                            {
                                isAllDelete = false;
                                continue;
                            }
                            CrmVO.PersonalID = pVO.PersonalID;
                            if (!cBO.UpdateCrm(CrmVO))
                            {
                                isAllDelete = false;
                            }
                        }
                        catch
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
                        return new ResultObject() { Flag = 1, Message = "部分操作成功!", Result = null };
                    }

                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
                }
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "操作失败!", Result = null };
            }
        }

        /// <summary>
        /// 从公海领取Crm信息
        /// </summary>
        /// <param name="VO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetPublicCrm"), HttpGet]
        public ResultObject GetPublicCrm(int CrmID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO bBO = new BusinessCardBO(new CustomerProfile());
            CrmBO cBO = new CrmBO(new CustomerProfile());

            PersonalVO pVO = bBO.FindPersonalByCustomerId(customerId);
            CrmVO CrmVO = cBO.FindCrmById(CrmID);

            if (CrmVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "操作失败!", Result = null };
            }

            //是否有修改权限
            if (CrmVO.BusinessID != pVO.BusinessID)
            {
                return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
            }

            CrmVO.PersonalID = pVO.PersonalID;

            if (cBO.UpdateCrm(CrmVO))
            {
                return new ResultObject() { Flag = 1, Message = "领取成功!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "操作失败!", Result = null };
            }
        }

        /// <summary>
        /// 移交Crm信息
        /// </summary>
        /// <param name="VO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("TransferCrm"), HttpGet]
        public ResultObject TransferCrm(int CrmID, int PersonalID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO bBO = new BusinessCardBO(new CustomerProfile());
            CrmBO cBO = new CrmBO(new CustomerProfile());

            PersonalVO pVO = bBO.FindPersonalByCustomerId(customerId);
            CrmVO CrmVO = cBO.FindCrmById(CrmID);

            if (CrmVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "移交失败!", Result = null };
            }
            /*
            if (CrmVO.Type == "Contract" && !cBO.EntitledToContractEdit(CrmVO.CrmID, pVO.PersonalID))
            {
                return new ResultObject() { Flag = 0, Message = "您没有修改合同的权限!", Result = null };
            }*/

            //是否有修改权限
            if (!cBO.EntitledToEdit(CrmVO.CrmID, pVO.PersonalID))
            {
                return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
            }

            CrmVO.PersonalID = PersonalID;

            if (cBO.UpdateCrm(CrmVO))
            {
                return new ResultObject() { Flag = 1, Message = "移交成功，点确定返回列表!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "移交失败!", Result = null };
            }
        }

        /// <summary>
        /// 批量移交Crm信息
        /// </summary>
        /// <param name="VO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("BatchTransferCrm"), HttpGet]
        public ResultObject BatchTransferCrm(string CrmID, int PersonalID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO bBO = new BusinessCardBO(new CustomerProfile());
            CrmBO cBO = new CrmBO(new CustomerProfile());

            PersonalVO pVO = bBO.FindPersonalByCustomerId(customerId);

            try
            {
                if (!string.IsNullOrEmpty(CrmID))
                {
                    string[] CrmIDIdArr = CrmID.Split(',');
                    bool isAllDelete = true;
                    CustomerBO uBO = new CustomerBO((CustomerProfile)CacheManager.GetUserProfile(token));
                    for (int i = 0; i < CrmIDIdArr.Length; i++)
                    {
                        try
                        {
                            CrmVO CrmVO = cBO.FindCrmById(Convert.ToInt32(CrmIDIdArr[i]));
                            if (CrmVO == null)
                            {
                                isAllDelete = false;
                                continue;
                            }
                            //是否有修改权限
                            if (!cBO.EntitledToEdit(CrmVO.CrmID, pVO.PersonalID))
                            {
                                isAllDelete = false;
                                continue;
                            }
                            CrmVO.PersonalID = PersonalID;
                            if (!cBO.UpdateCrm(CrmVO))
                            {
                                isAllDelete = false;
                            }
                        }
                        catch
                        {
                            isAllDelete = false;
                        }
                    }
                    if (isAllDelete)
                    {
                        return new ResultObject() { Flag = 1, Message = "移交成功，点确定返回列表!", Result = null };
                    }
                    else
                    {
                        return new ResultObject() { Flag = 1, Message = "部分移交成功!", Result = null };
                    }

                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
                }
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "移交失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取Crm信息详情
        /// </summary>
        /// <param name="SortID"></param>
        /// <returns></returns>
        [Route("GetCrmSite"), HttpGet]
        public ResultObject GetCrmSite(int CrmID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO bBO = new BusinessCardBO(new CustomerProfile());
            CrmBO cBO = new CrmBO(new CustomerProfile());
            CrmVO sVO = cBO.FindCrmById(CrmID);
            PersonalVO pVO = bBO.FindPersonalByCustomerId(customerId);
            List<PersonalVO> ForPersonal = new List<PersonalVO>();

            if (sVO != null && pVO != null)
            {
                //是否有查看权限
                if (!cBO.EntitledToEdit(CrmID, pVO.PersonalID))
                {
                    return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
                }

                if (sVO.ForPersonalID != "")
                {
                    string[] PersonalIDIdArr = sVO.ForPersonalID.Split(',');
                    for (int i = 0; i < PersonalIDIdArr.Length; i++)
                    {
                        try
                        {

                            PersonalVO PersonalVO = bBO.FindPersonalById(Convert.ToInt32(PersonalIDIdArr[i].Replace("#", "")));
                            if (PersonalVO != null)
                            {
                                ForPersonal.Add(PersonalVO);
                            }
                        }
                        catch
                        {

                        }
                    }
                }

                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = sVO, Subsidiary = ForPersonal };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取Crm信息详情(视图)
        /// </summary>
        /// <param name="SortID"></param>
        /// <returns></returns>
        [Route("GetCrmViewSite"), HttpGet]
        public ResultObject GetCrmViewSite(int CrmID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO bBO = new BusinessCardBO(new CustomerProfile());
            CrmBO cBO = new CrmBO(new CustomerProfile());
            CrmViewVO sVO = cBO.FindCrmViewById(CrmID);
            PersonalVO pVO = bBO.FindPersonalByCustomerId(customerId);
            List<PersonalVO> ForPersonal = new List<PersonalVO>();

            if (sVO != null && pVO != null)
            {
                //是否有查看权限
                List<InfoViewVO> CommentList = bBO.FindInfoViewByInfoID(sVO.CrmID, 30);

                //如果是本人就把评论设置为已读
                bBO.UpdateInfo("Status=2", "CrmID=" + sVO.CrmID + " and Status=1 and ToPersonalID=" + pVO.PersonalID);

                sVO.isApproval = cBO.GetIsApproval(sVO, pVO.PersonalID);


                if (sVO.ForPersonalID != "")
                {
                    string[] PersonalIDIdArr = sVO.ForPersonalID.Split(',');
                    for (int i = 0; i < PersonalIDIdArr.Length; i++)
                    {
                        try
                        {

                            PersonalVO PersonalVO = bBO.FindPersonalById(Convert.ToInt32(PersonalIDIdArr[i].Replace("#", "")));
                            if (PersonalVO != null)
                            {
                                PersonalVO.isApproval = cBO.GetIsApproval(sVO, PersonalVO.PersonalID);
                                ForPersonal.Add(PersonalVO);
                            }
                        }
                        catch
                        {

                        }
                    }
                }

                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = sVO, Subsidiary = CommentList, Subsidiary2 = ForPersonal };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取CRM信息列表
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        [Route("GetCrmViewList"), HttpPost]
        public ResultObject GetCrmViewList([FromBody] ConditionModel condition, int ListDisplay, string Type, string token)
        {
            if (condition == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            else if (condition.Filter == null || condition.PageInfo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            if (Type != "Clue" && Type != "Chance" && Type != "Clients" && Type != "Contract" && Type != "GoOut" && Type != "Daily" && Type != "Weekly" && Type != "Monthly" && Type != "qingjia" && Type != "baoxiao" && Type != "chuchai" && Type != "jiaban")
            {
                return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
            }

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO bBO = new BusinessCardBO(new CustomerProfile());
            CrmBO cBO = new CrmBO(new CustomerProfile());
            PersonalVO pVO = bBO.FindPersonalByCustomerId(customerId);
            List<SPLibrary.BusinessCardManagement.VO.DepartmentVO> dVO = bBO.FindDepartmentList(pVO.BusinessID, pVO.PersonalID);

            string conditionStr = "Status = 1 and BusinessID = " + pVO.BusinessID + " and PersonalID = " + pVO.PersonalID + " and Type='" + Type + "' and (" + condition.Filter.Result() + ")";

            //ListDisplay  0:显示全部，1：显示自己,2:显示公海数据
            if (ListDisplay == 0)
            {
                //如果是部门主管，则可以看到下属的Crm信息
                string sqlstr = " and (ForPersonalID is not NULL or ";
                if (dVO.Count > 0)
                {
                    List<PersonalVO> ListVO = new List<PersonalVO>();
                    for (int i = 0; i < dVO.Count; i++)
                    {
                        List<PersonalVO> pListVO = bBO.FindPersonalByDepartmentID(dVO[i].DepartmentID);
                        ListVO.AddRange(pListVO);
                    }
                    if (ListVO.Count > 0)
                    {

                        for (int i = 0; i < ListVO.Count; i++)
                        {
                            sqlstr += "PersonalID=" + ListVO[i].PersonalID;
                            sqlstr += " or ";
                        }
                    }
                    sqlstr += "PersonalID = " + pVO.PersonalID + ")";
                }
                else
                {
                    sqlstr = "";
                }

                //超级管理员权限
                bool isAdmin = bBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Admin");

                //查看所有CRM信息权限
                bool isClients = bBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Clients");

                //查看所有合同信息权限
                bool isPerformance = bBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Performance");

                //是否属于除合同外其他CRM
                bool isTypeClient = Type == "Clue" || Type == "Chance" || Type == "Clients" || Type == "GoOut" || Type == "Daily" || Type == "Weekly" || Type == "Monthly" || Type == "qingjia" || Type == "baoxiao" || Type == "chuchai" || Type == "jiaban";

                //是否属于合同
                bool isTypePerformance = Type == "Contract";

                //拥有相应权限的，解除人员ID限制
                if (isAdmin || (isClients && isTypeClient) || (isPerformance && isTypePerformance))
                {
                    sqlstr = "";
                }
                //既没有权限也不是部长，就只能看到自己的信息
                else if (sqlstr == "")
                {
                    sqlstr = " and (ForPersonalID is not NULL or PersonalID = " + pVO.PersonalID + " or " + pVO.PersonalID + " = 3)";
                }

                string sql = "";
                //如果有指定汇报的人，就只有指定的人才能看得到
                if (Type == "Daily" || Type == "Weekly" || Type == "Monthly" || Type == "qingjia" || Type == "baoxiao" || Type == "chuchai" || Type == "jiaban")
                {
                    sql = " and (ForPersonalID is NULL or ForPersonalID Like '%#" + pVO.PersonalID + "#%' or PersonalID = " + pVO.PersonalID + " or " + pVO.PersonalID + " = 3)";
                }
                conditionStr = "BusinessID = " + pVO.BusinessID + " and Type='" + Type + "'" + sqlstr + " and (" + condition.Filter.Result() + ") and PersonalID <> 0" + sql;
            }
            else if (ListDisplay == 1)
            {
                conditionStr = "Status = 1 and BusinessID = " + pVO.BusinessID + " and PersonalID = " + pVO.PersonalID + " and Type='" + Type + "' and (" + condition.Filter.Result() + ")";
            }
            else if (ListDisplay == 2)
            {
                conditionStr = "Status = 1 and BusinessID = " + pVO.BusinessID + " and PersonalID = 0  and Type='" + Type + "' and (" + condition.Filter.Result() + ")";
            }

            Paging pageInfo = condition.PageInfo;
            List<CrmViewVO> list = cBO.FindCrmViewAllByPageIndex(conditionStr, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);

            for (int i = 0; i < list.Count; i++)
            {
                //获取未读评论数量
                list[i].UnreadCount = cBO.FindCommentViewCountByCrmID(list[i].CrmID, pVO.PersonalID);
                list[i].isApproval = cBO.GetIsApproval(list[i], pVO.PersonalID);
                list[i].isAllApproval = cBO.GetIsApproval(list[i]);
            }

            int count = cBO.FindCrmViewCount(conditionStr);

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = new { list = list, count = count } };
        }

        /// <summary>
        /// 获取销售资源库数量
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        [Route("GetCrmStorehouse"), HttpGet]
        public ResultObject GetCrmStorehouse(string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO bBO = new BusinessCardBO(new CustomerProfile());
            CrmBO cBO = new CrmBO(new CustomerProfile());
            PersonalVO pVO = bBO.FindPersonalByCustomerId(customerId);

            CardBO CardBO = new CardBO(new CustomerProfile());

            //获取活动报名数量
            int party = 0;
            List<CardPartyViewVO> uVO = CardBO.FindPartyByCustomerIdOrContactsCustomerId(customerId);
            for (int i = 0; i < uVO.Count; i++)
            {
                party += uVO[i].SignupCount;
            }

            //获取名片组名片数量
            int Group = 0;
            List<CardGroupViewVO> gVO = CardBO.FindCardGroupViewByCustomerId(customerId);
            for (int i = 0; i < gVO.Count; i++)
            {
                Group += gVO[i].NumberOfPeople;
            }

            //获取名片夹数量
            int collection = 0;
            string conditionStr = "t_CustomerId = " + customerId;
            collection = CardBO.FindCardCollectionAllCount(conditionStr);

            //获取访客数量
            int accessrecords = 0;
            accessrecords = bBO.FindNumberOfVisitors(pVO.PersonalID, 1);

            //获取公海数据数量
            int PublicCrm = 0;
            PublicCrm = cBO.FindCrmViewCount("PersonalID=0 and BusinessID=" + pVO.BusinessID);


            object CrmStorehouse = new
            {
                group = Group,
                party = party,
                collection = collection,
                accessrecords = accessrecords,
                PublicCrm = PublicCrm
            };

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = CrmStorehouse };
        }

        /// <summary>
        /// 处理审批信息
        /// </summary>
        /// <param name="VO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("SetApproval"), HttpGet]
        public ResultObject SetApproval(int CrmID, int isApproval, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO bBO = new BusinessCardBO(new CustomerProfile());
            CrmBO cBO = new CrmBO(new CustomerProfile());

            PersonalVO pVO = bBO.FindPersonalByCustomerId(customerId);
            CrmVO CrmVO = cBO.FindCrmById(CrmID);

            if (CrmVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
            }
            /*
            if (CrmVO.Type == "Contract" && !cBO.EntitledToContractEdit(CrmVO.CrmID, pVO.PersonalID))
            {
                return new ResultObject() { Flag = 0, Message = "您没有修改合同的权限!", Result = null };
            }*/

            //是否有修改权限
            CrmViewVO CrmViewVO = cBO.FindCrmViewById(CrmID);
            int IsA = cBO.GetIsApproval(CrmViewVO, pVO.PersonalID);
            if (IsA == 3)
            {
                return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
            }
            if (IsA == 1 || IsA == 2)
            {
                return new ResultObject() { Flag = 0, Message = "您已处理过该申请!", Result = null };
            }

            if (isApproval == 1)
            {
                if (CrmVO.ApprovalPersonalID != "")
                {
                    CrmVO.ApprovalPersonalID += ",#" + pVO.PersonalID + "#";
                }
                else
                {
                    CrmVO.ApprovalPersonalID += "#" + pVO.PersonalID + "#";
                }
            }
            if (isApproval == 2)
            {
                if (CrmVO.DisapprovalPersonalID != "")
                {
                    CrmVO.DisapprovalPersonalID += ",#" + pVO.PersonalID + "#";
                }
                else
                {
                    CrmVO.DisapprovalPersonalID += "#" + pVO.PersonalID + "#";
                }
            }

            if (cBO.UpdateCrm(CrmVO))
            {
                return new ResultObject() { Flag = 1, Message = "操作成功，点确定返回列表!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "操作失败!", Result = null };
            }
        }


        /// <summary>
        /// 添加访问记录
        /// </summary>
        /// <returns></returns>
        [Route("AddAccessrecords"), HttpGet]
        public ResultObject AddAccessrecords(string Type, int ById, int ToPersonalID, string token, int AppType = 0)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            AppVO AppVO = AppBO.GetApp(AppType);
            BusinessCardBO bBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = bBO.FindPersonalByCustomerId(customerId);

            AccessrecordsVO aVO = new AccessrecordsVO();
            aVO.AccessRecordsID = 0;
            aVO.ById = ById;
            aVO.Type = Type;
            aVO.PersonalID = pVO.PersonalID;
            aVO.ToPersonalID = ToPersonalID;
            aVO.AccessAt = DateTime.Now;
            aVO.Counts = bBO.FindAccessrecordsViewCount("PersonalID=" + pVO.PersonalID + "  and ToPersonalID=" + ToPersonalID) + 1;
            aVO.ReturnCounts = bBO.FindAccessrecordsViewCount("PersonalID=" + pVO.PersonalID + "  and ToPersonalID=" + ToPersonalID + " and Type='ReturnCard'");

            //如果有订阅消息，则发送通知
            PersonalVO toPVO = bBO.FindPersonalById(ToPersonalID);
            string details = "访问了您的名片";

            if (Type == "ReturnCard")
            {
                aVO.ReturnCounts += 1;
                details = "给您回递了名片";
            }
            else if (Type == "Personal")
            {
                details = "访问了您的名片";
            }
            else if (Type == "Product" || Type == "News" || Type == "Video" || Type == "Info")
            {
                InfoVO iVO = bBO.FindInfoById(ById);
                if (iVO != null)
                {
                    iVO.ReadCount += 1;
                    bBO.UpdateInfo(iVO);
                    details = "访问了《" + iVO.Title + "》";
                }
            }
            else if (Type == "GreetingCard")
            {
                GreetingCardVO gVO = bBO.FindGreetingCardById(ById);
                if (gVO != null)
                {
                    details = "转发了短视频《" + gVO.Title + "》";
                }
            }
            else if (Type == "BrowseGreetingCard")
            {
                GreetingCardVO gVO = bBO.FindGreetingCardById(ById);
                if (gVO != null)
                {
                    details = "浏览了短视频《" + gVO.Title + "》";
                }
            }
            else if (Type == "ColorPage")
            {
                details = "访问了您的电子彩页";
            }
            if (toPVO.PersonalID != pVO.PersonalID)
            {
                bBO.sendTemplateMessage(toPVO.CustomerId, pVO.Name, details, AppType);
            }
            int AccessRecordsID = bBO.AddAccessrecords(aVO);
            if (AccessRecordsID > 0)
                return new ResultObject() { Flag = 1, Message = "添加成功!", Result = AccessRecordsID };
            else
                return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
        }

        /// <summary>
        /// 添加访问记录(活动专属)
        /// </summary>
        /// <returns></returns>
        [Route("AddPartyAccessrecords"), HttpGet]
        public ResultObject AddPartyAccessrecords(string Type, int ById, int ToCustomerId, string token, int AppType = 0)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            AppVO AppVO = AppBO.GetApp(AppType);
            BusinessCardBO bBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = bBO.FindPersonalByCustomerId(customerId);
            PersonalVO toPVO = new PersonalVO();
            if (ToCustomerId != 0)
            {
                toPVO = bBO.FindPersonalByCustomerId(ToCustomerId);
            }
            AccessrecordsVO aVO = new AccessrecordsVO();
            aVO.AccessRecordsID = 0;
            aVO.ById = ById;
            aVO.Type = Type;
            aVO.PersonalID = pVO.PersonalID;
            aVO.ToPersonalID = toPVO.PersonalID;
            aVO.AccessAt = DateTime.Now;
            aVO.Counts = bBO.FindAccessrecordsViewCount("PersonalID=" + pVO.PersonalID + "  and ToPersonalID=" + toPVO.PersonalID) + 1;
            aVO.ReturnCounts = bBO.FindAccessrecordsViewCount("PersonalID=" + pVO.PersonalID + "  and ToPersonalID=" + toPVO.PersonalID + " and Type='ReturnCard'");

            int AccessRecordsID = bBO.AddAccessrecords(aVO);
            if (AccessRecordsID > 0)
                return new ResultObject() { Flag = 1, Message = "添加成功!", Result = AccessRecordsID };
            else
                return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
        }

        /// <summary>
        /// 添加访问记录（匿名）
        /// </summary>
        /// <returns></returns>
        [Route("AddAccessrecords"), HttpGet, Anonymous]
        public ResultObject AddAccessrecords(string Type, int ById, int ToPersonalID, int AppType = 0)
        {

            BusinessCardBO bBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = bBO.FindPersonalById(1557);
            AppVO AppVO = AppBO.GetApp(AppType);
            AccessrecordsVO aVO = new AccessrecordsVO();
            aVO.AccessRecordsID = 0;
            aVO.ById = ById;
            aVO.Type = Type;
            aVO.PersonalID = pVO.PersonalID;
            aVO.ToPersonalID = ToPersonalID;
            aVO.AccessAt = DateTime.Now;
            aVO.Counts = bBO.FindAccessrecordsViewCount("PersonalID=" + pVO.PersonalID + "  and ToPersonalID=" + ToPersonalID) + 1;
            aVO.ReturnCounts = bBO.FindAccessrecordsViewCount("PersonalID=" + pVO.PersonalID + "  and ToPersonalID=" + ToPersonalID + " and Type='ReturnCard'");

            //如果有订阅消息，则发送通知
            PersonalVO toPVO = bBO.FindPersonalById(ToPersonalID);
            string details = "访问了您的名片";

            if (Type == "ReturnCard")
            {
                aVO.ReturnCounts += 1;
                details = "给您回递了名片";
            }
            else if (Type == "Personal")
            {
                details = "访问了您的名片";
            }
            else if (Type == "Product" || Type == "News" || Type == "Video" || Type == "Info")
            {
                InfoVO iVO = bBO.FindInfoById(ById);
                if (iVO != null)
                {
                    iVO.ReadCount += 1;
                    bBO.UpdateInfo(iVO);
                    details = "访问了《" + iVO.Title + "》";
                }
            }
            else if (Type == "GreetingCard")
            {
                GreetingCardVO gVO = bBO.FindGreetingCardById(ById);
                if (gVO != null)
                {
                    details = "转发了短视频《" + gVO.Title + "》";
                }
            }
            else if (Type == "BrowseGreetingCard")
            {
                GreetingCardVO gVO = bBO.FindGreetingCardById(ById);
                if (gVO != null)
                {
                    details = "浏览了短视频《" + gVO.Title + "》";
                }
            }
            else if (Type == "ColorPage")
            {
                details = "访问了您的电子彩页";
            }
            if (toPVO.PersonalID != pVO.PersonalID)
            {
                bBO.sendTemplateMessage(toPVO.CustomerId, pVO.Name, details, AppType);
            }
            int AccessRecordsID = bBO.AddAccessrecords(aVO);
            if (AccessRecordsID > 0)
                return new ResultObject() { Flag = 1, Message = "添加成功!", Result = AccessRecordsID };
            else
                return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
        }

        /// <summary>
        /// 删除访问记录
        /// </summary>
        /// <returns></returns>
        [Route("DelAccessrecords"), HttpGet]
        public ResultObject DelAccessrecords(int ToPersonalID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO bBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = bBO.FindPersonalByCustomerId(customerId);

            if (bBO.DeleteAccessrecords(ToPersonalID, pVO.PersonalID) > 0)
                return new ResultObject() { Flag = 1, Message = "删除成功!", Result = null };
            else
                return new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
        }

        /// <summary>
        /// 修改访问记录停留时间
        /// </summary>
        /// <returns></returns>
        [Route("EditAccessrecords"), HttpGet, Anonymous]
        public ResultObject EditAccessrecords(int AccessRecordsID, int ResidenceAt)
        {
            BusinessCardBO bBO = new BusinessCardBO(new CustomerProfile());
            AccessrecordsVO aVO = bBO.FindAccessrecordsByAccessrecordsID(AccessRecordsID);
            if (aVO != null)
            {
                aVO.ResidenceAt = ResidenceAt;
                bBO.UpdateAccessrecords(aVO);
                return new ResultObject() { Flag = 1, Message = "保存停留时间成功!", Result = AccessRecordsID };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "保存停留时间失败!", Result = AccessRecordsID };
            }
        }


        /// <summary>
        /// 获取情报厅数据
        /// </summary>
        /// <returns></returns>
        [Route("GetStatisticsData"), HttpGet]
        public ResultObject GetStatisticsData(string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO bBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = bBO.FindPersonalByCustomerId(customerId);

            List<StatisticsList> Statistics30days = new List<StatisticsList>();//30天内访问人数记录
            List<StatisticsList> Statistics30daysOfVisits = new List<StatisticsList>();//30天内访问次数记录

            for (int i = 0; i < 30; i++)
            {
                StatisticsList sl = new StatisticsList();
                DateTime Time = DateTime.Now.AddDays(-i);

                sl.Time = Time;

                sl.Personal = bBO.FindNumberOfVisitors("Personal", pVO.PersonalID, Time, 1); //名片
                sl.Product = bBO.FindNumberOfVisitors("Product", pVO.PersonalID, Time, 1);  //产品
                sl.News = bBO.FindNumberOfVisitors("News", pVO.PersonalID, Time, 1);   //新闻
                sl.Total = bBO.FindNumberOfVisitors(pVO.PersonalID, Time, 1);    //总计

                Statistics30days.Add(sl);
            }

            for (int i = 0; i < 30; i++)
            {
                StatisticsList sl = new StatisticsList();
                DateTime Time = DateTime.Now.AddDays(-i);

                sl.Time = Time;

                sl.Personal = bBO.FindNumberOfVisits("Personal", pVO.PersonalID, Time, 1); //名片
                sl.Product = bBO.FindNumberOfVisits("Product", pVO.PersonalID, Time, 1);  //产品
                sl.News = bBO.FindNumberOfVisits("News", pVO.PersonalID, Time, 1);   //新闻
                sl.Total = bBO.FindNumberOfVisits(pVO.PersonalID, Time, 1);    //总计

                Statistics30daysOfVisits.Add(sl);
            }


            object Statistics = new
            {
                Visitors = new //访问人数
                {
                    Total = new //总数量
                    {
                        Personal = bBO.FindNumberOfVisitors("Personal", pVO.PersonalID, 1), //名片
                        Product = bBO.FindNumberOfVisitors("Product", pVO.PersonalID, 1),  //产品
                        News = bBO.FindNumberOfVisitors("News", pVO.PersonalID, 1),    //新闻
                        Total = bBO.FindNumberOfVisitors(pVO.PersonalID, 1)    //总计
                    },
                    Today = new //今天
                    {
                        Personal = bBO.FindNumberOfVisitors("Personal", pVO.PersonalID, 1, 1), //名片
                        Product = bBO.FindNumberOfVisitors("Product", pVO.PersonalID, 1, 1),  //产品
                        News = bBO.FindNumberOfVisitors("News", pVO.PersonalID, 1, 1),    //新闻
                        Total = bBO.FindNumberOfVisitors(pVO.PersonalID, 1, 1)    //总计
                    },
                    Week = new //7天内
                    {
                        Personal = bBO.FindNumberOfVisitors("Personal", pVO.PersonalID, 2, 1), //名片
                        Product = bBO.FindNumberOfVisitors("Product", pVO.PersonalID, 2, 1),  //产品
                        News = bBO.FindNumberOfVisitors("News", pVO.PersonalID, 2, 1),    //新闻
                        Total = bBO.FindNumberOfVisitors(pVO.PersonalID, 2, 1)    //总计
                    },
                    Month = new //30天内
                    {
                        Personal = bBO.FindNumberOfVisitors("Personal", pVO.PersonalID, 3, 1), //名片
                        Product = bBO.FindNumberOfVisitors("Product", pVO.PersonalID, 3, 1),  //产品
                        News = bBO.FindNumberOfVisitors("News", pVO.PersonalID, 3, 1),    //新闻
                        Total = bBO.FindNumberOfVisitors(pVO.PersonalID, 3, 1)    //总计
                    }
                },
                Visits = new //访问次数
                {
                    Total = new //总数量
                    {
                        Personal = bBO.FindNumberOfVisits("Personal", pVO.PersonalID, 1), //名片
                        Product = bBO.FindNumberOfVisits("Product", pVO.PersonalID, 1),  //产品
                        News = bBO.FindNumberOfVisits("News", pVO.PersonalID, 1),    //新闻
                        Total = bBO.FindNumberOfVisits(pVO.PersonalID, 1)    //总计
                    },
                    Today = new //今天
                    {
                        Personal = bBO.FindNumberOfVisits("Personal", pVO.PersonalID, 1, 1), //名片
                        Product = bBO.FindNumberOfVisits("Product", pVO.PersonalID, 1, 1),  //产品
                        News = bBO.FindNumberOfVisits("News", pVO.PersonalID, 1, 1),    //新闻
                        Total = bBO.FindNumberOfVisits(pVO.PersonalID, 1, 1)    //总计
                    },
                    Week = new //7天内
                    {
                        Personal = bBO.FindNumberOfVisits("Personal", pVO.PersonalID, 2, 1), //名片
                        Product = bBO.FindNumberOfVisits("Product", pVO.PersonalID, 2, 1),  //产品
                        News = bBO.FindNumberOfVisits("News", pVO.PersonalID, 2, 1),    //新闻
                        Total = bBO.FindNumberOfVisits(pVO.PersonalID, 2, 1)    //总计
                    },
                    Month = new //30天内
                    {
                        Personal = bBO.FindNumberOfVisits("Personal", pVO.PersonalID, 3, 1), //名片
                        Product = bBO.FindNumberOfVisits("Product", pVO.PersonalID, 3, 1),  //产品
                        News = bBO.FindNumberOfVisits("News", pVO.PersonalID, 3, 1),    //新闻
                        Total = bBO.FindNumberOfVisits(pVO.PersonalID, 3, 1)    //总计
                    }
                },
                Statistics30days = Statistics30days,
                Statistics30daysOfVisits = Statistics30daysOfVisits
            };

            BusinessCard_JurisdictionVO B_Jurisdiction = bBO.getBusinessCard_Jurisdiction(pVO.BusinessID);

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = Statistics, Subsidiary = B_Jurisdiction };
        }

        /// <summary>
        /// 获取访问列表
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        [Route("GetAccessList"), HttpPost]
        public ResultObject GetAccessList([FromBody] ConditionModel condition, string token, int PersonalID = 0, int isGROUP = 1)
        {
            if (condition == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            else if (condition.Filter == null || condition.PageInfo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());

            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            if (PersonalID > 0)
            {
                pVO = cBO.FindPersonalById(PersonalID);
            }

            string conditionStr = "ToPersonalID =" + pVO.PersonalID + " and PersonalID<>" + pVO.PersonalID + " and (" + condition.Filter.Result() + ")";
            Paging pageInfo = condition.PageInfo;
            List<AccessrecordsViewVO> list = new List<AccessrecordsViewVO>();
            int count = 0;

            if (isGROUP == 1)
            {
                list = cBO.FindAccessrecordsViewGroupAllByPageIndex(conditionStr, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);
                count = cBO.FindAccessrecordsViewGroupCount(conditionStr);
            }
            else
            {
                list = cBO.FindAccessrecordsViewAllByPageIndex(conditionStr, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);
                count = cBO.FindAccessrecordsViewCount(conditionStr);
            }

            for (int i = 0; i < list.Count; i++)
            {
                list[i].IsImportant = cBO.GetIsImportant(list[i].PersonalID, list[i].ToPersonalID);
            }

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list, Count = count };
        }

        /// <summary>
        /// 测试
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        [Route("test3"), HttpGet, Anonymous]
        public ResultObject test3()
        {
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            List<OrderVO> list = cBO.FindOrderList("OrderNO='2022110717532469144300'");
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list };
        }

        /// <summary>
        /// 获取回递名片列表
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        [Route("GetReturnCardList"), HttpPost]
        public ResultObject GetReturnCardList([FromBody] ConditionModel condition, string token)
        {
            if (condition == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            else if (condition.Filter == null || condition.PageInfo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);


            string conditionStr = "Type = 'ReturnCard' and ToPersonalID=" + pVO.PersonalID + " and (" + condition.Filter.Result() + ")  GROUP BY PersonalID";
            Paging pageInfo = condition.PageInfo;

            List<AccessrecordsViewVO> list = new List<AccessrecordsViewVO>();
            int count = 0;

            list = cBO.FindAccessrecordsViewAllByPageIndex(conditionStr, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);
            count = cBO.FindReturnCardCount(pVO.PersonalID, condition.Filter.Result());

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list, Count = count };
        }

        /// <summary>
        /// 一建将所有访客创建为销售线索
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        [Route("OnekeyCreateCrmByAcces"), HttpGet]
        public ResultObject OnekeyCreateCrmByAcces(string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            CrmBO CrmBO = new CrmBO(new CustomerProfile());

            string conditionStr = "ToPersonalID =" + pVO.PersonalID + " and PersonalID<>" + pVO.PersonalID;
            List<AccessrecordsViewVO> list = cBO.FindAccessrecordsViewGroupAll(conditionStr);

            int sum = 0;
            int repeatsum = 0;

            for (int i = 0; i < list.Count; i++)
            {
                //如果有重复的客户就不添加
                if (CrmBO.FindCrmViewCount("PersonalID = " + pVO.PersonalID + " and Title = '" + list[i].Name + "'") == 0)
                {

                    CrmVO CrmVO = new CrmVO();
                    CrmVO.CrmID = 0;
                    CrmVO.CreatedAt = DateTime.Now;
                    CrmVO.PersonalID = pVO.PersonalID;
                    CrmVO.BusinessID = pVO.BusinessID;
                    CrmVO.Title = list[i].Name;

                    string Content = list[i].AccessAt.ToString("yyyy年MM月dd日 hh:mm") + "访问了";
                    if (list[i].Type == "Personal")
                    {
                        Content += "您的名片";
                    }
                    if (list[i].Type == "Product")
                    {
                        Content += "产品《" + GetPropertyValue(list[i].Info, "Title") + "》";
                    }
                    if (list[i].Type == "News")
                    {
                        Content += "新闻《" + GetPropertyValue(list[i].Info, "Title") + "》";
                    }
                    if (list[i].Type == "Video")
                    {
                        Content += "视频《" + GetPropertyValue(list[i].Info, "Title") + "》";
                    }
                    if (list[i].Type == "Info")
                    {
                        Content += "资料《" + GetPropertyValue(list[i].Info, "Title") + "》";
                    }
                    if (list[i].Type == "ReturnCard")
                    {
                        Content = list[i].AccessAt.ToString("yyyy年MM月dd日 hh:mm") + "回递了名片";
                    }
                    if (list[i].Type == "GreetingCard")
                    {
                        GreetingCardVO GreetingCardVO = cBO.FindGreetingCardById(list[i].ById);
                        Content = list[i].AccessAt.ToString("yyyy年MM月dd日 hh:mm") + "转发了短视频《" + GreetingCardVO.Title + "》";
                    }
                    if (list[i].Type == "BrowseGreetingCard")
                    {
                        GreetingCardVO GreetingCardVO = cBO.FindGreetingCardById(list[i].ById);
                        Content = list[i].AccessAt.ToString("yyyy年MM月dd日 hh:mm") + "浏览了短视频《" + GreetingCardVO.Title + "》";
                    }
                    if (list[i].Type == "ColorPage")
                    {
                        Content += "电子彩页";
                    }

                    CrmVO.Content = Content;

                    CrmVO.Type = "Clue";
                    CrmVO.Order_info = 0;
                    CrmVO.Status = 1;
                    CrmVO.Field1 = list[i].Address;
                    CrmVO.Field2 = list[i].Phone;
                    CrmVO.Field3 = list[i].BusinessName;
                    CrmVO.Field4 = "未曾接触";
                    CrmVO.Field5 = "访客";

                    if (CrmBO.AddCrm(CrmVO) > 0)
                        sum += 1;
                }
                else
                {
                    repeatsum += 1;
                }
            }

            return new ResultObject() { Flag = 1, Message = "成功创建" + sum + "条线索，已排除" + repeatsum + "条重复数据", Result = sum };
        }
        //获取object键值
        public static object GetPropertyValue(object info, string field)
        {
            if (info == null) return null;
            Type t = info.GetType();
            IEnumerable<System.Reflection.PropertyInfo> property = from pi in t.GetProperties() where pi.Name.ToLower() == field.ToLower() select pi;
            return property.First().GetValue(info, null);
        }

        /// <summary>
        /// 一建将名片夹所有名片创建为销售线索
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        [Route("OnekeyCreateCrmByCollection"), HttpGet]
        public ResultObject OnekeyCreateCrmByCollection(string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            CrmBO CrmBO = new CrmBO(new CustomerProfile());
            CardBO CardBO = new CardBO(new CustomerProfile());

            List<CardDataVO> list = CardBO.FindCardCollectionByCustomerId(customerId);

            int sum = 0;
            int repeatsum = 0;

            for (int i = 0; i < list.Count; i++)
            {
                //如果有重复的客户就不添加
                if (CrmBO.FindCrmViewCount("PersonalID = " + pVO.PersonalID + " and Title = '" + list[i].Name + "'") == 0)
                {

                    CrmVO CrmVO = new CrmVO();
                    CrmVO.CrmID = 0;
                    CrmVO.CreatedAt = DateTime.Now;
                    CrmVO.PersonalID = pVO.PersonalID;
                    CrmVO.BusinessID = pVO.BusinessID;
                    CrmVO.Title = list[i].Name;

                    CrmVO.Type = "Clue";
                    CrmVO.Order_info = 0;
                    CrmVO.Status = 1;
                    CrmVO.Field1 = list[i].Address;
                    CrmVO.Field2 = list[i].Phone;
                    CrmVO.Field3 = list[i].CorporateName;
                    CrmVO.Field4 = "未曾接触";
                    CrmVO.Field5 = "名片夹";

                    if (CrmBO.AddCrm(CrmVO) > 0)
                        sum += 1;
                }
                else
                {
                    repeatsum += 1;
                }
            }

            return new ResultObject() { Flag = 1, Message = "成功创建" + sum + "条线索，已排除" + repeatsum + "条重复数据", Result = sum };
        }

        /// <summary>
        /// 一建将活动所有报名创建为销售线索
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        [Route("OnekeyCreateCrmByParty"), HttpGet]
        public ResultObject OnekeyCreateCrmByParty(string token, int PartyID)
        {

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            CrmBO CrmBO = new CrmBO(new CustomerProfile());
            CardBO CardBO = new CardBO(new CustomerProfile());

            List<CardPartySignUpViewVO> list = CardBO.FindSignUpViewByPartyID(PartyID);

            int sum = 0;
            int repeatsum = 0;

            for (int i = 0; i < list.Count; i++)
            {
                List<CardPartySignUpFormVO> Form = CardBO.FindSignUpFormByFormStr(list[i].SignUpForm);

                string CorporateName = "", Phone = "", Name = "", Address = "";

                for (int j = 0; j < Form.Count; j++)
                {
                    if (Form[j].Name == "姓名")
                    {
                        Name = Form[j].value;
                    }
                    if (Form[j].Name == "手机")
                    {
                        Phone = Form[j].value;
                    }
                    if (Form[j].Name == "工作单位")
                    {
                        CorporateName = Form[j].value;
                    }
                    if (Form[j].Name == "单位地址")
                    {
                        Address = Form[j].value;
                    }
                }


                //如果有重复的客户就不添加
                if (CrmBO.FindCrmViewCount("PersonalID = " + pVO.PersonalID + " and Title = '" + Name + "'") == 0)
                {
                    CrmVO CrmVO = new CrmVO();
                    CrmVO.CrmID = 0;
                    CrmVO.CreatedAt = DateTime.Now;
                    CrmVO.PersonalID = pVO.PersonalID;
                    CrmVO.BusinessID = pVO.BusinessID;
                    CrmVO.Title = Name;

                    CrmVO.Type = "Clue";
                    CrmVO.Order_info = 0;
                    CrmVO.Status = 1;
                    CrmVO.Field1 = Address;
                    CrmVO.Field2 = Phone;
                    CrmVO.Field3 = CorporateName;
                    CrmVO.Field4 = "未曾接触";
                    CrmVO.Field5 = list[i].Title;

                    if (CrmBO.AddCrm(CrmVO) > 0)
                        sum += 1;
                }
                else
                {
                    repeatsum += 1;
                }
            }

            return new ResultObject() { Flag = 1, Message = "成功创建" + sum + "条线索，已排除" + repeatsum + "条重复数据", Result = sum };
        }

        /// <summary>
        /// 一建将名片群所有名片创建为销售线索
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        [Route("OnekeyCreateCrmByGroup"), HttpGet]
        public ResultObject OnekeyCreateCrmByGroup(string token, int GroupID)
        {

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            CrmBO CrmBO = new CrmBO(new CustomerProfile());
            CardBO CardBO = new CardBO(new CustomerProfile());

            List<CardGroupCardViewVO> list = CardBO.FindCardGroupCardViewByGroupID(GroupID);
            CardGroupVO cVO = CardBO.FindCardGroupById(GroupID);

            int sum = 0;
            int repeatsum = 0;

            for (int i = 0; i < list.Count; i++)
            {
                //如果有重复的客户就不添加
                if (CrmBO.FindCrmViewCount("PersonalID = " + pVO.PersonalID + " and Title = '" + list[i].Name + "'") == 0 && list[i].Status != 0)
                {
                    CrmVO CrmVO = new CrmVO();
                    CrmVO.CrmID = 0;
                    CrmVO.CreatedAt = DateTime.Now;
                    CrmVO.PersonalID = pVO.PersonalID;
                    CrmVO.BusinessID = pVO.BusinessID;
                    CrmVO.Title = list[i].Name;

                    CrmVO.Type = "Clue";
                    CrmVO.Order_info = 0;
                    CrmVO.Status = 1;
                    CrmVO.Field1 = list[i].Address;
                    CrmVO.Field2 = list[i].Phone;
                    CrmVO.Field3 = list[i].CorporateName;
                    CrmVO.Field4 = "未曾接触";
                    CrmVO.Field5 = cVO.GroupName;

                    if (CrmBO.AddCrm(CrmVO) > 0)
                        sum += 1;
                }
                else
                {
                    if (list[i].Status != 0)
                        repeatsum += 1;
                }
            }

            return new ResultObject() { Flag = 1, Message = "成功创建" + sum + "条线索，已排除" + repeatsum + "条重复数据", Result = sum };
        }

        /// <summary>
        /// 下载名片夹,分页
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DownloadCollectionListsByPageIndex"), HttpPost]
        public ResultObject DownloadCollectionListsByPageIndex([FromBody] ConditionModel condition, string token)
        {
            if (condition == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            else if (condition.Filter == null || condition.PageInfo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CardBO cBO = new CardBO(new CustomerProfile());
            try
            {
                string conditionStr = "t_CustomerId = " + customerId + " and (" + condition.Filter.Result() + ")";
                Paging pageInfo = condition.PageInfo;
                List<CardDataVO> list = cBO.FindCardCollectionAllByPageIndex(conditionStr, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);
                int count = cBO.FindCardCollectionAllCount(conditionStr);

                object CardDatalist = new
                {
                    List = list,
                    Count = count
                };

                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = CardDatalist };
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }

        }

        /// <summary>
        /// 下载我发起的活动（包括我作为联系人的活动）
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DownloadPartyList"), HttpGet]
        public ResultObject DownloadPartyList(string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CardBO cBO = new CardBO(new CustomerProfile());
            List<CardPartyViewVO> uVO = cBO.FindPartyByCustomerIdOrContactsCustomerId(customerId);

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO };
        }

        /// <summary>
        /// 下载我的名片群
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DownloadGroupList"), HttpGet]
        public ResultObject DownloadGroupList(string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CardBO cBO = new CardBO(new CustomerProfile());
            List<CardGroupViewVO> uVO = cBO.FindCardGroupViewByCustomerId(customerId);

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO };
        }

        /// <summary>
        /// 获取我的代理商列表
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetAgentList"), HttpGet]
        public ResultObject GetAgentList(string token, int AgentLevelID)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);

            List<AgentViewVO> dVO = cBO.FindAgentViewByBusinessID(pVO.BusinessID, AgentLevelID);

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = dVO };
        }

        /// <summary>
        /// 获取我的代理商级别
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetAgentLevelList"), HttpGet]
        public ResultObject GetAgentLevelList(string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);

            List<AgentLevelVO> dVO = cBO.FindAgentLevelByBusinessID(pVO.BusinessID);

            for (int i = 0; i < dVO.Count; i++)
            {
                dVO[i].AgentCount = cBO.FindAgentCountByCondition("AgentLevelID = " + dVO[i].AgentLevelID);
            }

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = dVO };
        }

        /// <summary>
        /// 获取代理商级别详情
        /// </summary>
        /// <param name="SortID"></param>
        /// <returns></returns>
        [Route("GetAgentLevelSite"), HttpGet]
        public ResultObject GetAgentLevelSite(string token, int AgentLevelID)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            AgentLevelVO sVO = cBO.FindAgentLevelById(AgentLevelID);

            if (sVO != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = sVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取加入代理商页面信息
        /// </summary>
        /// <param name="SortID"></param>
        /// <returns></returns>
        [Route("GetJoinAgent"), HttpGet]
        public ResultObject GetJoinAgent(string token, int AgentLevelID)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            AgentLevelVO sVO = cBO.FindAgentLevelById(AgentLevelID);
            BusinessCardVO bVO = cBO.FindBusinessCardById(sVO.BusinessID);


            if (sVO != null && bVO != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = new { AgentLevel = sVO, BusinessCard = bVO } };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }
        /// <summary>
        /// 加入代理商
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("JoinAgent"), HttpGet]
        public ResultObject JoinAgent(int AgentLevelID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            if (pVO != null)
            {
                AgentLevelVO sVO = cBO.FindAgentLevelById(AgentLevelID);
                if (sVO == null)
                {
                    return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
                }
                if (cBO.isJoinAgent(pVO.PersonalID, sVO.AgentLevelID))
                {
                    return new ResultObject() { Flag = 0, Message = "您已经是代理商!", Result = null };
                }

                AgentVO aVO = new AgentVO();
                aVO.AgentLevelID = sVO.AgentLevelID;
                aVO.BusinessID = sVO.BusinessID;
                aVO.CreatedAt = DateTime.Now;
                aVO.PersonalID = pVO.PersonalID;

                if (cBO.AddAgent(aVO) > 0)
                {

                    return new ResultObject() { Flag = 1, Message = "加入成功!", Result = null };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "加入失败!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "加入失败!", Result = null };
            }
        }

        /// <summary>
        /// 批量删除代理商
        /// </summary>
        /// <param name="VO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("BatchdelAgent"), HttpGet]
        public ResultObject BatchdelAgent(string AgentID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            if (!cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Product") && !cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Admin"))
            {
                return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
            }

            try
            {
                if (!string.IsNullOrEmpty(AgentID))
                {
                    string[] CrmIDIdArr = AgentID.Split(',');
                    bool isAllDelete = true;
                    for (int i = 0; i < CrmIDIdArr.Length; i++)
                    {
                        try
                        {

                            AgentVO AgentVO = cBO.FindAgentById(Convert.ToInt32(CrmIDIdArr[i]));
                            if (AgentVO == null)
                            {
                                isAllDelete = false;
                                continue;
                            }
                            //是否有修改权限
                            if (AgentVO.BusinessID != pVO.BusinessID)
                            {
                                isAllDelete = false;
                                continue;
                            }
                            if (cBO.DeleteAgentlById(AgentVO.AgentID) < 0)
                            {
                                isAllDelete = false;
                            }
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
        /// 添加或更新代理级别
        /// </summary>
        /// <param name="VO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateAgentLevel"), HttpPost]
        public ResultObject UpdateAgentLevel([FromBody] AgentLevelVO AgentLevelVO, string token)
        {
            if (AgentLevelVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);

            /*审核文本是否合法*/
            /*
            if (!cBO.msg_sec_check(AgentLevelVO))
            {
                return new ResultObject() { Flag = 0, Message = "有非法关键词，请重新填写!", Result = null };
            }*/
            /*审核文本是否合法*/

            if (!cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Product") && !cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Admin"))
            {
                return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
            }

            if (AgentLevelVO.AgentLevelID > 0)
            {
                AgentLevelVO sVO = cBO.FindAgentLevelById(AgentLevelVO.AgentLevelID);

                if (sVO.BusinessID != AgentLevelVO.BusinessID)
                {
                    return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
                }

                if (cBO.UpdateAgentLevel(AgentLevelVO))
                {
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = AgentLevelVO.AgentLevelID };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }
            }
            else
            {
                AgentLevelVO.BusinessID = pVO.BusinessID;

                int AgentLevelID = cBO.AddAgentLevel(AgentLevelVO);
                if (AgentLevelID > 0)
                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = AgentLevelID };
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
        }

        /// <summary>
        /// 删除代理级别
        /// </summary>
        /// <param name="VO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DelAgentLevel"), HttpGet]
        public ResultObject DelAgentLevel(int AgentLevelID, string token)
        {
            if (AgentLevelID <= 0)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);

            if (!cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Product") && !cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Admin"))
            {
                return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
            }

            AgentLevelVO sVO = cBO.FindAgentLevelById(AgentLevelID);
            if (sVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "该级别已被删除!", Result = null };
            }

            List<AgentVO> LpVO = cBO.FindAgentByAgentLevelID(AgentLevelID);

            if (LpVO.Count > 0)
            {
                return new ResultObject() { Flag = 0, Message = "请先移除该级别所属的代理商!", Result = null };
            }

            if (cBO.DeleteAgentLevelById(AgentLevelID) > 0)
            {
                return new ResultObject() { Flag = 1, Message = "删除成功!", Result = sVO.AgentLevelID };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
            }
        }

        /// <summary>
        /// 添加或更新产品代理价
        /// </summary>
        /// <param name="InfoID">产品ID</param>
        /// <param name="AgentLevelID">代理级别ID</param>
        /// <param name="Cost">代理价格</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateAgentlevelCost"), HttpGet]
        public ResultObject UpdateAgentlevelCost(int InfoID, int AgentLevelID, int Discount, string token)
        {
            if (InfoID <= 0 || AgentLevelID <= 0 || Discount < 0)
            {
                return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
            }

            if (Discount == 0)
            {
                return new ResultObject() { Flag = 0, Message = "不能设置为0折扣!", Result = null };
            }

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);

            if (!cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Product") && !cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Admin"))
            {
                return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
            }

            InfoViewVO sVO = cBO.FindInfoViewById(InfoID);
            AgentLevelVO AgentLevelVO = cBO.FindAgentLevelById(AgentLevelID);
            if (AgentLevelVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "该代理级别不存在或被删除!", Result = null };
            }
            if (sVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "该产品不存在或被删除!", Result = null };
            }

            if (sVO.BusinessID != pVO.BusinessID || AgentLevelVO.BusinessID != pVO.BusinessID)
            {
                return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
            }

            List<AgentlevelCostVO> aVOList = cBO.FindAgentByAgentlevelCostID(sVO.InfoID, AgentLevelVO.AgentLevelID);
            if (aVOList.Count <= 0)
            {
                AgentlevelCostVO aVO = new AgentlevelCostVO();
                aVO.AgentLevelID = AgentLevelVO.AgentLevelID;
                aVO.Discount = Discount;
                aVO.InfoID = sVO.InfoID;
                cBO.AddAgentlevelCost(aVO);
            }
            else
            {
                aVOList[0].Discount = Discount;
                cBO.UpdateAgentlevelCost(aVOList[0]);
            }
            return new ResultObject() { Flag = 1, Message = "设置成功!", Result = null };
        }

        /// <summary>
        /// 删除产品代理价
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DelAgentlevelCost"), HttpGet]
        public ResultObject DelAgentlevelCost(int InfoID, int AgentLevelID, string token)
        {
            if (InfoID <= 0 || AgentLevelID <= 0)
            {
                return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
            }

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);

            if (!cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Product") && !cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Admin"))
            {
                return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
            }

            InfoViewVO sVO = cBO.FindInfoViewById(InfoID);
            AgentLevelVO AgentLevelVO = cBO.FindAgentLevelById(AgentLevelID);
            if (AgentLevelVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "该代理级别不存在或被删除!", Result = null };
            }
            if (sVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "该产品不存在或被删除!", Result = null };
            }

            if (sVO.BusinessID != pVO.BusinessID || AgentLevelVO.BusinessID != pVO.BusinessID)
            {
                return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
            }

            if (cBO.DeleteAgentlevelCostById(sVO.InfoID, AgentLevelVO.AgentLevelID) > 0)
            {
                return new ResultObject() { Flag = 1, Message = "成功删除该产品的代理价!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 1, Message = "删除失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取微信支付数据,在线开通
        /// </summary>
        /// <param name="InfoID">产品id</param>
        /// <param name="PersonalID">邀请者id</param>
        /// <param name="isUsed">0：存入库存 1：立即使用</param>
        /// <param name="token"></param>
        /// <returns></returns>
        [Route("GetUnifiedOrderResult"), HttpGet]
        public async Task<ResultObject> GetUnifiedOrderResult(int InfoID, int PersonalID, int isUsed, string FormId, string code, string token, string Phone = "", string Address = "", string Name = "", int CostID = 0, int AppType = 0, int GroupBuyID = 0, int isGroupBuy = 1, string ShareCode = "")
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            AppVO AppVO = AppBO.GetApp(AppType);

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            InfoViewVO sVO = cBO.FindInfoViewById(InfoID);

            BusinessCardVO InfoBVO = cBO.FindBusinessCardById(sVO.BusinessID);
            if (InfoBVO.isPay == 0 || InfoBVO.BusinessID == 73)
            {
                return new ResultObject() { Flag = 0, Message = "该产品未开启在线支付，请咨询客服!", Result = null };
            }

            string OpenId = cBO.getOpenId(code, AppType);
            CardBO CardBO = new CardBO(new CustomerProfile(), 1);
            CardFormListVO suVO = new CardFormListVO();
            suVO.Style = 0;
            suVO.FormId = "";
            suVO.CustomerId = customerId;
            suVO.CreatedAt = DateTime.Now;
            suVO.OpenId = OpenId;
            int FormListID = CardBO.AddFormId(suVO);

            if (CostID > 0)
            {
                InfoCostVO InfoCostVO = cBO.FindInfoCostById(CostID);
                if (InfoCostVO == null)
                {
                    return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
                }
                if (InfoCostVO.InfoID != sVO.InfoID)
                {
                    return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
                }
                if (InfoCostVO.Status < 0)
                {
                    return new ResultObject() { Flag = 0, Message = "该产品规格已被删除，无法购买!", Result = null };
                }

                sVO.Cost = InfoCostVO.Cost;
                sVO.CostName = InfoCostVO.CostName;
                if (InfoCostVO.Attribute != "")
                {
                    sVO.CostName += "-" + InfoCostVO.Attribute;
                }
                sVO.PerPersonLimit = InfoCostVO.PerPersonLimit;
                sVO.GiveIntegral = InfoCostVO.GiveIntegral;
            }

            try
            {
                if (Phone != "" && pVO.Phone == "")
                {
                    pVO.Phone = Phone;
                }
                if (Address != "" && pVO.Address == "")
                {
                    pVO.Address = Address;
                }
                if (Name != "" && pVO.Name == "")
                {
                    pVO.Name = Name;
                }
                cBO.UpdatePersonal(pVO);
            }
            catch
            {

            }

            if (sVO == null || sVO.Status == -2)
            {
                return new ResultObject() { Flag = 0, Message = "该产品不存在或被删除!", Result = null };
            }
            if (sVO.Status == 0)
            {
                return new ResultObject() { Flag = 0, Message = "该产品未上架，暂不能购买!", Result = null };
            }
            if (sVO.Cost < 0)
            {
                return new ResultObject() { Flag = 0, Message = "价格错误，请联系商家!", Result = null };
            }
            if (sVO.isSeckill == 1)
            {
                if (sVO.SeckillStartTime > DateTime.Now)
                {
                    return new ResultObject() { Flag = 0, Message = "秒杀活动暂未开始!", Result = null };
                }
                if (sVO.SeckillEndTime < DateTime.Now)
                {
                    return new ResultObject() { Flag = 0, Message = "秒杀活动已结束!", Result = null };
                }
            }

            if (sVO.PerPersonLimit < 0)
            {
                return new ResultObject() { Flag = 0, Message = "库存不足，请联系商家!", Result = null };
                /*
                int sum = 0;
                if (CostID > 0)
                {
                    sum = cBO.FindOrderByCondition("CostID=" + CostID + " and PersonalID=" + pVO.PersonalID + " and Status=1");
                } else
                {
                    sum = cBO.FindOrderByCondition("InfoID=" + sVO.InfoID + " and PersonalID=" + pVO.PersonalID + " and Status=1");
                }
                if (sum >= sVO.PerPersonLimit)
                {
                    return new ResultObject() { Flag = 0, Message = "购买失败，本产品规格每人限购" + sVO.PerPersonLimit + "份!", Result = null };

                }*/
            }

            if (sVO.ProductLimit > 0)
            {
                int sum = 0;
                sum = cBO.FindOrderByCondition("InfoID=" + sVO.InfoID + " and PersonalID=" + pVO.PersonalID + " and Status=1");
                if (sum >= sVO.ProductLimit)
                {
                    return new ResultObject() { Flag = 0, Message = "购买失败，本产品每人限购" + sVO.ProductLimit + "份!", Result = null };
                }
            }

            if (sVO.StoreAmount < 0)
            {
                return new ResultObject() { Flag = 0, Message = "库存不足，请联系商家!", Result = null };
                /*
                int sum = 0;
                sum = cBO.FindOrderByCondition("InfoID=" + sVO.InfoID + " and Status=1");
                if (sum >= sVO.StoreAmount)
                {
                    return new ResultObject() { Flag = 0, Message = "库存不足，请联系商家!", Result = null };
                }*/
            }
            if (sVO.isSeckill == 1 & sVO.SeckillLimit < 0)
            {
                return new ResultObject() { Flag = 0, Message = "秒杀库存不足，请联系商家!", Result = null };
            }

            if (isUsed != 0 && isUsed != 1 && isUsed != 2)
            {
                return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
            }

            if (sVO.InfoID == 905)
            {
                CustomerBO uBO = new CustomerBO(new CustomerProfile());
                CustomerVO uVO = uBO.FindCustomenById(pVO.CustomerId);
                if (uVO.isVip && uVO.isVip && uVO.ExpirationAt > DateTime.Now && (uVO.VipLevel == 2 || uVO.VipLevel == 3))
                {
                    return new ResultObject() { Flag = 0, Message = "您已经是合伙人或分公司Vip，如想降级为普通Vip请等当前VIP到期后再开通！", Result = null };
                }
            }

            //判断能否续费
            if (isUsed == 1 && pVO.BusinessID > 0 && sVO.OfficialProducts != "")
            {
                BusinessCardVO bVO = cBO.FindBusinessCardById(pVO.BusinessID);
                int l1 = 0;
                if (sVO.OfficialProducts == "SelfEmployed2")
                    l1 = 0;
                if (sVO.OfficialProducts == "SelfEmployed")
                    l1 = 1;
                if (sVO.OfficialProducts == "Basic")
                    l1 = 2;
                if (sVO.OfficialProducts == "Standard")
                    l1 = 3;
                if (sVO.OfficialProducts == "Advanced")
                    l1 = 4;

                int l2 = 0;
                if (sVO.OfficialProducts == "SelfEmployed2")
                    l2 = 0;
                if (sVO.OfficialProducts == "SelfEmployed")
                    l2 = 1;
                if (bVO.OfficialProducts == "Basic")
                    l2 = 2;
                if (bVO.OfficialProducts == "Standard")
                    l2 = 3;
                if (bVO.OfficialProducts == "Advanced")
                    l2 = 4;

                if (bVO.HeadquartersID > 0)
                {
                    return new ResultObject() { Flag = 0, Message = "您的公司属于分店，无法单独续费!", Result = null };
                }

                if (l2 > l1)
                {
                    return new ResultObject() { Flag = 0, Message = "您的公司已开通了更高版本的企业名片，请选择对应版本进行续费!", Result = null };
                }
            }





            OrderVO oVO = new OrderVO();

            oVO.PersonalID = pVO.PersonalID;
            oVO.FormId = FormId;
            oVO.OpenId = OpenId;
            oVO.CreatedAt = DateTime.Now;
            oVO.BusinessID = 0;
            oVO.Status = 0;
            oVO.Phone = Phone;
            oVO.Address = Address;
            oVO.CostID = CostID;
            oVO.CostName = sVO.CostName;
            oVO.OriginalCost = sVO.Cost;
            oVO.Name = Name;
            oVO.AppType = AppVO.AppType;
            oVO.GiveShopVipID = sVO.GiveShopVipID;
            oVO.GiveShopVipDay = sVO.GiveShopVipDay;

            Random ran = new Random();
            oVO.OrderNO = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ran.Next(10000, 99999);
            oVO.AgentPersonalID = PersonalID;
            oVO.InfoID = sVO.InfoID;

            oVO.isUsed = isUsed;
            string body = "购买产品";
            if (InfoBVO.isAgent == 1)
            {
                //判断是否为代理商
                List<AgentViewVO> AgentViewVO = cBO.FindAgentViewByPersonalID(sVO.BusinessID, pVO.PersonalID);

                if (AgentViewVO.Count > 0)
                {
                    body = "代理保证金购买";
                    oVO.Cost = cBO.FindAgentlevelCostByPersonalID(pVO.PersonalID, sVO.InfoID, CostID);

                    decimal agentB = cBO.FindMyAgentIntegral(sVO.BusinessID, pVO.PersonalID);

                    if (agentB < oVO.Cost)
                        return new ResultObject() { Flag = 0, Message = "您的保证金余额不足，请联系客服充值!", Result = null };

                    //用代理保证金支付

                    oVO.Status = 1;
                    oVO.PayOutStatus = 1;
                    oVO.payAt = DateTime.Now;
                    oVO.isAgentBuy = 1;

                    int AddOrderID = cBO.AddOrder(oVO);
                    if (AddOrderID > 0)
                    {
                        //发放vip
                        if (oVO.GiveShopVipID > 0)
                        {
                            cBO.AddShopVipPersonal(oVO.PersonalID, oVO.GiveShopVipID, oVO.GiveShopVipDay, AddOrderID);
                        }

                        if ((oVO.isUsed == 1 || oVO.isUsed == 2) && sVO.OfficialProducts != "")
                        {
                            if (!cBO.EstablishedBusinessCard(oVO.InfoID, oVO.PersonalID, AddOrderID, AppType))
                            {
                                OrderVO newVO = new OrderVO();
                                newVO.OrderID = AddOrderID;
                                newVO.Status = 0;
                                newVO.PayOutStatus = 0;
                                cBO.UpdateOrder(newVO);
                                return new ResultObject() { Flag = 0, Message = "开通或续费失败，请重试!", Result = null };
                            }
                        }

                        //开通个人会员
                        if (sVO.InfoID == 905)
                        {
                            CardOrderVO cpVO = new CardOrderVO();
                            cpVO.CustomerId = customerId;
                            cpVO.OpenId = "";
                            cpVO.CreatedAt = DateTime.Now;
                            cpVO.Type = 2;
                            cpVO.Cost = oVO.Cost;
                            cpVO.OrderNO = "BusinessCardOrder(保证金购买)_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ran.Next(10000, 99999);

                            cpVO.CardOrderID = 0;
                            cpVO.Status = 1;
                            cpVO.payAt = DateTime.Now;
                            int CardOrderID = CardBO.AddOrder(cpVO);

                            if (CardOrderID > 0)
                            {
                                //开通会员
                                CardBO.OpeningVIP(CardOrderID);
                            }
                        }

                        //扣除保证金
                        AgentIntegralVO iVO = new AgentIntegralVO();
                        iVO.PersonalID = pVO.PersonalID;
                        iVO.OperPersonalID = PersonalID;
                        iVO.Status = 1;
                        iVO.OrderID = AddOrderID;
                        iVO.CreatedAt = DateTime.Now;
                        iVO.BusinessID = sVO.BusinessID;
                        iVO.Balance = -oVO.Cost;
                        iVO.Note = "购买" + sVO.Title + "-" + sVO.CostName;
                        oVO.AgentIntegralID = cBO.AddAgentIntegral(iVO);
                        cBO.UpdateOrder(oVO);

                        //扣除库存
                        cBO.StoreAmount(sVO.InfoID, CostID);

                        return new ResultObject() { Flag = 5, Message = "购买成功", Result = null };
                    }
                    else
                    {
                        return new ResultObject() { Flag = 0, Message = "操作失败，请重试!", Result = null };
                    }

                }
            }

            oVO.Cost = sVO.Cost;

            //判断是否为二级商户
            int isMerchant = 0;
            EcommerceBO eBO = new EcommerceBO();
            wxMerchantVO mVO = eBO.getBusinessMerchant(sVO.BusinessID);
            if (mVO != null && AppVO.AppType == 1)
            {
                isMerchant = 1;
            }

            //拼团折扣价购买
            if (sVO.isGroupBuy == 1 && isGroupBuy == 1 && isMerchant != 1)
            {
                if (GroupBuyID > 0)
                {
                    GroupBuyVO gbVO = cBO.FindGroupBuyById(GroupBuyID);
                    if (gbVO == null)
                    {
                        return new ResultObject() { Flag = 0, Message = "拼团失败", Result = null };
                    }
                    int GroupBuyOrderNum = cBO.FindOrderByCondition("InfoID=" + sVO.InfoID + " and GroupBuyID=" + GroupBuyID + " and (Status=1 or Status=3)");
                    if (GroupBuyOrderNum >= gbVO.PeopleNumber)
                    {
                        return new ResultObject() { Flag = 0, Message = "该拼团人数已满！", Result = null };
                    }
                    if (gbVO.ExpireAt < DateTime.Now)
                    {
                        return new ResultObject() { Flag = 0, Message = "该拼团已超时！", Result = null };
                    }
                }

                body = "拼团折扣价购买";
                oVO.isGroupBuy = 1;
                oVO.Discount = sVO.GroupBuyDiscount;
                oVO.Cost = oVO.Cost * oVO.Discount / 100;
                oVO.GroupBuyID = GroupBuyID;
            }

            //秒杀折扣价购买
            if (sVO.isSeckill == 1 && sVO.isGroupBuy == 0)
            {
                body = "秒杀折扣价购买";
                oVO.Discount = sVO.SeckillDiscount;
                oVO.Cost = oVO.Cost * oVO.Discount / 100;
            }

            //Vip折扣价购买
            if (sVO.isVipDiscount == 1 && sVO.isSeckill == 0 && sVO.isGroupBuy == 0)
            {
                body = "Vip折扣价购买";
                oVO.Discount = cBO.GetVipDiscount(oVO.PersonalID, sVO.InfoID);
                oVO.Cost = oVO.Cost * oVO.Discount / 100;
            }

            if (sVO.GiveIntegral > 0)
            {
                //赠送积分
                IntegralVO iVO = new IntegralVO();
                iVO.PersonalID = pVO.PersonalID;
                iVO.OperPersonalID = PersonalID;
                iVO.Status = 0;
                iVO.CreatedAt = DateTime.Now;
                iVO.BusinessID = sVO.BusinessID;
                iVO.Balance = sVO.GiveIntegral;
                iVO.Note = "购买" + sVO.Title;

                oVO.IntegralID = cBO.AddIntegral(iVO);
            }


            //如果是二级商户
            if (isMerchant == 1)
            {
                oVO.sub_mchid = mVO.sub_mchid;
                oVO.isEcommerceBuy = 1;
                if (mVO.SplitProportion > 0)
                {
                    oVO.SplitCost = oVO.Cost * mVO.SplitProportion / 100;
                }
            }

            //是否分账
            int isprofit_sharing = 0;

            //返利计算
            if (sVO.isProfitsharing == 1 && sVO.Profitsharing > 0 && sVO.InfoID != 905 && isMerchant != 1 && oVO.Cost > 0 && sVO.isGroupBuy == 0)
            {
                List<ShareVO> ShareVO = cBO.FindShareList("Code='" + ShareCode + "' and SendOrReceive=1");
                if (ShareVO.Count > 0)
                {
                    //可分账总金额,扣除手续费
                    decimal TProfitsharingCost = oVO.Cost;
                    if (oVO.Cost > 1)
                    {
                        TProfitsharingCost = oVO.Cost - (oVO.Cost * decimal.Parse("0.006"));
                    }

                    //自己不能给自己推荐
                    if (pVO.PersonalID != ShareVO[0].PersonalID)
                    {
                        if (sVO.isProfitsharingToVIP == 0)
                        {
                            oVO.ProfitsharingCost = TProfitsharingCost * sVO.Profitsharing / 100;
                        }
                        if (sVO.isProfitsharingToVIP == 1)
                        {
                            int Profitsharing = cBO.GetProfitsharing(ShareVO[0].PersonalID, sVO.InfoID);
                            oVO.ProfitsharingCost = TProfitsharingCost * Profitsharing / 100;
                        }
                        if (oVO.ProfitsharingCost > 0)
                        {
                            oVO.ProfitsharingOpenId = ShareVO[0].OpenId;
                            PersonalVO SharePVO = cBO.FindPersonalById(ShareVO[0].PersonalID);
                            oVO.ProfitsharingCid = SharePVO.CustomerId;
                            oVO.ProfitsharingStatus = 0;
                            isprofit_sharing = 1;
                        }

                    }
                }
            }
            //二级返利计算
            if (sVO.isProfitsharing == 1 && sVO.TowProfitsharing > 0 && oVO.ProfitsharingCid > 0 && sVO.InfoID != 905 && isMerchant != 1 && oVO.Cost > 0 && sVO.isGroupBuy == 0)
            {
                List<ProfitsharingVO> pList = cBO.FindProfitsharingList("CustomerId=" + oVO.ProfitsharingCid + " and BusinessID=" + sVO.BusinessID);
                if (pList.Count > 0)
                {
                    //自己不能给自己推荐
                    if (pVO.CustomerId != pList[0].ToCustomerId)
                    {
                        if (sVO.isProfitsharingToVIP == 0)
                        {
                            oVO.TowProfitsharingCost = oVO.Cost * sVO.TowProfitsharing / 100;
                        }
                        if (sVO.isProfitsharingToVIP == 1)
                        {
                            int Profitsharing = cBO.GetTowProfitsharing(pList[0].ToCustomerId, sVO.InfoID);
                            oVO.TowProfitsharingCost = oVO.Cost * Profitsharing / 100;
                        }
                        if (oVO.TowProfitsharingCost > 0)
                        {
                            oVO.TowProfitsharingOpenId = pList[0].ToOpenID;
                            oVO.TowProfitsharingCid = pList[0].ToCustomerId;
                            oVO.TowProfitsharingStatus = 0;
                            isprofit_sharing = 1;
                        }

                    }
                }
            }
            int OrderID = cBO.AddOrder(oVO);
            if (OrderID > 0)
            {
                OrderVO OrderVO = cBO.FindOrderById(OrderID);

                JsApiPay Ja = new JsApiPay();
                if (OrderVO.Cost < 0)
                {
                    return new ResultObject() { Flag = 0, Message = "价格错误", Result = null };
                }

                if (OrderVO.Cost == 0)
                {
                    OrderVO.Status = 1;
                    OrderVO.payAt = DateTime.Now;
                    cBO.UpdateOrder(OrderVO);
                    cBO.EstablishedBusinessCard(sVO.InfoID, pVO.PersonalID, OrderID, AppType);
                    //生效积分
                    if (OrderVO.IntegralID > 0)
                    {
                        IntegralVO IVO = new IntegralVO();
                        IVO.IntegralID = OrderVO.IntegralID;
                        IVO.Status = 1;
                        cBO.UpdateIntegral(IVO);
                    }

                    //扣除库存
                    cBO.StoreAmount(sVO.InfoID, CostID);

                    //发放vip
                    if (oVO.GiveShopVipID > 0)
                    {
                        cBO.AddShopVipPersonal(oVO.PersonalID, oVO.GiveShopVipID, oVO.GiveShopVipDay, OrderID);
                    }


                    //开通个人会员
                    if (sVO.InfoID == 905)
                    {
                        CardOrderVO cpVO = new CardOrderVO();
                        cpVO.CustomerId = customerId;
                        cpVO.OpenId = "";
                        cpVO.CreatedAt = DateTime.Now;
                        cpVO.Type = 2;
                        cpVO.Cost = oVO.Cost;
                        cpVO.OrderNO = "BusinessCardOrder_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ran.Next(10000, 99999);

                        cpVO.CardOrderID = 0;
                        cpVO.Status = 1;
                        cpVO.payAt = DateTime.Now;
                        int CardOrderID = CardBO.AddOrder(cpVO);

                        if (CardOrderID > 0)
                        {
                            //开通会员
                            CardBO.OpeningVIP(CardOrderID);
                        }
                    }

                    return new ResultObject() { Flag = 5, Message = "购买成功", Result = null };
                }


                string total_fee_1 = Convert.ToInt32((OrderVO.Cost * 100)).ToString();
                string NOTIFY_URL = "http://api.leliaomp.com/Pay/BusinessCard_Notify_Url.aspx";

                String costName = Regex.Replace(sVO.Title, @"\p{Cs}", "");

                int n = 30;//如果长度比需要的长度n小,返回原字符串
                string temp = string.Empty;
                if (System.Text.Encoding.Default.GetByteCount(costName) > n)
                {
                    int t = 0;
                    char[] q = costName.ToCharArray();
                    for (int i = 0; i < q.Length && t < n; i++)
                    {
                        if ((int)q[i] >= 0x4E00 && (int)q[i] <= 0x9FA5)//是否汉字
                        {
                            temp += q[i];
                            t += 2;
                        }
                        else
                        {
                            temp += q[i];
                            t++;
                        }
                    }
                    costName = temp + "...";
                }

                BusinessCardVO bVO = cBO.FindBusinessCardById(sVO.BusinessID);
                string BusinessName = bVO.BusinessName + "门店产品";
                if (System.Text.Encoding.Default.GetByteCount(BusinessName) > n)
                {
                    BusinessName = body;
                }

                //如果有个人二级商户，就用二级商户结账
                if (OrderVO.sub_mchid != "")
                {
                    NOTIFY_URL = "https://api.leliaomp.com/Pay/Ecommerce_Business_Notify_Url.aspx";
                    string description = costName;
                    bool profit_sharing = false;
                    if (OrderVO.SplitCost > 0) profit_sharing = true;

                    AppletsPayDataVO res = await eBO.GetPay(AppVO.AppId, OrderVO.sub_mchid, description, OrderVO.OrderNO, int.Parse(total_fee_1), OrderVO.OpenId, NOTIFY_URL, profit_sharing);
                    if (res == null)
                    {
                        return new ResultObject() { Flag = 0, Message = "支付失败，请咨询客服", Result = null };
                    }

                    JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                    string s = jsonSerializer.Serialize(res);
                    return new ResultObject() { Flag = 1, Message = "成功", Result = s };
                }

                WxPayData wp = Ja.GetUnifiedOrderResult(AppVO.AppId, OrderVO.OrderNO, OrderVO.OpenId, total_fee_1, BusinessName, costName, body, NOTIFY_URL, AppType, isprofit_sharing);

                if (wp != null)
                {
                    string reslut = Ja.GetJsApiParameters(wp, AppType);
                    if (reslut != "")
                    {
                        return new ResultObject() { Flag = 1, Message = "成功", Result = reslut };
                    }
                    else
                    {
                        return new ResultObject() { Flag = 0, Message = "失败", Result = null };
                    }
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "失败", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "操作失败，请重试!", Result = null };
            }
        }

        /// <summary>
        /// 测试分账
        /// </summary>
        /// <param name="SortID"></param>
        /// <returns></returns>
        [Route("GetProfitsharing"), HttpGet, Anonymous]
        public ResultObject GetProfitsharing()
        {
            JsApiPay Ja = new JsApiPay();
            //bool wp = Ja.GetProfitsharingResult("oMfC94jD6fLUWq5xslq8IiLE6Duo", 13, "测试", "4200001613202209030097332164", "2022090310212190659398", 0);
            //WxPayData wp = Ja.GetProfitsharingResult("oMfC94jD6fLUWq5xslq8IiLE6Duo", 0);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = null };

        }


        /// <summary>
        /// 获取订单信息(使用物品)
        /// </summary>
        /// <param name="SortID"></param>
        /// <returns></returns>
        [Route("GetOrder"), HttpGet]
        public ResultObject GetOrder(string token, int OrderID)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            OrderViewVO OrderVO = cBO.FindOrderViewById(OrderID);

            if (OrderVO != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = OrderVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }


        /// <summary>
        /// 获取订单信息(根据OrderID)
        /// </summary>
        /// <param name="SortID"></param>
        /// <returns></returns>
        [Route("GetOrderById"), HttpGet , Anonymous]
        public ResultObject GetOrderById(int OrderID)
        {
            try
            {
                BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
                OrderViewVO OrderVO = cBO.FindOrderViewById(OrderID);

                if (OrderVO != null)
                {
                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = OrderVO };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
                }
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = -1, Message = "获取失败!", Result = ex };
            }

        }

        /// <summary>
        /// 获取订单信息(核销)
        /// </summary>
        /// <param name="SortID"></param>
        /// <returns></returns>
        [Route("GetOrderView"), HttpGet]
        public ResultObject GetOrderView(string token, int OrderID, int AppType = 0)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            OrderViewVO OrderVO = cBO.FindOrderViewById(OrderID);
            AppVO AppVO = AppBO.GetApp(AppType);
            if (OrderVO != null)
            {
                bool isAdmin = false;

                if (cBO.isJoinBusiness(pVO, OrderVO.ProdustsBusinessID))
                {
                    isAdmin = true;
                    try
                    {
                        PersonalVO RpVO = cBO.FindPersonalByCustomerId(OrderVO.PeopleRebateCid);
                        if (RpVO != null)
                        {
                            OrderVO.PeopleRebateName = RpVO.Name;
                        }
                        PersonalVO RpVO1 = cBO.FindPersonalByCustomerId(OrderVO.RebateCid1);
                        if (RpVO1 != null)
                        {
                            OrderVO.RebateCid1Name = RpVO1.Name;
                        }
                        PersonalVO RpVO2 = cBO.FindPersonalByCustomerId(OrderVO.RebateCid2);
                        if (RpVO2 != null)
                        {
                            OrderVO.RebateCid2Name = RpVO2.Name;
                        }
                        PersonalVO RpVO3 = cBO.FindPersonalByCustomerId(OrderVO.RebateCid3);
                        if (RpVO3 != null)
                        {
                            OrderVO.RebateCid3Name = RpVO3.Name;
                        }
                        PersonalVO RpVO4 = cBO.FindPersonalByCustomerId(OrderVO.RebateCid4);
                        if (RpVO4 != null)
                        {
                            OrderVO.RebateCid4Name = RpVO4.Name;
                        }

                        PersonalVO RpVO5 = cBO.FindPersonalByCustomerId(OrderVO.ProfitsharingCid);
                        if (RpVO5 != null)
                        {
                            OrderVO.ProfitsharingName = RpVO5.Name;
                        }
                        PersonalVO RpVO6 = cBO.FindPersonalByCustomerId(OrderVO.TowProfitsharingCid);
                        if (RpVO6 != null)
                        {
                            OrderVO.TowProfitsharingName = RpVO6.Name;
                        }
                    }
                    catch
                    {

                    }
                }
                try
                {
                    if (OrderVO.QRImg == "")
                    {
                        OrderVO.QRImg = cBO.GetOrderQRImg(OrderVO.OrderID, AppType);
                    }
                }
                catch
                {

                }

                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = OrderVO, Subsidiary = isAdmin };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取订单信息(退款)
        /// </summary>
        /// <param name="SortID"></param>
        /// <returns></returns>
        [Route("GetOrderRefundResult"), HttpGet]
        public ResultObject GetOrderRefundResult(string token, int OrderID, string TestPass, int AppType = 0)
        {
            if (TestPass != "leliao2020")
            {
                return new ResultObject() { Flag = 0, Message = "退款密码错误", Result = null };
            }
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            AppVO AppVO = AppBO.GetApp(AppType);
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            OrderViewVO OrderVO = cBO.FindOrderViewById(OrderID);

            if (OrderVO != null)
            {
                if (OrderVO.Status != 1)
                {
                    return new ResultObject() { Flag = 0, Message = "订单未支付或已退款", Result = null };
                }
                if (OrderVO.isAgentBuy == 1)
                {
                    return new ResultObject() { Flag = 0, Message = "该订单是保证金支付订单，请与商家协商退款", Result = null };
                }
                if (OrderVO.isEcommerceBuy == 1)
                {
                    return new ResultObject() { Flag = 0, Message = "该订单被商家提现，请与商家协商退款", Result = null };
                }
                if (OrderVO.isWriteOff == 1)
                {
                    return new ResultObject() { Flag = 0, Message = "订单已核销，无法退款！", Result = null };
                }
                if (OrderVO.Cost <= 0)
                {
                    OrderVO oVO = new SPLibrary.BusinessCardManagement.VO.OrderVO();
                    oVO.OrderID = OrderVO.OrderID;
                    oVO.Status = -2;
                    cBO.UpdateOrder(oVO);
                    return new ResultObject() { Flag = 1, Message = "退款成功", Result = null };
                }
                if (!cBO.FindJurisdiction(pVO.PersonalID, OrderVO.ProdustsBusinessID, "Admin") && !cBO.FindJurisdiction(pVO.PersonalID, OrderVO.ProdustsBusinessID, "Order"))
                {
                    return new ResultObject() { Flag = 0, Message = "你没有权限进行退款!", Result = null };
                }
                if (OrderVO.RebateStatus1 == 1 || OrderVO.RebateStatus2 == 1 || OrderVO.RebateStatus3 == 1 || OrderVO.RebateStatus4 == 1 || OrderVO.PeopleRebateStatus == 1 || OrderVO.ProfitsharingStatus == 1)
                {
                    return new ResultObject() { Flag = 0, Message = "该订单的返佣已被提现，无法进行退款!", Result = null };
                }
                if (OrderVO.PayOutStatus == 1)
                {
                    if (!cBO.IsHasMoreBusinessBalance(OrderVO.ProdustsBusinessID, OrderVO.Cost))
                    {
                        return new ResultObject() { Flag = 0, Message = "商家余额不足！无法进行退款!", Result = null };
                    }
                }
                string total_fee_1 = Convert.ToInt32((OrderVO.Cost * 100)).ToString();

                JsApiPay Ja = new JsApiPay();
                WxPayData wp = Ja.GetRefundResult(OrderVO.OpenId, total_fee_1, total_fee_1, OrderVO.OrderNO, AppVO.AppType, AppVO.AppId);

                if (wp != null)
                {
                    string reslut = Ja.GetJsApiParameters(wp, AppVO.AppType);
                    if (reslut != "")
                    {
                        OrderVO oVO = new SPLibrary.BusinessCardManagement.VO.OrderVO();
                        oVO.OrderID = OrderVO.OrderID;
                        oVO.Status = -2;
                        cBO.UpdateOrder(oVO);

                        if (OrderVO.PayOutStatus == 1)
                        {
                            cBO.ReduceCardBalance(OrderVO.ProdustsBusinessID, OrderVO.Cost);
                            BalanceHistoryVO vo = new BalanceHistoryVO();
                            vo.Balance = -OrderVO.Cost;
                            vo.OrderID = OrderVO.OrderID + "|Refund";
                            vo.BusinessID = OrderVO.ProdustsBusinessID;
                            cBO.AddBalanceHistory(vo);
                        }

                        return new ResultObject() { Flag = 1, Message = "退款成功", Result = null };
                    }
                    else
                    {
                        return new ResultObject() { Flag = 0, Message = "退款失败", Result = null };
                    }
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "退款失败", Result = null };
                }

                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = OrderVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 拼团失败订单退款
        /// </summary>
        /// <param name="SortID"></param>
        /// <returns></returns>
        [Route("GetGroupBuyOrderRefundResult"), HttpGet, Anonymous]
        public ResultObject GetGroupBuyOrderRefundResult(string TestPass)
        {
            if (TestPass != "leliao2022")
            {
                return new ResultObject() { Flag = 0, Message = "退款密码错误", Result = null };
            }
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            List<OrderGroupBuyNotGroupViewVO> newOrderVO = cBO.FindOrderGroupBuyNotGroupViewList("Status=3 and  ExpireAt<now() and isGroupBuy=1 and isEcommerceBuy=0 and isWriteOff=0 and RebateStatus1=0  and RebateStatus2=0  and RebateStatus3=0  and RebateStatus4=0  and PeopleRebateStatus=0 ");

            foreach (OrderGroupBuyNotGroupViewVO OrderVO in newOrderVO)
            {
                if (OrderVO != null)
                {
                    if (OrderVO.Cost <= 0 || OrderVO.isAgentBuy == 1)
                    {
                        OrderVO oVO = new SPLibrary.BusinessCardManagement.VO.OrderVO();
                        oVO.OrderID = OrderVO.OrderID;
                        oVO.Status = -2;
                        cBO.UpdateOrder(oVO);

                        if (OrderVO.isAgentBuy == 1)
                        {
                            //返还保证金
                            AgentIntegralVO iVO = new AgentIntegralVO();
                            iVO.PersonalID = OrderVO.PersonalID;
                            iVO.OperPersonalID = 0;
                            iVO.Status = 1;
                            iVO.OrderID = OrderVO.OrderID;
                            iVO.CreatedAt = DateTime.Now;
                            iVO.BusinessID = OrderVO.BusinessID;
                            iVO.Balance = OrderVO.Cost;
                            iVO.Note = "拼团失败自动退款";
                            cBO.AddAgentIntegral(iVO);
                        }

                        continue;
                    }
                    string total_fee_1 = Convert.ToInt32((OrderVO.Cost * 100)).ToString();

                    JsApiPay Ja = new JsApiPay();
                    AppVO AppVO = AppBO.GetApp(OrderVO.AppType);
                    try
                    {
                        WxPayData wp = Ja.GetRefundResult(OrderVO.OpenId, total_fee_1, total_fee_1, OrderVO.OrderNO, 1, AppVO.AppId);
                        if (wp != null)
                        {
                            string reslut = Ja.GetJsApiParameters(wp);
                            if (reslut != "")
                            {
                                OrderVO oVO = new SPLibrary.BusinessCardManagement.VO.OrderVO();
                                oVO.OrderID = OrderVO.OrderID;
                                oVO.Status = -2;
                                cBO.UpdateOrder(oVO);
                            }
                        }
                    }
                    catch
                    {

                    }

                }
            }
            return new ResultObject() { Flag = 1, Message = "退款成功", Result = newOrderVO.Count };

        }

        /// <summary>
        /// 获取核销二维码
        /// </summary>
        /// <param name="SortID"></param>
        /// <returns></returns>
        [Route("GetOrderQRImg"), HttpGet]
        public ResultObject GetOrderQRImg(string token, int OrderID, int AppType = 0)
        {
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            AppVO AppVO = AppBO.GetApp(AppType);
            string QRImg = "";
            try
            {
                QRImg = cBO.GetOrderQRImg(OrderID, AppType);
            }
            catch
            {
                QRImg = "";
            }

            if (QRImg != "")
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = QRImg };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }

        }

        /// <summary>
        /// 核销订单
        /// </summary>
        /// <param name="SortID"></param>
        /// <returns></returns>
        [Route("WriteOffOrder"), HttpGet]
        public ResultObject WriteOffOrder(string token, int OrderID)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            OrderViewVO OrderVO = cBO.FindOrderViewById(OrderID);

            if (OrderVO != null)
            {
                if (!cBO.FindJurisdiction(pVO.PersonalID, OrderVO.ProdustsBusinessID, "Admin") && !cBO.FindJurisdiction(pVO.PersonalID, OrderVO.ProdustsBusinessID, "Order"))
                {
                    return new ResultObject() { Flag = 0, Message = "你没有权限进行核销!", Result = null };
                }
                if (OrderVO.isWriteOff == 1)
                {
                    return new ResultObject() { Flag = 0, Message = "该订单已在“" + OrderVO.WriteOffAt.ToString("yyyy年MM月dd日HH时mm分") + "”核销过了!", Result = null };
                }
                OrderVO oVO = new SPLibrary.BusinessCardManagement.VO.OrderVO();
                oVO.OrderID = OrderVO.OrderID;
                oVO.isWriteOff = 1;
                oVO.WriteOffAt = DateTime.Now;
                oVO.WriteOffPersonalID = pVO.PersonalID;

                //请求分账
                new ProfitsharingToOrder().ProfitsharingToOrderID(OrderVO.OrderID);


                if (cBO.UpdateOrder(oVO))
                {
                    OrderViewVO newOrderVO = cBO.FindOrderViewById(OrderID);
                    try
                    {
                        PersonalVO RpVO = cBO.FindPersonalByCustomerId(OrderVO.PeopleRebateCid);
                        if (RpVO != null)
                        {
                            newOrderVO.PeopleRebateName = RpVO.Name;
                        }
                        PersonalVO RpVO1 = cBO.FindPersonalByCustomerId(OrderVO.RebateCid1);
                        if (RpVO1 != null)
                        {
                            newOrderVO.RebateCid1Name = RpVO1.Name;
                        }
                        PersonalVO RpVO2 = cBO.FindPersonalByCustomerId(OrderVO.RebateCid2);
                        if (RpVO2 != null)
                        {
                            newOrderVO.RebateCid2Name = RpVO2.Name;
                        }
                        PersonalVO RpVO3 = cBO.FindPersonalByCustomerId(OrderVO.RebateCid3);
                        if (RpVO3 != null)
                        {
                            newOrderVO.RebateCid3Name = RpVO3.Name;
                        }
                        PersonalVO RpVO4 = cBO.FindPersonalByCustomerId(OrderVO.RebateCid4);
                        if (RpVO4 != null)
                        {
                            newOrderVO.RebateCid4Name = RpVO4.Name;
                        }

                        PersonalVO RpVO5 = cBO.FindPersonalByCustomerId(OrderVO.ProfitsharingCid);
                        if (RpVO5 != null)
                        {
                            newOrderVO.ProfitsharingName = RpVO5.Name;
                        }
                    }
                    catch
                    {

                    }
                    return new ResultObject() { Flag = 1, Message = "核销成功!", Result = newOrderVO };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "核销失败，请重试!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "核销失败!", Result = null };
            }
        }


        /// <summary>
        /// 订单发货
        /// </summary>
        /// <param name="SortID"></param>
        /// <returns></returns>
        [Route("setDeliver"), HttpGet]
        public ResultObject setDeliver(string token, int OrderID, string LogisticsOrderNo, int AppType = 1)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            OrderViewVO OrderVO = cBO.FindOrderViewById(OrderID);

            if (OrderVO != null)
            {
                if (!cBO.FindJurisdiction(pVO.PersonalID, OrderVO.ProdustsBusinessID, "Admin") && !cBO.FindJurisdiction(pVO.PersonalID, OrderVO.ProdustsBusinessID, "Order"))
                {
                    return new ResultObject() { Flag = 0, Message = "你没有权限操作!", Result = null };
                }

                OrderVO oVO = new SPLibrary.BusinessCardManagement.VO.OrderVO();
                oVO.OrderID = OrderVO.OrderID;
                oVO.isDeliver = 1;
                oVO.DeliverAt = DateTime.Now;
                oVO.LogisticsOrderNo = LogisticsOrderNo;

                if (cBO.UpdateOrder(oVO))
                {
                    return new ResultObject() { Flag = 1, Message = "发货成功!", Result = null };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "发货失败，请重试!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "发货失败!", Result = null };
            }
        }

        /// <summary>
        /// 超过15天的订单统一分账
        /// </summary>
        /// <param name="SortID"></param>
        /// <returns></returns>
        [Route("Profitsharing"), HttpGet, Anonymous]
        public ResultObject Profitsharing()
        {
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());

            List<OrderVO> OrderList = cBO.FindOrderList("Status=1 and isAgentBuy=0 and isEcommerceBuy=0 and (ProfitsharingCost>0 or TowProfitsharingCost>0) and ProfitsharingOpenId<>'' and ProfitsharingStatus=0 and payAt < DATE_SUB(DATE(now()), INTERVAL 15 DAY)");
            for (int i = 0; i < OrderList.Count; i++)
            {
                //请求分账
                new ProfitsharingToOrder().ProfitsharingToOrderID(OrderList[i].OrderID);
            }
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = null };

        }

        /// <summary>
        /// 使用库存物品
        /// </summary>
        /// <param name="SortID"></param>
        /// <returns></returns>
        [Route("UseWarehouse"), HttpGet]
        public ResultObject UseWarehouse(string token, int OrderID, int isUsed = 1, int AppType = 1)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            AppVO AppVO = AppBO.GetApp(AppType);
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            OrderViewVO OrderVO = cBO.FindOrderViewById(OrderID);

            if (OrderVO.Status != 1)
            {
                return new ResultObject() { Flag = 0, Message = "该物品未完成支付，无法使用!", Result = null };
            }
            if (OrderVO.Status != 1 || OrderVO.isUsed == 1 || OrderVO.isUsed == 2)
            {
                return new ResultObject() { Flag = 0, Message = "该物品已被使用过，无法使用!", Result = null };
            }

            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);

            //判断能否续费
            if (pVO.BusinessID > 0 && OrderVO.OfficialProducts != "" && isUsed == 1)
            {
                BusinessCardVO bVO = cBO.FindBusinessCardById(pVO.BusinessID);
                int l1 = 0;
                if (OrderVO.OfficialProducts == "SelfEmployed")
                    l1 = 0;
                if (OrderVO.OfficialProducts == "Basic")
                    l1 = 1;
                if (OrderVO.OfficialProducts == "Standard")
                    l1 = 2;
                if (OrderVO.OfficialProducts == "Advanced")
                    l1 = 3;

                int l2 = 0;
                if (OrderVO.OfficialProducts == "SelfEmployed")
                    l2 = 0;
                if (bVO.OfficialProducts == "Basic")
                    l2 = 1;
                if (bVO.OfficialProducts == "Standard")
                    l2 = 2;
                if (bVO.OfficialProducts == "Advanced")
                    l2 = 3;

                if (l2 > l1)
                {
                    return new ResultObject() { Flag = 0, Message = "您的公司已开通了更高版本的企业名片，请选择对应版本进行续费!", Result = null };
                }
            }

            if (isUsed == 2)
            {
                OrderVO oVO = cBO.FindOrderById(OrderID);
                oVO.isUsed = 2;
                cBO.UpdateOrder(oVO);
            }

            if (cBO.EstablishedBusinessCard(OrderVO.InfoID, pVO.PersonalID, OrderVO.OrderID, AppType))
            {
                return new ResultObject() { Flag = 1, Message = "物品使用成功!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "物品使用失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取我的代理价
        /// </summary>
        /// <param name="SortID"></param>
        /// <returns></returns>
        [Route("GetAgentlevelCostByPersonalID"), HttpGet]
        public ResultObject GetAgentlevelCostByPersonalID(string token, int InfoID)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);

            decimal Cost = cBO.FindAgentlevelCostByPersonalID(pVO.PersonalID, InfoID);
            InfoVO sVO = cBO.FindInfoById(InfoID);

            decimal VipCost = sVO.Cost * cBO.GetVipDiscount(pVO.PersonalID, InfoID) / 100;

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = new { Cost = Cost, isAgent = Cost < sVO.Cost, VipCost, isVipCost = VipCost < sVO.Cost } };
        }

        /// <summary>
        /// 获取我的库存（买家订单）
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        [Route("GetOrderViewList"), HttpPost]
        public ResultObject GetOrderViewList([FromBody] ConditionModel condition, string token)
        {
            if (condition == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            else if (condition.Filter == null || condition.PageInfo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);

            string conditionStr = "PersonalID=" + pVO.PersonalID + " and (Status=1 or Status=3) and (" + condition.Filter.Result() + ")";
            Paging pageInfo = condition.PageInfo;

            List<OrderViewVO> list = new List<OrderViewVO>();
            int count = 0;

            list = cBO.FindOrderViewAllByPageIndex(conditionStr, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);
            count = cBO.FindOrderViewCount(conditionStr);

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list, Count = count };
        }

        /// <summary>
        /// 获取销售订单（卖家订单）
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        [Route("GetOrderViewListBySeller"), HttpPost]
        public ResultObject GetOrderViewListBySeller([FromBody] ConditionModel condition, string token)
        {
            if (condition == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            else if (condition.Filter == null || condition.PageInfo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);

            if (!cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Admin") && !cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Order"))
            {
                return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
            }

            string conditionStr = "ProdustsBusinessID=" + pVO.BusinessID + " and Status=1 and (" + condition.Filter.Result() + ")";
            Paging pageInfo = condition.PageInfo;

            List<OrderViewVO> list = new List<OrderViewVO>();
            int count = 0;

            list = cBO.FindOrderViewAllByPageIndex(conditionStr, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);
            count = cBO.FindOrderViewCount(conditionStr);

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list, Count = count };
        }

        /// <summary>
        /// 导出销售订单（Excel）
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        [Route("GetOrderExcel"), HttpGet]
        public ResultObject GetOrderExcel(string token, string StartTime = "", string EndTime = "")
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);

            if (!cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Admin") && !cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Order"))
            {
                return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
            }

            string conditionStr = "ProdustsBusinessID=" + pVO.BusinessID + " and Status=1";
            if (StartTime != "")
            {
                conditionStr += " and DATE_FORMAT(payAt,'%y-%m-%d') >= DATE_FORMAT('" + StartTime + "','%y-%m-%d')";
            }
            if (EndTime != "")
            {
                conditionStr += " and DATE_FORMAT(payAt,'%y-%m-%d') <= DATE_FORMAT('" + EndTime + "','%y-%m-%d')";
            }

            List<OrderViewVO> list = new List<OrderViewVO>();
            list = cBO.FindOrderViewList(conditionStr);

            if (list != null)
            {
                if (list.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("订单号", typeof(String));
                    dt.Columns.Add("姓名", typeof(String));
                    dt.Columns.Add("手机", typeof(String));
                    dt.Columns.Add("地址", typeof(String));
                    dt.Columns.Add("订单金额", typeof(Decimal));
                    dt.Columns.Add("购买折扣", typeof(String));
                    dt.Columns.Add("下单时间", typeof(String));
                    dt.Columns.Add("产品名称", typeof(String));
                    dt.Columns.Add("购买规格", typeof(String));
                    dt.Columns.Add("购买数量", typeof(String));
                    dt.Columns.Add("是否已核销", typeof(String));
                    dt.Columns.Add("核销时间", typeof(String));
                    dt.Columns.Add("核销人员", typeof(String));
                    dt.Columns.Add("订单二维码", typeof(String));
                    dt.Columns.Add("销售员", typeof(String));
                    dt.Columns.Add("所属拼团", typeof(String));
                    dt.Columns.Add("团长人头返现(名称)", typeof(String));
                    dt.Columns.Add("团长人头返现(金额)", typeof(Decimal));
                    dt.Columns.Add("团长人头返现(是否提现)", typeof(String));

                    dt.Columns.Add("折扣返现1号(名称)", typeof(String));
                    dt.Columns.Add("折扣返现1号(金额)", typeof(Decimal));
                    dt.Columns.Add("折扣返现1号(是否提现)", typeof(String));

                    dt.Columns.Add("折扣返现2号(名称)", typeof(String));
                    dt.Columns.Add("折扣返现2号(金额)", typeof(Decimal));
                    dt.Columns.Add("折扣返现2号(是否提现)", typeof(String));

                    dt.Columns.Add("折扣返现3号(名称)", typeof(String));
                    dt.Columns.Add("折扣返现3号(金额)", typeof(Decimal));
                    dt.Columns.Add("折扣返现3号(是否提现)", typeof(String));

                    dt.Columns.Add("折扣返现4号(名称)", typeof(String));
                    dt.Columns.Add("折扣返现4号(金额)", typeof(Decimal));
                    dt.Columns.Add("折扣返现4号(是否提现)", typeof(String));

                    dt.Columns.Add("推荐分成(名称)", typeof(String));
                    dt.Columns.Add("推荐分成(金额)", typeof(Decimal));
                    dt.Columns.Add("推荐分成(是否提现)", typeof(String));

                    dt.Columns.Add("二级分成(名称)", typeof(String));
                    dt.Columns.Add("二级分成(金额)", typeof(Decimal));
                    dt.Columns.Add("二级分成(是否提现)", typeof(String));

                    int j = 1;
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].Status > 0)
                        {
                            DataRow row = dt.NewRow();
                            row["订单号"] = list[i].OrderNO;

                            if (list[i].sName != "")
                            {
                                row["姓名"] = list[i].sName;
                            }
                            else
                            {
                                row["姓名"] = list[i].Name;
                            }

                            if (list[i].sPhone != "")
                            {
                                row["手机"] = list[i].sPhone;
                            }
                            else
                            {
                                row["手机"] = list[i].Phone;
                            }

                            row["地址"] = list[i].sAddress;
                            row["订单金额"] = list[i].Cost;
                            row["下单时间"] = list[i].payAt.ToString("yyyy-MM-dd HH:mm:ss");
                            row["产品名称"] = list[i].Title;
                            row["购买规格"] = list[i].CostName;
                            row["购买数量"] = list[i].Number;
                            row["销售员"] = list[i].AgentName;

                            if (list[i].isWriteOff == 1)
                            {
                                row["是否已核销"] = "是";
                                row["核销时间"] = list[i].WriteOffAt.ToString("yyyy-MM-dd HH:mm:ss");
                                PersonalVO hxVO = cBO.FindPersonalById(list[i].WriteOffPersonalID);
                                if (hxVO != null)
                                {
                                    row["核销人员"] = hxVO.Name;
                                }

                            }
                            else
                            {
                                row["核销时间"] = "";
                                row["是否已核销"] = "否";
                            }

                            row["订单二维码"] = list[i].QRImg;
                            row["购买折扣"] = list[i].Discount + "%";

                            if (list[i].GroupBuyID > 0)
                            {
                                GroupBuyVO gVO = cBO.FindGroupBuyById(list[i].GroupBuyID);
                                if (gVO != null)
                                {
                                    row["所属拼团"] = gVO.Name;
                                }
                            }
                            PersonalVO RpVO = cBO.FindPersonalByCustomerId(list[i].PeopleRebateCid);
                            if (RpVO != null)
                            {
                                list[i].PeopleRebateName = RpVO.Name;
                            }
                            PersonalVO RpVO1 = cBO.FindPersonalByCustomerId(list[i].RebateCid1);
                            if (RpVO1 != null)
                            {
                                list[i].RebateCid1Name = RpVO1.Name;
                            }
                            PersonalVO RpVO2 = cBO.FindPersonalByCustomerId(list[i].RebateCid2);
                            if (RpVO2 != null)
                            {
                                list[i].RebateCid2Name = RpVO2.Name;
                            }
                            PersonalVO RpVO3 = cBO.FindPersonalByCustomerId(list[i].RebateCid3);
                            if (RpVO3 != null)
                            {
                                list[i].RebateCid3Name = RpVO3.Name;
                            }
                            PersonalVO RpVO4 = cBO.FindPersonalByCustomerId(list[i].RebateCid4);
                            if (RpVO4 != null)
                            {
                                list[i].RebateCid4Name = RpVO4.Name;
                            }

                            PersonalVO Profitsharing = cBO.FindPersonalByCustomerId(list[i].ProfitsharingCid);
                            if (Profitsharing != null)
                            {
                                list[i].ProfitsharingName = Profitsharing.Name;
                            }

                            row["团长人头返现(名称)"] = list[i].PeopleRebateName;
                            row["团长人头返现(金额)"] = list[i].PeopleRebateCost;
                            row["团长人头返现(是否提现)"] = list[i].PeopleRebateStatus == 1 ? "是" : "否";

                            row["折扣返现1号(名称)"] = list[i].RebateCid1Name;
                            row["折扣返现1号(金额)"] = list[i].RebateCost1;
                            row["折扣返现1号(是否提现)"] = list[i].RebateStatus1 == 1 ? "是" : "否";

                            row["折扣返现2号(名称)"] = list[i].RebateCid2Name;
                            row["折扣返现2号(金额)"] = list[i].RebateCost2;
                            row["折扣返现2号(是否提现)"] = list[i].RebateStatus2 == 1 ? "是" : "否";

                            row["折扣返现3号(名称)"] = list[i].RebateCid3Name;
                            row["折扣返现3号(金额)"] = list[i].RebateCost3;
                            row["折扣返现3号(是否提现)"] = list[i].RebateStatus3 == 1 ? "是" : "否";

                            row["折扣返现4号(名称)"] = list[i].RebateCid4Name;
                            row["折扣返现4号(金额)"] = list[i].RebateCost4;
                            row["折扣返现4号(是否提现)"] = list[i].RebateStatus4 == 1 ? "是" : "否";

                            row["推荐分成(名称)"] = list[i].ProfitsharingName;
                            row["推荐分成(金额)"] = list[i].ProfitsharingCost;
                            row["推荐分成(是否提现)"] = list[i].ProfitsharingStatus == 1 ? "是" : "否";

                            row["二级分成(名称)"] = list[i].TowProfitsharingName;
                            row["二级分成(金额)"] = list[i].TowProfitsharingCost;
                            row["二级分成(是否提现)"] = list[i].TowProfitsharingStatus == 1 ? "是" : "否";

                            dt.Rows.Add(row);
                            j++;
                        }
                    }

                    string FileName = EPPlus.DataToExcel(dt, "BcOrderExcel/", DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".xlsx");
                    if (FileName != null)
                    {
                        return new ResultObject() { Flag = 1, Message = "获取成功!", Result = FileName };
                    }
                    else
                    {
                        return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
                    }
                }
                else
                {
                    return new ResultObject() { Flag = 2, Message = "暂无订单!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取海报背景
        /// </summary>
        /// <returns></returns>
        [Route("GetPosterback"), HttpGet, Anonymous]
        public ResultObject GetPosterback(int PageCount = 50, int PageIndex = 1)
        {
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            List<FileVO> FileVO = cBO.GetPosterback(PageCount, PageIndex);
            List<string> FileList = new List<string>();
            foreach (FileVO item in FileVO)
            {
                FileList.Add(item.Url);
            }
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = FileList, Count = cBO.GetPosterbackNum() };
        }

        /// <summary>
        /// 获取我的其他绑定企业
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getMySecondBusiness"), HttpGet]
        public ResultObject getMySecondBusiness(string token)
        {
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            try
            {
                PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
                BusinessCardVO bVO = new BusinessCardVO();

                if (pVO.BusinessID == 0)
                {
                    bVO = null;
                }
                else
                {
                    bVO = cBO.FindBusinessCardById(pVO.BusinessID);
                }
                //清除营业执照等保密信息
                bVO.BusinessLicenseImg = "";

                List<SecondBusinessViewVO> ListSecondBusiness = cBO.FindSecondBusinessViewByPersonalID(pVO.PersonalID);
                if (ListSecondBusiness != null)
                {
                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = new { ListSecondBusiness = ListSecondBusiness, BusinessCard = bVO } };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
                }
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = ex };
            }

        }

        /// <summary>
        /// 切换绑定企业
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("ChangeBusiness"), HttpGet]
        public ResultObject ChangeBusiness(string token, int BusinessID, int AppType = 0)
        {
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            try
            {
                PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
                List<SecondBusinessVO> Lsb = cBO.FindSecondBusinessByPersonalID(pVO.PersonalID, BusinessID);
                if (Lsb.Count <= 0)
                {
                    return new ResultObject() { Flag = 0, Message = "切换失败!", Result = null };
                }

                //将原公司添加进附属列表
                if (cBO.FindSecondBusinessByPersonalID(pVO.PersonalID, pVO.BusinessID).Count <= 0)
                {
                    //添加绑定
                    SecondBusinessVO sVO = new SecondBusinessVO();
                    sVO.PersonalID = pVO.PersonalID;
                    sVO.BusinessID = pVO.BusinessID;
                    sVO.DepartmentID = pVO.DepartmentID;
                    sVO.Position = pVO.Position;
                    sVO.isExternal = pVO.isExternal;
                    cBO.AddSecondBusiness(sVO);
                }
                //将新公司从附属列表中删除
                cBO.DeleteSecondBusinessById(pVO.PersonalID, Lsb[0].BusinessID);

                pVO.BusinessID = Lsb[0].BusinessID;
                pVO.DepartmentID = Lsb[0].DepartmentID;
                pVO.Position = Lsb[0].Position;
                pVO.isExternal = Lsb[0].isExternal;
                if (cBO.UpdatePersonal(pVO))
                {
                    pVO.QRimg = cBO.GetQRImgByHeadimg(pVO.PersonalID, AppType);
                    pVO.PosterImg3 = cBO.GetPosterCardIMG(pVO.PersonalID, pVO.BusinessID);
                    BusinessCardVO bVO = new BusinessCardVO();

                    if (BusinessID == 0)
                    {
                        bVO = null;
                    }
                    else
                    {
                        bVO = cBO.FindBusinessCardById(BusinessID);
                    }
                    //清除营业执照等保密信息
                    bVO.BusinessLicenseImg = "";
                    List<SecondBusinessViewVO> ListSecondBusiness = cBO.FindSecondBusinessViewByPersonalID(pVO.PersonalID);
                    return new ResultObject() { Flag = 1, Message = "切换成功!", Result = new { ListSecondBusiness = ListSecondBusiness, BusinessCard = bVO } };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "切换失败!", Result = null };
                }
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "切换失败!", Result = ex };
            }

        }

        /// <summary>
        /// 上传视频
        /// </summary>
        /// <returns></returns>
        [Route("UploadVideo"), HttpPost]
        public ResultObject UploadVideo(int InfoID, string token, int duration)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            InfoVO InfoVO = cBO.FindInfoById(InfoID);

            if (duration > 300)
            {
                return new ResultObject() { Flag = 0, Message = "最大只能上传5分钟长度的视频!", Result = null };
            }

            if (InfoVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
            }

            if (!cBO.FindJurisdiction(pVO.PersonalID, InfoVO.BusinessID, "Web") && !cBO.FindJurisdiction(pVO.PersonalID, InfoVO.BusinessID, "Admin"))
            {
                return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
            }

            HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
            string folder = "/UploadFolder/Video/" + DateTime.Now.ToString("yyyyMM") + "/";
            string imgPath = "";
            if (hfc.Count > 0)
            {
                try
                {
                    FileInfo fi = new FileInfo(hfc[0].FileName);

                    string ext = fi.Extension.ToLower();
                    if (ext.IndexOf("asp") > -1 || ext.IndexOf("aspx") > -1 || ext.IndexOf("php") > -1)
                    {
                        return new ResultObject() { Flag = 0, Message = "文件类型错误", Result = null };
                    }

                    string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + fi.Extension;

                    //本地路径
                    string localPath = ConfigInfo.Instance.UploadFolder + folder;
                    if (!Directory.Exists(localPath))
                    {
                        Directory.CreateDirectory(localPath);
                    }
                    string PhysicalPath = localPath + newFileName;
                    imgPath = "~" + folder + newFileName;
                    hfc[0].SaveAs(PhysicalPath);

                    /*
                    string ThumbnailImg = "";
                    try
                    {
                        //封面路径
                        string ThumbnailImgfolder = "/UploadFolder/VideoThumbnail/";
                        string ThumbnailImglocalPath = ConfigInfo.Instance.UploadFolder + ThumbnailImgfolder;
                        string ThumbnailImgFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".jpg";

                        //截取封面
                        Thumbnail Thumbnail = new Thumbnail();
                        Thumbnail.GenerateThumbnail(PhysicalPath, ThumbnailImglocalPath + ThumbnailImgFileName);
                        ThumbnailImg = ConfigInfo.Instance.APIURL + ThumbnailImgfolder + ThumbnailImgFileName;
                    }
                    catch
                    {

                    }
                    */

                    //网络路径
                    string url = ConfigInfo.Instance.APIURL + folder + newFileName;

                    try
                    {//删除旧视频
                        if (InfoVO.Video != "")
                        {
                            string FilePath = InfoVO.Video;
                            FilePath = FilePath.Replace(ConfigInfo.Instance.APIURL, "");
                            FilePath = ConfigInfo.Instance.UploadFolder + FilePath;
                            File.Delete(FilePath);
                        }
                    }
                    catch
                    {

                    }



                    //保存连接
                    InfoVO.Video = url;
                    InfoVO.Duration = duration;
                    cBO.UpdateInfo(InfoVO);

                    return new ResultObject() { Flag = 1, Message = "上传成功", Result = url, Subsidiary = null };
                }
                catch (Exception er)
                {
                    return new ResultObject() { Flag = 0, Message = "上传失败", Result = er.Message };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "上传失败", Result = "文件为空" };
            }
        }

        /// <summary>
        /// 选择展示视频
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("ChooseShowVideo"), HttpGet]
        public ResultObject ChooseShowVideo(string token, int InfoID, int SortID)
        {
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);

            if (pVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
            }

            if (!cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Web") && !cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Admin"))
            {
                return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
            }

            InfoVO InfoVO = cBO.FindInfoById(InfoID);
            InfoSortVO InfoSortVO = cBO.FindInfoSortById(SortID);

            if (InfoVO == null || InfoSortVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "选择视频失败!", Result = null };
            }

            if (InfoVO.Video == "")
            {
                return new ResultObject() { Flag = 0, Message = "该视频未上传，请选择其他视频!", Result = null };
            }

            InfoSortVO.Content = InfoVO.Video;
            if (cBO.UpdateInfoSort(InfoSortVO))
            {
                return new ResultObject() { Flag = 1, Message = "更改视频成功!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "选择视频失败!", Result = null };
            }
        }


        /// <summary>
        /// 获取分店列表
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        [Route("GetSubsidiaryList"), HttpPost]
        public ResultObject GetSubsidiaryList([FromBody] ConditionModel condition, string token)
        {
            if (condition == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            else if (condition.Filter == null || condition.PageInfo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);

            if (pVO == null || pVO.BusinessID == 0)
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }

            string conditionStr = "HeadquartersID = " + pVO.BusinessID + " and (" + condition.Filter.Result() + ")";
            Paging pageInfo = condition.PageInfo;
            List<BusinessCardViewVO> list = cBO.FindAllByPageIndexByBusinessCard(conditionStr, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);
            int count = cBO.FindBusinessCardTotalCount(conditionStr);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list, Count = count };
        }

        /// <summary>
        /// 添加或更新分店
        /// </summary>
        /// <param name="VO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateSubsidiary"), HttpPost]
        public ResultObject UpdateSubsidiary([FromBody] BusinessCardVO BusinessCardVO, string token)
        {
            if (BusinessCardVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);

            /*审核文本是否合法*/
            /*
            if (!cBO.msg_sec_check(BusinessCardVO))
            {
                return new ResultObject() { Flag = 0, Message = "有非法关键词，请重新填写!", Result = null };
            }*/
            /*审核文本是否合法*/

            if (!cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Admin"))
            {
                return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
            }
            BusinessCardVO bVO = cBO.FindBusinessCardById(pVO.BusinessID);

            if (BusinessCardVO.BusinessID > 0)
            {
                BusinessCardVO sVO = cBO.FindBusinessCardById(BusinessCardVO.BusinessID);

                if (bVO == null || sVO.HeadquartersID != bVO.BusinessID)
                {
                    return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
                }

                sVO.BusinessName = BusinessCardVO.BusinessName;
                sVO.LogoImg = BusinessCardVO.LogoImg;
                sVO.Industry = BusinessCardVO.Industry;
                sVO.BusinessLicenseImg = BusinessCardVO.BusinessLicenseImg;

                if (cBO.UpdateBusinessCard(sVO))
                {
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = sVO.BusinessID };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }
            }
            else
            {
                if (bVO == null || bVO.isGroup != 1)
                {
                    return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
                }

                if (cBO.FindBusinessCardByHeadquartersID(bVO.BusinessID).Count >= BusinessCardVO.SubsidiarySum)
                {
                    return new ResultObject() { Flag = 0, Message = "分店数量已满!", Result = null };
                }

                BusinessCardVO.OfficialProducts = bVO.OfficialProducts;
                BusinessCardVO.Number = bVO.Number;
                BusinessCardVO.ExpirationAt = bVO.ExpirationAt;
                BusinessCardVO.CreatedAt = DateTime.Now;
                BusinessCardVO.HeadquartersID = bVO.BusinessID;

                int BusinessID = cBO.AddBusinessCard(BusinessCardVO);
                if (BusinessID > 0)
                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = BusinessID };
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
        }

        /// <summary>
        /// 申请创建并加入分店
        /// </summary>
        /// <param name="VO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateSubsidiary"), HttpPost]
        public ResultObject UpdateSubsidiary([FromBody] BusinessCardVO BusinessCardVO, int BusinessID, string token, int AppType = 0)
        {
            if (BusinessCardVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            BusinessCardVO bVO = cBO.FindBusinessCardById(BusinessID);
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);

            if (bVO == null || pVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            if (bVO.isGroup != 1)
            {
                return new ResultObject() { Flag = 0, Message = "当前申请公司非集团版名片!", Result = null };
            }

            if (cBO.FindBusinessCardByHeadquartersID(bVO.BusinessID).Count >= BusinessCardVO.SubsidiarySum)
            {
                return new ResultObject() { Flag = 0, Message = "分店数量已满!", Result = null };
            }

            BusinessCardVO.OfficialProducts = bVO.OfficialProducts;
            BusinessCardVO.Number = bVO.Number;
            BusinessCardVO.ExpirationAt = bVO.ExpirationAt;
            BusinessCardVO.CreatedAt = DateTime.Now;
            BusinessCardVO.HeadquartersID = bVO.BusinessID;



            int BusinessID2 = cBO.AddBusinessCard(BusinessCardVO);
            if (BusinessID2 > 0)
            {
                cBO.JoinBusiness(pVO, BusinessID2, AppType);
                return new ResultObject() { Flag = 1, Message = "添加成功!", Result = BusinessID2 };
            }
            else
                return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
        }

        /// <summary>
        /// 删除分店
        /// </summary>
        /// <param name="VO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DelSubsidiary"), HttpGet]
        public ResultObject DelSubsidiary(int BusinessID, string token, int AppType = 0)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);

            if (!cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Admin"))
            {
                return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
            }
            BusinessCardVO bVO = cBO.FindBusinessCardById(pVO.BusinessID);

            if (BusinessID > 0)
            {
                BusinessCardVO sVO = cBO.FindBusinessCardById(BusinessID);

                if (bVO == null || sVO == null || sVO.HeadquartersID != bVO.BusinessID)
                {
                    return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
                }

                //获取所有成员
                List<PersonalViewVO> Personal = cBO.FindPersonalViewByBusinessID(sVO.BusinessID);
                for (int i = 0; i < Personal.Count; i++)
                {
                    try
                    {
                        //删除成员

                        //清除所有权限
                        cBO.DeleteJurisdiction(Personal[i].PersonalID, sVO.BusinessID);

                        if (sVO.BusinessID == Personal[i].BusinessID)
                        {
                            //如果有其他附属公司
                            List<SecondBusinessVO> SecondBusinessVO = cBO.FindSecondBusinessByPersonalID(Personal[i].PersonalID);
                            if (SecondBusinessVO.Count > 0 && SecondBusinessVO[0].BusinessID != sVO.BusinessID)
                            {
                                Personal[i].BusinessID = SecondBusinessVO[0].BusinessID;
                                Personal[i].DepartmentID = SecondBusinessVO[0].DepartmentID;
                                Personal[i].Position = SecondBusinessVO[0].Position;
                                Personal[i].isExternal = SecondBusinessVO[0].isExternal;
                                cBO.DeleteSecondBusinessById(SecondBusinessVO[0].PersonalID, SecondBusinessVO[0].BusinessID);
                            }
                            else
                            {
                                Personal[i].BusinessID = 0;
                                Personal[i].DepartmentID = 0;
                            }

                            PersonalVO PersonalVO = new PersonalVO();
                            PersonalVO.PersonalID = Personal[i].PersonalID;
                            PersonalVO.BusinessID = Personal[i].BusinessID;
                            PersonalVO.DepartmentID = Personal[i].DepartmentID;
                            PersonalVO.Position = Personal[i].Position;
                            PersonalVO.isExternal = Personal[i].isExternal;
                            cBO.UpdatePersonal(PersonalVO);
                            PersonalVO.QRimg = cBO.GetQRImgByHeadimg(PersonalVO.PersonalID, AppType);
                        }
                        else
                        {
                            //删除绑定
                            cBO.DeleteSecondBusinessById(Personal[i].PersonalID, sVO.BusinessID);
                        }
                    }
                    catch
                    {

                    }
                }

                sVO.Status = 0;
                sVO.HeadquartersID = 0;

                if (cBO.UpdateBusinessCard(sVO))
                {
                    return new ResultObject() { Flag = 1, Message = "删除成功!", Result = null };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
                }

            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取公司信息
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getBusiness"), HttpGet]
        public ResultObject getBusiness(int BusinessID, string token)
        {
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            BusinessCardVO bVO = cBO.FindBusinessCardById(BusinessID);
            if (bVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            if (bVO != null)
            {
                List<PersonalViewVO> Personal = cBO.FindPersonalViewByBusinessID(bVO.BusinessID);
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = new { BusinessCard = bVO, Personal = Personal } };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("SubscriptionMessage"), HttpGet]
        public ResultObject SubscriptionMessage(string OpenId, string token)
        {
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            if (cBO.AddSubscription(customerId, OpenId) > 0)
            {
                return new ResultObject() { Flag = 1, Message = "订阅成功!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "订阅失败!", Result = null };
            }
        }

        /// <summary>
        /// 企业微信回调
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("qywxURL"), HttpGet, Anonymous]
        public string qywxURL(string msg_signature, string timestamp, string nonce, string echostr)
        {
            string CorpID = "ww05d52e294e33edd5";
            string Token = "hEi74AmmoqTpxaPxcXUICpLb";
            string EncodingAESKey = "WCf6E2tkbMnuDyg4cCsgIyrIUXfYhTU5u5eHnkFpDLh";
            /**url中的签名**/
            WXBizMsgCrypt wxcpt = new WXBizMsgCrypt(Token, EncodingAESKey, CorpID);
            string sEchoStr = "";
            int r = wxcpt.VerifyURL(msg_signature, timestamp, nonce, echostr, ref sEchoStr);
            if (r == 0)
            {
                return sEchoStr;
            }
            else
            {
                return r + "";
            }

        }

        /// <summary>
        /// 添加或更新任务
        /// </summary>
        /// <param name="VO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateRequire"), HttpPost]
        public ResultObject UpdateRequire([FromBody] RequireModel requireModelVO, string token)
        {
            if (requireModelVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            RequireBO uBO = new RequireBO(new CustomerProfile());
            BusinessBO bBO = new BusinessBO(new CustomerProfile());

            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);

            RequirementVO RequirementVO = requireModelVO.Requirement;
            List<RequirementTargetCategoryVO> requireCategoryVOList = requireModelVO.RequireCategory;
            List<RequirementTargetCityVO> requireCityVOList = requireModelVO.RequireCity;
            List<RequirementFileVO> requireFileVOList = requireModelVO.RequireFile;
            List<RequirementTargetClientVO> requireClientVOList = requireModelVO.RequireClient;

            if (RequirementVO.CityId < 1)
                RequirementVO.SetValue("CityId", DBNull.Value);
            if (RequirementVO.CategoryId < 1)
                RequirementVO.SetValue("CategoryId", DBNull.Value);
            if (RequirementVO.RequirementId <= 0)
            {
                RequirementVO.CreatedAt = DateTime.Now;
                RequirementVO.PublishAt = DateTime.Now;
                RequirementVO.RequirementCode = uBO.GetRequireCode();
                RequirementVO.Status = 1;
                RequirementVO.CustomerId = customerId;
                RequirementVO.CommissionType = 1;
                RequirementVO.Commission = RequirementVO.Cost;

                if (bBO.FindBusinessByCustomerId(customerId) == null)
                {
                    //添加雇主
                    BusinessVO businessVO = new BusinessVO();
                    List<BusinessCategoryVO> businessCategoryVOList = new List<BusinessCategoryVO>();
                    List<TargetCategoryVO> targetCategoryVOList = new List<TargetCategoryVO>();
                    List<TargetCityVO> targetCityVOList = new List<TargetCityVO>();
                    List<BusinessIdcardVO> businessIdcardVOList = new List<BusinessIdcardVO>();

                    businessVO.CustomerId = customerId;
                    businessVO.CreatedAt = DateTime.Now;
                    businessVO.BusinessId = 0;

                    BusinessCardVO bVO = cBO.FindBusinessCardById(pVO.BusinessID);
                    if (bVO != null)
                    {
                        businessVO.CompanyLogo = bVO.LogoImg;
                        businessVO.Address = bVO.Address;
                        businessVO.CompanyName = bVO.BusinessName;
                        businessVO.BusinessLicenseImg = bVO.BusinessLicenseImg;
                        businessVO.CompanyTel = bVO.Tel;
                        businessVO.SetupDate = bVO.CreatedAt;
                        businessVO.RealNameStatus = 1;
                        businessVO.Status = 1;
                        int businessId = bBO.Add(businessVO, businessCategoryVOList, targetCategoryVOList, targetCityVOList, businessIdcardVOList);
                    }
                    else
                    {
                        businessVO.CompanyLogo = pVO.Headimg;
                        businessVO.Address = pVO.Address;
                        businessVO.CompanyName = pVO.Name;
                        businessVO.CompanyTel = pVO.Phone;
                        businessVO.SetupDate = pVO.CreatedAt;
                        businessVO.RealNameStatus = 1;
                        businessVO.Status = 1;
                        int businessId = bBO.Add(businessVO, businessCategoryVOList, targetCategoryVOList, targetCityVOList, businessIdcardVOList);
                    }
                }

                int requireId = uBO.AddRequirement(RequirementVO, requireCategoryVOList, requireCityVOList, requireFileVOList, requireClientVOList);
                if (requireId > 0)
                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = requireId };
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
            else
            {
                bool isSuccess = uBO.UpdateRequirement(RequirementVO, requireCategoryVOList, requireCityVOList, requireFileVOList, requireClientVOList);
                if (isSuccess)
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = RequirementVO.RequirementId };
                else
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }
        }

        /// <summary>
        /// 添加或更新分享记录
        /// </summary>
        /// <param name="ShareVO"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateShare"), HttpPost]
        public ResultObject UpdateShare([FromBody] ShareVO ShareVO, string token, string code = "", int AppType = 0)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            if (ShareVO.ShareID > 0)
            {
                if (cBO.UpdateShare(ShareVO))
                {
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = ShareVO.ShareID };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }
            }
            else
            {
                ShareVO.CreatedAt = DateTime.Now;
                ShareVO.PersonalID = pVO.PersonalID;
                if (code != "")
                {
                    string OpenId = cBO.getOpenId(code, AppType);
                    ShareVO.OpenId = OpenId;
                }
                AppVO AppVO = AppBO.GetApp(AppType);
                ShareVO.AppType = AppVO.AppType;
                if (!ShareVO.SendOrReceive)
                {
                    List<ShareVO> sVO = cBO.FindShareList("Code='" + ShareVO.Code + "' and SendOrReceive=1");
                    if (sVO.Count > 0)
                    {
                        ShareVO.ToPersonalID = sVO[0].PersonalID;
                    }
                }

                int ShareID = cBO.AddShare(ShareVO);
                if (ShareID > 0)
                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = ShareID };
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取我的页面数据
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetMyData"), HttpGet]
        public ResultObject GetMyData(int TimeType, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);

            MyDataVO myVO = cBO.GetMyData(pVO.PersonalID, pVO.BusinessID, TimeType);

            BusinessCard_JurisdictionVO B_Jurisdiction = cBO.getBusinessCard_Jurisdiction(pVO.BusinessID);

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = myVO, Subsidiary = B_Jurisdiction };
        }

        /// <summary>
        /// 获取销售数据
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetMyData"), HttpGet]
        public ResultObject GetMyData(int PersonalID, int TimeType, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalById(PersonalID);

            MyDataVO myVO = cBO.GetMyData(pVO.PersonalID, pVO.BusinessID, TimeType);

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = myVO, Subsidiary = pVO };
        }


        /// <summary>
        /// 添加或更新销售目标
        /// </summary>
        /// <param name="ShareVO"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateTarget"), HttpPost]
        public ResultObject UpdateTarget([FromBody] TargetVO TargetVO, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            if (TargetVO.TargetID > 0)
            {
                if (cBO.UpdateTarget(TargetVO))
                {
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = TargetVO.TargetID };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }
            }
            else
            {
                TargetVO.CreatedAt = DateTime.Now;
                TargetVO.BusinessID = pVO.BusinessID;

                int TargetID = cBO.AddTarget(TargetVO);
                if (TargetID > 0)
                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = TargetID };
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取当月销售目标
        /// </summary>
        /// <param name="DepartmentID"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getTarget"), HttpGet]
        public ResultObject getTarget(int DepartmentID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            DateTime dt = DateTime.Now;

            SPLibrary.BusinessCardManagement.VO.DepartmentVO dVO = cBO.FindDepartmentById(DepartmentID);
            if (dVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
            }

            List<TargetVO> tVO = cBO.FindTargetByCondtion("Type='Department' and Year=" + dt.Year + " and Month=" + dt.Month + " and DepartmentID=" + DepartmentID);

            TargetVO DepartmentTVO = new TargetVO();
            //如果当月没有设置目标，自动创建
            if (tVO.Count == 0)
            {
                DepartmentTVO.DepartmentID = DepartmentID;
                DepartmentTVO.BusinessID = pVO.BusinessID;
                DepartmentTVO.CreatedAt = DateTime.Now;

                DateTime dt2 = dt.AddMonths(-1);
                List<TargetVO> tVO2 = cBO.FindTargetByCondtion("Type='Department' and Year=" + dt2.Year + " and Month=" + dt2.Month + " and DepartmentID=" + DepartmentID);
                if (tVO2.Count > 0)
                {
                    DepartmentTVO.Cost = tVO2[0].Cost;
                }
                else
                {
                    DepartmentTVO.Cost = 0;
                }
                DepartmentTVO.TargetID = 0;
                DepartmentTVO.Year = dt.Year;
                DepartmentTVO.Month = dt.Month;
                DepartmentTVO.Type = "Department";
                DepartmentTVO.TargetID = cBO.AddTarget(DepartmentTVO);
            }
            else
            {
                DepartmentTVO = tVO[0];
            }
            DepartmentTVO.DepartmentName = dVO.DepartmentName;

            List<TargetVO> PersonalTVO = new List<TargetVO>();
            List<PersonalVO> pListVO = cBO.FindPersonalByDepartmentID(DepartmentID);
            for (int i = 0; i < pListVO.Count; i++)
            {
                List<TargetVO> ptVO = cBO.FindTargetByCondtion("Type='Personal' and Year=" + dt.Year + " and Month=" + dt.Month + " and PersonalID=" + pListVO[i].PersonalID + " and DepartmentID=" + DepartmentID);

                TargetVO TargetVO = new TargetVO();
                //如果当月没有设置目标，自动创建
                if (ptVO.Count == 0)
                {
                    TargetVO.PersonalID = pListVO[i].PersonalID;
                    TargetVO.BusinessID = pVO.BusinessID;
                    TargetVO.CreatedAt = DateTime.Now;
                    TargetVO.DepartmentID = DepartmentID;

                    DateTime dt2 = dt.AddMonths(-1);
                    List<TargetVO> tVO2 = cBO.FindTargetByCondtion("Type='Personal' and Year=" + dt2.Year + " and Month=" + dt2.Month + " and PersonalID=" + pListVO[i].PersonalID + " and DepartmentID=" + DepartmentID);
                    if (tVO2.Count > 0)
                    {
                        TargetVO.Cost = tVO2[0].Cost;
                    }
                    else
                    {
                        TargetVO.Cost = 0;
                    }
                    TargetVO.TargetID = 0;
                    TargetVO.Year = dt.Year;
                    TargetVO.Month = dt.Month;
                    TargetVO.Type = "Personal";
                    TargetVO.TargetID = cBO.AddTarget(TargetVO);
                    TargetVO.Name = pListVO[i].Name;
                    TargetVO.Headimg = pListVO[i].Headimg;

                    PersonalTVO.Add(TargetVO);
                }
                else
                {
                    ptVO[0].Name = pListVO[i].Name;
                    ptVO[0].Headimg = pListVO[i].Headimg;
                    PersonalTVO.Add(ptVO[0]);
                }
            }

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = new { Department = DepartmentTVO, Personal = PersonalTVO } };
        }

        /// <summary>
        /// 获取总团队当月销售目标
        /// </summary>
        /// <param name="DepartmentID"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getBusinessTarget"), HttpGet]
        public ResultObject getBusinessTarget(string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            DateTime dt = DateTime.Now;

            List<TargetVO> tVO = cBO.FindTargetByCondtion("Type='Business' and Year=" + dt.Year + " and Month=" + dt.Month + " and BusinessID=" + pVO.BusinessID);

            TargetVO BusinessTVO = new TargetVO();
            //如果当月没有设置目标，自动创建
            if (tVO.Count == 0)
            {
                BusinessTVO.BusinessID = pVO.BusinessID;
                BusinessTVO.CreatedAt = DateTime.Now;

                DateTime dt2 = dt.AddMonths(-1);
                List<TargetVO> tVO2 = cBO.FindTargetByCondtion("Type='Business' and Year=" + dt2.Year + " and Month=" + dt2.Month + " and BusinessID=" + pVO.BusinessID);
                if (tVO2.Count > 0)
                {
                    BusinessTVO.Cost = tVO2[0].Cost;
                }
                else
                {
                    BusinessTVO.Cost = 0;
                }
                BusinessTVO.TargetID = 0;
                BusinessTVO.Year = dt.Year;
                BusinessTVO.Month = dt.Month;
                BusinessTVO.Type = "Business";
                BusinessTVO.TargetID = cBO.AddTarget(BusinessTVO);
            }
            else
            {
                BusinessTVO = tVO[0];
            }
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = new { Business = BusinessTVO } };
        }

        /// <summary>
        /// 根据日期获取打卡记录
        /// </summary>
        /// <param name="Date"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getPunchByDate"), HttpGet]
        public ResultObject getPunchByDate(string token, string Date = "")
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            DateTime dt = DateTime.Now;
            try
            {
                if (Date != "")
                {
                    dt = DateTime.Parse(Date);
                }
            }
            catch
            {
                dt = DateTime.Now;
            }


            PunchVO PunchVO = cBO.FindPunchByDate(dt, pVO.PersonalID, pVO.BusinessID);

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = PunchVO };
        }

        /// <summary>
        /// 添加或更新打卡记录
        /// </summary>
        /// <param name="VO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdatePunch"), HttpPost]
        public ResultObject UpdatePunch([FromBody] PunchVO PunchVO, string token, int PunchOut = 0)
        {
            if (PunchVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO bBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = bBO.FindPersonalByCustomerId(customerId);

            if (PunchVO.PunchID > 0)
            {
                if (PunchOut == 1)
                {
                    //下班打卡
                    PunchVO.PunchOutAt = DateTime.Now;
                    PunchVO.isPunchOut = true;
                    if (bBO.UpdatePunch(PunchVO))
                    {
                        PunchVO newPunchVO = bBO.FindPunchById(PunchVO.PunchID);
                        return new ResultObject() { Flag = 1, Message = "打卡成功!", Result = newPunchVO };
                    }
                    else
                    {
                        return new ResultObject() { Flag = 0, Message = "打卡失败!请重试", Result = null };
                    }
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "你已经打了上班卡", Result = null };
                }

            }
            else
            {
                //上班打卡
                PunchVO.PunchInAt = DateTime.Now;
                PunchVO.PersonalID = pVO.PersonalID;
                PunchVO.BusinessID = pVO.BusinessID;

                int PunchID = bBO.AddPunch(PunchVO);
                if (PunchID > 0)
                {
                    PunchVO newPunchVO = bBO.FindPunchById(PunchID);
                    return new ResultObject() { Flag = 1, Message = "打卡成功!", Result = newPunchVO };
                }
                else
                    return new ResultObject() { Flag = 0, Message = "打卡失败!请重试", Result = null };
            }
        }

        /// <summary>
        /// 根据日期获取当月打卡统计
        /// </summary>
        /// <param name="Date"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("FindMonthPunchByDate"), HttpGet]
        public ResultObject FindMonthPunchByDate(string token, string Date = "", int PersonalID = 0)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            DateTime dt = DateTime.Now;
            if (Date != "")
            {
                dt = DateTime.Parse(Date);
            }

            if (PersonalID == 0)
            {
                PersonalID = pVO.PersonalID;
            }

            List<PunchVO> ListPunchVO = cBO.FindMonthPunchByDate(dt, PersonalID, pVO.BusinessID);
            PunchVO PunchVO = cBO.FindPunchByDate(dt, PersonalID, pVO.BusinessID);

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = ListPunchVO, Subsidiary = PunchVO };
        }

        /// <summary>
        /// 导出打卡记录（Excel）
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        [Route("GetPunchExcel"), HttpGet]
        public ResultObject GetPunchExcel(string token, string Date = "")
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            CrmBO CrmBO = new CrmBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);

            if (!cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Admin") && !cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Personnel"))
            {
                return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
            }

            List<PersonalViewVO> Personal = cBO.FindPersonalViewByBusinessID(pVO.BusinessID, 0);

            DateTime Time = DateTime.Now;
            if (Date != "")
            {
                Time = DateTime.Parse(Date);
            }

            DataTable dt = new DataTable();

            int days = DateTime.DaysInMonth(Time.Year, Time.Month);
            dt.Columns.Add("日期", typeof(String));

            DataRow dr = dt.NewRow();
            dr["日期"] = "姓名";

            for (int i = 1; i <= days; i++)
            {
                dt.Columns.Add(i.ToString(), typeof(String));

                string week = DateTime.Parse(Time.Year + "-" + Time.Month + "-" + i).DayOfWeek.ToString();
                string weekText = "";
                switch (week)
                {
                    case "Monday": weekText = "一"; break;
                    case "Tuesday": weekText = "二"; break;
                    case "Wednesday": weekText = "三"; break;
                    case "Thursday": weekText = "四"; break;
                    case "Friday": weekText = "五"; break;
                    case "Saturday": weekText = "六"; break;
                    case "Sunday": weekText = "日"; break;
                    default: weekText = ""; break;
                }
                dr[i.ToString()] = weekText;
            }
            dt.Rows.Add(dr);

            dt.Columns.Add("总出勤天数", typeof(String));
            dt.Columns.Add("总日报数", typeof(String));
            dt.Columns.Add("总周报数", typeof(String));
            dt.Columns.Add("请假记录", typeof(String));
            dt.Columns.Add("报销记录", typeof(String));
            dt.Columns.Add("出差记录", typeof(String));
            dt.Columns.Add("加班记录", typeof(String));

            for (int i = 0; i < Personal.Count; i++)
            {
                try
                {

                    DataRow row = dt.NewRow();
                    row["日期"] = Personal[i].Name;
                    List<PunchVO> ListPunchVO = cBO.FindMonthPunchByDate(Time, Personal[i].PersonalID, pVO.BusinessID);

                    int PunchDay = 0;
                    int Daily = 0;
                    for (int p = 0; p < ListPunchVO.Count; p++)
                    {
                        try
                        {
                            if (ListPunchVO[p] != null)
                            {
                                //string Icon = "√";
                                row[(p + 1).ToString()] = SetTime(ListPunchVO[p].PunchInAt.Hour) + ":" + SetTime(ListPunchVO[p].PunchInAt.Minute);
                                row[(p + 1).ToString()] += " - " + SetTime(ListPunchVO[p].PunchOutAt.Hour) + ":" + SetTime(ListPunchVO[p].PunchOutAt.Minute);
                                //row[p.ToString()] = "√";
                                PunchDay++;

                                if (ListPunchVO[p].DailyID > 0)
                                    Daily++;
                            }
                        }
                        catch
                        {

                        }
                    }
                    row["总出勤天数"] = PunchDay;
                    row["总日报数"] = Daily;
                    row["总周报数"] = CrmBO.FindCrmCount("BusinessID=" + pVO.BusinessID + " and PersonalID=" + Personal[i].PersonalID + " and Type='Weekly' and year(CreatedAt)='" + Time.Year + "' and month(CreatedAt)='" + Time.Month + "'");
                    row["请假记录"] = CrmBO.GetApprovalToMonth(Time, "qingjia", pVO.BusinessID, Personal[i].PersonalID);
                    row["报销记录"] = CrmBO.GetApprovalToMonth(Time, "baoxiao", pVO.BusinessID, Personal[i].PersonalID);
                    row["出差记录"] = CrmBO.GetApprovalToMonth(Time, "chuchai", pVO.BusinessID, Personal[i].PersonalID);
                    row["加班记录"] = CrmBO.GetApprovalToMonth(Time, "jiaban", pVO.BusinessID, Personal[i].PersonalID);
                    dt.Rows.Add(row);
                }
                catch
                {

                }
            }

            string FileName = EPPlus.DataToExcel(dt, "BcPunchExcel/", DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".xlsx");
            if (FileName != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = FileName };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }
        public string SetTime(int hours)
        {
            string h = hours.ToString();
            if (hours < 10) h = "0" + hours;
            return h;
        }

        /// <summary>
        /// 根据日期获取所有下属当月打卡统计
        /// </summary>
        /// <param name="Date"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("FindMonthPersonalPunchByDate"), HttpGet]
        public ResultObject FindMonthPersonalPunchByDate(string token, string Date = "")
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            DateTime dt = DateTime.Now;
            if (Date != "")
            {
                dt = DateTime.Parse(Date);
            }

            List<PersonalPunchVO> ListPersonalPunchVO = cBO.FindMonthPersonalPunchByDate(dt, pVO.PersonalID, pVO.BusinessID);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = ListPersonalPunchVO };
        }

        /// <summary>
        /// 根据日期获取地图轨迹
        /// </summary>
        /// <param name="Date"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("FindPunchMapByList"), HttpGet]
        public ResultObject FindPunchMapByList(string token, string Date = "", int PersonalID = 0)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            DateTime dt = DateTime.Now;
            if (Date != "")
            {
                dt = DateTime.Parse(Date);
            }
            List<PunchMapVO> ListPunchMapVO = cBO.FindPunchMapByList(dt, pVO.PersonalID, PersonalID, pVO.BusinessID);
            List<PersonalVO> PersonalVO = cBO.FindPersonalByPersonalID(pVO.BusinessID, pVO.PersonalID);

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = ListPunchMapVO, Subsidiary = PersonalVO };
        }

        /// <summary>
        /// 获取我的拼团
        /// </summary>
        /// <param name="Date"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getMyGroupBuy"), HttpGet]
        public ResultObject getMyGroupBuy(string token, int BusinessID, int InfoID, int goGroupBuyID = 0)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());

            //判断是否为拼团成员
            string sql = "CustomerId =" + customerId + " and BusinessID=" + BusinessID + " and InfoID=" + InfoID;
            if (goGroupBuyID > 0)
            {
                sql += " and GroupBuyID=" + goGroupBuyID;
            }
            List<GroupBuyMemberVO> gVO = cBO.FindGroupBuyMemberList(sql + " order by CreatedAt desc");
            if (gVO.Count > 0)
            {
                int GroupBuyID = gVO[0].GroupBuyID;
                GroupBuyVO GroupBuyVO = cBO.FindGroupBuyById(GroupBuyID);
                if (GroupBuyVO == null)
                {
                    return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
                }

                //拼团成员
                List<GroupBuyMemberViewVO> GroupBuyMemberList = cBO.FindGroupBuyMemberViewList("GroupBuyID =" + GroupBuyVO.GroupBuyID + " and BusinessID=" + GroupBuyVO.BusinessID, 10);

                //折扣商品
                List<InfoViewVO> Products = cBO.FindInfoViewByInfoID("Products", GroupBuyVO.BusinessID, 0, "Order_info desc,CreatedAt desc", 5, "InfoID=" + InfoID);

                //可提现余额
                decimal Balance = cBO.getMyRebateCost(customerId, 1);

                //累计奖金
                decimal Balance2 = cBO.getMyRebateCost(customerId, 0);

                //订单数量
                int OrderCount = cBO.FindOrderViewCount("Status=1 and CustomerId=" + customerId);

                //已下单人数
                int OrderGroupBuyCount = cBO.FindOrderGroupBuyViewCount("GroupBuyID=" + GroupBuyVO.GroupBuyID);

                object res = new { GroupBuyVO = GroupBuyVO, GroupBuyMemberList = GroupBuyMemberList, Products = Products, Balance = Balance, Balance2 = Balance2, OrderCount = OrderCount, OrderGroupBuyCount = OrderGroupBuyCount };
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = res };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "您还未加入拼团!", Result = null };
            }
        }

        /// <summary>
        /// 退出拼团
        /// </summary>
        /// <param name="Date"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DeleteGroupBuyMember"), HttpGet]
        public ResultObject DeleteGroupBuyMember(string token, int InfoID)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            if (cBO.DeleteGroupBuyMember(customerId, InfoID) > 0)
            {
                return new ResultObject() { Flag = 1, Message = "成功退出!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "退出失败，请重试!", Result = null };
            }
        }

        /// <summary>
        /// 获取拼团好友
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        [Route("GetGroupBuyMemberList"), HttpPost]
        public ResultObject GetGroupBuyMemberList([FromBody] ConditionModel condition, int GroupBuyID, int BusinessID, string token)
        {
            if (condition == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            else if (condition.Filter == null || condition.PageInfo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            if (BusinessID <= 0)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            //判断是否为拼团成员
            List<GroupBuyMemberVO> gVO = cBO.FindGroupBuyMemberList("CustomerId =" + customerId + " and BusinessID=" + BusinessID + " and GroupBuyID=" + GroupBuyID);
            if (gVO.Count > 0)
            {
                string conditionStr = "BusinessID=" + gVO[0].BusinessID + " and GroupBuyID=" + gVO[0].GroupBuyID + " and (" + condition.Filter.Result() + ")";
                Paging pageInfo = condition.PageInfo;

                List<GroupBuyMemberViewVO> list = new List<GroupBuyMemberViewVO>();
                int count = 0;

                list = cBO.FindGroupBuyMemberViewAllByPageIndex(conditionStr, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);
                count = cBO.FindGroupBuyMemberViewCount(conditionStr);

                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list, Count = count };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "您还未加入拼团!", Result = null };
            }
        }

        /// <summary>
        /// 加入拼团
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("JoinGroupBuy"), HttpGet]
        public ResultObject JoinGroupBuy(int GroupBuyID, string token)
        {
            return new ResultObject() { Flag = 0, Message = "本接口已关闭!", Result = null };

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            if (pVO != null)
            {
                GroupBuyVO GroupBuyVO = cBO.FindGroupBuyById(GroupBuyID);
                if (GroupBuyVO == null)
                {
                    return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
                }

                InfoViewVO sVO = cBO.FindInfoViewById(GroupBuyVO.InfoID);
                if (sVO.isGroupBuy == 0)
                {
                    return new ResultObject() { Flag = 0, Message = "加入失败，本产品拼团功能已关闭!", Result = null };
                }

                //判断是否为拼团成员
                List<GroupBuyMemberVO> gVO = cBO.FindGroupBuyMemberList("CustomerId =" + customerId + " and BusinessID=" + GroupBuyVO.BusinessID + " and InfoID=" + GroupBuyVO.InfoID);
                if (gVO.Count > 0)
                {
                    return new ResultObject() { Flag = 0, Message = "您已加入了该产品的其他拼团，请先退出拼团!", Result = null };
                }

                GroupBuyMemberVO mVO = new GroupBuyMemberVO();
                mVO.GroupBuyMemberID = 0;
                mVO.GroupBuyID = GroupBuyVO.GroupBuyID;
                mVO.PersonalID = pVO.PersonalID;
                mVO.CustomerId = pVO.CustomerId;
                mVO.BusinessID = GroupBuyVO.BusinessID;
                mVO.InfoID = GroupBuyVO.InfoID;

                if (cBO.AddGroupMemberBuy(mVO) > 0)
                {
                    return new ResultObject() { Flag = 1, Message = "加入成功!", Result = null };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "加入失败!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "加入失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取加入拼团信息
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getJoinGroupBuy"), HttpGet]
        public ResultObject getJoinGroupBuy(int GroupBuyID, string token)
        {
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());

            GroupBuyVO GroupBuyVO = cBO.FindGroupBuyById(GroupBuyID);
            if (GroupBuyVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            PersonalVO pVO = cBO.FindPersonalByCustomerId(GroupBuyVO.CustomerId);
            if (pVO != null)
            {
                List<GroupBuyMemberVO> gVO = cBO.FindGroupBuyMemberList("CustomerId =" + customerId + " and GroupBuyID=" + GroupBuyID);
                bool isjion = gVO.Count > 0;

                List<GroupBuyMemberViewVO> list = cBO.FindGroupBuyMemberViewList("BusinessID=" + GroupBuyVO.BusinessID + " and GroupBuyID=" + GroupBuyVO.GroupBuyID, 10);
                int count = cBO.FindGroupBuyMemberViewCount("BusinessID=" + GroupBuyVO.BusinessID + " and GroupBuyID=" + GroupBuyVO.GroupBuyID);

                object res = new { GroupBuyVO = GroupBuyVO, MemberlList = list, IsJion = isjion, Personal = pVO, MemberCount = count };
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = res };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取我的奖金（买家订单返现）
        /// </summary>
        /// <returns></returns>
        [Route("GetRebateOrderViewList"), HttpGet]
        public ResultObject GetRebateOrderViewList(int PageCount, int PageIndex, string token)
        {

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            List<RebateOrder> RebateOrderList = cBO.getMyRebateOrder(customerId, 0);

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = RebateOrderList.Skip((PageIndex - 1) * PageCount).Take(PageCount), Count = RebateOrderList.Count };
        }



        /// <summary>
        /// 添加佣金收入提现记录信息(提现到微信零钱)
        /// </summary>
        /// <param name="wxUserInfoVO">登录信息</param>
        /// <param name="code">第一次拉手返回Code</param>
        /// <param name="formId">formId</param>
        /// <returns></returns>
        [Route("AddPayoutHistoryWXPacket"), HttpPost, Anonymous]
        public ResultObject AddPayoutHistoryWXPacket([FromBody] wxUserInfoVO wxUserInfoVO, string code, string AccountName, int AppType = 0)
        {
            if (AccountName == "")
            {
                return new ResultObject() { Flag = 0, Message = "请填写您的真实姓名!", Result = null };
            }

            AppVO AppVO = AppBO.GetApp(AppType);
            if (AppVO.AppType > 1)
            {
                return new ResultObject() { Flag = 0, Message = "您的企业已绑定独立收款账户，请到微信商家平台提现!", Result = null };
            }
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            CardBO CardBO = new CardBO(new CustomerProfile());
            try
            {
                string appid = AppVO.AppId;
                string secret = AppVO.Secret;

                string url = "https://api.weixin.qq.com/sns/jscode2session?appid=" + appid + "&secret=" + secret + "&js_code=" + code + "&grant_type=authorization_code";
                string jsonStr = HttpHelper.HtmlFromUrlGet(url);
                var serializer = new DataContractJsonSerializer(typeof(OpenIdAndSessionKey));
                var mStream = new MemoryStream(Encoding.Default.GetBytes(jsonStr));
                OpenIdAndSessionKey readConfig = (OpenIdAndSessionKey)serializer.ReadObject(mStream);

                if (readConfig.unionid != "")
                {
                    CustomerBO uBO = new CustomerBO(new CustomerProfile());
                    CustomerViewVO customerVO = uBO.FindCustomerByOpenId(readConfig.openid, readConfig.unionid, "2", AppVO.AppType);
                    if (customerVO != null)
                    {
                        //登录成功
                        DateTime now = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                        int count = cBO.FindBcPayoutHistoryTotalCount("CustomerId=" + customerVO.CustomerId + " and (PayOutStatus=1 or PayOutStatus=0) and to_days(PayOutDate) = to_days(now())");
                        if (count > 0)
                        {
                            return new ResultObject() { Flag = 0, Message = "提现失败，每天只可以提现一次!", Result = null };
                        }

                        if (!cBO.isLegitimate(customerVO.CustomerId))
                        {
                            return new ResultObject() { Flag = 0, Message = "您的可提现余额不足或者您的账户余额与平台收入不一致，请联系客服处理!", Result = null };
                        }

                        int PayoutHistoryId = cBO.AddPayOutHistoryByRebate(customerVO.CustomerId, readConfig.openid, AccountName);


                        if (PayoutHistoryId > 0)
                        {
                            BcPayOutHistoryVO pVO = cBO.FindBcPayOutHistoryById(PayoutHistoryId);
                            //企业付款到零钱
                            string resultbyPay = CardBO.PayforWXUserCash(pVO.Cost, pVO.AccountName, readConfig.openid, customerVO.CustomerId, 3);
                            if (resultbyPay == "NAME_MISMATCH")
                            {
                                pVO.PayOutStatus = -2;
                                pVO.HandleComment = "真实姓名填写错误";
                                cBO.HandlePayOut(pVO, AppType);
                                return new ResultObject() { Flag = 0, Message = "用户真实姓名填写错误，请填写微信认证时的真实姓名！", Result = resultbyPay };
                            }
                            if (resultbyPay == "FAIL")
                            {
                                pVO.PayOutStatus = -2;
                                pVO.HandleComment = "系统错误，请联系官方客服处理";
                                cBO.HandlePayOut(pVO, AppType);
                                return new ResultObject() { Flag = 0, Message = "提现失败,请重试!", Result = resultbyPay };
                            }
                            if (resultbyPay == "NOTENOUGH")
                            {
                                MessageTool.SendMobileMsg(AccountName + "发起微信提现，商户平台余额不足，请管理员前往充值后在后台进行操作！【众销乐 -资源共享众包销售平台】", "18620584620");//
                                MessageTool.SendMobileMsg(AccountName + "发起微信提现，商户平台余额不足，请管理员前往充值后在后台进行操作！【众销乐 -资源共享众包销售平台】", "13592808422");//
                                pVO.PayOutStatus = 0;//微信提现成功
                                pVO.HandleComment = "已提交转账申请，请留意微信通知！";
                                cBO.HandlePayOut(pVO, AppType);

                                return new ResultObject() { Flag = 1, Message = "申请成功，提现金额将会在24小时内到达，请留意服务通知！", Result = PayoutHistoryId };
                            }
                            if (resultbyPay == "AMOUNT_LIMIT")
                            {
                                pVO.PayOutStatus = 0;//微信提现成功
                                pVO.HandleComment = "已提交转账申请，因微信付款单日限额5000元的原因，可能会分批到账！";
                                cBO.HandlePayOut(pVO, AppType);

                                return new ResultObject() { Flag = 1, Message = "申请成功，因微信付款单日限额5000元的原因，可能会分批到账！", Result = PayoutHistoryId };
                            }
                            if (resultbyPay == "MONEY_LIMIT")
                            {
                                pVO.PayOutStatus = 0;//微信提现成功
                                pVO.HandleComment = "已提交转账申请，因微信付款单日限额5000元的原因，可能会分批到账！";
                                cBO.HandlePayOut(pVO, AppType);

                                return new ResultObject() { Flag = 1, Message = "申请成功，因微信付款单日限额5000元的原因，可能会分批到账！", Result = PayoutHistoryId };
                            }
                            if (resultbyPay == "V2_ACCOUNT_SIMPLE_BAN")
                            {
                                pVO.PayOutStatus = -2;
                                pVO.HandleComment = "您的微信没有实名认证";
                                cBO.HandlePayOut(pVO, AppType);
                                return new ResultObject() { Flag = 0, Message = "您的微信没有实名认证，请完成微信实名认证后再申请！", Result = resultbyPay };
                            }
                            if (resultbyPay == "SUCCESS")
                            {
                                pVO.PayOutStatus = 1;//微信提现成功
                                pVO.HandleComment = "已发放至微信零钱！";
                                cBO.HandlePayOut(pVO, AppType);
                                return new ResultObject() { Flag = 1, Message = "提现成功，已发放至微信零钱！", Result = PayoutHistoryId };
                            }
                            pVO.PayOutStatus = 1;//微信提现成功
                            pVO.HandleComment = "已提交转账申请，请留意微信通知！";
                            cBO.HandlePayOut(pVO, AppType);
                            return new ResultObject() { Flag = 1, Message = "申请成功，请留意微信通知！", Result = PayoutHistoryId };
                        }
                        else if (PayoutHistoryId == -2)
                        {
                            return new ResultObject() { Flag = 0, Message = "提现金额不能小于1块钱!", Result = null };
                        }
                        else
                        {
                            return new ResultObject() { Flag = 0, Message = "提现失败,请重试!", Result = null };
                        }
                    }
                }
                return new ResultObject() { Flag = 0, Message = "提现失败,请重试!", Result = null };
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "提现失败,请重试!", Result = null };
            }
        }


        /// <summary>
        /// 添加商家收入提现记录信息
        /// </summary>
        /// <param name="wxUserInfoVO">登录信息</param>
        /// <param name="code">第一次拉手返回Code</param>
        /// <param name="formId">formId</param>
        /// <returns></returns>
        [Route("AddPayoutHistory"), HttpPost, Anonymous]
        public ResultObject AddPayoutHistory([FromBody] wxUserInfoVO wxUserInfoVO, string code, decimal Cost, int AppType = 0)
        {
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            AppVO AppVO = AppBO.GetApp(AppType);
            try
            {
                string appid = AppVO.AppId;
                string secret = AppVO.Secret;

                string url = "https://api.weixin.qq.com/sns/jscode2session?appid=" + appid + "&secret=" + secret + "&js_code=" + code + "&grant_type=authorization_code";
                string jsonStr = HttpHelper.HtmlFromUrlGet(url);
                var serializer = new DataContractJsonSerializer(typeof(OpenIdAndSessionKey));
                var mStream = new MemoryStream(Encoding.Default.GetBytes(jsonStr));
                OpenIdAndSessionKey readConfig = (OpenIdAndSessionKey)serializer.ReadObject(mStream);

                if (readConfig.unionid != "")
                {
                    CustomerBO uBO = new CustomerBO(new CustomerProfile());
                    CustomerViewVO customerVO = uBO.FindCustomerByOpenId(readConfig.openid, readConfig.unionid, "2", AppVO.AppType);
                    if (customerVO != null)
                    {
                        //登录成功
                        DateTime now = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                        PersonalVO pVO = cBO.FindPersonalByCustomerId(customerVO.CustomerId);

                        if (!cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Admin"))
                        {
                            return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
                        }

                        /*
                        int count = cBO.FindBcPayoutHistoryTotalCount("BusinessID=" + pVO.BusinessID + " and (PayOutStatus=1 or PayOutStatus=0) and to_days(PayOutDate) = to_days(now())");
                        if (count > 0)
                        {
                            return new ResultObject() { Flag = 0, Message = "提现失败，每天只可以提现一次!", Result = null };
                        }*/

                        List<BcBankAccountVO> bankVO = cBO.FindBankAccountList("BusinessID=" + pVO.BusinessID);

                        if (bankVO.Count == 0)
                        {
                            return new ResultObject() { Flag = 0, Message = "您的企业未绑定收款账户，请联系客服!", Result = null };
                        }

                        if (!cBO.IsHasMoreBusinessBalance(pVO.BusinessID, Cost))
                        {
                            return new ResultObject() { Flag = 0, Message = "余额不足！请输入正确金额", Result = null };
                        }

                        int PayoutHistoryId = cBO.AddPayOutHistoryByBusiness(Cost, pVO.BusinessID, customerVO.CustomerId, readConfig.openid);


                        if (PayoutHistoryId > 0)
                        {
                            return new ResultObject() { Flag = 1, Message = "申请成功，请留意微信通知！", Result = PayoutHistoryId };
                        }
                        else if (PayoutHistoryId == -2)
                        {
                            return new ResultObject() { Flag = 0, Message = "提现金额不能小于1块钱!", Result = null };
                        }
                        else
                        {
                            return new ResultObject() { Flag = 0, Message = "提现失败,请重试!", Result = null };
                        }
                    }
                }
                return new ResultObject() { Flag = 0, Message = "提现失败,请重试!", Result = null };
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "提现失败,请重试!", Result = null };
            }
        }


        /// <summary>
        /// 获取我的提现信息
        /// </summary>
        /// <returns></returns>
        [Route("GetMyPayOut"), HttpGet]
        public ResultObject GetMyPayOut(int PageCount, int PageIndex, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            List<BcPayOutHistoryVO> PayOutList = cBO.FindBcPayOutHistoryList("CustomerId=" + customerId + " and Type=1");
            PayOutList.Sort((a, b) => a.PayOutDate.CompareTo(b.PayOutDate));
            PayOutList.Reverse();

            //总收入
            decimal TotalBalance = cBO.getMyRebateCost(customerId, 0);

            //可提现余额
            decimal Balance = cBO.getMyRebateCost(customerId, 1);

            //已提现余额
            decimal PayOutBalance = cBO.getMyRebateCost(customerId, 2);

            //提现中余额
            decimal PayOutInProgressBalance = cBO.getMyRebateCost(customerId, 3);

            object data = new { TotalBalance, Balance, PayOutBalance, PayOutInProgressBalance };

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = PayOutList.Skip((PageIndex - 1) * PageCount).Take(PageCount), Count = PayOutList.Count, Subsidiary = data };
        }

        /// <summary>
        /// 获取商家提现信息
        /// </summary>
        /// <returns></returns>
        [Route("GetBusinessPayOut"), HttpGet]
        public ResultObject GetBusinessPayOut(int PageCount, int PageIndex, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);

            if (!cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Admin"))
            {
                return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
            }

            List<BcPayOutHistoryVO> PayOutList = cBO.FindBcPayOutHistoryList("BusinessID=" + pVO.BusinessID + " and Type=2");
            PayOutList.Sort((a, b) => a.PayOutDate.CompareTo(b.PayOutDate));
            PayOutList.Reverse();

            cBO.OrderSettlement(pVO.BusinessID);

            //总收入
            decimal TotalBalance = cBO.getBusinessCost(pVO.BusinessID, 0);

            //可提现余额
            decimal Balance = cBO.getBusinessCost(pVO.BusinessID, 1);

            //已提现余额
            decimal PayOutBalance = cBO.getBusinessCost(pVO.BusinessID, 2);

            //提现中余额
            decimal PayOutInProgressBalance = cBO.getBusinessCost(pVO.BusinessID, 3);

            List<BcBankAccountVO> bankVO = cBO.FindBankAccountList("BusinessID=" + pVO.BusinessID);

            BcBankAccountVO bank = new BcBankAccountVO();

            bool isbank = bankVO.Count > 0;

            if (bankVO.Count > 0)
            {
                bank = bankVO[0];
            }
            else
            {
                bank = null;
            }

            object data = new { TotalBalance, Balance, PayOutBalance, PayOutInProgressBalance, isbank = isbank, bank = bank };

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = PayOutList.Skip((PageIndex - 1) * PageCount).Take(PageCount), Count = PayOutList.Count, Subsidiary = data };
        }
        /// <summary>
        /// 获取名片主题修改信息
        /// </summary>
        /// <returns></returns>
        [Route("GetThemeEdit"), HttpGet]
        public ResultObject GetThemeEdit(int ThemeID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            CrmBO CrmBO = new CrmBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);

            try
            {
                if (pVO != null)
                {
                    BusinessCardVO bVO = new BusinessCardVO();

                    if (pVO.BusinessID == 0)
                    {
                        bVO = null;
                    }
                    else
                    {
                        bVO = cBO.FindBusinessCardById(pVO.BusinessID);
                        //清除营业执照等保密信息
                        bVO.BusinessLicenseImg = "";
                    }
                    int ReadNum = 0, todayReadNum = 0;

                    try
                    {
                        ReadNum = cBO.FindNumberOfVisitors("Personal", pVO.PersonalID);
                        todayReadNum = cBO.FindNumberOfVisitors("Personal", pVO.PersonalID, 1, 0);
                    }
                    catch
                    {

                    }

                    ThemeVO ThemeVO = new ThemeVO();
                    ThemeVO.setThemeVO();
                    ThemeVO.BusinessID = bVO.BusinessID;

                    if (ThemeID > 0)
                    {
                        ThemeVO = cBO.FindThemeById(ThemeID);
                    }


                    object res = new { Personal = pVO, ReadNum = ReadNum, todayReadNum = todayReadNum, Theme = ThemeVO, BusinessName = bVO.BusinessName, LogoImg = bVO.LogoImg };
                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = res };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
                }
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = ex };
            }
        }

        /// <summary>
        /// 添加或更新主题
        /// </summary>
        /// <param name="VO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateTheme"), HttpPost]
        public ResultObject UpdateTheme([FromBody] ThemeVO ThemeVO, string token)
        {
            if (ThemeVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());

            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);



            if (ThemeVO.BusinessID == 0)
            {
                return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
            }
            if (!cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Web") && !cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Admin"))
            {
                return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
            }

            if (ThemeVO.ThemeID > 0)
            {
                ThemeVO sVO = cBO.FindThemeById(ThemeVO.ThemeID);
                if (sVO.BusinessID != pVO.BusinessID)
                {
                    return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
                }

                ThemeVO.CreatedAt = sVO.CreatedAt;
                ThemeVO.BusinessID = sVO.BusinessID;

                if (cBO.UpdateTheme(ThemeVO))
                {
                    BusinessCardVO bVO = new BusinessCardVO();
                    bVO.BusinessID = pVO.BusinessID;
                    bVO.ThemeID = ThemeVO.ThemeID;
                    cBO.UpdateBusinessCard(bVO);
                    cBO.GetPosterCardIMGByBusinessID(pVO.BusinessID);
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = ThemeVO.ThemeID };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }
            }
            else
            {
                ThemeVO.CreatedAt = DateTime.Now;
                ThemeVO.BusinessID = pVO.BusinessID;

                int ThemeID = cBO.AddTheme(ThemeVO);
                if (ThemeID > 0)
                {
                    BusinessCardVO bVO = new BusinessCardVO();
                    bVO.BusinessID = pVO.BusinessID;
                    bVO.ThemeID = ThemeID;
                    cBO.UpdateBusinessCard(bVO);
                    cBO.GetPosterCardIMGByBusinessID(pVO.BusinessID);
                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = ThemeID };
                }
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
        }

        /// <summary>
        /// 使用主题
        /// </summary>
        /// <param name="VO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UseTheme"), HttpGet]
        public ResultObject UseTheme(int ThemeID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            ThemeVO ThemeVO = cBO.FindThemeById(ThemeID);

            if (ThemeVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
            }

            if (pVO.BusinessID == 0)
            {
                return new ResultObject() { Flag = 0, Message = "您当前未加入企业!", Result = null };
            }

            if (!cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Web") && !cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Admin"))
            {
                return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
            }
            BusinessCardVO bVO = new BusinessCardVO();
            bVO.BusinessID = pVO.BusinessID;
            bVO.ThemeID = ThemeID;
            cBO.UpdateBusinessCard(bVO);
            cBO.GetPosterCardIMGByBusinessID(pVO.BusinessID);
            return new ResultObject() { Flag = 1, Message = "使用成功!", Result = null };
        }

        /// <summary>
        /// 删除主题
        /// </summary>
        /// <param name="VO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DelTheme"), HttpGet]
        public ResultObject DelTheme(int ThemeID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            ThemeVO ThemeVO = cBO.FindThemeById(ThemeID);

            if (ThemeVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
            }

            if (ThemeVO.BusinessID == 0)
            {
                return new ResultObject() { Flag = 0, Message = "官方主题不能删除!", Result = null };
            }

            if (!cBO.FindJurisdiction(pVO.PersonalID, ThemeVO.BusinessID, "Web") && !cBO.FindJurisdiction(pVO.PersonalID, ThemeVO.BusinessID, "Admin"))
            {
                return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
            }
            string CardBackImg = ThemeVO.CardBackImg;
            if (cBO.DeleteThemeById(ThemeVO.ThemeID) > 0)
            {
                BusinessCardVO bVO = cBO.FindBusinessCardById(pVO.BusinessID);
                if (bVO.ThemeID == ThemeVO.ThemeID)
                    bVO.ThemeID = 0;
                cBO.UpdateBusinessCard(bVO);
                cBO.GetPosterCardIMGByBusinessID(pVO.BusinessID);
                return new ResultObject() { Flag = 1, Message = "删除成功!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取主题列表
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getThemeList"), HttpGet]
        public ResultObject getThemeList(string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            List<ThemeVO> List = cBO.FindThemeList("BusinessID=" + pVO.BusinessID + " or BusinessID=0  Order By ThemeID desc");
            int ReadNum = 0, todayReadNum = 0;

            try
            {
                ReadNum = cBO.FindNumberOfVisitors("Personal", pVO.PersonalID);
                todayReadNum = cBO.FindNumberOfVisitors("Personal", pVO.PersonalID, 1, 0);
            }
            catch
            {

            }

            BusinessCardVO bVO = new BusinessCardVO();

            if (pVO.BusinessID > 0)
            {
                bVO = cBO.FindBusinessCardById(pVO.BusinessID);
            }

            object res = new { Personal = pVO, ReadNum = ReadNum, todayReadNum = todayReadNum, BusinessName = bVO.BusinessName, LogoImg = bVO.LogoImg, BusinessThemeID = bVO.ThemeID };
            if (List != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = List, Subsidiary = res };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取公司信息
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getBusinessCard"), HttpGet]
        public ResultObject getBusinessCard(string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);

            BusinessCardVO bVO = new BusinessCardVO();
            if (pVO.BusinessID > 0)
            {
                bVO = cBO.FindBusinessCardById(pVO.BusinessID);
            }
            if (bVO != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = bVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 更新访问设置
        /// </summary>
        /// <param name="WebVO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateAccessSetUp"), HttpGet]
        public ResultObject UpdateAccessSetUp(string Type, int Status, string token)
        {
            if (Type == "")
            {
                return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
            }
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());

            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);

            if (pVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }

            if (!cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Web") && !cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Admin"))
            {
                return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
            }

            BusinessCardVO bVO = cBO.FindBusinessCardById(pVO.BusinessID);
            if (bVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }

            if (Type == "AccessSetUpSignIn")
            {
                bVO.AccessSetUpSignIn = Status;
            }
            if (Type == "AccessSetUpPhone")
            {
                bVO.AccessSetUpPhone = Status;
            }
            if (Type == "AccessSetUpExchange")
            {
                bVO.AccessSetUpExchange = Status;
            }
            if (Type == "DisplayCard")
            {
                bVO.DisplayCard = Status;
            }
            if (Type == "DisplayVideo")
            {
                bVO.DisplayVideo = Status;
            }
            if (cBO.UpdateBusinessCard(bVO))
            {
                return new ResultObject() { Flag = 1, Message = "更新成功!", Result = bVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }
        }


        /// <summary>
        /// 获取我的积分列表
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getIntegralList"), HttpGet]
        public ResultObject getIntegralList(string token, int PageCount = 20, int PageIndex = 1, int AppType = 0)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO PersonalVO = cBO.FindPersonalByCustomerId(customerId);
            AppVO AppVO = AppBO.GetApp(AppType);
            string sql = "PersonalID=" + PersonalVO.PersonalID + " and Status=1";
            sql += " GROUP BY BusinessID order by CreatedAt desc";

            List<IntegralVO> IntegralList = cBO.FindIntegralList(sql);
            IEnumerable<IntegralVO> newVO = IntegralList.Skip((PageIndex - 1) * PageCount).Take(PageCount);
            int Count = cBO.FindIntegralByCondition(sql);

            foreach (IntegralVO item in newVO)
            {
                BusinessCardVO bVO = cBO.FindBusinessCardById(item.BusinessID);
                if (bVO != null)
                {
                    item.Name = bVO.BusinessName;
                    item.Headimg = bVO.LogoImg;
                }
                item.Balance = cBO.FindMyIntegral(item.BusinessID, item.PersonalID);
            }

            if (PersonalVO.IntegralQRimg == "")
            {
                PersonalVO.IntegralQRimg = cBO.GetIntegralQRimg(PersonalVO.PersonalID, AppType);
            }


            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = newVO, Count = Count, Subsidiary = PersonalVO.IntegralQRimg };
        }

        /// <summary>
        /// 获取顾客积分列表
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getPersonalIntegralList"), HttpGet]
        public ResultObject getPersonalIntegralList(string token, int PageCount = 20, int PageIndex = 1, string Search = "")
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO PersonalVO = cBO.FindPersonalByCustomerId(customerId);


            string sql = "BusinessID=" + PersonalVO.BusinessID + " and Status=1";

            if (Search != "")
            {
                sql += " and (Name like '%" + Search + "%' or Phone like '%" + Search + "%')";
            }

            sql += " GROUP BY PersonalID order by CreatedAt desc";

            List<IntegralViewVO> IntegralList = cBO.FindIntegralViewList(sql);
            IEnumerable<IntegralViewVO> newVO = IntegralList.Skip((PageIndex - 1) * PageCount).Take(PageCount);
            int Count = cBO.FindIntegralByCondition(sql);

            foreach (IntegralViewVO item in newVO)
            {
                item.Balance = cBO.FindMyIntegral(item.BusinessID, item.PersonalID);
            }
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = newVO, Count = Count };
        }

        /// <summary>
        /// 获取积分详情
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getIntegral"), HttpGet]
        public ResultObject getIntegral(string token, int BusinessID = 0, int PersonalID = 0, int ListType = 1, int PageCount = 20, int PageIndex = 1)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO PersonalVO = cBO.FindPersonalByCustomerId(customerId);
            BusinessCardVO bVO = new BusinessCardVO();
            PersonalVO pVO = new PersonalVO();

            if (BusinessID == 0 && PersonalVO.BusinessID > 0)
            {
                BusinessID = PersonalVO.BusinessID;
                bVO = cBO.FindBusinessCardById(PersonalVO.BusinessID);
            }
            else if (BusinessID > 0)
            {
                bVO = cBO.FindBusinessCardById(BusinessID);
            }

            if (PersonalID == 0)
            {
                PersonalID = PersonalVO.PersonalID;
                pVO = PersonalVO;
            }
            else
            {
                pVO = cBO.FindPersonalById(PersonalID);
            }

            string sql = "BusinessID=" + BusinessID + " and PersonalID=" + PersonalID + " and Status=1";
            if (ListType == 2)
            {
                sql += " and Balance>=0";
            }
            if (ListType == 3)
            {
                sql += " and Balance<0";
            }
            sql += " order by CreatedAt desc";

            List<IntegralVO> IntegralList = cBO.FindIntegralList(sql);
            int Count = cBO.FindIntegralByCondition(sql);

            bool isJurisdiction = false;
            if ((cBO.FindJurisdiction(PersonalVO.PersonalID, BusinessID, "Admin") || cBO.FindJurisdiction(PersonalVO.PersonalID, BusinessID, "Order")) && BusinessID == PersonalVO.BusinessID)
            {
                isJurisdiction = true;
            }

            decimal Balance = cBO.FindMyIntegral(BusinessID, PersonalID);


            IEnumerable<IntegralVO> newVO = IntegralList.Skip((PageIndex - 1) * PageCount).Take(PageCount);
            foreach (IntegralVO item in newVO)
            {
                PersonalVO ppVO = cBO.FindPersonalById(item.OperPersonalID);

                if (ppVO != null)
                {
                    item.Name = ppVO.Name;
                    item.Headimg = ppVO.Headimg;
                }
            }

            object res = new { Personal = pVO, Business = bVO, IntegralList = newVO, Count = Count, isJurisdiction = isJurisdiction, Balance = Balance };

            if (res != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = res };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取添加积分记录
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("addIntegral"), HttpGet]
        public ResultObject addIntegral(int PersonalID, decimal Balance, string token, string Note = "")
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO OperPersonalVO = cBO.FindPersonalByCustomerId(customerId);
            PersonalVO PersonalVO = cBO.FindPersonalById(PersonalID);

            if (OperPersonalVO == null || PersonalVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
            }

            if (!cBO.FindJurisdiction(OperPersonalVO.PersonalID, OperPersonalVO.BusinessID, "Admin") && !cBO.FindJurisdiction(OperPersonalVO.PersonalID, OperPersonalVO.BusinessID, "Order"))
            {
                return new ResultObject() { Flag = 0, Message = "您没有更改积分的权限!", Result = null };
            }


            decimal oldBalance = cBO.FindMyIntegral(OperPersonalVO.BusinessID, PersonalVO.PersonalID);

            if (oldBalance + Balance < 0)
            {
                return new ResultObject() { Flag = 0, Message = "积分不足，扣除失败!", Result = null };
            }

            IntegralVO IVO = new IntegralVO();
            IVO.IntegralID = 0;
            IVO.PersonalID = PersonalVO.PersonalID;
            IVO.BusinessID = OperPersonalVO.BusinessID;
            IVO.OperPersonalID = OperPersonalVO.PersonalID;
            IVO.Note = Note;
            IVO.Balance = Balance;
            IVO.CreatedAt = DateTime.Now;
            IVO.Status = 0;

            int IntegralID = cBO.AddIntegral(IVO);
            if (IntegralID > 0)
                return new ResultObject() { Flag = 1, Message = "添加成功!", Result = IntegralID };
            else
                return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
        }

        /// <summary>
        /// 启用积分记录
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("openIntegral"), HttpGet]
        public ResultObject openIntegral(int IntegralID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());

            IntegralVO IVO = cBO.FindIntegralById(IntegralID);
            PersonalVO OperPersonalVO = cBO.FindPersonalByCustomerId(customerId);

            if (OperPersonalVO == null || IVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
            }

            if (IVO.OperPersonalID != OperPersonalVO.PersonalID)
            {
                return new ResultObject() { Flag = 0, Message = "您没有权限!", Result = null };
            }

            IVO.Status = 1;

            if (cBO.UpdateIntegral(IVO))
                return new ResultObject() { Flag = 1, Message = "已成功生效!", Result = null };
            else
                return new ResultObject() { Flag = 0, Message = "生效失败，请重新提交!", Result = null };
        }

        /// <summary>
        /// 获取VIP
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("FindShopVip"), HttpGet]
        public ResultObject FindShopVip(string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);

            List<ShopVipVO> sVO = cBO.FindShopVipList("BusinessID=" + pVO.BusinessID + " Order By Level asc");
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = sVO };
        }


        /// <summary>
        /// 获取vip会员列表
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getShopVipPersonalList"), HttpGet]
        public ResultObject getShopVipPersonalList(string token, int ShopVipID, int PageCount = 20, int PageIndex = 1)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());

            string sql = "ShopVipID=" + ShopVipID;
            sql += " order by CreatedAt desc";

            List<ShopVipPersonalVO> IntegralList = cBO.FindShopVipPersonalList(sql);
            IEnumerable<ShopVipPersonalVO> newVO = IntegralList.Skip((PageIndex - 1) * PageCount).Take(PageCount);


            foreach (ShopVipPersonalVO item in newVO)
            {
                PersonalVO PersonalVO = cBO.FindPersonalById(item.PersonalID);
                if (PersonalVO != null)
                {
                    item.Name = PersonalVO.Name;
                    item.Headimg = PersonalVO.Headimg;
                }
            }

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = newVO, Count = IntegralList.Count };
        }


        /// <summary>
        /// 添加或更新VIP
        /// </summary>
        /// <param name="VO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateShopVipVO"), HttpPost]
        public ResultObject UpdateShopVipVO([FromBody] ShopVipVO ShopVipVO, string token)
        {
            if (ShopVipVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);

            if (!cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Order") && !cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Admin"))
            {
                return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
            }

            if (ShopVipVO.ShopVipID > 0)
            {
                ShopVipVO sVO = cBO.FindShopVipById(ShopVipVO.ShopVipID);
                ShopVipVO.BusinessID = sVO.BusinessID;
                if (cBO.UpdateShopVip(ShopVipVO))
                {
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = sVO.ShopVipID };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }
            }
            else
            {
                ShopVipVO.BusinessID = pVO.BusinessID;

                int ShopVipID = cBO.AddShopVip(ShopVipVO);
                if (ShopVipID > 0)
                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = ShopVipID };
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
        }

        /// <summary>
        /// 删除VIP
        /// </summary>
        /// <param name="VO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DelShopVip"), HttpGet]
        public ResultObject DelShopVip(int ShopVipID, string token)
        {
            if (ShopVipID <= 0)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);

            ShopVipVO sVO = cBO.FindShopVipById(ShopVipID);
            if (sVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "该VIP已被删除!", Result = null };
            }

            if (!cBO.FindJurisdiction(pVO.PersonalID, sVO.BusinessID, "Order") && !cBO.FindJurisdiction(pVO.PersonalID, sVO.BusinessID, "Admin"))
            {
                return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
            }

            if (cBO.DeleteShopVipById(ShopVipID) > 0)
            {
                return new ResultObject() { Flag = 1, Message = "删除成功!", Result = sVO.ShopVipID };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取我的vip列表
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getMyVipList"), HttpGet]
        public ResultObject getMyVipList(string token, int PageCount = 20, int PageIndex = 1, int AppType = 0)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO PersonalVO = cBO.FindPersonalByCustomerId(customerId);
            AppVO AppVO = AppBO.GetApp(AppType);
            string sql = "PersonalID=" + PersonalVO.PersonalID;
            sql += " order by CreatedAt desc";

            List<ShopVipPersonalVO> IntegralList = cBO.FindShopVipPersonalList(sql);
            IEnumerable<ShopVipPersonalVO> newVO = IntegralList.Skip((PageIndex - 1) * PageCount).Take(PageCount);

            foreach (ShopVipPersonalVO item in newVO)
            {
                BusinessCardVO bVO = cBO.FindBusinessCardById(item.BusinessID);
                if (bVO != null)
                {
                    item.Name = bVO.BusinessName;
                    item.Headimg = bVO.LogoImg;
                }
                ShopVipVO sVO = cBO.FindShopVipById(item.ShopVipID);
                if (sVO != null)
                {
                    item.VipName = sVO.VipName;
                }
            }
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = newVO, Count = IntegralList.Count };
        }

        /// <summary>
        /// 获取我的代理列表
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getAgentIntegralList"), HttpGet]
        public ResultObject getAgentIntegralList(string token, int PageCount = 20, int PageIndex = 1)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO PersonalVO = cBO.FindPersonalByCustomerId(customerId);

            string sql = "PersonalID=" + PersonalVO.PersonalID + " and Status=1";
            sql += " GROUP BY BusinessID order by CreatedAt desc";

            List<AgentIntegralVO> IntegralList = cBO.FindAgentIntegralList(sql);
            IEnumerable<AgentIntegralVO> newVO = IntegralList.Skip((PageIndex - 1) * PageCount).Take(PageCount);
            int Count = cBO.FindIntegralByCondition(sql);

            foreach (AgentIntegralVO item in newVO)
            {
                BusinessCardVO bVO = cBO.FindBusinessCardById(item.BusinessID);
                if (bVO != null)
                {
                    item.Name = bVO.BusinessName;
                    item.Headimg = bVO.LogoImg;
                }
                item.Balance = cBO.FindMyAgentIntegral(item.BusinessID, item.PersonalID);
            }
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = newVO, Count = Count };
        }

        /// <summary>
        /// 获取代理详情
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getAgentIntegral"), HttpGet]
        public ResultObject getAgentIntegral(string token, int BusinessID = 0, int PersonalID = 0, int ListType = 1, int PageCount = 20, int PageIndex = 1)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO PersonalVO = cBO.FindPersonalByCustomerId(customerId);
            BusinessCardVO bVO = new BusinessCardVO();
            PersonalVO pVO = new PersonalVO();

            if (BusinessID == 0 && PersonalVO.BusinessID > 0)
            {
                BusinessID = PersonalVO.BusinessID;
                bVO = cBO.FindBusinessCardById(PersonalVO.BusinessID);
            }
            else if (BusinessID > 0)
            {
                bVO = cBO.FindBusinessCardById(BusinessID);
            }

            if (PersonalID == 0)
            {
                PersonalID = PersonalVO.PersonalID;
                pVO = PersonalVO;
            }
            else
            {
                pVO = cBO.FindPersonalById(PersonalID);
            }

            string sql = "BusinessID=" + BusinessID + " and PersonalID=" + PersonalID + " and Status=1";
            if (ListType == 2)
            {
                sql += " and Balance>=0";
            }
            if (ListType == 3)
            {
                sql += " and Balance<0";
            }
            sql += " order by CreatedAt desc";

            List<AgentIntegralVO> AgentIntegralList = cBO.FindAgentIntegralList(sql);
            int Count = cBO.FindAgentIntegralByCondition(sql);

            bool isJurisdiction = false;
            if (cBO.FindJurisdiction(PersonalVO.PersonalID, BusinessID, "Admin") && BusinessID == PersonalVO.BusinessID)
            {
                isJurisdiction = true;
            }

            decimal Balance = cBO.FindMyAgentIntegral(BusinessID, PersonalID);


            IEnumerable<AgentIntegralVO> newVO = AgentIntegralList.Skip((PageIndex - 1) * PageCount).Take(PageCount);
            foreach (AgentIntegralVO item in newVO)
            {
                PersonalVO ppVO = cBO.FindPersonalById(item.OperPersonalID);

                if (ppVO != null)
                {
                    item.Name = ppVO.Name;
                    item.Headimg = ppVO.Headimg;
                }
            }

            object res = new { Personal = pVO, Business = bVO, IntegralList = newVO, Count = Count, isJurisdiction = isJurisdiction, Balance = Balance };

            if (res != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = res };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取代理商代理的商品
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getAgentProductList"), HttpGet]
        public ResultObject getAgentProductList(string token, int BusinessID, int PersonalID = 0)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO PersonalVO = cBO.FindPersonalByCustomerId(customerId);

            if (BusinessID == 0)
            {
                BusinessID = PersonalVO.BusinessID;
            }

            List<InfoVO> sListVO = cBO.FindInfoByCondtion("Type='Products' and BusinessID = " + BusinessID + " and Status=1");

            if (PersonalID == 0)
            {
                PersonalID = PersonalVO.PersonalID;
            }

            List<InfoVO> newListVO = new List<InfoVO>();
            for (int i = 0; i < sListVO.Count; i++)
            {
                decimal Cost = cBO.FindAgentlevelCostByPersonalID(PersonalID, sListVO[i].InfoID);
                if (sListVO[i].Cost != Cost)
                {
                    sListVO[i].Cost = Cost;
                    newListVO.Add(sListVO[i]);
                }
            }

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = newListVO };
        }

        /// <summary>
        /// 获取添加代理商保证金记录
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("addAgentIntegral"), HttpGet]
        public ResultObject addAgentIntegral(int PersonalID, decimal Balance, string token, string Note = "")
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO OperPersonalVO = cBO.FindPersonalByCustomerId(customerId);
            PersonalVO PersonalVO = cBO.FindPersonalById(PersonalID);

            if (OperPersonalVO == null || PersonalVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
            }

            if (!cBO.FindJurisdiction(OperPersonalVO.PersonalID, OperPersonalVO.BusinessID, "Admin"))
            {
                return new ResultObject() { Flag = 0, Message = "您没有更改保证金的权限!", Result = null };
            }


            decimal oldBalance = cBO.FindMyAgentIntegral(OperPersonalVO.BusinessID, PersonalVO.PersonalID);

            if (oldBalance + Balance < 0)
            {
                return new ResultObject() { Flag = 0, Message = "余额不足，扣除失败!", Result = null };
            }

            AgentIntegralVO IVO = new AgentIntegralVO();
            IVO.AgentIntegralID = 0;
            IVO.PersonalID = PersonalVO.PersonalID;
            IVO.BusinessID = OperPersonalVO.BusinessID;
            IVO.OperPersonalID = OperPersonalVO.PersonalID;
            IVO.Note = Note;
            IVO.Balance = Balance;
            IVO.CreatedAt = DateTime.Now;
            IVO.Status = 0;

            int AgentIntegralID = cBO.AddAgentIntegral(IVO);
            if (AgentIntegralID > 0)
                return new ResultObject() { Flag = 1, Message = "添加成功!", Result = AgentIntegralID };
            else
                return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
        }

        /// <summary>
        /// 启用代理商保证金记录
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("openAgentIntegral"), HttpGet]
        public ResultObject openAgentIntegral(int AgentIntegralID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());

            AgentIntegralVO IVO = cBO.FindAgentIntegralById(AgentIntegralID);
            PersonalVO OperPersonalVO = cBO.FindPersonalByCustomerId(customerId);

            if (OperPersonalVO == null || IVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
            }

            if (IVO.OperPersonalID != OperPersonalVO.PersonalID)
            {
                return new ResultObject() { Flag = 0, Message = "您没有权限!", Result = null };
            }

            IVO.Status = 1;

            if (cBO.UpdateAgentIntegral(IVO))
                return new ResultObject() { Flag = 1, Message = "已成功生效!", Result = null };
            else
                return new ResultObject() { Flag = 0, Message = "生效失败，请重新提交!", Result = null };
        }


        /// <summary>
        /// 添加或更新企业版帮助
        /// </summary>
        /// <param name="CardHelpVO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("AddBusinessCardHelp"), HttpPost]
        public ResultObject AddBusinessCardHelp([FromBody] HelpVO HelpVO, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            if (HelpVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            IUserViewDAO uDAO = new UserViewDAO(new UserProfile());
            UserViewVO uVO = uDAO.FindById(uProfile.UserId);
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());

            if (HelpVO.HelpID > 0)
            {
                if (cBO.UpdateHelp(HelpVO))
                {
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = HelpVO.HelpID };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }
            }
            else
            {
                HelpVO.CreatedAt = DateTime.Now;
                int HelpID = cBO.AddHelp(HelpVO);
                if (HelpID > 0)
                {
                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = HelpID };
                }
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取帮助
        /// </summary>
        /// <param name="HelpID">帮助ID</param>
        /// <returns></returns>
        [Route("GetBusinessCardHelp"), HttpGet, Anonymous]
        public ResultObject GetBusinessCardHelp(int HelpID)
        {
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());

            HelpVO vo = cBO.FindCardHelpById(HelpID);
            if (vo != null)
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = vo };
            else
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };

        }

        /// <summary>
        /// 获取全部帮助
        /// </summary>
        /// <param name="HelpID">帮助ID</param>
        /// <returns></returns>
        [Route("GetBusinessCardHelpList"), HttpGet, Anonymous]
        public ResultObject GetBusinessCardHelpList()
        {
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());

            List<HelpVO> vo = cBO.FindHelpByConditionStr("Type = 1");
            List<HelpVO> vo2 = cBO.FindHelpByConditionStr("Type = 2");
            List<HelpVO> vo3 = cBO.FindHelpByConditionStr("Type = 3");

            List<bcHelpList> List = new List<bcHelpList>();

            bcHelpList HelpList = new bcHelpList();
            HelpList.Title = "个人功能相关";
            HelpList.List = vo;
            List.Add(HelpList);

            bcHelpList HelpList2 = new bcHelpList();
            HelpList2.Title = "企业功能相关";
            HelpList2.List = vo2;
            List.Add(HelpList2);

            bcHelpList HelpList3 = new bcHelpList();
            HelpList3.Title = "其他功能";
            HelpList3.List = vo3;
            List.Add(HelpList3);

            if (vo != null)
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = List };
            else
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };

        }

        /// <summary>
        /// 删除帮助
        /// </summary>
        /// <param name="HelpID">帮助ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DeleteBusinessCardHelp"), HttpGet]
        public ResultObject DeleteBusinessCardHelp(string HelpID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            IUserViewDAO uDAO = new UserViewDAO(new UserProfile());
            UserViewVO uVO = uDAO.FindById(uProfile.UserId);
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            try
            {
                if (!string.IsNullOrEmpty(HelpID))
                {
                    string[] messageIdArr = HelpID.Split(',');
                    bool isAllDelete = true;
                    for (int i = 0; i < messageIdArr.Length; i++)
                    {
                        try
                        {
                            cBO.DeleteHelpById(Convert.ToInt32(messageIdArr[i]));
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
        /// 对账
        /// </summary>
        /// <param name="HelpID">帮助ID</param>
        /// <returns></returns>
        [Route("Reconciliation"), HttpGet, Anonymous]
        public ResultObject Reconciliation(int start, int end)
        {
            try
            {
                JsApiPay Ja = new JsApiPay();

                BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
                List<OrderViewVO> oeder = cBO.FindOrderViewAllByPageIndex("ProdustsBusinessID=164 and Cost>0", start, end, "CreatedAt", "desc");

                int total = 0;
                List<OrderQueryData> rightOrder = new List<OrderQueryData>();
                List<OrderViewVO> errOrder = new List<OrderViewVO>();
                foreach (OrderViewVO item in oeder)
                {
                    OrderQueryData wp = Ja.OrderQueryResult(item.OrderNO);
                    if (wp != null)
                    {
                        rightOrder.Add(wp);
                    }
                    else
                    {
                        errOrder.Add(item);
                    }

                }

                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = rightOrder, Subsidiary = errOrder };
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = ex };
            }


        }

        /// <summary>
        /// 获取呼叫中心实例列表
        /// </summary>
        /// <param name="HelpID">帮助ID</param>
        /// <returns></returns>
        [Route("GetListInstances"), HttpGet, Anonymous]
        public ResultObject GetListInstances(int PageNumber, int PageSize)
        {
            try
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = AliCloudBO.ListInstances(PageNumber, PageSize) };
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = ex };
            }
        }

        /// <summary>
        /// 获取呼叫中心实例通话记录
        /// </summary>
        /// <param name="HelpID">帮助ID</param>
        /// <returns></returns>
        [Route("ListCallDetailRecords"), HttpGet, Anonymous]
        public ResultObject ListCallDetailRecords(string InstanceId, int PageNumber, int PageSize)
        {
            try
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = AliCloudBO.ListCallDetailRecords(InstanceId, PageNumber, PageSize) };
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = ex };
            }
        }

        /// <summary>
        /// 发起双呼
        /// </summary>
        /// <param name="Caller">主叫号码</param>
        /// <param name="Callee">被叫号码</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("StartBack2BackCall"), HttpGet]
        public ResultObject StartBack2BackCall(string Caller, string Callee, string token)
        {
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);

            if (pVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
            }

            if (!cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "CloudCall") && !cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Admin"))
            {
                return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
            }

            List<CallCenterVO> CallCenter = cBO.GetCallCenterList("BusinessID=" + pVO.BusinessID);

            if (CallCenter.Count <= 0) { return new ResultObject() { Flag = 0, Message = "您的企业暂未开通云呼功能，请联系客服开通!", Result = null }; }

            if (CallCenter[0].Status == 0) { return new ResultObject() { Flag = 0, Message = "您的云呼功能已暂停服务!", Result = null }; }

            List<CallNumberVO> CallNumber = cBO.GetCallNumberList(CallCenter[0].CallCenterID);
            StartBack2BackCallResponse Response = AliCloudBO.StartBack2BackCall(CallCenter[0].InstanceId, Caller, Callee, CallNumber[0].Number);

            if (Response.Body.Code == "OK")
            {
                return new ResultObject() { Flag = 1, Message = "外呼成功,请注意接听!", Result = Response.Body };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "外呼失败!", Result = Response.Body };
            }
        }
        /// <summary>
        /// 获取产品分类和VIP
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("FindProductSortList"), HttpGet]
        public ResultObject FindProductSortList(string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);

            List<InfoSortVO> dVO = cBO.FindInfoSortList("ProductSort", pVO.BusinessID);
            List<ShopVipVO> sVO = cBO.FindShopVipList("BusinessID = " + pVO.BusinessID);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = dVO, Subsidiary = sVO };
        }



        /// <summary>
        /// 添加或更新产品分类
        /// </summary>
        /// <param name="VO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateProductSort"), HttpPost]
        public ResultObject UpdateProductSort([FromBody] InfoSortVO InfoSortVO, string token)
        {
            if (InfoSortVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);


            if (!cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Product") && !cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Admin"))
            {
                return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
            }

            if (InfoSortVO.SortID > 0)
            {
                InfoSortVO sVO = cBO.FindInfoSortById(InfoSortVO.SortID);
                InfoSortVO.BusinessID = sVO.BusinessID;
                if (cBO.UpdateInfoSort(InfoSortVO))
                {
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = sVO.SortID };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }
            }
            else
            {
                InfoSortVO.BusinessID = pVO.BusinessID;
                InfoSortVO.Type = "ProductSort";

                int SortID = cBO.AddInfoSort(InfoSortVO);
                if (SortID > 0)
                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = SortID };
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
        }
        /// <summary>
        /// 删除产品分类
        /// </summary>
        /// <param name="VO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DelProductSort"), HttpGet]
        public ResultObject DelProductSort(int SortID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            InfoSortVO InfoSortVO = cBO.FindInfoSortById(SortID);

            if (InfoSortVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
            }
            if (!cBO.FindJurisdiction(pVO.PersonalID, InfoSortVO.BusinessID, "Product") && !cBO.FindJurisdiction(pVO.PersonalID, InfoSortVO.BusinessID, "Admin"))
            {
                return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
            }
            if (cBO.DeleteInfoSortById(SortID) > 0)
            {
                return new ResultObject() { Flag = 1, Message = "删除成功!" };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
            }
        }


        /// <summary>
        /// 添加拜访
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [Route("AddVisit"), HttpPost]
        public ResultObject AddVisit(VisitVO vo, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            try
            {
                BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
                PersonalVO per = cBO.FindPersonalByCustomerId(customerId);

                if (per != null)
                {
                    vo.AccessAt = DateTime.Now;
                    vo.Status = 1;
                    vo.PersonalID = per.PersonalID;
                    vo.AppType = per.AppType;
                    vo.Type = "";

                    if (cBO.AddVisit(vo))
                    {
                        var content = $"【悦售企业助手】提醒您：{vo.VisitDate.ToString("yyyy-MM-dd")} 收到一条企业上门拜访预约！请登录您的企业名片查看确认。";
                        var result = cBO.SendVisitMessage(vo.VisitPhone, content);
                        if (result.Success)
                        {
                            return new ResultObject() { Flag = 1, Message = "添加成功!", Result = result };
                        }
                        else
                        {
                            return new ResultObject() { Flag = 2, Message = "短信发送失败!", Result = null };
                        }
                    }
                    else
                    {
                        return new ResultObject() { Flag = 2, Message = "添加失败!", Result = null };
                    }
                }
                return new ResultObject() { Flag = 2, Message = "添加失败!", Result = null };
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "添加失败!" + ex.ToString(), Result = null };
            }
        }

        [Route("SendVisitMessages"), HttpGet, Anonymous]
        public ResultObject SendVisitMessages()
        {
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            var phone = "15574797453";
            var result = cBO.SendVisitMessage(phone, "");
            return new ResultObject() { Flag = 0, Message = "短信发送!", Result = result };

        }
        /// <summary>
        /// 修改拜访信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [Route("UpdateVisit"), HttpGet]
        public ResultObject UpdateVisit(string token, int VisitID, int Type)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            try
            {
                BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
                PersonalVO per = cBO.FindPersonalByCustomerId(customerId);
                VisitVO VO = cBO.FindVisit(VisitID);
                VO.Status = Type;
                VO.VisitID = VisitID;
                if (cBO.UpdateVisit(VO))
                {
                    return new ResultObject() { Flag = 1, Message = "修改成功!", Result = null };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "修改失败!", Result = null };
                }

            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }
        /// <summary>
        /// 获取感谢单列表（感谢我的）
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        [Route("GetVisitForMyList"), HttpPost]
        public ResultObject GetVisitForMyList([FromBody] ConditionModel condition, string token)
        {
            if (condition == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            else if (condition.Filter == null || condition.PageInfo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);

            string conditionStr = "ToPersonalID=" + pVO.PersonalID;

            Paging pageInfo = condition.PageInfo;

            List<VisitViewVO> list = new List<VisitViewVO>();
            int count = 0;

            list = cBO.FindVisitAllByPageIndex(conditionStr, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);
            count = cBO.FindVisitCount(conditionStr);
            foreach (var item in list)
            {
                item.Remark1 = item.AccessAt.ToString("yyyy") + "年" + item.AccessAt.ToString("MM") + "月" + item.AccessAt.ToString("dd") + "日";
            }
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list, Count = count };
        }


        /// <summary>
        /// 获取感谢单列表(我感谢的)
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        [Route("GetVisitList"), HttpPost]
        public ResultObject GetVisitList([FromBody] ConditionModel condition, string token)
        {
            try
            {
                if (condition == null)
                {
                    return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
                }
                else if (condition.Filter == null || condition.PageInfo == null)
                {
                    return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
                }

                UserProfile uProfile = CacheManager.GetUserProfile(token);
                CustomerProfile cProfile = uProfile as CustomerProfile;
                int customerId = cProfile.CustomerId;

                BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
                PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);

                string conditionStr = " PersonalID=" + pVO.PersonalID;

                Paging pageInfo = condition.PageInfo;

                List<VisitViewVO> list = new List<VisitViewVO>();
                int count = 0;

                list = cBO.FindVisitAllByPageIndex(conditionStr, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);
                count = cBO.FindVisitCount(conditionStr);

                foreach (var item in list)
                {
                    item.Remark1 = item.AccessAt.ToString("yyyy") + "年" + item.AccessAt.ToString("MM") + "月" + item.AccessAt.ToString("dd") + "日";
                }
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list, Count = count };
            }
            catch (Exception ex)
            {

                return new ResultObject() { Flag = 0, Message = ex.Message, Result = null };
            }

        }

        /// <summary>
        /// 获取感谢单详情
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        [Route("GetVisitDetail"), HttpGet]
        public ResultObject GetVisitDetail(string token, int VisitID)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            VisitViewVO vo = cBO.FindVisitInfo(VisitID);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = vo };
        }



        /// <summary>
        /// 获取新闻分类
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("FindNewsSortList"), HttpGet]
        public ResultObject FindNewsSortList(string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);

            List<InfoSortVO> dVO = cBO.FindInfoSortList("NewsSort", pVO.BusinessID);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = dVO };
        }

        /// <summary>
        /// 获取专栏分类
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("FindTypeSortList"), HttpGet]
        public ResultObject FindTypeSortList(string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);

            List<InfoSortVO> dVO = cBO.FindInfoSortList("special_column", pVO.BusinessID);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = dVO };
        }


        /// <summary>
        /// 添加或更新新闻分类
        /// </summary>
        /// <param name="VO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateNewsSort"), HttpPost]
        public ResultObject UpdateNewsSort([FromBody] InfoSortVO InfoSortVO, string token)
        {
            if (InfoSortVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);


            if (!cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Web") && !cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Admin"))
            {
                return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
            }

            if (InfoSortVO.SortID > 0)
            {
                InfoSortVO sVO = cBO.FindInfoSortById(InfoSortVO.SortID);
                InfoSortVO.BusinessID = sVO.BusinessID;
                if (cBO.UpdateInfoSort(InfoSortVO))
                {
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = sVO.SortID };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }
            }
            else
            {
                InfoSortVO.BusinessID = pVO.BusinessID;
                InfoSortVO.Type = "NewsSort";

                int SortID = cBO.AddInfoSort(InfoSortVO);
                if (SortID > 0)
                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = SortID };
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
        }



        /// <summary>
        /// 删除新闻分类
        /// </summary>
        /// <param name="VO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DelNewsSort"), HttpGet]
        public ResultObject DelNewsSort(int SortID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            InfoSortVO InfoSortVO = cBO.FindInfoSortById(SortID);

            if (InfoSortVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
            }
            if (!cBO.FindJurisdiction(pVO.PersonalID, InfoSortVO.BusinessID, "Web") && !cBO.FindJurisdiction(pVO.PersonalID, InfoSortVO.BusinessID, "Admin"))
            {
                return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
            }
            if (cBO.DeleteInfoSortById(SortID) > 0)
            {
                return new ResultObject() { Flag = 1, Message = "删除成功!" };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
            }
        }




        /// <summary>
        /// 添加或更新专栏分类
        /// </summary>
        /// <param name="VO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateSpecialSort"), HttpPost]
        public ResultObject UpdateSpecialSort([FromBody] InfoSortVO InfoSortVO, string token)
        {
            try
            {
                if (InfoSortVO == null)
                {
                    return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
                }
                LogBO _log = new LogBO(typeof(BusinessCardBO));
                UserProfile uProfile = CacheManager.GetUserProfile(token);
                CustomerProfile cProfile = uProfile as CustomerProfile;
                int customerId = cProfile.CustomerId;

                BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
                PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);


                if (!cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Web") && !cBO.FindJurisdiction(pVO.PersonalID, pVO.BusinessID, "Admin"))
                {
                    return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
                }

                if (InfoSortVO.SortID > 0)
                {
                    //根据id查询数据
                    InfoSortVO sVO = cBO.FindInfoSortById(InfoSortVO.SortID);
                    InfoSortVO.BusinessID = sVO.BusinessID;
                    if (cBO.UpdateInfoSort(InfoSortVO))
                    {
                        return new ResultObject() { Flag = 1, Message = "更新成功!", Result = sVO.SortID, Subsidiary = "修改", Subsidiary2 = InfoSortVO };
                    }
                    else
                    {
                        return new ResultObject() { Flag = 0, Message = "更新失败!", Result = InfoSortVO };
                    }
                }
                else
                {
                    InfoSortVO.BusinessID = pVO.BusinessID;
                    InfoSortVO.CreatedAt = DateTime.Now;
                    InfoSortVO.Type = "special_column";
                    int SortID = cBO.AddInfoSort(InfoSortVO);
                    if (SortID > 0)
                        return new ResultObject() { Flag = 1, Message = "添加成功!", Result = SortID, Subsidiary = "添加", Subsidiary2 = InfoSortVO };
                    else
                        return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
                }
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = ex.Message, Result = null };
            }

        }

        /// <summary>
        /// 获取提款银行账户
        /// </summary>
        /// <param name="HelpID">帮助ID</param>
        /// <returns></returns>
        [Route("GetBcBankAccount"), HttpGet, Anonymous]
        public ResultObject GetBcBankAccount(string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            if (pVO.BusinessID == 0)
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
            List<BcBankAccountVO> bankVO = cBO.FindBankAccountList("BusinessID=" + pVO.BusinessID);

            BcBankAccountVO bank = new BcBankAccountVO();
            bool isbank = bankVO.Count > 0;

            if (bankVO.Count > 0)
            {
                bank = bankVO[0];
            }
            else
            {
                bank = null;
            }


            if (bank != null)
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = bank };
            else
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };

        }

        /// <summary>
        /// 添加或更新提款银行账户
        /// </summary>
        /// <param name="VO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateBcBankAccount"), HttpPost]
        public ResultObject UpdateBcBankAccount([FromBody] BcBankAccountVO BcBankAccountVO, string token)
        {
            if (BcBankAccountVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            BusinessCardBO bBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = bBO.FindPersonalByCustomerId(customerId);

            if (BcBankAccountVO.BankAccountID > 0)
            {
                //是否有修改权限
                if (!bBO.FindJurisdiction(pVO.PersonalID, BcBankAccountVO.BusinessID, "Admin"))
                {
                    return new ResultObject() { Flag = 0, Message = "只有超级管理员才能修改提款账户!", Result = null };
                }
                if (bBO.UpdateBankAccount(BcBankAccountVO))
                {
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = BcBankAccountVO.BankAccountID };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }
            }
            else
            {
                BcBankAccountVO.BusinessID = pVO.BusinessID;

                int BankAccountID = bBO.AddBankAccount(BcBankAccountVO);
                if (BankAccountID > 0)
                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = BankAccountID };
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
        }
        /// <summary>
        /// 添加或更新活动
        /// </summary>
        /// <param name="VO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateActivity"), HttpPost]
        public ResultObject UpdateActivity([FromBody] ActivityModel dataVO, string token)
        {
            if (dataVO.ActivityInfo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            try
            {
                UserProfile uProfile = CacheManager.GetUserProfile(token);
                CustomerProfile cProfile = uProfile as CustomerProfile;
                int customerId = cProfile.CustomerId;
                BusinessCardBO bBO = new BusinessCardBO(new CustomerProfile());
                PersonalVO pVO = bBO.FindPersonalByCustomerId(customerId);
                ActivityVO aVO = dataVO.ActivityInfo;
                List<ActivityCountVO> acVO = dataVO.ActivityCountInfo;
                List<ActivityTicketVO> atVO = dataVO.ActivityTicketInfo;
                if (aVO.ActId > 0)
                {
                    //修改
                    bool flag = bBO.UpdateActivity(aVO);
                    if (flag)
                    {
                        foreach (var item in acVO)
                        {
                            if (item.ActCountId == 0)
                            {
                                item.ActId = aVO.ActId;
                                item.CreatedAt = DateTime.Now;
                                int ActCountId = bBO.AddActivityCount(item);
                            }

                        }
                        foreach (var item in atVO)
                        {
                            if (item.ActTicketId == 0)
                            {
                                item.ActId = aVO.ActId;
                                item.CreatedAt = DateTime.Now;
                                int ActTicketId = bBO.AddActivityTicket(item);
                            }
                        }
                        return new ResultObject() { Flag = 1, Message = "修改成功!", Result = true };
                    }
                }
                else
                {
                    //添加
                    aVO.CreatedAt = DateTime.Now;
                    aVO.PersonalID = pVO.PersonalID;
                    aVO.BusinessID = pVO.BusinessID;
                    if (aVO.CountType == 1)
                    {
                        aVO.StartAt = acVO[0].StartAt;
                        aVO.EndAt = acVO[0].EndAt;
                        aVO.SignAt = acVO[0].SignAt;
                    }
                    int ActivityID = bBO.AddActivity(aVO);
                    if (ActivityID > 0)
                    {
                        foreach (var item in acVO)
                        {
                            item.ActId = ActivityID;
                            item.ActCountId = 0;
                            item.CreatedAt = DateTime.Now;
                            int ActCountId = bBO.AddActivityCount(item);
                        }
                        foreach (var item in atVO)
                        {
                            item.ActId = ActivityID;
                            item.ActTicketId = 0;
                            item.CreatedAt = DateTime.Now;
                            int ActTicketId = bBO.AddActivityTicket(item);
                        }
                        return new ResultObject() { Flag = 1, Message = "添加成功!", Result = ActivityID };
                    }
                }
                return new ResultObject() { Flag = 0, Message = "操作失败!", Result = null };
            }
            catch (Exception ex)
            {

                return new ResultObject() { Flag = 0, Message = ex.Message, Result = null };
            }

        }

        /// <summary>
        /// 获取活动
        /// </summary>
        /// <param name="ActID">活动ID</param>
        /// <param name="CountID">活动场次ID</param>
        /// <returns></returns>
        [Route("GetActivityById"), HttpGet, Anonymous]
        public ResultObject GetActivityById(int ActID, int CountID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);


            ActivityVO vo = cBO.FindActivityById(ActID);

            if (vo != null)
            {
                List<ActivityNameModel> BossList = new List<ActivityNameModel>();
                List<ActivityNameModel> BossList2 = new List<ActivityNameModel>();
                List<ActivityNameModel> SignUp = new List<ActivityNameModel>();
                List<ActivityNameModel> ForPersonal = new List<ActivityNameModel>();
                List<ActivityNameModel> ImgUrls = new List<ActivityNameModel>();
                List<ActivityCountVO> ActtivityCountList = new List<ActivityCountVO>();
                if (CountID > 0)
                {
                    ActtivityCountList = cBO.FindActivityCountList("ActCountId=" + CountID);
                }
                else
                {
                    ActtivityCountList = cBO.FindActivityCountList("ActId=" + vo.ActId);
                }

                List<ActivityTicketVO> ActivityTicketList = cBO.FindActivityTicketList("ActId=" + vo.ActId);
                if (!string.IsNullOrEmpty(vo.BossList))
                {
                    foreach (var item in vo.BossList.Split(','))
                    {
                        ActivityNameModel info = new ActivityNameModel() { Name = item, Content = "" };
                        BossList.Add(info);
                    }
                }
                if (!string.IsNullOrEmpty(vo.BossList2))
                {
                    foreach (var item in vo.BossList2.Split(','))
                    {
                        ActivityNameModel info = new ActivityNameModel() { Name = item, Content = "" };
                        BossList2.Add(info);
                    }
                }
                if (!string.IsNullOrEmpty(vo.SignUp))
                {
                    foreach (var item in vo.SignUp.Split(','))
                    {
                        ActivityNameModel info = new ActivityNameModel() { Name = item, Content = "" };
                        SignUp.Add(info);
                    }
                }
                if (!string.IsNullOrEmpty(vo.ForPersonal))
                {
                    foreach (var item in vo.ForPersonal.Split(','))
                    {
                        ActivityNameModel info = new ActivityNameModel() { Name = item, Content = "" };
                        ForPersonal.Add(info);
                    }
                }
                if (!string.IsNullOrEmpty(vo.ImgUrls))
                {
                    foreach (var item in vo.ImgUrls.Split(','))
                    {
                        ActivityNameModel info = new ActivityNameModel() { Name = item, Content = "" };
                        ImgUrls.Add(info);
                    }
                }
                int endSign = 0;
                int isEnd = 0;

                int signDay = 0;
                int signShi = 0;
                int signFen = 0;

                if (vo.SignAt < DateTime.Now)
                {
                    endSign = 1;
                    signDay = 0;
                }
                else
                {
                    endSign = 0;
                    TimeSpan time = vo.SignAt - DateTime.Now;//报名剩余天-时-分
                    signDay = time.Days;
                    signFen = time.Minutes;
                    signShi = time.Hours;
                }
                if (vo.EndAt < DateTime.Now)
                {
                    isEnd = 1;
                    signDay = 0;
                }
                else
                {
                    isEnd = 0;
                }


                object obj = new
                {
                    Title = vo.Title,
                    AddressType = vo.AddressType,
                    Address = vo.Address,
                    Content = vo.Content,
                    BossList = BossList,
                    BossList2 = BossList2,
                    CountType = vo.CountType,
                    ActivityCountList = ActtivityCountList,
                    StartAt = vo.StartAt,
                    EndAt = vo.EndAt,
                    SignAt = vo.SignAt,
                    TicketList = ActivityTicketList,
                    SignUp = SignUp,
                    ForPersonal = ForPersonal,
                    ImgUrls = ImgUrls,
                    endSign = endSign,
                    isEnd = isEnd,
                    signDay = signDay,
                    signFen = signFen,
                    signShi = signShi,
                };

                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = obj };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取活动列表
        /// </summary>
        /// <param name="isSign">0：我发布的   1：我报名的</param>
        /// <returns></returns>
        [Route("GetActivityList"), HttpPost, Anonymous]
        public ResultObject GetActivityList(int isSign, string queryText, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            if (pVO != null)
            {
                string sql = string.Format("BusinessID={0} ", pVO.BusinessID);
                if (!string.IsNullOrEmpty(queryText))
                {
                    sql += string.Format(" and Title like '%{0}%'  ", queryText);//模糊查询标题或活动地址
                }
                List<ActivityVO> list = new List<ActivityVO>();
                List<ActivityAllModel> alllist = new List<ActivityAllModel>();
                if (isSign == 0)
                {
                    //我发布的活动
                    sql += string.Format(" and PersonalID ={0} order by SignAt", pVO.PersonalID);
                    list = cBO.FindActivityList(sql);
                    foreach (var avo in list)
                    {
                        ActivityAllModel allvo = new ActivityAllModel();
                        if (avo.CountType == 1)
                        {
                            List<ActivityCountVO> tvo = cBO.FindActivityCountList(" ActId=" + avo.ActId);
                            if (tvo != null && tvo.Count > 0)
                            {
                                avo.StartAt = tvo[0].StartAt;
                                avo.EndAt = tvo[0].EndAt;
                                avo.SignAt = tvo[0].SignAt;
                            }
                        }
                        if (avo.SignAt < DateTime.Now)
                        {
                            allvo.endSign = 1;
                            allvo.signDay = 0;
                        }
                        else
                        {
                            allvo.endSign = 0;
                            TimeSpan time = avo.SignAt - DateTime.Now;//报名剩余天-时-分
                            allvo.signDay = time.Days;
                            allvo.signFen = time.Minutes;
                            allvo.signShi = time.Hours;
                        }
                        if (avo.EndAt < DateTime.Now)
                        {
                            allvo.isEnd = 1;
                            allvo.signDay = 0;
                        }
                        else
                        {
                            allvo.isEnd = 0;
                        }
                        allvo.avo = avo;
                        allvo.cvo = null;
                        allvo.svo = null;
                        allvo.tvo = null;
                        alllist.Add(allvo);
                    }
                }
                else if (isSign == 1)
                {

                    //我报名的活动
                    List<ActivitySignTicketVO> tlist = cBO.FindActivitySignList(" PersonalID=" + pVO.PersonalID);

                    foreach (var item in tlist)
                    {
                        ActivityAllModel allvo = new ActivityAllModel();
                        ActivityVO avo = cBO.FindActivityById(item.ActId);
                        ActivityCountVO cvo = new ActivityCountVO();
                        ActivityTicketVO tvo = new ActivityTicketVO();
                        if (item.ActCountId > 0)
                        {
                            cvo = cBO.FindActivityCountById(item.ActCountId);
                            avo.StartAt = cvo.StartAt;
                            avo.EndAt = cvo.EndAt;
                            avo.SignAt = cvo.SignAt;
                        }
                        if (item.ActTicketId > 0)
                        {
                            tvo = cBO.FindActivityTicketById(item.ActTicketId);
                        }
                        if (avo.SignAt < DateTime.Now)
                        {
                            allvo.endSign = 1;
                            allvo.signDay = 0;
                        }
                        else
                        {
                            allvo.endSign = 0;
                            TimeSpan time = avo.SignAt - DateTime.Now;//报名剩余天-时-分
                            allvo.signDay = time.Days;
                            allvo.signFen = time.Minutes;
                            allvo.signShi = time.Hours;
                        }
                        if (avo.EndAt < DateTime.Now)
                        {
                            allvo.isEnd = 1;
                            allvo.signDay = 0;
                        }
                        else
                        {
                            allvo.isEnd = 0;
                        }
                        allvo.avo = avo;
                        allvo.cvo = cvo;
                        allvo.svo = item;
                        allvo.tvo = tvo;
                        if (!string.IsNullOrEmpty(queryText) && !avo.Title.Contains(queryText))
                        {
                            continue;
                        }
                        alllist.Add(allvo);
                    }
                    alllist.OrderByDescending(a => a.avo.EndAt).ToList();

                }
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = alllist };
            }
            return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
        }

        /// <summary>
        /// 删除活动
        /// </summary>
        /// <param name="VO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DelActivity"), HttpGet]
        public ResultObject DelActivity(int ActID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            ActivityVO Activityvo = cBO.FindActivityById(ActID);

            if (Activityvo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
            }

            if (cBO.DeleteActivityById(ActID) > 0)
            {
                return new ResultObject() { Flag = 1, Message = "删除成功!" };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
            }
        }

        /// <summary>
        /// 删除活动场次
        /// </summary>
        /// <param name="VO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DelActivityCount"), HttpGet]
        public ResultObject DelActivityCount(int ActCountId, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);

            if (cBO.DeleteActivityCountById(ActCountId) > 0)
            {
                return new ResultObject() { Flag = 1, Message = "删除成功!" };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
            }
        }

        /// <summary>
        /// 删除活动门票
        /// </summary>
        /// <param name="VO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DelActivityTicket"), HttpGet]
        public ResultObject DelActivityTicket(int ActTicketId, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);

            if (cBO.DeleteActivityTicketById(ActTicketId) > 0)
            {
                return new ResultObject() { Flag = 1, Message = "删除成功!" };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
            }
        }

        /// <summary>
        /// 更新活动门票
        /// </summary>
        /// <param name="VO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateActivityTicket"), HttpPost]
        public ResultObject UpdateActivityTicket([FromBody] ActivityTicketVO dataVO, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);

            if (dataVO.ActTicketId > 0)
            {
                if (cBO.UpdateActivityTicket(dataVO))
                {
                    return new ResultObject() { Flag = 1, Message = "更新成功!" };
                }
            }

            return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
        }


        /// <summary>
        /// 添加或更新活动报名
        /// </summary>
        /// <param name="VO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateSignTicket"), HttpPost, Anonymous]
        public ResultObject UpdateSignTicket([FromBody] ActivitySignTicketVO InfoVO, string token, int AppType)
        {
            if (InfoVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);



            if (InfoVO.ActivitySignTicketId > 0)
            {
                //修改
            }
            else
            {
                //添加
                InfoVO.CreatedAt = DateTime.Now;
                InfoVO.PersonalID = pVO.PersonalID;
                int ActivitySignTicketId = cBO.AddActivitySignTicket(InfoVO);

                if (ActivitySignTicketId > 0)
                {
                    try
                    {
                        ActivityVO avo = cBO.FindActivityById(InfoVO.ActId);
                        string codeimg = cBO.GetActHeQRImg(ActivitySignTicketId, InfoVO.ActId, avo.PersonalID, AppType);
                        InfoVO.ActivitySignTicketId = ActivitySignTicketId;
                        InfoVO.CodeUrl = codeimg;
                        cBO.UpdateActivitySignTicket(InfoVO);
                    }
                    catch (Exception)
                    {

                    }
                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = ActivitySignTicketId };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
                }
            }
            return new ResultObject() { Flag = 0, Message = "操作失败!", Result = null };
        }

        /// <summary>
        /// 获取门票列表
        /// </summary>
        /// <param name="ActId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [Route("GetActivityTicketList"), HttpGet, Anonymous]
        public ResultObject GetActivityTicketList(int ActId, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            if (pVO != null)
            {
                List<ActivityTicketVO> list = cBO.FindActivityTicketList("ActId=" + ActId);
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list };
            }
            return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
        }

        /// <summary>
        /// 获取报名信息
        /// </summary>
        /// <param name="ActId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [Route("GetActivitySignTicketList"), HttpGet, Anonymous]
        public ResultObject GetActivitySignTicketList(int ActId, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            if (pVO != null)
            {
                ActivityVO avo = cBO.FindActivityById(ActId);
                List<ActivityCountVO> clist = cBO.FindActivityCountList("ActId=" + ActId);
                List<ActivityTicketVO> tlist = cBO.FindActivityTicketList("ActId=" + ActId);
                List<ActivitySignTicketVO> slist = cBO.FindActivitySignList("ActId=" + ActId);
                List<AccessrecordsVO> acclist = cBO.FindAccessrecordsByCondtion("Type='ActId' and ById=" + ActId);
                List<object> olist = new List<object>();
                if (clist != null && clist.Count > 0 && avo.CountType == 1)
                {
                    //场次
                    foreach (var citem in clist)
                    {
                        //根据场次筛选后的报名列表
                        List<ActivitySignTicketVO> cslist = slist.FindAll(t => t.ActCountId == citem.ActCountId);
                        //对应 场次-门票 的报名人数
                        List<ActivityTicketCountModel> tclist = new List<ActivityTicketCountModel>();
                        //门票
                        foreach (var titem in tlist)
                        {
                            ActivityTicketCountModel tcVo = new ActivityTicketCountModel();
                            tcVo.tvo = titem;//门票
                                             //门票对应的报名人数
                            tcVo.SignCount = cslist.FindAll(t => t.ActTicketId == titem.ActTicketId).Count;
                            tclist.Add(tcVo);
                        }
                        object info = new
                        {
                            Title = citem.Title,//场次标题
                            StartAt = citem.StartAt,//活动场次时间
                            SignCount = cslist.Count,//报名人数
                            PayCost = cslist.Sum(t => t.Cost),//收款金额
                            TicketList = tclist,//场次-门票-报名人数 列表
                            Content = cslist.Select(t => t.Content).ToList()
                        };
                        olist.Add(info);
                    }
                }
                else
                {
                    //根据活动筛选后的报名列表
                    List<ActivitySignTicketVO> cslist = slist.FindAll(t => t.ActId == ActId);

                    //对应 场次-门票 的报名人数
                    List<ActivityTicketCountModel> tclist = new List<ActivityTicketCountModel>();
                    //门票
                    foreach (var titem in tlist)
                    {
                        ActivityTicketCountModel tcVo = new ActivityTicketCountModel();
                        tcVo.tvo = titem;//门票
                                         //门票对应的报名人数
                        tcVo.SignCount = cslist.FindAll(t => t.ActTicketId == titem.ActTicketId).Count;
                        tclist.Add(tcVo);
                    }
                    object info = new
                    {
                        Title = avo.Title,//场次标题
                        StartAt = avo.StartAt,//活动场次时间
                        SignCount = cslist.Count,//报名人数
                        PayCost = cslist.Sum(t => t.Cost),//收款金额
                        TicketList = tclist,//场次-门票-报名人数 列表
                        Content = cslist.Select(t => t.Content).ToList()
                    };
                    olist.Add(info);
                }

                object list = new
                {
                    Title = avo.Title,
                    CountType = avo.CountType,//场次类型0：单场次  1：多场次
                    SignCount = slist.Count,//报名人数
                    PayCost = slist.Sum(t => t.Cost),//收款金额
                    AccCount = acclist.Count,//浏览人数
                    CountList = olist,//场次集合
                };
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list };
            }
            return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
        }

        /// <summary>
        /// 获取推广信息
        /// </summary>
        /// <param name="ActId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [Route("GetActivityExList"), HttpGet, Anonymous]
        public ResultObject GetActivityExList(int ActId, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            if (pVO != null)
            {
                ActivityVO avo = cBO.FindActivityById(ActId);
                List<ActivitySignTicketVO> slist = cBO.FindActivitySignList("ActId=" + ActId + " and IsYao=1 and ForPersonalID=" + pVO.PersonalID);
                List<object> olist = new List<object>();
                List<AccessrecordsVO> acclist = cBO.FindAccessrecordsByCondtion("Type='ActId' and ById=" + ActId);
                foreach (var sitem in slist)
                {
                    string Name = "";
                    string Title = "";
                    decimal Cost = sitem.RewardCost;
                    if (!string.IsNullOrEmpty(sitem.Content))
                    {
                        Name = sitem.Content.Split(',')[0];
                    }
                    if (sitem.ActTicketId > 0)
                    {
                        ActivityTicketVO tvo = cBO.FindActivityTicketById(sitem.ActTicketId);
                        Title = tvo.Title;
                    }

                    object info = new
                    {
                        Name = Name,//报名人
                        Title = Title,//门票标题
                        Cost = Cost,//奖励金额
                    };
                    olist.Add(info);
                }
                string imgurl = "";
                if (!string.IsNullOrEmpty(avo.ImgUrls))
                {
                    imgurl = avo.ImgUrls.Split(',')[0];
                }
                object list = new
                {
                    ImgUrl = imgurl,
                    CountType = avo.CountType,
                    Title = avo.Title,
                    StartAt = avo.StartAt,
                    EndAt = avo.EndAt,
                    SignCount = slist.Count,//报名人数
                    PayCost = slist.Sum(t => t.RewardCost),//金额
                    AccCount = acclist.Count,//浏览人数
                    YaoList = olist,//报名集合
                };
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list };
            }
            return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
        }

        /// <summary>
        /// 获取分享二维码（活动）
        /// </summary>
        /// <param name="ActId"></param>
        /// <param name="token"></param>
        /// <param name="PersonalID"></param>
        /// <param name="BusinessID"></param>
        /// <param name="Code"></param>
        /// <param name="AppType"></param>
        /// <returns></returns>
        [Route("GetActShareImg"), HttpGet, Anonymous]
        public ResultObject GetActShareImg(int ActId, string token, int PersonalID, int AppType)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            if (ActId > 0)
            {
                string codeimg = cBO.GetActQRImg(ActId, PersonalID, AppType);
                ActivityVO npVO = new ActivityVO();
                npVO.ActId = ActId;
                npVO.CodeImg = codeimg;
                bool flag = cBO.UpdateActivity(npVO);
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = codeimg };
            }
            return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
        }

        /// <summary>
        /// 扫二维码（活动核销）
        /// </summary>
        /// <param name="SignId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [Route("DoActHe"), HttpGet, Anonymous]
        public ResultObject DoActHe(int SignId, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            if (SignId > 0)
            {
                ActivitySignTicketVO svo = cBO.FindActivitySignById(SignId);
                ActivityVO avo = cBO.FindActivityById(svo.ActId);
                if (avo.PersonalID != pVO.PersonalID)
                {

                    return new ResultObject() { Flag = 0, Message = "请让发布者来扫码核销!", Result = null };
                }
                if (svo.IsUse == 0)
                {
                    return new ResultObject() { Flag = 0, Message = "此二维码已使用!使用时间：" + svo.UseAt, Result = null };
                }
                svo.IsUse = 1;
                svo.UseAt = DateTime.Now;
                bool flag = cBO.UpdateActivitySignTicket(svo);
                return new ResultObject() { Flag = 1, Message = "核销成功!", Result = null };
            }
            return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
        }

        /// <summary>
        /// 导出报名查询（Excel）
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        [Route("GetActExcel"), HttpGet]
        public ResultObject GetActExcel(int ActId, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            try
            {
                ActivityVO avo = cBO.FindActivityById(ActId);
                List<ActivityCountVO> clist = cBO.FindActivityCountList("ActId=" + ActId);
                List<ActivityTicketVO> tlist = cBO.FindActivityTicketList("ActId=" + ActId);
                List<ActivitySignTicketVO> slist = cBO.FindActivitySignList("ActId=" + ActId);
                List<AccessrecordsVO> acclist = cBO.FindAccessrecordsByCondtion("Type='ActId' and ById=" + ActId);
                //List<object> olist = new List<object>();
                DataTable dt = new DataTable();
                dt.Columns.Add("", typeof(String));
                dt.Columns.Add("", typeof(String));
                dt.Columns.Add("", typeof(String));
                //标题
                DataRow row = dt.NewRow();
                row[0] = avo.Title;
                dt.Rows.Add(row);
                //统计
                row = dt.NewRow();
                row[0] = "总报名数";
                row[1] = "总收款金额";
                row[2] = "总浏览人数";
                dt.Rows.Add(row);

                row = dt.NewRow();
                row[0] = slist.Count;
                row[1] = slist.Sum(t => t.Cost);
                row[2] = acclist.Count;
                dt.Rows.Add(row);

                //场次
                if (clist != null && clist.Count > 0 && avo.CountType == 1)
                {
                    //场次
                    foreach (var citem in clist)
                    {
                        //根据场次筛选后的报名列表
                        List<ActivitySignTicketVO> cslist = slist.FindAll(t => t.ActCountId == citem.ActCountId);
                        //对应 场次-门票 的报名人数
                        List<ActivityTicketCountModel> tclist = new List<ActivityTicketCountModel>();

                        //空格
                        row = dt.NewRow();
                        row[0] = "";
                        dt.Rows.Add(row);

                        row = dt.NewRow();
                        row[0] = "场次标题";
                        row[1] = "时间";
                        dt.Rows.Add(row);

                        row = dt.NewRow();
                        row[0] = citem.Title;
                        row[1] = citem.StartAt.ToString("yyyy-MM-dd");
                        dt.Rows.Add(row);

                        //空格
                        row = dt.NewRow();
                        row[0] = "";
                        dt.Rows.Add(row);

                        row = dt.NewRow();
                        row[0] = "报名人数";
                        row[1] = "收款金额";
                        dt.Rows.Add(row);

                        row = dt.NewRow();
                        row[0] = cslist.Count;
                        row[1] = cslist.Sum(t => t.Cost);
                        dt.Rows.Add(row);

                        //空格
                        row = dt.NewRow();
                        row[0] = "";
                        dt.Rows.Add(row);

                        //门票
                        row = dt.NewRow();
                        row[0] = "门票名称";
                        row[1] = "门票详情";
                        row[2] = "报名数";
                        dt.Rows.Add(row);
                        foreach (var titem in tlist)
                        {
                            row = dt.NewRow();
                            row[0] = titem.Title;
                            row[1] = titem.Content;
                            row[2] = cslist.FindAll(t => t.ActTicketId == titem.ActTicketId).Count;
                            dt.Rows.Add(row);
                        }
                        //空格
                        row = dt.NewRow();
                        row[0] = "";
                        dt.Rows.Add(row);

                        //报名信息
                        row = dt.NewRow();
                        row[0] = "报名信息";
                        dt.Rows.Add(row);
                        foreach (var csitem in cslist)
                        {
                            row = dt.NewRow();
                            row[0] = "";
                            row[1] = csitem.Content;
                            dt.Rows.Add(row);
                        }

                    }
                }
                else
                {
                    //根据活动筛选后的报名列表
                    List<ActivitySignTicketVO> cslist = slist.FindAll(t => t.ActId == ActId);

                    //对应 场次-门票 的报名人数
                    List<ActivityTicketCountModel> tclist = new List<ActivityTicketCountModel>();

                    //空格
                    row = dt.NewRow();
                    row[0] = "";
                    dt.Rows.Add(row);

                    row = dt.NewRow();
                    row[0] = "场次标题";
                    row[1] = "时间";
                    dt.Rows.Add(row);

                    row = dt.NewRow();
                    row[0] = avo.Title;
                    row[1] = avo.StartAt.ToString("yyyy-MM-dd");
                    dt.Rows.Add(row);

                    //空格
                    row = dt.NewRow();
                    row[0] = "";
                    dt.Rows.Add(row);

                    row = dt.NewRow();
                    row[0] = "报名人数";
                    row[1] = "收款金额";
                    dt.Rows.Add(row);

                    row = dt.NewRow();
                    row[0] = cslist.Count;
                    row[1] = cslist.Sum(t => t.Cost);
                    dt.Rows.Add(row);

                    //空格
                    row = dt.NewRow();
                    row[0] = "";
                    dt.Rows.Add(row);

                    //门票
                    row = dt.NewRow();
                    row[0] = "门票名称";
                    row[1] = "门票详情";
                    row[2] = "报名数";
                    dt.Rows.Add(row);
                    foreach (var titem in tlist)
                    {
                        row = dt.NewRow();
                        row[0] = titem.Title;
                        row[1] = titem.Content;
                        row[2] = cslist.FindAll(t => t.ActTicketId == titem.ActTicketId).Count;
                        dt.Rows.Add(row);
                    }
                    //空格
                    row = dt.NewRow();
                    row[0] = "";
                    dt.Rows.Add(row);

                    //报名信息
                    row = dt.NewRow();
                    row[0] = "报名信息";
                    dt.Rows.Add(row);
                    foreach (var csitem in cslist)
                    {
                        row = dt.NewRow();
                        row[0] = "";
                        row[1] = csitem.Content;
                        dt.Rows.Add(row);
                    }

                }
                if (dt != null && dt.Rows.Count > 0)
                {
                    string FileName = EPPlus.DataToExcel(dt, "BcActExcel/", DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".xlsx");

                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = FileName };
                }
            }
            catch (Exception)
            {

            }
            return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };

        }

        /// <summary>
        /// 迁移官网数据
        /// </summary>
        /// <param name="BusinessID">ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("CopyWeb"), HttpGet, Anonymous]
        public ResultObject CopyWeb(int BusinessID, int toBusinessID)
        {
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());

            bool bVO = cBO.CopyWeb(BusinessID, toBusinessID);
            cBO.CopyInfo(BusinessID, toBusinessID);
            cBO.CopyProducts(BusinessID, toBusinessID);
            if (bVO)
                return new ResultObject() { Flag = 1, Message = "复制成功!", Result = null };
            else
                return new ResultObject() { Flag = 0, Message = "复制失败!", Result = null };
        }

        #region 活动

        /// <summary>
        /// 获取我的报名列表和我发布的活动列表
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("FindPartyAndSignUpByCustomerId"), HttpGet]
        public ResultObject FindPartyAndSignUpByCustomerId(string token, string keyword = "")
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO bcBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = bcBO.FindPersonalByCustomerId(customerId);
            List<BCPartySignUpViewVO> uVO = bcBO.FindPartyAndSignUpByCustomerId(customerId, pVO.AppType);
            List<BCPartySignUpViewVO> BCPartyVO = new List<BCPartySignUpViewVO>();

            if (keyword == "")
            {
                //保留前50条数据
                for (int i = 0; i < uVO.Count && i < 50; i++)
                {
                    BCPartyVO.Add(uVO[i]);
                }
            }
            else
            {
                //保留符合关键字的活动
                for (int i = 0; i < uVO.Count; i++)
                {
                    if (uVO[i].Title.IndexOf(keyword) > -1)
                        BCPartyVO.Add(uVO[i]);
                }
            }

            if (BCPartyVO != null)
            {
                if (BCPartyVO.Count > 0)
                {
                    #region 获取权限

                    JurisdictionViewVO jVO = bcBO.FindJurisdictionView(pVO.PersonalID, pVO.BusinessID);

                    #endregion

                    object res = new { BCPartyList = BCPartyVO, Jurisdiction = jVO };
                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = res };
                }
                else
                {
                    return new ResultObject() { Flag = 2, Message = "没有活动!", Result = customerId };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取首页活动列表
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("FindAllPartyAndSignUp"), HttpGet]
        public ResultObject FindAllPartyAndSignUp(string token)
        {
            try
            {
                UserProfile uProfile = CacheManager.GetUserProfile(token);
                CustomerProfile cProfile = uProfile as CustomerProfile;
                int customerId = cProfile.CustomerId;

                BusinessCardBO bcBO = new BusinessCardBO(new CustomerProfile());
                PersonalVO pVO = bcBO.FindPersonalByCustomerId(customerId);
                //IsDisplayIndex=1 AND EndTime > now() AND Status<>0 AND SignUpStatus<>2 AND
                string condition = "  AppType=" + pVO.AppType;
                List<BCPartySignUpViewVO> uVO = bcBO.FindAllPartyAndSignUp(condition, pVO.AppType);
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO };
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = ex };
            }
        }

        /// <summary>
        /// 添加或更新活动
        /// </summary>
        /// <param name="partyModelVO">任务VO</param>
        /// <param name="ContactsListID">联系人ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateParty"), HttpPost]
        public ResultObject UpdateParty([FromBody] BCPartyModel partyModelVO, string ContactsListID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            if (partyModelVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            //CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            //CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            BusinessCardBO bcBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = bcBO.FindPersonalByCustomerId(customerId);
            ///*审核文本是否合法*/
            //if (!cBO.msg_sec_check(partyModelVO) && cProfile.CustomerId != 19894)
            //{
            //    return new ResultObject() { Flag = 0, Message = "有政治敏感或违法关键词，请重新填写!", Result = null };
            //}
            /*审核文本是否合法*/

            BCPartyVO bcPartyVO = partyModelVO.BCParty;
            bcPartyVO.AppType = pVO.AppType;

            if (bcPartyVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            if (bcPartyVO.StartTime < DateTime.Now && bcPartyVO.Type == 1)
            {
                return new ResultObject() { Flag = 0, Message = "开始时间不能小于当前时间!", Result = null };
            }

            if (bcPartyVO.Address != "")
            {
                /*
                WeiXinGeocoder Geocoder = cBO.getLatitudeAndLongitude(cardPartyVO.Address);
                if (Geocoder != null)
                {
                    cardPartyVO.latitude = Geocoder.result.location.lat;
                    cardPartyVO.longitude = Geocoder.result.location.lng;
                } else
                {
                    cardPartyVO.latitude = 0;
                    cardPartyVO.longitude = 0;
                }*/
            }
            List<BCPartySignUpFormVO> BCPartySignUpFormVOList = partyModelVO.BCPartySignUpForm;
            List<BCPartyCostVO> BCPartyCost = partyModelVO.BCPartyCost;
            List<BCPartyContactsVO> BCPartyContactsVOList = new List<BCPartyContactsVO>();

            if (ContactsListID != "")
            {
                string[] ContactsList = ContactsListID.Split(',');
                List<BCPartyContactsVO> ContactsVO = new List<BCPartyContactsVO>();
                for (int i = 0; i < ContactsList.Length; i++)
                {
                    BCPartyContactsVO cVO = new BCPartyContactsVO();
                    cVO.PartyContactsID = 0;
                    cVO.PersonalID = Convert.ToInt32(ContactsList[i]);
                    cVO.PartyID = bcPartyVO.PartyID;
                    ContactsVO.Add(cVO);
                }
                BCPartyContactsVOList = ContactsVO;
            }
            else
            {
                BCPartyContactsVOList = partyModelVO.BCPartyContacts;
            }

            if (bcPartyVO != null)
            {
                if (bcPartyVO.PartyID < 1)
                {
                    bcPartyVO.CreatedAt = DateTime.Now;
                    bcPartyVO.Status = 1;
                    bcPartyVO.PersonalID = pVO.PersonalID;
                    bcPartyVO.BusinessID = pVO.BusinessID;

                    if (cProfile != null)
                        bcPartyVO.CustomerId = cProfile.CustomerId;

                    //try
                    //{
                    //    //为活动创建名片组
                    //    CardGroupVO CardGroupVO = new CardGroupVO();
                    //    CardGroupVO.CreatedAt = DateTime.Now;
                    //    CardGroupVO.CustomerId = cProfile.CustomerId;
                    //    CardGroupVO.JoinSetUp = 1;
                    //    CardGroupVO.GroupName = "活动-" + cardPartyVO.Title;

                    //    int GroupID = cBO.AddCardGroup(CardGroupVO);
                    //    if (GroupID > 0)
                    //    {
                    //        //将联系人作为管理员加入名片组
                    //        cardPartyVO.GroupID = GroupID;
                    //        List<int> Cid = new List<int>();
                    //        for (int i = 0; i < CardPartyContactsVOList.Count; i++)
                    //        {
                    //            CardDataVO cVO = cBO.FindCardById(CardPartyContactsVOList[i].CardID);
                    //            if (cVO != null)
                    //            {
                    //                int CustomerId = cVO.CustomerId;
                    //                if (!Cid.Contains(CustomerId))
                    //                {
                    //                    CardGroupCardVO cgcVO = new CardGroupCardVO();
                    //                    cgcVO.CustomerId = CustomerId;
                    //                    cgcVO.GroupID = GroupID;
                    //                    cgcVO.Status = 3;
                    //                    cgcVO.CreatedAt = DateTime.Now;
                    //                    cgcVO.CardID = CardPartyContactsVOList[i].CardID;
                    //                    cBO.AddCardToGroup(cgcVO);

                    //                    Cid.Add(CustomerId);
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                    //catch
                    //{

                    //}
                    BCPartyModel p = new BCPartyModel();
                    p.BCPartyContacts = BCPartyContactsVOList;
                    p.BCPartyCost = BCPartyCost;
                    p.BCPartySignUpForm = BCPartySignUpFormVOList;
                    p.BCParty = bcPartyVO;
                    string serializedString = JsonConvert.SerializeObject(p);
                    int requireId = bcBO.AddParty(bcPartyVO, BCPartyContactsVOList, BCPartySignUpFormVOList, BCPartyCost);
                    if (requireId > 0)
                        return new ResultObject() { Flag = 1, Message = "添加成功!", Result = requireId };
                    else
                        return new ResultObject() { Flag = 0, Message = "添加失败!", Result = requireId + "测试:" + serializedString };
                }
                else
                {
                    BCPartyVO rVO = bcBO.FindPartyById(bcPartyVO.PartyID);
                    if (rVO == null)
                    {
                        return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                    }
                    if (rVO.SignUpTime < DateTime.Now && rVO.Type == 1)
                    {
                        return new ResultObject() { Flag = 0, Message = "报名已截止，不能修改活动!", Result = null };
                    }

                    if (cProfile == null)
                    {
                        return new ResultObject() { Flag = 0, Message = "您不是活动发布者，更新失败!", Result = null };
                    }
                    bool isSuccess = false;
                    if (cProfile.CustomerId == rVO.CustomerId)
                    {
                        if (bcPartyVO.limitPeopleNum == 0)
                        {
                            bcPartyVO.limitPeopleNum = 0;
                        }
                        isSuccess = bcBO.UpdateParty(bcPartyVO, BCPartyContactsVOList, BCPartySignUpFormVOList, BCPartyCost);
                    }
                    else if (bcBO.isPartyContacts(bcPartyVO.PartyID, cProfile.CustomerId))
                    {
                        BCPartyVO bcpVO = new BCPartyVO();
                        bcpVO.PartyID = bcPartyVO.PartyID;
                        bcpVO.Title = bcPartyVO.Title;
                        bcpVO.Content = bcPartyVO.Content;
                        bcpVO.Details = bcPartyVO.Details;
                        bcpVO.Details2 = bcPartyVO.Details2;
                        bcpVO.Host = bcPartyVO.Host;
                        bcpVO.PartyTag = bcPartyVO.PartyTag;
                        bcpVO.MainImg = bcPartyVO.MainImg;
                        bcpVO.Address = bcPartyVO.Address;
                        bcpVO.latitude = bcPartyVO.latitude;
                        bcpVO.longitude = bcPartyVO.longitude;
                        bcpVO.style = bcPartyVO.style;
                        bcpVO.Audio = bcPartyVO.Audio;
                        bcpVO.AudioName = bcPartyVO.AudioName;

                        isSuccess = bcBO.UpdateParty(bcpVO);
                    }

                    if (isSuccess)
                    {
                        try
                        {
                            if (rVO.Type == 3)
                            {
                                BCPartyVO bcpVO = new BCPartyVO();
                                bcpVO.PartyID = rVO.PartyID;
                                bcpVO.LuckDrawShareImg = bcBO.getPosterByPartyID(rVO.PartyID, 38);
                                bcBO.UpdateParty(bcpVO);
                            }
                        }
                        catch
                        {

                        }

                        return new ResultObject() { Flag = 1, Message = "更新成功!", Result = bcPartyVO.PartyID };
                    }

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
        /// 更新活动（修改显示隐藏联系人等设置）
        /// </summary>
        /// <param name="cardPartyVO">活动VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateParty"), HttpPost]
        public ResultObject UpdateParty([FromBody] BCPartyVO bcPartyVO, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            if (bcPartyVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            if (bcPartyVO != null)
            {
                CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
                CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
                BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
                BCPartyVO rVO = cBO.FindPartyById(bcPartyVO.PartyID);
                if (rVO != null)
                {
                    if (cProfile != null)
                    {
                        //如果提交者并非该任务所属的会员，直接禁止更新
                        if (cProfile.CustomerId != rVO.CustomerId)
                            return new ResultObject() { Flag = 0, Message = "您不是活动发布者，更新失败!", Result = null };
                    }
                    else
                    {
                        return new ResultObject() { Flag = 0, Message = "您不是活动发布者，更新失败!", Result = null };
                    }
                }

                rVO.isDisplayContacts = bcPartyVO.isDisplayContacts;
                rVO.isDisplayCost = bcPartyVO.isDisplayCost;
                rVO.isDisplaySignup = bcPartyVO.isDisplaySignup;
                rVO.isPromotionAward = bcPartyVO.isPromotionAward;
                rVO.isPromotionSignup = bcPartyVO.isPromotionSignup;
                rVO.isPromotionRead = bcPartyVO.isPromotionRead;
                rVO.isClickSignup = bcPartyVO.isClickSignup;

                bool isSuccess = cBO.UpdateParty(rVO);
                if (isSuccess)
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = rVO.PartyID };
                else
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }
        }


        /// <summary>
        /// 添加或更新活动
        /// </summary>
        /// <returns></returns>
        [Route("AddTest"), HttpPost]
        public ResultObject AddTest(string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            BusinessCardBO BO = new BusinessCardBO(new CustomerProfile());
            BO.AddTest();
            return new ResultObject() { Flag = 1, Message = "成功!", Result = "ces" };
        }

        /// <summary>
        /// 获取活动详情，匿名
        /// </summary>
        /// <param name="PartyID">活动ID</param>
        /// <returns></returns>
        [Route("GetPartySite"), HttpGet, Anonymous]
        public ResultObject GetPartySite(int PartyID, int AppType = 0, int InviterCID = 0)
        {
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());


            BCPartyVO cVO = cBO.FindPartyById(PartyID);
            if (cVO != null)
            {

                if (cVO.QRCodeImg == "")
                {
                    cVO.QRCodeImg = cBO.GetBCPartyQR(PartyID);
                }

                if (cVO.QRSignInImg == "")
                {
                    cVO.QRSignInImg = cBO.GetBCPartySignUpQRByUser(PartyID);
                }

                if (cVO.SignupConditions == 1 && cVO.ConditionsQR == "")
                {
                    //cVO.ConditionsQR = cBO.getQRIMGByIDAndType(PartyID, 7);
                }

                if (cVO.LuckDrawShareImg == "" && cVO.Type == 3)
                {
                    // cVO.LuckDrawShareImg = cBO.getPosterByPartyID(PartyID, 38);
                }

                BCPartyModel partyModelVO = new BCPartyModel();
                partyModelVO.BCParty = cVO;
                partyModelVO.BCPartySignUp = cBO.FindSignUpByCondtion("PartyID=" + PartyID + " GROUP BY CustomerId ORDER BY CreatedAt desc LIMIT 30"); ;


                if (cVO.RecordSignUpCount <= 1)
                {
                    cVO.RecordSignUpCount = cBO.FindBCPartSignInSumCount("Number", "PartyID=" + cVO.PartyID + " and (SignUpStatus=1 or SignUpStatus=0)");
                    cBO.UpdateParty(cVO);
                }

                partyModelVO.BCPartySignCount = cVO.RecordSignUpCount;
                if (partyModelVO.BCPartySignUp.Count > 50)
                {
                    List<BCPartySignUpVO> CardPartySignUpVO = new List<BCPartySignUpVO>();
                    //保留前50条数据
                    for (int i = 0; i < partyModelVO.BCPartySignUp.Count && i < 50; i++)
                    {
                        CardPartySignUpVO.Add(partyModelVO.BCPartySignUp[i]);
                    }
                    partyModelVO.BCPartySignUp = CardPartySignUpVO;
                }

                partyModelVO.BCPartyCost = cBO.FindCostByPartyID(PartyID);
                partyModelVO.BCPartySignUpForm = cBO.FindSignUpFormByPartyID(PartyID);
                if (cVO.Type != 3)
                {
                    //partyModelVO.BCPartyInviterList = cBO.FindSignUpViewInviterByPartyID(PartyID);
                    partyModelVO.BCPartyContactsView = cBO.FindPartyContactsViewByPartyId(PartyID);
                }
                else
                {
                    //partyModelVO.CardPartyInviterList = new List<CardPartySignUpViewVO>();
                    partyModelVO.BCPartyContactsView = new List<BCPartyContactsViewVO>();
                }
                partyModelVO.BCPartyContactsView = cBO.FindPartyContactsViewByPartyId(PartyID);
                PersonalVO pVO = cBO.FindPersonalByCustomerId(cVO.CustomerId);
                //List<CardDataVO> CardDataVO = cBO.FindCardByCustomerId(cVO.CustomerId);
                if (pVO != null)
                {
                    partyModelVO.Personal = pVO;
                }

                BusinessCardBO BusinessCardBO = new BusinessCardBO(new CustomerProfile());
                BusinessCardVO bVO = new BusinessCardVO();
                if (cVO.BusinessID != 0)
                {
                    bVO = BusinessCardBO.FindBusinessCardById(cVO.BusinessID);
                    //清除营业执照等保密信息

                    if (bVO != null)
                    {
                        bVO.BusinessLicenseImg = "";
                    }
                }

                CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
                CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(cVO.CustomerId);
                int ChangePartyID = 0;
                BCPartyVO AdParty = null;
                List<BCPartyVO> PartyVO = new List<BCPartyVO>();
                if (cVO.Type == 3)
                {
                    try
                    {
                        PartyVO = cBO.FindBCPartyByCondtion("Type = 3 and PartyLuckDrawStatus = 0 and (isHot = 1 or isIndex = 1) and SignUpTime > now()");
                        Random Rdm = new Random();
                        if (PartyVO.Count > 0)
                        {
                            ChangePartyID = PartyVO[Rdm.Next(0, PartyVO.Count - 1)].PartyID;
                            AdParty = PartyVO[Rdm.Next(0, PartyVO.Count - 1)];
                        }

                        //List<CardPartyCostVO> FirstPrizeList = cBO.FindCostByFirstPrize(AdParty.PartyID);
                        //if (FirstPrizeList.Count > 0)
                        //{
                        //    AdParty.FirstPrize = FirstPrizeList[0];
                        //}
                        //else
                        //{
                        //    List<CardPartyCostVO> Cost = cBO.FindCostByPartyID(AdParty.PartyID);
                        //    if (Cost.Count > 0)
                        //    {
                        //        AdParty.FirstPrize = Cost[0];
                        //    }
                        //}
                    }
                    catch
                    {

                    }

                }

                CardDataVO InviterDataVO = null;

                //if (InviterCID > 0)
                //{
                //    List<CardDataVO> InviterCardDataVO = cBO.FindCardByCustomerId(InviterCID);
                //    if (InviterCardDataVO.Count > 0)
                //    {
                //        InviterDataVO = InviterCardDataVO[0];
                //    }
                //}

                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = partyModelVO, Subsidiary = bVO, Subsidiary2 = new { CustomerVO2.isIdCard, ChangePartyID, AdParty, InviterDataVO } };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取活动报名费用，匿名
        /// </summary>
        /// <param name="PartyID">活动ID</param>
        /// <returns></returns>
        [Route("GetPartyCost"), HttpGet, Anonymous]
        public ResultObject GetPartyCost(int PartyID, int AppType = 1, string token = "")
        {
            try
            {
                BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
                List<BCPartyCostVO> cVO = cBO.FindCostByPartyID(PartyID);
                BCPartyVO PartyVO = cBO.FindPartyById(PartyID);
                int customerId = 0;
                if (token != "")
                {
                    UserProfile uProfile = CacheManager.GetUserProfile(token);
                    CustomerProfile cProfile = uProfile as CustomerProfile;
                    customerId = cProfile.CustomerId;
                }

                foreach (BCPartyCostVO item in cVO)
                {
                    List<BCPartySignUpViewVO> CostSignUpVO = cBO.PartyCostSignUpView(item.Names, PartyID);
                    item.QuantitySold = CostSignUpVO.Sum(p => p.Number);

                    if (PartyVO.isPromotionRead == 1 && item.PromotionRead > 0)
                    {
                        item.isPromotionRead = 1;
                        if (customerId > 0)
                            item.MyPromotionRead = cBO.FindAccessrecordsCount("ShareCustomerId=" + customerId + " and CustomerId<>" + customerId + " and ById=" + PartyID + " and Type='ReadParty'", true);
                        else
                        {
                            item.MyPromotionRead = 0;
                        }
                    }

                    if (PartyVO.isPromotionSignup == 1 && item.PromotionSignup > 0)
                    {
                        item.isPromotionSignup = 1;
                        if (customerId > 0)
                            item.MyPromotionSignup = cBO.FindPartyOrderTotalCount("InviterCID=" + customerId + " and CustomerId<>" + customerId + " and Status=1 and PartyID=" + PartyID);
                        else
                        {
                            item.MyPromotionSignup = 0;
                        }
                    }
                }

                if (cVO != null)
                {
                    if (cVO.Count > 0)
                    {
                        int MyReadSigUp = cBO.FindPartyOrderTotalCount("CustomerId=" + customerId + " and Status=1 and PartyID=" + PartyID + " and (PromotionReadStatus=1 or PromotionSignupStatus=1)");
                        return new ResultObject() { Flag = 1, Message = "获取成功!", Result = cVO, Subsidiary = MyReadSigUp };
                    }
                    else
                    {
                        return new ResultObject() { Flag = 2, Message = "无费用信息!", Result = null };
                    }
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
                }
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = ex };
            }
        }

        /// <summary>
        /// 设置活动报名费用推广奖励
        /// </summary>
        /// <param name="PartyCostID">费用ID</param>
        /// <param name="PromotionAward">返佣比例</param>
        /// <returns></returns>
        [Route("GetPartyCost"), HttpGet]
        public ResultObject GetPartyCost(int PartyCostID, int PromotionAward, string token, string Promotion = "Award")
        {
            try
            {
                UserProfile uProfile = CacheManager.GetUserProfile(token);
                CustomerProfile cProfile = uProfile as CustomerProfile;
                int customerId = cProfile.CustomerId;

                CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
                CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
                BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());

                BCPartyCostVO CostVO = cBO.FindCostById(PartyCostID);
                if (CostVO == null || PromotionAward < 0 || PromotionAward > 100)
                {
                    return new ResultObject() { Flag = 0, Message = "参数错误", Result = null };
                }

                BCPartyVO rVO = cBO.FindPartyById(CostVO.PartyID);
                if (rVO.CustomerId != customerId)
                {
                    return new ResultObject() { Flag = 0, Message = "你没有权限", Result = null };
                }


                if (Promotion == "Award")
                {
                    //如果是独立商户
                    EcommerceBO eBO = new EcommerceBO();
                    wxMerchantVO mVO = eBO.getMyMerchant(customerId);
                    if (mVO != null)
                    {
                        int p = 30 - mVO.SplitProportion;
                        if (p - PromotionAward < 0)
                        {
                            return new ResultObject() { Flag = 0, Message = "由于您是独立商户，目前只支持最大分佣比例" + p + "%", Result = null };
                        }
                    }
                    CostVO.PromotionAward = PromotionAward;
                }
                if (Promotion == "Signup")
                {
                    CostVO.PromotionSignup = PromotionAward;
                }
                if (Promotion == "Read")
                {
                    CostVO.PromotionRead = PromotionAward;
                }

                if (cBO.UpdateCost(CostVO))
                {
                    return new ResultObject() { Flag = 1, Message = "设置成功!", Result = null };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "设置失败!", Result = null };
                }
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "设置失败!", Result = ex };
            }
        }



        /// <summary>
        /// 获取活动的所有报名列表
        /// </summary>
        /// <param name="PartyID"></param>
        /// <param name="PageCount"></param>
        /// <param name="PageIndex"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [Route("getSignUplistByShow"), HttpGet]
        public ResultObject getSignUplistByShow(int PartyID, int PageIndex, int PageCount, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile(), CustomerVO2.AppType);
            //List<BCPartySignUpViewVO> cVO = cBO.FindSignUpViewByPartyID(PartyID, false);
            string sql = "PartyID = " + PartyID + " and PartySignUpID > 0 and SignUpStatus<>2 and AppType=" + CustomerVO2.AppType;
            if (PageIndex == 0) PageIndex = 1;
            List<BCPartySignUpViewVO> cVO = cBO.FindSignUpViewIndexByPartyID(sql, (PageIndex - 1) * PageCount, (PageIndex+1) * PageCount, "CreatedAt", "DESC");
            int count = cBO.FindBCPartSignInNumTotalCount(sql);
            if (cVO != null)
            {
                if (cVO.Count > 0)
                {
                   
                    int numberOfPeople = cBO.FindBCPartSignInSumCount("Number", "PartyID=" + PartyID + " and (SignUpStatus=1 or SignUpStatus=0)"); //总人数
                    decimal Earning = 0; //总金额
                    List<CostItem> CostList = new List<CostItem>();

                    //for (int i = 0; i < cVO.Count; i++)
                    //{
                    //    if (cVO[i].OrderStatus == 1 || cVO[i].PartyOrderID == 0)
                    //    {
                    //        string CostName = cVO[i].CostName;

                    //        if (CostName == "")
                    //        {
                    //            CostName = "免费";
                    //        }

                    //        if (CostList.Exists(p => p.CostName == CostName))
                    //        {
                    //            CostList.FirstOrDefault(p => p.CostName == CostName).Cost += cVO[i].Cost;
                    //            CostList.FirstOrDefault(p => p.CostName == CostName).People += 1;
                    //        }
                    //        else
                    //        {
                    //            CostItem CostItem = new CostItem();
                    //            CostItem.CostName = CostName;
                    //            CostItem.Cost = cVO[i].Cost;
                    //            CostItem.People = 1;
                    //            CostList.Add(CostItem);
                    //        }
                    //        Earning += cVO[i].Cost;
                    //    }
                    //}

                    /*
                    for (int i = 0; i < cVO.Count; i++)  //外循环是循环的次数
                    {
                        for (int j = cVO.Count - 1; j > i; j--)  //内循环是 外循环一次比较的次数
                        {

                            if (cVO[i].Name == cVO[j].Name&& cVO[i].CustomerId == cVO[j].CustomerId)
                            {
                                cVO.RemoveAt(j);
                            }

                        }
                    }*/

                    int Costcount = CostList.Count;//总收费项数量
                    bool isHost = false;//是否是主办方
                    BCPartyVO pVO = cBO.FindPartyById(PartyID);
                    if (pVO != null)
                    {
                        isHost = pVO.CustomerId == customerId;
                    }

                    int ReadCount = cBO.FindAccessrecordsCount("Type='ReadParty' and ById=" + PartyID);
                    int ForwardCount = cBO.FindAccessrecordsCount("Type='ForwardParty' and ById=" + PartyID);
                    int SignUpPartyCount = cBO.FindAccessrecordsCount("Type='SignUpParty' and ById=" + PartyID);

                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = cVO, Count = count, Subsidiary = new { numberOfPeople = numberOfPeople, Earning = Earning, CostList = CostList, Costcount = Costcount, isHost = isHost, ForwardCount = ForwardCount, SignUpPartyCount = SignUpPartyCount, ReadCount = ReadCount } };
                }
                else
                {
                    return new ResultObject() { Flag = 2, Message = "暂无报名!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 下载活动所有报名的Excel文件
        /// </summary>
        /// <param name="PartyID"></param>
        /// <returns></returns>
        [Route("getSignUplistToExcel"), HttpGet]
        public ResultObject getSignUplistToExcel(int PartyID, string token)
        {
            CustomerProfile uProfile = CacheManager.GetUserProfile(token) as CustomerProfile;
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            CardBO cdBO = new CardBO(new CustomerProfile(), CustomerVO2.AppType);
            BCPartyVO CardPartyVO = cBO.FindPartyById(PartyID);

            if (CardPartyVO == null) { return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null }; }
            if (uProfile.CustomerId != CardPartyVO.CustomerId) { return new ResultObject() { Flag = 0, Message = "权限不足，下载失败!", Result = null }; }

            List<BCPartySignUpViewVO> cVO = cBO.FindSignUpViewByPartyID(PartyID);
            List<BCPartyCostVO> CardPartyCost = cBO.FindCostByPartyID(PartyID);

            if (cVO != null)
            {
                if (cVO.Count > 0)
                {
                    DataTable dt = new DataTable();

                    dt.Columns.Add("序号", typeof(Int32));
                    dt.Columns.Add("姓名", typeof(String));
                    dt.Columns.Add("手机", typeof(String));
                    dt.Columns.Add("报名时间", typeof(DateTime));
                    dt.Columns.Add("状态", typeof(String));

                    if (CardPartyCost.Count > 0)
                    {
                        dt.Columns.Add("报名项目", typeof(String));
                        dt.Columns.Add("报名金额", typeof(Decimal));
                    }
                    try
                    {
                        //获取所有填写信息
                        List<BCPartySignUpFormVO> SignUpFormVO = cBO.FindSignUpFormByPartyID(PartyID, 1);
                        for (int i = 0; i < SignUpFormVO.Count; i++)
                        {
                            if (SignUpFormVO[i].Name != "姓名" && SignUpFormVO[i].Name != "手机" && SignUpFormVO[i].Name != "")
                            {
                                string ColumnsName = SignUpFormVO[i].Name;
                                if (dt.Columns.IndexOf(ColumnsName) > -1)
                                {
                                    ColumnsName = ColumnsName + cdBO.RndCode(4) + i;
                                }
                                dt.Columns.Add(ColumnsName, typeof(String));
                            }
                        }
                        dt.Columns.Add("邀约人(分享来源)", typeof(String));
                        for (int i = 0; i < cVO.Count; i++)
                        {

                            DataRow row = dt.NewRow();
                            row["序号"] = i + 1;
                            row["姓名"] = cVO[i].Name;
                            row["手机"] = cVO[i].Phone;
                            row["报名时间"] = cVO[i].CreatedAt;

                            if (cVO[i].Type == 1)
                            {
                                if (cVO[i].SignUpStatus == 0)
                                {
                                    row["状态"] = "未核销";
                                }
                                else if (cVO[i].SignUpStatus == 2)
                                {
                                    row["状态"] = "已退费";
                                }
                                else
                                {
                                    row["状态"] = "已核销";
                                }
                            }

                            //if (CardPartyCost.Count > 0)
                            //{
                            //    row["报名项目"] = cVO[i].CostName;
                            //    row["报名金额"] = cVO[i].Cost;
                            //}

                            List<BCPartySignUpFormVO> svO = cBO.FindSignUpFormByFormStr(cVO[i].SignUpForm);
                            for (int j = 0; j < svO.Count; j++)
                            {
                                if (svO[j].Name != "姓名" && svO[j].Name != "手机" && svO[j].Name != "")
                                {
                                    if (!dt.Columns.Contains(svO[j].Name))
                                    {
                                        dt.Columns.Add(svO[j].Name, typeof(String));
                                    }
                                    row[svO[j].Name] = svO[j].value;
                                }

                            }


                            if (cVO[i].InviterCID > 0)
                            {
                                CustomerVO CVO = CustomerBO.FindCustomenById(cVO[i].InviterCID);

                                if (CVO != null)
                                {
                                    row["邀约人(分享来源)"] = CVO.CustomerName;
                                }
                            }

                            dt.Rows.Add(row);//这样就可以添加了 
                        }

                        string FileName = cBO.DataToExcel(dt, "PartySignUpExcel/", PartyID + ".xls");

                        if (FileName != null)
                        {
                            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = FileName };
                        }
                        else
                        {
                            return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
                        }
                    }
                    catch (Exception ex)
                    {
                        LogBO _log = new LogBO(typeof(CardBO));
                        string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                        _log.Error(strErrorMsg);
                        return new ResultObject() { Flag = 0, Message = "获取失败!", Result = ex };
                    }
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "暂无报名!", Result = null };
                }

            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取入场券
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getMySignUpDetailsByCustomerId"), HttpGet]
        public ResultObject getMySignUpDetailsByCustomerId(int PartyID, int CustomerId, string token, int AppType = 1)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);


            CardBO cdBO = new CardBO(new CustomerProfile(), CustomerVO2.AppType);
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            BCPartyVO pVO = cBO.FindPartyById(PartyID);
            List<BCPartySignUpViewVO> cVO = cBO.FindSignUpViewById(CustomerId, PartyID, CustomerVO2.AppType);

            if (cVO == null || cVO.Count <= 0)
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = cVO };

            List<BCPartySignUpVO> sVO = cBO.FindSignUpByPartyID(PartyID);
            sVO.Sort((a, b) => a.CreatedAt.CompareTo(b.CreatedAt));

            for (int i = 0; i < cVO.Count; i++)
            {
                if (cVO[i].SignUpQRCodeImg == "")
                {
                    cVO[i].SignUpQRCodeImg = cBO.GetBCPartySignUpQR(cVO[i].PartySignUpID);
                }

                cVO[i].isSponsor = false;
                if (cVO[i].HostCustomerId == customerId)
                    cVO[i].isSponsor = true;
                else
                {
                    List<BCPartyContactsViewVO> ContactsVO = cBO.FindPartyContactsViewByPartyId(cVO[i].PartyID);
                    for (int j = 0; j < ContactsVO.Count; j++)
                    {
                        if (customerId == ContactsVO[j].CustomerId)
                        {
                            cVO[i].isSponsor = true;
                        }
                    }
                }
                cVO[i].BCPartyContactsView = cBO.FindPartyContactsViewByPartyId(cVO[i].PartyID);

                int number = 1;
                for (int j = 0; j < sVO.Count; j++)
                {
                    if (sVO[j].PartySignUpID == cVO[i].PartySignUpID)
                    {
                        number = j + 1;
                    }
                }
                cVO[i].Sequence = number;
            }

            if (cVO[0].CustomerId != customerId && !cVO[0].isSponsor)
            {
                if (pVO.Type == 3)
                    return new ResultObject() { Flag = 0, Message = "只有本人和举办方才能查看抽奖券!", Result = null };
                else
                    return new ResultObject() { Flag = 0, Message = "只有本人和举办方才能查看入场券!", Result = null };
            }

            List<BCPartySignUpViewVO> newCVO = new List<BCPartySignUpViewVO>();
            if (pVO.Type == 3)
                newCVO.Add(cVO[0]);
            else
                newCVO = cVO;

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = newCVO };
        }


        /// 获取入场券
        /// </summary>
        /// <param name="PartySignUpID"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getMySignUpDetails"), HttpGet]
        public ResultObject getMySignUpDetails(int PartySignUpID, string token, int AppType = 1)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            if (AppType == 1)
            {
                AppType = CustomerVO2.AppType;
            }
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());

            BCPartySignUpViewVO cVO = cBO.FindSignUpViewById(PartySignUpID);

            if (cVO == null)
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            try
            {
                if (cVO.SignUpQRCodeImg == "")
                {
                    cVO.SignUpQRCodeImg = cBO.GetBCPartySignUpQR(cVO.PartySignUpID);
                }

                cVO.isSponsor = false;
                if (cVO.HostCustomerId == customerId)
                    cVO.isSponsor = true;
                else
                {
                    List<BCPartyContactsViewVO> ContactsVO = cBO.FindPartyContactsViewByPartyId(cVO.PartyID);
                    for (int i = 0; i < ContactsVO.Count; i++)
                    {
                        if (customerId == ContactsVO[i].CustomerId)
                        {
                            cVO.isSponsor = true;
                        }
                    }
                }

                if (cVO.CustomerId != customerId && !cVO.isSponsor)
                {
                    if (cVO.Type == 3)
                        return new ResultObject() { Flag = 0, Message = "只有本人和举办方才能查看抽奖券!", Result = null };
                    else
                        return new ResultObject() { Flag = 0, Message = "只有本人和举办方才能查看入场券!", Result = null };
                }

                List<BCPartySignUpVO> sVO = cBO.FindSignUpByPartyID(cVO.PartyID);
                sVO.Sort((a, b) => a.CreatedAt.CompareTo(b.CreatedAt));

                int number = 1;
                for (int i = 0; i < sVO.Count; i++)
                {
                    if (sVO[i].PartySignUpID == PartySignUpID)
                    {
                        number = i + 1;
                    }
                }

                cVO.BCPartyContactsView = cBO.FindPartyContactsViewByPartyId(cVO.PartyID);
                cVO.Sequence = number;

                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = cVO };
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = ex.StackTrace };
                throw;
            }


        }

        /// <summary>
        /// 核销入场券
        /// </summary>
        /// <param name="PartySignUpID"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("VerificationSignUp"), HttpGet]
        public ResultObject VerificationSignUp(int PartySignUpID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            //CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO2.AppType);
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            BCPartySignUpViewVO cVO = cBO.FindSignUpViewById(PartySignUpID);
            PersonalVO per = cBO.FindPersonalByCustomerId(cVO.CustomerId);
            if (cVO == null)
                return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };

            cVO.isSponsor = false;
            if (cVO.HostCustomerId == customerId)
                cVO.isSponsor = true;
            else
            {
                List<BCPartyContactsViewVO> ContactsVO = cBO.FindPartyContactsViewByPartyId(cVO.PartyID);
                for (int i = 0; i < ContactsVO.Count; i++)
                {
                    if (customerId == ContactsVO[i].CustomerId)
                    {
                        cVO.isSponsor = true;
                    }
                }
            }

            if (!cVO.isSponsor)
                return new ResultObject() { Flag = 0, Message = "只有举办方才能核销入场券!", Result = null };
            if (cVO.SignUpStatus == 1)
                return new ResultObject() { Flag = 0, Message = "该券已经核销过了，请勿重复操作!", Result = null };
            if (cVO.SignUpStatus == 2)
                return new ResultObject() { Flag = 0, Message = "该入场券已退款，无法核销!", Result = null };

            BCPartySignUpVO cuVO = new BCPartySignUpVO();
            cuVO.PartySignUpID = PartySignUpID;
            cuVO.SignUpStatus = 1;
            if (cBO.UpdateSignUp(cuVO))
            {

                //try
                //{
                //    //将报名的名片加入活动的名片组
                //    //CardPartyVO cpVO = cBO.FindPartyById(cVO.PartyID);
                //    //if (cpVO != null && cpVO.GroupID != 0)
                //    //{
                //    //    List<CardGroupCardViewVO> cgVO = cBO.isJionCardGroup(cVO.CustomerId, cpVO.GroupID);
                //    //    if (cgVO.Count <= 0)
                //    //    {
                //    //        CardGroupCardVO cgcVO = new CardGroupCardVO();
                //    //        cgcVO.CustomerId = cVO.CustomerId;
                //    //        cgcVO.GroupID = cVO.GroupID;

                //    //        CardGroupVO gVO = cBO.FindCardGroupById(cpVO.GroupID);
                //    //        cgcVO.Status = 1;
                //    //        cgcVO.CreatedAt = DateTime.Now;
                //    //        cgcVO.CardID = cVO.CardID;
                //    //        cBO.AddCardToGroup(cgcVO);
                //    //    }
                //    //    else if (cgVO[0].Status == 0)
                //    //    {
                //    //        CardGroupCardVO cgcVO = new CardGroupCardVO();
                //    //        cgcVO.GroupCardID = cgVO[0].GroupCardID;

                //    //        CardGroupVO gVO = cBO.FindCardGroupById(cpVO.GroupID);
                //    //        cgcVO.Status = 1;

                //    //        cBO.UpdateCardToGroup(cgcVO);
                //    //    }
                //    //}
                //}
                //catch
                //{

                //}

                return new ResultObject() { Flag = 1, Message = "核销成功!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "核销失败，请重试!", Result = null };
            }
        }


        /// <summary>
        /// 获取活动海报
        /// </summary>
        /// <param name="PartyID">活动ID</param>
        /// <param name="isRestart">0：不重新生成，1：重新生成海报</param>
        /// <param name="style">海报样式</param>
        /// <returns></returns>
        [Route("GetPartyPoster"), HttpGet, Anonymous]
        public ResultObject GetPartyPoster(int PartyID, int isRestart = 0, int style = 0, int CustomerId = 0, int AppType = 0)
        {
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            BCPartyVO cVO = cBO.FindPartyById(PartyID);

            if (cVO != null)
            {
                if (cVO.PosterImg == "")
                {
                    cVO.PosterImg = cBO.getPosterByPartyID(PartyID, style, CustomerId, AppType);
                }
                else if (isRestart == 1)
                {
                    cVO.PosterImg = cBO.getPosterByPartyID(PartyID, style, CustomerId, AppType);
                }

                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = cVO.PosterImg };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 上传自定义海报背景
        /// </summary>
        /// <returns></returns>
        [Route("UploadCardPoter"), HttpPost]
        public ResultObject UploadCardPoter(string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            CustomerVO cVO = uBO.FindCustomenById(cProfile.CustomerId);
            CardBO cBO = new CardBO(new CustomerProfile(), cVO.AppType);

            HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
            string folder = "/UploadFolder/BCPoterImage/" + DateTime.Now.ToString("yyyyMM") + "/";
            string imgPath = "";
            if (hfc.Count > 0)
            {
                try
                {
                    FileInfo fi = new FileInfo(hfc[0].FileName);

                    string ext = fi.Extension.ToLower();
                    if (ext.IndexOf("asp") > -1 || ext.IndexOf("aspx") > -1 || ext.IndexOf("php") > -1)
                    {
                        return new ResultObject() { Flag = 0, Message = "文件类型错误", Result = null };
                    }

                    string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + fi.Extension;

                    //可以修改为网络路径
                    string localPath = ConfigInfo.Instance.UploadFolder + folder;
                    if (!Directory.Exists(localPath))
                    {
                        Directory.CreateDirectory(localPath);
                    }
                    string PhysicalPath = localPath + newFileName;

                    hfc[0].SaveAs(PhysicalPath);

                    imgPath = ConfigInfo.Instance.APIURL + folder + newFileName;

                    CardPoterVO CardPoterVO = new CardPoterVO();
                    CardPoterVO.CardPoterID = 0;
                    CardPoterVO.CustomerId = customerId;
                    CardPoterVO.FileName = newFileName;
                    CardPoterVO.Url = imgPath;
                    cBO.AddCardPoter(CardPoterVO);

                    return new ResultObject() { Flag = 1, Message = "上传成功", Result = imgPath };
                }
                catch (Exception er)
                {
                    return new ResultObject() { Flag = 0, Message = "上传失败", Result = er.Message };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "上传失败", Result = "文件为空" };
            }
        }

        /// <summary>
        /// 删除自定义海报
        /// </summary>
        /// <param name="CardPoterID">海报ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("delCardPoter"), HttpGet]
        public ResultObject delCardPoter(int CardPoterID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO.AppType);

            if (cBO.DeleteCardPoterAdminById(CardPoterID) > 0)
            {
                return new ResultObject() { Flag = 1, Message = "删除成功!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
            }
        }





        /// <summary>
        /// 获取活动报名填写项，匿名
        /// </summary>
        /// <param name="PartyID">活动ID</param>
        /// <returns></returns>
        [Route("GetPartySignUpForm"), HttpGet, Anonymous]
        public ResultObject GetPartySignUpForm(int PartyID, int AppType = 1)
        {
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            List<BCPartySignUpFormVO> cVO = cBO.FindSignUpFormByPartyID(PartyID);

            if (cVO != null)
            {
                if (cVO.Count > 0)
                {
                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = cVO };
                }
                else
                {
                    return new ResultObject() { Flag = 2, Message = "无报名信息!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }


        /// <summary>
        /// 报名活动
        /// </summary>
        /// <param name="BCPartySignUpFormVO">报名信息</param>
        /// <param name="PartyID"></param>
        /// <param name="FormId"></param>
        /// <param name="code"></param>
        /// <param name="token">口令</param>
        /// <param name="Headimg">头像</param>
        ///<param name="InviterCID">邀请者ID</param>
        ///<param name="Number">报名数量</param>
        /// <returns></returns>
        [Route("JoinCardParty"), HttpPost]
        public ResultObject JoinCardParty([FromBody] List<BCPartySignUpFormVO> BCPartySignUpFormVO, int PartyID, string FjUrl, string FormId, string code, string token, string Headimg, int InviterCID, int Number = 1, int PayType = 1)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);

            int AppType = CustomerVO2.AppType;
            if (PayType != 1)
            {
                AppType = PayType;
            }

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile(), CustomerVO2.AppType);

            int oldCount = cBO.FindBCPartSignInNumTotalCount("CustomerId = " + customerId + " and PartyID=" + PartyID + " and NOW()-CreatedAt<30");
            if (oldCount > 0)
            {
                return new ResultObject() { Flag = 0, Message = "操作过于频繁，请稍后!", Result = null };
            }

            /*审核文本是否合法*/
            if (!cBO.msg_sec_check1(BCPartySignUpFormVO))
            {
                return new ResultObject() { Flag = 0, Message = "有政治敏感或违法关键词，请重新填写!", Result = null };
            }
            /*审核文本是否合法*/

            if (Number <= 0)
            {
                return new ResultObject() { Flag = 0, Message = "请输入购买数量", Result = PartyID };
            }

            BCPartyVO cpvo = cBO.FindPartyById(PartyID);
            List<BCPartySignUpViewVO> cpsuVO = cBO.FindSignUpViewByPartyID(PartyID);
            int _NumberSum = cpsuVO.Sum(p => p.Number);


            if (cpvo.limitPeopleNum != 0 && cpvo.limitPeopleNum < _NumberSum + Number) { return new ResultObject() { Flag = 0, Message = "报名人数已满", Result = null }; }

            if (cpvo.SignUpTime < DateTime.Now && cpvo.Type == 1)
            {
                return new ResultObject() { Flag = 0, Message = "报名已截止!", Result = null };
            }

            if (cpvo.Status == 0)
            {
                return new ResultObject() { Flag = 0, Message = "该活动已被主办方删除，请勿报名!", Result = null };
            }

            //List<BCPartyCostVO> CostVO = cBO.FindCostByPartyID(PartyID);
            //if (CostVO.Count > 0 && cpvo.Type != 3)
            //{
            //    return new ResultObject() { Flag = 0, Message = "请选择购买类型", Result = PartyID };
            //}

            // List<CardDataVO> uVO = cBO.FindCardByCustomerId(customerId);
            BCPartySignUpVO suVO = new BCPartySignUpVO();

            int CardID = 0;
            string name = "";
            string phone = "";
            string SignUpForm = "";
            string Position = "";
            string CorporateName = "";
            string Address = "";
            decimal latitude = 0;
            decimal longitude = 0;
            string WeChat = "";

            for (int i = 0; i < BCPartySignUpFormVO.Count; i++)
            {
                if (BCPartySignUpFormVO[i].Name == "姓名")
                    name = BCPartySignUpFormVO[i].value;
                if (BCPartySignUpFormVO[i].Name == "手机")
                    phone = BCPartySignUpFormVO[i].value;
                if (BCPartySignUpFormVO[i].Name == "职位")
                    Position = BCPartySignUpFormVO[i].value;
                if (BCPartySignUpFormVO[i].Name == "工作单位")
                    CorporateName = BCPartySignUpFormVO[i].value;
                if (BCPartySignUpFormVO[i].Name == "微信")
                    WeChat = BCPartySignUpFormVO[i].value;
                if (BCPartySignUpFormVO[i].Name == "单位地址")
                {
                    Address = BCPartySignUpFormVO[i].value;
                    WeiXinGeocoder Geocoder = cBO.getLatitudeAndLongitude(BCPartySignUpFormVO[i].value);
                    if (Geocoder != null)
                    {
                        latitude = Geocoder.result.location.lat;
                        longitude = Geocoder.result.location.lng;
                    }
                }

                if (BCPartySignUpFormVO[i].Status > 0)
                {
                    SignUpForm += "<SignUpForm>" + "<Name>" + BCPartySignUpFormVO[i].Name + "</Name>" + "<Value>" + BCPartySignUpFormVO[i].value + "</Value>" + "</SignUpForm>";
                }
            }

            //if (uVO.Count > 0)
            //{
            //    Headimg = uVO[0].Headimg;
            //    CardID = uVO[0].CardID;

            //    if (name == "")
            //    {
            //        name = uVO[0].Name;
            //    }

            //    if (uVO[0].Phone == "" && phone != "")
            //    {
            //        uVO[0].Phone = phone;
            //    }
            //    if (uVO[0].Position == "" && Position != "")
            //    {
            //        uVO[0].Position = Position;
            //    }
            //    if (uVO[0].CorporateName == "" && CorporateName != "")
            //    {
            //        uVO[0].CorporateName = CorporateName;
            //    }
            //    if (uVO[0].WeChat == "" && WeChat != "")
            //    {
            //        uVO[0].WeChat = WeChat;
            //    }
            //    if (uVO[0].Address == "" && Address != "")
            //    {
            //        uVO[0].Address = Address;
            //    }

            //    cBO.Update(uVO[0]);
            //}
            //else
            //{
            //    if (name == "")
            //    {
            //        name = CustomerVO2.CustomerName;
            //    }

            //    CardDataVO CardDataVO = new CardDataVO();
            //    CardDataVO.Name = name;
            //    CardDataVO.Phone = phone;
            //    CardDataVO.Position = Position;
            //    CardDataVO.CorporateName = CorporateName;
            //    CardDataVO.Headimg = Headimg;
            //    CardDataVO.Address = Address;
            //    CardDataVO.WeChat = WeChat;
            //    CardDataVO.latitude = latitude;
            //    CardDataVO.longitude = longitude;

            //    CardDataVO.CreatedAt = DateTime.Now;
            //    CardDataVO.Status = 1;//0:禁用，1:启用
            //    CardDataVO.CustomerId = customerId;
            //    CardDataVO.isParty = 1;

            //    CardID = cBO.AddCard(CardDataVO);
            //}

            suVO.CardID = CardID;
            suVO.CustomerId = customerId;
            suVO.Name = name;
            suVO.Headimg = Headimg;
            suVO.Phone = phone;
            suVO.PartyID = PartyID;
            suVO.FormId = FormId;
            suVO.FjUrl = FjUrl;
            suVO.OpenId = cBO.getOpenId(code);
            suVO.CreatedAt = DateTime.Now;
            suVO.SignUpForm = SignUpForm;
            suVO.InviterCID = InviterCID;
            suVO.Number = Number;



            int PartySignUpID = cBO.AddCardToParty(suVO);
            if (PartySignUpID > 0)
            {
                //cBO.sendSignUpMessage(PartySignUpID);
                //CardDataVO CardDataVO = cBO.FindCardById(CardID);
                //string url = "/pages/Party/SignUpList/SignUpList?isadmin=true&PartyID=" + cpvo.PartyID;
                //if (cpvo.Type == 3)
                //{
                //    url = "/package/package_party/SignUpList/SignUpList?isadmin=true&PartyID=" + cpvo.PartyID;
                //}
                //cBO.AddCardMessage(CardDataVO.Name + "报名了活动《" + cpvo.Title + "》", cpvo.CustomerId, "活动报名", url);

                //if (AppType == 1 || AppType == 2)
                //{
                //    string Title = "您成功报名了活动《" + cpvo.Title + "》";
                //    if (cpvo.Type == 2)
                //    {
                //        Title = "您成功购买了商品《" + cpvo.Title + "》";
                //    }
                //    cBO.AddCardMessage(Title, customerId, "活动报名", "/pages/Party/PartyShow/PartyShow?PartyID=" + cpvo.PartyID);
                //}

                return new ResultObject() { Flag = 1, Message = "报名成功!", Result = PartySignUpID };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "操作失败，请重试!", Result = null };
            }
        }

        /// <summary>
        /// 删除活动
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DelParty"), HttpGet]
        public ResultObject DelParty(int PartyID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            BCPartyVO rVO = cBO.FindPartyById(PartyID);
            if (rVO != null)
            {
                if (cProfile != null)
                {
                    //如果提交者并非该任务所属的会员，直接禁止更新
                    if (cProfile.CustomerId != rVO.CustomerId)
                        return new ResultObject() { Flag = 0, Message = "删除失败，只有活动创建者才可以删除!", Result = null };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "删除失败，只有活动创建者才可以删除!", Result = null };
                }
            }

            rVO.Status = 0;

            bool isSuccess = cBO.UpdateParty(rVO);
            if (isSuccess)
                return new ResultObject() { Flag = 1, Message = "删除成功!", Result = rVO.PartyID };
            else
                return new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
        }

        /// <summary>
        /// 获取我报名的活动 分页
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getMySignUpPartylist"), HttpGet]
        public ResultObject getMySignUpPartylist(int PageCount, int PageIndex, string token, int AppType = 1)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            if (AppType == 1)
            {
                AppType = CustomerVO2.AppType;
            }
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            List<BCPartySignUpViewVO> uVO = cBO.FindSignUpViewByCustomerId(customerId, AppType);
            if (uVO != null)
            {
                uVO.Sort((a, b) => a.StartTime.CompareTo(b.StartTime));
                uVO.Reverse();
                IEnumerable<BCPartySignUpViewVO> newVO = uVO.Skip((PageIndex - 1) * PageCount).Take(PageCount);
                foreach (BCPartySignUpViewVO item in newVO)
                {
                    //item.CardPartySignUp = cBO.FindSignUpByPartyID(item.PartyID);

                    CustomerVO CVO = uBO.FindCustomenById(item.HostCustomerId);
                    PersonalVO pVO = cBO.FindPersonalByCustomerId(item.HostCustomerId);
                    if (CVO != null)
                    {
                        item.HeaderLogo = CVO.HeaderLogo;
                        item.CustomerName = CVO.CustomerName;
                    }

                    if (pVO != null)
                    {
                        item.Name = pVO.Name;
                        item.Headimg = pVO.Headimg;
                    }
                }
                int BeoverdueCount = cBO.FindSignUpViewBeoverdueCount(customerId);
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = newVO, Count = uVO.Count, Subsidiary = BeoverdueCount };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 修改活动是否显示首页
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpPartyByIsDisplayIndex"), HttpGet]
        public ResultObject UpPartyByIsDisplayIndex(int PartyID, int IsDisplayIndex, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            BCPartyVO rVO = cBO.FindPartyById(PartyID);

            rVO.IsDisplayIndex = IsDisplayIndex;

            bool isSuccess = cBO.UpdateParty(rVO);
            if (isSuccess)
                return new ResultObject() { Flag = 1, Message = "设置成功!", Result = rVO.PartyID };
            else
                return new ResultObject() { Flag = 0, Message = "设置失败!", Result = null };
        }




        /// <summary>
        /// 获取活动的邀请人列表数
        /// </summary>
        /// <param name="PartyID"></param>
        /// <returns></returns>
        [Route("getSignUpInviterlist"), HttpGet, Anonymous]
        public ResultObject getSignUpInviterlist(int PartyID, int AppType = 1)
        {
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            List<BCPartySignUpViewVO> cVO = cBO.FindSignUpViewInviterByPartyID(PartyID);
            if (cVO != null)
            {
                if (cVO.Count > 0)
                {
                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = cVO };
                }
                else
                {
                    return new ResultObject() { Flag = 2, Message = "暂无数据!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 改变活动的转发，收藏，浏览
        /// </summary>
        /// <param name="PartID">名片ID</param>
        /// <param name="field">操作字段</param>
        /// <param name="sum">数量</param>
        /// <returns></returns>
        [Route("PartReadData"), HttpGet, Anonymous]
        public ResultObject PartReadData(int PartID, string field, int sum, int AppType = 1)
        {
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());

            BCPartyVO pVO = cBO.FindPartyById(PartID);


            if (field == "ReadCount")
            {
                pVO.ReadCount += sum;
            }
            if (field == "Forward")
            {
                pVO.Forward += sum;
            }
            if (cBO.UpdateParty(pVO))
            {
                return new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }
        }


        /// <summary>
        /// 报名活动（下单）
        /// </summary>
        /// <param name="BCPartySignUpFormVO">报名信息</param>
        /// <param name="PartyID"></param>
        /// <param name="FormId"></param>
        /// <param name="code"></param>
        /// <param name="token">口令</param>
        /// <param name="Headimg">头像</param>
        /// <param name="PartyCostID">费用信息ID</param>
        /// <param name="InviterCID">InviterCID</param>
        ///<param name="Number">报名数量</param>
        /// <returns></returns>
        [Route("PlaceAnOrder"), HttpPost]
        public ResultObject PlaceAnOrder([FromBody] List<BCPartySignUpFormVO> BCPartySignUpFormVO, int PartyID, string FormId, string code, string token, string Headimg, int PartyCostID, int InviterCID, int Number = 1, int PayType = 1, int isH5 = 0)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);

            int AppType = CustomerVO2.AppType;
            if (PayType != 1)
            {
                AppType = PayType;
            }
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            // CardBO cBO = new CardBO(new CustomerProfile(), AppType);

            if (Number <= 0)
            {
                return new ResultObject() { Flag = 0, Message = "请输入购买数量", Result = PartyID };
            }

            /*审核文本是否合法*/
            if (!cBO.msg_sec_check(BCPartySignUpFormVO))
            {
                return new ResultObject() { Flag = 0, Message = "有政治敏感或违法关键词，请重新填写!", Result = null };
            }
            /*审核文本是否合法*/

            BCPartyVO cpvo = cBO.FindPartyById(PartyID);

            //if (cpvo.Type == 3)
            //{
            //    return new ResultObject() { Flag = 0, Message = "抽奖活动无法下单!", Result = PartyID };
            //}

            List<BCPartySignUpViewVO> cpsuVO = cBO.FindSignUpViewByPartyID(PartyID);
            int _NumberSum = cpsuVO.Sum(p => p.Number);

            if (cpvo.limitPeopleNum != 0 && cpvo.limitPeopleNum < _NumberSum + Number) { return new ResultObject() { Flag = 0, Message = "报名人数已满", Result = null }; }

            if (cpvo.Status == 0)
            {
                return new ResultObject() { Flag = 0, Message = "该活动已被主办方删除，请勿报名!", Result = null };
            }

            if (PartyCostID <= 0)
            {
                return new ResultObject() { Flag = 0, Message = "请选择购买类型", Result = PartyID };
            }

            if (cpvo.SignUpTime < DateTime.Now && cpvo.Type == 1)
            {
                return new ResultObject() { Flag = 0, Message = "报名已截止!", Result = null };
            }

            BCPartyCostVO CostVO = cBO.FindCostById(PartyCostID);
            if (CostVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "请选择购买类型", Result = PartyID };
            }


            /*活动发起人未实名无法报名*/
            /*
            if(cpvo.Type!=3&& CostVO.Cost>0)
            {
                string st1 = "2021-05-26 00:00:00";
                DateTime dt1 = Convert.ToDateTime(st1);
                if(DateTime.Compare(dt1, cpvo.CreatedAt) > 0)
                {
                    CustomerVO HostCVO = CustomerBO.FindCustomenById(cpvo.CustomerId);
                    if (!HostCVO.isIdCard)
                    {
                        return new ResultObject() { Flag = 0, Message = "该活动商家未经过实名认证，不得进行收费！", Result = PartyID };
                    }
                }
            }*/


            List<BCPartySignUpViewVO> CostSignUpVO = cBO.PartyCostSignUpView(CostVO.Names, PartyID);
            int _CostNumberSum = CostSignUpVO.Sum(p => p.Number);

            if (CostVO.limitPeopleNum != 0 && CostVO.limitPeopleNum < _CostNumberSum + Number) { return new ResultObject() { Flag = 0, Message = "该类型报名人数已满，请选择其他购买类型", Result = new { CostVO = CostVO, CostSignUpVO = CostSignUpVO } }; }
            if (CostVO.EffectiveTime.Year > 1900 && CostVO.EffectiveTime < DateTime.Now)
            {
                return new ResultObject() { Flag = 0, Message = "该类型报名已截止,请选择其他购买类型!", Result = CostVO.EffectiveTime };
            }


            if (CostVO.PartyID != PartyID)
            {
                return new ResultObject() { Flag = 0, Message = "参数错误", Result = PartyID };
            }

            /*
            List<CardPartySignUpVO> cVO = cBO.isJionCardParty(customerId, PartyID);
            if (cVO.Count > 0)
            {
                return new ResultObject() { Flag = 4, Message = "您已经报名了该活动，请GetPartyCost勿重复操作!", Result = PartyID };
            }*/



            PersonalVO uVO = cBO.FindPersonalByCustomerId(customerId);
            BCPartyOrderVO OrderVO = new BCPartyOrderVO();

            int PersonalID = 0;
            string name = "";
            string phone = "";
            string SignUpForm = "";
            string Position = "";
            string CorporateName = "";
            string Address = "";
            decimal latitude = 0;
            decimal longitude = 0;
            string WeChat = "";

            for (int i = 0; i < BCPartySignUpFormVO.Count; i++)
            {
                if (BCPartySignUpFormVO[i].Name == "姓名")
                    name = BCPartySignUpFormVO[i].value;
                if (BCPartySignUpFormVO[i].Name == "手机")
                    phone = BCPartySignUpFormVO[i].value;
                if (BCPartySignUpFormVO[i].Name == "职位")
                    Position = BCPartySignUpFormVO[i].value;
                if (BCPartySignUpFormVO[i].Name == "工作单位")
                    CorporateName = BCPartySignUpFormVO[i].value;
                if (BCPartySignUpFormVO[i].Name == "微信")
                    WeChat = BCPartySignUpFormVO[i].value;
                if (BCPartySignUpFormVO[i].Name == "单位地址")
                {
                    Address = BCPartySignUpFormVO[i].value;
                    WeiXinGeocoder Geocoder = cBO.getLatitudeAndLongitude(BCPartySignUpFormVO[i].value);
                    if (Geocoder != null)
                    {
                        latitude = Geocoder.result.location.lat;
                        longitude = Geocoder.result.location.lng;
                    }
                }

                if (BCPartySignUpFormVO[i].Status > 0)
                {
                    SignUpForm += "<SignUpForm>" + "<Name>" + BCPartySignUpFormVO[i].Name + "</Name>" + "<Value>" + BCPartySignUpFormVO[i].value + "</Value>" + "</SignUpForm>";
                }
            }

            if (uVO != null)
            {
                Headimg = uVO.Headimg;
                PersonalID = uVO.PersonalID;

                if (name == "")
                {
                    name = uVO.Name;
                }

                if (uVO.Phone == "" && phone != "")
                {
                    uVO.Phone = phone;
                }
                if (uVO.Position == "" && Position != "")
                {
                    uVO.Position = Position;
                }
                //if (uVO.CorporateName == "" && CorporateName != "")
                //{
                //    uVO.CorporateName = CorporateName;
                //}
                if (uVO.WeChat == "" && WeChat != "")
                {
                    uVO.WeChat = WeChat;
                }
                if (uVO.Address == "" && Address != "")
                {
                    uVO.Address = Address;
                }

                cBO.UpdatePersonal(uVO);
            }

            OrderVO.CustomerId = customerId;
            OrderVO.Name = name;
            OrderVO.Headimg = Headimg;
            OrderVO.Phone = phone;
            OrderVO.PartyID = PartyID;
            OrderVO.FormId = FormId;

            string OpenId = "";
            if (isH5 == 1)
            {
                ////获取微信的用户信息
                CustomerController customerCon = new CustomerController();
                ResultObject result = customerCon.GetThirdPartUserInfo("2", code, "", "2");
                WeiXinHelper.WeiXinUserInfoResult userInfo = result.Result as WeiXinHelper.WeiXinUserInfoResult;
                //用户信息（判断是否已经获取到用户的微信用户信息）
                if (userInfo.Result && userInfo.UserInfo.openid != "")
                {
                    OpenId = userInfo.UserInfo.openid;
                }
            }
            else
            {
                OpenId = cBO.getOpenId(code);
            }

            OrderVO.OpenId = OpenId;
            OrderVO.CreatedAt = DateTime.Now;
            OrderVO.SignUpForm = SignUpForm;
            OrderVO.CostName = CostVO.Names;

            OrderVO.Cost = CostVO.Cost * Number;
            if (CostVO.isDiscount == 1 && (CostVO.DiscountTime > DateTime.Now || CostVO.DiscountTime < DateTime.Now.AddYears(-20)))
            {
                List<BCPartySignUpViewVO> CostSignUpVO2 = cBO.PartyCostSignUpView(CostVO.Names, CostVO.DiscountCost, PartyID);
                int DiscountNumberSum = CostSignUpVO2.Sum(p => p.Number);
                if (CostVO.DiscountNum >= DiscountNumberSum + Number || CostVO.DiscountNum == 0)
                {
                    OrderVO.Cost = CostVO.DiscountCost * Number;
                }
            }



            ////满足分享浏览条件则免费
            //try
            //{
            //    if ((cpvo.isPromotionRead == 1 && CostVO.PromotionRead > 0) || (cpvo.isPromotionSignup == 1 && CostVO.PromotionSignup > 0))
            //    {
            //        int MyReadSigUp = cBO.FindPartyOrderTotalCount("CustomerId=" + customerId + " and Status=1 and PartyID=" + PartyID + " and (PromotionReadStatus=1 or PromotionSignupStatus=1)");
            //        if (MyReadSigUp == 0)
            //        {
            //            if (cBO.isPromotionRead(customerId, PartyID, CostVO, cpvo))
            //            {
            //                OrderVO.Cost = 0;
            //                OrderVO.PromotionReadStatus = 1;
            //                Number = 1;
            //            }
            //            if (cBO.isPromotionSignup(customerId, PartyID, CostVO, cpvo))
            //            {
            //                OrderVO.Cost = 0;
            //                OrderVO.PromotionSignupStatus = 1;
            //                Number = 1;
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LogBO _log2 = new LogBO(typeof(CardBO));
            //    string strErrorMsg2 = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
            //    _log2.Error(strErrorMsg2);
            //}


            OrderVO.InviterCID = InviterCID;
            OrderVO.Number = Number;
            OrderVO.sub_mchid = "";


            //如果是二级商户
            EcommerceBO eBO = new EcommerceBO();
            wxMerchantVO mVO = eBO.getMyMerchant(cpvo.CustomerId);

            if (mVO != null)
            {
                OrderVO.sub_mchid = mVO.sub_mchid;
                if (mVO.SplitProportion > 0)
                {
                    OrderVO.SplitCost = OrderVO.Cost * mVO.SplitProportion / 100;
                }
            }

            if (InviterCID > 0 && cpvo.isPromotionAward == 1 && CostVO.PromotionAward > 0)
            {
                OrderVO.PromotionAward = CostVO.PromotionAward;
                OrderVO.PromotionAwardCost = OrderVO.Cost * CostVO.PromotionAward / 100;
            }

            Random ran = new Random();
            OrderVO.OrderNO = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ran.Next(10000, 99999);
            int PartyOrderID = cBO.AddPartyOrder(OrderVO, uVO.AppType);
            if (PartyOrderID > 0)
            {
                return new ResultObject() { Flag = 1, Message = "下单成功!", Result = PartyOrderID };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "操作失败，请重试!", Result = null };
            }
        }

        /// <summary>
        /// 获取微信支付数据
        /// </summary>
        /// <param name="PartyOrderID">活动订单id</param>
        /// <param name="token"></param>
        ///<param name="InviterCID">邀请者id</param>
        ///<param name="PayType">1：乐聊名片，2：引流王</param>
        /// <returns></returns>
        [Route("GetUnifiedOrderResult"), HttpGet]
        public async Task<ResultObject> GetUnifiedOrderResult(int PartyOrderID, string token, int InviterCID, int PayType = 1, int isH5 = 0)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);

            int AppType = CustomerVO2.AppType;
            if (PayType != 1)
            {
                AppType = PayType;
            }
            if (AppType == 1)
            {
                AppType = 0;
            }

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            AppVO AppVO = AppBO.GetApp(AppType);
            string appid = AppVO.AppId;
            BCPartyOrderViewVO cVO = cBO.FindPartyOrderViewById(PartyOrderID);
            if (cVO == null)
                return new ResultObject() { Flag = 0, Message = "获取订单失败!", Result = null };
            if (cVO.CreatedAt.AddMinutes(30) < DateTime.Now)
            {
                return new ResultObject() { Flag = 0, Message = "订单已过期，请重新报名!", Result = null };
            }

            /*
            List<CardPartySignUpVO> SignUpVO = cBO.isJionCardParty(customerId, cVO.PartyID);
            if (SignUpVO.Count > 0)
            {
                return new ResultObject() { Flag = 4, Message = "您已经报名了该活动，请勿重复操作!", Result = null };
            }*/

            JsApiPay Ja = new JsApiPay();

            if (cVO.Cost < 0)
            {
                return new ResultObject() { Flag = 0, Message = "价格错误", Result = null };
            }
            if (cVO.Cost == 0)
            {
                //完成报名
                BCPartySignUpVO suVO = new BCPartySignUpVO();
                suVO.CardID = cVO.CardID;
                suVO.CustomerId = cVO.CustomerId;
                suVO.Name = cVO.Name;
                suVO.Headimg = cVO.Headimg;
                suVO.Phone = cVO.Phone;
                suVO.PartyID = cVO.PartyID;
                suVO.FormId = cVO.FormId;
                suVO.OpenId = cVO.OpenId;
                suVO.CreatedAt = DateTime.Now;
                suVO.SignUpForm = cVO.SignUpForm;
                suVO.InviterCID = cVO.InviterCID;
                suVO.Number = cVO.Number;
                if (cVO.Type == 2)
                {
                    suVO.isDeliver = 0;
                }

                int PartySignUpID = cBO.AddCardToParty(suVO);
                //if (PartySignUpID > 0)
                //{
                //    cBO.sendSignUpMessage(PartySignUpID);
                //}
                //修改订单
                BCPartyOrderVO cpVO = new BCPartyOrderVO();
                cpVO.InviterCID = cVO.InviterCID;
                cpVO.PartyOrderID = cVO.PartyOrderID;
                cpVO.Status = 1;
                cpVO.payAt = DateTime.Now;
                cpVO.PartySignUpID = PartySignUpID;
                cBO.UpdatePartyOrder(cpVO);

                return new ResultObject() { Flag = 5, Message = "0元报名成功", Result = PartySignUpID };
            }


            int total_fee_1 = Convert.ToInt32((cVO.Cost * 100));
            string NOTIFY_URL = "https://api.leliaomp.com/BusinessCardTest/Pay/BusinessCardParty_Notify_Url.aspx";

            string title = Regex.Replace(cVO.Title, @"\p{Cs}", "");
            string costName = Regex.Replace(cVO.CostName, @"\p{Cs}", "");

            //if (isH5 == 1)
            //{
            //    appid = cBO.appidH5;
            //}

            //如果有个人二级商户，就用二级商户结账
            //EcommerceBO eBO = new EcommerceBO();
            //if (cVO.sub_mchid != "")
            //{
            //    NOTIFY_URL = "https://api.leliaomp.com/Pay/Ecommerce_Party_Notify_Url.aspx";
            //    string description = costName + "*" + cVO.Number;
            //    bool profit_sharing = false;
            //    if (cVO.SplitCost > 0) profit_sharing = true;

            //    AppletsPayDataVO res = await eBO.GetPay(appid, cVO.sub_mchid, description, cVO.OrderNO, total_fee_1, cVO.OpenId, NOTIFY_URL, profit_sharing);
            //    if (res == null)
            //    {
            //        return new ResultObject() { Flag = 0, Message = "支付失败，请咨询客服", Result = null };
            //    }

            //    JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            //    string s = jsonSerializer.Serialize(res);
            //    return new ResultObject() { Flag = 1, Message = "成功", Result = s };
            //}

            WxPayData wp = Ja.GetUnifiedOrderResult(appid, cVO.OrderNO, cVO.OpenId, total_fee_1.ToString(), "报名收费", costName, "报名收费", NOTIFY_URL, AppType);

            if (wp != null)
            {
                string reslut = Ja.GetJsApiParameters(wp, AppType);
                if (reslut != "")
                {
                    return new ResultObject() { Flag = 1, Message = "成功", Result = reslut };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "支付失败，请重新下单", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "支付失败，请重新下单", Result = null };
            }
        }

        /// <summary>
        /// 获取活动分享推广数据
        /// </summary>
        ///<param name="PageIndex"></param>
        ///<param name="PageCount"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getPartyShare"), HttpGet]
        public ResultObject getPartyShare(int PartyID, int PageIndex, int PageCount, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            try
            {
                CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
                CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
                BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
                PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
                string sql = "ById=" + PartyID + " and ToPersonalID=" + pVO.PersonalID + "  and (Type='ReadParty' or  Type='ForwardParty' or  Type='SignUpParty')";
                List<AccessrecordsViewVO> cVO = cBO.FindAccessrecordsViewAllByPageIndex(sql, (PageIndex - 1) * PageCount + 1, PageIndex * PageCount, "AccessAt", "desc");
                int Count = cBO.FindAccessrecordsViewCount(sql);

                List<BCPartySignUpViewVO> csVO = cBO.FindSignUpViewByInviterCID(PartyID, customerId, CustomerVO2.AppType);

                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = cVO, Count = Count, Subsidiary = new { SignCount = csVO.Count } };
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = ex };
            }
        }

        /// <summary>
        /// 获取邀请人列表
        /// </summary>
        ///  <param name="InviterCID"></param>
        /// <param name="PartyID"></param>
        /// <returns></returns>
        [Route("GetSignUpInviterlistData"), HttpGet, Anonymous]
        public ResultObject GetSignUpInviterlistData(int PartyID, int InviterCID, int AppType = 1)
        {
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            if (AppType == 0)
            {
                AppType = 1;
            }
            List<BCPartySignUpViewVO> cVO = cBO.FindSignUpViewByInviterCID(PartyID, InviterCID, AppType);
            if (cVO != null)
            {
                if (cVO.Count > 0)
                {
                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = cVO };
                }
                else
                {
                    return new ResultObject() { Flag = 2, Message = "暂无数据!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }
        #endregion

        #region 名片列表
        /// <summary>
        /// 获取我的名片列表
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getMyCardlist"), HttpGet]
        public ResultObject getMyCardlist(string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO.AppType);
            List<CardDataVO> uVO = cBO.FindCardByCustomerId(customerId);
            if (uVO != null)
            {
                if (uVO.Count > 0)
                {
                    foreach (CardDataVO item in uVO)
                    {
                        if (CustomerVO.isVip && CustomerVO.ExpirationAt > DateTime.Now)
                        {
                            item.isVip = true;
                        }
                        else
                        {
                            item.isVip = false;
                        }

                    }

                    //获取最新消息提醒
                    List<CardMessageVO> CardMessageVO = cBO.FindCardMessageByCondtion("CustomerId = " + customerId + " ORDER BY CreatedAt desc limit 5");
                    int CardMessageCount = cBO.FindCardMessageCount("CustomerId = " + customerId + " and Status=0");

                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO, Subsidiary = CardMessageVO, Count = CardMessageCount };
                }
                else
                {
                    return new ResultObject() { Flag = 2, Message = "没有名片!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 下载名片夹,分页
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DownloadCollectionListsByPageIndex"), HttpPost]
        public ResultObject DownloadCollectionListsByPageIndex([FromBody] CollectionListVO CollectionList, string token)
        {
            if (CollectionList.Condition == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            else if (CollectionList.Condition.Filter == null || CollectionList.Condition.PageInfo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO.AppType);
            try
            {
                string conditionStr = "t_CustomerId = " + customerId + " and (" + CollectionList.Condition.Filter.Result() + ")";
                Paging pageInfo = CollectionList.Condition.PageInfo;
                List<CardDataVO> list = cBO.FindCardCollectionAllByPageIndex(conditionStr, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);

                foreach (CardDataVO item in list)
                {
                    CustomerVO CuVO = CustomerBO.FindCustomenById(item.CustomerId);
                    if (CuVO.isVip && CuVO.ExpirationAt > DateTime.Now)
                    {
                        item.isVip = true;
                    }
                    else
                    {
                        item.isVip = false;
                    }
                }

                int count = cBO.FindCardCollectionAllCount(conditionStr);
                /*
                for (int i = 0; i < list.Count; i++) {
                    list[i].TongCardCount = cBO.FindTongCardByCustomerId(list[i].CustomerId, customerId).Count;
                }
                */
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list, Count = count, Subsidiary = count };
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = CollectionList };
            }

        }
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getPersonlist"), HttpGet]
        public ResultObject getPersonlist(string token, string name)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO per = cBO.FindPersonalByCustomerId(customerId);
            if (per != null)
            {
                string condition = "PersonalID != " + per.PersonalID;
                if (name != "")
                {
                    condition += " AND Name Like '%" + name + "%' ";
                }
                List<PersonalVO> Personal = cBO.FindPersonalList(condition);
                if (Personal.Count > 0)
                {
                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = Personal };
                }
                else
                {
                    return new ResultObject() { Flag = 2, Message = "未查询到数据!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }
        #endregion


        #region 订单信息
        /// <summary>
        /// 获取我的订单列表
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("FindMyPartyOrderlist"), HttpPost]
        public ResultObject FindMyPartyOrderlist([FromBody] ConditionModel condition, string token)
        {
            try
            {
                if (condition == null)
                {
                    return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
                }
                else if (condition.Filter == null || condition.PageInfo == null)
                {
                    return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
                }
                UserProfile uProfile = CacheManager.GetUserProfile(token);
                CustomerProfile cProfile = uProfile as CustomerProfile;
                int customerId = cProfile.CustomerId;

                CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
                CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
                BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile(),CustomerVO2.AppType);
                string conditionStr = " CustomerID=" + customerId;
                int count = 0;
                Paging pageInfo = condition.PageInfo;
                List<BCPartyOrderViewVO> uVO = cBO.FindPartyOrderViewByCustomerId(conditionStr, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);
                count = cBO.FindPartyOrderTotalCount(conditionStr);
                if (uVO != null)
                {
                    if (uVO.Count > 0)
                    {
                        return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO, Count = count };
                    }
                    else
                    {
                        return new ResultObject() { Flag = 2, Message = "暂无订单!", Result = null };
                    }
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
                }
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                return new ResultObject() { Flag = 0, Message = strErrorMsg, Result = null };
            }
        }

        /// <summary>
        /// 获取活动订单
        /// </summary>
        /// <param name="PartyOrderID"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getPartyOrderDetails"), HttpGet]
        public ResultObject getPartyOrderDetails(int PartyOrderID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            BCPartyOrderViewVO cVO = cBO.FindPartyOrderViewById(PartyOrderID);

            if (cVO == null)
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };


            if (cVO.CustomerId != customerId)
                return new ResultObject() { Flag = 0, Message = "只有本人才能查看订单!", Result = null };


            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = cVO };
        }

        #endregion

        #region 粤省情

        /// <summary>
        /// 获取首页页数据，匿名
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getCardByShareYSQ"), HttpGet, Anonymous]
        public ResultObject getCardByShareYSQ(int PersonalID, int SortID = 0, int OrderNO = 0, int BusinessID = 0, int AppType = 0)
        {
            try
            {
                BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
                if (PersonalID == 0)
                {
                    PersonalID = AppBO.GetApp(AppType).TPersonalID;
                }
                PersonalVO pVO = cBO.FindPersonalById(PersonalID);
                if (pVO != null)
                {
                    //pVO.ReadNum = cBO.FindNumberOfVisitors("Personal", pVO.PersonalID);
                    //pVO.todayReadNum = cBO.FindNumberOfVisitors("Personal", pVO.PersonalID, 1, 0);

                    //if (pVO.QRimg == "")
                    //{
                    //    //pVO.QRimg = cBO.GetQRImgByHeadimg(pVO.PersonalID, AppType);
                    //}

                    //if (pVO.PosterImg3 == "" || BusinessID > 0)
                    //{
                    //    pVO.PosterImg3 = cBO.GetPosterCardIMG(pVO.PersonalID, BusinessID);
                    //}

                    WebVO WebVO = new WebVO();
                    List<InfoViewVO> product = new List<InfoViewVO>();
                    List<InfoViewVO> CaseList = new List<InfoViewVO>();

                    if (BusinessID == 0 || BusinessID == pVO.BusinessID)
                        BusinessID = pVO.BusinessID;
                    else
                    {
                        //判断是否是指定公司成员
                        List<SecondBusinessVO> sListVO = cBO.FindSecondBusinessByPersonalID(pVO.PersonalID, BusinessID);
                        if (sListVO.Count <= 0)
                        {
                            BusinessID = pVO.BusinessID;
                        }
                        else
                        {
                            pVO.Position = sListVO[0].Position;
                        }
                    }

                    ThemeVO ThemeVO = new ThemeVO();
                    ThemeVO.setThemeVO();
                    BusinessCardVO BusinessCardVO = null;
                    if (BusinessID > 0)
                    {
                        WebVO = cBO.FindYSQWebByBusinessID(BusinessID);


                        string Order = "Order_info desc,CreatedAt desc";
                        if (OrderNO == 1)
                        {
                            Order = "CreatedAt desc";
                        }
                        if (OrderNO == 2)
                        {
                            Order = "ReadCount desc";
                        }
                        if (OrderNO == 3)
                        {
                            Order = "Cost asc";
                        }
                        //product = cBO.FindInfoViewByInfoID("Products", BusinessID, SortID, Order, 29);
                        CaseList = cBO.FindInfoViewByInfoID("Case", BusinessID, 0, "Order_info desc,CreatedAt desc", 20);

                        BusinessCardVO bVO = cBO.FindBusinessCardById(BusinessID);
                        if (bVO != null)
                        {
                            if (bVO.ThemeID > 0)
                            {
                                ThemeVO = cBO.FindThemeById(bVO.ThemeID);
                            }
                            BusinessCardVO = bVO;
                        }

                    }
                    else
                    {
                        WebVO = null;
                    }

                    int AccessNumber = cBO.FindNumberOfVisits("Personal", pVO.PersonalID, 1); //名片访问次数

                    string conditionStr = "ToPersonalID =" + pVO.PersonalID + " and PersonalID<>" + pVO.PersonalID;
                    List<AccessrecordsViewVO> Accesslist = new List<AccessrecordsViewVO>();
                    Accesslist = cBO.FindAccessrecordsViewGroupAllByPageIndex(conditionStr, 1, 10, "AccessAt", "desc");

                    //获取他的任务
                    string condition = " CustomerId = " + pVO.CustomerId + " and Status=1 and EffectiveEndDate > now() ";
                    RequireBO uBO = new RequireBO(new CustomerProfile());
                    List<RequirementViewVO> list = uBO.FindRequireAllByPageIndex(condition, 1, 10, "CreatedAt", "desc");
                    int count = uBO.FindRequireTotalCount(condition);

                    //BusinessCard_JurisdictionVO B_Jurisdiction = cBO.getBusinessCard_Jurisdiction(BusinessID);
                    List<InfoViewVO> videolist = cBO.FindInfoViewAllByPageIndex("Type ='Video'  AND Status<>-2 and Status<>0 AND Video!='' AND BusinessID =" + BusinessID, 1, 3, "Order_info desc,", "CreatedAt desc");
                    //首页显示活动 IsDisplayIndex=1 and EndTime > now() and
                    List<BCPartyVO> partyvo = cBO.FindBCPartyByPage(" AppType=" + pVO.AppType, 1, 6, "IsDisplayIndex", "desc");
                    List<QuestionnaireDataVO> qVO = cBO.FindQuestionnaireByFour(4);
                    object res = new
                    {
                        Personal = pVO,
                        Web = WebVO,
                        PartyList = partyvo,
                        Theme = ThemeVO,
                        coverList = qVO,
                        Videolist = videolist,
                        //B_Jurisdiction = B_Jurisdiction,
                        //Products = product,
                        AccessNumber = AccessNumber,
                        Accesslist = Accesslist,
                        RequirementList = list,
                        RequirementCount = count,
                        CaseList = CaseList,
                        BusinessCard = BusinessCardVO,
                    };
                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = res };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
                }
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = -1, Message = "获取失败!", Result = ex.ToString() };
            }

        }

        /// <summary>
        /// 获取我的名片在首页
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getMyCardByYSQIndex"), HttpGet]
        public ResultObject getMyCardByYSQIndex(string token, int AppType = 0)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            CrmBO CrmBO = new CrmBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);

            try
            {
                if (pVO != null)
                {
                    BusinessCardVO bVO = new BusinessCardVO();
                    ThemeVO ThemeVO = new ThemeVO();
                    ThemeVO.setThemeVO();
                    if (pVO.BusinessID == 0)
                    {
                        bVO = null;
                    }
                    else
                    {
                        bVO = cBO.FindBusinessCardById(pVO.BusinessID);
                        //清除营业执照等保密信息
                        bVO.BusinessLicenseImg = "";
                        if (bVO.ThemeID > 0)
                        {
                            ThemeVO = cBO.FindThemeById(bVO.ThemeID);
                        }
                    }

                    if (pVO.QRimg == "")
                    {
                        pVO.QRimg = cBO.GetQRImgByHeadimg(pVO.PersonalID, AppType);
                    }

                    if (pVO.PosterImg3 == "")
                    {
                        pVO.PosterImg3 = cBO.GetPosterCardIMG(pVO.PersonalID, pVO.BusinessID);
                    }

                    if (pVO.PosterImg == "")
                    {
                        pVO.PosterImg = cBO.GetPersonalIMG(pVO.PersonalID);
                    }

                    int ReadNum = 0, toReadNum = 0, todayReadNum = 0, notUsedOrderCount = 0, UsedOrderCount = 0, SecondBusinessCount = 0, ReturnCardCount = 0, UnreadCount = 0, TenderInviteCount = 0;
                    List<AccessrecordsViewVO> aVO = new List<AccessrecordsViewVO>();
                    List<AccessrecordsViewByRespondentsVO> ToaVO = new List<AccessrecordsViewByRespondentsVO>();
                    List<AccessrecordsViewVO> ReturnCardVO = new List<AccessrecordsViewVO>();
                    decimal Balance = 0;
                    decimal Balance2 = 0;

                    try
                    {
                        ReadNum = cBO.FindNumberOfVisitors("Personal", pVO.PersonalID);
                        toReadNum = cBO.FindNumberOfVisitors("Personal", pVO.PersonalID, 1);
                        todayReadNum = cBO.FindNumberOfVisitors("Personal", pVO.PersonalID, 1, 0);
                        aVO = cBO.FindAccessrecordsListOfMonth(null, pVO.PersonalID, 0, 1, false);
                        ToaVO = cBO.FindAccessrecordsViewByRespondentsGroupAllByPageIndex("PersonalID=" + pVO.PersonalID + " and ToPersonalID<>" + pVO.PersonalID + " and ToPersonalID<>1557", 1, 12, "AccessAt", "desc");

                        notUsedOrderCount = cBO.FindOrderViewCount("PersonalID=" + pVO.PersonalID + " and Status=1 and isUsed=0 and OfficialProducts IS NOT NULL");
                        UsedOrderCount = cBO.FindOrderByCondition("PersonalID=" + pVO.PersonalID + " and Status=1 and isUsed>0");
                        SecondBusinessCount = cBO.FindSecondBusinessByPersonalID(pVO.PersonalID).Count;

                        ReturnCardVO = cBO.FindAccessrecordsViewByCondtion("Type = 'ReturnCard' and ToPersonalID=" + pVO.PersonalID + " GROUP BY PersonalID ORDER BY AccessAt DESC LIMIT 1");
                        ReturnCardCount = cBO.FindReturnCardCount(pVO.PersonalID);

                        UnreadCount = CrmBO.FindCommentViewCount(pVO.PersonalID, pVO.BusinessID) + CrmBO.FindApprovalCount(pVO.PersonalID, pVO.BusinessID);

                        RequireBO rBO = new RequireBO(new CustomerProfile());
                        TenderInviteCount = rBO.FindRequireSumByCondtion("TenderInviteCount", "CustomerId=" + pVO.CustomerId);

                        //可提现余额
                        Balance = cBO.getMyRebateCost(customerId, 1);

                        //累计奖金
                        Balance2 = cBO.getMyRebateCost(customerId, 0);
                    }
                    catch
                    {

                    }

                    //直播信息
                    bool isline = false;
                    int lineNo = 3;
                    string lineTitle = "订阅直播";
                    string lineDesc = "7月30号晚上8点准时开播，现场抽奖";
                    List<InfoVO> banner = cBO.FindInfoByCondtion("Type IN ('Banner','MyBanner','MallBanner') AND BusinessID=" + bVO.BusinessID);

                    BusinessCard_JurisdictionVO B_Jurisdiction = cBO.getBusinessCard_Jurisdiction(pVO.BusinessID);
                    JurisdictionViewVO jVO = cBO.FindJurisdictionView(pVO.PersonalID, pVO.BusinessID);
                    List<BCPartyVO> partyvo = cBO.FindBCPartyByCondtion(" AppType=" + pVO.AppType);
                    object res = new
                    {
                        Personal = pVO,
                        BusinessCard = bVO,
                        Jurisdiction = jVO,

                        PartyList = partyvo,
                        B_Jurisdiction = B_Jurisdiction,
                        Banner = banner,
                        Theme = ThemeVO,
                        ReadNum = ReadNum,
                        toReadNum = toReadNum,
                        todayReadNum = todayReadNum,
                        AccessList = aVO,
                        ToAccessList = ToaVO,
                        notUsedOrderCount = notUsedOrderCount,
                        UsedOrderCount = UsedOrderCount,
                        ReturnCard = ReturnCardVO,
                        ReturnCardCount = ReturnCardCount,
                        SecondBusinessCount = SecondBusinessCount,
                        UnreadCount = UnreadCount,
                        TenderInviteCount = TenderInviteCount,
                        Balance = Balance,
                        Balance2 = Balance2,
                        isline,
                        lineNo,
                        lineTitle,
                        lineDesc
                    };
                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = res };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
                }
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = ex };
            }

        }
        #endregion

        #region 问卷
        /// <summary>
        /// 获取问卷列表
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getQuestionnairelist"), HttpPost, Anonymous]
        public ResultObject getQuestionnairelist([FromBody] ConditionModel condition, int status)
        {
            if (condition == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            else if (condition.Filter == null || condition.PageInfo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            //UserProfile uProfile = CacheManager.GetUserProfile(token);
            //CustomerProfile cProfile = uProfile as CustomerProfile;
            //int customerId = cProfile.CustomerId;
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            string conditionStr = "";
            if (status == 0)
            {
                conditionStr = " 1=1  AND status <> 3 ";
            }
            else if (status == 1)
            {
                conditionStr = " status = 1 AND status <> 3 ";//查询进行中的问卷 等于1且不等于3
            }
            else if (status == 2)
            {
                conditionStr = " status = 2 AND status <> 3 ";//查询结束的问卷 等于2且不等于3
            }

            Paging pageInfo = condition.PageInfo;
            List<QuestionnaireDataVO> qVO = cBO.GetQuestionnaireList(conditionStr, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);
            var count = cBO.FindQuestionnaireDataCount(conditionStr);
            if (qVO.Count > 0)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = qVO, Subsidiary = count };
            }
            return new ResultObject() { Flag = 2, Message = "未查询到数据!", Result = null };
        }



        /// <summary>
        /// 获取问卷前四条数据
        /// </summary>
        /// <returns></returns>
        [Route("GetQuestionnaireLimt"), HttpGet]
        public ResultObject GetQuestionnaireLimt(int limit, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            List<QuestionnaireDataVO> qVO = cBO.FindQuestionnaireByFour(limit);
            if (qVO.Count > 0)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = qVO, Subsidiary = qVO.Count };
            }
            return new ResultObject() { Flag = 2, Message = "未查询到数据!", Result = null };
        }
        #endregion


        #region 云米粒首页数据
        /// <summary>
        /// 获取分享页数据，匿名
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getCardByShareYML"), HttpGet, Anonymous]
        public ResultObject getCardByShareYML(int PersonalID, int SortID = 0, int OrderNO = 0, int BusinessID = 0, int AppType = 0)
        {
            try
            {
                BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
                if (PersonalID == 0)
                {
                    PersonalID = AppBO.GetApp(AppType).TPersonalID;
                }
                PersonalVO pVO = cBO.FindPersonalById(PersonalID);
                if (pVO != null)
                {
                    pVO.ReadNum = cBO.FindNumberOfVisitors("Personal", pVO.PersonalID);
                    pVO.todayReadNum = cBO.FindNumberOfVisitors("Personal", pVO.PersonalID, 1, 0);

                    if (pVO.QRimg == "")
                    {
                        pVO.QRimg = cBO.GetQRImgByHeadimg(pVO.PersonalID, AppType);
                    }

                    if (pVO.PosterImg3 == "" || BusinessID > 0)
                    {
                        pVO.PosterImg3 = cBO.GetPosterCardIMG(pVO.PersonalID, BusinessID);
                    }

                    WebVO WebVO = new WebVO();
                    List<InfoViewVO> product = new List<InfoViewVO>();
                    //List<InfoViewVO> CaseList = new List<InfoViewVO>();
                    List<InfoSortVO> CaseList = new List<InfoSortVO>();

                    if (BusinessID == 0 || BusinessID == pVO.BusinessID)
                        BusinessID = pVO.BusinessID;
                    else
                    {
                        //判断是否是指定公司成员
                        List<SecondBusinessVO> sListVO = cBO.FindSecondBusinessByPersonalID(pVO.PersonalID, BusinessID);
                        if (sListVO.Count <= 0)
                        {
                            BusinessID = pVO.BusinessID;
                        }
                        else
                        {
                            pVO.Position = sListVO[0].Position;
                        }
                    }

                    ThemeVO ThemeVO = new ThemeVO();
                    ThemeVO.setThemeVO();
                    BusinessCardVO BusinessCardVO = null;
                    if (BusinessID > 0)
                    {
                        WebVO = cBO.FindWebByBusinessID(BusinessID);


                        string Order = "Order_info desc,CreatedAt desc";
                        if (OrderNO == 1)
                        {
                            Order = "CreatedAt desc";
                        }
                        if (OrderNO == 2)
                        {
                            Order = "ReadCount desc";
                        }
                        if (OrderNO == 3)
                        {
                            Order = "Cost asc";
                        }
                        product = cBO.FindInfoViewByInfoID("Products", BusinessID, SortID, Order, 29);
                        //CaseList = cBO.FindInfoViewByInfoID("Case", BusinessID, 0, "Order_info desc,CreatedAt desc", 20);
                        CaseList = cBO.FindInfoSortList("special_column", pVO.BusinessID);

                        BusinessCardVO bVO = cBO.FindBusinessCardById(BusinessID);
                        if (bVO != null)
                        {
                            if (bVO.ThemeID > 0)
                            {
                                ThemeVO = cBO.FindThemeById(bVO.ThemeID);
                            }
                            BusinessCardVO = bVO;
                        }

                    }
                    else
                    {
                        WebVO = null;
                    }

                    //获取导航
                    List<InfoSortVO> sVO = cBO.FindNavigationList(BusinessID);
                    for (int i = 0; i < sVO.Count; i++)
                    {
                        sVO[i].InfoSortlist = cBO.FindNavigationList(pVO.BusinessID, sVO[i].SortID);
                    }
                    int AccessNumber = cBO.FindNumberOfVisits("Personal", pVO.PersonalID, 1); //名片访问次数

                    string conditionStr = "ToPersonalID =" + pVO.PersonalID + " and PersonalID<>" + pVO.PersonalID;
                    List<AccessrecordsViewVO> Accesslist = new List<AccessrecordsViewVO>();
                    Accesslist = cBO.FindAccessrecordsViewGroupAllByPageIndex(conditionStr, 1, 10, "AccessAt", "desc");

                    //获取他的任务
                    string condition = " CustomerId = " + pVO.CustomerId + " and Status=1 and EffectiveEndDate > now() ";
                    RequireBO uBO = new RequireBO(new CustomerProfile());
                    List<RequirementViewVO> list = uBO.FindRequireAllByPageIndex(condition, 1, 10, "CreatedAt", "desc");
                    int count = uBO.FindRequireTotalCount(condition);

                    BusinessCard_JurisdictionVO B_Jurisdiction = cBO.getBusinessCard_Jurisdiction(BusinessID);

                    List<InfoSortVO> InfoSort = cBO.FindInfoSortList("ProductSort", BusinessID);

                    InfoSortVO SVO = new InfoSortVO();
                    SVO.SortID = 0;
                    SVO.SortName = "全部分类";
                    InfoSort.Reverse();
                    InfoSort.Add(SVO);
                    InfoSort.Reverse();

                    //首页显示活动
                    List<BCPartyVO> partyvo = cBO.FindBCPartyByCondtion("AppType = " + pVO.AppType + " and IsDisplayIndex=1 and EndTime > now()");

                    object res = new
                    {
                        Personal = pVO,
                        Web = WebVO,
                        PartyList = partyvo,
                        Theme = ThemeVO,
                        Navigation = sVO,
                        B_Jurisdiction = B_Jurisdiction,
                        Products = product,
                        AccessNumber = AccessNumber,
                        Accesslist = Accesslist,
                        RequirementList = list,
                        RequirementCount = count,
                        CaseList = CaseList,
                        BusinessCard = BusinessCardVO,
                        ProductSort = InfoSort
                    };
                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = res };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
                }
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = -1, Message = "获取失败!", Result = ex.ToString() };
            }

        }
        #endregion


        #region 粤省情抽奖
        /// <summary>
        /// 获取抽奖活动列表
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getCJLotteriesList"), HttpPost]
        public ResultObject getCJLotteriesList([FromBody] ConditionModel condition, string token)
        {
            try
            {
                LogBO _log = new LogBO(typeof(BusinessCardController));
                _log.Info("获取抽奖活动列表");
                if (condition == null)
                {
                    return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
                }
                else if (condition.Filter == null || condition.PageInfo == null)
                {
                    return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
                }
                UserProfile uProfile = CacheManager.GetUserProfile(token);
                CustomerProfile cProfile = uProfile as CustomerProfile;
                int customerId = cProfile.CustomerId;

                string conditionStr = "1=1  and (" + condition.Filter.Result() + ")";
                BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
                Paging pageInfo = condition.PageInfo;
                List<CJLotteriesVO> qVO = cBO.GetCJLotteriesList(conditionStr, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);
                var count = cBO.FindCJLotteriesCount(conditionStr);
                if (qVO.Count > 0)
                {
                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = qVO, Count = count, Subsidiary = condition, Subsidiary2 = conditionStr };
                }
                return new ResultObject() { Flag = 2, Message = "未查询到数据!", Result = null };
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = -1, Message = "未查询到数据!", Result = ex };
            }

        }

        /// <summary>
        /// 获取抽奖活动信息
        /// </summary>
        /// <returns></returns>
        [Route("GetCJLotteriesDetial"), HttpGet]
        public ResultObject GetCJLotteriesDetial(int lottery_id, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            CJLotteriesVO qVO = cBO.FindCJLotteriesById(lottery_id);
            if (qVO != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = qVO };
            }
            return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
        }


        /// <summary>
        /// 添加或更新销售目标
        /// </summary>
        /// <param name="lotteriesVO"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateLotteries"), HttpPost]
        public ResultObject UpdateLotteries([FromBody] CJLotteriesVO lotteriesVO, string token)
        {
            try
            {
                UserProfile uProfile = CacheManager.GetUserProfile(token);
                CustomerProfile cProfile = uProfile as CustomerProfile;
                int customerId = cProfile.CustomerId;

                BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
                PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
                if (lotteriesVO.lottery_id > 0)
                {
                    if (cBO.UpdateCJLotteries(lotteriesVO))
                    {
                        return new ResultObject() { Flag = 1, Message = "更新成功!", Result = lotteriesVO.lottery_id };
                    }
                    else
                    {
                        return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                    }
                }
                else
                {
                    lotteriesVO.created_at = DateTime.Now;
                    lotteriesVO.personal_id = pVO.PersonalID;

                    int personal_id = cBO.AddCJLotteries(lotteriesVO);
                    if (personal_id > 0)
                        return new ResultObject() { Flag = 1, Message = "添加成功!", Result = personal_id };
                    else
                        return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
                }
            }
            catch (Exception ex)
            {

                return new ResultObject() { Flag = -1, Message = "添加失败!", Result = ex };
            }

        }

        /// <summary>
        ///  生成红包接口
        /// </summary>
        /// <returns></returns>
        [Route("ReceivePrize"), HttpPost]
        public ResultObject ReceivePrize(int lottery_id, string code, string token)
        {
            try
            {
                UserProfile uProfile = CacheManager.GetUserProfile(token);
                CustomerProfile cProfile = uProfile as CustomerProfile;
                int customerId = cProfile.CustomerId;

                BusinessCardBO cBO = new BusinessCardBO(cProfile);
                PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
                CJLotteriesVO lotteries = cBO.FindCJLotteriesById(lottery_id);
                if (pVO == null)
                    return new ResultObject() { Flag = 0, Message = "用户未注册!", Result = null };
                // 1. 验证抽奖活动
                if (lotteries == null)
                    return new ResultObject() { Flag = 0, Message = "抽奖活动不存在!", Result = null };

                // 1表示进行中
                if (lotteries.status != 1)
                    return new ResultObject() { Flag = 0, Message = "抽奖活动未在进行中!", Result = null };

                // 检查活动时间
                //if (DateTime.Now < qVO.start_time || DateTime.Now > qVO.end_time)
                //    return new ResultObject() { Flag = 0, Message = "不在活动时间内!", Result = null };

                // 2. 验证用户是否已参与
                int participationCheck = cBO.FindWinningRecordCount("lottery_id =" + lottery_id + " AND personal_id = " + pVO.PersonalID);
                if (participationCheck > 0)
                {
                    return new ResultObject() { Flag = 0, Message = "您已参与过此活动!", Result = null };
                }

                // 3. 检查剩余金额和数量
                decimal used_amount = cBO.FindWinningRecordSum("winning_amount", "lottery_id =" + lottery_id);
                if ((lotteries.total_amount - used_amount) <= 0)
                {
                    return new ResultObject() { Flag = 0, Message = "奖品已发完!", Result = null };
                }

                // 4. 生成随机金额
                decimal Amount = cBO.GenerateRandomAmount(lotteries.min_value, lotteries.max_value, lotteries.total_amount, used_amount);

                if (Amount <= 0)
                {
                    return new ResultObject() { Flag = 0, Message = "金额不足!", Result = null };
                }

                // 5. 保存中奖记录
                int recordId = cBO.SaveWinningRecord(lottery_id, pVO.PersonalID, Amount, code, pVO.AppType);
                if (recordId > 0)
                {
                    object res = new
                    {
                        amount = Amount,
                        winningrecords_id = recordId
                    };
                    return new ResultObject() { Flag = 1, Message = "领取成功!", Result = res };
                }

                return new ResultObject() { Flag = 0, Message = "领取失败!", Result = null };
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = -1, Message = "系统错误，请稍后重试!", Result = ex };
            }
        }

        /// <summary>
        ///  领取红包接口
        /// </summary>
        /// <returns></returns>
        [Route("ReceivePrizePay"), HttpPost]
        public ResultObject ReceivePrizePay(int lottery_id, int winningrecords_id, string token)
        {
            try
            {
                UserProfile uProfile = CacheManager.GetUserProfile(token);
                CustomerProfile cProfile = uProfile as CustomerProfile;
                int customerId = cProfile.CustomerId;

                BusinessCardBO cBO = new BusinessCardBO(cProfile);
                PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
                CJLotteriesVO lotteries = cBO.FindCJLotteriesById(lottery_id);
                if (pVO == null)
                    return new ResultObject() { Flag = 0, Message = "用户未注册!", Result = null };
                // 1. 验证抽奖活动
                if (lotteries == null)
                    return new ResultObject() { Flag = 0, Message = "抽奖活动不存在!", Result = null };

                // 1表示进行中
                if (lotteries.status != 1)
                    return new ResultObject() { Flag = 0, Message = "抽奖活动未在进行中!", Result = null };

                // 检查活动时间
                //if (DateTime.Now < qVO.start_time || DateTime.Now > qVO.end_time)
                //    return new ResultObject() { Flag = 0, Message = "不在活动时间内!", Result = null };

                // 2. 验证用户是否已参与
                int participationCheck = cBO.FindWinningRecordCount("lottery_id =" + lottery_id + " AND personal_id = " + pVO.PersonalID);
                if (participationCheck > 1)
                {
                    return new ResultObject() { Flag = 0, Message = "您已参与过此活动!", Result = null };
                }

                // 3. 检查剩余金额和数量
                decimal used_amount = cBO.FindWinningRecordSum("winning_amount", "lottery_id =" + lottery_id);
                if ((lotteries.total_amount - used_amount) <= 0)
                {
                    return new ResultObject() { Flag = 0, Message = "红包已发完!", Result = null };
                }

                var records = cBO.FindCJWinningRecordsById(winningrecords_id);
                if (records == null)
                {
                    return new ResultObject() { Flag = 0, Message = "红包未领取!", Result = null };
                }
                if (records.status == 1)
                {
                    return new ResultObject() { Flag = 0, Message = "红包已领取!", Result = null };
                }
                // 5.  调用领取方法 保存中奖记录
                var transferResult = cBO.WinningRecordPayment(records, pVO.AppType);

                // 构建返回给前端的结果
                var result = new ResultObject
                {
                    Flag = transferResult.Success ? 1 : 0,
                    Message = transferResult.Message,
                    Result = new
                    {
                        success = transferResult.Success,
                        state = transferResult.State,
                        message = transferResult.Message,
                        out_bill_no = transferResult.OutBillNo,
                        transfer_bill_no = transferResult.TransferBillNo,
                        package_info = transferResult.PackageInfo,
                        create_time = transferResult.CreateTime,
                        need_user_confirm = transferResult.State == "WAIT_USER_CONFIRM",
                        error_code = transferResult.Code,
                        error_detail = transferResult.ErrorDetail
                    }
                };

                return result;
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = -1, Message = "系统错误，请稍后重试!", Result = ex };
            }
        }
        #endregion

        #region 签到表格
        /// <summary>
        /// 添加或更新签到表
        /// </summary>
        /// <param name="CardRegistertableVO"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateQuestionnaire"), HttpPost]
        public ResultObject UpdateQuestionnaire([FromBody] CardRegistertableVO CardRegistertableVO, string token)
        {
            if (CardRegistertableVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile(), CustomerVO2.AppType);

            /*审核文本是否合法*/
            if (!cBO.msg_sec_check(CardRegistertableVO))
            {
                return new ResultObject() { Flag = 0, Message = "有政治敏感或违法关键词，请重新填写!", Result = null };
            }
            /*审核文本是否合法*/

            if (CardRegistertableVO.QuestionnaireID > 0)
            {

                if (CardRegistertableVO.CustomerId != customerId && !cBO.isQuestionnaireAdmin(CardRegistertableVO.QuestionnaireID, customerId))
                {
                    return new ResultObject() { Flag = 0, Message = "更新失败,你没有权限!", Result = null };
                }

                if (cBO.UpdateCardRegistertable(CardRegistertableVO))
                {
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = CardRegistertableVO.QuestionnaireID };
                }
                else
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }
            else
            {
                CardRegistertableVO.CreatedAt = DateTime.Now;
                CardRegistertableVO.CustomerId = customerId;

                int QuestionnaireID = cBO.AddQuestionnaire(CardRegistertableVO);
                if (QuestionnaireID > 0)
                {
                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = QuestionnaireID };
                }
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
        }

        /// <summary>
        /// 添加或更新签到表管理员
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <param name="QuestionnaireID"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateQuestionnaireAdmin"), HttpGet]
        public ResultObject UpdateQuestionnaireAdmin(string CustomerId, int QuestionnaireID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile(), CustomerVO2.AppType);

            CardRegistertableVO CardRegistertableVO = cBO.FindCardRegistertableByQuestionnaireID(QuestionnaireID);
            if (CardRegistertableVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
            }
            if (CardRegistertableVO.CustomerId != customerId)
            {
                return new ResultObject() { Flag = 0, Message = "更新失败,你没有权限!", Result = null };
            }
            try
            {
                if (!string.IsNullOrEmpty(CustomerId))
                {
                    string[] messageIdArr = CustomerId.Split(',');
                    bool isAllDelete = true;
                    cBO.DeleteQuestionnaireAdminById(QuestionnaireID);
                    for (int i = 0; i < messageIdArr.Length; i++)
                    {
                        try
                        {
                            int customerid = Convert.ToInt32(messageIdArr[i]);
                            if (!cBO.isQuestionnaireAdmin(QuestionnaireID, customerid))
                            {
                                CardRegistertableAdminVO cCVO = new CardRegistertableAdminVO();
                                cCVO.QuestionnaireID = QuestionnaireID;
                                cCVO.CustomerId = customerid;
                                cBO.AddQuestionnaireAdmin(cCVO);
                            }
                        }
                        catch
                        {
                            isAllDelete = false;
                        }
                    }
                    if (isAllDelete)
                    {
                        return new ResultObject() { Flag = 1, Message = "修改管理员成功!", Result = null };
                    }
                    else
                    {
                        return new ResultObject() { Flag = 1, Message = "修改管理员成功!", Result = null };
                    }
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
                }
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "修改管理员失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取我的签到表列表
        /// </summary>
        ///<param name="PageIndex"></param>
        ///<param name="PageCount"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getMyQuestionnaireList"), HttpGet]
        public ResultObject getMyQuestionnaireList(int PageIndex, int PageCount, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile(), CustomerVO2.AppType);
            List<CardRegistertableVO> qVO = cBO.FindCardRegistertableByCondtion("CustomerId = " + customerId + " and isTemplate=0");


            //仅作为管理员的签到表
            List<CardRegistertableVO> newlVO = new List<CardRegistertableVO>();
            List<CardRegistertableAdminVO> aVO = cBO.FindQuestionnaireAdminByCondition("CustomerId = " + customerId);

            for (int i = 0; i < aVO.Count; i++)
            {
                try
                {
                    if (!qVO.Exists(p => p.QuestionnaireID == aVO[i].QuestionnaireID))
                    {
                        CardRegistertableVO cVo = cBO.FindCardRegistertableByQuestionnaireID(aVO[i].QuestionnaireID);

                        if (cVo != null)
                        {
                            newlVO.Add(cVo);
                        }
                    }
                }
                catch
                {

                }
            }

            if (qVO.Count > 0)
                qVO.AddRange(newlVO);
            else
                qVO = newlVO;

            IEnumerable<CardRegistertableVO> newqVO = qVO.OrderBy(f => f.CreatedAt).Reverse().Skip((PageIndex - 1) * PageCount).Take(PageCount);

            foreach (CardRegistertableVO item in newqVO)
            {
                try
                {
                    item.PeopleCount = cBO.FindCardRegistertableSignup("QuestionnaireID = " + item.QuestionnaireID + " and Status=1");
                }
                catch
                {

                }
            }

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = newqVO, Count = qVO.Count + newlVO.Count };
        }

        /// <summary>
        /// 获取签到表模板列表
        /// </summary>
        /// <returns></returns>
        [Route("getQuestionnaireByTemplate"), HttpGet, Anonymous]
        public ResultObject getQuestionnaireByTemplate()
        {
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            List<CardRegistertableVO> qVO = cBO.FindCardRegistertableByCondtion("isTemplate = 1");
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = qVO, };
        }

        /// <summary>
        /// 获取签到表详情
        /// </summary>
        ///<param name="QuestionnaireID"></param>
        /// <returns></returns>
        [Route("getQuestionnaire"), HttpGet, Anonymous]
        public ResultObject getQuestionnaire(int QuestionnaireID, int AppType = 30)
        {
            try
            {
                BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile(), AppType);
                CardRegistertableVO qVO = cBO.FindCardRegistertableByQuestionnaireID(QuestionnaireID);

                if (qVO != null)
                {
                    var logger = new LogBO(typeof(BusinessCardController));
                    if (qVO.QRImg == "")
                    {
                        qVO.QRImg = cBO.GetQuestionnaireQR(QuestionnaireID, AppType);
                    }

                    BusinessCardBO BusinessCardBO = new BusinessCardBO(new CustomerProfile());
                    BusinessCardVO bVO = new BusinessCardVO();
                    if (qVO.BusinessID != 0)
                    {
                        bVO = BusinessCardBO.FindBusinessCardById(qVO.BusinessID);
                    }

                    List<CardRegistertableAdminVO> aVO = cBO.FindQuestionnaireAdminByCondition("QuestionnaireID = " + QuestionnaireID);

                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = qVO, Subsidiary = bVO, Subsidiary2 = aVO };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
                }
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(BusinessCardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = ex };
            }

        }

        /// <summary>
        /// 删除签到表
        /// </summary>
        /// <param name="QuestionnaireID">签到表ID</param>
        /// <param name="token"></param>
        /// <returns></returns>
        [Route("DelQuestionnaire"), HttpGet]
        public ResultObject DelQuestionnaire(int QuestionnaireID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile(), CustomerVO2.AppType);
            CardRegistertableVO uVO = cBO.FindCardRegistertableByQuestionnaireID(QuestionnaireID);


            if (uVO != null)
            {
                if (uVO.CustomerId != customerId)
                {
                    return new ResultObject() { Flag = 0, Message = "你没有权限删除!", Result = null };
                }
                //删除所有签到
                List<CardRegistertableSignupVO> qsVO = cBO.FindCardRegistertableSignupByCondtion("QuestionnaireID = " + QuestionnaireID);
                for (int i = 0; i < qsVO.Count; i++)
                {
                    cBO.DeleteByQuestionnaireSignupID(qsVO[i].QuestionnaireSignupID);
                }
                cBO.DeleteByQuestionnaireID(QuestionnaireID);

                //删除管理员
                cBO.DeleteQuestionnaireAdminById(QuestionnaireID);

                return new ResultObject() { Flag = 1, Message = "删除成功!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
            }
        }

        /// <summary>
        /// 添加或更新签到
        /// </summary>
        /// <param name="CardRegistertableSignupVO"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateQuestionnaireSignup"), HttpPost]
        public ResultObject UpdateQuestionnaire([FromBody] CardRegistertableSignupVO CardRegistertableSignupVO, string token)
        {
            if (CardRegistertableSignupVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile(), CustomerVO2.AppType);

            int oldCount = cBO.FindCardRegistertableSignup("CustomerId = " + customerId + " and QuestionnaireID=" + CardRegistertableSignupVO.QuestionnaireID + " and NOW()-CreatedAt<30");
            if (oldCount > 0)
            {
                return new ResultObject() { Flag = 0, Message = "操作过于频繁，请稍后!", Result = null };
            }
            try
            {


                /*审核文本是否合法*/
                if (!cBO.msg_sec_check(CardRegistertableSignupVO))
                {
                    return new ResultObject() { Flag = 0, Message = "有政治敏感或违法关键词，请重新填写!", Result = null };
                }
                /*审核文本是否合法*/

                if (CardRegistertableSignupVO.QuestionnaireSignupID > 0)
                {
                    if (cBO.UpdateCardRegistertableSignup(CardRegistertableSignupVO))
                    {
                        return new ResultObject() { Flag = 1, Message = "更新成功!", Result = CardRegistertableSignupVO.QuestionnaireSignupID };
                    }
                    else
                        return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }
                else
                {
                    CardRegistertableVO qVO = cBO.FindCardRegistertableByQuestionnaireID(CardRegistertableSignupVO.QuestionnaireID);

                    if (qVO == null)
                    {
                        return new ResultObject() { Flag = 0, Message = "签到失败，该签到项目已被删除!", Result = null };
                    }

                    if (qVO.limitPeopleNum > 0)
                    {
                        int PeopleNum = cBO.FindCardRegistertableSignup("QuestionnaireID=" + qVO.QuestionnaireID + " and Status=1");
                        if (PeopleNum >= qVO.limitPeopleNum)
                        {
                            return new ResultObject() { Flag = 2, Message = "签到失败，签到人数已满!", Result = null };
                        }
                    }

                    if (!qVO.isRepeat)
                    {
                        int PeopleNum = cBO.FindCardRegistertableSignup("QuestionnaireID=" + qVO.QuestionnaireID + " and CustomerId=" + customerId + " and Status=1");
                        if (PeopleNum > 0)
                        {
                            return new ResultObject() { Flag = 2, Message = "签到失败，您已经提交过了!", Result = null };
                        }
                    }

                    CustomerBO uBO = new CustomerBO(new CustomerProfile());
                    CustomerVO uVO = uBO.FindCustomenById(customerId);
                    CardRegistertableSignupVO.CreatedAt = DateTime.Now;
                    CardRegistertableSignupVO.CustomerId = customerId;
                    CardRegistertableSignupVO.Headimg = uVO.HeaderLogo;

                    var models = JsonConvert.DeserializeObject<List<RegistertableSigupForm>>(CardRegistertableSignupVO.SigupForm);
                    for (int i = 0; i < models.Count; i++)
                    {
                        if (models[i].Name == "姓名")
                            CardRegistertableSignupVO.Name = models[i].value;
                        if (models[i].Name == "手机")
                            CardRegistertableSignupVO.Phone = models[i].value;
                    }

                    if (CardRegistertableSignupVO.Name == "")
                    {
                        CardRegistertableSignupVO.Name = uVO.CustomerName;
                    }
                    if (CardRegistertableSignupVO.Phone == "")
                    {
                        CardRegistertableSignupVO.Phone = uVO.Phone;
                    }

                    int QuestionnaireSignupID = cBO.AddQuestionnaireSignup(CardRegistertableSignupVO);
                    if (QuestionnaireSignupID > 0)
                    {
                        //List<CardDataVO> cVO = cBO.FindCardByCustomerId(customerId);
                        //if (cVO.Count <= 0)
                        //{
                        //    CardDataVO CardDataVO = new CardDataVO();
                        //    CardDataVO.Name = CardRegistertableSignupVO.Name;
                        //    CardDataVO.Phone = CardRegistertableSignupVO.Phone;
                        //    CardDataVO.Headimg = CardRegistertableSignupVO.Headimg;

                        //    try
                        //    {

                        //        string Position = "";
                        //        string CorporateName = "";
                        //        string Address = "";
                        //        decimal latitude = 0;
                        //        decimal longitude = 0;
                        //        string WeChat = "";

                        //        var model = JsonConvert.DeserializeObject<List<RegistertableSigupForm>>(CardRegistertableSignupVO.SigupForm);

                        //        for (int i = 0; i < model.Count; i++)
                        //        {
                        //            if (model[i].Name == "职位")
                        //                Position = model[i].value;
                        //            if (model[i].Name == "工作单位")
                        //                CorporateName = model[i].value;
                        //            if (model[i].Name == "微信")
                        //                WeChat = model[i].value;
                        //            if (model[i].Name == "单位地址")
                        //            {
                        //                Address = model[i].value;
                        //                WeiXinGeocoder Geocoder = cBO.getLatitudeAndLongitude(model[i].value);
                        //                if (Geocoder != null)
                        //                {
                        //                    latitude = Geocoder.result.location.lat;
                        //                    longitude = Geocoder.result.location.lng;
                        //                }
                        //            }
                        //        }

                        //        CardDataVO.Position = Position;
                        //        CardDataVO.CorporateName = CorporateName;
                        //        CardDataVO.Address = Address;
                        //        CardDataVO.WeChat = WeChat;
                        //        CardDataVO.latitude = latitude;
                        //        CardDataVO.longitude = longitude;
                        //    }
                        //    catch
                        //    {
                        //    }

                        //    CardDataVO.CreatedAt = DateTime.Now;
                        //    CardDataVO.Status = 1;//0:禁用，1:启用
                        //    CardDataVO.CustomerId = customerId;
                        //    CardDataVO.isQuestionnaire = 1;
                        //    cBO.AddCard(CardDataVO);
                        //}
                        //cBO.AddCardMessage(CustomerVO2.CustomerName + "填写了表格《" + qVO.Title + "》", qVO.CustomerId, "表格签到", "/pages/index/SignInFormByUserList/SignInFormByUserList?QuestionnaireID=" + qVO.QuestionnaireID);
                        return new ResultObject() { Flag = 1, Message = "添加成功!", Result = QuestionnaireSignupID };
                    }
                    else
                        return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
                }
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "添加失败!", Result = ex };
            }
        }

        /// <summary>
        /// 删除签到
        /// </summary>
        /// <param name="QuestionnaireSignupID">签到表ID</param>
        /// <param name="token"></param>
        /// <returns></returns>
        [Route("DelQuestionnaireSignup"), HttpGet]
        public ResultObject DelQuestionnaireSignup(int QuestionnaireSignupID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile(), CustomerVO2.AppType);

            CardRegistertableSignupVO uVO = cBO.FindCardRegistertableSignupByQuestionnaireSignupID(QuestionnaireSignupID);

            if (uVO != null)
            {
                uVO.Status = 0;
                cBO.UpdateCardRegistertableSignup(uVO);
                return new ResultObject() { Flag = 1, Message = "删除成功!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
            }
        }

        /// <summary>
        /// 清空签到列表
        /// </summary>
        ///<param name="QuestionnaireID"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("delQuestionnaireSignupByQuestionnaireID"), HttpGet]
        public ResultObject getQuestionnaireSignupByQuestionnaireID(int QuestionnaireID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile(), CustomerVO2.AppType);
            CardRegistertableVO qVO = cBO.FindCardRegistertableByQuestionnaireID(QuestionnaireID);
            if (qVO != null)
            {
                //删除所有签到
                List<CardRegistertableSignupVO> qsVO = cBO.FindCardRegistertableSignupByCondtion("QuestionnaireID = " + QuestionnaireID);
                for (int i = 0; i < qsVO.Count; i++)
                {
                    qsVO[i].Status = 0;
                    cBO.UpdateCardRegistertableSignup(qsVO[i]);
                }
                return new ResultObject() { Flag = 1, Message = "删除成功!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取签到列表
        /// </summary>
        ///<param name="PageIndex"></param>
        ///<param name="PageCount"></param>
        ///<param name="QuestionnaireID"></param>
        /// <param name="token">口令</param>
        /// <param name="Time"></param>
        /// <returns></returns>
        [Route("getQuestionnaireSignupByQuestionnaireID"), HttpGet]
        public ResultObject getQuestionnaireSignupByQuestionnaireID(int PageIndex, int PageCount, int QuestionnaireID, string token, string Time = "")
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile(), CustomerVO2.AppType);
            CardRegistertableVO qVO = cBO.FindCardRegistertableByQuestionnaireID(QuestionnaireID);
            if (qVO != null)
            {
                string sql = "QuestionnaireID = " + QuestionnaireID + " and Status=1";
                if (Time != "")
                {
                    sql += " and to_days(CreatedAt) = to_days('" + Time + "')";
                }
                List<CardRegistertableSignupVO> qsVO = cBO.FindCardRegistertableSignupByCondtion(sql);
                qsVO.Reverse();
                qVO.PeopleCount = qsVO.Count;
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = qsVO.Skip((PageIndex - 1) * PageCount).Take(PageCount), Count = qsVO.Count, Subsidiary = qVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }



        /// <summary>
        /// 获取签到二维码
        /// </summary>
        /// <param name="QuestionnaireID">分享路径</param>
        /// <returns></returns>
        [Route("GetQuestionnaireSignupQR"), HttpGet]
        public ResultObject GetQuestionnaireSignupQR(int QuestionnaireID, string token, int AppType = 1)
        {
            try
            {
                var logger = new LogBO(typeof(BusinessCardBO));
                UserProfile uProfile = CacheManager.GetUserProfile(token);
                CustomerProfile cProfile = uProfile as CustomerProfile;
                int customerId = cProfile.CustomerId;
                CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
                CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
                if (AppType == 1)
                {
                    AppType = CustomerVO2.AppType;
                }
                BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile(), AppType);
                string str = cBO.getQRIMGByIDAndType(QuestionnaireID, 5, AppType, customerId);
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = str };
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 下载签到表所有报名的Excel文件
        /// </summary>
        /// <param name="QuestionnaireID"></param>
        /// <returns></returns>
        [Route("getQuestionnaireSignupToExcel"), HttpGet]
        public ResultObject getQuestionnaireSignupToExcel(int QuestionnaireID, string token)
        {

            CustomerProfile uProfile = CacheManager.GetUserProfile(token) as CustomerProfile;
            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(uProfile.CustomerId);
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile(), CustomerVO2.AppType);
            CardRegistertableVO qVO = cBO.FindCardRegistertableByQuestionnaireID(QuestionnaireID);

            if (qVO == null) { return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null }; }
            if (uProfile.CustomerId != qVO.CustomerId) { return new ResultObject() { Flag = 0, Message = "权限不足，下载失败!", Result = null }; }

            List<CardRegistertableSignupVO> cVO = cBO.FindCardRegistertableSignupByCondtion("QuestionnaireID = " + QuestionnaireID + " and Status = 1");

            if (cVO != null)
            {
                if (cVO.Count > 0)
                {
                    DataTable dt = new DataTable();

                    dt.Columns.Add("序号", typeof(Int32));
                    dt.Columns.Add("姓名", typeof(String));
                    dt.Columns.Add("手机", typeof(String));
                    dt.Columns.Add("填写时间", typeof(DateTime));

                    for (int i = 0; i < cVO.Count; i++)
                    {

                        DataRow row = dt.NewRow();
                        row["序号"] = i + 1;
                        row["姓名"] = cVO[i].Name;
                        row["手机"] = cVO[i].Phone;
                        row["填写时间"] = cVO[i].CreatedAt;

                        try
                        {


                            var model = JsonConvert.DeserializeObject<List<RegistertableSigupForm>>(cVO[i].SigupForm);

                            for (int j = 0; j < model.Count; j++)
                            {
                                if (model[j].Name != "姓名" && model[j].Name != "手机")
                                {
                                    if (!dt.Columns.Contains(model[j].Name))
                                    {
                                        dt.Columns.Add(model[j].Name, typeof(String));
                                    }
                                    if (model[j].Type == 4)
                                    {
                                        row[model[j].Name] = (model[j].UrlList != null && model[j].UrlList.Count > 0)
                                       ? model[j].UrlList[0].url
                                       : string.Empty;
                                    }
                                    else
                                    {
                                        row[model[j].Name] = model[j].value ?? string.Empty;
                                    }

                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            LogBO _log = new LogBO(typeof(CardBO));
                            string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                            _log.Error(strErrorMsg);
                        }

                        dt.Rows.Add(row);//这样就可以添加了 
                    }

                    string FileName = cBO.DataToExcel(dt, "QuestionnaireSignUpExcel/", QuestionnaireID + ".xls");

                    if (FileName != null)
                    {
                        return new ResultObject() { Flag = 1, Message = "获取成功!", Result = FileName };
                    }
                    else
                    {
                        return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
                    }
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "暂无报名!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }
        #endregion

        #region 粤省情排行榜单
        /// <summary>
        /// 添加或更新榜单
        /// </summary>
        /// <param name="rankVO"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateRank"), HttpPost]
        public ResultObject UpdateRank([FromBody] RankVO rankVO, string token)
        {
            if (rankVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile(), CustomerVO2.AppType);
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            /*审核文本是否合法*/
            if (!cBO.msg_sec_check(rankVO))
            {
                return new ResultObject() { Flag = 0, Message = "有政治敏感或违法关键词，请重新填写!", Result = null };
            }
            /*审核文本是否合法*/

            if (rankVO.rank_list_id > 0)
            {
                if (cBO.UpdateRank(rankVO))
                {
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = rankVO.rank_list_id };
                }
                else
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }
            else
            {
                rankVO.created_at = DateTime.Now;
                rankVO.personal_id = pVO.PersonalID;
                rankVO.publisher = pVO.Name;
                rankVO.publish_time = DateTime.Now;
                int rank_list_id = cBO.AddRank(rankVO);
                if (rank_list_id > 0)
                {
                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = rank_list_id };
                }
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取榜单列表
        /// </summary>
        /// <returns></returns>
        [Route("QueryRankList"), HttpPost]
        public ResultObject QueryRankList([FromBody] ConditionModel condition, string token)
        {
            try
            {
                if (condition == null)
                {
                    return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
                }
                else if (condition.Filter == null || condition.PageInfo == null)
                {
                    return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
                }

                UserProfile uProfile = CacheManager.GetUserProfile(token);
                CustomerProfile cProfile = uProfile as CustomerProfile;
                int customerId = cProfile.CustomerId;

                BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
                PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);

                string conditionStr = " Status=1";

                Paging pageInfo = condition.PageInfo;

                List<RankVO> list = new List<RankVO>();
                int count = 0;

                list = cBO.FindRankAllByPageIndex(conditionStr, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);
                count = cBO.FindRankCount(conditionStr);

                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list, Count = count };
            }
            catch (Exception ex)
            {

                return new ResultObject() { Flag = 0, Message = ex.Message, Result = null };
            }
        }


        /// <summary>
        /// 获取榜单详情
        /// </summary>
        /// <returns></returns>
        [Route("QueryRankDeatail"), HttpGet, Anonymous]
        public ResultObject QueryRankDeatail(int rank_lists_id)
        {
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            RankVO sVO = cBO.FindRankById(rank_lists_id);

            if (sVO != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = sVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }


        /// <summary>
        /// 添加或更新榜单
        /// </summary>
        /// <param name="rankVO"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateRankItem"), HttpPost]
        public ResultObject UpdateRankItem([FromBody] RankItemVO rankItemVO, string token)
        {
            if (rankItemVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile(), CustomerVO2.AppType);
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            /*审核文本是否合法*/
            if (!cBO.msg_sec_check(rankItemVO))
            {
                return new ResultObject() { Flag = 0, Message = "有政治敏感或违法关键词，请重新填写!", Result = null };
            }
            /*审核文本是否合法*/

            if (rankItemVO.rank_items_id > 0)
            {
                if (cBO.UpdateRankItem(rankItemVO))
                {
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = rankItemVO.rank_items_id };
                }
                else
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }
            else
            {
                rankItemVO.created_at = DateTime.Now;
                int rank_items_id = cBO.AddRankItem(rankItemVO);
                if (rank_items_id > 0)
                {
                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = rank_items_id };
                }
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取榜单列表
        /// </summary>
        /// <returns></returns>
        [Route("QueryRankItemList"), HttpPost]
        public ResultObject QueryRankItemList([FromBody] ConditionModel condition, string token)
        {
            try
            {
                if (condition == null)
                {
                    return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
                }
                else if (condition.Filter == null || condition.PageInfo == null)
                {
                    return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
                }

                UserProfile uProfile = CacheManager.GetUserProfile(token);
                CustomerProfile cProfile = uProfile as CustomerProfile;
                int customerId = cProfile.CustomerId;

                BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
                PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);

                string conditionStr = " tl.status=1";

                Paging pageInfo = condition.PageInfo;

                List<RankItemListVO> list = new List<RankItemListVO>();
                int count = 0;

                list = cBO.FindRankItemAllByPageIndex(conditionStr, pageInfo.SortName, pageInfo.SortType, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount);
                count = cBO.FindRankItemCount(conditionStr);

                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list, Count = count };
            }
            catch (Exception ex)
            {

                return new ResultObject() { Flag = 0, Message = ex.Message, Result = null };
            }
        }
        #endregion

        /// <summary>
        /// 123
        /// </summary>
        /// <param name="code">第一次拉手返回Code</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("Test2"), HttpPost, Anonymous]
        public ResultObject Test2()
        {
            try
            {
                //人脸识别
                //CsharpVO CsharpVO = CsharpTest.Main("新闻",  0);
                BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile(), 1);
                var retu = cBO.WechatPayToChange(0.5M, 2, 23, "oYnrm7SHCMGz-Zws4aucMmNdjUHY", 30, "问卷调查中奖奖金");
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = retu };
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "获取失败,请重试!", Result = null };
            }
        }
    }
    class ActivityNameModel
    {
        public string Name { get; set; }
        public string Content { get; set; }

    }

    class ActivityAllModel
    {
        public ActivityVO avo { get; set; }
        public ActivityCountVO cvo { get; set; }

        public ActivityTicketVO tvo { get; set; }

        public ActivitySignTicketVO svo { get; set; }


        /// <summary>
        /// 是否已截止报名 1已截止  0 ：未截止
        /// </summary>
        public int endSign { get; set; }
        /// <summary>
        /// 1：已结束  0：进行中
        /// </summary>
        public int isEnd { get; set; }

        public int signDay { get; set; }
        public int signShi { get; set; }
        public int signFen { get; set; }

    }

    class ActivityTicketCountModel
    {
        public ActivityTicketVO tvo { get; set; }
        public int SignCount { get; set; }//报名人数
    }
    class StatisticsList
    {
        public int Personal; //名片
        public int Product;  //产品
        public int News;    //新闻
        public int Total; //总计
        public DateTime Time;
    };
}


