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
    public partial class RequirementCreateEdit_New : CustomerBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "我是雇主";
                (this.Master as Shared.MasterPage).PageNameText = "任务发布";
            }

            int requirementId = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["RequirementId"]) ? "0" : Request.QueryString["RequirementId"]);
            this.hidRequirementId.Value = requirementId.ToString();

            CustomerViewVO customerVO = SiteCommon.GetCustomerById(CustomerProfile.CustomerId);
            bool isBusiness = false;
            if (customerVO.BusinessId > 0 && customerVO.BusinessStatus == 1)
                isBusiness = true;

            StringBuilder sb = new StringBuilder();
            sb.Append("var hidRequirementId='").Append(hidRequirementId.ClientID).Append("';\n");
            sb.Append("var isBusiness= ").Append(isBusiness.ToString().ToLower()).Append(";\n");
            sb.Append("var _BusinessId= ").Append(customerVO.BusinessId).Append(";\n");
            Utilities.RegisterJs(Page, "JSCommonVar_RequirementCreateEdit", sb.ToString());

            if (isBusiness && requirementId > 0)
            {
                RequireBO rBO = new RequireBO(new CustomerProfile());
                //getRequirement
                RequirementViewVO requirementVO = SiteCommon.GetRequireById(requirementId);
                //绑定行业区域
                BindData(requirementVO.CityId, requirementVO.CategoryId);

                txtRequirementCode.Text = requirementVO.RequirementCode;
                txtTitle.Text = requirementVO.Title;
                imgMainPic.Src = requirementVO.MainImg;
                txtEffectiveEndDate.Text = requirementVO.EffectiveEndDate.ToString("yyyy-MM-dd");

                if (requirementVO.CommissionType == 1)
                    radDecimal.Checked = true;
                else
                    radPer.Checked = true;

                txtCommission.Text = requirementVO.Commission.ToString();
                txtCost.Text = requirementVO.Cost.ToString();
                txtCommissionDescription.Text = requirementVO.CommissionDescription;
                txtPhone.Text = requirementVO.Phone;
                txtTargetAgency.Text = requirementVO.TargetAgency;
                txtAgencyCondition.Text = requirementVO.AgencyCondition;
                txtContactPerson.Text = requirementVO.ContactPerson;
                txtContactPhone.Text = requirementVO.ContactPhone;
                txtAgencySum.Text = requirementVO.agencySum.ToString();

                /*if (requirementVO.Status == 4)
                {
                    ProjectBO pBO = new ProjectBO(new CustomerProfile());
                    ProjectViewVO pVO = pBO.FindProjectByRequireId(requirementVO.RequirementId);
                    lblRequireCommission.Text = pBO.FindCommisionDelegationTotal(pVO.ProjectId).ToString("N2");
                }
                else
                {
                    lblRequireCommission.Text = rBO.FindRequireDelegateCommisionTotal(requirementVO.RequirementId).ToString("N2");
                }*/

                hidDescription.Value = requirementVO.Description;
                string scriptStr = "";

                scriptStr += "         var ue = UE.getEditor(\"container\"); \r\n";
                scriptStr += "         ue.ready(function() { \r\n";
                scriptStr += "             var desObj = $(\"#" + hidDescription.ClientID + "\"); \r\n";
                scriptStr += "             this.setContent(desObj.val()); \r\n";
                scriptStr += "             desObj.val(''); \r\n";
                scriptStr += "         }); \r\n";

                ClientScript.RegisterStartupScript(this.GetType(), "JS_Requirement_BindUEDITOR", scriptStr, true);

                hidStatus.Value = requirementVO.Status.ToString();

                //if (requirementVO.Status == 0)
                //{
                //    txtStatus.Text = "保存";
                //    btn_submit.Visible = true;
                //    btn_start.Visible = false;
                //    btn_stop.Visible = false;
                //    btn_updaterequirestatus.Visible = false;
                //}
                //else if (requirementVO.Status == 1)
                //{
                //    txtStatus.Text = "发布";
                //    btn_submit.Visible = false;
                //    btn_start.Visible = false;
                //    btn_stop.Visible = true;
                //    btn_updaterequirestatus.Visible = true;
                //}
                //else if (requirementVO.Status == 2)
                //{
                //    txtStatus.Text = "关闭";
                //    btn_submit.Visible = true;
                //    btn_start.Visible = false;
                //    btn_stop.Visible = false;
                //    btn_updaterequirestatus.Visible = false;
                //}
                //else if (requirementVO.Status == 3)
                //{
                //    txtStatus.Text = "暂停投标";
                //    btn_submit.Visible = false;
                //    btn_start.Visible = true;
                //    btn_stop.Visible = false;
                //    btn_updaterequirestatus.Visible = false;
                //}
                //else if (requirementVO.Status == 4)
                //{
                //    txtStatus.Text = "已选定销售";
                //    btn_save.Visible = false;
                //    btn_submit.Visible = false;
                //    btn_start.Visible = false;
                //    btn_stop.Visible = false;
                //    btn_updaterequirestatus.Visible = false;
                //}
                txtCreatedAt.Text = requirementVO.CreatedAt.ToString("yyyy-MM-dd");

                //BindTenderInvite(requirementVO.RequirementId);
                string oTR = "";
                
                //绑定目标客户行业
                //BindTargetCategory(requireId);
                List<RequirementTargetCategoryViewVO> targetCategoryList = SiteCommon.GetRequireCategoryByRequire(requirementId);
                oTR = "<table id=\"TargetCategoryList\" class=\"table table-striped table-bordered table-hover dataTable ui-jqgrid-btable\" width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\"> \r\n";
                oTR += "<thead> \r\n";
                oTR += "     <tr class=\"ui-jqgrid-labels\"> \r\n";
                oTR += "        <th class=\"ui-state-default ui-th-column ui-th-ltr\" style=\"width: 30px;\"> \r\n";
                oTR += "            <div class=\"\" title=\"\"> \r\n";
                oTR += "                <input class=\"cbox\" type=\"checkbox\" onchange=\"checkAll(this, 'TargetCategoryList')\" /> \r\n";
                oTR += "            </div> \r\n";
                oTR += "        </th> \r\n";
                oTR += "        <th class=\"ui-state-default ui-th-column ui-th-ltr\" style=\"width: 150px;\"> \r\n";
                oTR += "            <div class=\"ui-jqgrid-sortable\" title=\"行业大类\">行业大类</div> \r\n";
                oTR += "        </th> \r\n";
                oTR += "        <th class=\"ui-state-default ui-th-column ui-th-ltr\" style=\"width: 150px;\"> \r\n";
                oTR += "            <div class=\"ui-jqgrid-sortable\" title=\"行业小类\">行业小类</div> \r\n";
                oTR += "        </th> \r\n";
                oTR += "    </tr> \r\n";
                oTR += "</thead> \r\n";
                oTR += "<tbody> \r\n";

                for (int i = 0; i < targetCategoryList.Count; i++)
                {
                    RequirementTargetCategoryViewVO tcVO = targetCategoryList[i];
                    oTR += "<tr class=\"ui-widget-content jqgrow ui-row-ltr\"> \r\n";
                    oTR += "  <td class=\"center\"> \r\n";
                    oTR += "    <input class=\"cbox\" type=\"checkbox\" /> \r\n";
                    oTR += "    <input type=\"hidden\" value=\"TargetCategory_" + tcVO.RequirementId + "_" + tcVO.CategoryId + "\" /> \r\n";
                    oTR += "  </td> \r\n";
                    oTR += "  <td class=\"center\" title=\"" + tcVO.ParentCategoryName + "\">" + tcVO.ParentCategoryName + "</td> \r\n";
                    oTR += "  <td class=\"center\" title=\"" + tcVO.CategoryName + "\">" + tcVO.CategoryName + "</td> \r\n";
                    oTR += "</tr> \r\n";
                }
                oTR += "</tbody> \r\n";
                oTR += "</table>";
                divTargetCategoryList.InnerHtml = oTR;
                //绑定目标客户区域
                //BindTargetCity(requireId);
                List<RequirementTargetCityViewVO> targetCityList = SiteCommon.GetRequireCityByRequire(requirementId);
                oTR = "<table id=\"TargetCityList\" class=\"table table-striped table-bordered table-hover dataTable ui-jqgrid-btable\" width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\"> \r\n";
                oTR += "<thead> \r\n";
                oTR += "     <tr class=\"ui-jqgrid-labels\"> \r\n";
                oTR += "        <th class=\"ui-state-default ui-th-column ui-th-ltr\" style=\"width: 30px;\"> \r\n";
                oTR += "            <div class=\"\" title=\"\"> \r\n";
                oTR += "                <input class=\"cbox\" type=\"checkbox\" onchange=\"checkAll(this, 'TargetCityList')\" /> \r\n";
                oTR += "            </div> \r\n";
                oTR += "        </th> \r\n";
                oTR += "        <th class=\"ui-state-default ui-th-column ui-th-ltr\" style=\"width: 150px;\"> \r\n";
                oTR += "            <div class=\"ui-jqgrid-sortable\" title=\"省（直辖市）\">省（直辖市）</div> \r\n";
                oTR += "        </th> \r\n";
                oTR += "        <th class=\"ui-state-default ui-th-column ui-th-ltr\" style=\"width: 150px;\"> \r\n";
                oTR += "            <div class=\"ui-jqgrid-sortable\" title=\"城市\">城市</div> \r\n";
                oTR += "        </th> \r\n";
                oTR += "    </tr> \r\n";
                oTR += "</thead> \r\n";
                oTR += "<tbody> \r\n";

                for (int i = 0; i < targetCityList.Count; i++)
                {
                    RequirementTargetCityViewVO tcVO = targetCityList[i];
                    oTR += "<tr class=\"ui-widget-content jqgrow ui-row-ltr\"> \r\n";
                    oTR += "  <td class=\"center\"> \r\n";
                    oTR += "    <input class=\"cbox\" type=\"checkbox\" /> \r\n";
                    oTR += "    <input type=\"hidden\" value=\"TargetCity_" + tcVO.RequirementId + "_" + tcVO.CityId + "\" /> \r\n";
                    oTR += "  </td> \r\n";
                    oTR += "  <td class=\"center\" title=\"" + tcVO.ProvinceName + "\">" + tcVO.ProvinceName + "</td> \r\n";
                    oTR += "  <td class=\"center\" title=\"" + tcVO.CityName + "\">" + tcVO.CityName + "</td> \r\n";
                    oTR += "</tr> \r\n";
                }
                oTR += "</tbody> \r\n";
                oTR += "</table>";
                divTargetCityList.InnerHtml = oTR;

                //BindRequireClient(_BusinessId);
                List<RequirementTargetClientVO> bclientVOList = SiteCommon.GetRequireClientByBusiness(requirementId);
                oTR = "<table id=\"RequireClientList\" class=\"table table-striped table-bordered table-hover dataTable ui-jqgrid-btable\" width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\"> \r\n";
                oTR += "<thead> \r\n";
                oTR += "     <tr class=\"ui-jqgrid-labels\"> \r\n";
                oTR += "        <th class=\"ui-state-default ui-th-column ui-th-ltr\" style=\"width: 30px;\"> \r\n";
                oTR += "            <div class=\"\" title=\"\"> \r\n";
                oTR += "                <input class=\"cbox\" type=\"checkbox\" onchange=\"checkAll(this, 'RequireClientList')\" /> \r\n";
                oTR += "            </div> \r\n";
                oTR += "        </th> \r\n";
                oTR += "        <th class=\"ui-state-default ui-th-column ui-th-ltr\" style=\"width: 150px;\"> \r\n";
                oTR += "            <div class=\"ui-jqgrid-sortable\" title=\"客户名称\">客户名称</div> \r\n";
                oTR += "        </th> \r\n";
                oTR += "        <th class=\"ui-state-default ui-th-column ui-th-ltr\" style=\"width: 150px;\"> \r\n";
                oTR += "            <div class=\"ui-jqgrid-sortable\" title=\"用户名称\">用户名称</div> \r\n";
                oTR += "        </th> \r\n";
                oTR += "        <th class=\"ui-state-default ui-th-column ui-th-ltr\" style=\"width: 150px;\"> \r\n";
                oTR += "            <div class=\"ui-jqgrid-sortable\" title=\"任务现状\">任务现状</div> \r\n";
                oTR += "        </th> \r\n";
                oTR += "    </tr> \r\n";
                oTR += "</thead> \r\n";
                oTR += "<tbody> \r\n";
                for (int i = 0; i < bclientVOList.Count; i++)
                {
                    RequirementTargetClientVO tiVO = bclientVOList[i];
                    oTR += "<tr class=\"ui-widget-content jqgrow ui-row-ltr\"> \r\n";
                    oTR += "  <td class=\"center\"> \r\n";
                    oTR += "    <input class=\"cbox\" type=\"checkbox\" /> \r\n";
                    oTR += "    <input type=\"hidden\" value=\"TargetSuperClient_" + tiVO.RequirementId + "_" + tiVO.RequireClientId + "\" /> \r\n";
                    oTR += "  </td> \r\n";
                    oTR += "  <td class=\"center\" title=\"" + tiVO.ClientName + "\">" + tiVO.ClientName + "</td> \r\n";
                    oTR += "  <td class=\"center\" title=\"" + tiVO.CompanyName + "\">" + tiVO.CompanyName + "</td> \r\n";
                    oTR += "  <td class=\"center\" title=\"" + tiVO.Description + "\">" + tiVO.Description + "</td> \r\n";
                    oTR += "</tr> \r\n";
                }
                oTR += "</tbody> \r\n";
                oTR += "</table> ";
                divRequireClientList.InnerHtml = oTR;

                //绑定附件
                //BindFile(requireId);
                List<RequirementFileVO> fileList = SiteCommon.GetRequireFileByRequire(requirementId);
                oTR = "<table id=\"FileList\" class=\"table table-striped table-bordered table-hover dataTable ui-jqgrid-btable\" width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\"> \r\n";
                oTR += "<thead> \r\n";
                oTR += "     <tr class=\"ui-jqgrid-labels\"> \r\n";
                oTR += "        <th class=\"ui-state-default ui-th-column ui-th-ltr\" style=\"width: 30px;\"> \r\n";
                oTR += "            <div class=\"\" title=\"\"> \r\n";
                oTR += "                <input class=\"cbox\" type=\"checkbox\" onchange=\"checkAll(this, 'FileList')\" /> \r\n";
                oTR += "            </div> \r\n";
                oTR += "        </th> \r\n";
                oTR += "        <th class=\"ui-state-default ui-th-column ui-th-ltr\" style=\"width: 150px;\"> \r\n";
                oTR += "            <div class=\"ui-jqgrid-sortable\" title=\"文件名称\">文件名称</div> \r\n";
                oTR += "        </th> \r\n";
                oTR += "        <th class=\"ui-state-default ui-th-column ui-th-ltr\" style=\"width: 150px;\"> \r\n";
                oTR += "            <div class=\"ui-jqgrid-sortable\" title=\"上传日期\">上传日期</div> \r\n";
                oTR += "        </th> \r\n";
                oTR += "    </tr> \r\n";
                oTR += "</thead> \r\n";
                oTR += "<tbody> \r\n";

                for (int i = 0; i < fileList.Count; i++)
                {
                    RequirementFileVO tcVO = fileList[i];
                    oTR += "<tr class=\"ui-widget-content jqgrow ui-row-ltr\"> \r\n";
                    oTR += "  <td class=\"center\"> \r\n";
                    oTR += "    <input class=\"cbox\" type=\"checkbox\" /> \r\n";
                    oTR += "    <input type=\"hidden\" value=\"File_" + tcVO.RequirementId + "_" + tcVO.RequirementFileId + "\" /> \r\n";
                    oTR += "  </td> \r\n";
                    oTR += "  <td class=\"center\" title=\"" + tcVO.FileName + "\"><a href=\"" + tcVO.FilePath + "\">" + tcVO.FileName + "</a></td> \r\n";
                    oTR += "  <td class=\"center\" title=\"" + tcVO.CreatedDate.ToString("yyyy-MM-dd hh:mm:ss") + "\">" + tcVO.CreatedDate.ToString("yyyy-MM-dd hh:mm:ss") + "</td> \r\n";
                    oTR += "</tr> \r\n";
                }
                oTR += "</tbody> \r\n";
                oTR += "</table>";
                divFileList.InnerHtml = oTR;
            }
            else
            {
                BindData(0, 0);
                radDecimal.Checked = true;
                //btn_start.Visible = false;
                //btn_stop.Visible = false;
                //btn_updaterequirestatus.Visible = false;
            }
        }
        private void BindData(int cityId, int categoryId)
        {
            int provinceId = 0;
            int parentCategoryId = 0;
            //根据cityID和Cagegoryid  查找ProvinceID和ParentCategoryid
            if (cityId > 0)
            {
                CityVO cityVO = SiteCommon.GetCity(cityId);
                provinceId = cityVO.ProvinceId;
            }
            if (categoryId > 0)
            {
                CategoryVO categoryVO = SiteCommon.GetCategory(categoryId);
                parentCategoryId = categoryVO.ParentCategoryId;
            }

            drpProvince.Items.Clear();
            drpProvince.Items.Add(new ListItem("全部", "-1"));
            List<ProvinceVO> voList = SiteCommon.GetProvinceList();

            for (int i = 0; i < voList.Count; i++)
            {
                ProvinceVO pVO = voList[i];
                drpProvince.Items.Add(new ListItem(pVO.ProvinceName, pVO.ProvinceId.ToString()));
                if (pVO.ProvinceId == provinceId)
                    drpProvince.SelectedIndex = i + 1;
            }
            if (provinceId <= 0)
                drpProvince.SelectedIndex = 0;

            drpCity.Items.Clear();
            drpCity.Items.Add(new ListItem("全部", "-1"));
            List<CityVO> voChildList = SiteCommon.GetCityList(Convert.ToInt32(drpProvince.Items[drpProvince.SelectedIndex].Value));

            for (int i = 0; i < voChildList.Count; i++)
            {
                CityVO cVO = voChildList[i];
                drpCity.Items.Add(new ListItem(cVO.CityName, cVO.CityId.ToString()));
                if (cVO.CityId == cityId)
                    drpCity.SelectedIndex = i + 1;
            }
            if (cityId <= 0)
                drpCity.SelectedIndex = 0;

            drpCategory1.Items.Clear();
            drpCategory1.Items.Add(new ListItem("全部", "-1"));
            List<CategoryVO> voCategoryList = SiteCommon.GetParentCategoryList();
            for (int i = 0; i < voCategoryList.Count; i++)
            {
                CategoryVO pVO = voCategoryList[i];
                drpCategory1.Items.Add(new ListItem(pVO.CategoryName, pVO.CategoryId.ToString()));
                if (pVO.CategoryId == parentCategoryId)
                    drpCategory1.SelectedIndex = i + 1;
            }
            if (parentCategoryId <= 0)
                drpCategory1.SelectedIndex = 0;

            drpCategory2.Items.Clear();
            drpCategory2.Items.Add(new ListItem("全部", "-1"));
            List<CategoryVO> voCategoryChildList = SiteCommon.GetCategoryList(Convert.ToInt32(drpCategory1.Items[drpCategory1.SelectedIndex].Value));
            for (int i = 0; i < voCategoryChildList.Count; i++)
            {
                CategoryVO cVO = voCategoryChildList[i];
                drpCategory2.Items.Add(new ListItem(cVO.CategoryName, cVO.CategoryId.ToString()));
                if (cVO.CategoryId == categoryId)
                    drpCategory2.SelectedIndex = i + 1;
            }
            if (categoryId <= 0)
                drpCategory2.SelectedIndex = 0;
        }
    }
}