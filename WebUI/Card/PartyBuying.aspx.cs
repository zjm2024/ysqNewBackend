
using CoreFramework.VO;
using SPlatformService.Controllers;
using SPlatformService.TokenMange;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.CustomerManagement.VO;
using SPLibrary.WebConfigInfo;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebUI.Card
{
    public partial class PartyBuying : System.Web.UI.Page
    {
        public string Token = "";
        public int CustomerId = 0;
        public int PartyID = 0;
        public List<CardPartySignUpFormVO> fVO = new List<CardPartySignUpFormVO>();
        public List<CardPartyCostVO> cVO = new List<CardPartyCostVO>();
        public CardPartyViewVO PartyViewVO = new CardPartyViewVO();
        public int InviterCID = 0;
        public ViewBag ViewBag;
        protected void Page_Load(object sender, EventArgs e)
        {
            wxlogin.login();
            PartyID = Convert.ToInt32(string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["PartyID"]) ? "0" : HttpContext.Current.Request.QueryString["PartyID"]);
            InviterCID = Convert.ToInt32(string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["InviterCID"]) ? "0" : HttpContext.Current.Request.QueryString["InviterCID"]);
            Token = wxlogin.Token;
            CustomerId = wxlogin.CustomerId;

            CardBO cBO = new CardBO(new CustomerProfile());
            fVO = cBO.FindSignUpFormByPartyID(PartyID);

            List<CardDataVO> dVO = cBO.FindCardByCustomerId(CustomerId);

            if (dVO.Count > 0)
            {
                for(int i=0;i< fVO.Count; i++)
                {
                    if (fVO[i].Name == "姓名") fVO[i].value = dVO[0].Name;
                    if (fVO[i].Name == "手机") fVO[i].value = dVO[0].Phone;
                    if (fVO[i].Name == "工作单位") fVO[i].value = dVO[0].CorporateName;
                    if (fVO[i].Name == "单位地址") fVO[i].value = dVO[0].Address;
                    if (fVO[i].Name == "职位") fVO[i].value = dVO[0].Position;
                    if (fVO[i].Name == "微信") fVO[i].value = dVO[0].WeChat;
                }
            }


            cVO = cBO.FindCostByPartyID(PartyID);
            PartyViewVO = cBO.FindPartyViewById(PartyID);
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