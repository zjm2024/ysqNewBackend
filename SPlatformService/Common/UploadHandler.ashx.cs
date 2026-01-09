using CoreFramework.VO;
using SPLibrary.CoreFramework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace SPlatformService.Common
{
    /// <summary>
    /// Summary description for UploadHandler
    /// </summary>
    public class UploadHandler : IHttpHandler
    {
        private HttpContext context;
        public void ProcessRequest(HttpContext context)
        {
           HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
            string subDir = context.Request.QueryString["SubDirectory"];

            string folder = "/UploadFolder/";
            if (!string.IsNullOrEmpty(subDir))
            {
                folder += subDir + "/";
            }

            string filePath = "";
            if (hfc.Count > 0)
            {
                FileInfo fi = new FileInfo(hfc[0].FileName);

                string ext = fi.Extension.ToLower();
                if (ext.IndexOf("asp") > -1 || ext.IndexOf("aspx") > -1 || ext.IndexOf("php") > -1)
                {
                    context.Response.End();
                }

                string newFileName = DateTime.Now.ToString("yyyyMMddhhssmm") + fi.Extension;
                
                //imgPath = folder + hfc[0].FileName;
                filePath = folder + newFileName;
                //可以修改为网络路径
                string localPath = ConfigInfo.Instance.UploadFolder + folder;
                if (!Directory.Exists(localPath))
                {
                    Directory.CreateDirectory(localPath);
                }
                string PhysicalPath = localPath + newFileName;
                hfc[0].SaveAs(PhysicalPath);
            }
            FileInfoVO fileVO = new FileInfoVO();
            fileVO.FileName = hfc[0].FileName;
            fileVO.FilePath = "~" + filePath;
            ResultObject result = new ResultObject() { Flag = 1, Message = "上传成功", Result = fileVO };
                        
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(result));
            context.Response.End();
            //}

            //String aspxUrl = context.Request.Path.Substring(0, context.Request.Path.LastIndexOf("/") + 1);

            //String dirName = context.Request.QueryString["dir"];
            //if (String.IsNullOrEmpty(dirName))
            //{
            //    dirName = "image";
            //}

            ////文件保存目录路径
            //String savePath = "attached/";
            ////文件保存目录URL
            //String saveUrl = aspxUrl + "attached/";
            ////定义允许上传的文件扩展名
            //Hashtable extTable = new Hashtable();
            ////最大文件大小
            //int maxSize = -1;
            //this.context = context;
            //HttpPostedFile imgFile = null;
            ////是否使用日期替换文件名，true则日期为文件名，false则日期为文件夹，文件保存在这个文件夹下（防止同名）
            ////审核文件下载时，不希望文件名被修改了。
            //bool isNewFileName = true;
            //if (dirName.Equals("image"))
            //{
            //    imgFile = context.Request.Files["imgFile"];
            //    extTable.Add("image", "gif,jpg,jpeg,png,bmp");
            //    maxSize = 1024 * 1024 * 20;
            //}

            //if (imgFile.ContentLength == 0)
            //{
            //    string msg = "请选择文件。";
            //    //if (context.Request.Files.Count > 0)
            //    //{
            //    //    msg += "文件个数为" + context.Request.Files.Count.ToString();
            //    //    string[] array = context.Request.Files.AllKeys;
            //    //    foreach (string name in array)
            //    //    {
            //    //        msg += name + ";";
            //    //    }
            //    //}
            //    showError(msg);
            //}

            ////WebConfigInfo.Instance.UploadFolder
            //String dirPath = string.Empty;
            //string uploadFolder = GetAppSettings("UploadFolder", "~/UploadFolder/");
            //bool IsNetWork = true;
            //if (uploadFolder.StartsWith(System.Web.HttpRuntime.AppDomainAppPath))
            //{
            //    //IIS路径
            //    dirPath = context.Server.MapPath(ConfigInfo.Instance.UploadFolder);//savePath);
            //    IsNetWork = false;
            //}
            //else
            //{
            //    //网络路径
            //    dirPath = uploadFolder;
            //}
            //if (!Directory.Exists(dirPath))
            //{
            //    Directory.CreateDirectory(dirPath);
            //    //showError("上传目录不存在。");
            //}
            //if (!extTable.ContainsKey(dirName))
            //{
            //    showError("目录名不正确。");
            //}
            //String fileName = imgFile.FileName;
            //String fileExt = Path.GetExtension(fileName).ToLower();
            //if (imgFile.InputStream == null || (imgFile.InputStream.Length > maxSize && maxSize > -1))
            //{
            //    showError("上传文件大小超过限制。文件最大大小为" + (maxSize / 1024 / 10).ToString() + "MB");
            //}
            //if (!dirName.Equals("file") && (String.IsNullOrEmpty(fileExt) || Array.IndexOf(((String)extTable[dirName]).Split(','), fileExt.Substring(1).ToLower()) == -1))
            //{
            //    showError("上传文件扩展名是不允许的扩展名。\n只允许" + ((String)extTable[dirName]) + "格式。");
            //}
            ////创建文件夹
            //dirPath += dirName + "/";
            //saveUrl += dirName + "/";
            //if (!Directory.Exists(dirPath))
            //{
            //    Directory.CreateDirectory(dirPath);
            //}
            //String ymd = DateTime.Now.ToString("yyyyMMdd", DateTimeFormatInfo.InvariantInfo);
            //String ymdt = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo);
            //dirPath += ymd + "/";
            //saveUrl += ymd + "/";
            //if (!isNewFileName)
            //{
            //    dirPath = dirPath + ymdt + "/";
            //    saveUrl = saveUrl + ymdt + "/";
            //}
            //if (!Directory.Exists(dirPath))
            //{
            //    Directory.CreateDirectory(dirPath);
            //}
            //String newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + fileExt;
            //if (!isNewFileName)
            //{
            //    newFileName = fileName;
            //}
            //String filePath = dirPath + newFileName;
            //imgFile.SaveAs(filePath);
            //String fileUrl = ConfigInfo.Instance.UploadFolder + dirName + "/" + ymd + "/" + newFileName;
            //if (!isNewFileName)
            //{
            //    fileUrl = ConfigInfo.Instance.UploadFolder + dirName + "/" + ymd + "/" + ymdt + "/" + newFileName;
            //}
            //Hashtable hash = new Hashtable();
            //hash["error"] = 0;
            //hash["url"] = fileUrl; StringBuilder json = new StringBuilder();
            //json.Append("{\"error\":0,");
            //if (dirName == "image")
            //{
            //    string temp = newFileName;
            //    string tempurl = fileUrl;
            //    newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + fileExt;
            //    String newpath = dirPath + newFileName;
            //    System.Drawing.Image image;
            //    try
            //    {
            //        if (GetPicThumbnail(filePath, newpath, 50))
            //        {
            //            FileStream fs = new FileStream(newpath, FileMode.Open, FileAccess.Read);
            //            image = Image.FromStream(fs);
            //            fs.Close();
            //            fileUrl = ConfigInfo.Instance.UploadFolder + dirName + "/" + ymd + "/" + newFileName;
            //            hash["url"] = fileUrl;
            //            File.Delete(filePath);
            //        }
            //        else
            //        {
            //            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            //            image = Image.FromStream(fs);
            //            fs.Close();
            //            newFileName = temp;
            //            fileUrl = tempurl;
            //        }
            //    }
            //    catch (Exception e)
            //    {
            //        FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            //        image = Image.FromStream(fs);
            //        fs.Close();
            //        newFileName = temp;
            //        fileUrl = tempurl;
            //    }
            //    if (IsNetWork)
            //    {
            //        //fileUrl = fileUrl.Substring(fileUrl.IndexOf(@"\UploadFolder\")).Replace(@"\", @"/");
            //        uploadFolder = uploadFolder.Substring(0, uploadFolder.LastIndexOf(@"\"));
            //        fileUrl = fileUrl.Substring(uploadFolder.LastIndexOf(@"\")).Replace(@"\", @"/");
            //    }
            //    else
            //    {
            //        fileUrl = fileUrl.Replace(@"\", @"/");
            //    }
            //    json.Append("\"width\":'" + image.Width.ToString() + "',");
            //    json.Append("\"height\":'" + image.Height.ToString() + "',");
            //    image.Dispose();
            //}
            //json.Append("\"name\":'" + newFileName + "',");
            //json.Append("\"url\":'" + fileUrl + "'}");

            //context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
            //context.Response.Write(json.ToString());
            //context.Response.End();
        }

        private static string GetAppSettings(string key, string defaultValue)
        {
            // get the application setting.
            string value = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrEmpty(value))
            {
                value = defaultValue;
            }
            if (key == "UploadFolder" && !Equals(HttpContext.Current, null))
            {
                if (!System.IO.Path.IsPathRooted(value))
                {
                    value = System.Web.HttpContext.Current.Server.MapPath(value);
                }
                if (!value.Trim().EndsWith(@"\"))
                {
                    value = string.Format(@"{0}\", value.Trim());
                }
            }
            else if (key == "UploadFolder" && Equals(HttpContext.Current, null))
            {
                value = ConfigurationManager.AppSettings["UploadFolder"];
            }
            return value;
        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }

    public class FileInfoVO
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public double FileSize { get; set; }
    }

}