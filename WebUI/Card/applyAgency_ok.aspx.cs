using CoreFramework.VO;
using SPlatformService.TokenMange;
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
    public partial class applyAgency_ok : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                CardBO cBO = new CardBO(new CustomerProfile());
                int Type = Convert.ToInt32(string.IsNullOrEmpty(Request.Form["Type"]) ? "0" : Request.Form["Type"]);
                int CustomerId = Convert.ToInt32(string.IsNullOrEmpty(Request.Form["CustomerId"]) ? "0" : Request.Form["CustomerId"]);

                string name = string.IsNullOrEmpty(Request.Form["name"]) ? "" : Request.Form["name"];
                string phone = string.IsNullOrEmpty(Request.Form["phone"]) ? "" : Request.Form["phone"];
                string corporateName = string.IsNullOrEmpty(Request.Form["corporateName"]) ? "" : Request.Form["corporateName"];
                string position = string.IsNullOrEmpty(Request.Form["position"]) ? "" : Request.Form["position"];
                string content = string.IsNullOrEmpty(Request.Form["content"]) ? "" : Request.Form["content"];

                if (name == "")
                {
                    Alert("请填写您的名称，方便客服人员联系您!");
                    return;
                }
                if (phone == "")
                {
                    Alert("请填写您的手机号码，方便客服人员联系您!");
                    return;
                }

                CardVipApplyVO CardVipApplyVO = new CardVipApplyVO();
                CardVipApplyVO.Name = name;
                CardVipApplyVO.Phone = phone;
                CardVipApplyVO.Position = position;
                CardVipApplyVO.CorporateName = corporateName;
                CardVipApplyVO.Content = content;
                CardVipApplyVO.CreatedAt = DateTime.Now;
                CardVipApplyVO.CustomerId = CustomerId;
                CardVipApplyVO.Type = Type.ToString() ;
                CardVipApplyVO.AppType = 1;

                int VipApplyID = cBO.AddVipApply(CardVipApplyVO);
                if (VipApplyID > 0)
                {
                    Alert("申请成功，48小时内会有工作人员跟您联系，请保持手机畅通!");
                }
                else
                    Alert("申请失败，请稍后再试!");
            }
            catch (Exception ex)
            {
                Alert("系统错误!");
            }
}
        void Alert(string text)
        {
            Response.Write("<script language=javascript>alert('" + text + "');history.go(-1);</script>");
        }
    }
}