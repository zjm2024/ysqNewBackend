using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SPLibrary.CoreFramework.BO;
using SPLibrary.UserManagement.BO;

namespace SPlatformService.UserManagement
{
    public partial class RoleCreateEdit : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "系统管理";
                (this.Master as Shared.MasterPage).PageNameText = "角色管理";
            }

            base.ValidPageRight("角色管理", "Read");

            int userTitleId = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["RoleId"]) ? "0" : Request.QueryString["RoleId"]);
            this.hidRoleId.Value = userTitleId.ToString();

            UserBO uBO = new UserBO(UserProfile);
            hidIsDelete.Value = uBO.IsHasSecurity(UserProfile.UserId, "角色管理", "Delete").ToString().ToLower();
            hidIsEdit.Value = uBO.IsHasSecurity(UserProfile.UserId, "角色管理", "Edit").ToString().ToLower();

            StringBuilder sb = new StringBuilder();
            sb.Append("var hidRoleId='").Append(hidRoleId.ClientID).Append("';\n");
            sb.Append("var hidIsDelete='").Append(hidIsDelete.ClientID).Append("';\n");
            sb.Append("var hidIsEdit='").Append(hidIsEdit.ClientID).Append("';\n");
            Utilities.RegisterJs(Page, "JSCommonVar_RoleEdit", sb.ToString());
        }
    }
}