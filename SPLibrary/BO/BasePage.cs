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
    public class BasePage : System.Web.UI.Page
    {
        private UserProfile _userProfile;
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
        public UserProfile UserProfile
        {
            get
            {
                if (_userProfile == null)
                {
                    _userProfile = new UserPrincipal().UserProfile;
                }
                return _userProfile;
            }
        }
        protected void ClearUserProile()
        {
            UserPrincipal.ClearProfileSession();
            this._userProfile = null;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name) 
                || new UserPrincipal().UserProfile.UserId.ToString() != HttpContext.Current.User.Identity.Name.Replace("SPUser_", "")
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
            UserBO uBO = new UserBO(UserProfile);
            if (!uBO.IsHasSecurity(UserProfile.UserId, securityName, actionName))
            {
                Response.Redirect("~/NoSecurity.aspx");
            }
        }
        
    }
}
