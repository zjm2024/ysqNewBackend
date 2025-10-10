using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SPLibrary.CoreFramework.BO;
using SPLibrary.UserManagement.BO;

namespace SPlatformService.CustomerManagement
{
    public partial class ZxbConfigBrowse : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "乐币奖励";
                //this.Master.PageNameText = "乐币奖励";
            }

            base.ValidPageRight("乐币奖励", "Read");
            UserBO uBO = new UserBO(UserProfile);
            hidIsEdit.Value = uBO.IsHasSecurity(UserProfile.UserId, "乐币奖励", "Edit").ToString().ToLower();

            StringBuilder sb = new StringBuilder();
            sb.Append("var hidIsEdit='").Append(hidIsEdit.ClientID).Append("';\n");
            Utilities.RegisterJs(Page, "JSCommonVar_CustomerBrowse", sb.ToString());
        }
    }
}