using SPLibrary.CoreFramework.BO;
using System;
using System.Text;


namespace SPlatformService.CustomerManagement
{
    public partial class GroupJoinPeople : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            (this.Master as Shared.MasterPage).MenuText = "名片组";
            (this.Master as Shared.MasterPage).PageNameText = "组员信息";

            int systemPartID = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["GroupID"]) ? "0" : Request.QueryString["GroupID"]);
            this.GroupID.Value = systemPartID.ToString();
    

            StringBuilder sb = new StringBuilder();
            sb.Append("var GroupID='").Append(GroupID.ClientID).Append("';\n");
            Utilities.RegisterJs(Page, "JSCommonVar_CardMessageCreateEdit", sb.ToString());
        }
    }
}