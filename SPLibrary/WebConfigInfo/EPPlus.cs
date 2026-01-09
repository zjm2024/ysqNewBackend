using OfficeOpenXml;
using SPLibrary.CoreFramework;
using SPLibrary.CoreFramework.Logging.BO;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace SPLibrary.WebConfigInfo
{
    public class EPPlus
    {
        /// <summary>
        /// Datatable生成Excel表格并返回路径
        /// </summary>
        /// <param name="m_DataTable">Datatable</param>
        /// <param name="s_folder">自定义路径</param>
        /// <param name="s_FileName">文件名</param>
        /// <returns></returns>
        public static string DataToExcel(DataTable m_DataTable, string s_folder, string s_FileName)
        {
            try
            {
                string folder = "/UploadFolder/ExcelFile/" + s_folder;
                string localPath = ConfigInfo.Instance.UploadFolder + folder;
                string ExcelUrl = ConfigInfo.Instance.APIURL + folder + s_FileName;
                if (!Directory.Exists(localPath))
                {
                    Directory.CreateDirectory(localPath);
                }
                ToExcel(m_DataTable, localPath + s_FileName);
                return ExcelUrl;        //返回生成文件的绝对路径
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(EPPlus));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return null;
            }
        }
        public static void ToExcel(DataTable dt, string strFilePath)
        {
            try
            {
                FileInfo file = new FileInfo(strFilePath);
                using (ExcelPackage ep = new ExcelPackage(file))
                {
                    ExcelWorksheet ws = ep.Workbook.Worksheets.Add("Sheet1");
                    ws.Cells["A1"].LoadFromDataTable(dt, true);

                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        string dtcolumntype = dt.Columns[j].DataType.Name.ToLower();
                        if (dtcolumntype == "datetime")
                        {
                            ws.Column(j + 1).Style.Numberformat.Format = "yyyy/m/d h:mm";
                        }
                    }

                    ep.Save();

                    //for (int j = 0; j < dt.Columns.Count; j++)
                    //{
                    //    string dtcolumntype = dt.Columns[j].DataType.Name.ToLower();
                    //    if (dtcolumntype == "datetime")
                    //    {
                    //        for (int i = 0; i < dt.Rows.Count; i++)
                    //        {
                    //            ws.Cells[i+1, j+1].Style.Numberformat.Format = "yyyy/m/d h:mm";
                    //        }
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static void ToExcel(DataTable dt, string strTemplete, string strExcelFile, int intSheet)
        {
            FileInfo strTemp = new FileInfo(strTemplete);
            FileInfo strNewTemp = new FileInfo(strExcelFile);
            try
            {
                ExcelPackage package = new ExcelPackage(strTemp);
                int vSheetCount = package.Workbook.Worksheets.Count; //获取总Sheet页

                ExcelWorksheet worksheet = package.Workbook.Worksheets[intSheet];//选定 指定页

                //int maxColumnNum = worksheet.Dimension.End.Column;//最大列
                //int minColumnNum = worksheet.Dimension.Start.Column;//最小列

                //int maxRowNum = worksheet.Dimension.End.Row;//最小行
                //int minRowNum = worksheet.Dimension.Start.Row;//最大行

                worksheet.Cells["A1"].LoadFromDataTable(dt, true);

                package.SaveAs(strNewTemp);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void ToExcel2(DataTable dt1, DataTable dt2, string strTemplete, string strExcelFile)
        {
            FileInfo strTemp = new FileInfo(strTemplete);
            FileInfo strNewTemp = new FileInfo(strExcelFile);
            try
            {
                ExcelPackage package = new ExcelPackage(strTemp);
                int vSheetCount = package.Workbook.Worksheets.Count; //获取总Sheet页

                ExcelWorksheet worksheet = package.Workbook.Worksheets["Sheet2"];//选定 表
                worksheet.Cells["A1"].LoadFromDataTable(dt1, true);

                worksheet = package.Workbook.Worksheets["Sheet3"];//选定 指定页
                worksheet.Cells["A1"].LoadFromDataTable(dt2, true);

                package.SaveAs(strNewTemp);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable GetExcel(string strFilePath, string tbName)
        {
            try
            {
                FileInfo file = new FileInfo(strFilePath);
                using (ExcelPackage ep = new ExcelPackage(file))
                {
                    ExcelWorksheet ws = ep.Workbook.Worksheets[tbName];
                    int maxColumnNum = ws.Dimension.End.Column;//最大列
                    int minColumnNum = ws.Dimension.Start.Column;//最小列

                    int maxRowNum = ws.Dimension.End.Row;//最小行
                    int minRowNum = ws.Dimension.Start.Row;//最大行

                    DataTable vTable = new DataTable();
                    DataColumn vC;
                    for (int j = 1; j <= maxColumnNum; j++)
                    {
                        vC = new DataColumn("A_" + j, typeof(string));
                        vTable.Columns.Add(vC);
                    }

                    for (int n = 2; n <= maxRowNum; n++)
                    {
                        DataRow vRow = vTable.NewRow();
                        for (int m = 1; m <= maxColumnNum; m++)
                        {
                            vRow[m - 1] = ws.Cells[n, m].Value;
                        }
                        vTable.Rows.Add(vRow);
                    }
                    return vTable;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable GetExcel(string strFilePath, int tbIndex)
        {
            try
            {
                FileInfo file = new FileInfo(strFilePath);
                using (ExcelPackage ep = new ExcelPackage(file))
                {
                    ExcelWorksheet ws = ep.Workbook.Worksheets[tbIndex];
                    int maxColumnNum = ws.Dimension.End.Column;//最大列
                    int minColumnNum = ws.Dimension.Start.Column;//最小列

                    int maxRowNum = ws.Dimension.End.Row;//最小行
                    int minRowNum = ws.Dimension.Start.Row;//最大行

                    DataTable vTable = new DataTable();
                    DataColumn vC;
                    for (int j = 1; j <= maxColumnNum; j++)
                    {
                        vC = new DataColumn("A_" + j, typeof(string));
                        vTable.Columns.Add(vC);
                    }

                    for (int n = 2; n <= maxRowNum; n++)
                    {
                        DataRow vRow = vTable.NewRow();
                        for (int m = 1; m <= maxColumnNum; m++)
                        {
                            vRow[m - 1] = ws.Cells[n, m].Value;
                        }
                        vTable.Rows.Add(vRow);
                    }
                    return vTable;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

/*
 * 导出Excel之Epplus使用教程2（样式设置） 
1、公式计算

     excel中离不开各种各样的公式计算，在Epplus中运用公式有两种方式，你都可以尝试一下：

worksheet.Cells["D2:D5"].Formula = "B2*C2";//这是乘法的公式，意思是第二列乘以第三列的值赋值给第四列，这种方法比较简单明了
worksheet.Cells[6, 2, 6, 4].Formula = string.Format("SUBTOTAL(9,{0})", new ExcelAddress(2, 2, 5, 2).Address);//这是自动求和的方法，至于subtotal的用法你需要自己去了解了
    至于别的公式大家可以自己尝试一下。

2、设置单元格格式

worksheet.Cells[5, 3].Style.Numberformat.Format = "#,##0.00";//这是保留两位小数
　单元格的格式设置还有很多，我就不一一列出来了，基本上excel上能实现的Epplus都能实现，大家可以去Epplus的源码上看。

3、设置字体和单元格样式

   设置单元格对齐方式   

worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;//水平居中
worksheet.Cells[1, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;//垂直居中
worksheet.Cells[1, 4, 1, 5].Merge = true;//合并单元格
worksheet.Cells.Style.WrapText = true;//自动换行
　设置单元格字体样式

worksheet.Cells[1, 1].Style.Font.Bold = true;//字体为粗体
worksheet.Cells[1, 1].Style.Font.Color.SetColor(Color.White);//字体颜色
worksheet.Cells[1, 1].Style.Font.Name = "微软雅黑";//字体
worksheet.Cells[1, 1].Style.Font.Size = 12;//字体大小
　设置单元格背景样式

worksheet.Cells[1, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(128, 128, 128));//设置单元格背景色
　设置单元格边框，两种方法

worksheet.Cells[1, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.FromArgb(191, 191, 191));//设置单元格所有边框
worksheet.Cells[1, 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//单独设置单元格底部边框样式和颜色（上下左右均可分开设置）
worksheet.Cells[1, 1].Style.Border.Bottom.Color.SetColor(Color.FromArgb(191, 191, 191));　
   设置单元格的行高和列宽

worksheet.Cells.Style.ShrinkToFit = true;//单元格自动适应大小
worksheet.Row(1).Height = 15;//设置行高
worksheet.Row(1).CustomHeight = true;//自动调整行高
worksheet.Column(1).Width = 15;//设置列宽
4、设置sheet背景

worksheet.View.ShowGridLines = false;//去掉sheet的网格线
worksheet.Cells.Style.Fill.PatternType = ExcelFillStyle.Solid;
worksheet.Cells.Style.Fill.BackgroundColor.SetColor(Color.LightGray);//设置背景色
worksheet.BackgroundImage.Image = Image.FromFile(@"firstbg.jpg");//设置背景图片
5、插入图片和形状

   插入图片

ExcelPicture picture = worksheet.Drawings.AddPicture("logo", Image.FromFile(@"firstbg.jpg"));//插入图片
picture.SetPosition(100, 100);//设置图片的位置
picture.SetSize(100, 100);//设置图片的大小
　插入形状

ExcelShape shape = worksheet.Drawings.AddShape("shape", eShapeStyle.Rect);//插入形状
shape.Font.Color = Color.Red;//设置形状的字体颜色
shape.Font.Size = 15;//字体大小
shape.Font.Bold = true;//字体粗细
shape.Fill.Style = eFillStyle.NoFill;//设置形状的填充样式
shape.Border.Fill.Style = eFillStyle.NoFill;//边框样式
shape.SetPosition(200, 300);//形状的位置
shape.SetSize(80, 30);//形状的大小
shape.Text = "test";//形状的内容
　Epplus里面内置了很多形状，大家可以自己试一试。

6、超链接

    给图片加超链接

ExcelPicture picture = worksheet.Drawings.AddPicture("logo", Image.FromFile(@"firstbg.jpg"), new ExcelHyperLink("http:\\www.baidu.com", UriKind.Relative));
　 给单元格加超链接

worksheet.Cells[1, 1].Hyperlink = new ExcelHyperLink("http:\\www.baidu.com", UriKind.Relative);
7、隐藏sheet

worksheet.Hidden = eWorkSheetHidden.Hidden;//隐藏sheet
worksheet.Column(1).Hidden = true;//隐藏某一列
worksheet.Row(1).Hidden = true;//隐藏某一行
　
*/
