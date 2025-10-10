using System;
using System.Text;
using SPLibrary.CoreFramework.BO;
using SPLibrary.UserManagement.BO;

namespace SPlatformService.ProjectManagement
{
    public partial class ComplaintsCreateEdit : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "任务管理";
                (this.Master as Shared.MasterPage).PageNameText = "维权申请";
            }

            base.ValidPageRight("维权申请", "Read");

            int complaintsId = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["ComplaintsId"]) ? "0" : Request.QueryString["ComplaintsId"]);
            this.hidComplaintsId.Value = complaintsId.ToString();
            

            StringBuilder sb = new StringBuilder();
            sb.Append("var hidComplaintsId='").Append(hidComplaintsId.ClientID).Append("';\n");
            Utilities.RegisterJs(Page, "JSCommonVar_ComplaintsCreateEdit", sb.ToString());
        }
    }
}