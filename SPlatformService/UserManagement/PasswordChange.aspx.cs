using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SPLibrary.CoreFramework.BO;

namespace SPlatformService.UserManagement
{
    public partial class PasswordChange : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "个人中心";
                (this.Master as Shared.MasterPage).PageNameText = "修改密码";
            }

            //base.ValidPageRight("修改密码", "View");
            
            this.hidUserId.Value = UserProfile.UserId.ToString();            

            StringBuilder sb = new StringBuilder();
            sb.Append("var hidUserId='").Append(hidUserId.ClientID).Append("';\n");
            Utilities.RegisterJs(Page, "JSCommonVar_UserEdit", sb.ToString());
        }
    }
}