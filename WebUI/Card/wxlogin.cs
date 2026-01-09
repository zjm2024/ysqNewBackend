using CoreFramework.VO;
using SPlatformService.Controllers;
using SPlatformService.Models;
using SPlatformService.TokenMange;
using SPLibrary.BusinessCardManagement.BO;
using SPLibrary.BusinessCardManagement.VO;
using SPLibrary.CoreFramework;
using SPLibrary.CoreFramework.BO;
using SPLibrary.CoreFramework.Logging.BO;
using SPLibrary.CoreFramework.WebConfigInfo;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.CustomerManagement.VO;
using SPLibrary.WebConfigInfo;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;

namespace WebUI.Card
{
    public class wxlogin : System.Web.UI.Page
    {
        public static string AppID_gzh = "wx9a65d7becbbb017a";
        public static string AppSecret_gzh = "2f75313a696eac6cef7a393641ff7a68";
        public static string Token
        {
            get
            {
                if (HttpContext.Current == null || HttpContext.Current.Session["#Session#TOKEN"] == null)
                {
                    return null;

                }
                else
                {
                    return HttpContext.Current.Session["#Session#TOKEN"].ToString();
                }
            }
        }
        public static int CustomerId
        {
            
            get
            {
                if (HttpContext.Current == null || HttpContext.Current.Session["#Session#CustomerId"] == null)
                {
                    return 0;

                }
                else
                {
                    return Convert.ToInt32(HttpContext.Current.Session["#Session#CustomerId"]);
                }
            }
        }
        public static void login()
        {
            CustomerController customerCon = new CustomerController();
            string state = string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["state"]) ? "" : HttpContext.Current.Request.QueryString["state"];

            //判断是否登录
            if (HttpContext.Current.Session["#Session#CustomerId"] != null)
            {
                return;
            }
            try
            {
                //_bo.Info("  time= " +DateTime.Now.ToString());
                //var weixinOAuth = new WXLogin();
                //微信第一次握手后得到的code 和state
                string code = string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["code"]) ? "" : HttpContext.Current.Request.QueryString["code"];
                string wxtype = string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["wxtype"]) ? "" : HttpContext.Current.Request.QueryString["wxtype"];
                
                if (code == "" || code == "authdeny")
                {
                    if (code == "")
                    {
                        //发起授权(第一次微信握手)
                        string returnURL = "https://" + HttpContext.Current.Request.Url.Host + "/" + HttpContext.Current.Request.Url.AbsolutePath;

                        //获取当前参数
                        string Queryurl = HttpUtility.UrlEncode(HttpContext.Current.Request.Url.Query);
                        

                        string url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + AppID_gzh + "&redirect_uri=" + returnURL + "&response_type=code&scope=snsapi_userinfo&state=" + Queryurl + "#wechat_redirect";
                        HttpContext.Current.Response.Redirect(url, true);
                        
                    }
                    else
                    {
                        // 用户取消授权
                        HttpContext.Current.Response.Write("<script>alert('微信登录失败');window.close();</script>");
                    }
                }
                else
                {
                    ////获取微信的用户信息
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

                                    CardBO CardBO = new CardBO(new CustomerProfile());
                                    //发送到期提醒
                                    CardBO.AddCardMessage("您的乐聊名片会员特权已到期", customerVO.CustomerId, "会员特权", "/pages/MyCenter/VipCenter/VipCenter");
                                }
                            }

                            //登录
                            string token = CacheManager.TokenInsert(customerVO.CustomerId);
                            HttpContext.Current.Session["#Session#TOKEN"] = token;
                            HttpContext.Current.Session["#Session#CustomerId"] = customerVO.CustomerId;
                            HttpContext.Current.Session["#Session#Openid"] = OpenId;
                            FormsAuthentication.SetAuthCookie("SPCustomer_" + customerVO.CustomerId.ToString(), false);

                            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
                            //判断是否有企业名片的个人信息
                            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerVO.CustomerId);
                            if (pVO != null)
                            {
                                CardBO CardBO = new CardBO(new CustomerProfile());
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
                        }
                        else
                        {

                            CustomerVO cVO = new CustomerVO();
                            cVO.CustomerCode = uBO.GetCustomerCode();
                            string password = Utilities.MakePassword(8);
                            cVO.Password = Utilities.GetMD5(password);
                            cVO.Status = 1;
                            cVO.CreatedAt = DateTime.Now;

                            string qs = HttpUtility.HtmlDecode(state);
                            try
                            {
                                cVO.originCustomerId = Convert.ToInt32(Regex.Match(qs, @"InviterCID=(?<text>[1-9]\d*)").Groups["text"].Value.Trim());
                            }
                            catch
                            {

                            }
                            

                            if (userInfo.UserInfo.nickname.Length > 0)
                            {
                                cVO.CustomerName = userInfo.UserInfo.nickname;
                            }

                            cVO.Sex = userInfo.UserInfo.sex == "1";
                            cVO.Leliao = true;

                            try
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

                                        //登录
                                        string token = CacheManager.TokenInsert(customerVO2.CustomerId);
                                        HttpContext.Current.Session["#Session#TOKEN"] = token;
                                        HttpContext.Current.Session["#Session#CustomerId"] = customerVO2.CustomerId;
                                        HttpContext.Current.Session["#Session#Openid"] = OpenId;
                                        FormsAuthentication.SetAuthCookie("SPCustomer_" + customerVO2.CustomerId.ToString(), false);

                                        //记录登录信息               
                                        clHistoryVO.CustomerId = customerVO2.CustomerId;
                                        uBO.AddCustomerLoginHistory(clHistoryVO);


                                    }
                                }
                            }
                        }

                        string url = "https://" + HttpContext.Current.Request.Url.Host + HttpContext.Current.Request.Url.AbsolutePath+ HttpUtility.HtmlDecode(state);
                        HttpContext.Current.Response.Redirect(url, true);
                    }
                    else
                    {
                        HttpContext.Current.Response.Write("<script>alert('微信登录失败');window.close();</script>");
                    }
                }
                return;
            }
            catch (Exception err)
            {
                HttpContext.Current.Response.Write("<script>alert('微信登录失败');window.close();</script>");
            }
        }
    }
}