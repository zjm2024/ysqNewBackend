using CoreFramework.VO;
using SPLibrary.BusinessCardManagement.BO;
using SPLibrary.BusinessCardManagement.VO;
using SPLibrary.CoreFramework.Logging.BO;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.CustomerManagement.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace BusinessCard.GenerateIMG
{
    public partial class BusinessCardIMG2QR : System.Web.UI.Page
    {
        public int IDType;//1:名片，2:名片组，3:活动,5：签到
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
            LogBO logger = new LogBO(this.GetType());
            Int64 ID = Convert.ToInt64(string.IsNullOrEmpty(Request.QueryString["ID"]) ? "0" : Request.QueryString["ID"]);
            Int64 ImgName = Convert.ToInt64(string.IsNullOrEmpty(Request.QueryString["ImgName"]) ? "0" : Request.QueryString["ImgName"]);
            Int64 CustomerId = Convert.ToInt64(string.IsNullOrEmpty(Request.QueryString["CustomerId"]) ? "0" : Request.QueryString["CustomerId"]);
            IDType = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["IDType"]) ? "0" : Request.QueryString["IDType"]);
            AppType = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["AppType"]) ? "0" : Request.QueryString["AppType"]);
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);
            BusinessCardBO bscBO = new BusinessCardBO(new CustomerProfile(),AppType);
            //logger.Info("传入的IDType值：" + Request.QueryString["IDType"]);  // 新增日志
            //logger.Info("IDType" + IDType);
           if (IDType == 5)
            {
                //获取签到表详情
                CardRegistertableVO cqVO = bscBO.FindCardRegistertableByQuestionnaireID(Convert.ToInt32(ID));
                //WrapQR1 = "WrapQR1";
                //WrapQR2 = "WrapQR1";
                WrapQR3 = "WrapQR3";
                Title = cqVO.Title;
                cqVO.QRImg = bscBO.GetCardRegistertableSignupQR(cqVO.QuestionnaireID, CustomerId,AppType);
                CardImg = cqVO.QRImg.Replace("https", "http");
                bottext = "扫一扫上面的二维码图案";

                List<CardDataVO> cvoList = cBO.FindCardByCustomerId(CustomerId);
                if (cvoList.Count > 0)
                {
                    bottext = cvoList[0].Name + "邀请您扫码填表";
                    Headimg = cvoList[0].Headimg.Replace("https", "http");
                }
            }
            
        }
    }
}