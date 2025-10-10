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
    public partial class QRIMG : System.Web.UI.Page
    {
        public PersonalVO PersonalVO { set; get; }
        protected void Page_Load(object sender, EventArgs e)
        {
            int PersonalID = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["PersonalID"]) ? "0" : Request.QueryString["PersonalID"]);
            int BusinessID = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["BusinessID"]) ? "0" : Request.QueryString["BusinessID"]);
            int AppType = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["AppType"]) ? "0" : Request.QueryString["AppType"]);
            string Code = string.IsNullOrEmpty(Request.QueryString["Code"]) ? "" : Server.UrlDecode(Request.QueryString["Code"]);
            if (PersonalID > 0)
            {
                BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
                PersonalVO = cBO.FindPersonalById(PersonalID);
                if (PersonalVO == null)
                {
                    PersonalVO = new PersonalVO();
                }
                PersonalVO.QRimg = cBO.GetQRImg(PersonalID, BusinessID, Code, AppType).Replace("https", "http");
                PersonalVO.Headimg= PersonalVO.Headimg.Replace("https", "http");
            }
        }
    }
}