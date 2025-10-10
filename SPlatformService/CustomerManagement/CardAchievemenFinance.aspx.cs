using System;
using System.Text;
using SPLibrary.CoreFramework.BO;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.CustomerManagement.VO;
using SPLibrary.ProjectManagement.BO;
using SPLibrary.RequireManagement.BO;
using CoreFramework.VO;
using WebUI.Common;
using System.Collections.Generic;

namespace SPlatformService.CustomerManagement
{
    public partial class CardAchievemenFinance : BasePage
    {
        public String MONTH{get;set;}
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "会员管理";
                //this.Master.PageNameText = "会员管理";
            }

            base.ValidPageRight("会员管理", "Read");
            int customerId = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["originCustomerId"]) ? "0" : Request.QueryString["originCustomerId"]);
            MONTH = string.IsNullOrEmpty(Request.QueryString["MONTH"]) ? DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() : Request.QueryString["MONTH"];
            TextBox3.Text = MONTH;

            this.hidCustomerId.Value = customerId.ToString();
            this.HidMONTH.Value = MONTH.ToString();

            StringBuilder sb = new StringBuilder();
            sb.Append("var hidCustomerId='").Append(hidCustomerId.ClientID).Append("';\n");
            sb.Append("var HidMONTH='").Append(HidMONTH.ClientID).Append("';\n");
            Utilities.RegisterJs(Page, "JSCommonVar_CustomerFinance", sb.ToString());

            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            CustomerViewVO customerVO = SiteCommon.GetCustomerById(customerId);
            CustomerName.Text = customerVO.CustomerName;

            CardBO cBO = new CardBO(new CustomerProfile());

            List<CardDataVO> CardDataVO = cBO.FindCardByCustomerId(customerId);

            if (CardDataVO.Count > 0)
            {
                Name.Text = CardDataVO[0].Name;
                Phone.Text = CardDataVO[0].Phone;
            }

            //一级用户数
            TextBox1.Text = uBO.FindCustomerCount("originCustomerId=" + customerId + " and " + "CreatedAt like '" + MONTH + "%'").ToString();
            List<CustomerVO> cVO=uBO.FindListByParams("originCustomerId=" + customerId + " and " + "CreatedAt like '" + MONTH + "%'");
            int countNum = 0;
            for (int i=0;i< cVO.Count;i++) {
                List<CardDataVO> cardDataVO = cBO.FindCardByCustomerId(cVO[i].CustomerId);
                if (cardDataVO.Count > 0)
                {
                    if (cardDataVO[0].Name!=""&& cardDataVO[0].Phone != "" && cardDataVO[0].Position != "" && cardDataVO[0].CorporateName != "") {
                        countNum++;
                    }
                }
            }
            //一级用户合格数
            TextBox4.Text = countNum.ToString();



            //二级用户数
            int CustomerCount2 = 0;
            int countNum2 = 0;
            List<CustomerViewVO> customerVOList = uBO.FindByCondition("originCustomerId=" + customerId);

         

            for (int i=0;i< customerVOList.Count; i++)
            {
                CustomerCount2 += uBO.FindCustomerCount("originCustomerId=" + customerVOList[i].CustomerId + " and " + "CreatedAt like '" + MONTH + "%'");

                List<CustomerVO> cVO1 = uBO.FindListByParams("originCustomerId=" + customerVOList[i].CustomerId + " and " + "CreatedAt like '" + MONTH + "%'");
                for (int j = 0; j < cVO1.Count; j++)
                {
                    List<CardDataVO> cardDataVO = cBO.FindCardByCustomerId(cVO[j].CustomerId);
                    if (cardDataVO.Count > 0)
                    {
                        if (cardDataVO[0].Name != "" && cardDataVO[0].Phone != "" && cardDataVO[0].Position != "" && cardDataVO[0].CorporateName != "")
                        {
                            countNum2++;
                        }
                    }
                }
            }

            TextBox6.Text = (countNum * 1 + countNum2 * 0.1).ToString();

            TextBox5.Text = countNum2.ToString();

            TextBox2.Text = CustomerCount2.ToString();
        }
    }
}