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
    public partial class ViolationBrowse : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "有害信息";
                //this.Master.PageNameText = "会员管理";
            }

            base.ValidPageRight("有害信息", "Read");
            UserBO uBO = new UserBO(UserProfile);
            hidIsEdit.Value = uBO.IsHasSecurity(UserProfile.UserId, "有害信息", "Edit").ToString().ToLower();

            StringBuilder sb = new StringBuilder();
            sb.Append("var hidIsEdit='").Append(hidIsEdit.ClientID).Append("';\n");
            Utilities.RegisterJs(Page, "JSCommonVar_CustomerBrowse", sb.ToString());
        }
    }
}