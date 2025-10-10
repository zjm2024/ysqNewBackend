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
    public partial class ProjectBrowse : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "任务管理";
                (this.Master as Shared.MasterPage).PageNameText = "项目列表";
            }

            base.ValidPageRight("项目列表", "Read");

            StringBuilder sb = new StringBuilder();
            Utilities.RegisterJs(Page, "JSCommonVar_ProjectBrowse", sb.ToString());
        }
    }
}