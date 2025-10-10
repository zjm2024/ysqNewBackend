using System;
using System.Text;
using SPLibrary.CoreFramework.BO;
using SPLibrary.UserManagement.BO;
using SPLibrary.UserManagement.VO;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Web.UI.WebControls;

namespace WebUI.ProjectManagement
{
    public partial class BusinessProjectCreateEdit : CustomerBasePage
    {
        public string CustomerName
        {
            get { return CustomerProfile.CustomerName; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "我是雇主";
                (this.Master as Shared.MasterPage).PageNameText = "项目工作台";
            }

            int projectId = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["ProjectId"]) ? "0" : Request.QueryString["ProjectId"]);
            this.hidProjectId.Value = projectId.ToString();
                       

            StringBuilder sb = new StringBuilder();
            sb.Append("var hidProjectId='").Append(hidProjectId.ClientID).Append("';\n");
            Utilities.RegisterJs(Page, "JSCommonVar_ProjectCreateEdit", sb.ToString());
        }
    }
}