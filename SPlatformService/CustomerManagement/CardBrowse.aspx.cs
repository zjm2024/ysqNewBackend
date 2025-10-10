using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SPLibrary.CoreFramework.BO;
using SPLibrary.UserManagement.BO;
using SPLibrary.CustomerManagement.BO;
using CoreFramework.VO;
using SPLibrary.CoreFramework.WebConfigInfo;

namespace SPlatformService.CustomerManagement
{
    public partial class CardBrowse : BasePage
    {
        public int CardNum = 0;
        public int CardCustomerNum = 0;
        public int VipNum = 0;//现有vip
        public int VipNum2 = 0;//已过期vip
        public int VipNum3 = 0;//历史vip总数
        public int VipNum4 = 0;//兑换码兑换vip总次数
        public int QuestionnaireNum = 0;
        public int PartyNum = 0;
        public int DummyNum = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "乐聊名片";
                (this.Master as Shared.MasterPage).PageNameText = "名片信息";
            }

            int AppType = UserProfile.AppType;
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);
            CustomerBO uBO = new CustomerBO(new CustomerProfile());

            CardNum = cBO.FindCardTotalCount("isDummy=0");
            DummyNum = cBO.FindCardTotalCount("isDummy=1");
            CardCustomerNum = cBO.FindCardDataViewTotalCount("isDummy=0");
            DateTime dt = DateTime.Now;
            VipNum = uBO.GetCustomerCount("isVip=1 and AppType="+ AppType+ " and DATE_FORMAT(ExpirationAt,'%y-%m-%d')>DATE_FORMAT(now(),'%y-%m-%d')");
            VipNum2 = uBO.GetCustomerCount("AppType=" + AppType + " and  (DATE_FORMAT(ExpirationAt,'%y-%m-%d')<=DATE_FORMAT(now(),'%y-%m-%d') or (DATE_FORMAT(ExpirationAt,'%y-%m-%d')>DATE_FORMAT(now(),'%y-%m-%d') and isVip=0))");
            VipNum3 = uBO.GetCustomerCount("AppType=" + AppType + " and  DATE_FORMAT(ExpirationAt,'%y-%m-%d')>DATE_FORMAT('2018-01-01','%y-%m-%d')");
            VipNum4 = cBO.FindExchangeCodeTotalCount("Status=1");
            QuestionnaireNum = cBO.FindCardTotalCount("isQuestionnaire=1");
            PartyNum = cBO.FindCardTotalCount("isParty=1");

            base.ValidPageRight("名片信息", "Read");
            int systemCustomerId = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["CustomerId"]) ? "0" : Request.QueryString["CustomerId"]);
            this.CustomerId.Value = systemCustomerId.ToString();

            StringBuilder sb = new StringBuilder();
            sb.Append("var hidCustomerId='").Append(CustomerId.ClientID).Append("';\n");
            Utilities.RegisterJs(Page, "JSCommonVar_CardBusinessCardCreateEdit", sb.ToString());
        }
    }
}