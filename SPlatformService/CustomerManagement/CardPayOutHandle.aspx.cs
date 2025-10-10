using CoreFramework.VO;
using SPLibrary.CoreFramework.BO;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.CustomerManagement.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SPlatformService.CustomerManagement
{
    public partial class CardPayOutHandle : BasePage
    {
        public bool isLegitimate { set; get; }
        public int AppType { set; get; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "会员提现";
                (this.Master as Shared.MasterPage).PageNameText = "会员提现";
            }

            base.ValidPageRight("会员提现", "Read");
            int PayOutHistoryId = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["PayOutHistoryId"]) ? "0" : Request.QueryString["PayOutHistoryId"]);
            this.hidPayOutHistoryId.Value = PayOutHistoryId.ToString();

            AppType = UserProfile.AppType;
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);

            CardPayOutVO cVO = cBO.FindPayOutViewById(PayOutHistoryId);
            var customerId = cVO.CustomerId;
            isLegitimate = cBO.isLegitimate(customerId);

            StringBuilder sb = new StringBuilder();
            sb.Append("var hidPayOutHistoryId='").Append(hidPayOutHistoryId.ClientID).Append("';\n");
            Utilities.RegisterJs(Page, "JSCommonVar_CustomerPayOutHandle", sb.ToString());
        }
    }
}