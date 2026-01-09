using CoreFramework.VO;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.CustomerManagement.VO;
using SPLibrary.WebConfigInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebUI.Card
{
    public partial class MyCenter : System.Web.UI.Page
    {
        public string Token = "";
        public int CustomerId = 0;
        public CardDataVO cVO = null;
        public CustomerViewVO uVO = new CustomerViewVO();
        public decimal FrozenBalance = 0;
        public decimal CardBalance = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            wxlogin.login();
            Token = wxlogin.Token;
            CustomerId = wxlogin.CustomerId;
            CardBO cBO = new CardBO(new CustomerProfile());
            List<CardDataVO> lVO = cBO.FindCardByCustomerId(CustomerId);
            if (lVO.Count > 0)
            {
                cVO = lVO[0];
            }

            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            uVO = uBO.FindById(CustomerId);

            //先查询并结算冻结金额
            FrozenBalance = cBO.FindFrozenBalanceByCustomerId(CustomerId).SumBalance;
            CardBalance = cBO.FindCardBalanceByCustomerId(CustomerId);

        }
    }
}