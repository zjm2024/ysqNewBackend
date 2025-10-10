using System;
using System.Text;
using SPLibrary.CoreFramework.BO;
using SPLibrary.UserManagement.BO;

namespace SPlatformService.UserManagement
{
    public partial class ConfigCreateEdit : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "系统管理";
                (this.Master as Shared.MasterPage).PageNameText = "系统设置";
            }

            base.ValidPageRight("系统设置", "Read");
            
            StringBuilder sb = new StringBuilder();
            sb.Append("var hidConfigId='").Append(hidConfigId.ClientID).Append("';\n");
            Utilities.RegisterJs(Page, "JSCommonVar_ConfigCreateEdit", sb.ToString());
        }
    }
}