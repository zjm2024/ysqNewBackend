using System;
using System.Text;
using SPLibrary.CoreFramework.BO;
using SPLibrary.UserManagement.BO;
using CoreFramework.VO;
using SPLibrary.UserManagement.VO;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using SPLibrary.RequireManagement.VO;
using SPLibrary.RequireManagement.BO;
using SPLibrary.CustomerManagement.VO;
using SPLibrary.CustomerManagement.BO;

namespace SPlatformService.RequireManagement
{
    public partial class CardSoftarticleShow : BasePage
    {
        public string Description = "";
        public int SoftArticleID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "乐聊名片";
                (this.Master as Shared.MasterPage).PageNameText = "软文详情";
            }

            base.ValidPageRight("软文管理", "Read");
            BindData();

        }

        public void BindData()
        {
            SoftArticleID = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["SoftArticleID"]) ? "0" : Request.QueryString["SoftArticleID"]);
            this.hidSoftArticleID.Value = SoftArticleID.ToString();
            StringBuilder sb = new StringBuilder();
            sb.Append("var hidSoftArticleID='").Append(hidSoftArticleID.ClientID).Append("';\n");
            Utilities.RegisterJs(Page, "JSCommonVar_CustomerPayOutHandle", sb.ToString());

            CardBO cBO = new CardBO(new CustomerProfile(), UserProfile.AppType);
            //getRequirement

            CardSoftArticleVO cVO = cBO.FindSoftArticleById(SoftArticleID);
            List<CardDataVO> CardDataVO = cBO.FindCardByCustomerId(cVO.CustomerId);
            if (CardDataVO.Count > 0)
            {
                cVO.Card = CardDataVO[0];
            }

            if (cVO.CustomerId != cVO.OriginalCustomerId)
            {
                List<CardDataVO> CardDataVO2 = cBO.FindCardByCustomerId(cVO.OriginalCustomerId);
                if (CardDataVO2.Count > 0)
                {
                    cVO.OriginalCard = CardDataVO2[0];
                }
            }else
            {
                cVO.OriginalCard = cVO.Card;
            }

            txtTitle.Text = cVO.Title;
            txtTitle.Enabled = false;
            
            txtEffectiveEndDateCreateEdit.Text = cVO.CreatedAt.ToString("yyyy-MM-dd");
            txtEffectiveEndDateCreateEdit.Enabled=false;
            Textbox1.Text = cVO.OriginalCard.Name;
            Textbox2.Text = cVO.Card.Name;
            Textbox1.Enabled = false;
            Textbox2.Enabled = false;
            Textbox3.Text = cVO.ReadCount.ToString();
            Textbox4.Text = cVO.ReprintCount.ToString();
            Textbox5.Text = cVO.GoodCount.ToString();
            Description = cVO.Description.Replace("<br/>", "\r\n");
        }
    }
}