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
    public partial class PersonalIMG : System.Web.UI.Page
    {
        public PersonalVO PersonalVO { set; get; }
        public string logo { set; get; }
        protected void Page_Load(object sender, EventArgs e)
        {
            int PersonalID = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["PersonalID"]) ? "0" : Request.QueryString["PersonalID"]);
            if (PersonalID > 0)
            {
                BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
                PersonalVO = cBO.FindPersonalById(PersonalID);
                if (PersonalVO == null)
                {
                    PersonalVO = new PersonalVO();
                }else
                {
                    PersonalVO.Headimg = PersonalVO.Headimg.Replace("https", "http");
                }
            }else
            {
                PersonalVO = new PersonalVO();
            }
        }
    }
}