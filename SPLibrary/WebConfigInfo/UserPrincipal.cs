using CoreFramework.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using SPLibrary.UserManagement.DAO;
using SPLibrary.UserManagement.VO;

namespace SPLibrary.CoreFramework.WebConfigInfo
{
    public sealed class UserPrincipal : IUserPrincipal
    {
        /// <summary>
        /// Gets the user profile.
        /// </summary>
        /// <value>The user profile.</value>
        public UserProfile UserProfile
        {
            get
            {
                int userId;
                UserProfile up = null;
                if (Equals(UserPrincipal.SessionProfile, null) || string.IsNullOrEmpty(SessionProfile.UserName))
                {
                    if (!Equals(HttpContext.Current, null))
                    {
                        int.TryParse(HttpContext.Current.User.Identity.Name.Replace("SPUser_", ""), out userId);
                    }
                    else
                    {
                        //For TaskEngine module 
                        userId = 1;
                    }


                    up = new UserProfile();
                                        
                    IUserViewDAO uDAO = new UserViewDAO(new UserProfile());
                    if (userId > 0){
                        UserViewVO uVO = uDAO.FindById(userId);
                        up.UserId = uVO.UserId;
                        up.LoginName = uVO.LoginName;
                        up.UserName = uVO.UserName;
                        up.DeaprtmentId = uVO.DepartmentId;
                        up.CompanyId = uVO.CompanyId;
                        up.Phone = uVO.Phone;
                        up.AppType = uVO.AppType;
                    }
                    if (!string.IsNullOrEmpty(up.UserName))
                    {
                        UserPrincipal.SessionProfile = up;
                    }
                }
                else
                {
                    up = UserPrincipal.SessionProfile;
                }
                return up;
            }
        }

        /// <summary>
        /// Get the state of current user login.
        /// </summary>
        public bool IsUserLoginIn
        {
            get { return (UserProfile != null && !string.IsNullOrEmpty(UserProfile.UserName)) ? true : false; }
        }

        /// <summary>
        /// Gets or sets the session profile.
        /// </summary>
        /// <value>The session profile.</value>
        private static UserProfile SessionProfile
        {
            get
            {

                if (HttpContext.Current == null || HttpContext.Current.Session["#Session#P#r#ofile"] == null)
                {
                    return null;

                }
                else
                {
                    return HttpContext.Current.Session["#Session#P#r#ofile"] as UserProfile;
                }

            }
            set
            {
                if (!Equals(HttpContext.Current, null))
                {
                    HttpContext.Current.Session["#Session#P#r#ofile"] = value;
                }
            }
        }

        /// <summary>
        /// Clears the profile session.
        /// </summary>
        public static void ClearProfileSession()
        {
            SessionProfile = new UserProfile();
            SessionProfile = null;
        }
    }
}
