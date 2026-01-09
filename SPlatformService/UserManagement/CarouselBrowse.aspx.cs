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
    public partial class CarouselBrowse : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "系统管理";
                (this.Master as Shared.MasterPage).PageNameText = "轮播消息";
            }

            base.ValidPageRight("轮播消息", "Read");
            UserBO uBO = new UserBO(UserProfile);
            hidIsDelete.Value = uBO.IsHasSecurity(UserProfile.UserId, "轮播消息", "Delete").ToString().ToLower();
            hidIsEdit.Value = uBO.IsHasSecurity(UserProfile.UserId, "轮播消息", "Edit").ToString().ToLower();

            StringBuilder sb = new StringBuilder();
            sb.Append("var hidIsDelete='").Append(hidIsDelete.ClientID).Append("';\n");
            sb.Append("var hidIsEdit='").Append(hidIsEdit.ClientID).Append("';\n");
            Utilities.RegisterJs(Page, "JSCommonVar_SuggestionBrowse", sb.ToString());
        }
    }
}