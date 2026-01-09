using CoreFramework.VO;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.CustomerManagement.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebUI.Card
{
    public partial class SignUpShow : System.Web.UI.Page
    {
        public int PartySignUpID = 0;
        public CardPartySignUpViewVO sVO = new CardPartySignUpViewVO();
        protected void Page_Load(object sender, EventArgs e)
        {
            PartySignUpID = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["PartySignUpID"]) ? "0" : Request.QueryString["PartySignUpID"]);

            CardBO cBO = new CardBO(new CustomerProfile());
            sVO = cBO.FindSignUpViewById(PartySignUpID);
            if (sVO == null)
            {
                Alert("获取入场券失败！");
            }
            if (sVO.SignUpQRCodeImg == "")
            {
                sVO.SignUpQRCodeImg = cBO.GetCardPartySignUpQR(sVO.PartySignUpID);
            }

        }
        void Alert(string text)
        {
            Response.Write("<script language=javascript>alert('" + text + "');history.go(-1);</script>");
        }
    }
}