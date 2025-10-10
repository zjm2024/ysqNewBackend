using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using SPLibrary.CoreFramework.Logging.BO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;

namespace ImportEXCEL
{
    public class EXCELHelper
    {
        /// <summary>
        /// 把DataTable写成EXCEL后放入文件流
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static MemoryStream RenderToExcel(DataTable table)
        {
            MemoryStream ms = new MemoryStream();

            using (table)
            {
                IWorkbook workbook = new HSSFWorkbook();

                ISheet sheet = workbook.CreateSheet();

                IRow headerRow = sheet.CreateRow(0);

                // handling header. 
                foreach (DataColumn column in table.Columns)
                    headerRow.CreateCell(column.Ordinal).SetCellValue(column.Caption);//If Caption not set, returns the ColumnName value 

                // handling value. 
                int rowIndex = 1;

                foreach (DataRow row in table.Rows)
                {
                    IRow dataRow = sheet.CreateRow(rowIndex);

                    foreach (DataColumn column in table.Columns)
                    {
                        dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                    }

                    rowIndex++;
                }

                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;
            }
            return ms;
        }

        /// <summary>
        /// 导出EXCEL
        /// </summary>
        /// <param name="table">数据源</param>
        /// <param name="localPath">存放目录</param>
        /// <param name="fileName">文件名</param>
        public static void ImportExcel(DataTable table, string localPath, string s_FileName)
        {
            MemoryStream ms = RenderToExcel(table);

            //创建所有子目录
            if (!Directory.Exists(localPath))
            {
                Directory.CreateDirectory(localPath);
            }

            string FileName = localPath + s_FileName;//文件存放路径

            if (System.IO.File.Exists(FileName)) //存在则删除
            {
                System.IO.File.Delete(FileName);
            }

            //保存到本地
            using (FileStream fs = new FileStream(FileName, FileMode.Create, FileAccess.Write))
            {
                byte[] data = ms.ToArray();

                fs.Write(data, 0, data.Length);
                fs.Flush();

                data = null;
            }
        }

