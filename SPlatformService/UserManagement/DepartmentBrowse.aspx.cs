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
    public partial class DepartmentBrowse : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "系统管理";
                (this.Master as Shared.MasterPage).PageNameText = "部门管理";
            }

            base.ValidPageRight("部门管理", "Read");
            UserBO uBO = new UserBO(UserProfile);
            hidIsDelete.Value = uBO.IsHasSecurity(UserProfile.UserId, "部门管理", "Delete").ToString().ToLower();
            hidIsEdit.Value = uBO.IsHasSecurity(UserProfile.UserId, "部门管理", "Edit").ToString().ToLower();

            StringBuilder sb = new StringBuilder();
            sb.Append("var hidIsDelete='").Append(hidIsDelete.ClientID).Append("';\n");
            sb.Append("var hidIsEdit='").Append(hidIsEdit.ClientID).Append("';\n");
            Utilities.RegisterJs(Page, "JSCommonVar_DepartmentEdit", sb.ToString());
        }
    }
}