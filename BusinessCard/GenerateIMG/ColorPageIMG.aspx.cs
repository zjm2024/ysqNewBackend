using CoreFramework.VO;
using SPLibrary.BusinessCardManagement.BO;
using SPLibrary.BusinessCardManagement.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BusinessCard.GenerateIMG
{
    public partial class ColorPageIMG : System.Web.UI.Page
    {
        public string QRimg { set; get; }
        public PersonalViewVO PersonalVO { set; get; }
        public BusinessCardVO BusinessCardVO { set; get; }
        protected void Page_Load(object sender, EventArgs e)
        {
            int PersonalID = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["PersonalID"]) ? "0" : Request.QueryString["PersonalID"]);
            int BusinessID = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["BusinessID"]) ? "0" : Request.QueryString["BusinessID"]);
            QRimg = string.IsNullOrEmpty(Request.QueryString["QRimg"]) ? "" : Server.UrlDecode(Request.QueryString["QRimg"]);
            QRimg= QRimg.Replace("https", "http");
            if (PersonalID > 0)
            {
                BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
                PersonalVO = cBO.FindPersonalViewById(PersonalID);
                if (PersonalVO == null)
                {
                    PersonalVO = new PersonalViewVO();
                }
                else
                {
                    PersonalVO.Headimg = PersonalVO.Headimg.Replace("https", "http");
                }
            }
            else
            {
                PersonalVO = new PersonalViewVO();
            }

            if (BusinessID > 0)
            {
                BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
                BusinessCardVO = cBO.FindBusinessCardById(BusinessID);
                if (BusinessCardVO == null)
                {
                    BusinessCardVO = new BusinessCardVO();
                }else
                {
                    BusinessCardVO.LogoImg= BusinessCardVO.LogoImg.Replace("https", "http");
                }
            }
            else
            {
                BusinessCardVO = new BusinessCardVO();
            }
        }
    }
}