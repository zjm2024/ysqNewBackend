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
    public partial class CardQuestionnaireSignupBrowse : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "乐聊名片";
                (this.Master as Shared.MasterPage).PageNameText = "万能表格";
            }

            base.ValidPageRight("万能表格", "Read");

            int systemMessageId = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["QuestionnaireID"]) ? "0" : Request.QueryString["QuestionnaireID"]);
            this.QuestionnaireID.Value = systemMessageId.ToString();

            StringBuilder sb = new StringBuilder();
            sb.Append("var hidQuestionnaireID='").Append(QuestionnaireID.ClientID).Append("';\n");
            Utilities.RegisterJs(Page, "JSCommonVar_CardBusinessCardCreateEdit", sb.ToString());
        }
    }
}