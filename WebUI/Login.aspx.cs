using System;
using System.Web;
using System.Runtime.Serialization;
using System.Web.Security;
using SPLibrary.CustomerManagement.VO;
using Newtonsoft.Json;
using SPLibrary.CoreFramework;
using CoreFramework.VO;
using SPLibrary.CoreFramework.BO;
using SPLibrary.CustomerManagement.BO;
using CoreFramework.DAO;
using System.Net;
using System.IO;
using WebUI.Common;
using SPlatformService.Models;

namespace WebUI
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string account = string.IsNullOrEmpty(Request.QueryString["account"]) ? "0" : Request.QueryString["account"];
            string password = string.IsNullOrEmpty(Request.QueryString["password"]) ? "0" : Request.QueryString["password"];
            string identity = string.IsNullOrEmpty(Request.QueryString["password"]) ? "0" : Request.QueryString["identity"];
            if (account != "0") {
                ResultObject result = SiteCommon.ValidCustomerAccount(account, password);
                if (result.Flag == 1)
                {
                    //CustomerLoginModel customerModelVO = JsonConvert.DeserializeObject<CustomerLoginModel>(result.Result.ToString());
                    CustomerLoginModel customerModelVO = result.Result as CustomerLoginModel;
                    if (customerModelVO != null)
                    {
                        HttpContext.Current.Session.Timeout = 240;
                        HttpContext.Current.Session["#Session#TOKEN"] = customerModelVO.Token;
                        FormsAuthentication.SetAuthCookie("SPCustomer_" + customerModelVO.Customer.CustomerId.ToString(), false);
                        if (identity == "1")
                        {
                            Response.Redirect("~/CustomerManagement/AgencyCreateEdit.aspx", true);
                        }
                        else if (identity == "2")
                        {
                            Response.Redirect("~/CustomerManagement/BusinessCreateEdit.aspx", true);
                        }
                        else
                        {
                            Response.Redirect("~/CustomerManagement/CustomerEdit.aspx", true);
                        }

                    }
                    else
                    {
                        lblMessage.Text = "未知错误，请重试！";
                    }
                }
                else
                {
                    lblMessage.Text = "账户密码错误，或者是账户已被禁用！";
                }
            }
        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            string userName = txtLoginName.Text.Trim();
            string pwd = txtPassword.Text.Trim();            

            ResultObject result = SiteCommon.ValidCustomerAccount(userName, pwd);


            if (result.Flag == 1)
            {
                //CustomerLoginModel customerModelVO = JsonConvert.DeserializeObject<CustomerLoginModel>(result.Result.ToString());
                CustomerLoginModel customerModelVO = result.Result as CustomerLoginModel;
                if (customerModelVO != null)
                {
                    HttpContext.Current.Session.Timeout = 240;
                    HttpContext.Current.Session["#Session#TOKEN"] = customerModelVO.Token;
                    FormsAuthentication.SetAuthCookie("SPCustomer_" + customerModelVO.Customer.CustomerId.ToString(), false);

                    Response.Redirect("~/CustomerManagement/CustomerEdit.aspx", true);
                }
                else
                {
                    lblMessage.Text = "未知错误，请重试！";
                }
            }
            else
            {
                lblMessage.Text = "账户密码错误，或者是账户已被禁用！";
            }

        }        
    }
}