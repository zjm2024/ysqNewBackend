using CoreFramework.VO;
using SPLibrary.CoreFramework.BO;
using SPLibrary.CoreFramework.WebConfigInfo;
using SPLibrary.CustomerManagement.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebUI.Common;

namespace WebUI.CustomerManagement
{
    public partial class CustomerPayOutCreateEidt : CustomerBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            (this.Master as Shared.MasterPage).MenuText = "会员提现";
            bindBankList();

        }

        private void bindBankList()
        {
            drpBankList.Items.Clear();
            drpBankList.Items.Add(new ListItem("新建银行账户", "-1"));
            List<BankAccountVO> voList = SiteCommon.GetBankListByCustomerId(CustomerProfile.CustomerId, Token);

            for (int i = 0; i < voList.Count; i++)
            {
                BankAccountVO pVO = voList[i];
                drpBankList.Items.Add(new ListItem(pVO.BankName+"-"+pVO.BankAccount, pVO.BankAccountId.ToString()));            
            }
        }
        public CustomerProfile CustomerProfile
        {
            get { return new CustomerPrincipal().CustomerProfile; }
        }
        public string Token
        {
            get
            {
                if (HttpContext.Current == null || HttpContext.Current.Session["#Session#TOKEN"] == null)
                {
                    return null;

                }
                else
                {
                    return HttpContext.Current.Session["#Session#TOKEN"].ToString();
                }
            }
        }
        protected void drpBankList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}