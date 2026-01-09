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
    public partial class QRCode : System.Web.UI.Page
    {
        public CardPartyViewVO PartyViewVO { set; get; }
        public ViewBag ViewBag;
        public int wxtype;
        protected void Page_Load(object sender, EventArgs e)
        {
            int PartyID = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["PartyID"]) ? "0" : Request.QueryString["PartyID"]);
            wxtype = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["wxtype"]) ? "1" : Request.QueryString["wxtype"]);
            
            if (PartyID > 0)
            {
                CardBO cBO = new CardBO(new CustomerProfile());
                CardPartyViewVO cVO = cBO.FindPartyViewById(PartyID);
                if (cVO != null)
                {
                    PartyViewVO = cVO;
                }
                else
                {
                    PartyViewVO = new CardPartyViewVO();
                }
            }
            else
            {
                PartyViewVO = new CardPartyViewVO();
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