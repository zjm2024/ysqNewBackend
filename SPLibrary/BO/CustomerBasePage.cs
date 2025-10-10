using CoreFramework;
using CoreFramework.VO;
using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using SPLibrary.CoreFramework.Logging.BO;
using SPLibrary.CoreFramework.WebConfigInfo;
using SPLibrary.UserManagement.BO;

namespace SPLibrary.CoreFramework.BO
{
    public class CustomerBasePage : System.Web.UI.Page
    {
        private CustomerProfile _customerProfile;
        protected const string SESSION_TIMEOUT = "SessionTimeOutName";
        protected const string CONNECTION_ERROR = "Connection_Error";

        protected void Page_Error(object sender, EventArgs e)
        {        
            Exception ex = Server.GetLastError();
            ExceptionLog(ex);
            Session["ExceptionHandleTip"] = ex.Message.ToString();
            Session["ExceptionHandleMsg"] = ex.StackTrace;
            Server.ClearError();
            Response.Redirect("~/ErrorMessage.aspx");
        }

        private void ExceptionLog(Exception exMsg)
        {
            LogBO _log = new LogBO(this.GetType());
            string strErrorMsg = "Message:" + exMsg.Message.ToString() + "\r\n  Stack :" + exMsg.StackTrace + " \r\n Source :" + exMsg.Source;
            _log.Error(strErrorMsg);
        }
        /// <summary>
        /// Gets the user profile.
        /// </summary>
        /// <value>The user profile.</value>
        public CustomerProfile CustomerProfile
        {
            get
            {
                if (_customerProfile == null)
                {
                    _customerProfile = new CustomerPrincipal().CustomerProfile;
                }
                return _customerProfile;
            }
        }
        protected void ClearCustomerProfile()
        {
            CustomerPrincipal.ClearProfileSession();
            this._customerProfile = null;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name) 
                || new CustomerPrincipal().CustomerProfile.CustomerId.ToString() != HttpContext.Current.User.Identity.Name.Replace("SPCustomer_", "")
                || HttpContext.Current.Session["#Session#TOKEN"] == null)
            {
                FormsAuthentication.SignOut();
                Session.Abandon(); 
                 SessionStateSection session = (SessionStateSection)ConfigurationManager.GetSection("system.web/sessionState");
                if (session != null)
                {
                    Response.Cookies.Add(new HttpCookie(session.CookieName, ""));
                }
                Response.Redirect("~/Login.aspx");
            }
        } 

        protected void ValidPageRight(string securityName,string actionName)
        {
            UserBO uBO = new UserBO(CustomerProfile);
            if (!uBO.IsHasSecurity(CustomerProfile.CustomerId, securityName, actionName))
            {
                Response.Redirect("~/NoSecurity.aspx");
            }
        }
        
    }
}
