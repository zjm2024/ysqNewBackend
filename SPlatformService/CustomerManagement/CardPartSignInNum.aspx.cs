using CoreFramework.VO;
using SPLibrary.CoreFramework.BO;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.CustomerManagement.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SPlatformService.CustomerManagement
{
    public partial class CardPartSignInNum : BasePage
    {
        public int PnumberOfPeople=0;
        public int SignupOfPeople = 0;
        public decimal PEarning=0;
        public List<CostItem> PCostList = new List<CostItem>();
        protected void Page_Load(object sender, EventArgs e)
        {
            (this.Master as Shared.MasterPage).MenuText = "活动信息";
            (this.Master as Shared.MasterPage).PageNameText = "报名信息";

            int systemPartID = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["PartID"]) ? "0" : Request.QueryString["PartID"]);
            this.PartID.Value = systemPartID.ToString();


            CardBO cBO = new CardBO(new CustomerProfile());
            List<CardPartySignUpViewVO> cVO = cBO.FindSignUpViewByPartyID(systemPartID);
            SignupOfPeople = cBO.FindCardPartSignInNumTotalCount("PartyID = " + systemPartID + " and isAutoAdd=0 and PartySignUpID > 0 and SignUpStatus<>2");
            if (cVO != null)
            {
                if (cVO.Count > 0)
                {
                    cVO.Reverse();
                    int numberOfPeople = cVO.Count; //总人数
                    decimal Earning = 0; //总金额
                    List<CostItem> CostList = new List<CostItem>();

                    for (int i = 0; i < cVO.Count; i++)
                    {
                        if (cVO[i].OrderStatus == 1 || cVO[i].PartyOrderID == 0)
                        {
                            string CostName = cVO[i].CostName;

                            if (CostName == "")
                            {
                                CostName = "免费";
                            }

                            if (CostList.Exists(p => p.CostName == CostName))
                            {
                                CostList.FirstOrDefault(p => p.CostName == CostName).Cost += cVO[i].Cost;
                                CostList.FirstOrDefault(p => p.CostName == CostName).People += 1;
                            }
                            else
                            {
                                CostItem CostItem = new CostItem();
                                CostItem.CostName = CostName;
                                CostItem.Cost = cVO[i].Cost;
                                CostItem.People = 1;
                                CostList.Add(CostItem);
                            }
                            Earning += cVO[i].Cost;
                        }
                    }
                    PnumberOfPeople = numberOfPeople;
                    PEarning = Earning;
                    PCostList = CostList;
                }
            }else
            {
                cVO = new List<CardPartySignUpViewVO>();
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("var PartID='").Append(PartID.ClientID).Append("';\n");
            Utilities.RegisterJs(Page, "JSCommonVar_CardMessageCreateEdit", sb.ToString());
        }
    }
}