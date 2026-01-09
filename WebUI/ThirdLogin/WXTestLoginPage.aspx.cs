using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebUI.ThirdLogin
{
    public partial class WXTestLoginPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Write(Request.QueryString["code"]);
        }
        protected void Done2_Click(object sender, EventArgs e)
        {
            string appId = txtPhone.Text.Trim();
            string URD = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + appId + "&redirect_uri=http%3a%2f%2fwww.zhongxiaole.net%2fThirdLogin%2fWXLoginPage.aspx&response_type=code&scope=snsapi_userinfo&state=STATE#wechat_redirect";

            Response.Redirect(URD);
        }
        protected void Done1_Click(object sender, EventArgs e)
        {
            string appId = txtPhone.Text.Trim();
            string URD = "https://open.weixin.qq.com/connect/qrconnect?appid=" + appId + "&redirect_uri=http%3a%2f%2fwww.zhongxiaole.net%2fThirdLogin%2fWXLoginPage.aspx&response_type=code&scope=snsapi_login&state=STATE#wechat_redirect";
            Response.Redirect(URD);
        }
        
    }
}