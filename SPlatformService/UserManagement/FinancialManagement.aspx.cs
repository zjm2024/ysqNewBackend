using CoreFramework.VO;
using SPLibrary.CoreFramework.BO;
using SPLibrary.UserManagement.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SPlatformService.UserManagement
{
    public partial class FinancialManagement : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "财务管理";
                (this.Master as Shared.MasterPage).PageNameText = "财务结算";
            }
            base.ValidPageRight("财务结算", "Read");

            bindData();
        }
        private void bindData()
        {
            SystemBO _sbo = new SystemBO(new UserProfile());
            decimal totalCommission = _sbo.PlatformTotalCommission();

            decimal totalBalance = _sbo.GetTotalBalance();

            decimal total = totalCommission + totalBalance;

            txtTotal.Text = total.ToString();
            txtTotalBalance.Text = totalBalance.ToString();
            txtProjectCommission.Text = totalCommission.ToString();


        }

    }
}