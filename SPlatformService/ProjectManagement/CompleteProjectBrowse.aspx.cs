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
    public partial class CompleteProjectBrowse : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "任务管理";
                (this.Master as Shared.MasterPage).PageNameText = "成交案例库";
            }

            base.ValidPageRight("成交案例库", "Read");

            StringBuilder sb = new StringBuilder();
            Utilities.RegisterJs(Page, "JSCommonVar_ProjectBrowse", sb.ToString());
        }
    }
}