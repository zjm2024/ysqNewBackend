using CoreFramework.VO;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.CustomerManagement.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebUI.Card
{
    public partial class CardShortH5 : System.Web.UI.Page
    {
        public int AppType;
        public string Html;
        protected void Page_Load(object sender, EventArgs e)
        {
            Int64 CardID = Convert.ToInt64(string.IsNullOrEmpty(Request.QueryString["CardID"]) ? "0" : Request.QueryString["CardID"]);
            AppType = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["AppType"]) ? "1" : Request.QueryString["AppType"]);
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);
            CardDataVO uVO = cBO.FindCardById(CardID);

            Html = cBO.getCardHtml(uVO,false);
        }
    }
}