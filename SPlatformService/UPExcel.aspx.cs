using CoreFramework.VO;
using ImportEXCEL;
using SPLibrary.BusinessCardManagement.BO;
using SPLibrary.BusinessCardManagement.VO;
using SPLibrary.CoreFramework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SPlatformService
{
    public partial class UPExcel : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void InputFileUploadButton_Click(object sender, EventArgs e)
        {
            HttpFileCollection files = Request.Files;
            string filePath = Server.MapPath("~/UploadFolder/ExcelFile/UploadExcel");
            if (files.Count != 0)
            {
                 FileInfo fi = new FileInfo(files[0].FileName);
                 string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + fi.Extension;
                 string PhysicalPath = Path.Combine(filePath, newFileName);
                 files[0].SaveAs(PhysicalPath);
                 Response.Write("<p>上传成功:</p>"+ PhysicalPath);
                GetExcelData(PhysicalPath);
            }
            else
            {
                 Response.Write("<p>未获取到Files:"+ files.Count.ToString()+"</p>");
            }
        }

        /// <summary>
        /// 识别excel文件
        /// </summary>
        /// <param name="PhysicalPath"></param>
        public void GetExcelData(string PhysicalPath)
        {
            try
            {
                DataTable dt = EXCELHelper.GetExcelDatatable(PhysicalPath);

                DataTable Newdt = new DataTable();
                Newdt.Columns.Add("姓名", typeof(String));
                Newdt.Columns.Add("所属客户", typeof(String));
                Newdt.Columns.Add("兴趣爱好", typeof(String));
                Newdt.Columns.Add("备注", typeof(String));
                Newdt.Columns.Add("手机", typeof(String));
                Newdt.Columns.Add("职位", typeof(String));
                Newdt.Columns.Add("负责人", typeof(String));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = Newdt.NewRow();
                    dr["姓名"] = dt.Rows[i]["姓名"].ToString();
                    dr["所属客户"] = dt.Rows[i]["所属客户"].ToString();
                    dr["兴趣爱好"] = dt.Rows[i]["兴趣爱好"].ToString();
                    dr["备注"] = dt.Rows[i]["备注"].ToString();
                    dr["手机"] = Regex.Replace(dt.Rows[i]["手机"].ToString(), @"[^0-9-]+", ""); ;
                    dr["职位"] = dt.Rows[i]["职位"].ToString();
                    dr["负责人"] = dt.Rows[i]["负责人"].ToString();

                    Newdt.Rows.Add(dr);
                }

                for (int i = 0; i < Newdt.Rows.Count; i++)
                {
                    CrmVO CrmVO = new CrmVO();
                    CrmVO.Title = Newdt.Rows[i]["姓名"].ToString();
                    CrmVO.Field3 = Newdt.Rows[i]["所属客户"].ToString();
                    CrmVO.Field8 = Newdt.Rows[i]["兴趣爱好"].ToString();
                    if (Newdt.Rows[i]["备注"].ToString() != "")
                    {
                        CrmVO.Content = "<div>" + Newdt.Rows[i]["备注"].ToString() + "</div></NTAG>";
                    }
                    CrmVO.Field2 = Newdt.Rows[i]["手机"].ToString();

                    if (Newdt.Rows[i]["职位"].ToString() != "")
                    {
                        CrmVO.Title += "-" + Newdt.Rows[i]["职位"].ToString();
                    }

                    CrmVO.PersonalID = 0;
                    if (Newdt.Rows[i]["负责人"].ToString() == "李岚"|| Newdt.Rows[i]["负责人"].ToString() == "$userName=LiLan$")
                    {
                        CrmVO.PersonalID = 4597;
                    }
                    if (Newdt.Rows[i]["负责人"].ToString() == "葛文良" || Newdt.Rows[i]["负责人"].ToString() == "$userName=13622683639$")
                    {
                        CrmVO.PersonalID = 4601;
                    }
                    if (Newdt.Rows[i]["负责人"].ToString() == "成玘" || Newdt.Rows[i]["负责人"].ToString() == "$userName=chengqi$")
                    {
                        CrmVO.PersonalID = 4436;
                    }
                    if (Newdt.Rows[i]["负责人"].ToString() == "曾晓燕" || Newdt.Rows[i]["负责人"].ToString() == "$userName=ZengXiaoYan$")
                    {
                        CrmVO.PersonalID = 4136;
                    }


                    UpdateCrmSite(CrmVO);
                }

                Response.Write("<p>识别excel成功</p>");
            }
            catch (Exception ex)
            {
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                Response.Write("<p>识别excel错误:</p>"+ strErrorMsg);
            }
        }
        public void UpdateCrmSite(CrmVO CrmVO)
        {
            CrmBO cBO = new CrmBO(new CustomerProfile());
            CrmVO.CrmID = 0;
            CrmVO.CreatedAt = DateTime.Now;
            
            CrmVO.BusinessID = 175;
            CrmVO.Field4 = "未曾接触";
            CrmVO.Field5 = "资源导入";
            CrmVO.Type = "Clue";
            CrmVO.Status = 1;
            int CrmID = cBO.AddCrm(CrmVO);
        }

    }
}