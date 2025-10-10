using CoreFramework.VO;
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
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BusinessCard.ThirdLogin
{
    public partial class WXLoginPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CustomerController customerCon = new CustomerController();
                LogBO _bo = new LogBO(this.GetType());
                try
                {
                    //_bo.Info("  time= " +DateTime.Now.ToString());
                    //var weixinOAuth = new WXLogin();
                    //微信第一次握手后得到的code 和state
                    string code = string.IsNullOrEmpty(Request.QueryString["code"]) ? "" : Request.QueryString["code"];
                    string state = string.IsNullOrEmpty(Request.QueryString["state"]) ? "" : Request.QueryString["state"];
                    if (code == "" || code == "authdeny")
                    {
                        if (code == "")
                        {
                            //发起授权(第一次微信握手)
                            //string authUrl = weixinOAuth.GetWeiXinCode(AppID, AppSecret, Server.UrlEncode(Request.Url.ToString()), false);
                            //Response.Redirect(authUrl, true);
                            string returnURL = "http://www.zhongxiaole.net/BusinessCard/ThirdLogin/WXLoginPage.aspx";

                            ResultObject result;
                            result = customerCon.GetThirdPartURL("2", Server.UrlEncode(returnURL));

                            Response.Redirect(result.Result.ToString(), true);
                        }
                        else
                        {
                            Response.Write(" <script>function window.onload() {alert( '用户取消授权！' ); } </script> ");
                            // 用户取消授权
                            Response.Redirect("~/Login.aspx", true);
                        }
                    }
                    else
                    {
                        ////获取微信的Access_Token（第二次微信握手）
                        //var modelResult = weixinOAuth.GetWeiXinAccessToken(AppID, AppSecret, code);
                        ////获取微信的用户信息(第三次微信握手)
                        //var userInfo = weixinOAuth.GetWeiXinUserInfo(modelResult.SuccessResult.access_token, modelResult.SuccessResult.openid);
                        string returnURL = "http://www.zhongxiaole.net/BusinessCard/ThirdLogin/WXLoginPage.aspx";
                        ResultObject result = customerCon.GetThirdPartUserInfo("2", code, Server.UrlEncode(Request.Url.ToString()), state);
                        //ResultObject result = customerCon.GetThirdPartUserInfo("2", code, Server.UrlEncode(Request.Url.ToString()));

                        WeiXinHelper.WeiXinUserInfoResult userInfo = result.Result as WeiXinHelper.WeiXinUserInfoResult;

                        //用户信息（判断是否已经获取到用户的微信用户信息）
                        if (userInfo.Result && userInfo.UserInfo.openid != "")
                        {
                            //_bo.Info("userInfo.UserInfo.openid = " + userInfo.UserInfo.openid);
                            //根据OpenID判断是否已经注册，有则直接跳过。否则需要输入手机号码才能正常使用                    

                            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
                            CustomerBO uBO = new CustomerBO(new CustomerProfile());
                            CustomerViewVO customerVO = uBO.FindCustomerByOpenId(userInfo.UserInfo.openid, userInfo.UserInfo.unionid, "2");
                            CustomerLoginHistoryVO clHistoryVO = new CustomerLoginHistoryVO();
                            clHistoryVO.LoginAt = DateTime.Now;
                            clHistoryVO.Status = true;
                            clHistoryVO.LoginOS = HttpContext.Current.Request.Browser.Platform;
                            clHistoryVO.LoginIP = HttpContext.Current.Request.UserHostAddress;
                            clHistoryVO.LoginBrowser = HttpContext.Current.Request.Browser.Browser;

                            result = customerCon.GetCustomerByOpenID(userInfo.UserInfo.openid, userInfo.UserInfo.unionid, "2");
                            //_bo.Info("GetCustomerByOpenID  result= "+result.Flag.ToString()+" message:" + result.Message);
                            if (result.Flag == 1&& customerVO != null)
                            {
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
                                    pVO.CreatedAt = DateTime.Now;
                                    cBO.AddPersonal(pVO);
                                }

                                //记录登录信息               
                                clHistoryVO.CustomerId = customerVO.CustomerId;
                                uBO.AddCustomerLoginHistory(clHistoryVO);

                                CustomerLoginModel customerModelVO = result.Result as CustomerLoginModel;
                                HttpContext.Current.Session["#Session#TOKEN"] = customerModelVO.Token;
                                FormsAuthentication.SetAuthCookie("SPCustomer_" + customerModelVO.Customer.CustomerId.ToString(), false);
                                Response.Redirect("~/index.aspx", true);  
                            }
                            else
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
                                cVO.BusinessCard = true;

                                try
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
                                    customerMatchVO.OpenId = userInfo.UserInfo.openid;
                                    customerMatchVO.UnionID = userInfo.UserInfo.unionid;
                                    customerMatchVO.CustomerId = customerId;
                                    customerMatchVO.MatchType = "2";
                                    int customerId2 = uBO.AddCustomerMatch(customerMatchVO);
                                    if (customerId2 > 0)
                                    {
                                        CustomerViewVO customerVO2 = uBO.FindCustomerByOpenId(userInfo.UserInfo.openid, userInfo.UserInfo.unionid, "2");
                                        if (customerVO2 != null)
                                        {
                                            //添加企业名片个人信息
                                            PersonalVO pVO = new PersonalVO();
                                            pVO.CustomerId = customerVO2.CustomerId;
                                            pVO.Name = customerVO2.CustomerName;
                                            pVO.Headimg = customerVO2.HeaderLogo;
                                            pVO.CreatedAt = DateTime.Now;
                                            cBO.AddPersonal(pVO);

                                            //记录登录信息               
                                            clHistoryVO.CustomerId = customerVO2.CustomerId;
                                            uBO.AddCustomerLoginHistory(clHistoryVO);

                                            string token = CacheManager.TokenInsert(customerVO2.CustomerId);
                                            HttpContext.Current.Session["#Session#TOKEN"] = token;
                                            FormsAuthentication.SetAuthCookie("SPCustomer_" + customerVO2.CustomerId.ToString(), false);
                                            Response.Redirect("~/index.aspx", true);
                                        }
                                    }
                                }
                                Response.Write(" <script>function window.onload() {alert( '注册失败，请返回重新操作！' ); } </script> ");
                            }
                        }
                        else
                        {
                            //return Content("授权失败，请返回重新操作！");
                            Response.Write(" <script>function window.onload() {alert( '授权失败，请返回重新操作！' ); } </script> ");
                        }
                    }
                }
                catch (Exception err)
                {
                    
                }
            }
        }
    }
}