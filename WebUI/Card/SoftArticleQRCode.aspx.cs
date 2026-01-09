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
    public partial class SoftArticleQRCode : System.Web.UI.Page
    {
        public CardSoftArticleVO CardSoftArticleVO { set; get; }
        public ViewBag ViewBag;
        public int wxtype;
        protected void Page_Load(object sender, EventArgs e)
        {
            int SoftArticleID = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["SoftArticleID"]) ? "0" : Request.QueryString["SoftArticleID"]);
            wxtype = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["wxtype"]) ? "1" : Request.QueryString["wxtype"]);
            
            if (SoftArticleID > 0)
            {
                CardBO cBO = new CardBO(new CustomerProfile());
                CardSoftArticleVO cVO = cBO.FindSoftArticleById(SoftArticleID);
                if (cVO != null)
                {
                    if (cVO.QRImg == "")
                    {
                        cVO.QRImg = cBO.GetSoftArticleQR(cVO.SoftArticleID);
                    }
                    CardSoftArticleVO = cVO;
                }
                else
                {
                    CardSoftArticleVO = new CardSoftArticleVO();
                }
            }
            else
            {
                CardSoftArticleVO = new CardSoftArticleVO();
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