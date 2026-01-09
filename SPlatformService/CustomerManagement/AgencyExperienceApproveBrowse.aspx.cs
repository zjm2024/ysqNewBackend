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
    public partial class AgencyExperienceApproveBrowse : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "销售管理";
                (this.Master as Shared.MasterPage).PageNameText = "销售案例审核";
            }

            base.ValidPageRight("销售案例审核", "Read");
        }
    }
}