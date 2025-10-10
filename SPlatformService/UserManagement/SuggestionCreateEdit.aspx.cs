using System;
using System.Text;
using SPLibrary.CoreFramework.BO;
using SPLibrary.UserManagement.BO;
using SPLibrary.UserManagement.VO;
using CoreFramework.VO;

namespace SPlatformService.UserManagement
{
    public partial class SuggestionCreateEdit : BasePage
    {
        public int CustomerId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "系统管理";
                (this.Master as Shared.MasterPage).PageNameText = "意见反馈";
            }

            base.ValidPageRight("意见反馈", "Read");

            int suggestionId = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["SuggestionId"]) ? "0" : Request.QueryString["SuggestionId"]);
            this.hidSuggestionId.Value = suggestionId.ToString();

            UserBO uBO = new UserBO(UserProfile);
            SystemBO sBO = new SystemBO(new UserProfile());
            SuggestionVO vo = sBO.FindSuggestionById(suggestionId);

            CustomerId = vo.CustomerId;
            hidIsDelete.Value = uBO.IsHasSecurity(UserProfile.UserId, "意见反馈", "Delete").ToString().ToLower();
            hidIsEdit.Value = uBO.IsHasSecurity(UserProfile.UserId, "意见反馈", "Edit").ToString().ToLower();

            StringBuilder sb = new StringBuilder();
            sb.Append("var hidSuggestionId='").Append(hidSuggestionId.ClientID).Append("';\n");
            sb.Append("var hidIsDelete='").Append(hidIsDelete.ClientID).Append("';\n");
            sb.Append("var hidIsEdit='").Append(hidIsEdit.ClientID).Append("';\n");
            Utilities.RegisterJs(Page, "JSCommonVar_SuggestionCreateEdit", sb.ToString());
        }
    }
}