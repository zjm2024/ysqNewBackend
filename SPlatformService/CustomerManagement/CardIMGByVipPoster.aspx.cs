using CoreFramework.VO;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.CustomerManagement.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SPlatformService.CustomerManagement
{
    public partial class CardIMGByVipPoster : System.Web.UI.Page
    {
        public string QRimg { set; get; }
        public string Headimg { set; get; }
        public string Name { set; get; }
        protected void Page_Load(object sender, EventArgs e)
        {
            QRimg = string.IsNullOrEmpty(Request.QueryString["QRimg"]) ? "" : Server.UrlDecode(Request.QueryString["QRimg"]);
            QRimg = QRimg.Replace("https", "http");
            int customerId = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["customerId"]) ? "0" : Server.UrlDecode(Request.QueryString["customerId"]));

            CardBO cBO = new CardBO(new CustomerProfile());

            List<CardDataVO> lVO = cBO.FindCardByCustomerId(customerId);
            if (lVO.Count > 0)
            {
                Headimg = lVO[0].Headimg;
                Name = lVO[0].Name;
            }
            else
            {
                CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
                CustomerVO CustomerVO = CustomerBO.FindCustomenById(customerId);
                Headimg = CustomerVO.HeaderLogo;
            }
        }
    }
}