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
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebUI.ThirdLogin
{
    public partial class QQLoginPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string returnURL = Server.UrlEncode(Request.Url.ToString());
                User_info ui = new User_info();
                string verifier = string.IsNullOrEmpty(Request.QueryString["code"]) ? "" : Request.QueryString["code"]; //获取Authorization Code
                CustomerController customerCon = new CustomerController();
                if (string.IsNullOrEmpty(verifier))
                {
                    //QQLogin q = new QQLogin();
                    //string url = q.Authorize(returnURL);//这里调用
                    //Response.Write(url);
                    returnURL = Server.UrlEncode("http://www.zhongxiaole.net/ThirdLogin/QQLoginPage.aspx");
                    ResultObject result = customerCon.GetThirdPartURL("1", returnURL);

                    Response.Write(result.Result.ToString());
                }
                else
                {
                    returnURL = Server.UrlEncode("http://www.zhongxiaole.net/ThirdLogin/QQLoginPage.aspx");
                    ResultObject result = customerCon.GetThirdPartUserInfo("1", verifier, returnURL,"1");

                    ui = result.Result as User_info;

                    hidOpenId.Value = ui.OpenID;
                    //根据OpenID判断是否已经注册，有则直接跳过。否则需要输入手机号码才能正常使用
                    result = customerCon.GetCustomerByOpenID(ui.OpenID, "1");

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
                }
            }
            catch (Exception err)
            {
                lblMessage.Text = "操作失败";
                LogBO _bo = new LogBO(this.GetType());
                _bo.Error(" QQ login messs" + err.Message + "\r\n" + err.StackTrace);
              
            }
        }
        public string APIURL
        {
            get
            {
                return ConfigInfo.Instance.APIURL;
            }
        }
        protected void Done_Click(object sender, EventArgs e)
        {
            CustomerController customerCon = new CustomerController();
            //先注册，再添加匹配，然后自动登录

            ResultObject result = customerCon.RegisterCustomer(txtPhone1.Text,txtPassword1.Text);

            if (result.Flag == 1)
            {
                int customerId = Convert.ToInt32(result.Result);

                CustomerMatchVO matchVO = new CustomerMatchVO();
                matchVO.CustomerId = customerId;
                matchVO.OpenId = hidOpenId.Value;
                matchVO.MatchType = "1";
                
                result = customerCon.UpdateCustomerMatch(matchVO);
                if (result.Flag == 1)
                {
                    result = customerCon.GetCustomerByOpenID(hidOpenId.Value, "1");

                    CustomerLoginModel customerModelVO = result.Result as CustomerLoginModel;

                    HttpContext.Current.Session["#Session#TOKEN"] = customerModelVO.Token;

                    FormsAuthentication.SetAuthCookie("SPCustomer_" + customerModelVO.Customer.CustomerId.ToString(), false);
                    Response.Redirect("~/CustomerManagement/CustomerEdit.aspx", true);
                }
            }
            else
            {
                LogBO _bo = new LogBO(this.GetType());
                _bo.Error(" QQ login message " + result.Message + "\r\n" );
            }
        }
    }
}