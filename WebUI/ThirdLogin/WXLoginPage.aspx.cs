using CoreFramework.VO;
using Newtonsoft.Json;
using SPlatformService.Controllers;
using SPlatformService.Models;
using SPLibrary.CoreFramework;
using SPLibrary.CoreFramework.BO;
using SPLibrary.CoreFramework.Logging.BO;
using SPLibrary.CustomerManagement.VO;
using SPLibrary.WebConfigInfo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebUI.ThirdLogin
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
                    string wxtype = string.IsNullOrEmpty(Request.QueryString["wxtype"]) ? "" : Request.QueryString["wxtype"];
                    if (code == "" || code == "authdeny")
                    {
                        if (code == "")
                        {
                            //发起授权(第一次微信握手)
                            //string authUrl = weixinOAuth.GetWeiXinCode(AppID, AppSecret, Server.UrlEncode(Request.Url.ToString()), false);
                            //Response.Redirect(authUrl, true);
                            string returnURL = "http://www.zhongxiaole.net/ThirdLogin/WXLoginPage.aspx";
                            
                            ResultObject result;
                            if (wxtype == "1")
                            {
                                result = customerCon.GetThirdPartURL("2", Server.UrlEncode(returnURL));
                            }else
                            {
                                result = customerCon.GetThirdPartURL("3", Server.UrlEncode(returnURL));
                            }
                            

                            Response.Redirect(result.Result.ToString(), true);
                        }
                        else
                        {
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
                        string returnURL = "http://www.zhongxiaole.net/ThirdLogin/WXLoginPage.aspx";
                        ResultObject result = customerCon.GetThirdPartUserInfo("2", code, Server.UrlEncode(Request.Url.ToString()), state);
                        //ResultObject result = customerCon.GetThirdPartUserInfo("2", code, Server.UrlEncode(Request.Url.ToString()));

                        WeiXinHelper.WeiXinUserInfoResult userInfo = result.Result as WeiXinHelper.WeiXinUserInfoResult;



                        //用户信息（判断是否已经获取到用户的微信用户信息）
                        if (userInfo.Result && userInfo.UserInfo.openid != "")
                        {
                            //_bo.Info("userInfo.UserInfo.openid = " + userInfo.UserInfo.openid);
                            hidOpenId.Value = userInfo.UserInfo.openid;
                            UnionID.Value = userInfo.UserInfo.unionid;
                            //根据OpenID判断是否已经注册，有则直接跳过。否则需要输入手机号码才能正常使用                    

                            result = customerCon.GetCustomerByOpenID(userInfo.UserInfo.openid, userInfo.UserInfo.unionid, "2");
                            //_bo.Info("GetCustomerByOpenID  result= "+result.Flag.ToString()+" message:" + result.Message);
                            if (result.Flag == 1)
                            {
                               
                               CustomerLoginModel customerModelVO = result.Result as CustomerLoginModel;

                                if (customerModelVO.Customer.Status != 1)
                                {
                                    // 用户被禁用
                                    string html = "<script type=\"text/javascript\">";
                                    html += "bootbox.dialog({";
                                    html += "message: '因违反平台规则，您的账号已被禁用！',";
                                    html += "buttons:";
                                    html += "{";
                                    html += "\"Confirm\":";
                                    html += "{";
                                    html += "\"label\": \"确定\",";
                                    html += "\"className\": \"btn-sm btn-primary\",";
                                    html += "\"callback\": function() {";
                                    html += "window.location.href = \"../Login.aspx\";";
                                    html += "}";
                                    html += "}";
                                    html += "}";
                                    html += "});";
                                    html += "</script>";
                                    mol.InnerHtml = html;
                                    return;
                                }
                                
                                HttpContext.Current.Session["#Session#TOKEN"] = customerModelVO.Token;

                                FormsAuthentication.SetAuthCookie("SPCustomer_" + customerModelVO.Customer.CustomerId.ToString(), false);
                                Response.Redirect("~/CustomerManagement/CustomerEdit.aspx", true);
                            }
                            else
                            {

                            }

                            ////根据OpenId判断数据库是否存在，如果存在，直接登录即可
                            //var oauthUser =
                            //    OperateContext<tb_UserLoginOauth>.SetServer.GetModel(m => m.OpenId == userInfo.UserInfo.openid);
                            //if (oauthUser != null)
                            //{
                            //    //直接登录即可  根据授权ID，查询用户信息
                            //    if (HttpContext.Session != null) HttpContext.Session["FytUser"] = oauthUser.tb_User;
                            //    UtilsHelper.WriteCookie("FytUserId",
                            //        DESEncrypt.Encrypt(oauthUser.tb_User.ID.ToString(CultureInfo.InvariantCulture)));
                            //}
                            //else
                            //{
                            //    //注册操作
                            //    OauthReg(userInfo);
                            //}
                            ////授权成功后，直接返回到首页
                            //return RedirectToAction("Index", "MHome");
                        }
                        else
                        {
                            //return Content("授权失败，请返回重新操作！");
                        }
                    }
                }
                catch (Exception err)
                {
                    lblMessage.Text = "操作失败";

                    _bo.Error(" WX login messs" + err.Message + "\r\n" + err.StackTrace);

                }
            }
        }

        protected void Done_Click(object sender, EventArgs e)
        {
            CustomerController customerCon = new CustomerController();
            LogBO _bo = new LogBO(this.GetType());
            //先注册，再添加匹配，然后自动登录
            ResultObject result = customerCon.RegisterCustomer(txtPhone.Text, txtPassword.Text);
            //_bo.Info("RegisterCustomer  result= " + result.Flag.ToString() + " message:" + result.Message);
            if (result.Flag == 1)
            {
                int customerId = Convert.ToInt32(result.Result);

                CustomerMatchVO matchVO = new CustomerMatchVO();
                matchVO.CustomerId = customerId;
                matchVO.OpenId = hidOpenId.Value;
                matchVO.UnionID = UnionID.Value;
                matchVO.MatchType = "2";

                result = customerCon.UpdateCustomerMatch(matchVO);
                if (result.Flag == 1)
                {
                    result = customerCon.GetCustomerByOpenID(hidOpenId.Value, "2");

                    CustomerLoginModel customerModelVO = result.Result as CustomerLoginModel;

                    HttpContext.Current.Session["#Session#TOKEN"] = customerModelVO.Token;

                    FormsAuthentication.SetAuthCookie("SPCustomer_" + customerModelVO.Customer.CustomerId.ToString(), false);
                    Response.Redirect("~/CustomerManagement/CustomerEdit.aspx", true);
                }
            }
            else if (result.Flag == 0 && result.Message == "登录名称已存在!")
            {
               
                int customerId = Convert.ToInt32(result.Result);

                CustomerMatchVO matchVO = new CustomerMatchVO();
                matchVO.CustomerId = customerId;
                matchVO.OpenId = hidOpenId.Value;
                matchVO.UnionID = UnionID.Value;
                matchVO.MatchType = "2";

                result = customerCon.UpdateCustomerMatch(matchVO);
                //_bo.Info("UpdateCustomerMatch = " + result.Flag.ToString() + " message:" + result.Message);
                if (result.Flag == 1)
                {
                    result = customerCon.GetCustomerByOpenID(hidOpenId.Value, "2");

                    CustomerLoginModel customerModelVO = result.Result as CustomerLoginModel;

                    HttpContext.Current.Session["#Session#TOKEN"] = customerModelVO.Token;

                    FormsAuthentication.SetAuthCookie("SPCustomer_" + customerModelVO.Customer.CustomerId.ToString(), false);
                    Response.Redirect("~/CustomerManagement/CustomerEdit.aspx", true);
                }
            }
            else
            {
                lblMessage.Text = result.Message;
            }
        }
        //public string APIURL
        //{
        //    get
        //    {
        //        return ConfigInfo.Instance.APIURL;
        //    }
        //}
    }
}