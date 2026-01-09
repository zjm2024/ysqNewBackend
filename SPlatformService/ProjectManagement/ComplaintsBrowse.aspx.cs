using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SPLibrary.CoreFramework.BO;
using SPLibrary.UserManagement.BO;

namespace SPlatformService.ProjectManagement
{
    public partial class ComplaintsBrowse : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "任务管理";
                (this.Master as Shared.MasterPage).PageNameText = "维权申请";
            }

            base.ValidPageRight("维权申请", "Read");

            StringBuilder sb = new StringBuilder();
            Utilities.RegisterJs(Page, "JSCommonVar_ComplaintsBrowse", sb.ToString());
        }
    }
}