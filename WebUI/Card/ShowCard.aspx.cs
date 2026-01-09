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
    public partial class ShowCard : System.Web.UI.Page
    {
        public CardDataVO cVO = null;
        public int CardID = 0;
        public ViewBag ViewBag;
        protected void Page_Load(object sender, EventArgs e)
        {
            CardID = Convert.ToInt32(string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["CardID"]) ? "0" : HttpContext.Current.Request.QueryString["CardID"]);
            CardBO cBO = new CardBO(new CustomerProfile());
            cVO = cBO.FindCardById(CardID);
            GetWX();
        }
        /// <summary>
        /// 获取微信分享接口参数
        /// </summary>
        public void GetWX()
        {
            WX_JSSDK jssdk = new WX_JSSDK();
            ViewBag = jssdk.getSignPackage();
        }
    }
}