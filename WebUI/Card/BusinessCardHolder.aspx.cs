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
    public partial class BusinessCardHolder : System.Web.UI.Page
    {
        public string Token = "";
        public int CustomerId = 0;
        public List<CardDataVO> list = new List<CardDataVO>();
        public int count = 0;
        public Paging pageInfo = new Paging();
        protected void Page_Load(object sender, EventArgs e)
        {
            wxlogin.login();
            Token = wxlogin.Token;
            CustomerId = wxlogin.CustomerId;
            CardBO cBO = new CardBO(new CustomerProfile());
            string conditionStr = "t_CustomerId = " + CustomerId;
            pageInfo.PageIndex = Convert.ToInt32(string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["PageIndex"]) ? "1" : HttpContext.Current.Request.QueryString["PageIndex"]);
            pageInfo.PageCount = 20;
            list = cBO.FindCardCollectionAllByPageIndex(conditionStr, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);
            count = cBO.FindCardCollectionAllCount(conditionStr);
        }
    }
}