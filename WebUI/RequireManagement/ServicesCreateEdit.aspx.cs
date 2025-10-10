using System;
using System.Text;
using SPLibrary.CoreFramework.BO;
using SPLibrary.UserManagement.BO;
using SPLibrary.UserManagement.VO;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Web.UI.WebControls;

namespace WebUI.RequireManagement
{
    public partial class ServicesCreateEdit : CustomerBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "我是销售";
                (this.Master as Shared.MasterPage).PageNameText = "发布服务";
            }            

            int servicesId = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["ServicesId"]) ? "0" : Request.QueryString["ServicesId"]);
            this.hidServicesId.Value = servicesId.ToString();
            
            StringBuilder sb = new StringBuilder();
            sb.Append("var hidServicesId='").Append(hidServicesId.ClientID).Append("';\n");
            Utilities.RegisterJs(Page, "JSCommonVar_ServicesCreateEdit", sb.ToString());
        }
    }
}