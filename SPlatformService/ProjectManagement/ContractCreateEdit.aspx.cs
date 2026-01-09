using System;
using System.Text;
using SPLibrary.CoreFramework.BO;
using SPLibrary.UserManagement.BO;
using SPLibrary.UserManagement.VO;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Web.UI.WebControls;
using SPLibrary.CustomerManagement.VO;
using SPLibrary.RequireManagement.BO;
using SPLibrary.RequireManagement.VO;
using SPLibrary.ProjectManagement.BO;
using SPLibrary.ProjectManagement.VO;
using SPLibrary.WebConfigInfo;
using SPLibrary.CustomerManagement.BO;

namespace SPlatformService.ProjectManagement
{
    public partial class ContractCreateEdit : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "我是雇主";
                (this.Master as Shared.MasterPage).PageNameText = "雇佣合同";
            }            

            int contractId = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["ContractId"]) ? "0" : Request.QueryString["ContractId"]);
            this.hidContractId.Value = contractId.ToString();

            RequireBO rBO = new RequireBO(new CustomerProfile());
            ProjectBO pBO = new ProjectBO(new CustomerProfile());
            CustomerBO cBO = new CustomerBO(new CustomerProfile());

            StringBuilder sb = new StringBuilder();
            if (contractId > 0)
            {
                ContractViewVO cVO = pBO.FindContractById(contractId);

                txtProjectName.Text = cVO.ProjectName;
                txtCost.Text = cVO.Cost.ToString();
                txtCommission.Text = cVO.Commission.ToString();
                txtStartDate.Text = cVO.StartDate.ToString("yyyy-MM-dd") == "1900-01-01" ? "" : cVO.StartDate.ToString("yyyy-MM-dd");
                txtEndDate.Text = cVO.EndDate.ToString("yyyy-MM-dd") == "1900-01-01" ? "" : cVO.EndDate.ToString("yyyy-MM-dd");
                divContractNote.InnerHtml = cVO.ContractNote;

                //绑定附件
                List<ContractFileVO> fileList = pBO.FindContractFileList(contractId);
                string oTR = "<table id=\"FileList\" class=\"table table-striped table-bordered table-hover dataTable ui-jqgrid-btable\" width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\"> \r\n";
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
                    ContractFileVO tcVO = fileList[i];
                    oTR += "<tr class=\"ui-widget-content jqgrow ui-row-ltr\"> \r\n";
                    oTR += "  <td class=\"center\"> \r\n";
                    oTR += "    <input class=\"cbox\" type=\"checkbox\" /> \r\n";
                    oTR += "    <input type=\"hidden\" value=\"File_" + tcVO.ContractId + "_" + tcVO.ContractFileId + "\" /> \r\n";
                    oTR += "  </td> \r\n";
                    oTR += "  <td class=\"center\" title=\"" + tcVO.FileName + "\"><a href=\"" + tcVO.FilePath + "\">" + tcVO.FileName + "</a></td> \r\n";
                    oTR += "  <td class=\"center\" title=\"" + tcVO.CreatedAt.ToString("yyyy-MM-dd hh:mm:ss") + "\">" + tcVO.CreatedAt.ToString("yyyy-MM-dd hh:mm:ss") + "</td> \r\n";
                    oTR += "</tr> \r\n";
                }
                oTR += "</tbody> \r\n";
                oTR += "</table>";
                divFileList.InnerHtml = oTR;

                //绑定步骤
                List<ContractStepsVO> stepsList = pBO.FindContractStepsList(contractId);
                oTR = "<table id=\"ContractStepsList\" class=\"table table-striped table-bordered table-hover dataTable ui-jqgrid-btable\" width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\"> \r\n";
                oTR += "<thead> \r\n";
                oTR += "     <tr class=\"ui-jqgrid-labels\"> \r\n";
                oTR += "        <th class=\"ui-state-default ui-th-column ui-th-ltr\" style=\"width: 30px;\"> \r\n";
                oTR += "            <div class=\"\" title=\"\"> \r\n";
                oTR += "                <input class=\"cbox\" type=\"checkbox\" onchange=\"checkAll(this, 'ContractStepsList')\" /> \r\n";
                oTR += "            </div> \r\n";
                oTR += "        </th> \r\n";
                oTR += "        <th class=\"ui-state-default ui-th-column ui-th-ltr\" style=\"width: 50px;\"> \r\n";
                oTR += "            <div class=\"ui-jqgrid-sortable\" title=\"序号\">序号</div> \r\n";
                oTR += "        </th> \r\n";
                oTR += "        <th class=\"ui-state-default ui-th-column ui-th-ltr\" style=\"width: 150px;\"> \r\n";
                oTR += "            <div class=\"ui-jqgrid-sortable\" title=\"阶段名称\">阶段名称</div> \r\n";
                oTR += "        </th> \r\n";               
                oTR += "        <th class=\"ui-state-default ui-th-column ui-th-ltr\" style=\"width: 150px;\"> \r\n";
                oTR += "            <div class=\"ui-jqgrid-sortable\" title=\"酬金\">酬金</div> \r\n";
                oTR += "        </th> \r\n";
                oTR += "        <th class=\"ui-state-default ui-th-column ui-th-ltr\" style=\"width: 250px;\"> \r\n";
                oTR += "            <div class=\"ui-jqgrid-sortable\" title=\"内容\">内容</div> \r\n";
                oTR += "        </th> \r\n";
                oTR += "    </tr> \r\n";
                oTR += "</thead> \r\n";
                oTR += "<tbody> \r\n";

                for (int i = 0; i < stepsList.Count; i++)
                {
                    ContractStepsVO tcVO = stepsList[i];
                    string id = "Steps_" + tcVO.ContractId + "_" + tcVO.ContractStepsId + "_" + tcVO.SortNO;
                    oTR += "<tr class=\"ui-widget-content jqgrow ui-row-ltr\"> \r\n";
                    oTR += "  <td class=\"center\" style=\"vertical-align: middle;\"> \r\n";
                    oTR += "    <input class=\"cbox\" type=\"checkbox\" /> \r\n";
                    oTR += "    <input type=\"hidden\" value=\"" + id + "\" /> \r\n";
                    oTR += "  </td> \r\n";
                    oTR += "  <td class=\"center\" style=\"vertical-align: middle;\" title=\"" + tcVO.SortNO + "\">" + tcVO.SortNO + "</td> \r\n";
                    oTR += "  <td class=\"center\" style=\"vertical-align: middle;\" title=\"" + tcVO.Title + "\"> \r\n";
                    oTR += "    <div class=\"hourinput\"> \r\n";
                    oTR += "      <input id=\"" + id + "_Title\" name=\"" + id + "_Title\" maxlength=\"50\" class=\"col-xs-10 col-sm-5 \" type=\"text\" style=\"width: 100%;\" value=\"" + tcVO.Title + "\" /> \r\n";
                    oTR += "    </div> \r\n";
                    oTR += "  </td> \r\n";
                    oTR += "  <td class=\"center\" style=\"vertical-align: middle;\" title=\"" + tcVO.Cost + "\"> \r\n";
                    oTR += "    <div class=\"hourinput\"> \r\n";
                    oTR += "      <input id=\"" + id + "_Cost\" name=\"" + id + "_Cost\" class=\"col-xs-10 col-sm-5 text-right\" type=\"text\" style=\"width: 100%;\" value=\"" + tcVO.Cost + "\" /> \r\n";
                    oTR += "    </div> \r\n";
                    oTR += "  </td> \r\n";
                    oTR += "  <td class=\"center\" style=\"vertical-align: middle;\" title=\"\"> \r\n";
                    oTR += "    <div class=\"hourinput\"> \r\n";
                    oTR += "      <textarea id=\"" + id + "_Comment\" name=\"" + id + "_Comment\" maxlength=\"400\" class=\"col-xs-10 col-sm-5 \" type=\"text\" style=\"width: 100%;\" >" + tcVO.Comment + "</textarea> \r\n";
                    oTR += "    </div> \r\n";
                    oTR += "  </td> \r\n";
                    oTR += "</tr> \r\n";

                    //sb.Append("$(\"#" + id + "_Cost\").rules(\"add\", { number: true, messages: { number: \"请输入正确数值！\" } });\n");
                    //sb.Append("$(\"#" + id + "_Title\").rules(\"add\", { required: true, messages: { required: \"请输入名称！\" } });\n");
                }
                oTR += "</tbody> \r\n";
                oTR += "</table>";
                divContractStepsList.InnerHtml = oTR;
                                

                sb.Append("var _RequireId='").Append(cVO.RequirementId).Append("';\n");
                sb.Append("var _AgencyCustomerId='").Append(cVO.AgencyCustomerId).Append("';\n");
                sb.Append("var _BusinessCustomerId='").Append(cVO.BusinessCustomerId).Append("';\n");
                sb.Append("var _BusinessStatus=").Append(cVO.BusinessStatus).Append(";\n");
                sb.Append("var _AgencyStatus=").Append(cVO.AgencyStatus).Append(";\n");
            }
            else
            {
                
            }

           
            sb.Append("var hidContractId='").Append(hidContractId.ClientID).Append("';\n");
            Utilities.RegisterJs(Page, "JSCommonVar_GenerateProject", sb.ToString());
        }
    }
}