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
    public partial class CardQRCode : System.Web.UI.Page
    {
        public CardDataVO CardDataVO { set; get; }
        public ViewBag ViewBag;
        public int wxtype;
        protected void Page_Load(object sender, EventArgs e)
        {
            int CardID = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["CardID"]) ? "0" : Request.QueryString["CardID"]);
            wxtype = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["wxtype"]) ? "1" : Request.QueryString["wxtype"]);
            
            if (CardID > 0)
            {
                CardBO cBO = new CardBO(new CustomerProfile());
                CardDataVO cVO = cBO.FindCardById(CardID);
                if (cVO != null)
                {
                    CardDataVO = cVO;
                }
                else
                {
                    CardDataVO = new CardDataVO();
                }
            }
            else
            {
                CardDataVO = new CardDataVO();
            }
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