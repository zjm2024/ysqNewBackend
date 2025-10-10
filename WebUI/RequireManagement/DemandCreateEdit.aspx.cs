using System;
using System.Text;
using SPLibrary.CoreFramework.BO;
using SPLibrary.UserManagement.BO;
using CoreFramework.VO;
using SPLibrary.UserManagement.VO;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using SPLibrary.CoreFramework;
using WebUI.Common;
using SPLibrary.CustomerManagement.VO;
using SPLibrary.RequireManagement.VO;
using SPLibrary.RequireManagement.BO;
using SPLibrary.ProjectManagement.BO;
using SPLibrary.ProjectManagement.VO;

namespace WebUI.RequireManagement
{
    public partial class DemandCreateEdit : CustomerBasePage
    {
        public bool dropdown14=false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "采购需求";
                (this.Master as Shared.MasterPage).PageNameText = "需求详情";
            }
            int DemandId = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["DemandId"]) ? "0" : Request.QueryString["DemandId"]);
            dropdown14 = string.IsNullOrEmpty(Request.QueryString["dropdown14"]);

            StringBuilder sb = new StringBuilder();
            sb.Append("var _DemandId=").Append(DemandId.ToString()).Append(";\n");
            Utilities.RegisterJs(Page, "JSCommonVar_RequirementCreateEdit", sb.ToString());

            DemandBO rBO = new DemandBO(new CustomerProfile());
            //getRequirement
            DemandViewVO demandviewVO = rBO.FindDemandById(DemandId);

            txtPhone.Text = demandviewVO.Phone;
            txtPhone.Enabled = false;
            txtDescription.Text = demandviewVO.Description.Replace("<br/>", "\r\n");
            txtEffectiveEndDateCreateEdit.Text = demandviewVO.EffectiveEndDate.ToString("yyyy-MM-dd");
            BindData(demandviewVO.CategoryId);


            if (demandviewVO.Status == 0)
            {
                btn_save.Visible = true;
                btn_submit.Visible = true;
                btn_updaterequirestatus.Visible = false;
            }else if(demandviewVO.Status == 1)
            {
                btn_save.Visible = false;
                btn_submit.Visible = true;
                btn_updaterequirestatus.Visible = true;
            }

            List<DemandOfferVO> tenderInviteList = rBO.FindOfferByDemand(DemandId);
            string oTR = "<table id=\"TenderInviteList\" class=\"table table-striped table-bordered table-hover dataTable ui-jqgrid-btable\" width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\"> \r\n";
            oTR += "       <thead> \r\n";
            oTR += "    <tr class=\"ui-jqgrid-labels\"> \r\n";
            oTR += "<th class=\"ui-state-default ui-th-column ui-th-ltr\" style=\"width: 150px; \"> \r\n";
            oTR += "    <div class=\"ui-jqgrid-sortable\" title=\"留言日期\">留言日期</div> \r\n";
            oTR += "</th> \r\n";
            oTR += "<th class=\"ui-state-default ui-th-column ui-th-ltr\" style=\"width: 150px; \"> \r\n";
            oTR += "    <div class=\"ui-jqgrid-sortable\" title=\"留言人名称\">留言人名称</div> \r\n";
            oTR += "</th> \r\n";
            oTR += "<th class=\"ui-state-default ui-th-column ui-th-ltr\" style=\"width: 150px; \"> \r\n";
            oTR += "    <div class=\"ui-jqgrid-sortable\" title=\"联系号码\">联系号码</div> \r\n";
            oTR += "</th> \r\n";
            oTR += "<th class=\"ui-state-default ui-th-column ui-th-ltr\" style=\"width: 150px; \"> \r\n";
            oTR += "    <div class=\"ui-jqgrid-sortable\" title=\"留言\">留言</div> \r\n";
            oTR += "</th> \r\n";
            oTR += "<th class=\"ui-state-default ui-th-column ui-th-ltr\" style=\"width: 150px; \"> \r\n";
            oTR += "    <div class=\"ui-jqgrid-sortable\" title=\"线上联系\">线上联系</div> \r\n";
            oTR += "</th> \r\n";
            oTR += "</tr> \r\n";
            oTR += "</thead> \r\n";
            oTR += "<tbody> \r\n";
            for (int i = 0; i < tenderInviteList.Count; i++)
            {
                DemandOfferVO tiVO = tenderInviteList[i];
                oTR += "<tr class=\"ui-widget-content jqgrow ui-row-ltr\"> \r\n";
                oTR += "  <td class=\"center\" title=\"" + tiVO.CreatedAt + "\">" + tiVO.CreatedAt+ "</td> \r\n";
                oTR += "  <td class=\"center\" title=\"" + tiVO.Name + "\">" + tiVO.Name + "</td> \r\n";
                oTR += "  <td class=\"center\" title=\"" + tiVO.Phone + "\">" + tiVO.Phone + "</td> \r\n";
                oTR += "  <td class=\"center\" title=\"" + tiVO.OfferDescription + "\">" + tiVO.OfferDescription + "</td> \r\n";
                oTR += "  <td class=\"center\" title=\"线上联系\"><a class=\"Demand_btn\"  href=\"../ZXTIM.aspx?MessageTo=" + tiVO.CustomerId + "\" target=\"_blank\">线上联系</a></td> \r\n";
                oTR += "</tr> \r\n";
            }
            oTR += "</tbody> \r\n";
            oTR += "</table> ";
            divTenderInviteList.InnerHtml = oTR;
        }
        private void BindData(int categoryId)
        {
            drpdemand_class.Items.Clear();
            DemandBO rBO = new DemandBO(new CustomerProfile());
            List <DemandCategoryVO> voList = rBO.FindCategory();
            for (int i = 0; i < voList.Count; i++)
            {
                DemandCategoryVO pVO = voList[i];
                drpdemand_class.Items.Add(new ListItem(pVO.CategoryName, pVO.CategoryId.ToString()));
                if (pVO.CategoryId == categoryId)
                    drpdemand_class.SelectedIndex = i;
            }
        }
    }
}