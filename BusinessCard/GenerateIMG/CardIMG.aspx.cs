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
    public partial class CardIMG : System.Web.UI.Page
    {
        public PersonalVO Personal { set; get; }
        public string BusinessName { set; get; }
        public string LogoImg { set; get; }
        public ThemeVO Theme { set; get; }
        protected void Page_Load(object sender, EventArgs e)
        {
            int PersonalID = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["PersonalID"]) ? "0" : Request.QueryString["PersonalID"]);
            int BusinessID = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["BusinessID"]) ? "0" : Request.QueryString["BusinessID"]);
            Theme = new ThemeVO();
            Theme.setThemeVO();
            if (PersonalID > 0)
            {
                BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
                Personal = cBO.FindPersonalById(PersonalID);
                
                if (Personal == null)
                {
                    Personal = new PersonalVO();
                }else
                {
                    Personal.Headimg = Personal.Headimg.Replace("https", "http");
                    Personal.ReadNum = cBO.FindNumberOfVisitors("Personal", Personal.PersonalID);
                    Personal.todayReadNum = cBO.FindNumberOfVisitors("Personal", Personal.PersonalID, 1, 0);
                }
                BusinessName = "";
                if (BusinessID > 0 && BusinessID != Personal.BusinessID)
                {
                    BusinessCardVO BusinessCardVO = cBO.FindBusinessCardById(BusinessID);
                    if (BusinessCardVO != null)
                    {
                        BusinessName = BusinessCardVO.BusinessName;
                        LogoImg = BusinessCardVO.LogoImg;
                        if (BusinessCardVO.ThemeID > 0)
                        {
                            Theme = cBO.FindThemeById(BusinessCardVO.ThemeID);
                        }
                    }
                    
                }
                else if (Personal.BusinessID > 0)
                {
                    BusinessCardVO BusinessCardVO = cBO.FindBusinessCardById(Personal.BusinessID);
                    if (BusinessCardVO != null)
                    {
                        BusinessName = BusinessCardVO.BusinessName;
                        LogoImg = BusinessCardVO.LogoImg;
                        if (BusinessCardVO.ThemeID > 0)
                        {
                            Theme = cBO.FindThemeById(BusinessCardVO.ThemeID);
                        }
                    }
                }
            }
            else
            {
                Personal = new PersonalVO();
            }
        }
    }
}