        /// <summary>
        /// 从excel文件中读取数据
        /// </summary>
        /// <param name="fileUrl">实体文件的存储路径</param>
        /// <returns></returns>
        public static DataTable GetExcelDatatable(string fileUrl,string TableName="")
        {
            //支持.xls和.xlsx，即包括office2010等版本的;HDR=Yes代表第一行是标题，不是数据；
            string cmdText = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileUrl + "; Extended Properties=\"Excel 12.0;HDR=Yes\"";
            System.Data.DataTable dt = null;
            //建立连接
            OleDbConnection conn = new OleDbConnection(cmdText);
            try
            {
                //打开连接
                if (conn.State == ConnectionState.Broken || conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                System.Data.DataTable schemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                if (TableName == "")
                {
                    TableName = "Sheet1$";
                }
                string strSql = "select * from ["+ TableName + "]";   //这里指定表明为Sheet1,如果修改过表单的名称，请使用修改后的名称
                OleDbDataAdapter da = new OleDbDataAdapter(strSql, conn);
                DataSet ds = new DataSet();
                da.Fill(ds);
                dt = ds.Tables[0];
                return dt;
            }
            catch (Exception exc)
            {
                LogBO _log = new LogBO(typeof(EXCELHelper));
                string strErrorMsg = "Message:" + exc.Message.ToString() + "\r\n  Stack :" + exc.StackTrace + " \r\n Source :" + exc.Source+ " \r\n cmdText:"+ cmdText;
                _log.Error(strErrorMsg);
                throw exc;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }


        /// <summary>
        /// 从excel文件中读取列名和数据
        /// </summary>
        /// <param name="fileUrl">实体文件的存储路径</param>
        /// <param name="TableName">表名</param>
        /// <param name="fieldRowIndex">栏目行标</param>
        /// <returns></returns>
        public static ExcelTableData GetExcelColumnName(string fileUrl, string TableName = "",int fieldRowIndex=1)
        {
            //支持.xls和.xlsx，即包括office2010等版本的;HDR=Yes代表第一行是标题，不是数据；
            string cmdText = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileUrl + "; Extended Properties=\"Excel 12.0;HDR=No;IMEX=1\"";
            System.Data.DataTable dt = null;
            //建立连接
            OleDbConnection conn = new OleDbConnection(cmdText);
            try
            {
                //打开连接
                if (conn.State == ConnectionState.Broken || conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                System.Data.DataTable schemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                if (TableName == "")
                {
                    TableName = "Sheet1$";
                }
                string strSql = "select * from [" + TableName + "]";   //这里指定表明为Sheet1,如果修改过表单的名称，请使用修改后的名称
                OleDbDataAdapter da = new OleDbDataAdapter(strSql, conn);
                DataSet ds = new DataSet();
                da.Fill(ds);
                dt = ds.Tables[0];

                List<ColumnData> ColumnData = new List<ColumnData>();
                if(fieldRowIndex> dt.Rows.Count|| fieldRowIndex<1)
                {
                    return null;
                }
                fieldRowIndex -= 1;

                DataRow dr = dt.Rows[fieldRowIndex];
                //遍历所有列
                for (int i=0;i< dt.Columns.Count; i++)
                {
                    string Name = dr[i].ToString();
                    ColumnData cd = new ColumnData();
                    //设置列名
                    cd.ColumnName = "F"+i;
                    dt.Columns[i].ColumnName = cd.ColumnName;
                    if (Name != "")
                    {
                        cd.ColumnText = Name;
                    }
                    else
                    {
                        cd.ColumnText = cd.ColumnName;
                    }
                    ColumnData.Add(cd);
                }
                ExcelTableData ExcelTableData = new ExcelTableData();
                ExcelTableData.ColumnData = ColumnData;
                ExcelTableData.ExcelData = dt;
                return ExcelTableData;
            }
            catch (Exception exc)
            {
                LogBO _log = new LogBO(typeof(EXCELHelper));
                string strErrorMsg = "Message:" + exc.Message.ToString() + "\r\n  Stack :" + exc.StackTrace + " \r\n Source :" + exc.Source + " \r\n cmdText:" + cmdText;
                _log.Error(strErrorMsg);
                return null;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        /// <summary>
        /// 从excel文件中读取表格名称
        /// </summary>
        /// <param name="fileUrl">实体文件的存储路径</param>
        /// <returns></returns>
        public static List<string> GetExcelTableName(string fileUrl)
        {
            //支持.xls和.xlsx，即包括office2010等版本的;HDR=Yes代表第一行是标题，不是数据；
            string cmdText = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileUrl + "; Extended Properties=\"Excel 12.0; HDR = Yes\"";
            System.Data.DataTable dt = null;
            //建立连接
            OleDbConnection conn = new OleDbConnection(cmdText);
            try
            {
                //打开连接
                if (conn.State == ConnectionState.Broken || conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                List<string> TableName = new List<string>();
                foreach (DataRow row in dt.Rows)
                {
                    string name = row["TABLE_NAME"].ToString();
                    if (!(name.IndexOf("_xlnm#_FilterDatabase") > -1))
                    {
                        TableName.Add(row["TABLE_NAME"].ToString());
                    }
                }
                return TableName;
            }
            catch (Exception exc)
            {
                LogBO _log = new LogBO(typeof(EXCELHelper));
                string strErrorMsg = "Message:" + exc.Message.ToString() + "\r\n  Stack :" + exc.StackTrace + " \r\n Source :" + exc.Source + " \r\n cmdText:" + cmdText;
                _log.Error(strErrorMsg);
                return null;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
    public class ExcelTableData
    {
        public DataTable ExcelData { get; set; }
        public List<ColumnData> ColumnData { get; set; }
    }
    public class ColumnData
    {
        public string ColumnName { get; set; }
        public string ColumnText { get; set; }
    }
}