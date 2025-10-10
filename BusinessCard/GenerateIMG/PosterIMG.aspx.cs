using System;
using SPLibrary.BusinessCardManagement.BO;
using SPLibrary.BusinessCardManagement.VO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreFramework.VO;

namespace BusinessCard.GenerateIMG
{
    public partial class PosterIMG : System.Web.UI.Page
    {
        public PersonalViewVO PersonalVO { set; get; }
        public string Posterback { set; get; }
        public string BusinessName { set; get; }
        protected void Page_Load(object sender, EventArgs e)
        {
            int PersonalID = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["PersonalID"]) ? "0" : Request.QueryString["PersonalID"]);
            int BusinessID = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["BusinessID"]) ? "0" : Request.QueryString["BusinessID"]);
            int AppType = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["AppType"]) ? "0" : Request.QueryString["AppType"]);
            Posterback = string.IsNullOrEmpty(Request.QueryString["Posterback"]) ? "" : Server.UrlDecode(Request.QueryString["Posterback"]);
            string Code = string.IsNullOrEmpty(Request.QueryString["Code"]) ? "" : Server.UrlDecode(Request.QueryString["Code"]);

            if (Posterback == "")
            {
                Posterback = "http://www.zhongxiaole.net/BusinessCard/images/Poster/1.jpg";
            }
            Posterback= Posterback.Replace("https", "http");


            if (PersonalID > 0)
            {
                BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
                PersonalVO = cBO.FindPersonalViewById(PersonalID);
                if (PersonalVO == null)
                {
                    PersonalVO = new PersonalViewVO();
                }else
                {
                    PersonalVO.Headimg = PersonalVO.Headimg.Replace("https", "http");
                    PersonalVO.QRimg= PersonalVO.QRimg.Replace("https", "http");
                }
                BusinessName = PersonalVO.BusinessName;
                PersonalVO.QRimg = cBO.GetQRImgByHeadimg(PersonalVO.PersonalID, AppType, BusinessID, Code).Replace("https", "http");
                if (BusinessID>0&& BusinessID!= PersonalVO.BusinessID)
                {
                    BusinessName = cBO.FindBusinessCardById(BusinessID).BusinessName;
                }
            }
            else
            {
                PersonalVO = new PersonalViewVO();
            }
        }
    }
}