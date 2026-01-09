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
    public partial class CardIndex : System.Web.UI.Page
    {
        public string Token = "";
        public int CustomerId = 0;
        public CardDataVO cVO = null;
        public ViewBag ViewBag;
        protected void Page_Load(object sender, EventArgs e)
        {
            wxlogin.login();
            Token = wxlogin.Token;
            CustomerId = wxlogin.CustomerId;
            CardBO cBO = new CardBO(new CustomerProfile());
            List<CardDataVO> uVO = cBO.FindCardByCustomerId(CustomerId);
            if (uVO.Count > 0)
            {
                cVO = uVO[0];
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