using CoreFramework.VO;
using SPLibrary.CoreFramework.Logging.BO;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.CustomerManagement.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebUI.Card
{
    public partial class WX_Redirect_Url : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string auth_code= string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["auth_code"]) ? "" : HttpContext.Current.Request.QueryString["auth_code"];
            int CustomerId = Convert.ToInt32(string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["CustomerId"]) ? "0" : HttpContext.Current.Request.QueryString["CustomerId"]);
            WxThirdPartyBO wBO = new WxThirdPartyBO(new CustomerProfile());
            AuthorizationVO aVO = wBO.GetApi_Query_Auth(auth_code, CustomerId);
            if (aVO != null)
            {
                wBO.GetApi_Get_Authorizer_Info(aVO.AuthorizationID);
            }

            LogBO _log = new LogBO(this.GetType());
            _log.Info("第三方授权事件(回调):" + auth_code);
        }
    }
}