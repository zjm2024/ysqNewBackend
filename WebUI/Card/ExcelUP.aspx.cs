using CoreFramework.VO;
using ImportEXCEL;
using Pathoschild.Http.Client.Internal;
using SPLibrary.BusinessCardManagement.BO;
using SPLibrary.BusinessCardManagement.VO;
using SPLibrary.CoreFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace WebUI.Card
{
    
    public partial class ExcelUP : System.Web.UI.Page
    {
        public static int nextindex = 0;//步骤数
        public static string savePath = "";
        public static int fieldRowIndex = 1;//标题行数
        public static int dataRowIndex = 1;//内容开始行数

        public static List<string> TableName = null;//excel所有表名
        public static List<string> ckTableName = null;//选中的表名
        public static Dictionary<string, ExcelTableData> dic = new Dictionary<string, ExcelTableData>();//导上来的excel数据
        public static List<ColumnName> columnNames = new List<ColumnName>();//选中的对应列集合
        public static int BusinessID = 0;
        public static int PersonalID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                nextindex = 0;
                nextBtn.Visible = false;
                cancelBtn.Visible = false;
                upBtn.Visible = false;
                //PersonalID = 1;
                //BusinessID = 1;
                PersonalID = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["PersonalID"]) ? "0" : Request.QueryString["PersonalID"]);
                BusinessID = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["BusinessID"]) ? "0" : Request.QueryString["BusinessID"]);
            }
            //BusinessCardBO bBO = new BusinessCardBO(new CustomerProfile());
            //List<OrderVO> list = bBO.FindOrderList("OrderNO='2022110717532469144300'");
            //fieldRowIndex = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["fieldRowIndex"]) ? "1" : Request.QueryString["fieldRowIndex"]);
        }
        protected void UploadBtn_Click(object sender, EventArgs e)
        {
            if (BusinessID>0 && PersonalID>0)
            {
                nextBtn.Visible = false;
                cancelBtn.Visible = false;
                dic.Clear();
                if (ExcelFileUpload.HasFile == false)//HasFile用来检查FileUpload是否有文件
                {
                    Response.Write("<script>alert('请您选择Excel文件')</script> ");
                    return;//当无文件时,返回
                }
                string IsXls = Path.GetExtension(ExcelFileUpload.FileName).ToString().ToLower();//System.IO.Path.GetExtension获得文件的扩展名
                if (IsXls != ".xlsx" && IsXls != ".xls")
                {
                    Response.Write(ExcelFileUpload.FileName);
                    Response.Write("<script>alert('只可以选择Excel文件')</script>");
                    return;//当选择的不是Excel文件时,返回
                }
                savePath = Server.MapPath("/UploadFolder/UploadExcel/");//Server.MapPath 服务器上的指定虚拟路径相对应的物理文件路径
                //创建目录
                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }
                savePath += DateTime.Now.ToString("yyyyMMddHHmmssffff") + IsXls;
                DataTable ds = new DataTable();
                ExcelFileUpload.SaveAs(savePath);//将文件保存到指定路径
                TableName = EXCELHelper.GetExcelTableName(savePath);//读取excel数据
                if (TableName != null)
                {
                    nextindex = 1;
                    nextBtn.Visible = true;
                    cancelBtn.Visible = true;
                    ExcelFileUpload.Visible = false;
                    UploadBtn.Visible = false;   
                    foreach (var item in TableName)
                    {
                        ExcelTableData ExcelTableData = EXCELHelper.GetExcelColumnName(savePath, item, fieldRowIndex);
                        dic.Add(item, ExcelTableData);
                        if (ExcelTableData.ExcelData.Rows.Count > 5000)
                        {
                            dic.Clear();
                            Response.Write("<script>alert('Excel文件不能超过5000条')</script>");
                            break;
                        }
                    }
                    
                    //if (ExcelTableData != null)
                    //{
                    //    for (int i = 0; i < ExcelTableData.ExcelData.Columns.Count && i < ExcelTableData.ColumnData.Count; i++)
                    //    {
                    //        Response.Write(ExcelTableData.ExcelData.Columns[i].ColumnName + "----" + ExcelTableData.ColumnData[i].ColumnText + "<br>");
                    //    }
                    //}
                    //else
                    //{
                    //    Response.Write("<script>alert('栏目名称识别错误')</script>");
                    //}
                }
                else
                {
                    Response.Write("<script>alert('Excel文件读取错误')</script>");
                }
                delExcel();
            }
            else
            {
                Response.Write("<script>alert('参数异常')</script>");
            }
        }
        protected void delExcel()
        {
            try
            {
                File.Delete(savePath);//删除文件
            }
            catch
            {

            }
        }

        protected void upBtn_Click(object sender, EventArgs e)
        {
            nextindex--;
            if (nextindex <= 1)
            {
                nextindex = 1;
                upBtn.Visible = false;
            }
        }

        protected void nextBtn_Click(object sender, EventArgs e)
        {
            if (nextindex==1)//选择表
            {
                ckTableName = new List<string>();//清空防重复
                foreach (var item in TableName)
                {
                    if (Request.Form[item]=="on")
                    {
                        ckTableName.Add(item);
                    }
                }
                if (ckTableName.Count >= 1)
                {
                    nextindex++;
                    upBtn.Visible = true;
                }
                else
                {
                    Response.Write("<script>alert('请勾选要导入Excel的表')</script>");
                }
            }
            else if (nextindex == 2)//获取行数
            {
                try
                {
                    //获取标题行
                    fieldRowIndex = Convert.ToInt32(Request.Form["text1"]);
                    dataRowIndex = Convert.ToInt32(Request.Form["text2"]);
                    nextindex++;
                }
                catch (Exception)
                {
                    Response.Write("<script>alert('请输入正确的行数')</script>");
                }

            }
            else if (nextindex == 3)//获取对应列
            {
                try
                {
                    columnNames.Clear();
                    for (int i = 0; i < ckTableName.Count; i++)
                    {
                        ColumnName columnName = new ColumnName();
                        columnName.tablename = ckTableName[i];
                        columnName.name1 = Request.Form["td" + i + "_1"];
                        columnName.name2 = Request.Form["td" + i + "_2"];
                        columnName.name3 = Request.Form["td" + i + "_3"];
                        columnName.name4 = Request.Form["td" + i + "_4"];
                        columnName.name5 = Request.Form["td" + i + "_5"];
                        columnName.name6 = Request.Form["td" + i + "_6"];
                        if (string.IsNullOrEmpty(columnName.name1) && string.IsNullOrEmpty(columnName.name2) && string.IsNullOrEmpty(columnName.name3) &&
                            string.IsNullOrEmpty(columnName.name4) && string.IsNullOrEmpty(columnName.name5) && string.IsNullOrEmpty(columnName.name6))
                        {
                            columnNames.Clear();
                            Response.Write(string.Format("<script>alert('表:{0}未选择对应的栏位')</script>", columnName.tablename));
                            return;
                        }
                        columnNames.Add(columnName);
                    }
                    nextBtn.Visible = false;
                    cancelBtn.Visible = false;
                    upBtn.Visible = false;
                    nextindex = 0;
                    doUp(columnNames);
                    ExcelFileUpload.Visible = true;
                    UploadBtn.Visible = true;
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('上传异常');</script>");
                }
                
            }
        }

        #region 上传提交
        private void doUp(List<ColumnName> list) {
            int falgCount = 0;
            int successCount = 0;
            if (list!=null && list.Count>0)
            {
                BusinessCardBO bBO = new BusinessCardBO(new CustomerProfile());
                CrmBO cBO = new CrmBO(new CustomerProfile());
                //循环导入的表
                for (int i = 0; i < list.Count; i++)
                {
                    //获取对应excel表的数据
                    ExcelTableData ExcelTableData = dic.FirstOrDefault(k => k.Key == list[i].tablename).Value;
                    for (int y = dataRowIndex-1; y < ExcelTableData.ExcelData.Rows.Count; y++)
                    {

                        CrmVO CrmVO = new CrmVO();
                        CrmVO.CreatedAt = DateTime.Now;
                        CrmVO.PersonalID = PersonalID;
                        CrmVO.BusinessID = BusinessID;
                        string name1 = list[i].name1;
                        string name2 = list[i].name2;
                        string name3 = list[i].name3;
                        string name4 = list[i].name4;
                        string name5 = list[i].name5;
                        string name6 = list[i].name6;
                        if (!string.IsNullOrEmpty(name1) && "null" !=name1)
                        {
                            CrmVO.Title = ExcelTableData.ExcelData.Rows[y][name1].ToString();//联系人
                        }
                        if (!string.IsNullOrEmpty(name2) && "null" != name2)
                        {
                            CrmVO.Field2 = ExcelTableData.ExcelData.Rows[y][name2].ToString();//联系电话
                        }
                        if (!string.IsNullOrEmpty(name3) && "null" != name3)
                        {
                            CrmVO.Field1 = ExcelTableData.ExcelData.Rows[y][name3].ToString();//联系地址
                        }
                        if (!string.IsNullOrEmpty(name4) && "null" != name4)
                        {
                            CrmVO.Field3 = ExcelTableData.ExcelData.Rows[y][name4].ToString();//单位名称
                        }
                        if (!string.IsNullOrEmpty(name5) && "null" != name5)
                        {
                            CrmVO.Field8 = ExcelTableData.ExcelData.Rows[y][name5].ToString();//行业
                        }
                        if (!string.IsNullOrEmpty(name6) && "null" != name6)
                        {
                            CrmVO.Content = ExcelTableData.ExcelData.Rows[y][name6].ToString();//备注
                        }
                        CrmVO.Field5 = "excel文件导入";
                        CrmVO.Type = "Clue";
                        CrmVO.Field4 = "未曾接触";


                        int CrmID = cBO.AddCrm(CrmVO);
                        if (CrmID > 0)
                        {
                            successCount++;
                        }
                        else
                        {
                            falgCount++;
                        }
                    }
                    
                }
            }
            Response.Write("<script>alert('成功："+ successCount+"个,失败："+ falgCount+"个');</script>");
        }

        #endregion

        #region 动态生成页面内容
        /// <summary>
        /// 动态生成页面内容
        /// </summary>
        /// <returns></returns>
        public string GetText()
        {
            string htmltext = "";
            //步骤1：选择表
            if (nextindex==1 && TableName != null)
            {
                
                for (int i = 0; i < TableName.Count; i++)
                {
                    htmltext+= "<div class=\"ckdiv\"><input name=\""+ TableName[i] + "\" class=\"ck\" type=\"checkbox\"/>"+ TableName[i]+"</div></br>";
                }
            }
            else if (nextindex == 2)//步骤2：输入开始行数
            {
                htmltext += "<div class=\"exceltitle\">标题名行</div> <div class=\"exceltext\"><input placeholder=\"请输入行数\" name=\"text1\" type=\"text\" value=\"1\" /></div> </br>";
                htmltext += "<div class=\"exceltitle\">第一个数据行</div> <div class=\"exceltext\"><input placeholder=\"请输入行数\" name=\"text2\" type=\"text\" value=\"2\" /></div> </br>";
            }
            else if (nextindex == 3)//步骤3：选择对应字段
            {
                htmltext = getTableHtml();
            }
            else
            {
                htmltext = "";
            }
            return htmltext;

        }

        public string getTableHtml() {
            StringBuilder htmltext = new StringBuilder();
            htmltext.Append(string.Format(@"
               <div style=""width:94%;""><div class=""td1"" style=""float:left;""> 源表：</div>
                <div class=""td2"" style=""float:left;"">  <select name=""seltable"" onchange = ""selTableChange(this)"" >
            "));
            foreach (var item in ckTableName)
            {
                htmltext.Append(string.Format(@"
                    <option>{0}</option>
                ", item));
            }
            htmltext.Append(string.Format(@"
                </select></div></div>
                <div style=""width:100%;height:70rem;overflow:hidden;"">
            "));
            for (int i = 0; i < ckTableName.Count; i++)
            {
                
                ExcelTableData ExcelTableData = dic.FirstOrDefault(k => k.Key == ckTableName[i]).Value;
                htmltext.Append(string.Format(@"
                         <table style=""height:70rem;display:block;"" id=""td{0}"">
                             <tr>
                                 <td class=""td1"">目标栏位</td>
                                 <td class=""td2""  style=""padding-right: 2vw;"">源栏位</td>
                             </tr>
                             <tr>
                                 <td class=""td1"">联系人</td>
                                 <td class=""td2"">
                                     <select name=""td{0}_1"">
                                         {1}
                                     </select>
                                 </td>
                             </tr>
                             <tr>
                                 <td class=""td1"">联系电话</td>
                                 <td class=""td2"">
                                     <select name=""td{0}_2"">
                                         {1}
                                     </select>
                                 </td>
                             </tr>
                             <tr>
                                 <td class=""td1"">联系地址</td>
                                 <td class=""td2"">
                                     <select name=""td{0}_3"">
                                         {1}
                                     </select>
                                 </td>
                             </tr>
                             <tr>
                                 <td class=""td1"">单位名称</td>
                                 <td class=""td2"">
                                     <select name=""td{0}_4"">
                                         {1}
                                     </select>
                                 </td>
                             </tr>
                             <tr>
                                 <td class=""td1"">行业</td>
                                 <td class=""td2"">
                                     <select name=""td{0}_5"">
                                         {1}
                                     </select>
                                 </td>
                             </tr>
                             <tr>
                                 <td class=""td1"">备注</td>
                                 <td class=""td2"">
                                     <select name=""td{0}_6"">
                                         {1}
                                     </select>
                                 </td>
                             </tr>
                         </table>
                ", i, getOptionText(ExcelTableData.ColumnData)));
            }

            htmltext.Append(string.Format(@"
                </div>
            "));

            return htmltext.ToString();
        }

        /// <summary>
        /// 获取列的选择项
        /// </summary>
        /// <param name="columnDatas"></param>
        /// <returns></returns>
        public string getOptionText(List<ColumnData> columnDatas) {
            StringBuilder str = new StringBuilder();
            str.Append(string.Format("<option> </option>"));
            foreach (var item in columnDatas)
            {
                str.Append(string.Format("<option value=\"{0}\">{1}</option>",item.ColumnName, item.ColumnText));
            }
            return str.ToString();
        }
        #endregion

        protected void cancelBtn_Click(object sender, EventArgs e)
        {
            //取消按钮
            nextindex = 0;//步骤归零
            nextBtn.Visible = false;
            cancelBtn.Visible = false;
            upBtn.Visible = false;
            ExcelFileUpload.Visible = true;
            UploadBtn.Visible = true;
        }
    }
    public class ColumnName {
        public string tablename { get; set; }
        public string name1 { get; set; }
        public string name2 { get; set; }
        public string name3 { get; set; }
        public string name4 { get; set; }
        public string name5 { get; set; }
        public string name6 { get; set; }

    }
}