using System;
using System.Text;
using SPLibrary.CoreFramework.BO;
using SPLibrary.UserManagement.BO;
using SPLibrary.UserManagement.VO;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Web.UI.WebControls;

namespace SPlatformService.CustomerManagement
{
    public partial class AgencyExperienceEdit : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "销售管理";
                (this.Master as Shared.MasterPage).PageNameText = "案例经验审核";
            }

            int agencyExperienceId = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["AgencyExperienceId"]) ? "0" : Request.QueryString["AgencyExperienceId"]);
            this.hidAgencyExperienceId.Value = agencyExperienceId.ToString();

            StringBuilder sb = new StringBuilder();
            sb.Append("var hidAgencyExperienceId='").Append(hidAgencyExperienceId.ClientID).Append("';\n");
            Utilities.RegisterJs(Page, "JSCommonVar_AgencyExperienceCreateEdit", sb.ToString());
        }        
    }
}