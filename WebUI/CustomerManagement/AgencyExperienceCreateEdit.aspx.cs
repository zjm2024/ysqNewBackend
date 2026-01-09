using System;
using System.Text;
using SPLibrary.CoreFramework.BO;
using SPLibrary.UserManagement.BO;
using SPLibrary.UserManagement.VO;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Web.UI.WebControls;

namespace WebUI.CustomerManagement
{
    public partial class AgencyExperienceCreateEdit : CustomerBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "我是销售";
                (this.Master as Shared.MasterPage).PageNameText = "我的案例经验";
            }            

            int agencyExperienceId = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["agencyExperienceId"]) ? "0" : Request.QueryString["agencyExperienceId"]);
            this.hidAgencyExperienceId.Value = agencyExperienceId.ToString();
            
            StringBuilder sb = new StringBuilder();
            sb.Append("var hidAgencyExperienceId='").Append(hidAgencyExperienceId.ClientID).Append("';\n");
            Utilities.RegisterJs(Page, "JSCommonVar_AgencyExperienceCreateEdit", sb.ToString());
        }
        
    }
}