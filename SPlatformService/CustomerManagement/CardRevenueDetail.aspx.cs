using CoreFramework.VO;
using SPLibrary.CoreFramework.BO;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.CustomerManagement.VO;
using System;
using System.Collections.Generic;
using System.Text;


namespace SPlatformService.CustomerManagement
{
    public partial class CardRevenueDetail : BasePage
    {

        public decimal PayOutMoneyed { set; get; }
        public decimal TotalMoney { set; get; }
        public decimal ResidueMoney { set; get; }
        public decimal CurrentMoney { set; get; }
        public decimal UnlawfulMoney { set; get; }
        public decimal Cashing { set; get; }
        public bool isLegitimate { set; get; }
        public int AppType { set; get; }
        protected void Page_Load(object sender, EventArgs e)
        {
            (this.Master as Shared.MasterPage).MenuText = "会员提现";
            (this.Master as Shared.MasterPage).PageNameText = "收入明细";


            int systemPartID = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["CustomerId"]) ? "0" : Request.QueryString["CustomerId"]);
            this.CustomerId.Value = systemPartID.ToString();
            StringBuilder sb = new StringBuilder();
            sb.Append("var CustomerId='").Append(CustomerId.ClientID).Append("';\n");
            Utilities.RegisterJs(Page, "JSCommonVar_CardMessageCreateEdit", sb.ToString());

            AppType = UserProfile.AppType;
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);
            var customerId = systemPartID;
            //获取提现记录
            List<CardPayOutVO> uVO = cBO.FindPayOutByCustomerId(customerId);

            for (int i=0;i< uVO.Count;i++) {
                if (uVO[i].PayOutStatus==1) {
                    PayOutMoneyed += uVO[i].PayOutCost;
                }
                if (uVO[i].PayOutStatus == 0)
                {
                    Cashing += uVO[i].PayOutCost;
                }
            }
            TotalMoney = cBO.FindBalanceByCustomerId(customerId);

            ResidueMoney = TotalMoney - PayOutMoneyed;

            CurrentMoney = cBO.FindCardBalanceByCustomerId(customerId);

            isLegitimate = cBO.isLegitimate(customerId);

            //decimal FrozenBalance = cBO.FindFrozenBalanceByCustomerId(customerId);
            //decimal CardBalance = cBO.FindCardBalanceByCustomerId(customerId);
            // CurrentMoney = FrozenBalance + CardBalance- cashing;//当前帐户余额
            // UnlawfulMoney = CurrentMoney - TotalMoney;//不明收入

        }
    }
}