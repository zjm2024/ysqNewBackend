using SPlatformService.Models;
using SPlatformService.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using SPLibrary.CoreFramework.BO;
using SPLibrary.UserManagement.VO;
using CoreFramework.VO;
using SPLibrary.WebConfigInfo;

namespace SPlatformService
{
    public partial class Login : System.Web.UI.Page
    {
        public string SiteName
        {
            get
            {
                return CacheSystemConfig.GetSystemConfig().SiteName;
            }
        }
        public string SiteDescription
        {
            get
            {
                return CacheSystemConfig.GetSystemConfig().SiteDescription;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            string userName = txtLoginName.Text.Trim();
            string pwd = txtPassword.Text.Trim();

            UserController uc = new UserController();
            ResultObject result = uc.ValidAccount(userName, Utilities.GetMD5(pwd));

            if (result.Flag == 1)
            {
                UserLoginModel userModelVO = result.Result as UserLoginModel;
                if (userModelVO != null)
                {
                    HttpContext.Current.Session["#Session#TOKEN"] = userModelVO.Token;

                    FormsAuthentication.SetAuthCookie("SPUser_" + userModelVO.User.UserId.ToString(), false);
                    Response.Redirect("~/Default.aspx", true);
                }
                else
                {
                    lblMessage.Text = "未知错误，请重试！";
                }
            }
            else
            {
                lblMessage.Text = "账户或密码错误，请重新输入！";
            }            
        }
    }
}