using System;
using System.Text;
using SPLibrary.CoreFramework.BO;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.CustomerManagement.VO;
using SPLibrary.ProjectManagement.BO;
using SPLibrary.RequireManagement.BO;
using CoreFramework.VO;
using WebUI.Common;

namespace SPlatformService.CustomerManagement
{
    public partial class CustomerFinance : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "会员管理";
                //this.Master.PageNameText = "会员管理";
            }

            base.ValidPageRight("会员管理", "Read");
            int customerId = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["CustomerId"]) ? "0" : Request.QueryString["CustomerId"]);
            this.hidCustomerId.Value = customerId.ToString();

            StringBuilder sb = new StringBuilder();
            sb.Append("var hidCustomerId='").Append(hidCustomerId.ClientID).Append("';\n");
            Utilities.RegisterJs(Page, "JSCommonVar_CustomerFinance", sb.ToString());

            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            ProjectBO pBO = new ProjectBO(new CustomerProfile());
            RequireBO rBO = new RequireBO(new CustomerProfile());
            CustomerViewVO customerVO = SiteCommon.GetCustomerById(customerId);
            BalanceVO bVO = uBO.FindBalanceByCustomerId(customerVO.CustomerId);

            CustomerName.Text = customerVO.CustomerName;
            if (bVO != null)
            {
                Balance.Text = bVO.Balance.ToString();
            }
            else {
                Balance.Text = "0";
            }
            decimal CostSum = uBO.FindPayinHistoryTotalCostSum("CustomerId=" + customerVO.CustomerId+ " and PayInStatus=1");
            PayInBalance.Text = CostSum.ToString();

            decimal PayOutSum = uBO.FindPayoutHistoryTotalCostSum("CustomerId=" + customerVO.CustomerId + " and PayOutStatus=1");
            PayOutBalance.Text = PayOutSum.ToString();

            decimal PayOutSum0 = uBO.FindPayoutHistoryTotalCostSum("CustomerId=" + customerVO.CustomerId + " and PayOutStatus=0");
            PayOutBalance0.Text = PayOutSum0.ToString();

            decimal InComeSum= uBO.FindCommissionIncomeTotalCostSum("CustomerId=" + customerVO.CustomerId);
            InCome.Text = InComeSum.ToString();

            decimal CustomerHostingSum = pBO.FindCommissionDelegationViewTotalSumCommission("CustomerId=" + customerVO.CustomerId);
            CustomerHosting.Text = CustomerHostingSum.ToString();

            decimal RequireCommissionSum = rBO.FindRequireCommissionDelegationViewTotalSumCommission("CustomerId=" + customerVO.CustomerId);
            RequireCommission.Text = RequireCommissionSum.ToString() ;
        }
    }
}