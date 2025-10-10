using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.CustomerManagement.VO;
using CoreFramework.VO;

namespace SPlatformService.CustomerManagement
{
    public partial class CardIMG2QR : System.Web.UI.Page
    {
        public int IDType;//1:名片，2:名片组，3:活动
        public string WrapQR1;
        public string WrapQR2;
        public string WrapQR3;
        public string Headimg;
        public string Name;
        public string Position;
        public string CardImg;
        public string Title;
        public string bottext;
        public int AppType;
        protected void Page_Load(object sender, EventArgs e)
        {
            Int64 ID = Convert.ToInt64(string.IsNullOrEmpty(Request.QueryString["ID"]) ? "0" : Request.QueryString["ID"]);
            Int64 ImgName = Convert.ToInt64(string.IsNullOrEmpty(Request.QueryString["ImgName"]) ? "0" : Request.QueryString["ImgName"]);
            Int64 CustomerId = Convert.ToInt64(string.IsNullOrEmpty(Request.QueryString["CustomerId"]) ? "0" : Request.QueryString["CustomerId"]);
            IDType = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["IDType"]) ? "0" : Request.QueryString["IDType"]);
            AppType = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["AppType"]) ? "0" : Request.QueryString["AppType"]);
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);


            if (IDType == 1)
            {
                CardDataVO uVO = cBO.FindCardById(ID);
                WrapQR1 = "";
                WrapQR2 = "WrapQR1";
                WrapQR3 = "WrapQR1";
                Headimg = uVO.Headimg.Replace("https", "http");
                Name = uVO.Name;
                Position = uVO.Position;
                if (uVO.CardImg == "" || uVO.CardImg == null)
                {
                    uVO.CardImg = cBO.GetCardQR(uVO.CardID);
                }
                CardImg = uVO.CardImg.Replace("https", "http");

            }
            else if (IDType == 2)
            {
                CardGroupVO cgVO = cBO.FindCardGroupById(ID);
                WrapQR1 = "WrapQR1";
                WrapQR2 = "";
                WrapQR3 = "WrapQR1";
                Title = cgVO.GroupName;
                if (cgVO.CardImg == "" || cgVO.CardImg == null)
                {
                    cgVO.CardImg = cBO.GetCardGroupQR(cgVO.GroupID);
                }
                CardImg = cgVO.CardImg.Replace("https", "http");
            }
            else if(IDType == 4&& CustomerId>0)
            {
                CardPartyVO cpVO = cBO.FindPartyById(ID);
                List<CardDataVO> cvoList = cBO.FindCardByCustomerId(CustomerId);

                CustomerBO cuBO = new CustomerBO(new CustomerProfile());
                CustomerVO cVO = cuBO.FindCustomenById(CustomerId);

                if (cpVO.MainImg == "" || cpVO.MainImg == null)
                {
                    cpVO.MainImg = "http://www.zhongxiaole.net/SPManager/Style/images/wxcard/7.jpg";
                }

                if (cpVO.Type == 3)
                {
                    List<CardPartyCostVO> FirstPrize = cBO.FindCostByFirstPrize(cpVO.PartyID);
                    if (FirstPrize.Count > 0)
                    {
                        cpVO.MainImg = FirstPrize[0].Image;
                    }
                }


                WrapQR1 = "WrapQR1";
                WrapQR2 = "WrapQR1";
                WrapQR3 = "";
                Headimg = cpVO.MainImg.Replace("https", "http");
                Title = cpVO.Title;

                if (cvoList.Count > 0)
                {
                    bottext = cvoList[0].Name + "邀请您微信扫码了解详情或报名";
                }
                else
                {
                    if (cVO != null)
                    {
                        bottext = cVO.CustomerName + "邀请您微信扫码了解详情或报名";
                    }
                    else
                    {
                        bottext = "扫一扫上面的二维码图案，参加活动";
                    }
                }
                
                CardImg = "http://www.zhongxiaole.net/SPManager/UploadFolder/CardPartyQRTemporaryFile/" + Convert.ToString(ImgName) + ".png";
            }
            else if (IDType == 5)
            {
                CardQuestionnaireVO cqVO = cBO.FindCardQuestionnaireByQuestionnaireID(Convert.ToInt32(ID));
                WrapQR1 = "WrapQR1";
                WrapQR2 = "WrapQR1";
                WrapQR3 = "";
                Title = cqVO.Title;
                cqVO.QRImg = cBO.GetQuestionnaireSignupQR(cqVO.QuestionnaireID, CustomerId);
                CardImg = cqVO.QRImg.Replace("https", "http");
                bottext = "扫一扫上面的二维码图案";

                List<CardDataVO> cvoList = cBO.FindCardByCustomerId(CustomerId);
                if (cvoList.Count > 0)
                {
                    bottext = cvoList[0].Name + "邀请您扫码填表";
                    Headimg = cvoList[0].Headimg.Replace("https", "http");
                }
            }
            else if (IDType == 6)
            {
                CardSoftArticleVO cqVO = cBO.FindSoftArticleById(ID);
                WrapQR1 = "WrapQR1";
                WrapQR2 = "WrapQR1";
                WrapQR3 = "";
                Title = cqVO.Title;
                if(cqVO.QRImg=="")
                    cqVO.QRImg = cBO.GetSoftArticleQR(cqVO.SoftArticleID);
                CardImg = cqVO.QRImg.Replace("https", "http");
                bottext = "扫一扫上面的二维码图案";
                List<CardDataVO> cvoList = cBO.FindCardByCustomerId(CustomerId);
                if (cvoList.Count > 0)
                {
                    Headimg = cvoList[0].Headimg.Replace("https", "http");
                }
            }

            else if (IDType == 3)
            {
                CardPartyVO cpVO = cBO.FindPartyById(ID);
                if (cpVO.MainImg==""|| cpVO.MainImg==null)
                {
                    cpVO.MainImg = "http://www.zhongxiaole.net/SPManager/Style/images/wxcard/7.jpg";
                 }
                if (cpVO.Type == 3)
                {
                    List<CardPartyCostVO> FirstPrize = cBO.FindCostByFirstPrize(cpVO.PartyID);
                    if (FirstPrize.Count > 0)
                    {
                        cpVO.MainImg = FirstPrize[0].Image;
                    }
                }

                WrapQR1 = "WrapQR1";
                WrapQR2 = "WrapQR1";
                WrapQR3 = "";
                Headimg = cpVO.MainImg.Replace("https", "http");
                Title = cpVO.Title;
                if (cpVO.QRCodeImg == "")
                {
                    cpVO.QRCodeImg = cBO.GetCardPartyQR(cpVO.PartyID);
                }
                CardImg = cpVO.QRCodeImg.Replace("https", "http");
                bottext = "扫一扫上面的二维码图案，参加活动";
            }

            else if (IDType == 7)
            {
                CardPartyVO cpVO = cBO.FindPartyById(ID);
                if (cpVO.MainImg == "" || cpVO.MainImg == null)
                {
                    cpVO.MainImg = "http://www.zhongxiaole.net/SPManager/Style/images/wxcard/7.jpg";
                }
                if (cpVO.Type == 3)
                {
                    List<CardPartyCostVO> FirstPrize = cBO.FindCostByFirstPrize(cpVO.PartyID);
                    if (FirstPrize.Count > 0)
                    {
                        cpVO.MainImg = FirstPrize[0].Image;
                    }
                }

                WrapQR1 = "WrapQR1";
                WrapQR2 = "WrapQR1";
                WrapQR3 = "";
                Headimg = cpVO.MainImg.Replace("https", "http");
                Title = cpVO.Title;
                cpVO.QRCodeImg = "https://www.zhongxiaole.net/SPManager/UploadFolder/CardPartyQRTemporaryFile/" + cBO.GetCardPartyQRByMessage(cpVO.PartyID, 179109);
                CardImg = cpVO.QRCodeImg.Replace("https", "http");
                bottext = "长按二维码，报名活动";
            }

        }
    }
}