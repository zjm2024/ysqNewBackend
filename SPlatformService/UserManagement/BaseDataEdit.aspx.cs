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
    public partial class BaseDataEdit : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "系统管理";
                (this.Master as Shared.MasterPage).PageNameText = "基础数据";
            }

            base.ValidPageRight("基础数据", "Read");

            StringBuilder sb = new StringBuilder();
            sb.Append("var hidBaseDataId='").Append(hidBaseDataId.ClientID).Append("';\n");
            Utilities.RegisterJs(Page, "JSCommonVar_BaseDataEdit", sb.ToString());
        }
    }
}